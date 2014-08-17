
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Text ' Encoding
Imports System.Net
Imports System.Net.Sockets

Imports System.Reflection
Imports System.Xml
Imports System.Data.OleDb
Imports System.Threading

' Imports Cornell.Regexes
Imports ICSharpCode.SharpZipLib
' Imports AlternateDataStream
Imports iTextSharp

Module Scanner


    Public APP_NAME As String
    
#Region "Enumerations"
    Public Enum CryptoAction
        Encrypt = 1
        Decrypt = 2
    End Enum
    Public Enum ArchiveType
        Zip = 1
        Bzip = 2
        Gzip = 3
    End Enum

    Public Enum SelectBool
        SelectOr = 1
        SelectAnd = 2
    End Enum
    Public Enum DateMatch
        cleared = 0
        accesstime = 1
        modifytime = 2
        createtime = 4
    End Enum
    Public Enum ResetDiffs
        Checkpoint = 1
        Incremental = 2
    End Enum
    Public Enum Form8_Dispo
        SkipPaths = 1
        SkipContent = 2
    End Enum
    Public Form8_State As Form8_Dispo = Form8_Dispo.SkipPaths
#End Region

    Public Class FileTimeMatch
        Private ctimeLo As Date
        Private ctimeHi As Date
        Private mtimeLo As Date
        Private mtimeHi As Date
        Private atimeLo As Date
        Private atimeHi As Date
        Private matchtype As DateMatch
        Public Sub New(ByVal dateLo As Date, ByVal dateHi As Date, ByVal datetype As DateMatch)
            If datetype = DateMatch.accesstime Then
                atimeLo = dateLo
                atimeHi = dateHi
            ElseIf datetype = DateMatch.createtime Then
                ctimeLo = dateLo
                ctimeHi = dateHi
            ElseIf datetype = DateMatch.modifytime Then
                mtimeLo = dateLo
                mtimeHi = dateHi
            Else

            End If

        End Sub
        Public Sub Add(ByVal dateLo As Date, ByVal dateHi As Date, ByVal datetype As DateMatch)
            If datetype = DateMatch.accesstime Then
                atimeLo = dateLo
                atimeHi = dateHi
            ElseIf datetype = DateMatch.createtime Then
                ctimeLo = dateLo
                ctimeHi = dateHi
            ElseIf datetype = DateMatch.modifytime Then
                mtimeLo = dateLo
                mtimeHi = dateHi
            Else

            End If
        End Sub
        Public ReadOnly Property GetMatchType() As DateMatch
            Get
                Return matchtype
            End Get
        End Property
        Public ReadOnly Property GetDateLo() As Object
            Get
                If matchtype = DateMatch.accesstime Then
                    Return atimeLo
                ElseIf matchtype = DateMatch.createtime Then
                    Return ctimeLo
                ElseIf matchtype = DateMatch.modifytime Then
                    Return mtimeLo
                Else
                    Throw New Exception("An unrecognized date type was encountered.")
                End If
            End Get
        End Property
        Public ReadOnly Property GetDateHi() As Object
            Get
                If matchtype = DateMatch.accesstime Then
                    Return atimeHi
                ElseIf matchtype = DateMatch.createtime Then
                    Return ctimeHi
                ElseIf matchtype = DateMatch.modifytime Then
                    Return mtimeHi
                Else
                    Throw New Exception("An unrecognized date type was encountered.")
                End If
            End Get
        End Property
    End Class
    ' linked list class to hold regular expressions.  we'll populate this at start time 
    ' according to our configuration, then iterate over it when matching
    Public Class RegexEntry
        Private reExpression As Regex
        Private reText As String
        Private reName As String
        Private reValidator As Validator
        Private reValidatorExtra As ValidatorTypes
        Private RENext As RegexEntry
        Public Sub New(ByVal regexObject As Regex, ByVal regexText As String, ByVal name As String, ByVal validator As Validator, ByVal validatorType As ValidatorTypes)
            reExpression = regexObject
            reText = regexText
            reValidator = validator
            reName = name
            reValidatorExtra = validatorType
        End Sub
        Public Property NextItem() As RegexEntry
            Get
                Return RENext
            End Get
            Set(ByVal Value As RegexEntry)
                RENext = Value
            End Set
        End Property
        Public ReadOnly Property RegExObject() As Object
            Get
                Return reExpression
            End Get
        End Property
        Public ReadOnly Property Regex() As Regex
            Get
                Return reExpression
            End Get
        End Property
        Public ReadOnly Property RegExText() As String
            Get
                Return reText
            End Get
        End Property
        Public ReadOnly Property RegName() As Object
            Get
                Return reName
            End Get
        End Property
        Public ReadOnly Property RegValidator() As Validator
            Get
                Return reValidator
            End Get
        End Property
        Public ReadOnly Property RegValidatorExtra() As Object
            Get
                Return reValidatorExtra
            End Get
        End Property
    End Class
    Public Class RegexList
        Private REHead As RegexEntry
        Private RETail As RegexEntry
        Public Sub Add(ByVal rent As Regex, ByVal rtext As String, ByVal rname As String, ByVal rval As Validator, ByVal rex As ValidatorTypes)
            Dim item As New RegexEntry(rent, rtext, rname, rval, rex)
            If REHead Is Nothing Then
                REHead = item
            End If
            If RETail Is Nothing Then
                RETail = item
            Else
                RETail.NextItem = item
                RETail = item
            End If
        End Sub
        Public Sub Clear()
            If Not REHead Is Nothing Then
                REHead = Nothing
            End If
        End Sub
        Public Sub Remove(ByVal rtext As String)
            Dim prevNode As RegexEntry = Nothing
            Dim curNode As RegexEntry = REHead
            While Not (curNode Is Nothing)
                If (curNode.RegExText = rtext) Then
                    If (prevNode Is Nothing) Then
                        If (REHead Is RETail) Then
                            RETail = Nothing
                            REHead = RETail
                        Else
                            REHead = REHead.NextItem
                        End If
                    Else
                        prevNode.NextItem = curNode.NextItem
                        Exit While
                    End If
                    Return
                End If
                prevNode = curNode
                curNode = curNode.NextItem
            End While
            Return
        End Sub
        Public Property Head() As RegexEntry
            Get
                Return REHead
            End Get
            Set(ByVal Value As RegexEntry)
                REHead = Value
            End Set
        End Property
    End Class
    ' head of our linked list of regular expressions
    Public RE_Start As New RegexList
    Public RE_Custom_Start As New RegexList
    Public RE_head_array(256) As Object
    Public Match_Time As New FileTimeMatch(Convert.ToDateTime("01/01/0001"), Convert.ToDateTime("01/01/0001"), DateMatch.cleared)



    ' constants
    Public Const MaxRedirections As Integer = 25
    Public Const READ_SIZE As Integer = 65535
    Public Const syslog_port As Integer = 514
    Public Const FILE_BEGIN As Integer = 0
    Public Const FILE_CURRENT As Integer = 1
    Public Const FILE_END As Integer = 2
    Public Const MIN_FREE_SPACE As Int64 = 1073741824

    ' floats live here:
    Public score As Double = 0.0

    ' dates live here
    Public Firefly_start As Date
    Public Firefly_end As Date
    Public LastScanDate As Date
    ' Public OUR_EPOCH As DateTime

    Public FileSelectBool As SelectBool = SelectBool.SelectOr

    ' checkpoint support here
    Public CheckPointPath As String = String.Empty
    Public CheckPointFound As Boolean = False


    ' Public BackupReport As Boolean = True

    ' Button Text
    Public RunButton_Stop As String
    Public RunButton_Play As String
    Public RunButton_Pause As String

    ' Windows loves me.
    Public editregex As Boolean = False
    Public regex_count As Integer
    Public Regexes(256) As String
    Public ArchiveDepth As Integer = 0

    ' Public My.Settings.StartDir As String
    Public WebModeStartURL As String

    Public NewConfig As Boolean = False

    Public MatchSize As Integer = 128
    Public MessageNum As Integer = 0
    Public subject As String

    ' WebFirefly
    Public MaxURLDepth As Integer = 10
    Public MaxContentLength As Integer = 10 ' value in MB
    Public DomainDepth As Integer = 3
    Public BasicAuthUser As String = ""
    Public BasicAuthPassword As String = ""
    Public UserAgent As String = ""

    Public UnallocSearchDrive As String
    Public UnallocFile As String = "unalloc_matches.dat"
    Public HDD_BLOCKSIZE As Integer = 4096
    Public HDD_FREESPACE As Int64
    Public HDD_NUMBER_OF_BLOCKS As Int64
    Public SMVN_priv As Boolean = False

    ' Public LogTotalMatches As Boolean = False
    ' Public totalMatches As Integer = 0
    ' Public validatedMatches As Integer = 0
    Public LogHostInfo As String = ""
    Public TotalHits As Long = 0
    Public Loc As String


    '  Public Priority As Integer = 0
    Public Done As Integer = 0
    Public addRegex As String

    ' hashtables live here
    Public ScanExts As New Hashtable
    ' Public KeepExts As New Hashtable
    Public SkipExts As New Hashtable
    Public SkipPaths As New Hashtable
    Public SkipFileNames As New Hashtable

    ' Public SkipPaths2Globs As New Hashtable
    Public Area2Group As New Hashtable
    Public CCPrefixes As New Hashtable
    '  Public RegexValidators As New Hashtable
    ' Public RegexNames As New Hashtable
    Public RobotsDone As New Hashtable
    Public DriveBlocksize As New Hashtable
    Public DriveFreespace As New Hashtable
    Public SkipContentRegex As New Hashtable ' regexes converted from globs
    Public SkipContentTypes As New Hashtable ' globs
    Public DelegateContentTypes As New Hashtable ' we'll populate this at startup
    ' Public HitItems As New Hashtable
    ' Public AllHits As New Hashtable
    Public URLLoopDetect As New Hashtable

    ' Canned regexes
    ' SSNs
    'Public reSSN324 As New Cornell.Regexes.SSN
    'Public reSSN324_break As New Cornell.Regexes.SSN_B
    'Public reSSN9 As New Cornell.Regexes.SSN_9 ' also used for SINs
    'Public reSSN9_break As New Cornell.Regexes.SSN_9_B ' also used for SINs
    ' SINs
    'Public reSIN333 As New Cornell.Regexes.SIN
    'Public reSIN333_break As New Cornell.Regexes.SIN_B
    ' NINOs
    'Public reNINO As New Cornell.Regexes.NINO
    ' AMEX pattern
    'Public reAMEX As New Cornell.Regexes.AMEX
    'Public reAMEX_break As New Cornell.Regexes.AMEX_B
    ' VMCD pattern
    'Public reVMCD As New Cornell.Regexes.VMCD
    'Public reVMCD_break As New Cornell.Regexes.VMCD_B

    Public Loghost As String
    Public Facility As String
    ' Public files_processed As Integer = 0
    ' Public ScanDepth As Int64 = 100024 ' 100,024 KB = 100 MB
    Public ScanDepth As Int64 = 0 ' Fully scan all files until complete.

    Public MaxArchiveSize As Integer = 200
    Public MinFreeGB As Integer = 2

    ' Private LastStatusUpdate As Date = New Date(0)
    ' Private TimeBetweenUpdates As Double = 1

    ' Public CCPrefixes(256) As String

    ' Public Firefly_running As Boolean = False
    Public ProgressSaved As Boolean = True

#Region "Security Report Counters"
    Public SkippedFiles_InUse As Integer = 0
    Public SkippedFiles_LargeSize As Integer = 0
    Public SkippedFiles_Permissions As Integer = 0
    Public SkippedFiles_Error As Integer = 0
    Public Skippedfiles_KnownType As Integer = 0

    Public SkippedFiles_UnknownType As Integer = 0
    Public UnrecognizedFileTypes As New System.Collections.ObjectModel.Collection(Of String)
    ' Public FilesWithMatches As Integer = 0
    Public MatchCounts As Hashtable

#End Region
    Public Function CheckForStopSignal() As Boolean
        If ThreadTracking.StopRequested Then
            Return True
        Else
            Return False
        End If
    End Function
    ' This class object holds data for all matches in a file.
    Public Class MatchesInFile
        Public PathToFile As String
        Public MatchedExpressions As Collection
        Public Reported As Boolean = False
        Private pMatchCount As Integer = 0
        Public FileName As String
        Public Fragment As String
        Public FirstMatchLocation As String = ""



        Public Sub New(ByVal path As String)
            PathToFile = path
            MatchedExpressions = New Collection
            FileName = path.Substring(path.LastIndexOf("\") + 1)
        End Sub

        Public Sub Append(ByVal m As MatchesInFile)

            ' If m has matches, copy them to this object.
            If m.HasMatches Then
                Dim i As Object
                For Each i In m.MatchedExpressions
                    Me.MatchedExpressions.Add(i)
                Next
            End If
            pMatchCount += m.MatchCount
            If Me.Fragment = "" Then
                Me.Fragment = m.Fragment
            End If

            ' If this object doesn't already have a path, copy the incoming one.
            If Me.PathToFile = "" Then
                Me.PathToFile = m.PathToFile
            End If
        End Sub
        Private Function RemoveDigits(ByVal aFrag As String) As String
            Dim ret As String
            ret = aFrag.Replace("1", "X")
            ret = ret.Replace("2", "X")
            ret = ret.Replace("3", "X")
            ret = ret.Replace("4", "X")
            ret = ret.Replace("5", "X")
            ret = ret.Replace("6", "X")
            ret = ret.Replace("7", "X")
            ret = ret.Replace("8", "X")
            ret = ret.Replace("9", "X")
            ret = ret.Replace("0", "X")
            Return ret
        End Function

        Private Function CleanFragment(ByVal aFrag As String) As String
            Try
                ' Return only the last three characters.
                Return RemoveDigits(aFrag.Substring(0, aFrag.Length - 3)) + aFrag.Substring(aFrag.Length - 3, 3)
            Catch ex As Exception
                LogError(New Exception("Error while cleaning up match fragment.", ex), True)
                Return "Error while cleaning match fragment. See application log for details."
            End Try
        End Function
        Public Sub AddMatch(ByVal regex As String, ByVal aFragment As String, Optional ByVal context As String = "")
            pMatchCount += 1
            ' If MatchedExpressions Is Nothing Then MatchedExpressions = New Collection
            If Not MatchedExpressions.Contains(regex) Then
                MatchedExpressions.Add(regex)
            End If

            Fragment = CleanFragment(aFragment) + context

        End Sub
        Public Function FirstMatch() As String
            If MatchedExpressions.Count > 0 Then
                Return MatchedExpressions.Item(1)
            Else
                Return ""
            End If
        End Function
        Public Function HasMatches() As Boolean
            If MatchedExpressions Is Nothing Then
                Return False
            Else
                Return (MatchedExpressions.Count > 0)
            End If
        End Function


        Public ReadOnly Property PathToFolder() As String
            Get
                Return Left(PathToFile, InStrRev(PathToFile, "\"))
            End Get
        End Property


        Public ReadOnly Property MatchCount() As Integer
            Get
                Return pMatchCount
            End Get
        End Property

    End Class

    ' Public Function GetMatches(ByVal to_match As String, ByRef hittable As Hashtable, ByRef frag As String) As Boolean
    Public Sub GetMatches(ByVal selection As String, ByRef matches As MatchesInFile, Optional ByVal context As String = "")
        Dim i As Integer
        Dim match As Match
        Dim matchText As String
        ' Dim matches As MatchesInFile = New MatchesInFile("")
        Try
            selection = " " + selection + " "

            ' 0 = One set of regexs.
            ' 1 = The other set of regexes.
            ' No, I don't like this design, but it's less troubling than other code that I'm updating right now.
            For i = 0 To 1
                Dim curRE As RegexEntry
                If Not RE_head_array(i) Is Nothing Then
                    curRE = RE_head_array(i).Head

                    While Not (curRE Is Nothing)
                        Dim sel As String = selection
                        ' Try to match the object version.
                        match = curRE.Regex.Match(sel)

                        While match.Success
                            ' Run the validator.
                            matchText = match.Value.Substring(1, match.Value.Length - 2)
                            If ValidateMatch(curRE, matchText) Then
                                ' matches.AddMatch(curRE.RegName, matchText, selection.Substring(match.Index - 5, match.Index))
                                matches.AddMatch(curRE.RegName, matchText, context)
                            End If
                            ' Next Match
                            ' Leave one character (the trailing non-digit) of our previous match. Scan from there.
                            sel = sel.Substring(match.Index + match.Value.Length - 1)
                            'match = match.NextMatch
                            match = curRE.Regex.Match(sel)
                        End While

                        ' Next Regular Expression
                        curRE = curRE.NextItem
                    End While
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ' Public Sub ReportMatch(ByVal filepath As String, ByVal regex As String, ByVal frag As String)
    Public Sub ReportMatch(ByVal matches As MatchesInFile)
        'If FireflyReportOptions And ReportOptions.NoSensitiveData Then
        '    frag = ""
        'End If

        ' This only works if each file is only reported once.
        '    But that is the preferred behavior.
        If ArchiveDepth > 0 Then Return

        Try
            My.Settings.FilesWithMatches += 1
            If MatchCounts.ContainsKey(matches.FirstMatch) Then
                MatchCounts(matches.FirstMatch) += 1
            Else
                MatchCounts(matches.FirstMatch) = 1
            End If

            Dim filePath As String = matches.PathToFile

            WriteToReport(matches)

            If (FireflyReportOptions And ReportOptions.WriteToEventLog) Then
                send_Evt_log(filePath.ToString)
            End If
            ' other log methods below
            If (FireflyReportOptions And ReportOptions.WriteToSysLog) Then
                syslog_send(filePath)
            End If
        Catch ex As Exception
            LogError(New Exception("Error while reporting matches found.", ex), True)
        End Try

    End Sub




    Public Sub get_ads_parts(ByVal pPath As String, ByRef basePath As String, ByRef dStream As String)
        Dim nStream As String
        ' Dim adStream As String
        Dim eStream() As String
        Dim adsPath() As String
        Dim lPath As String

        Try
            adsPath = pPath.Split(System.IO.Path.DirectorySeparatorChar)
            lPath = adsPath((adsPath.Length - 1))
            eStream = lPath.Split(":")
            If eStream.GetLength(0) <> 2 Then
                ' nested ADS?  Abort!
                basePath = ""
                dStream = ""
                Return
            End If
            nStream = eStream(0)
            dStream = eStream(1)
            basePath = pPath.TrimEnd(eStream(1).ToCharArray)
            basePath = basePath.TrimEnd(":")
            Debug.WriteLine("base path: " + basePath)
            '            basePath = fInfo.Directory.ToString + "\" + nStream
        Catch ex As Exception
            basePath = ""
            dStream = ""
            Return
        End Try

    End Sub
    Public Function get_date(ByVal mydate As Date) As String
        ' here's the format we want:
        ' Mon Day YYYY HH:MM:SS
        Dim retDate As String = ""

        Select Case mydate.Month
            Case 1
                retDate += "Jan "
            Case 2
                retDate += "Feb "
            Case 3
                retDate += "Mar "
            Case 4
                retDate += "Apr "
            Case 5
                retDate += "May "
            Case 6
                retDate += "Jun "
            Case 7
                retDate += "Jul "
            Case 8
                retDate += "Aug "
            Case 9
                retDate += "Sep "
            Case 10
                retDate += "Oct "
            Case 11
                retDate += "Nov "
            Case 12
                retDate += "Dec "
        End Select

        retDate += mydate.Day.ToString + " " + mydate.Year.ToString + " "

        If mydate.Hour <= 9 Then
            retDate += "0" + mydate.Hour.ToString
        Else
            retDate += mydate.Hour.ToString
        End If

        retDate += ":"

        If mydate.Minute <= 9 Then
            retDate += "0" + mydate.Minute.ToString
        Else
            retDate += mydate.Minute.ToString
        End If

        retDate += ":"

        If mydate.Second <= 9 Then
            retDate += "0" + mydate.Second.ToString
        Else
            retDate += mydate.Second.ToString
        End If

        Return retDate

    End Function

    'Sub UpdateStatus(ByVal message As String, ByRef theMainForm As MainForm)
    '    ' Only update the status twice per second.
    '    ' Any more often would over-burdern the interface call stack.
    '    'If Now() >= DateAndTime.DateAdd(DateInterval.Second, TimeBetweenUpdates, LastStatusUpdate) Then
    '    'theMainForm.BeginInvoke(New MainForm.InvokeUpdateStatus(AddressOf theMainForm.UpdateProgressLabel), message)
    '    'theMainForm.LastStatusUpdate = Now()
    '    'End If
    '    theMainForm.UpdateInterfaceMessage(message, MainForm.StatusMedia.Status)
    'End Sub

    Public Sub get_disk_properties(ByVal driveletter As String)
        driveletter = driveletter.TrimEnd("\")
        Debug.WriteLine("drive: " + driveletter.ToString)
        Dim disk As New Management.ManagementObject("Win32_LogicalDisk.DeviceID=""" + driveletter + """")

        HDD_BLOCKSIZE = disk.Properties.Item("BlockSize").Value
        If HDD_BLOCKSIZE = 0 Then
            HDD_BLOCKSIZE = 4096
        End If

        '        Debug.WriteLine("blocksize: " + HDD_BLOCKSIZE.ToString)
        HDD_FREESPACE = Convert.ToInt64(disk.Properties.Item("FreeSpace").Value)
        HDD_NUMBER_OF_BLOCKS = Convert.ToInt64(disk.Properties.Item("NumberOfBlocks").Value)
        DriveBlocksize.Item(driveletter) = HDD_BLOCKSIZE
        DriveFreespace.Item(driveletter) = HDD_FREESPACE

        Return
    End Sub
    Public Function get_disk_freespace(ByVal driveletter As String) As Int64

        Dim disk As New Management.ManagementObject("Win32_LogicalDisk.DeviceID=""" + driveletter + """")
        Dim freespace As Int64

        freespace = Convert.ToInt64(disk.Properties.Item("FreeSpace").Value)

        Return freespace

    End Function

    Public Sub send_Evt_log(ByVal message As String)
        Try
            EventLog.CreateEventSource(My.Application.Info.ProductName, My.Application.Info.ProductName + "Log")
        Catch ex As Exception
            Debug.WriteLine("event log exception: " + ex.ToString)
        End Try

        Dim eLog As New System.Diagnostics.EventLog(My.Application.Info.ProductName + "Log", Environment.MachineName, My.Application.Info.ProductName)

        Try
            eLog.WriteEntry(message, EventLogEntryType.Information)
            eLog.Dispose()
        Catch ex As Exception
            Debug.WriteLine("send to event log exception: " + ex.ToString)
        End Try

        Return
    End Sub

    'Public Sub prep_regexes()
    '    ' we'll add system regexes as requested

    '    RE_Start.Clear()

    '    If (FireflyRegexOptions And LRegexOptions.RegexAMEX) Then
    '        Debug.WriteLine("Adding default AMEX regex")
    '        RE_Start.Add(reAMEX, "AMEX", "AMEX", Validator.CCN, ValidatorTypes.CreditCardPrefixes)
    '    End If

    '    If (FireflyRegexOptions And LRegexOptions.RegexAMEX_b) Then
    '        Debug.WriteLine("Adding default AMEX_b regex")
    '        RE_Start.Add(reAMEX_break, "AMEX_b", "AMEX_b", Validator.CCN, ValidatorTypes.CreditCardPrefixes)
    '    End If

    '    If (FireflyRegexOptions And LRegexOptions.RegexSIN333) Then
    '        Debug.WriteLine("adding default SIN regex")
    '        RE_Start.Add(reSIN333, "SIN333", "SIN333", Validator.CCN, ValidatorTypes.CanadianSIN)
    '    End If

    '    If (FireflyRegexOptions And LRegexOptions.RegexSIN333_b) Then
    '        Debug.WriteLine("Adding default SIN_b regex")
    '        RE_Start.Add(reSIN333_break, "SIN333_b", "SIN333_b", Validator.CCN, ValidatorTypes.CanadianSIN)
    '    End If

    '    If (FireflyRegexOptions And LRegexOptions.RegexSIN9) Then
    '        Debug.WriteLine("Adding default SIN9 regex")
    '        RE_Start.Add(reSSN9, "SIN9", "SIN9", Validator.CCN, ValidatorTypes.CanadianSIN)
    '    End If

    '    If (FireflyRegexOptions And LRegexOptions.RegexSIN9_b) Then
    '        Debug.WriteLine("Adding default SIN9_b regex")
    '        RE_Start.Add(reSSN9_break, "SIN9_b", "SIN9_b", Validator.CCN, ValidatorTypes.CanadianSIN)
    '    End If

    '    If (FireflyRegexOptions And LRegexOptions.RegexSSN324) Then
    '        Debug.WriteLine("Adding default SSN324 regex")
    '        RE_Start.Add(reSSN324, "SSN324", "SSN324", Validator.SSN, ValidatorTypes.US_SSN)
    '    End If

    '    If (FireflyRegexOptions And LRegexOptions.RegexSSN324_b) Then
    '        Debug.WriteLine("Adding default SSN324_b regex")
    '        RE_Start.Add(reSSN324_break, "SSN324_b", "SSN324_b", Validator.SSN, ValidatorTypes.US_SSN)
    '    End If

    '    If (FireflyRegexOptions And LRegexOptions.RegexSSN9) Then
    '        Debug.WriteLine("Adding default SSN9 regex")
    '        RE_Start.Add(reSSN9, "SSN9", "SSN9", Validator.SSN, ValidatorTypes.US_SSN)
    '    End If

    '    If (FireflyRegexOptions And LRegexOptions.RegexSSN9_b) Then
    '        Debug.WriteLine("Adding default SSN9_b regex")
    '        RE_Start.Add(reSSN9_break, "SSN9_b", "SSN9_b", Validator.SSN, ValidatorTypes.US_SSN)
    '    End If

    '    If (FireflyRegexOptions And LRegexOptions.RegexVMCD) Then
    '        Debug.WriteLine("adding default VMCD regex")
    '        RE_Start.Add(reVMCD, "VMCD", "VMCD", Validator.CCN, ValidatorTypes.CreditCardPrefixes)
    '    End If

    '    If (FireflyRegexOptions And LRegexOptions.RegexVMCD_b) Then
    '        Debug.WriteLine("Adding default VMCD_b regex")
    '        RE_Start.Add(reVMCD_break, "VMCD_b", "VMCD_b", Validator.CCN, ValidatorTypes.CreditCardPrefixes)
    '    End If

    '    If (FireflyRegexOptions And LRegexOptions.RegexNINO) Then
    '        RE_Start.Add(reNINO, "NINO", "NINO", Validator.None, ValidatorTypes.cleared)
    '    End If

    '    Return
    'End Sub

    Public Function gen_hit_string(ByVal hit As String, ByVal hititems As Hashtable) As String
        Dim i As Integer

        If (FireflyRunOptions And RunOptions.FastMatch) Then
            hit = hititems.Item("0")
        Else
            ' we use the hash table to keep unique keys;
            ' no more logs like "SSNSSNSSNSSN"
            ' but, this is the only way to get the results back in the order we added
            ' them to the hash.
            ' sites with more than 500 custom-regexes are either very patient or 
            ' doing something very wrong.
            For i = 0 To 512
                If Not hititems.Contains(i.ToString) Then
                    Exit For
                End If
                hit += hititems.Item(i.ToString) + "|"
            Next
        End If
        hit = hit.TrimEnd("|")
        Return hit
    End Function
    Public Function prep_contenttype(ByVal type As String) As String
        ' we'll convert the input content type into a regex by escaping anything
        ' the regex parser would interpret as being meaningful
        Dim newtype As String = ""
        Dim i As Integer

        For i = 0 To type.Length - 1
            Select Case type.Substring(0, i)
                Case "/"
                    newtype += "\/"
                Case "."
                    newtype += "\."
                Case "+"
                    newtype += "\+"
                Case Else
                    newtype += type.Substring(0, i)
            End Select
        Next

        Return newtype

    End Function
    Public Sub populate_robots(ByVal URL As String)
        Dim roborequest As HttpWebRequest
        Dim roboresponse As HttpWebResponse
        Dim roboDenies As New ArrayList
        Dim req As String
        Dim thisURI As New Uri(URL)
        Dim UA_found As Boolean = False
        ' try-catch block to pull and parse robots.txt
        ' if there isn't one, we'll push an empty array into RobotsDone so we don't
        ' pass this way again
        ' we'll need to respect any sort of User-Agent string the user has customized
        ' without that, we're looking for a wildcard
        Try
            If URL.StartsWith("http:") Then
                req = "http://" + thisURI.Host.ToString + "/robots.txt"
                Debug.WriteLine("URL for robots.txt: " + req)
            ElseIf URL.StartsWith("https:") Then
                req = "https://" + thisURI.Host.ToString + "/robots.txt"
                Debug.WriteLine("URL for robots.txt: " + req)
            Else
                Throw New Exception("Unknown internet protocol encountered.")
            End If

            roborequest = DirectCast(WebRequest.Create(req), HttpWebRequest)

            If UserAgent <> String.Empty Then
                roborequest.UserAgent = UserAgent
            End If
            roboresponse = DirectCast(roborequest.GetResponse(), HttpWebResponse)
            ' check our status codes; anything that starts with 4 or 5 means we continue
            ' otherwise, we pull and parse
            Dim readRobots As StreamReader = New StreamReader(roboresponse.GetResponseStream)
            Dim roboLine As String
            Dim roboChunks As String()
            Dim AddDisallows As Boolean = False
            While readRobots.Peek <> -1
                roboLine = readRobots.ReadLine
                roboLine.TrimEnd(" ")
                ' for now, we can't handle comments *within* a line either
                If roboLine.IndexOf("#") >= 0 Then
                    roboLine = ""
                End If
                If UserAgent <> String.Empty Then
                    If roboLine.ToLower.IndexOf("user-agent") = 0 Then
                        ' chunk it up
                        UA_found = True
                        roboChunks = roboLine.Split(" ")
                        If roboChunks(roboChunks.Length - 1) = "*" Then
                            Debug.WriteLine("wildcard User-agent")
                            ' wildcard, see if roboDenies is empty
                            If Not roboDenies.Count > 0 Then
                                AddDisallows = True
                            End If
                            ' End If
                            ' if there's a better match, clear roboDenies and set AddDisallows to be true
                        ElseIf UserAgent.IndexOf(roboChunks(roboChunks.Length - 1)) >= 0 Then
                            Debug.WriteLine("User-agent specific to us")
                            ' more specific match
                            roboDenies.Clear()
                            AddDisallows = True
                        Else
                            AddDisallows = False
                        End If
                    End If
                Else
                    ' we don't have a specific user-agent, so we'll ignore anything but wildcards 
                    ' in robots.txt
                    If roboLine.ToLower.IndexOf("user-agent") = 0 Then
                        UA_found = True
                        roboChunks = roboLine.Split(" ")
                        If roboChunks(roboChunks.Length - 1) = "*" Then
                            AddDisallows = True
                        End If
                    End If
                    '                            AddDisallows = True
                End If
                Debug.WriteLine("Line: (" + roboLine + ") index: " + roboLine.ToLower.IndexOf("disallow").ToString)
                If roboLine.ToLower.IndexOf("disallow") = 0 Then
                    If Not UA_found Then
                        AddDisallows = True
                        UA_found = True
                    End If
                    If AddDisallows Then
                        ' parse and push into the roboDenies array
                        Debug.WriteLine("Disallow found: " + roboLine)
                        roboChunks = roboLine.Split(" ")
                        If roboChunks(roboChunks.Length - 1) <> String.Empty Then
                            Debug.WriteLine("adding robot rule: " + roboChunks(roboChunks.Length - 1))
                            roboDenies.Add(roboChunks(roboChunks.Length - 1))
                        End If
                    End If
                End If
            End While
            ' we'll copy the roboDenies array into the robotSkipList array, then store it.
            '      robotSkipList = roboDenies
            RobotsDone.Item(thisURI.Host.ToLower) = roboDenies
            roboresponse.Close()
        Catch ex As Exception
            ' Debug.WriteLine("error pulling robots.txt: " + ex.ToString)
            ' OK, cool.  record that fact in robots.txt
            roboDenies.Clear()
            RobotsDone.Item(thisURI.Host.ToLower) = roboDenies
            LogError(New Exception("An error occurred while processing robots.txt.", ex))
        End Try

        Return
    End Sub
    Public Function check_robots(ByVal URL As String) As Boolean
        ' true: something in robots.txt for the host in URL hits the current URL
        ' false: it does not
        Dim robotSkipList As New ArrayList
        Dim i As Integer
        Dim thisURI As New Uri(URL)

        robotSkipList = RobotsDone.Item(thisURI.Host.ToLower)
        ' iterate over the skiplist, assuming each piece is a Disallow
        For i = 0 To robotSkipList.Count - 1
            Debug.WriteLine("Skip item " + i.ToString + ":" + robotSkipList.Item(i))
            If robotSkipList.Item(i) = "/" Then
                ' wildcard deny
                ' Debug.WriteLine("robots denies access to all")
                ' WriteToLog("robots.txt denies access to all content.")
                Return True
            End If
            If URL.IndexOf(robotSkipList.Item(i)) >= 0 Then
                ' it's a match, we're done
                ' Debug.WriteLine("robots.txt says to avoid: " + URL)
                Return True
            End If
        Next

        Return False

    End Function
    Public Function getHrefs(ByVal pagefrag As String, ByVal currhost As String, ByVal currbase As String, ByVal currlist As ArrayList) As ArrayList
        Dim linkslist As New ArrayList
        '  Dim m As Match
        ' Dim re As Regex
        Dim regstr As String = "<a[^>]*href\s*=\s*""?(?<HRef>[^"">\s]*)""?[^>]*>"
        Dim mc As MatchCollection
        Dim i As Integer = 0
        Dim tmpStr As String

        ' push the current list into the one we'll return
        linkslist.AddRange(currlist)

        mc = Regex.Matches(pagefrag, regstr, RegexOptions.IgnoreCase Or RegexOptions.Singleline)

        For i = 0 To mc.Count - 1
            Dim strHRef As String = mc.Item(i).Groups("HRef").Value
            tmpStr = ""
            strHRef.Trim()
            If strHRef <> String.Empty Then
                ' relative URLs get padded with the current base URL
                ' if the HRef starts with the current host, prepend "http://"
                ' if the HRef starts with "http:// or "https://", just add it
                ' otherwise, prepend the currbase
                If strHRef.Substring(0, 1) <> "#" And strHRef.Substring(0, 1) <> "/" And strHRef.Substring(0, 1) <> "?" Then
                    If strHRef.StartsWith("http:") Or strHRef.StartsWith("https:") Then
                        '                       linkslist.Add(strHRef)
                        tmpStr = strHRef
                    ElseIf strHRef.StartsWith(currhost) Then
                        tmpStr = "http://" + strHRef
                        '                        linkslist.Add(strHRef)
                    ElseIf strHRef.StartsWith("file://") Or strHRef.StartsWith("ftp://") Then
                        tmpStr = ""
                    Else
                        If currbase.EndsWith("/") Then
                            tmpStr = currbase + strHRef
                        Else
                            tmpStr = currbase + "/" + strHRef
                        End If
                        '                        tmpStr = currbase + "/" + strHRef
                    End If
                    If tmpStr <> String.Empty Then
                        If Not URLLoopDetect.Contains(tmpStr) Then
                            Debug.WriteLine("Added: " + tmpStr)
                            linkslist.Add(escape_hrefs(tmpStr))
                            URLLoopDetect.Add(tmpStr, "1")
                        Else
                            Debug.WriteLine("SEEN THIS URL before:" + tmpStr)
                        End If
                    End If
                End If
            End If
        Next

        Return linkslist

    End Function
    Public Function escape_hrefs(ByVal inRef As String) As String
        Dim fixedRef As String

        ' an opportunity to fix various things about URLs that shouldn't be there

        fixedRef = inRef.Replace("/#", "/%23")

        Return fixedRef
    End Function

    Public Sub set_width(ByVal width As Integer, ByRef startpos As Integer, ByRef runlen As Integer)
        ' we'll take these in and decide how best to fit up to 128 chars of context around the match
        ' Debug.WriteLine("starting with: " + startpos.ToString)
        ' Debug.WriteLine("length: " + runlen.ToString)
        If (startpos - (MatchSize / 2)) < 0 Then
            startpos = 0
        Else
            startpos -= (MatchSize / 2)
        End If
        ' run length
        If (startpos + MatchSize) > width Then
            runlen = (width - startpos) - 1
        Else
            runlen = MatchSize
        End If
        Return
    End Sub

End Module