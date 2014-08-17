'' Devleped by Edward Delaporte at University of Illinois
'' Branched from Cornell Spider 3 Beta
''
'' Please see Cornell University's and U of I's webpages for license details.

Imports Microsoft.Win32
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Text
Imports System.Net
Imports System.Net.Sockets
Imports System.Security.Cryptography
Imports System.Reflection
Imports System.Xml
Imports System.Data.OleDb
Imports System.Threading
Imports ICSharpCode.SharpZipLib
' Imports AlternateDataStream ' ADSFile

Imports iTextSharp
Public Enum UnableToScanReason
    WhiteListed = 0
    LargeSize = 1
    UnrecognizedFileType
    AccessDenied
    FileInUse
    UnhandledError
    DatabaseNullCount
    FileEncrypted
End Enum


Module LogHandler

    Private ReportStyle As String = "<style type='text/css'>" & _
    " body { " & _
    "	margin: 0em;" & _
    "	padding: 0em;" & _
    "    		font-size: 100%;" & _
    "	font-family: Arial, Helvetica, sans-serif;" & _
    "	background-color: white;" & _
    "}" & _
    "a {" & _
    "	text-decoration: none;" & _
    "}" & _
    "a:link {" & _
    "    	color: #3F3F72;" & _
    "	text-decoration: underline;" & _
    "}" & _
    "a:visited {" & _
    "	color: #666;" & _
    "	text-decoration: underline;" & _
    "}" & _
    "a:hover {" & _
    "	color: #D56132;" & _
    "	text-decoration: underline;" & _
    "}" & _
    "#content {" & _
    "	clear: both;" & _
    "	margin: 0 0 0 0px;" & _
    "	padding: 1em 0 4em .5em;" & _
    "	width: 44em;" & _
    "	float: right;" & _
    "	background-color: #FFF;" & _
    "	font-size: .8em;" & _
    "}" & _
    "h1 {" & _
    "	margin: 0 0 0.7em 0;" & _
    "	font-weight: bold;" & _
    "	font-size: 1.8em;" & _
    "	color: #333;" & _
    "	font-variant: small-caps;		" & _
    "}" & _
    "h2 {" & _
    "	margin: 0 0 0.3em 0;" & _
    "   		padding: 1em 0 0.3em;" & _
    "	font-size: 1.4em;" & _
    "	font-weight: bold;" & _
    "	color:#333;" & _
    "	border-bottom: solid #4C56A6 1px;" & _
    "}" & _
    "p {" & _
    "	margin: 0;" & _
    "	padding: 0 1em 1.2em;" & _
    "	font-weight: normal;" & _
    "	line-height: 1.4em;" & _
    "	color:#000000;}</style>"


    Private LogErrorStackTrace As Boolean = False
    Private LogPathIsInaccessible As Boolean = False

    ' Public SecurityReportSummary As String = ""
    Public SecurityReportSent As Boolean = False

    ' Log and Report directories

    Public LogFooter As String
    Public LogDirectory As String
    Public ReportDirectory As String

    Public ReportFileName As String
    Public ReportPath As String

    Public AppLogPath As String
    Public SkippedFilesListPath As String
    Public SecurityReportPath As String
    Public SecurityReportSummaryPath As String

    Public UnprocessedFilesPath As String
    Public CsvReportPath As String


    Public Function gen_header() As String
        Dim cMsg As String = ""

        If (My.Settings.FireflyLogAttributes And LogMask.attrPath) Then
            If (FireflyRunMode And Configuration.RunMode.Disk) Then
                cMsg += "PATH,"
            End If
            If (FireflyRunMode And Configuration.RunMode.Web) Then
                cMsg += "URL,"
            End If
        End If

        If (My.Settings.FireflyLogAttributes And LogMask.attrLocation) Then
            cMsg += "LOC,"
        End If

        If (My.Settings.FireflyLogAttributes And LogMask.attrHash) Then
            cMsg += "MD5,"
        End If


        If (My.Settings.FireflyLogAttributes And LogMask.attrType) Then
            cMsg += "TYPE,"
        End If


        If (My.Settings.FireflyLogAttributes And LogMask.attrApp) Then
            cMsg += "APPLICATION,"
        End If


        If (My.Settings.FireflyLogAttributes And LogMask.attrSize) Then
            cMsg += "SIZE,"
        End If

        If (My.Settings.FireflyLogAttributes And LogMask.attrCtime) Then
            cMsg += "CREATE TIME,"
        End If


        If (My.Settings.FireflyLogAttributes And LogMask.attrMtime) Then
            cMsg += "MODIFY TIME,"
        End If


        If (My.Settings.FireflyLogAttributes And LogMask.attrAtime) Then
            cMsg += "ACCESS TIME,"
        End If


        If (My.Settings.FireflyLogAttributes And LogMask.attrRegex) Then
            cMsg += "REGEX,"
        End If

        If (My.Settings.FireflyLogAttributes And LogMask.attrMatch) Then
            cMsg += "MATCH,"
        End If


        If (My.Settings.FireflyLogAttributes And LogMask.attrTotalMatches) Then
            cMsg += "COUNT,"
        End If

        If (My.Settings.FireflyLogAttributes And LogMask.attrScore) Then
            cMsg += "SCORE,"
        End If

        If (My.Settings.FireflyLogAttributes And Configuration.LogMask.attrAllHits) Then
            cMsg += "HITS,"
        End If

        cMsg = cMsg.TrimEnd(",")

        Return cMsg

    End Function
    'Public Function craft_csv_entry(ByVal pPath As String, ByVal hit As String, ByVal frag As String) As String
    '    ' we'll do the equivalent of a stat() on the file and provide the values 
    '    ' wanted
    '    ' Dim fAttr As Long
    '    Dim cMsg As String = ""
    '    ' Dim pArray() As String
    '    ' Dim basename As String
    '    Dim ext As String
    '    Dim assoc As String
    '    Dim fDate As Date
    '    Dim cArray(64) As String
    '    Dim item As Integer = 0
    '    '        Dim fInfo As New FileInfo(pPath)
    '    Dim fAttr As FileAttributes
    '    Dim i As Integer = 0
    '    Dim bArray() As Byte
    '    ' Dim hostinfo As String
    '    Dim md5 As New MD5CryptoServiceProvider


    '    ' what's the rule here?
    '    ' fields containing commas must be quoted
    '    ' fields containing double quotes must be quoted with the quote marks themselves quoted

    '    ' Debug.WriteLine("craft csv entry hit: " + hit)
    '    ' Debug.WriteLine("Loc: " + Loc)
    '    ' what are we logging?
    '    cMsg = ""
    '    If My.Settings.FireflyLogAttributes And LogMask.attrPath Then
    '        ' if this is an archive delegate, Loc carries the original path
    '        If ArchiveDelegate Then
    '            Debug.WriteLine("adding LOC as PATH")
    '            cArray(item) = quote_path(Loc)
    '            ArchiveDelegate = False
    '        Else
    '            cArray(item) = quote_path(pPath)
    '            Debug.WriteLine("adding PATH as PATH")
    '        End If
    '        Debug.WriteLine("ADDING: (" + cArray(item) + ")")
    '        item += 1
    '    End If

    '    If My.Settings.FireflyLogAttributes And LogMask.attrLocation Then
    '        If Loc = String.Empty Then
    '            Loc = "Unknown"
    '        End If
    '        ' if this is an archive delegate, Loc carries the original pat
    '        If ArchiveDelegate Then
    '            cArray(item) = System.IO.Path.GetFileName(pPath)
    '        Else
    '            cArray(item) = Loc
    '        End If
    '        item += 1
    '    End If

    '    '        Loc = ""

    '    If My.Settings.FireflyLogAttributes And LogMask.attrHash Then
    '        Dim md5hex As String
    '        Try
    '            Dim f As New FileStream(pPath, FileMode.Open, FileAccess.Read)
    '            md5.ComputeHash(f)
    '            f.Close()
    '            Dim md5byte() As Byte = md5.Hash
    '            Dim buff As StringBuilder = New StringBuilder
    '            Dim hashbyte As Byte
    '            For Each hashbyte In md5byte
    '                buff.Append(String.Format("{0:X1}", hashbyte))
    '            Next
    '            md5hex = buff.ToString
    '        Catch
    '            md5hex = "MD5"
    '        End Try
    '        cArray(item) = md5hex
    '        item += 1
    '    End If

    '    '        Debug.WriteLine("Logattr: " + LogAttributes.ToString)
    '    If My.Settings.FireflyLogAttributes And LogMask.attrType Then
    '        Try
    '            fAttr = File.GetAttributes(pPath)
    '            Debug.WriteLine("fAttr: " + fAttr.ToString)
    '            '    Debug.WriteLine("archive: " + IO.FileAttributes.Archive.ToString)
    '            If fAttr = FileAttributes.Archive Then
    '                cMsg += "Archive "
    '            End If
    '            If fAttr = FileAttributes.Compressed Then
    '                cMsg += "Compressed "
    '            End If
    '            If fAttr = FileAttributes.Device Then
    '                cMsg += "Device "
    '            End If
    '            If fAttr = FileAttributes.Directory Then
    '                cMsg += "Directory "

    '            End If
    '            If fAttr = FileAttributes.Encrypted Then
    '                cMsg += "Encrypted "

    '            End If
    '            If fAttr = FileAttributes.Hidden Then
    '                cMsg += "Hidden "

    '            End If
    '            If fAttr = FileAttributes.Normal Then
    '                cMsg += "Normal "

    '            End If
    '            If fAttr = FileAttributes.NotContentIndexed Then
    '                cMsg += "NotContentIndexed "

    '            End If
    '            If fAttr = FileAttributes.Offline Then
    '                cMsg += "Offline "

    '            End If
    '            If fAttr = FileAttributes.ReadOnly Then
    '                cMsg += "ReadOnly "

    '            End If
    '            If fAttr = FileAttributes.ReparsePoint Then
    '                cMsg += "ReparsePoint "

    '            End If
    '            If fAttr = FileAttributes.SparseFile Then
    '                cMsg += "Sparse "

    '            End If
    '            If fAttr = FileAttributes.System Then
    '                cMsg += "System "

    '            End If
    '            If fAttr = FileAttributes.Temporary Then
    '                cMsg += "Temporary "

    '            End If
    '        Catch ex As Exception
    '            cMsg += "Unavailable "

    '        End Try
    '        cArray(item) = cMsg
    '        item += 1
    '    End If

    '    If My.Settings.FireflyLogAttributes And LogMask.attrApp Then
    '        ' get the file extension
    '        ext = System.IO.Path.GetExtension(pPath).TrimStart(".")
    '        If ext = String.Empty Then
    '            assoc = "unavailable"
    '        Else
    '            assoc = Get_type(ext)
    '        End If
    '        Debug.WriteLine("assoc: " + assoc)
    '        If assoc = String.Empty Then
    '            cArray(item) = "Unavailable"
    '            item += 1
    '            '                cMsg += ",Unavailable,"
    '        Else
    '            cArray(item) = assoc
    '            item += 1
    '            '                cMsg += "," + assoc + ","
    '        End If
    '    End If

    '    If My.Settings.FireflyLogAttributes And LogMask.attrSize Then
    '        Dim fLen As Long = FileLen(pPath)
    '        cArray(item) = fLen.ToString
    '        item += 1
    '    End If

    '    If My.Settings.FireflyLogAttributes And LogMask.attrCtime Then
    '        fDate = IO.File.GetCreationTime(pPath)
    '        cArray(item) = get_date(fDate)
    '        item += 1
    '    End If

    '    If My.Settings.FireflyLogAttributes And LogMask.attrMtime Then
    '        fDate = IO.File.GetLastWriteTime(pPath)
    '        cArray(item) = get_date(fDate)
    '        item += 1
    '    End If

    '    If My.Settings.FireflyLogAttributes And LogMask.attrAtime Then
    '        fDate = IO.File.GetLastAccessTime(pPath)
    '        cArray(item) = get_date(fDate)
    '        item += 1
    '    End If

    '    If My.Settings.FireflyLogAttributes And LogMask.attrRegex Then
    '        If MessageNum > 0 Then
    '            hit += "[" + MessageNum.ToString + "]"
    '            hit += "[" + subject + "]"
    '        End If
    '        cArray(item) = hit
    '        item += 1
    '    End If

    '    If My.Settings.FireflyLogAttributes And LogMask.attrMatch Then
    '        bArray = StrToByteArray(frag)
    '        frag = ""
    '        Dim encoding As New System.Text.ASCIIEncoding
    '        ' frag = encoding.GetString(bArray)
    '        For i = 0 To (bArray.GetLength(0) - 1)
    '            If bArray(i) = 44 Then
    '                frag += "."
    '            ElseIf bArray(i) = 34 Then
    '                frag += "."
    '            ElseIf (bArray(i) >= 32 And bArray(i) < 127) Then
    '                'Debug.WriteLine("here: " + encoding.GetString(bArray, i, 1))
    '                frag += encoding.GetString(bArray, i, 1)
    '            Else
    '                frag += "."
    '            End If
    '        Next
    '        Debug.WriteLine("frag: " + frag)
    '        cArray(item) = frag
    '        item += 1
    '    End If

    '    If My.Settings.FireflyLogAttributes And LogMask.attrTotalMatches Then
    '        cArray(item) = totalMatches.ToString
    '        item += 1
    '    End If

    '    If (My.Settings.FireflyLogAttributes And LogMask.attrScore) Then
    '        If totalMatches > 0 Then
    '            score = validatedMatches / totalMatches
    '        Else
    '            score = 1.0
    '        End If
    '        cArray(item) = score.ToString("n2")
    '        item += 1
    '    End If

    '    If (My.Settings.FireflyLogAttributes And Configuration.LogMask.attrAllHits) Then
    '        Dim en As IDictionaryEnumerator = AllHits.GetEnumerator
    '        While en.MoveNext
    '            cArray(item) += en.Key + " "
    '        End While
    '    End If

    '    'cMsg = cMsg.Join(",", cArray)
    '    cMsg = ""
    '    For i = 0 To item
    '        cMsg += cArray(i) + ","
    '    Next

    '    cMsg = cMsg.TrimEnd(",")

    '    ' Debug.WriteLine("writing: " + cMsg)
    '    Return cMsg

    'End Function
    Public Sub syslog_send(ByVal message As String)
        ' we'll craft a syslog UDP packet and send it off, using the requested facility and a canned priority
        Dim udpClient As New UdpClient
        Dim outbytes() As Byte
        Dim fMsg As String
        Dim r As Integer
        Dim prio As Integer
        ' overkill, as we'll only let the user select audit/alert/local0 through local7
        Dim local0 As Integer = 16
        Dim local1 As Integer = 17
        Dim local2 As Integer = 18
        Dim local3 As Integer = 19
        Dim local4 As Integer = 20
        Dim local5 As Integer = 21
        Dim local6 As Integer = 22
        Dim local7 As Integer = 23
        Dim LOG_INFO As Integer = 6
        Dim LOG_NOTICE As Integer = 5
        Dim myHostname As String
        Dim now As Date

        Select Case Facility
            Case "local0"
                prio = (local0 * 8) + LOG_NOTICE
            Case "local1"
                prio = (local1 * 8) + LOG_NOTICE
            Case "local2"
                prio = (local2 * 8) + LOG_NOTICE
            Case "local3"
                prio = (local3 * 8) + LOG_NOTICE
            Case "local4"
                prio = (local4 * 8) + LOG_NOTICE
            Case "local5"
                prio = (local5 * 8) + LOG_NOTICE
            Case "local6"
                prio = (local6 * 8) + LOG_NOTICE
            Case "local7"
                prio = (local7 * 8) + LOG_NOTICE
        End Select

        fMsg = "<" + prio.ToString + ">"

        ' we need date/time
        now = Date.Now
        Select Case now.Month
            Case 1
                fMsg += "Jan "
            Case 2
                fMsg += "Feb "
            Case 3
                fMsg += "Mar "
            Case 4
                fMsg += "Apr "
            Case 5
                fMsg += "May "
            Case 6
                fMsg += "Jun "
            Case 7
                fMsg += "Jul "
            Case 8
                fMsg += "Aug "
            Case 9
                fMsg += "Sep "
            Case 10
                fMsg += "Oct "
            Case 11
                fMsg += "Nov "
            Case 12
                fMsg += "Dec "
        End Select

        fMsg += now.Day.ToString + " "
        fMsg += now.Hour.ToString + ":"
        fMsg += now.Minute.ToString + ":"
        fMsg += now.Second.ToString + " "


        ' we need our hostname
        ' this'll probably be a windows node name or some other silly shit.
        ' it'll certainly be upper case, which I can't stand.  

        myHostname = Dns.GetHostName.ToLower

        fMsg += " " + myHostname.ToString + " Firefly "

        fMsg += message.ToString

        udpClient.Connect(Loghost, syslog_port)

        outbytes = StrToByteArray(fMsg)
        ' blah, blah, craft packet

        r = udpClient.Send(outbytes, outbytes.Length)

        udpClient.Close()

    End Sub

    ' Write an error message to the log.
    Public Sub LogError(ByVal ex As Exception, Optional ByVal ShowDetails As Boolean = False)
        WriteToLog(ex.Message)

        If FireflyRunOptions And RunOptions.Debug Then ShowDetails = True
        If ShowDetails Then
            If Not ex.InnerException Is Nothing Then
                WriteToLog("Error Message: " + ex.InnerException.Message)
                If (FireflyRunOptions And RunOptions.Debug) Then
                    WriteToLog("Additional Debug Details: " + ex.InnerException.ToString)
                End If
            End If
        End If
    End Sub

    ' Write an error message for a file that Firefly failed to scan.
    '    This is our most common error.
    Public Sub LogError(ByVal ex As ScanFailedException, Optional ByVal ShowDetails As Boolean = False)

        If ex.FilePath = "" Then
            WriteToLog(ex.Message)
        Else
            WriteToLog(ex.Message + " " + ex.FilePath)
            UnableToScan(ex.FilePath, UnableToScanReason.UnhandledError)
        End If

        If FireflyRunOptions And RunOptions.Debug Then ShowDetails = True
        If ShowDetails Then
            If Not ex.InnerException Is Nothing Then
                WriteToLog("Error Message: " + ex.InnerException.Message)
                If (FireflyRunOptions And RunOptions.Debug) Then
                    WriteToLog("Additional Debug Details: " + ex.InnerException.ToString)
                End If
            End If
        End If

    End Sub

    ' Display and log an error.
    '   Use this ONLY if the error is unrecoverable.
    Public Sub DisplayAndLogError(ByVal ex As Exception, Optional ByVal ShowDetails As Boolean = False)
        LogError(ex, ShowDetails)
        If Not (FireflyRunOptions And RunOptions.Unattended) Then
            MsgBox(ex.Message, MsgBoxStyle.Critical, My.Application.Info.ProductName)
        End If
    End Sub

    Public Sub WriteToLog(ByVal toWrite As String)
        WriteToFile(toWrite, AppLogPath)
    End Sub

    Public Function PrepareReportFiles() As Boolean
        ' Dim ReportPath As String
        ' Dim adsPath() As String

        MatchCounts = New Hashtable(10)

        If FireflyReportOptions And ReportOptions.SendSecurityReport Then
            Try
                If File.Exists(SecurityReportPath) Then File.Delete(SecurityReportPath)
            Catch ex As Exception
                LogError(New Exception("Unable to clear the security report file at " + SecurityReportPath))
            End Try
            Try
                If File.Exists(SecurityReportSummaryPath) Then File.Delete(SecurityReportSummaryPath)
            Catch ex As Exception
                LogError(New Exception("Unable to clear the security report summary file at " + SecurityReportSummaryPath))
            End Try
        End If

        WriteToLog(APP_NAME + " started scanning on " + Now())

        ' WriteToLog("The report will be written to " + ExpandedReportPath)

        ' if there's a local log file, truncate it now
        ' if not, create an empty one

        ' if the log appears to contain ADS syntax, we'll check for the named stream.  In the case
        ' of ADS logging, we will *not* truncate the named stream if it already exists

        'If (FireflyReportOptions And Configuration.ReportOptions.HtmlFormat) Or (FireflyReportOptions And ReportOptions.CsvFormat) Or (FireflyReportOptions And ReportOptions.TextFormat) Then
        ' Debug.WriteLine("dealing with log path: " + ExpandedReportPath)
        'adsPath = ExpandedReportPath.Split(System.IO.Path.DirectorySeparatorChar)
        'ReportPath = adsPath((adsPath.Length - 1))
        'If InStr(ReportPath, ":") Then
        '    ' AlternateDataStreams
        'Else
        '    If IO.File.Exists(ExpandedReportPath) And Not (FireflyReportOptions And Configuration.ReportOptions.AppendLog) Then
        '        ' 7 pass wipe the previous log before truncating it.
        '        If (FireflyReportOptions And Configuration.ReportOptions.WipeLogBeforeUse) Then
        '            UpdateInterfaceMessage("Wiping log file ... " + ExpandedReportPath, StatusMedia.Status)
        '            wipe(ExpandedReportPath, Module2.WipeTarget.File, Module2.WipeMethod.DoD_7Pass, Module2.WipeWhenDone.Truncate)
        '        Else
        '            UpdateInterfaceMessage("Truncating log file ... " + ExpandedReportPath, StatusMedia.Status)
        '            wipe(ExpandedReportPath, Module2.WipeTarget.File, Module2.WipeMethod.Truncate, Module2.WipeWhenDone.Truncate)
        '        End If
        '    End If
        'End If
        '        End If

        ' If we're starting a new scan, delete the results file.
        If Not My.Settings.ScanInProgress Then
            ' We are starting a new scan.
            My.Settings.ScanPaused = False

            ' Delete/clear the list of skipped files.
            If FireflyReportOptions And ReportOptions.SaveSkippedFilesList Then
                Try
                    If File.Exists(SkippedFilesListPath) Then File.Delete(SkippedFilesListPath)
                Catch ex As Exception
                    LogError(New Exception("Unable to restart the list of skipped files at " + SkippedFilesListPath))
                End Try
            End If
            Try
                ' And delete any of version of the report file.
                If IO.File.Exists(CsvReportPath) Then
                    IO.File.Delete(CsvReportPath)
                End If
                If IO.File.Exists(ReportPath + ".txt") Then
                    IO.File.Delete(ReportPath + ".txt")
                End If
                If IO.File.Exists(ReportPath + ".htm") Then
                    IO.File.Delete(ReportPath + ".htm")
                End If
            Catch ex As Exception
                DisplayAndLogError(New Exception("Unable to restart the report file. Check to make sure you have permissions to access the " + My.Application.Info.ProductName + " report file.", ex), True)
                Return False
            End Try
            WriteReport_Header()
            WriteSecurityReport_Header()
        End If

        Return True
    End Function

    Public Sub UnableToScan(ByVal fileName As String, ByVal reason As UnableToScanReason)

        Dim reasonString As String
        Dim fileExt As String = fileName.Substring(fileName.LastIndexOf(".") + 1)
        Dim matchName As String = fileName.Substring(fileName.LastIndexOf("\") + 1)

        ' Let's track why we could not scan this file.
        Select Case reason
            Case UnableToScanReason.AccessDenied
                SkippedFiles_Permissions += 1
                reasonString = My.Application.Info.ProductName + " lacked sufficient permissions to scan this file."
            Case UnableToScanReason.FileInUse
                SkippedFiles_InUse += 1
                reasonString = "This file was in use by another application."
            Case UnableToScanReason.LargeSize
                WriteToLog("Quit scanning file due to large size: " + fileName)
                SkippedFiles_LargeSize += 1
                reasonString = "This file was larger than maximum size to be scanned."
            Case UnableToScanReason.UnrecognizedFileType
                SkippedFiles_UnknownType += 1
                reasonString = My.Application.Info.ProductName + " " + My.Application.Info.Version.ToString + " does not recognize files of this type."
            Case UnableToScanReason.UnhandledError
                SkippedFiles_Error += 1
                reasonString = "An unexpected error occurred while scanning this file. Consult the error log for details."
            Case UnableToScanReason.FileEncrypted
                SkippedFiles_Permissions += 1
                reasonString = "This file is encrypted."
            Case Else
                reasonString = ""
        End Select

        If FireflyReportOptions And ReportOptions.SaveSkippedFilesList Then
            Try
                WriteToFile(matchName + ", " + fileExt + ", " + fileName + ", " + reasonString, SkippedFilesListPath, True)
            Catch ex As Exception
                LogError(New Exception("Unable to write to the skipped files list at " + SkippedFilesListPath, ex))
                FireflyReportOptions = FireflyReportOptions Xor ReportOptions.SaveSkippedFilesList
            End Try
        End If
    End Sub

    ' This is the primary entry point for writing to reports.
    Public Sub WriteToReport(ByVal toWrite As String, Optional ByVal newline As Boolean = True)
        If (FireflyReportOptions And ReportOptions.CsvReport) Then
            WriteToCsvReport(toWrite)
        End If

        If (FireflyReportOptions And ReportOptions.TextFormat) Then
            WriteToTextReport(toWrite)
        End If

        If (FireflyReportOptions And ReportOptions.HtmlFormat) Then
            If newline Then
                WriteToHtmlReport(toWrite + "<br/>")
            Else
                WriteToHtmlReport(toWrite)
            End If
        End If
    End Sub

    ' This is how matches found are reported.
    Public Sub WriteToReport(ByVal matches As MatchesInFile)

        Dim regex As String = matches.FirstMatch
        Dim filePath As String = matches.PathToFile
        Dim matchName As String = matches.FileName
        Dim fileExt As String = matches.PathToFile.Substring(matches.PathToFile.LastIndexOf(".") + 1)

        If (FireflyReportOptions And ReportOptions.CsvReport) Then
            ' Filename, FirstMatch, Path to File, Extension
            WriteToCsvReport(matchName + "," + matches.Fragment + "," + filePath + "," + fileExt + ", " + regex)
        End If

        If (FireflyReportOptions And ReportOptions.TextFormat) Then
            WriteToTextReport("Match (" + regex + "): " + filePath)
        End If

        If (FireflyReportOptions And ReportOptions.HtmlFormat) Then
            Dim entry As String = ""
            entry += "<tr>"
            entry += "<td>" + matchName + "</td>"
            ' entry += "<td>" + matches.FirstMatch + "</td>"
            ' entry += "<td>" + matches.MatchCount.ToString + "</td>"
            entry += "<td>" + matches.Fragment + "</td>"
            entry += "<td><a href='file:\\\" + filePath + "'> View File</a> <a href='file:///" + matches.PathToFolder + "'>Open Folder</a></td>"
            entry += "<td>" + filePath + "</td>"
            entry += "</tr>"
            WriteToHtmlReport(entry)
        End If

    End Sub

    ' These are helper functions that support the different report file types.
    Private Sub WriteToHtmlReport(ByVal toWrite As String)
        If FireflyReportOptions And ReportOptions.HtmlFormat Then
            WriteToFile(toWrite, ReportPath + ".htm")
        End If
    End Sub
    Private Sub WriteToCsvReport(ByVal toWrite As String)
        If FireflyReportOptions And ReportOptions.CsvReport Then
            WriteToFile(toWrite, CsvReportPath)
        End If
    End Sub
    Private Sub WriteToTextReport(ByVal toWrite As String)
        WriteToFile(toWrite, ReportPath + ".txt")
    End Sub

    Public Sub StoreSecurityMetric(ByVal name As String, ByVal value As String)
        ' WriteToSecurityReport("<member><name>" + name + "</name><value>" + value + "</value></member>")
        WriteToFile("<member><name>" + name + "</name><value>" + value + "</value></member>", SecurityReportPath)
        If name <> "Unrecognized extensions" Then
            WriteToFile(name + ": " + value, SecurityReportSummaryPath)
        End If
    End Sub
    Public Sub WriteReport_Header()

        ' "Firefly Version x.x.x Scan Results"
        WriteToHtmlReport("<html><head><title>" + My.Application.Info.ProductName + " Scan Results</title>")
        WriteToCsvReport(My.Application.Info.ProductName + " Scan Results")

        ' Prepare the HTML report...
        WriteToHtmlReport(ReportStyle)
        WriteToHtmlReport("</head><body>")
        WriteToHtmlReport("<h1>" + My.Application.Info.ProductName + " Scan Results</h1>")
        WriteToHtmlReport("<p>Firefly has identified the following files as possibly containing sensitive data. All but the last three digits of each match have been replaced with 'X's. Please examine each of these files and delete or securely archive each file that contains SSNs or Credit Card numbers. For additional help see <a href='http://www.cites.uiuc.edu/ssnprogram/'>http://www.cites.uiuc.edu/ssnprogram/</a></p>")
        WriteToHtmlReport("<hr/><h2>Scan Results</h2>")

        ' Print file-type specific table column headings...
        WriteToHtmlReport("<table><caption>These files may contain sensitive information.</caption><tr><th>Filename</th><th>Suspected sensitive content</th><th>Actions</th><th>Path to file</th></tr>")
        ' WriteToCsvReport(gen_header()) ' Handles customized fields - we don't currently support this.
        WriteToCsvReport("Filename, Suspected sensitive content, Path to file, File Extension")

    End Sub
    Public Sub WriteReport_Footer()

        If My.Settings.FilesWithMatches = 0 Then
            WriteToCsvReport("Congratulations. None of the files scanned appear to contain sensitive information.")
            WriteToHtmlReport("<tr><td colspan=*><i>Congratulations. None of the files scanned appear to contain sensitive information.</i></td></tr>")
        End If

        ' HTML report specific
        WriteToHtmlReport("</table><hr/>")
        WriteToHtmlReport("<h2>Scan Summary</h2>")

        ' Content for all reports
        WriteToReport(APP_NAME + " scanned " + My.Settings.TotalFilesScanned.ToString + " files in " + My.Settings.ScanDir + ".")
        WriteToReport("Firefly found that " + My.Settings.FilesWithMatches.ToString + " of the files scanned contain may contain sensitive information.")
        WriteToReport("These files contain " + My.Settings.ReportAfterXMatches.ToString + " or more matches for possible sensitive information.")
        If SkippedFiles_UnknownType > 0 Then
            WriteToReport(SkippedFiles_UnknownType.ToString + " files were skipped because Firefly did not recognize their file types.")
        End If
        If SkippedFiles_Permissions > 0 Then WriteToReport(SkippedFiles_Permissions.ToString + " files were skipped because the current user does not have permission to access them.")
        If SkippedFiles_LargeSize > 0 Then WriteToReport(SkippedFiles_LargeSize.ToString + " files were only partially scanned due to being very large.")
        If Skippedfiles_KnownType > 0 Then WriteToReport(Skippedfiles_KnownType.ToString + " files were skipped because Firefly recognized them as system files.")
        WriteToReport(SkippedFiles_InUse.ToString + " files were skipped because they were in use.")
        If SkippedFiles_Error > 0 Then WriteToReport(SkippedFiles_Error.ToString + " files were skipped due to an unanticipated error. See the application log for more details.")

        ' Security report information
        If SecurityReportSent Then
            WriteToReport("A summary of this scan has been sent to CITES Security.")
        Else
            WriteToReport(My.Application.Info.ProductName + " was unable to send a summary of this scan to CITES Security.")
        End If

        ' Backup report, and report backup
        If BackupReport() Then
            Dim BackUpFile As String = ReportDirectory + "\" + ReportFileName + "." + System.Net.Dns.GetHostName + "." + Format(Now, "yyyy.MM.dd.hh.mm")
            WriteToHtmlReport("<hr/>This report is saved at " + BackUpFile + ".htm")
            WriteToCsvReport("This report is saved at " + BackUpFile + ".csv")
        Else
            WriteToHtmlReport("<hr/>This report is saved at " + ReportPath + ".htm")
            WriteToCsvReport("This report is saved at " + ReportPath + ".csv")
        End If

        ' HTML report specific closure
        WriteToHtmlReport("</body>")
        WriteToHtmlReport("</html>")

    End Sub

    Public Function BackupReport() As Boolean
        Dim BackUpFile As String = LogDirectory + "\" + ReportFileName + "." + System.Net.Dns.GetHostName + "." + Format(Now, "yyyy.MM.dd.HH.mm")
        Dim BackUpFile_WithExt As String
        Dim ReportFile As String

        Try
            Dim aryExt As New System.Collections.Specialized.StringCollection
            aryExt.Add(".csv")
            aryExt.Add(".txt")
            aryExt.Add(".htm")
            Dim ext As String

            For Each ext In aryExt
                ReportFile = ReportPath + ext
                If ext = ".csv" Then ReportFile = CsvReportPath
                If IO.File.Exists(ReportFile) Then
                    BackUpFile_WithExt = BackUpFile + ext
                    Try
                        IO.File.Copy(ReportFile, BackUpFile_WithExt)
                    Catch nio As IOException
                        LogError(New Exception("Unable to make an archive copy of the report at " + BackUpFile_WithExt + "; file is in use.", nio))
                        Return False
                    Catch avex As AccessViolationException
                        LogError(New Exception("Unable to make an archive copy of the report at " + BackUpFile_WithExt + "; current user does not have proper permissions.", avex))
                        Return False
                    Catch uaEx As UnauthorizedAccessException
                        LogError(New Exception("Unable to make an archive copy of the report at " + BackUpFile_WithExt + "; current user does not have proper permissions.", uaEx))
                        Return False
                    Catch ex As Exception
                        LogError(New Exception("Unable to make an archive copy of the report at " + BackUpFile_WithExt + "", ex), True)
                        Return False
                    End Try
                End If
            Next
            Return True
        Catch ex As Exception
            LogError(ex)
            Return False
        End Try
    End Function

    Public Sub WriteSecurityReport_Header()
        StoreSecurityMetric("ScanStarted", Now().ToString("yyyy/MM/dd:hh:mm:ss"))
        StoreSecurityMetric("ReportSource", APP_NAME)
        
        Dim host As New Net.IPHostEntry()
        host = Net.Dns.GetHostEntry(Net.Dns.GetHostName())
        StoreSecurityMetric("HostName", host.HostName)
        Dim x As Net.IPAddress
        Dim ipAddresses As String = ""
        For Each x In host.AddressList
            ipAddresses += x.ToString + " "
        Next
        StoreSecurityMetric("IpAddresses", ipAddresses)
    End Sub
    Public Sub WriteSecurityReport_Footer()
        StoreSecurityMetric("ScanFinished", Now().ToString("yyyy/MM/dd:hh:mm:ss"))
        StoreSecurityMetric("LocationScanned", My.Settings.ScanDir)
        StoreSecurityMetric("FilesWithResults", My.Settings.FilesWithMatches)
        StoreSecurityMetric("SkippedFilesLarge", SkippedFiles_LargeSize.ToString)
        StoreSecurityMetric("SkippedFilesInUse", SkippedFiles_InUse.ToString)
        StoreSecurityMetric("SkippedFilesPermissions", SkippedFiles_Permissions.ToString)
        StoreSecurityMetric("SkippedFilesError", SkippedFiles_Error.ToString)
        StoreSecurityMetric("SkippedFilesIgnoreExtension", Skippedfiles_KnownType.ToString)
        StoreSecurityMetric("MatchThreshhold", My.Settings.ReportAfterXMatches)

        If My.Settings.ScanPaused Then
            StoreSecurityMetric("ScanPaused", 1)
        Else
            StoreSecurityMetric("ScanPaused", 0)
        End If

        Dim ssns As Integer = 0
        If MatchCounts.ContainsKey("Nine Digit Number (Possible SSN)") Then
            ssns += MatchCounts.Item("Nine Digit Number (Possible SSN)")
        End If
        If MatchCounts.ContainsKey("Formatted SSN") Then
            ssns += MatchCounts.Item("Formatted SSN")
        End If
        StoreSecurityMetric("SsnMatches", ssns)
        Dim ccns As Integer = 0
        If MatchCounts.ContainsKey("Visa or Mastercard Number") Then
            ccns += MatchCounts.Item("Visa or Mastercard Number")
        End If
        If MatchCounts.ContainsKey("American Express Card Number") Then
            ccns += MatchCounts.Item("American Express Card Number")
        End If
        StoreSecurityMetric("CcnMatches", ccns)

        StoreSecurityMetric("SkippedFilesUnRecExt", SkippedFiles_UnknownType)

        'If SkippedFiles_UnknownType > 0 Then
        '    Dim extensions As String = ""
        '    For Each i As Object In UnrecognizedFileTypes
        '        extensions += " " + i.ToString
        '    Next
        '    StoreSecurityMetric("UnrecognizedExtensions", extensions)
        'End If

        StoreSecurityMetric("TotalFilesScanned", My.Settings.TotalFilesScanned)
    End Sub
    Public Function PromptSecurityReport() As Boolean
        Dim info As String
        ' Dim sr2 As StreamReader = New StreamReader(SecurityReportSummaryPath)
        ' info = sr2.ReadToEnd()
        Dim promptForm As SecurityReportPrompt = New SecurityReportPrompt
        ' info += Chr(13) + "And a list of unrecognized file extensions."
        info = ""
        info += "The host name of the computer." + Chr(13)
        info += "The ip addresses of the computer." + Chr(13)
        info += "The disk location scanned." + Chr(13)
        info += "The number of files scanned." + Chr(13)
        info += "The number of files skipped." + Chr(13)
        info += "The numbers and types of possible matches encountered." + Chr(13)
        info += "The numbers and types of errors encountered." + Chr(13)
        info += "The date and time the scan started and finished." + Chr(13)

        info += My.Application.Info.ProductName + " will NOT send "
        info += " the files it scans, or the file names or contents." + Chr(13)

        promptForm.reportContent.Text = info
        Dim result As DialogResult
        result = promptForm.ShowDialog()
        ' sr2.Close()
        ' sr2.Dispose()

        Return (result = DialogResult.OK)
        ' Return True
        ' Return (MsgBox("Submit feedback to CITES Security?" + Chr(13) + "If you select 'Yes', following information will be sent:" + Chr(13) + info, MsgBoxStyle.YesNo, My.Application.Info.ProductName) = MsgBoxResult.Yes)
    End Function
    Public Function SendSecurityReport() As Boolean
        Try
            ' Dim req As HttpWebRequest = WebRequest.Create("http://localhost:8000/RPC2")
            Dim req As HttpWebRequest
            Dim sw As StreamWriter
            Dim sr2 As StreamReader = New StreamReader(SecurityReportPath)

            ' Dim result As IAsyncResult

            ' req = HttpWebRequest.Create("https://secdev2.cites-security.uiuc.edu/firefly/report.cgi") ' Test server
            ' req = HttpWebRequest.Create("https://tools.cites-security.uiuc.edu/firefly/report.cgi") ' Legacy Production server
            req = HttpWebRequest.Create("https://tools.cites-security.uiuc.edu/firefly/report.0.8.5.cgi") ' Legacy Production server

            req.Method = "POST"
            req.ContentType = "text/xml"

            ' req.Timeout = 10000

            req.UserAgent = APP_NAME

            ' Accept our SSL certificate.
            ' Dim certPolicy As New TrustAllCertificatesPolicy()
            System.Net.ServicePointManager.CertificatePolicy = New TrustAllCertificatesPolicy

            sw = New StreamWriter(req.GetRequestStream())
            Dim message As String = "<?xml version='1.0'?> <methodCall>  <methodName>LogStruct</methodName>  <params>    <param>        <value><struct>"
            message = message + sr2.ReadToEnd
            sr2.Close()
            message = message + "</struct></value>    </param>  </params></methodCall> "
            ' If FireflyRunOptions And RunOptions.Debug Then Clipboard.SetDataObject(message, True)

            Try
                sw.Write(message)
                sw.Close()

                Dim sr As StreamReader = New StreamReader(req.GetResponse.GetResponseStream())
                Dim result As String = sr.ReadToEnd()

                'If Not result.Contains("<boolean>1</boolean>") Then
                If FireflyRunOptions And RunOptions.Debug Then
                    ' DisplayAndLogError(New Exception("The CITES Security data collector returned: " + result), False)
                End If
                'End If
            Catch wEx As WebException
                If wEx.Message.Contains("404") Then
                    LogError(New Exception(My.Application.Info.ProductName + " was unable to find the CITES Security data collector.", wEx), False)
                ElseIf wEx.Message.Contains("500") Then
                    LogError(New Exception(My.Application.Info.ProductName + " was unable to submit a report summary to CITES Security as the data collector is currently unavailable.", wEx), False)
                Else
                    LogError(New Exception(My.Application.Info.ProductName + " was unable to submit a report summary to CITES Security. Details follow: ", wEx), True)
                End If
                Return False
            Catch ex As Exception
                LogError(New Exception(My.Application.Info.ProductName + " was unable to submit a report summary to CITES Security. Details follow: ", ex), True)
                Return False
            Finally
                sw.Close()
                sw.Dispose()
            End Try
            Return True
            WriteToLog("Security report sent.")
        Catch wEx As WebException
            LogError(New Exception("Failed to send security report.", wEx), True)
            Return False
        Catch ex As Exception
            LogError(New Exception("Failed to send security report.", ex), True)
            Return False
        End Try
    End Function

    Public Function SendSecurityHostName() As Boolean
        Try
            Dim req As HttpWebRequest
            Dim sw As StreamWriter

            ' Dim result As IAsyncResult

            req = HttpWebRequest.Create("https://secdev2.cites-security.uiuc.edu/firefly/report.cgi")
            req.Method = "POST"
            req.ContentType = "text/xml"

            ' req.Timeout = 10000

            req.UserAgent = APP_NAME

            Dim host As New Net.IPHostEntry()
            host = Net.Dns.GetHostEntry(Net.Dns.GetHostName())

            ' Accept our SSL certificate.
            ' Dim certPolicy As New TrustAllCertificatesPolicy()
            System.Net.ServicePointManager.CertificatePolicy = New TrustAllCertificatesPolicy

            sw = New StreamWriter(req.GetRequestStream())
            Dim message As String = "<?xml version='1.0'?> <methodCall>  <methodName>LogValue</methodName>  <params>    <param>        <value><string>"
            message = message + APP_NAME + " ran on " + host.HostName + " on " + Date.Today.ToString
            message = message + "</string></value>    </param>  </params></methodCall> "

            Try
                sw.Write(message)
                sw.Close()

                Dim sr As StreamReader = New StreamReader(req.GetResponse.GetResponseStream())
                Dim result As String = sr.ReadToEnd()

                ' MsgBox(result)
            Catch wEx As WebException
                LogError(New Exception("Unable to report scan date and hostname to CITES Security.", wEx), True)
                Return False
            Catch ex As Exception
                LogError(New Exception("Unable to report scan date and hostname to CITES Security.", ex), True)
                Return False
            Finally
                sw.Close()
                sw.Dispose()
            End Try
            Return True
            WriteToLog("Scan date and hostname reported to CITES Security.")
        Catch wEx As WebException
            LogError(New Exception("Unable to report scan date and hostname to CITES Security.", wEx), True)
            Return False
        Catch ex As Exception
            LogError(New Exception("Unable to report scan date and hostname to CITES Security.", ex), True)
            Return False
        End Try
    End Function

    'Public Sub WriteToSecurityReport(ByVal toWrite As String, Optional ByVal newline As Boolean = True)
    '    ' WriteToReport(toWrite)
    '    WriteToFile(toWrite, SecurityReportPath, newline)
    'End Sub

    'Public Sub WriteToReport(ByVal toWrite As String, ByVal pPath As String)
    '    WriteToFile(toWrite, pPath)
    'End Sub
    Public Sub WriteToFile(ByVal toWrite As String, ByVal pPath As String, Optional ByVal newline As Boolean = True)
        Dim bArray() As Byte
        Dim lWrite As System.IO.FileStream
        Try
            lWrite = New System.IO.FileStream(pPath, FileMode.Append, FileAccess.Write)
            Try
                bArray = StrToByteArray(toWrite)
                lWrite.Write(bArray, 0, bArray.Length)
                If newline Then
                    lWrite.WriteByte(System.Byte.Parse("13"))
                    lWrite.WriteByte(System.Byte.Parse("10"))
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                lWrite.Close()
            End Try
        Catch ex As Exception
            If pPath = AppLogPath Then
                MsgBox(APP_NAME + " cannot write to the application log at " + pPath + ". If the file is open, close it and then restart this scan. Full error details follow: " + ex.Message)
                ThreadTracking.StopRequested = True
            ElseIf pPath = ReportPath Then
                MsgBox(APP_NAME + " cannot write to the report file at " + pPath + ". If the file is open, close it and then restart this scan. Full error details follow: " + ex.Message)
                ThreadTracking.StopRequested = True
            ElseIf pPath = CsvReportPath Then
                MsgBox(APP_NAME + " cannot write to the report file at " + pPath + ". If the file is open, close it and then restart this scan. Full error details follow: " + ex.Message)
                ThreadTracking.StopRequested = True
            Else
                LogError(New Exception(APP_NAME + " tried and failed to write to: " + pPath, ex), True)
            End If
        End Try
    End Sub

    Public Sub EncryptReport(ByVal pPath As String, ByVal direction As CryptoAction)
        ' we'll treat this as a file stream and pump enc_log_array entries into the encryptor 
        ' until we're done, then close the file.
        ' reading the file will be the reverse process.

        '        Dim plaintext As String
        Dim logText As String = ""
        Dim logCipher As String = ""
        ' Dim isADS As Boolean = False
        Dim newPath As String = ""
        Dim dStream As String = ""

        If PASSWORD = String.Empty Then
            MsgBox("No password supplied for log encryption.", MsgBoxStyle.OkOnly, My.Application.Info.ProductName)
            Return
        End If

        '' are we logging to an ADS?
        'Try
        '    Dim dInfo As New DirectoryInfo(pPath)
        '    Dim fInfo As New FileInfo(pPath)
        '    If InStr(dInfo.Name, ":") Then
        '        Debug.WriteLine("logfile is an ADS")
        '        isADS = True
        '        ' if the existing named stream exists, call ADSFile.appendtext
        '        ' if not, create the named stream, then call ADSFile.appendtext
        '        ' we'll need to grab the file name, split it into the named and alternate streams
        '        get_ads_parts(pPath, newPath, dStream)
        '        If Not IO.File.Exists(newPath) Then
        '            Try
        '                IO.File.Create(newPath)
        '            Catch ex As Exception
        '                MsgBox("Unable to create the log file. Check the directory permissions.")
        '            End Try
        '        End If
        '    End If
        'Catch ex As Exception
        '    ' Debug.WriteLine("exception: " + ex.ToString)
        '    LogError(ex)
        '    Return
        'End Try

        enc_log_plaintext = ""
        Try 'In case of errors.

            'Determine if ecryption or decryption and setup CryptoStream.
            Select Case direction
                Case CryptoAction.Encrypt
                    Dim i As Integer
                    For i = 0 To (enc_log_entries - 1)
                        logText &= enc_log_array(i) + vbCrLf
                    Next
                    logCipher = FireflyEncrypt(logText, PASSWORD, SALT, "SHA1", 8, IV, 128)
                    'If isADS Then
                    '    ' call adsfile.appendtext to write the base64 encoded ciphertext to the log
                    '    ' if the log exists, truncate it
                    '    If ADSFile.Exists(newPath, dStream) Then
                    '        ADSFile.Delete(newPath, dStream)
                    '    End If
                    '    Dim adsOutput As System.IO.StreamWriter = ADSFile.AppendText(newPath, dStream)
                    '    adsOutput.Write(logCipher)
                    '    adsOutput.Close()
                    'Else
                    Dim fsOutput As New System.IO.FileStream(pPath, _
                    FileMode.OpenOrCreate, _
                    FileAccess.Write)
                    fsOutput.SetLength(0) 'make sure fsOutput is empty
                    'ReDim logByte(logText.Length)
                    fsOutput.Write(StrToByteArray(logCipher), 0, logCipher.Length)
                    fsOutput.Close()
                    'End If
                    ' nuke the cleartext log
                    ReDim enc_log_array(1)
                    enc_log_entries = 0
                Case CryptoAction.Decrypt
                    ' pull the whole file into a string and decrypt
                    ' we'll return that
                    ' need to handle ADS
                    'If isADS Then
                    '    Dim adsInput As System.IO.StreamReader = ADSFile.OpenText(newPath, dStream)
                    '    logCipher = adsInput.ReadToEnd
                    '    adsInput.Close()
                    'Else
                    ' read in the base64 encoded ciphertext
                    ' Dim fsInput As System.IO.File
                    Dim fsRead As IO.StreamReader
                    fsRead = File.OpenText(pPath)
                    logCipher = fsRead.ReadToEnd
                    fsRead.Close()
                    'End If
                    enc_log_plaintext = FireflyDecrypt(logCipher, PASSWORD, SALT, "SHA1", 8, IV, 128)
                    ' Debug.WriteLine("got back: " + enc_log_plaintext)
            End Select
        Catch ex As Exception
            ' Debug.WriteLine("Exception thrown")
            ' Debug.WriteLine(ex.ToString)
            LogError(ex)
            Return
        End Try

        Return
    End Sub


End Module