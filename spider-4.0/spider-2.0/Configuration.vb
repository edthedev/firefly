' Imports Microsoft.Win32 ' Registry

Module Configuration
#Region "Enumerations"
    Public Enum LogMask
        cleared = 0
        attrPath = 1
        attrType = 2
        attrApp = 4
        attrSize = 8
        attrCtime = 16
        attrAtime = 32
        attrMtime = 64
        attrRegex = 128
        attrMatch = 256
        attrHostinfo = 512
        attrTotalMatches = 1024
        attrScore = 2048
        attrHash = 4096
        attrAllHits = 8192
        attrLocation = 16384
    End Enum
    ' Public My.Settings.FireflyLogAttributes As LogMask = LogMask.attrPath
    Public Enum RunPriority
        cleared = 0
        Low = 1
        Normal = 2
        High = 3
        Highest = 4
    End Enum
    ' Public My.Settings.FireflyRunPriority As RunPriority = RunPriority.Normal
    Public Enum DiskModeOptions
        cleared = 0
        '         FastMatch = 1
        NoAtime = 1
        CheckPoint = 2
        Incremental = 4
        Recurse = 8
        AllLocalDrives = 16
        TypesAll = 32
        SaveAndResumeSearch = 64
        ScanUnknownFileTypes = 128
        ScanFileList = 256
        ScanEverything = 512

    End Enum
    Public FireflyScanDiskOptions As DiskModeOptions = DiskModeOptions.cleared
    Public Enum WebModeOptions
        cleared = 0
        RespectRobots = 1
        ChangeUA = 2
        Recurse = 4
        JumpDomain = 8
        SupplyBasicAuth = 16
    End Enum
    Public FireflyWebModeOptions As WebModeOptions = WebModeOptions.cleared
    Public Enum HiddenModeOptions
        cleared = 0
        ScanADS = 1
        ScanSlack = 2
        ScanUnalloc = 4
        PullUnalloctoFile = 8
        WipeUnallocMatchingBlocks = 16
        SlackOnly = 32
        BothSlackandFile = 64
        NoAtime = 128
    End Enum
    Public FireflyHiddenModeOptions As HiddenModeOptions = HiddenModeOptions.cleared

    ' Application Log Options
    Public Enum LogOptions
        Cleared = 0

        ' Periodically print a progress message in the application log.
        LogProgress = 1

    End Enum
    Public FireflyLogOptions As LogOptions = LogOptions.Cleared

    ' SSN Reporting Options
    Public Enum ReportOptions
        cleared = 0

        ' Write the report to:
        TextFormat = 1
        WriteToEventLog = 2
        WriteToSysLog = 4

        ' Report Formats
        EncryptReport = 8
        CsvReport = 16
        HtmlFormat = 32

        ' File Writing Options
        AppendLog = 64
        WipeLogBeforeUse = 128

        ' Additional option
        NoSensitiveData = 256

        SaveSkippedFilesList = 512
        SendSecurityReport = 1024



    End Enum
    Public FireflyReportOptions As ReportOptions = ReportOptions.cleared

    Public Enum LRegexOptions
        cleared = 0
        ' vanilla regexes
        RegexSSN324 = 1
        RegexVMCD = 2
        RegexAMEX = 4
        RegexSSN9 = 8
        RegexSIN333 = 16
        RegexSIN9 = 32
        ' regexes bracketed with \b below
        RegexSSN324_b = 64
        RegexVMCD_b = 128
        RegexAMEX_b = 256
        RegexSSN9_b = 512
        RegexSIN333_b = 1024
        RegexSIN9_b = 2048
        RegexNINO = 4096
        ' Custom regexes follow
        CustomRegexes = 32768
    End Enum
    Public FireflyRegexOptions As LRegexOptions = LRegexOptions.cleared
    Public Enum RunOptions
        cleared = 0
        Renice = 1
        LaunchLogViewer = 2
        Minimize = 4
        ExitWhenDone = 8
        ' NoAtime = 16
        FastMatch = 32
        Restore = 64
        ' TypesAll = 128;
        Unattended = 256
        Debug = 512
        ' NoPrompts = 1024
    End Enum
    Public FireflyRunOptions As RunOptions = RunOptions.cleared
    Public Enum RunMode
        cleared = 0
        Disk = 1
        Web = 2
        Hidden = 4
    End Enum
    Public FireflyRunMode As RunMode = RunMode.Disk

#End Region

    Public REGISTRY_HOME As String = "Software\Security\" + My.Application.Info.ProductName

    ' Web links
    Public Const Firefly_HELP_URL As String = "http://www.cites.uiuc.edu/ssnprogram/firefly/windows_ie.html#running"
    Public Const Firefly_LICENSE_URL As String = "http://www.fsf.org/licensing/licenses/info/GPLv2.html"

    ' White list file extensions that are safe to skip scanning.
    Public Const SkipExtsMedia As String = "avi bmp eps gif ico icns icon jpeg jpg m4a mda mov mp3 mpeg mpg png ppt psd swf tif tiff vsd vss wav wmb wmdb wmf wmv"
    Public Const SkipExtsSystem As String = "application bat bkf cab cnf config dll dtd fon hiv hlp idx inf ini jar laccdb ldb lib lng lnk msc msf msi mst pdb p12 pfx properties reg rsrc scm skin sms swp sys tcl thmx tlb tmd toc ttf url vst xsd xsl"
    Public Const SkipExtsBinary As String = "bin chm exe iso vmdk"
    Public Const SkipExtsTroublesome As String = "dat htm html"
    Public Const SkipExtsCode As String = "asax ascx asmx asp aspx cat cgi class cpp cs csproj css deploy frm h js manifest mpr ocx php pl projdata ps1 py pyc pyd pyw rc resources resx settings sln suo svn-base svn\entires svn\format vb vbproj vbs vcproj vdproj vsmacros vssettings webinfo wixobj wixproj wxs"
    Public Const WishWeCouldScan As String = ""

    ' This is authoritative. It trumps the lists above.
    Public Const ScanExtsString As String = "accdb accdr adp bz2 bzip csv doc docx gz gzip log mbx mdb odf odp ods odt pdf qif rtf tar tgz txt xls xlsx xml xlt zip"
    '                                        accdb accdr adp bz2 bzip csv doc docx gz gzip log mbx mdb odf odp ods odt pdf qif rtf tar tgz txt xls xlsx xml xlt zip



    'Public Function Get_type(ByVal Extension As String) As String
    '    ' shamelessly poached from the net
    '    Dim regKey As RegistryKey

    '    regKey = Registry.ClassesRoot.CreateSubKey("\." + Extension.ToLower)

    '    Try
    '        Get_type = regKey.GetValue("Content Type", "")
    '    Catch
    '        Return ("unavailable")
    '    End Try

    '    regKey.Close()

    '    If Get_type = String.Empty Then
    '        Return ("unavailable")
    '    End If

    '    Return Get_type

    'End Function

    'Public Function read_config() As Boolean

    '    Dim i As Integer
    '    Dim sString As String

    '    Try

    '        KeepExts.Clear()
    '        SkipExts.Clear()

    '        ' typeslist == extensions to keep

    '        ' sString = regKey.GetValue("TypesList", "")
    '        sString = My.Settings.ScanExts

    '        Dim parts() As String

    '        parts = sString.Split(" ")

    '        For i = 0 To parts.Length - 1
    '            If Not KeepExts.Contains(parts(i).ToUpper) Then
    '                KeepExts.Add(parts(i).ToUpper, "1")
    '            End If
    '        Next

    '        ' sString = regKey.GetValue("SkipList", "")
    '        sString = My.Settings.SkipExts
    '        parts = sString.Split(" ")

    '        For i = 0 To parts.Length - 1
    '            ' Debug.WriteLine("adding skip extension: " + parts(i))
    '            If parts(i) <> String.Empty Then
    '                SkipExts.Add(parts(i).ToUpper, "1")
    '            End If
    '        Next

    '    Catch ex As Exception
    '        ' LogError(New Exception(ErrorReadingSettingsMessage, ex), True)
    '        LogError(New Exception("Unable to load the saved settings.", ex), True)
    '        Return False
    '    Finally
    '        ' regKey.Close()
    '    End Try

    '    ' convert the path globs to regexes
    '    ' paths2globs(SkipPaths, SkipPaths2Globs)
    '    ' SkipPaths2Globs = SkipPaths

    '    ' convert the MIME type globs to regexes
    '    ' paths2globs(SkipContentTypes, SkipContentRegex)
    '    SkipContentRegex = SkipContentTypes

    '    NewConfig = False

    '    Return True

    'End Function

    Public Function save_config() As Boolean
        ' Dim sString As String

        'Try

        '    'My.Settings.ScanExts = ""
        '    'My.Settings.SkipExts = ""

        '    'If Not (FireflyScanDiskOptions And DiskModeOptions.TypesAll) Then
        '    '    sString = ""
        '    '    Dim en As IDictionaryEnumerator = KeepExts.GetEnumerator
        '    '    '            For i = 0 To TypeExts.GetLength(0)
        '    '    '            sString += TypeExts(i) + " "
        '    '    '            Next
        '    '    While en.MoveNext
        '    '        sString += en.Key + " "
        '    '    End While
        '    '    ' regKey.SetValue("TypesList", sString.ToString)
        '    '    My.Settings.ScanExts = sString.ToString

        '    '    If SkipExts.Count > 0 Then
        '    '        sString = ""
        '    '        Dim sen As IDictionaryEnumerator = SkipExts.GetEnumerator
        '    '        While sen.MoveNext
        '    '            sString += sen.Key + " "
        '    '        End While
        '    '        ' regKey.SetValue("SkipList", sString.ToString)
        '    '        My.Settings.SkipExts = sString.ToString
        '    '    End If
        '    'End If

        '    'If NewConfig Then
        '    '    regKey.CreateSubKey("SkipPaths")
        '    'End If

        'Catch ex As Exception
        '    LogError(New Exception("Unable to save the application settings to the registry.", ex))
        '    Return False
        'Finally

        'End Try

        Return True

    End Function

    Public Sub EnforceFireflySettings()
        ' These settings are not optional.

        ' Default Disk Scanning Options:
        FireflyScanDiskOptions = FireflyScanDiskOptions Or DiskModeOptions.Recurse
        FireflyScanDiskOptions = FireflyScanDiskOptions Or DiskModeOptions.TypesAll
        FireflyScanDiskOptions = FireflyScanDiskOptions Or DiskModeOptions.AllLocalDrives
        FireflyScanDiskOptions = FireflyScanDiskOptions Or DiskModeOptions.SaveAndResumeSearch

        ' regex options
        FireflyRegexOptions = FireflyRegexOptions Or LRegexOptions.RegexSSN324
        FireflyRegexOptions = FireflyRegexOptions Or LRegexOptions.RegexSSN9

        ' run mode
        FireflyRunMode = RunMode.Disk

        ' report options
        ' FireflyReportOptions = FireflyReportOptions Or ReportOptions.TextFormat
        FireflyReportOptions = FireflyReportOptions Or ReportOptions.CsvReport
        FireflyReportOptions = FireflyReportOptions Or ReportOptions.HtmlFormat

        FireflyReportOptions = FireflyReportOptions Or ReportOptions.NoSensitiveData
        FireflyReportOptions = FireflyReportOptions Or ReportOptions.SaveSkippedFilesList

        FireflyReportOptions = FireflyReportOptions Or ReportOptions.SendSecurityReport

        ' run options
        FireflyRunOptions = FireflyRunOptions Or RunOptions.FastMatch
        FireflyRunOptions = FireflyRunOptions Or RunOptions.LaunchLogViewer
        FireflyRunOptions = FireflyRunOptions Or RunOptions.Renice

        ' FireflyRunOptions = FireflyRunOptions Or RunOptions.Debug

        ' Choose where to place the user reports.
        ReportDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
        ' Log and Report directories

        ' This directory path is ugly and useless: LogDirectory = Application.UserAppDataPath
        LogDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\Firefly Files"

        RE_Start.Clear()
        'RE_Start.Add(reSSN324, "SSN324", "SSN324", Validator.SSN, ValidatorTypes.US_SSN)
        'RE_Start.Add(reSSN9, "SSN9", "SSN9", Validator.SSN, ValidatorTypes.US_SSN)

        ' All regexes used by Firefly must start and end with \D (non-digit character).
        '    Using this reduces false positives; and failing to use it will require modification of the GetMatches and Validation routines.

        Dim ssn9_nonuin As New System.Text.RegularExpressions.Regex("\D[12345780][1234890]\d\d\d\d\d\d\d\D")
        RE_Start.Add(ssn9_nonuin, "SSN9_NONUID", "Nine Digit Number (Possible SSN)", Validator.SSN, ValidatorTypes.US_SSN)

        Dim ssn324_nonuin As New System.Text.RegularExpressions.Regex("\D[12345780][1234890]\d[\s\-]\d\d[\s\-]\d{4}\D")
        RE_Start.Add(ssn324_nonuin, "SSN3-2-4_NONUID", "Formatted SSN", Validator.SSN, ValidatorTypes.US_SSN)

        ' [12345780][1234890][0-9][\s\-]{0,1}[0-9]{2}[\s\-]{0,1}[0-9]{4}

        'Dim ssn3_2_4_nonuin As New System.Text.RegularExpressions.Regex("\D[12345780][1234890]\d\s\d\d\s\d\d\d\d\D")
        'RE_Start.Add(ssn324_nonuin, "SSN3_2_4_NONUID", "Formatted SSN", Validator.SSN, ValidatorTypes.US_SSN)

        'RE_Start.Add(reVMCD, "VMCD", "Visa or Mastercard Number", Validator.CCN, ValidatorTypes.CreditCardPrefixes)
        'RE_Start.Add(reAMEX, "AMEX", "American Express Card Number", Validator.CCN, ValidatorTypes.CreditCardPrefixes)

        Dim ccnVisaOrMasterCard As New System.Text.RegularExpressions.Regex("\D(6011|5[1-5]\d{2}|4\d{3}|3\d{3})[\s\-]{0,1}\d{4}[\s\-]{0,1}\d{4}[\s\-]{0,1}\d{4}\D")
        Dim ccnAmericanExpress As New System.Text.RegularExpressions.Regex("\D(3[4|7]\d{2}|2014|2149|2131|1800)[\s\-]{0,1}\d{4}[\s\-]{0,1}\d{4}[\s\-]{0,1}\d{3}\D")
        ' Dim anyStinkinCCN As New System.Text.RegularExpressions.Regex("\d{4}[\s\-]{0,1}\d{4}[\s\-]{0,1}\d{4}[\s\-]{0,1}\d{4}")

        RE_Start.Add(ccnVisaOrMasterCard, "VMCD", "Visa or Mastercard Number", Validator.CCN, ValidatorTypes.CreditCardPrefixes)
        RE_Start.Add(ccnAmericanExpress, "AMEX", "American Express Card Number", Validator.CCN, ValidatorTypes.CreditCardPrefixes)
        ' RE_Start.Add(anyStinkinCCN, "CCN", "A Credit Card Number", Validator.CCN, ValidatorTypes.CreditCardPrefixes)

        RE_head_array(0) = RE_Start
        RE_head_array(1) = RE_Custom_Start

        ' delegate content types
        DelegateContentTypes.Clear()
        DelegateContentTypes.Add("application/pdf", "1")
        DelegateContentTypes.Add("application/zip", "1")
        DelegateContentTypes.Add("application/vnd.ms-excel", "1")
        DelegateContentTypes.Add("application/x-gzip", "1")
        DelegateContentTypes.Add("application/vnd.openxmlformats", "1")
        DelegateContentTypes.Add("application/x-bz2", "1")
        DelegateContentTypes.Add("application/x-bzip2", "1")
        ' add more, as more type scanners become available

        ' clear the SkipContentRegex/SkipContentTypes hash table
        SkipContentRegex.Clear()
        SkipContentTypes.Clear()

        ' white list content-types we simply don't care about
        SkipContentTypes.Add("audio/*", "1")
        SkipContentTypes.Add("image/*", "1")
        SkipContentTypes.Add("video/*", "1")

        ' Skip/Scan extensions
        Dim i As Integer
        Dim extarray() As String

        ' Scan known file types
        ScanExts.Clear()
        extarray = ScanExtsString.Split(" ")
        For i = 0 To extarray.Length - 1
            ScanExts.Add(extarray(i).ToLower, "1")
        Next

      
        AddSkipPaths()

        ' White list some directories that should not ever be scanned.



        ' Files to skip. These files are known to be innocent.
        SkipFileNames.Clear()
        SkipFileNames.Add("\\Outlook\\Junk Senders\.txt", "1") ' The outlook junk senders list.
        ' SkipFileNames.Add("cookies\.txt", "1")  ' Internet cookies
        SkipFileNames.Add("(rss|feeds).*\.(xml|rss)", "1")  ' RSS Feeds
        SkipFileNames.Add("FireflyReport\.(htm|csv|txt)", "1")  ' Firefly's own report.
        SkipFileNames.Add("Firefly SSN Finder\.log", "1")  ' Firefly's own log file.
        SkipFileNames.Add("\\Northwind*\.(accdb|mdb|sql)", "1")
        SkipFileNames.Add("winword\.doc", "1") ' This is a helper file for Word templates. It does bad things to Firefly.
        SkipFileNames.Add("winword2\.doc", "1") ' This is a helper file for Word templates. It does bad things to Firefly.
       SkipFileNames.Add("\:\\Program Files\\.*\.zip", "1") ' Zip files in the Program Files directory. Wastes lots of time, and doesn't contain user data.
        SkipFileNames.Add("\:\\Program Files\\.*\.log", "1") ' Log files in the program files directory.
        SkipFileNames.Add("\\license.*\.(htm|rtf|txt)", "1") ' Various license files from Mathematica, Java, etc.
        SkipFileNames.Add("\\.*EULA.*\.(htm|rtf|txt)", "1") ' Various End User License Agreement files
        SkipFileNames.Add("\\K95\\KeyMaps\\keycodes\.txt", "1") ' This is a file from the old U of I Direct application that many people will still have installed and deleting this file likely breaks Kermit 95.

        ' At the moment, we skip DAT files anyways.
        ' SkipPaths.Add("xpti\.dat", "1")   ' Common Mozilla file containing long numbers.
        ' SkipPaths.Add("compreg\.dat", "1")   ' Common Mozilla file containing long numbers.

    End Sub

    Public Sub AddSkipExts()
        Dim i As Integer
        Dim extarray() As String

        ' Skip media files
        extarray = SkipExtsMedia.Split(" ")
        For i = 0 To extarray.Length - 1
            SkipExts.Add(extarray(i).ToLower, "1")
        Next
        ' Skip binary files
        extarray = SkipExtsBinary.Split(" ")
        For i = 0 To extarray.Length - 1
            SkipExts.Add(extarray(i).ToLower, "1")
        Next
        ' Skip development files
        extarray = SkipExtsCode.Split(" ")
        For i = 0 To extarray.Length - 1
            SkipExts.Add(extarray(i).ToLower, "1")
        Next
        ' Skip system files
        extarray = SkipExtsSystem.Split(" ")
        For i = 0 To extarray.Length - 1
            SkipExts.Add(extarray(i).ToLower, "1")
        Next
        ' Skip files that create a lot of false positives 
        extarray = SkipExtsTroublesome.Split(" ")
        For i = 0 To extarray.Length - 1
            SkipExts.Add(extarray(i).ToLower, "1")
        Next
    End Sub

    Private Sub AddSkipPaths()
        ' The strings should be regular expressions
        '  any path containing a match will be skipped

        ' System Folders 
        SkipPaths.Clear()
        SkipPaths.Add("\:\\WINDOWS", "1")
        SkipPaths.Add("\:\\\$", "1") ' Vista system directories.
        SkipPaths.Add("\:\\WIN32", "1")
        SkipPaths.Add("\:\\I386", "1")
        SkipPaths.Add("\:\\DELL", "1")
        SkipPaths.Add("\:\\System Volume Information", "1")
        SkipPaths.Add("\:\\drivers\\", "1") ' Don't scan drivers directories.
        SkipPaths.Add("\:\\system32\\inetsrv\\History\\", "1")
        SkipPaths.Add("\:\\Documents And Settings\\.*\\Cookies\\", "1")

        ' Application data
        SkipPaths.Add("\\QUARANTINE\\", "1") ' Antivirus Quarantine folder
        SkipPaths.Add("\\Cookies\\", "1")
        SkipPaths.Add("\:\\Program Files\\Adobe", "1") ' Adobe has a lot of sample files with false positives.
        SkipPaths.Add("\:\\Program Files\\.*\\Samples\\", "1") ' MS Office, Adobe, etc. Sample files.
        SkipPaths.Add("\:\\Program Files\\.*\\templates\\", "1") ' MS Office, Adobe, etc. template files.

        SkipPaths.Add("\\Firefly Files\\", "1")  ' Other Firefly files.

        SkipPaths.Add("Local Settings\\Temp", "1")
        SkipPaths.Add("\\src\\", "1") ' Don't scan source code directories.
        SkipPaths.Add("\\Copy of UPS\\", "1") ' For Bob Douglas' computer.
        SkipPaths.Add("\\UPS\\", "1") ' For Bob Douglas' computer.

        ' Common Applications with log files that slow Firefly and cause a lot of false positives.
        SkipPaths.Add("\\logs\\", "1")
        SkipPaths.Add("Ad\-Aware\\Logs", "1")
        SkipPaths.Add("Application Data\\Microsoft\\Outlook", "1")
        SkipPaths.Add("Application Data\\McAfee", "1")
        SkipPaths.Add("Microsof\.NET\\SDK", "1")

        SkipPaths.Add("\\Content\.IE5\\", "1")
        SkipPaths.Add("\\Temporary Internet Files\\", "1") ' Some Outlook attachments are stored here.
    End Sub


    Public Sub default_config()
        ' this is where we set all of our options back to defaults.
        ' we won't write to the registry.  This is simply for the benefit of the GUI and at load-time,
        ' prior to calling read_config

        ' step one, clear all our masks
        FireflyScanDiskOptions = DiskModeOptions.cleared
        FireflyWebModeOptions = WebModeOptions.cleared
        FireflyHiddenModeOptions = HiddenModeOptions.cleared
        FireflyRegexOptions = LRegexOptions.cleared
        FireflyReportOptions = ReportOptions.cleared
        FireflyRunOptions = RunOptions.cleared
        FireflyRunMode = RunMode.cleared

        ' clear our hashtables; we'll re-read those from the registry if we need them
        ' KeepExts.Clear()
        SkipExts.Clear()
        Area2Group.Clear()
        CCPrefixes.Clear()
        SkipContentRegex.Clear()
        DelegateContentTypes.Clear()

        ' Most settings aren't optional anymore.
        EnforceFireflySettings()

        ' run priority is still a user setting.
        My.Settings.FireflyRunPriority = RunPriority.Low

        ' Hidden mode options
        ' None

        ' now some specifics
        ' SSN_XML_PATH = SSN_XML_PATH_DEFAULT
        ' CCPREFIXES_XML_PATH = CCPREFIXES_XML_PATH_DEFAULT

        CheckPointPath = String.Empty

        ' LastScanDate = OUR_EPOCH.Date

        CheckPointFound = False

        'My.Settings.StartDir = "C:\"
        My.Settings.ScanDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal)

        MaxURLDepth = 10

        MaxContentLength = 10 ' value in MB

        DomainDepth = 3

        '  Priority = 0

        Done = 0

        Loghost = "localhost"

        Facility = "local0"

        ' ScanDepth = 0

        UnallocSearchDrive = "C:"

        WebModeStartURL = "http://localhost"

        LogFooter = " End of Log "

        UserAgent = APP_NAME

        BasicAuthUser = "none"
        BasicAuthPassword = "none"

        MaxArchiveSize = 200
        MinFreeGB = 2

        ' Start a new scan with the next run.
        '  (Delete our unprocessed files list.)
        My.Settings.ScanInProgress = False

        Return

    End Sub
    'Public Sub reset_diffs(ByVal toreset As ResetDiffs)
    '    ' we'll reset our checkpoint and incremental scan values here
    '    Dim regKey As RegistryKey

    '    If toreset And ResetDiffs.Checkpoint Then
    '        ' clobber the reg key that contains the file where we left off
    '        Try
    '            Registry.CurrentUser.DeleteSubKey("CheckPointPath")
    '        Catch
    '            Return
    '        End Try
    '    End If

    '    If toreset And ResetDiffs.Incremental Then
    '        ' reset the incremental timestamp back to the epoch
    '        LastScanDate = OUR_EPOCH
    '        Try
    '            regKey = Registry.CurrentUser.CreateSubKey(REGISTRY_HOME)
    '            regKey.SetValue("LastScanDate", LastScanDate.ToString)
    '            regKey.Close()
    '        Catch ex As Exception
    '            Return
    '        End Try
    '    End If
    '    Return
    'End Sub

End Module
