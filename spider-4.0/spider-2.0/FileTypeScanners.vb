Imports Microsoft.Win32
' Imports mshtml
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Text
Imports System.Net
Imports System.Net.Sockets
' Imports System.Security.Cryptography
Imports System.Reflection
Imports System.Xml
Imports System.Data.OleDb
Imports System.Threading

Imports ICSharpCode.SharpZipLib
' Imports AlternateDataStream
Imports iTextSharp
Imports Microsoft.Office.Interop ' Word documents
Module FileTypeScanners
    Public Const MAX_TEMP_SIZE As Int64 = 1024 * 1024 ' MB
    Public RealPath As String = ""

    Public WordTrouble As Boolean = False

    Public Sub Delegate_Content_Type(ByVal content As String, ByVal pPath As String, Optional ByVal context As String = "")
        Dim hit As String = ""
        Dim frag As String = ""

        Select Case content.ToLower
            Case "application/pdf"
                process_pdf_file(pPath, hit, frag)
                Return
            Case "application/zip"
                process_zip_archive(pPath, hit, frag)
                Return
            Case "application/vnd.ms-excel"
                process_excel(pPath, context)
                Return
            Case "application/x-gzip"
                process_gzip_archive(pPath, hit, frag)
                Return
            Case "application/vnd.openxmlformats"
                process_zip_archive(pPath, hit, frag)
                Return
            Case "application/x-bz2"
                process_bzip_archive(pPath, hit, frag)
                Return
            Case "application/x-bzip2"
                process_bzip_archive(pPath, hit, frag)
                Return
            Case "application/msaccess"
                process_access(pPath, context)
                Return
            Case "text/html"
                process_plain_text(pPath)
                Return
            Case "text/plain"
                process_plain_text(pPath)
                Return
            Case Else
                If FireflyScanDiskOptions And DiskModeOptions.ScanUnknownFileTypes Then
                    If Not SkipContentTypes.Contains(content.ToLower) Then process_plain_text(pPath)
                Else
                    UnableToScan(pPath, UnableToScanReason.UnrecognizedFileType)
                End If
                ' Whether we scanned it or not, it was an unrecognized file type.
                ' 'If Not UnrecognizedFileTypes.Contains(content.ToLower) Then UnrecognizedFileTypes.Add(content.ToLower)
                Return
        End Select

        Return
    End Sub

    Public Function ShouldScanExt(ByVal fileExt As String, ByRef reason As UnableToScanReason) As Boolean
        If fileExt = "" Then
            If FireflyScanDiskOptions And DiskModeOptions.ScanUnknownFileTypes Then
                Return True
            Else
                reason = UnableToScanReason.UnrecognizedFileType
                Return False
            End If
        End If

        fileExt = fileExt.ToLower()

        If (ScanExts.Count > 0) And Not ScanExts.Contains(fileExt) Then
            reason = UnableToScanReason.WhiteListed
            Return False ' We have a list of extensions we scan, and this isn't one of them.
        End If

        If (SkipExts.Count > 0) And SkipExts.Contains(fileExt) Then
            reason = UnableToScanReason.WhiteListed
            Return False ' This file extension is marked to skip.
        End If

        Return True 'If it was not excluded by the rules above, we scan it.

        'If FireflyScanDiskOptions And DiskModeOptions.ScanEverything Then
        '    ' Skip every extension that wasn't carefully specified '/skip' on the command line.
        '    If (SkipExts.Count > 0) And SkipExts.Contains(fileExt) Then
        '        reason = UnableToScanReason.WhiteListed
        '        Return False
        '    Else
        '        Return True
        '    End If
        'End If

        'If FireflyScanDiskOptions And DiskModeOptions.ScanUnknownFileTypes Then
        '    ' Scan every extension that isn't black listed.
        '    If (SkipExts.Count > 0) And SkipExts.Contains(fileExt) Then
        '        reason = UnableToScanReason.WhiteListed
        '        Return False
        '    Else
        '        Return True
        '    End If
        'Else ' This is the default scanning mode.




        '' Only scan extensions on our white list.
        'If (ScanExts.Count > 0) And ScanExts.Contains(fileExt) Then
        '    ' This is a file type we know how to scan.
        '    Return True
        'Else
        '    If (SkipExts.Count > 0) And SkipExts.Contains(fileExt) Then
        '        reason = UnableToScanReason.WhiteListed
        '    Else
        '        reason = UnableToScanReason.UnrecognizedFileType
        '    End If
        '    Return False
        'End If
        'End If
    End Function


    Public Function Delegate_File_Ext(ByVal pPath As String, Optional ByVal withinArchive As Boolean = False, Optional ByVal context As String = "") As MatchesInFile
        Dim ext As String
        Dim hit As String
        Dim frag As String

        Dim matches As New MatchesInFile(pPath)
        Try
            hit = ""
            frag = ""

            ext = System.IO.Path.GetExtension(pPath).TrimStart(".")

            ' Remove miscellaneous changes that some programs make.
            ext = ext.Replace("~", "")

            ' This is checked when we built the file list.
            'If SkipExts.Contains(ext.ToUpper) Then
            '    Skippedfiles_KnownType += 1
            '    Return matches
            'End If

            'If Not (System.IO.File.Exists(pPath)) Then
            '    ' not there, nothing to do.
            '    Return matches
            'End If

            If withinArchive Then
                Dim reason As UnableToScanReason
                If Not ShouldScanExt(ext, reason) Then
                    If reason <> UnableToScanReason.WhiteListed Then
                        UnableToScan(RealPath + " extracted to " + pPath, reason)
                    End If
                    ' Don't scan this extension.
                    Return matches
                End If
            Else
                Loc = ""
                RealPath = ""
            End If

            Select Case ext.ToLower
                Case "accdb"
                    matches = process_access(pPath, context)
                Case "accdr"
                    matches = process_access(pPath, context)
                    'Case "adp"
                    '   matches = process_access(pPath, hit, frag)
                Case "mdb"
                    matches = process_access(pPath, context)
                Case "xlsx"
                    matches = process_zip_archive(pPath, hit, frag)
                Case "xls"
                    matches = process_excel(pPath, context)
                Case "xlt"
                    process_excel(pPath, context)
                    'Case "xslt"
                    '    process_excel(pPath, hit, frag)
                Case "bz2"
                    If magic(pPath) = "BZIP" Then
                        matches = process_bzip_archive(pPath, hit, frag)
                    Else
                        Throw New Exception("Internal format of archive file unrecognized.")
                    End If
                Case "pdf"
                    If magic(pPath) = "PDF" Then
                        matches = process_pdf_file(pPath, hit, frag)
                    Else
                        Throw New Exception("Internal format of PDF file unrecognized.")
                    End If
                Case "mbx"
                    matches = process_mbox(pPath, hit, frag)

                    ' OpenOffice.org documents. Zip archives containing Xml files.
                Case "odf"
                    matches = process_plain_text(pPath)
                Case "odp"
                    matches = process_zip_archive(pPath, hit, frag)
                Case "ods"
                    matches = process_zip_archive(pPath, hit, frag)
                Case "odt"
                    matches = process_zip_archive(pPath, hit, frag)

                    ' Various archive formats.
                Case "gz"
                    If magic(pPath) = "GZIP" Then
                        matches = process_gzip_archive(pPath, hit, frag)
                    Else
                        Throw New Exception("Internal format of archive file unrecognized.")
                    End If
                Case "tgz"
                    If magic(pPath) = "GZIP" Then
                        matches = process_gzip_archive(pPath, hit, frag)
                    Else
                        Throw New Exception("Internal format of archive file unrecognized.")
                    End If
                Case "tar"
                    ' no magic available for this; we'll trust it.
                    Dim FS As New FileStream(pPath, FileMode.Open, FileAccess.Read)
                    matches = process_tar_archive(FS, pPath, hit, frag)
                    FS.Close()
                Case "doc"
                    matches = process_Word(pPath)
                Case "docx"
                    matches = process_zip_archive(pPath, hit, frag)
                Case "rtf"
                    matches = process_Word(pPath)
                Case "csv"
                    matches = process_plain_text(pPath)
                Case "dat"
                    matches = process_plain_text(pPath)
                Case "txt"
                    matches = process_plain_text(pPath)
                Case "html"
                    matches = process_plain_text(pPath)
                Case "htm"
                    matches = process_plain_text(pPath)
                Case "log"
                    matches = process_plain_text(pPath)
                Case "qif"
                    matches = process_plain_text(pPath)
                Case "sql"
                    matches = process_plain_text(pPath)
                Case "xml"
                    Dim RemoveXmlTags As Boolean = False
                    ' RemoveXmlTags = RealPath.Contains(".docx")
                    RemoveXmlTags = True
                    matches = process_plain_text(pPath, RemoveXmlTags)
                Case "zip"
                    If magic(pPath) = "ZIP" Then
                        matches = process_zip_archive(pPath, hit, frag)
                    Else
                        Throw New Exception("Internal format of archive file unrecognized.")
                    End If
                Case Else
                    If FireflyScanDiskOptions And DiskModeOptions.ScanUnknownFileTypes Then
                        matches = process_plain_text(pPath)
                    Else
                        UnableToScan(pPath, UnableToScanReason.UnrecognizedFileType)
                    End If

                    ' If Not UnrecognizedFileTypes.Contains(ext.ToLower) Then UnrecognizedFileTypes.Add(ext.ToLower)
            End Select
        Catch ex As Exception
            LogError(New ScanFailedException("Unable determine the type of this file.", ex, pPath), True)
        End Try
        Return matches

    End Function

    Private Function RemoveTagsFromXml(ByVal selection As String) As String
        Dim r1 As New Regex("<(w\:p|c).*?>")
        Dim r2 As New Regex("<.*?>")
        Dim match As Match
        Dim returnValue As String = selection
        ' Replace paragraph tags <w:p...> and colum tags <c...> with white space.
        '   This allows our regexes to match after the Xml is stripped out.
        match = r1.Match(returnValue)
        While match.Success
            returnValue = returnValue.Replace(match.Value, " ")
            match = r1.Match(returnValue)
        End While

        ' Remove all tags.
        match = r2.Match(returnValue)
        While match.Success
            returnValue = returnValue.Replace(match.Value, "")
            match = r2.Match(returnValue)
        End While
        Return returnValue
    End Function

    Public Function process_plain_text(ByVal pPath As String, Optional ByVal RemoveXmlTags As Boolean = False) As MatchesInFile

        Dim pathToReport As String = pPath
        If RealPath <> "" Then pathToReport = RealPath
        Dim matches As MatchesInFile = New MatchesInFile(pathToReport)

        Dim cRead(READ_SIZE) As Char
        Dim nRead As Integer

        Dim oRead As System.IO.StreamReader

        Dim depth As Long = 0

        Dim fSize As Long = 0

        MessageNum = 0
        subject = ""

        Try
            ' oRead = New StreamReader(pPath)

            oRead = File.OpenText(pPath)

            While oRead.Peek <> -1

                nRead = oRead.ReadBlock(cRead, 0, READ_SIZE)

                Dim length As Integer = Array.IndexOf(cRead, Nothing)
                Dim selection As String
                If length <> READ_SIZE Then
                    Dim shortRead(length - 1) As Char
                    Array.Copy(cRead, shortRead, length)
                    selection = System.Convert.ToString(shortRead)
                Else
                    selection = System.Convert.ToString(cRead)
                End If

                If RemoveXmlTags Then selection = RemoveTagsFromXml(selection)

                GetMatches(selection, matches)

                If matches.HasMatches Then
                    ' If we're not logging total matches, then report now.
                    If FireflyRunOptions And RunOptions.FastMatch Then
                        If matches.MatchCount >= My.Settings.ReportAfterXMatches Then
                            ReportMatch(matches)
                            Exit While
                        End If
                    End If
                End If
                depth += nRead
                If (ScanDepth > 0) And (depth >= (ScanDepth * 1024)) Then
                    UnableToScan(pPath, UnableToScanReason.LargeSize)
                    Exit While
                End If
            End While
            oRead.Close()

            If (FireflyRunOptions And RunOptions.FastMatch) <> RunOptions.FastMatch Then
                If matches.HasMatches() Then
                    ReportMatch(matches)
                End If
            End If

        Catch nio As IOException
            UnableToScan(pPath, UnableToScanReason.FileInUse)
        Catch avex As AccessViolationException
            UnableToScan(pPath, UnableToScanReason.AccessDenied)
        Catch uaEx As UnauthorizedAccessException
            UnableToScan(pPath, UnableToScanReason.AccessDenied)
        Catch ex As Exception
            UnableToScan(pPath, UnableToScanReason.UnhandledError)
            LogError(New ScanFailedException("Unable to scan plain text file.", ex, pPath), True)
        End Try

        Return matches

    End Function

    Public Function process_access(ByVal pPath As String, Optional ByVal context As String = "") As MatchesInFile
        ' we'll be passed a path to the excel spreadsheet, which we'll then process
        ' as an OLE object

        Dim pathToReport As String = pPath
        If RealPath <> "" Then pathToReport = RealPath
        Dim matches As MatchesInFile = New MatchesInFile(pathToReport)

        Dim match_string As String
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim HitItems As New Hashtable
        match_string = ""
        HitItems.Clear()

        Dim fileExt As String = pPath.Substring(pPath.LastIndexOf(".") + 1)

        Dim data As New DataTable
        Dim Finished As Boolean = False

        Dim dbConnection As OleDbConnection
        If fileExt = "accdb" Or fileExt = "accdr" Then
            dbConnection = New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pPath + ";Persist Security Info=False")
        Else
            dbConnection = New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pPath + "")
        End If
        Try
            dbConnection.Open()
            Try
                Dim dbSchema As DataTable = dbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
                Dim sheetRecord As Data.DataRow
                For Each sheetRecord In dbSchema.Rows
                    Dim table_name As String = sheetRecord("TABLE_NAME").ToString
                    If Not table_name.StartsWith("MSys") Then
                        Dim linkString As String = sheetRecord("TABLE_TYPE").ToString
                        Dim linked As Boolean = (linkString = "LINK")

                        If Not linked Then
                            Dim dbAdapter As New OleDbDataAdapter("SELECT * FROM [" + table_name + "]", dbConnection)
                            Try
                                dbAdapter.Fill(data)
                                Dim row As Data.DataRow
                                For Each row In data.Rows
                                    Dim field As Object
                                    For Each field In row.ItemArray
                                        ' If field.ToString <> "" Then If pPath.Contains("Book") Then MsgBox(field.ToString, Title:=pPath)
                                        Dim thisContext As String = ""
                                        thisContext = context + " Table: " + table_name
                                        GetMatches(field.ToString, matches, thisContext)
                                        If matches.MatchCount >= My.Settings.ReportAfterXMatches Then
                                            If FireflyRunOptions And RunOptions.FastMatch Then
                                                ReportMatch(matches)
                                                Finished = True
                                                Exit For
                                            End If
                                        End If
                                    Next
                                    If Finished Then Exit For
                                Next
                            Catch ex As Exception
                                Throw ex
                            Finally
                                dbAdapter.Dispose()
                            End Try
                            If Finished Then Exit For
                        End If

                    End If
                Next
            Catch ex As Exception
                Throw ex
            Finally
                dbConnection.Close()
            End Try
        Catch nio As IOException
            UnableToScan(pPath, UnableToScanReason.FileInUse)
        Catch avex As AccessViolationException
            UnableToScan(pPath, UnableToScanReason.AccessDenied)
        Catch uaEx As UnauthorizedAccessException
            UnableToScan(pPath, UnableToScanReason.AccessDenied)
        Catch OleEx As OleDbException
            If OleEx.Message.Contains("no read permission") Then
                UnableToScan(pPath, UnableToScanReason.AccessDenied)
                LogError(New ScanFailedException("Error while scanning Access database.", OleEx, pPath), True)
            Else
                LogError(New ScanFailedException("Error while scanning Access database.", OleEx, pPath), True)
            End If
        Catch ex As Exception
            LogError(New ScanFailedException("Error while scanning Access database.", ex, pPath), True)
        End Try

        Return matches

    End Function

    Public Function process_bzip_archive(ByVal pPath As String, ByRef hit As String, ByRef frag As String) As MatchesInFile

        Dim pathToReport As String = pPath
        If RealPath <> "" Then pathToReport = RealPath
        Dim matches As MatchesInFile = New MatchesInFile(pathToReport)


        Dim nBytes As Integer = 32768
        Dim inBytes(32768) As Byte
        'Dim hit As String
        Dim depth As Int64 = 0
        Dim isTar As Boolean = False
        Dim HitItems As New Hashtable
        'Dim frag As String

        HitItems.Clear()

        ' OK, let's go for it.
        Dim zStream As New BZip2.BZip2InputStream(IO.File.OpenRead(pPath))

        ' special delegation for tar files
        If pPath.ToLower.EndsWith("tar.bz2") Then
            process_tar_archive(zStream, pPath, hit, frag)
            zStream.Close()
            Return matches
        End If

        Try
            While (nBytes = zStream.Read(inBytes, 0, 32768))
                If CheckForStopSignal() Then
                    Return matches
                End If

                ' do the matches
                GetMatches(New System.Text.ASCIIEncoding().GetString(inBytes, 0, nBytes), matches)
                If matches.MatchCount >= My.Settings.ReportAfterXMatches Then
                    ReportMatch(matches)
                    Exit While
                End If
                depth += inBytes.Length
                If (ScanDepth > 0) And (depth >= (ScanDepth * 1024)) Then
                    UnableToScan(pPath, UnableToScanReason.LargeSize)
                    Exit While
                End If
            End While
            zStream.Close()

            If (FireflyRunOptions And RunOptions.FastMatch) <> RunOptions.FastMatch Then
                If matches.HasMatches() Then
                    ReportMatch(matches)
                End If
            End If

        Catch nio As IOException
            UnableToScan(pPath, UnableToScanReason.FileInUse)
        Catch avex As AccessViolationException
            UnableToScan(pPath, UnableToScanReason.AccessDenied)
        Catch uaEx As UnauthorizedAccessException
            UnableToScan(pPath, UnableToScanReason.AccessDenied)
        Catch ex As Exception
            LogError(New ScanFailedException("An error occurred while scanning BZIP archive.", ex, pPath), True)
        End Try

        Return matches

    End Function
    Public Function process_zip_archive(ByVal pPath As String, ByRef hit As String, ByRef frag As String) As MatchesInFile
        Dim nBytes As Integer = 32768
        Dim inBytes(32768) As Byte
        Dim depth As Int64 = 0
        Dim HitItems As New Hashtable
        Dim OOXML As Boolean = False
        Dim skip As Boolean = False

        HitItems.Clear()

        Dim pathToReport As String = pPath
        If RealPath <> "" Then pathToReport = RealPath
        Dim matches As MatchesInFile = New MatchesInFile(pathToReport)

        ' need this to determine if we have enough free space at the temp location
        Dim driveletter As String = System.IO.Path.GetTempPath.Substring(0, 2)

        Dim zStream As New Zip.ZipInputStream(IO.File.OpenRead(pPath))

        If Path.GetExtension(pPath).ToLower = ".docx" Or Path.GetExtension(pPath).ToLower = ".xlsx" Then
            OOXML = True
        End If

        Try
            '            While True
            Dim zEntry As Zip.ZipEntry = zStream.GetNextEntry
            While Not zEntry Is Nothing
                If CheckForStopSignal() Then
                    Return matches
                    'Else
                    '    UpdateInterfaceMessage(pPath.ToString, StatusMedia.Progress, Now())
                End If
                If zEntry.IsFile And Not zEntry.IsCrypted Then
                    If OOXML Then
                        If Not OOXML_relevant(zEntry.Name) Then skip = True
                    End If
                    If Not skip Then
                        Dim TP As String = System.IO.Path.GetTempPath
                        Dim FN As String = System.IO.Path.GetFileName(zEntry.Name)
                        If Not TP.EndsWith(System.IO.Path.DirectorySeparatorChar) Then
                            TP += "\"
                        End If
                        Dim NF As String = TP + FN

                        ' make a unique filename by prepending a random number
                        Dim RC As New Random
                        While System.IO.File.Exists(NF)
                            Dim rint As Integer = RC.Next(0, 2048)
                            NF = TP + rint.ToString + FN
                        End While
                        If Loc = String.Empty And Not (ArchiveDepth > 0) Then
                            Loc = pPath
                        End If

                        ' If (zEntry.Size <= (MAX_TEMP_SIZE * MaxArchiveSize)) And (get_disk_freespace(driveletter) > Convert.ToInt64(MIN_FREE_SPACE * MinFreeGB)) Then
                        If zEntry.Size <= get_disk_freespace(driveletter) Then
                            ' dump the entry to a temp file
                            ' send the entry to Delegate_file
                            ' nuke the file, initially with a delete; we could overwrite
                            Try
                                Dim FS As New System.IO.FileStream(NF, FileMode.Create, FileAccess.Write)
                                While True
                                    nBytes = zStream.Read(inBytes, 0, 32768)
                                    If nBytes > 0 Then
                                        'Debug.WriteLine("read: " + nBytes.ToString)
                                        'Debug.WriteLine("writing: " + Convert.ToString(inBytes))
                                        FS.Write(inBytes, 0, nBytes)
                                    Else
                                        Exit While
                                    End If
                                End While
                                FS.Close()
                                ' real file, so we simply delegate it somewhere.
                                ArchiveDepth += 1
                                Try
                                    If RealPath = "" Then RealPath = pPath
                                    matches.Append(Delegate_File_Ext(NF, True))
                                Catch ex As Exception
                                    Throw ex
                                Finally
                                    ArchiveDepth -= 1
                                End Try
                            Catch ex As Exception
                                LogError(New ScanFailedException("Error reading Zip file.", ex, pPath), True)
                            Finally
                                ' wipe(NF, Module2.WipeTarget.File, Module2.WipeMethod.Delete, Module2.WipeWhenDone.Delete)
                                Try
                                    System.IO.File.Delete(NF)
                                Catch ex As Exception
                                    LogError(New Exception("Unable to clean up temporary file: " + NF + "which was unpacked from the Zip archive: " + pPath, ex), True)
                                End Try
                            End Try
                        Else
                            If FireflyScanDiskOptions And DiskModeOptions.ScanUnknownFileTypes Then
                                ' scan the entry the old-fashioned way.
                                If Loc = String.Empty And Not (ArchiveDepth > 0) Then
                                    Loc = zEntry.Name
                                End If
                                While True
                                    nBytes = zStream.Read(inBytes, 0, 32768)
                                    If nBytes < 0 Then
                                        Exit While
                                    End If
                                    ' do the matches
                                    GetMatches(New System.Text.ASCIIEncoding().GetString(inBytes, 0, nBytes), matches)
                                    depth += nBytes
                                    If (ScanDepth > 0) And (depth >= (ScanDepth * 1024)) Then
                                        UnableToScan(pPath, UnableToScanReason.LargeSize)
                                        Exit While
                                    End If
                                End While
                            Else
                                LogError(New ScanFailedException("Unable to unpack ZIP archive.", New Exception("Firefly can only scan this archive if plain text contents are assumed. Set the /su flag if that is the case."), pPath), False)
                            End If ' ScanUnknownFileTypes
                        End If ' Temp space available or not
                    End If ' Not a skippable Zip file meta-data entry.
                End If ' IsFile and Not Encrypted

                If FireflyRunOptions And RunOptions.FastMatch Then
                    If matches.MatchCount >= My.Settings.ReportAfterXMatches Then
                        ReportMatch(matches)
                        Exit While
                    End If
                End If

                skip = False
                Try
                    zEntry = zStream.GetNextEntry
                Catch ex As Exception
                    LogError(New ScanFailedException("Unable to read entry in Zip archive.", ex, pPath), True)
                    zEntry = Nothing
                End Try
            End While
            zStream.Close()

            If (FireflyRunOptions And RunOptions.FastMatch) <> RunOptions.FastMatch Then
                If matches.HasMatches() Then
                    ReportMatch(matches)
                End If
            End If

        Catch nio As IOException
            UnableToScan(pPath, UnableToScanReason.FileInUse)
        Catch avex As AccessViolationException
            UnableToScan(pPath, UnableToScanReason.AccessDenied)
        Catch uaEx As UnauthorizedAccessException
            UnableToScan(pPath, UnableToScanReason.AccessDenied)
        Catch ex As Exception
            LogError(New ScanFailedException("Error reading Zip archive.", ex, pPath), True)
        End Try

        Return matches

    End Function
    Public Function process_Word_alternate(ByVal pPath As String, Optional ByVal realPath As String = "") As MatchesInFile
        Dim pathToReport As String = pPath
        If realPath <> "" Then pathToReport = realPath
        Dim matches As New MatchesInFile(pathtoreport)

        If Not System.IO.File.Exists(pPath) Then
            Return matches
        End If

        Dim fsr As FileStream
        fsr = IO.File.OpenRead(pPath)



        Return matches
    End Function


    Public Function process_Word(ByVal pPath As String, Optional ByVal realPath As String = "") As MatchesInFile

        Dim pathToReport As String = pPath
        If realPath <> "" Then pathToReport = realPath
        Dim matches As MatchesInFile = New MatchesInFile(pathToReport)

        If WordTrouble Then
            matches = process_plain_text(pPath)
            Return matches
        End If

        Try

            Dim wordFix As New WordFix()
            wordFix.SetPermissive()

            Dim app As Word.Application
            Dim doc As Word.Document
            Dim section As Word.Section
            Dim content As String

            app = New Word.Application()
            app.Visible = False
            app.DisplayAlerts = Word.WdAlertLevel.wdAlertsNone

            Dim isVisible As Object = False
            Dim isReadOnly As Object = True
            ' Here is the way to handle parameters you don't care about in .NET
            Dim missing As Object = System.Reflection.Missing.Value
            Dim pathObj As Object = pPath

            Try
                doc = app.Documents.OpenNoRepairDialog(pathObj, False, True, False, "badpassword", "badpassword", False, missing, missing, missing, missing, isVisible, False, missing, True)
            Catch ex As Exception
                app.Quit(False, missing, missing)
                wordFix.RestoreSettings()
                Throw ex
            End Try
            Try
                For Each section In doc.Sections
                    content = section.Range.Text
                    GetMatches(content, matches)
                    If FireflyRunOptions And RunOptions.FastMatch Then
                        If matches.MatchCount >= My.Settings.ReportAfterXMatches Then
                            ReportMatch(matches)
                            Exit For
                        End If
                    End If
                Next
            Catch ex As Exception
                Throw ex
            Finally
                doc.Close(False, missing, missing)
                app.Quit(False, missing, missing)
                wordFix.RestoreSettings()
            End Try

            If (FireflyRunOptions And RunOptions.FastMatch) <> RunOptions.FastMatch Then
                If matches.HasMatches() Then
                    ReportMatch(matches)
                End If
            End If

        Catch nio As IOException
            UnableToScan(pPath, UnableToScanReason.FileInUse)
        Catch avex As AccessViolationException
            WordTrouble = True
            matches = process_plain_text(pPath)
            LogError(New ScanFailedException("The Microsoft Office interop library failed to scan this file. This file hasbeen scanned as plain text instead. Future Word documents will also be scanned as plain text.", avex, pPath), False)
            ' LogError(New ScanFailedException("Unable to open this Word document. It may contain scripts or active content.", avex, pPath))
        Catch uaEx As UnauthorizedAccessException
            UnableToScan(pPath, UnableToScanReason.AccessDenied)
        Catch comEx As System.Runtime.InteropServices.COMException
            If comEx.Message.Contains("The password is incorrect.") Then
                UnableToScan(pPath, UnableToScanReason.AccessDenied)
                ' LogError(New ScanFailedException("Unable to open this Word document. It is password protected.", comEx, pPath), False)
            Else
                WordTrouble = True
                matches = process_plain_text(pPath)
                LogError(New ScanFailedException("Unable to scan this Word document the normal way. Instead, it has been scanned as plain text. Future Word documents will also be scanned as plain text. This may cause additional false positives in Word documents, but will ensure the documents are scanned. The error that occurred is listed below.", comEx, pPath), True)
                ' LogError(New ScanFailedException("Because Microsoft Word 2003 or later is not installed, Firefly cannot scan this Word document.", comEx, pPath))
            End If
        Catch ex As Exception
            LogError(New ScanFailedException("Unable to scan this Word document.", ex, pPath), True)
        End Try

        Return matches

    End Function
    Private Function ColumnNumberToLetter(ByVal col As Integer) As Char
        Return Convert.ToChar(col + 64)
    End Function
    Public Function process_excel(ByVal pPath As String, Optional ByRef context As String = "") As MatchesInFile
        ' we'll be passed a path to the excel spreadsheet, which we'll then process
        ' as an OLE object

        Dim pathToReport As String = pPath
        If RealPath <> "" Then pathToReport = RealPath
        Dim matches As MatchesInFile = New MatchesInFile(pathToReport)

        Dim match_string As String
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim HitItems As New Hashtable
        match_string = ""
        HitItems.Clear()

        Dim data As New DataTable
        Dim Finished As Boolean = False
        Dim dbConnection As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pPath + ";Extended Properties=""Excel 8.0;HDR=Yes;""")
        ' Dim dbConnection As New OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pPath + "")

        Try
            dbConnection.Open()
            Try
                Dim dbSchema As DataTable = dbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
                Dim sheetRecord As Data.DataRow
                For Each sheetRecord In dbSchema.Rows
                    Dim table_name As String = sheetRecord("TABLE_NAME").ToString
                    ' Sheet names containing 'Print_Area' are not scannable and don't contain user data.
                    If Not table_name.Contains("Print_Area") Then
                        'MsgBox(table_name, Title:=pPath)
                        Dim dbAdapter As New OleDbDataAdapter("SELECT * FROM [" + table_name + "]", dbConnection)
                        Try
                            Dim rowCount As Integer = 1
                            Dim colCount As Integer = 0
                            dbAdapter.Fill(data)
                            Dim row As Data.DataRow
                            For Each row In data.Rows
                                colCount = 0
                                rowCount += 1
                                Dim field As Object
                                For Each field In row.ItemArray
                                    colCount += 1
                                    ' If field.ToString <> "" Then If pPath.Contains("Book") Then MsgBox(field.ToString, Title:=pPath)
                                    Dim thisContext As String = ""
                                    thisContext = context + " Excel Sheet: " + table_name.Replace("$", "") + " Cell: " + ColumnNumberToLetter(colCount).ToString + rowCount.ToString()
                                    GetMatches(field.ToString, matches, thisContext)
                                    If matches.MatchCount >= My.Settings.ReportAfterXMatches Then
                                        If FireflyRunOptions And RunOptions.FastMatch Then
                                            ReportMatch(matches)
                                            Finished = True
                                            Exit For
                                        End If
                                    End If
                                Next
                                If Finished Then Exit For
                            Next
                        Catch ex As Exception
                            Throw ex
                        Finally
                            dbAdapter.Dispose()
                        End Try
                        If Finished Then Exit For
                    End If
                Next
            Catch ex As Exception
                Throw ex
            Finally
                dbConnection.Close()
            End Try
        Catch nio As IOException
            UnableToScan(pPath, UnableToScanReason.FileInUse)
        Catch avex As AccessViolationException
            UnableToScan(pPath, UnableToScanReason.AccessDenied)
        Catch uaEx As UnauthorizedAccessException
            UnableToScan(pPath, UnableToScanReason.AccessDenied)
        Catch OleEx As OleDbException
            LogError(New ScanFailedException("Error while scanning Excel document.", OleEx, pPath), True)
        Catch ex As Exception
            LogError(New ScanFailedException("Error while scanning Excel document.", ex, pPath), True)
        End Try

        Return matches

    End Function
    Public Function process_gzip_archive(ByVal pPath As String, ByRef hit As String, ByRef frag As String) As MatchesInFile

        Dim pathToReport As String = pPath
        If RealPath <> "" Then pathToReport = RealPath
        Dim matches As MatchesInFile = New MatchesInFile(pathToReport)

        Dim nBytes As Integer = 32768
        Dim inBytes(32768) As Byte
        'Dim hit As String
        Dim depth As Int64 = 0
        Dim isTar As Boolean = False
        'Dim frag As String
        Dim HitItems As New Hashtable

        HitItems.Clear()

        Dim zStream As New GZip.GZipInputStream(IO.File.OpenRead(pPath))

        ' delegation for tar archives

        If pPath.ToLower.EndsWith("tar.gz") Or pPath.ToLower.EndsWith("tgz") Then
            process_tar_archive(zStream, pPath, hit, frag)
            zStream.Close()
            Return matches
        End If

        Try
            While (nBytes = zStream.Read(inBytes, 0, 32768))

                If CheckForStopSignal() Then
                    Return matches
                End If

                ' do the matches
                GetMatches(New System.Text.ASCIIEncoding().GetString(inBytes, 0, nBytes), matches)
                If FireflyRunOptions And RunOptions.FastMatch Then
                    If matches.MatchCount >= My.Settings.ReportAfterXMatches Then
                        ReportMatch(matches)
                        Exit While
                    End If
                End If
                depth += nBytes
                If (ScanDepth > 0) And (depth >= (ScanDepth * 1024)) Then
                    UnableToScan(pPath, UnableToScanReason.LargeSize)
                    Exit While
                End If
            End While
            zStream.Close()

            If (FireflyRunOptions And RunOptions.FastMatch) <> 0 Then
                If matches.HasMatches() Then
                    ReportMatch(matches)
                End If
            End If

        Catch nio As IOException
            UnableToScan(pPath, UnableToScanReason.FileInUse)
        Catch avex As AccessViolationException
            UnableToScan(pPath, UnableToScanReason.AccessDenied)
        Catch uaEx As UnauthorizedAccessException
            UnableToScan(pPath, UnableToScanReason.AccessDenied)
        Catch ex As Exception
            LogError(New ScanFailedException("Error while scanning GZIP archive.", ex, pPath), True)
        End Try

        Return matches

    End Function

    'Public Sub process_http(ByVal StartURL As String, ByVal Currentdepth As Integer, ByVal Currentdomain As String)
    '    ' Dim scanpath As String
    '    ' Dim currdomain As String
    '    Dim frag As String = ""
    '    Dim hit As String = ""
    '    Dim sRead As String
    '    Dim nRead As Integer
    '    Dim currdepth As Integer = 0
    '    Dim i As Integer
    '    Dim Links As New ArrayList

    '    Dim request As HttpWebRequest
    '    Dim response As HttpWebResponse

    '    Dim HitItems As New Hashtable

    '    Dim matches As New Scanner.MatchesInFile(StartURL)


    '    HitItems.Clear()

    '    ' Dim thisURI As Uri
    '    ' we'll Firefly the site, recursing to depth N off the main site
    '    ' we'll expect the following:
    '    ' MaxURLDepth int -- max distance from the main page
    '    ' JumpDomain bool -- whether or not to visit links that lead to other servers

    '    ' we'll start by pulling the first page and scanning it as though it were a file
    '    ' we'll then grab all the links off the page, using JumpDomain as a gauge for relevance
    '    ' we'll then pull and scan those, recursing as we go

    '    Me.Firefly_progress.Text = "Searching " + StartURL
    '    Me.Refresh()

    '    Dim thisURI As New Uri(StartURL)

    '    Me.Firefly_progress.Text = "Scanning: " + StartURL
    '    Me.Refresh()

    '    If (Currentdepth = MaxURLDepth) Then
    '        Debug.WriteLine("Max depth reached")
    '        Return
    '    End If

    '    ' don't scan robots.txt
    '    If StartURL.EndsWith("obots.txt") Then
    '        Debug.WriteLine("hit a robots.txt")
    '        Return
    '    End If

    '    ' check the domain of this URL against the Currentdomain and JumpDomain values
    '    If (FireflyWebModeOptions And Configuration.WebModeOptions.JumpDomain) Then
    '        ' check
    '        ' if DomainDepth is 0, it means an exact match is required
    '        ' otherwise, we're just looking for tail-end
    '        If DomainDepth = 0 Then
    '            If thisURI.Host.ToLower <> Currentdomain.ToLower Then
    '                Return
    '            End If
    '        Else
    '            If Not thisURI.Host.EndsWith(Currentdomain) Then
    '                Return
    '            End If
    '        End If
    '    Else
    '        ' JumpDomain not set, we stay on this host
    '        If Not thisURI.Host.ToLower.EndsWith(Currentdomain.ToLower) Then
    '            Return
    '        End If
    '    End If

    '    ' if RespectRobots is set, and we haven't pulled one for this host yet, try now
    '    ' we'll parse it, and set a skip disposition for this host
    '    ' if RespectRobots is set and we already have a skip dispo for this host, 
    '    ' process it now
    '    If (FireflyWebModeOptions And Configuration.WebModeOptions.RespectRobots) Then
    '        ' Dim robotSkipList As New ArrayList
    '        If RobotsDone.Contains(thisURI.Host.ToLower) Then
    '            ' we've been here before
    '            If check_robots(StartURL) Then
    '                ' nope
    '                Me.Firefly_progress.Text = "Robots.txt skipping: " + StartURL
    '                Me.Refresh()
    '                Return
    '            End If
    '        Else
    '            populate_robots(StartURL)
    '            If check_robots(StartURL) Then
    '                Me.Firefly_progress.Text = "Robots.txt skipping: " + StartURL
    '                Me.Refresh()
    '                Return
    '            End If
    '        End If
    '    End If


    '    ' for our various file-type scanners (ZIP, PDF, etc), we'll pull the contents of the URL
    '    ' down to a temporary file that's removed when Firefly closes, then hand that 
    '    ' path off to process_whatever
    '    ' the trick is going to be getting the logfile to reflect the URL instead of the disk-path
    '    ' to the temporary file.  *sigh*  code bloat.

    '    Try
    '        request = DirectCast(WebRequest.Create(StartURL), HttpWebRequest)
    '        If UserAgent <> String.Empty Then
    '            request.UserAgent = UserAgent
    '        End If
    '        ' if we've got a username and password, ready those, too
    '        request.MaximumAutomaticRedirections = MaxRedirections

    '        ' fire away
    '        response = DirectCast(request.GetResponse(), HttpWebResponse)
    '        ' check our status codes

    '        ' redirects get checked for proper domain versus JumpDomain

    '        If MaxContentLength > 0 Then
    '            If response.ContentLength >= (MaxContentLength * 1024 * 1024) Then
    '                response.Close()
    '                Return
    '            End If
    '        End If

    '        Debug.WriteLine("content-type: " + response.ContentType)

    '        If SkipContentRegex.Count <> 0 Then
    '            Dim en As IDictionaryEnumerator = SkipContentRegex.GetEnumerator
    '            ' Dim skipContentReg As String
    '            While en.MoveNext
    '                Dim re As New Regex(en.Key, RegexOptions.IgnoreCase)
    '                If re.IsMatch(response.ContentType) Then
    '                    Debug.WriteLine("Skipping content-type for regex: " + en.Key)
    '                    response.Close()
    '                    Return
    '                End If
    '            End While
    '        End If

    '        ' barf!  I hate doing it this way, with two while loops, one used, one not, depending
    '        ' on the circumstances.

    '        Dim readHTML As StreamReader = New StreamReader(response.GetResponseStream)
    '        If DelegateContentTypes.Contains(response.ContentType) Then
    '            ' dump to a temp file and scan that file
    '            Dim tempContent As String = System.IO.Path.GetTempFileName
    '            Dim content As String = response.ContentType
    '            Dim writeData As StreamWriter = New StreamWriter(System.IO.File.OpenWrite(tempContent))
    '            Dim cRead(65535) As Char
    '            While readHTML.Peek <> -1
    '                If ThreadTracking.StopRequested Then
    '                    readHTML.Close()
    '                    writeData.Close()
    '                    response.Close()
    '                    System.IO.File.Delete(tempContent)
    '                    'UpdateInterfaceMessage("Scan halted by user request.", StatusMedia.Status, True)
    '                    StopScanning(Thread.CurrentThread)
    '                    Return
    '                End If
    '                nRead = readHTML.Read(cRead, 0, READ_SIZE)
    '                writeData.Write(cRead, 0, nRead)
    '            End While
    '            readHTML.Close()
    '            writeData.Close()
    '            response.Close()
    '            Delegate_Content_Type(content, tempContent, hit, frag)
    '            ' none of these types natively contains links, so we're done
    '            System.IO.File.Delete(tempContent)
    '            Return
    '        End If


    '        While readHTML.Peek <> -1
    '            If ThreadTracking.StopRequested Then
    '                readHTML.Close()
    '                response.Close()
    '                UpdateInterfaceMessage("Scan halted by user request.", StatusMedia.Status, True)
    '                StopScanning(Thread.CurrentThread)
    '                Return
    '            End If
    '            sRead = readHTML.ReadLine
    '            ' if RecurseURL is set, grab the links from this page and add them to an array
    '            ' when we're finished with this page, we'll iterate over those
    '            If (FireflyWebModeOptions And Configuration.WebModeOptions.Recurse) Then
    '                Links = getHrefs(sRead, thisURI.Host, thisURI.ToString, Links)
    '            End If
    '            matches.Append(GetMatches(sRead))
    '            If matches.HasMatches Then
    '                ' decide what to do
    '                Debug.WriteLine("HIT ON PAGE: " + StartURL)
    '                ' hit = gen_hit_string(hit, HitItems)
    '                If Not (My.Settings.FireflyLogAttributes And LogMask.attrTotalMatches) Then
    '                    ' ReportMatch(StartURL, hit, frag)
    '                    ReportMatch(matches)
    '                    Exit While
    '                End If
    '            End If
    '        End While
    '        readHTML.Close()
    '        ' if there are any links on the page, pull those and recurse to this function
    '    Catch ex As Exception
    '        Debug.WriteLine("error pulling HTML from web page: " + ex.ToString)
    '        Return
    '    End Try

    '    If (My.Settings.FireflyLogAttributes And LogMask.attrTotalMatches) And totalMatches > 0 Then
    '        '            ReportMatch(StartURL, hit, frag)
    '        ReportMatch(matches)
    '    End If

    '    If (FireflyWebModeOptions And Configuration.WebModeOptions.Recurse) And Links.Count > 0 Then
    '        ' run through again
    '        For i = 0 To Links.Count - 1
    '            Debug.WriteLine("Descending: " + Links.Item(i).ToString)
    '            process_http(Links.Item(i).ToString, (Currentdepth + 1), Currentdomain)
    '        Next
    '    End If

    '    Return

    'End Sub

    Public Function process_mbox(ByVal pPath As String, ByRef hit As String, ByRef frag As String) As MatchesInFile
        ' pretty easy.  We read it as text, looking for lines that start with From
        ' we'll count that as a message.
        ' otherwise, this is straight text-file processing
        ' Dim oFile As System.IO.File

        Dim pathToReport As String = pPath
        If RealPath <> "" Then pathToReport = RealPath
        Dim matches As MatchesInFile = New MatchesInFile(pathToReport)

        Dim oRead As System.IO.StreamReader
        Dim bRead As String
        Dim depth As Int64
        'Dim hit As String
        'Dim frag As String
        Dim matchnum As Integer
        Dim SOM As Boolean = False
        Dim HitItems As New Hashtable
        HitItems.Clear()

        Try
            oRead = File.OpenText(pPath)

            Try
                While oRead.Peek <> -1
                    '            Debug.WriteLine("HERE2")
                    bRead = oRead.ReadLine
                    If bRead.StartsWith("From ") Then
                        MessageNum += 1
                    End If
                    If bRead.StartsWith("Subject") Then
                        subject = bRead
                    End If
                    If bRead.StartsWith(Chr(10).ToString) Or bRead.StartsWith(Chr(13).ToString) Or bRead = String.Empty Then
                        SOM = True
                    End If
                    If SOM Then
                        GetMatches(bRead, matches)
                        If matches.HasMatches Then
                            matchnum = MessageNum

                            If Loc = String.Empty Then
                                Loc = "Message: " + matchnum.ToString
                            End If

                            ' if we're logging total matches, we'll defer ReportMatch until done
                            'If Not (My.Settings.FireflyLogAttributes And Configuration.LogMask.attrTotalMatches) Then
                            If FireflyRunOptions And RunOptions.FastMatch Then
                                If matches.MatchCount >= My.Settings.ReportAfterXMatches Then
                                    ReportMatch(matches)
                                    Exit While
                                End If
                            End If

                            ' Exit While

                        End If
                        '  depth += bRead.Length
                        If (ScanDepth > 0) Then
                            depth += bRead.Length
                            If (depth >= (ScanDepth * 1024)) Then
                                UnableToScan(pPath, UnableToScanReason.LargeSize)
                                Exit While
                            End If
                        End If
                    End If
                    '         bRead = String.Empty
                End While

            Catch ex As Exception
                LogError(New ScanFailedException("Error while scanning GZIP archive.", ex, pPath), True)
            Finally
                oRead.Close()
            End Try
            MessageNum = matchnum

            If (FireflyRunOptions And RunOptions.FastMatch) <> RunOptions.FastMatch Then
                If matches.HasMatches() Then
                    ReportMatch(matches)
                End If
            End If

        Catch nio As IOException
            UnableToScan(pPath, UnableToScanReason.FileInUse)
        Catch avex As AccessViolationException
            UnableToScan(pPath, UnableToScanReason.AccessDenied)
        Catch uaEx As UnauthorizedAccessException
            UnableToScan(pPath, UnableToScanReason.AccessDenied)
        Catch ex As Exception
            LogError(New ScanFailedException("Error while scanning mail-box.", ex, pPath), True)
        End Try

        Return matches

    End Function
    Public Function process_pdf_file(ByVal pPath As String, ByRef hit As String, ByRef frag As String) As MatchesInFile
        ' we'll try to unpack the text from the PDF file and deal with it.
        ' this is going to be ugly

        Dim pathToReport As String = pPath
        If RealPath <> "" Then pathToReport = RealPath
        Dim matches As MatchesInFile = New MatchesInFile(pathToReport)

        Dim fooBytes() As Byte
        '        Dim toMatch As String
        ' Dim encoding As New System.Text.ASCIIEncoding
        Dim pages As Integer
        Dim currpage As Integer
        Dim depth As Int64 = 0
        Dim inBytes(8) As Byte
        'Dim hit As String
        'Dim frag As String
        Dim HitItems As New Hashtable

        HitItems.Clear()

        ' first test, can we open it?

        Try
            Dim pdf As New iTextSharp.text.pdf.PdfReader(pPath)

            Try

                ' is it encrypted?
                If pdf.IsEncrypted Then
                    UnableToScan(pPath, UnableToScanReason.FileEncrypted)
                    pdf.Close()
                    Return matches
                End If

                ' get the total number of pages
                pages = pdf.NumberOfPages
                ' Debug.WriteLine("total pages: " + pages.ToString)
                For currpage = 1 To pages
                    ' get the content of each page and run the matcher
                    If Loc = String.Empty Then
                        Loc = "Page: " + currpage.ToString
                    End If
                    fooBytes = pdf.GetPageContent(currpage)

                    GetMatches(New System.Text.ASCIIEncoding().GetString(fooBytes, 0, fooBytes.Length), matches)

                    If FireflyRunOptions And RunOptions.FastMatch Then
                        If matches.MatchCount >= My.Settings.ReportAfterXMatches Then
                            ReportMatch(matches)
                            Exit For
                        End If
                    End If

                    depth += fooBytes.Length
                    If (ScanDepth > 0) And (depth >= (ScanDepth * 1024)) Then
                        UnableToScan(pPath, UnableToScanReason.LargeSize)
                        Exit For
                    End If
                Next
                pdf.Close()
            Catch ex As Exception
                LogError(New ScanFailedException("Error while scanning PDF file.", ex, pPath), True)
                Return matches
            Finally
                pdf.Close()
            End Try

            If (FireflyRunOptions And RunOptions.FastMatch) <> RunOptions.FastMatch Then
                If matches.HasMatches() Then
                    ReportMatch(matches)
                End If
            End If

        Catch nio As IOException
            UnableToScan(pPath, UnableToScanReason.FileInUse)
        Catch avex As AccessViolationException
            UnableToScan(pPath, UnableToScanReason.AccessDenied)
        Catch uaEx As UnauthorizedAccessException
            UnableToScan(pPath, UnableToScanReason.AccessDenied)
        Catch ex As Exception
            LogError(New ScanFailedException("Error while scanning PDF file.", ex, pPath), True)
        End Try

        Return matches

    End Function
    Private Function CleanFileName(ByVal name As String) As String
        name = name.Replace("\", " ")
        name = name.Replace("/", " ")
        Return name
    End Function

    Public Function process_tar_archive(ByVal pPath As Stream, ByVal fileName As String, ByRef hit As String, ByRef frag As String) As MatchesInFile
        Dim nBytes As Integer = 32768
        Dim inBytes(32768) As Byte
        'Dim hit As String
        Dim depth As Int64 = 0
        'Dim frag As String
        Dim HitItems As New Hashtable
        Dim tempTAR As String

        Dim matches As New Scanner.MatchesInFile(fileName)

        HitItems.Clear()
        Dim driveletter As String = System.IO.Path.GetTempPath.Substring(0, 2)

        ' we're going to have to unpack the tar archive, delegating the files we find within to 
        ' various other handlers as we find them
        Try
            Dim tstream As New Tar.TarInputStream(pPath)
            Dim tentry As Tar.TarEntry
            tentry = tstream.GetNextEntry
            While Not tentry Is Nothing

                If CheckForStopSignal() Then
                    Return matches
                End If

                If Not tentry.IsDirectory Then
                    tempTAR = System.IO.Path.GetTempPath
                    If Not tempTAR.EndsWith(System.IO.Path.DirectorySeparatorChar) Then
                        tempTAR += "\"
                    End If
                    Dim tempF As String = tempTAR + CleanFileName(tentry.Name)
                    ' If tentry.Size <= (MAX_TEMP_SIZE * MaxArchiveSize) And (get_disk_freespace(driveletter) > Convert.ToInt64(MIN_FREE_SPACE * MinFreeGB)) Then
                    If tentry.Size <= get_disk_freespace(driveletter) Then
                        ' pull to the temp file and delegate
                        If Loc = String.Empty And Not (ArchiveDepth > 0) Then
                            Loc = fileName
                        End If
                        Try
                            Dim RC As New Random
                            While System.IO.File.Exists(tempF)
                                ' make random number
                                Dim rnum As Integer = RC.Next(0, 2048)
                                tempF = tempTAR + rnum.ToString + CleanFileName(tentry.Name)
                            End While
                            Dim FS As New FileStream(tempF, FileMode.Create, FileAccess.Write)
                            tstream.CopyEntryContents(FS)
                            FS.Close()
                            RealPath = pPath.ToString
                            ArchiveDepth += 1
                            Try
                                matches.Append(Delegate_File_Ext(tempF, True))
                            Catch ex As Exception
                                Throw ex
                            Finally
                                ArchiveDepth -= 1
                            End Try
                        Catch ex As Exception
                            LogError(New ScanFailedException("An error occurred while scanning the Tar archive.", ex, pPath.ToString), True)
                        Finally
                            ' wipe(tempF, Module2.WipeTarget.File, Module2.WipeMethod.Delete, Module2.WipeWhenDone.Delete)
                            Try
                                System.IO.File.Delete(tempF)
                            Catch ex As Exception
                                LogError(New Exception("Unable to clean up temporary file: " + tempF + "which was unpacked from the tar archive: " + pPath.ToString, ex), True)
                            End Try
                        End Try
                    Else
                        ' deal with the stream, bytewise
                        While (nBytes = tstream.Read(inBytes, 0, 32768))
                            ' do the matches
                            GetMatches(New System.Text.ASCIIEncoding().GetString(inBytes, 0, nBytes), matches)
                            depth += inBytes.Length
                            If (ScanDepth > 0) And (depth >= (ScanDepth * 1024)) Then
                                UnableToScan(pPath.ToString, UnableToScanReason.LargeSize)
                                Exit While
                            End If
                        End While
                    End If
                End If

                If FireflyRunOptions And RunOptions.FastMatch Then
                    If matches.MatchCount >= My.Settings.ReportAfterXMatches Then
                        Loc = "TAR archive"
                        ReportMatch(matches)
                        Exit While
                    End If
                End If

                ' Debug.WriteLine("entry name: " + tentry.Name.ToString)
                tentry = tstream.GetNextEntry
            End While
            tstream.Close()

            If (FireflyRunOptions And RunOptions.FastMatch) <> RunOptions.FastMatch Then
                If matches.HasMatches() Then
                    ReportMatch(matches)
                End If
            End If

        Catch nio As IOException
            UnableToScan(pPath.ToString, UnableToScanReason.FileInUse)
        Catch avex As AccessViolationException
            UnableToScan(pPath.ToString, UnableToScanReason.AccessDenied)
        Catch uaEx As UnauthorizedAccessException
            UnableToScan(pPath.ToString, UnableToScanReason.AccessDenied)
        Catch ex As Exception
            LogError(New ScanFailedException("An error occurred while scanning TAR archive.", ex, fileName), True)
        End Try

        Return matches

    End Function

    'Public Function scanADS(ByVal pPath As String, ByVal pStream As String) As MatchesInFile
    '    ' we'll open the ADS passed to us and treat it like a normal file
    '    Dim bRead(4096) As Byte
    '    Dim nRead As Integer
    '    Dim depth As Integer = 0
    '    Dim hit As String
    '    hit = ""
    '    Dim encoding As New System.Text.ASCIIEncoding
    '    Dim frag As String
    '    frag = ""


    '    Dim HitItems As New Hashtable

    '    Dim matches As New Scanner.MatchesInFile(pPath)

    '    HitItems.Clear()

    '    ' we can't yet deal with special files like ZIPs, .gz, etc
    '    ' I wonder if it might not be easier to simply pull to a temp file
    '    ' then delegate that temp file, then delete the temp file?

    '    ' nor can we reset atimes
    '    Try
    '        Dim oFile As System.IO.FileStream = ADSFile.OpenRead(pPath, pStream)
    '        While (nRead = oFile.Read(bRead, 0, 4096)) <> -1
    '            ' bRead = oFile.ReadLine
    '            ' Debug.WriteLine("inside: " + encoding.GetString(bRead))
    '            GetMatches(encoding.GetString(bRead), matches)
    '            If matches.HasMatches Then
    '                hit += ":ADS:" + pStream
    '                ReportMatch(matches)
    '                Exit While
    '            End If
    '            depth += nRead
    '            If (ScanDepth > 0) And (depth >= (ScanDepth * 1024)) Then
    '                '          Debug.WriteLine("exiting loop due to scandepth: " + depth.ToString)
    '                Exit While
    '            End If
    '        End While
    '        oFile.Close()
    '    Catch ex As Exception
    '        LogError(New Exception("An error occurred while processing alternate data stream: " + pPath, ex))
    '    End Try

    '    Return matches
    'End Function

    Public Function OOXML_relevant(ByVal name As String) As Boolean

        Dim re As New Regex("sheet\d{1,4}\.xml", RegexOptions.IgnoreCase)
        Dim re2 As New Regex("document\.xml", RegexOptions.IgnoreCase)

        ' things we care about: document.xml

        If name.ToLower.EndsWith("rels") Then
            Return False
        End If

        If re2.IsMatch(name) Then
            Return True
        End If

        ' Excel worksheets
        If re.IsMatch(name) Then
            Return True
        End If

        If name.ToLower.EndsWith("xml") Then
            Return False
        End If

        Return True

    End Function
    Public Function magic(ByVal pPath As String) As String
        ' returns a guess as to the file type;
        ' right now, we can handle gzip, bzip, ZIP, PDF.  We may extend this to others

        ' the basic procedure is binary open the file and pull the first byte
        ' if it's any one of the ones we need, keep looking.
        ' if not, return "UNK"
        '
        ' returns:
        ' "ZIP"
        ' "GZIP"
        ' "BZIP"
        ' "PDF"


        Dim inByte(32) As Byte
        Dim depth As Integer
        Dim ret As String = ""

        Try
            Dim tStream As New IO.FileStream(pPath, FileMode.Open, FileAccess.Read)
            depth = tStream.Read(inByte, 0, 1)
            Select Case inByte(0)
                Case 66 ' bzip, next = 90
                    depth = tStream.Read(inByte, 0, 1)
                    If inByte(0) = 90 Then
                        ret = "BZIP"
                    Else
                        ret = "UNK"
                    End If
                    Exit Select
                Case 80 ' ZIP, next 75, then 3
                    depth = tStream.Read(inByte, 0, 2)
                    If inByte(0) = 75 And inByte(1) = 3 Then
                        ret = "ZIP"
                    Else
                        ret = "UNK"
                    End If
                    Exit Select
                Case 31 ' gzip, next 139
                    depth = tStream.Read(inByte, 0, 1)
                    If inByte(0) = 139 Then
                        ret = "GZIP"
                    Else
                        ret = "UNK"
                    End If
                    Exit Select
                Case 37 ' PDF, 80, 68, 70
                    depth = tStream.Read(inByte, 0, 3)
                    If inByte(0) = 80 And inByte(1) = 68 And inByte(2) = 70 Then
                        ret = "PDF"
                    Else
                        ret = "UNK"
                    End If
                    Exit Select
                Case Else
                    Exit Select
            End Select
            tStream.Close()
            Return ret
        Catch
            Return "UNK"
        End Try

        Return "UNK"

    End Function
End Module
