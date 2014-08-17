Imports Microsoft.Win32
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Windows.Forms.FormWindowState
Imports System.IO
Imports System.Text
Imports System.Net
Imports System.Reflection
Imports System.Net.Sockets
Imports System.Security.Cryptography
Imports System.Xml
Imports System.Data.OleDb
Imports System.Diagnostics.Process

Imports AlternateDataStream


Public Class Form1
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents file_menu As System.Windows.Forms.MenuItem
    Friend WithEvents view_log_menu As System.Windows.Forms.MenuItem
    Friend WithEvents exit_menu As System.Windows.Forms.MenuItem
    Friend WithEvents configure_menu As System.Windows.Forms.MenuItem
    Friend WithEvents settings_menu As System.Windows.Forms.MenuItem
    Friend WithEvents help_menu As System.Windows.Forms.MenuItem
    Friend WithEvents about_menu As System.Windows.Forms.MenuItem
    Friend WithEvents full_help As System.Windows.Forms.MenuItem
    Friend WithEvents run_button As System.Windows.Forms.Button
    Friend WithEvents exit_button As System.Windows.Forms.Button
    Friend WithEvents spider_progress As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.MainMenu1 = New System.Windows.Forms.MainMenu
        Me.file_menu = New System.Windows.Forms.MenuItem
        Me.view_log_menu = New System.Windows.Forms.MenuItem
        Me.exit_menu = New System.Windows.Forms.MenuItem
        Me.configure_menu = New System.Windows.Forms.MenuItem
        Me.settings_menu = New System.Windows.Forms.MenuItem
        Me.help_menu = New System.Windows.Forms.MenuItem
        Me.about_menu = New System.Windows.Forms.MenuItem
        Me.full_help = New System.Windows.Forms.MenuItem
        Me.run_button = New System.Windows.Forms.Button
        Me.exit_button = New System.Windows.Forms.Button
        Me.spider_progress = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.file_menu, Me.configure_menu, Me.help_menu})
        '
        'file_menu
        '
        Me.file_menu.Index = 0
        Me.file_menu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.view_log_menu, Me.exit_menu})
        Me.file_menu.Text = "&File"
        '
        'view_log_menu
        '
        Me.view_log_menu.Index = 0
        Me.view_log_menu.Text = "&View Log"
        '
        'exit_menu
        '
        Me.exit_menu.Index = 1
        Me.exit_menu.Text = "E&xit"
        '
        'configure_menu
        '
        Me.configure_menu.Index = 1
        Me.configure_menu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.settings_menu})
        Me.configure_menu.Text = "&Configure"
        '
        'settings_menu
        '
        Me.settings_menu.Index = 0
        Me.settings_menu.Text = "&Settings"
        '
        'help_menu
        '
        Me.help_menu.Index = 2
        Me.help_menu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.about_menu, Me.full_help})
        Me.help_menu.Text = "&Help"
        '
        'about_menu
        '
        Me.about_menu.Index = 0
        Me.about_menu.Text = "&About"
        '
        'full_help
        '
        Me.full_help.Index = 1
        Me.full_help.Text = "&Spider Help"
        '
        'run_button
        '
        Me.run_button.Location = New System.Drawing.Point(24, 16)
        Me.run_button.Name = "run_button"
        Me.run_button.Size = New System.Drawing.Size(136, 32)
        Me.run_button.TabIndex = 0
        Me.run_button.Text = "Run Spider"
        '
        'exit_button
        '
        Me.exit_button.Location = New System.Drawing.Point(24, 80)
        Me.exit_button.Name = "exit_button"
        Me.exit_button.Size = New System.Drawing.Size(136, 32)
        Me.exit_button.TabIndex = 1
        Me.exit_button.Text = "Exit"
        '
        'spider_progress
        '
        Me.spider_progress.Location = New System.Drawing.Point(16, 128)
        Me.spider_progress.Name = "spider_progress"
        Me.spider_progress.Size = New System.Drawing.Size(528, 56)
        Me.spider_progress.TabIndex = 2
        Me.spider_progress.Text = "Spider ready"
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(552, 193)
        Me.Controls.Add(Me.spider_progress)
        Me.Controls.Add(Me.exit_button)
        Me.Controls.Add(Me.run_button)
        Me.MaximizeBox = False
        Me.Menu = Me.MainMenu1
        Me.Name = "Form1"
        Me.Text = "Spider"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub exit_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exit_button.Click
        spider_running = False
        Thread.CurrentThread.Sleep(200)
        System.Environment.Exit(0)
    End Sub

    Private Sub exit_menu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exit_menu.Click
        System.Environment.Exit(0)
    End Sub

    Private Sub settings_menu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles settings_menu.Click
        Dim frm2 As New Form2
        frm2.Show()
    End Sub


    ' Dim t As Thread
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' read our environment
        ' read our config
        ' get going
        Dim reg As RegistryKey
        Dim foo As String
        Dim args() As String
        Dim s As String
        Dim i As Integer

        RegexValidators.Clear()
        OUR_EPOCH = Convert.ToDateTime("01/01/0001")
        Debug.WriteLine("EPOCH: " + OUR_EPOCH.ToString)

        ' only allow one instance
        Dim aModuleName As String = Diagnostics.Process.GetCurrentProcess.MainModule.ModuleName

        Dim aProcName As String = System.IO.Path.GetFileNameWithoutExtension(aModuleName)

        If Process.GetProcessesByName(aProcName).Length > 1 Then
            Application.Exit()
        End If

        Me.Text = "Spider " + SPIDER_VERSION
        args = Environment.GetCommandLineArgs()
        Debug.WriteLine("number of args: " + args.Length.ToString)

        For i = 0 To UBound(args)
            Debug.WriteLine("ARG: " + args(i).ToString)
        Next

        read_config()

        For Each arg As String In args
            Debug.WriteLine("arg:" + arg.ToString)
            ' if one of our args is "/run", we'll set unattended mode and go from
            ' there
            If arg.Equals("/run") Then
                Unattended = True
                ExitWhenFinished = True
                Restorewindow = False
                LaunchLogViewer = False
                Me.WindowState = Minimized
                Me.Visible = False
                Me.spider_progress.Text = "Unattended"
                Me.Refresh()
            End If
            ' log path
            If arg.Equals("/L:") Then
                If args(args.IndexOf(args, "/L:") + 1) <> String.Empty Then
                    LogPath = args(args.IndexOf(args, "/L:") + 1)
                End If
                Debug.WriteLine("log path: " + LogPath)
                '                prep_start_dir(args, args.IndexOf(args, "/L:"))
                NewLogPath = expand_path(LogPath)
                Debug.WriteLine("log path NOW: " + NewLogPath)
            End If
            ' recursive start directory
            If arg.Equals("/R:") Then
                If args(args.IndexOf(args, "/R:") + 1) <> String.Empty Then
                    StartDir = args(args.IndexOf(args, "/R:") + 1)
                    Recurse = True
                    Debug.WriteLine("Starting at path: " + StartDir)
                End If
            End If
            ' nonrecursive start directory
            If arg.Equals("/D:") Then
                If args(args.IndexOf(args, "/D:") + 1) <> String.Empty Then
                    StartDir = args(args.IndexOf(args, "/D:") + 1)
                    Recurse = False
                End If
            End If
            If arg.ToLower.Equals("/h") Or arg.Equals("/?") Then
                Console.OpenStandardOutput()
                Console.WriteLine("Usage: " + vbCrLf + "spider.exe")
                Console.WriteLine("/run             causes spider to run unattended")
                Console.WriteLine("/L: <logfile>    write logs to <logfile>")
                Console.WriteLine("/D: <path>       scan path without descending directories")
                Console.WriteLine("/R: <path>       scan path, descending directories")
                Console.WriteLine("/h or /?         this text")
                System.Environment.Exit(0)
            End If
            If arg.ToLower.Equals("/debug") Then
                DoDebug = True
            End If
            If arg.Equals("n") Then
                Debug.WriteLine("encryption off")
                Encrypt = False
            End If
        Next

        If Unattended Then
            Dim t As New Thread(AddressOf Me.run_spider)
            spider_running = True
            t.IsBackground = True
            t.Start()
            If Priority = 1 Then
                t.Priority = ThreadPriority.BelowNormal
            End If
            If Priority = 2 Then
                t.Priority = ThreadPriority.AboveNormal
            End If
            '            While t.IsAlive
            '           Thread.CurrentThread.Sleep(1000)
            '          End While
            '         spider_running = False
            '        System.Environment.Exit(0)
        End If

    End Sub

    Public Sub run_spider()
        ' do the dirty work
        ' we should have start path, etc from our registry settings
        ' if we're writing a local log file, truncate it now
        Dim drivelist() As String
        Dim filelist As New ArrayList
        Dim ADSOrig As Boolean
        Dim logPath As String
        Dim newPath As String
        Dim adsPath() As String
        Dim hostinfo As String
        Dim tList As New ArrayList
        Dim i As Integer
        Dim regKey As RegistryKey
        Dim ScanWeb As Boolean = False

        ' flush the file list
        ' filelist.Clear()

        ' Dim total_files As Integer
        Debug.WriteLine("inside run_spider")

        Me.run_button.Text = "Stop Spider"
        Me.Refresh()

        spider_running = True

        spider_start = DateTime.Now

        ADSOrig = ProcessADS

        Me.spider_progress.Text = "Building file list ..."
        Debug.WriteLine("progress: " + Me.spider_progress.Text)
        Me.Refresh()

        ' see what we're scanning
        If StartDir.StartsWith("http:") Or StartDir.StartsWith("https:") Then
            ScanWeb = True
        End If

        ' if there's a local log file, truncate it now
        ' if not, create an empty one

        ' if the log appears to contain ADS syntax, we'll check for the named stream.  In the case
        ' of ADS logging, we will *not* truncate the named stream if it already exists

        If LocalLog Then
            Debug.WriteLine("dealing with log path: " + NewLogPath)
            adsPath = NewLogPath.Split(System.IO.Path.DirectorySeparatorChar)
            logPath = adsPath((adsPath.Length - 1))
            If InStr(logPath, ":") Then

            Else
                '                Dim dInfo As New DirectoryInfo(NewLogPath)
                '                logPath = dInfo.Name
                Debug.WriteLine("truncating old log file")
                If IO.File.Exists(NewLogPath) And Not AppendLog Then
                    ' 7 pass wipe the previous log before truncating it.
                    wipe(NewLogPath, Module2.WipeTarget.File, Module2.WipeMethod.DoD_7Pass, Module2.WipeWhenDone.Truncate)
                End If
            End If
            '            IO.File.Create(LogPath)
        End If

        If Drives = 1 And Not ScanWeb And Not ScanUnalloc Then
            drivelist = Directory.GetLogicalDrives()
            For Each driveletter As String In drivelist
                Try
                    Debug.WriteLine("drive: " + driveletter.ToString)
                    driveletter += "\"
                    filelist = FindFiles(driveletter, Recurse, True)
                    ' Me.spider_progress.Text = "0/" + total_files.ToString
                    Me.spider_progress.Text = "Searching"
                    Me.Refresh()
                    process_files(filelist)
                Catch ex As Exception
                    Debug.WriteLine("exception with drive: " + driveletter + " is: " + ex.ToString)
                End Try
            Next
        ElseIf Drives = 0 And Not ScanWeb And Not ScanUnalloc Then
            Debug.WriteLine("inside run_spider and my start dir is: " + StartDir)
            ' if "StartDir" is actually a file in the filesystem, open it and treat 
            ' each line as a start directory
            ' as we're recursing, what we'll do is simply read the entire
            ' file into the arraylist and go on to process files
            If System.IO.File.Exists(StartDir) And Not System.IO.Directory.Exists(StartDir) Then
                Debug.WriteLine("appears we were passed a file instead of a directory: " + StartDir)
                ' read the lines of the file into an arraylist
                Try
                    Dim oFile As System.IO.File
                    Dim oRead As System.IO.StreamReader
                    oRead = oFile.OpenText(StartDir)
                    While (oRead.Peek <> -1)
                        tList.Add(oRead.ReadLine)
                    End While
                    oRead.Close()
                Catch ex As Exception
                    Debug.WriteLine("Exception reading target list: " + ex.ToString)
                End Try
                Me.spider_progress.Text = "Searching"
                Me.Refresh()
                For i = 0 To tList.Count - 1
                    Me.spider_progress.Text = "Acquiring file list from: " + tList.Item(i)
                    Me.Refresh()
                    filelist = FindFiles(tList.Item(i), Recurse, True)
                    process_files(filelist)
                    filelist.Clear()
                Next
            Else
                filelist = FindFiles(StartDir, Recurse, True)
                ' Me.spider_progress.Text = "0/" + total_files.ToString
                ' End If
                Me.spider_progress.Text = "Searching"
                Me.Refresh()
                process_files(filelist)
            End If
        ElseIf ScanWeb Then
            Me.spider_progress.Text = "Searching " + StartDir
            Me.Refresh()
            ' http/https scanning here
            ' need to figure out the start domain
            Dim fooURI As New Uri(StartDir)
            ' if DomainDepth is set, craft a tmp host and send that over
            Dim tmpHost As String
            Debug.WriteLine("DOMAINDEPTH: " + DomainDepth.ToString)
            If DomainDepth = 0 Then
                tmpHost = fooURI.Host
            Else
                Dim tmpParts As String() = fooURI.Host.Split(".")
                ' if the number of parts is less than the DomainDepth, send that over
                If tmpParts.Length < DomainDepth Then
                    tmpHost = fooURI.Host
                Else
                    ' take the last pieces
                    Dim j As Integer
                    For j = (tmpParts.Length - DomainDepth) To tmpParts.Length - 1
                        Debug.WriteLine("PIECE: " + tmpParts(j))
                        tmpHost += "." + tmpParts(j)
                    Next
                End If
            End If
            Debug.WriteLine("TMPHOST: " + tmpHost)
            process_http(StartDir, 0, tmpHost)
        ElseIf ScanUnalloc Then
            Me.spider_progress.Text = "Scanning unallocated space"
            Me.Refresh()
            process_unallocated("C:")
        End If

        spider_end = DateTime.Now
        CSVHeader = False

        Debug.WriteLine("incrementals check")
        If Incrementals And Not ScanWeb And Not ScanUnalloc Then
            Debug.WriteLine("incrementals are SET")
            ' need to open the registry for writing and store spider_end as LastScanDate
            Debug.WriteLine("setting last scan date to: " + spider_end.ToString)
            Try
                regKey = Registry.LocalMachine.OpenSubKey("Software\Cornell University\Spider\Runtime", True)
            Catch
                MsgBox("Registry keys for spider not found.", MsgBoxStyle.OKOnly, "Spider " + SPIDER_VERSION)
                System.Environment.Exit(0)
            End Try
            regKey.SetValue("LastScanDate", spider_end.ToString)
            regKey.Close()
            LastScanDate = spider_end
        End If

        If CheckPoint And Not ScanWeb And Not ScanUnalloc Then
            ' we're done, so ...
            Try
                regKey = Registry.LocalMachine.OpenSubKey("Software\Cornell University\Spider\Runtime", True)
            Catch
                MsgBox("Registry keys for spider not found.", MsgBoxStyle.OKOnly, "Spider " + SPIDER_VERSION)
                System.Environment.Exit(0)
            End Try
            regKey.SetValue("CheckPointPath", "")
            regKey.Close()
            CheckPointFound = False
        End If


        ' if we've been set to annotate the log file, create that entry now
        Debug.WriteLine("logPath: " + logPath)
        Debug.WriteLine("newlogpath: " + NewLogPath)
        If LogAttributes And LogMask.attrHostinfo Then
            hostinfo = expand_path(LogHostInfo)
            Debug.WriteLine("host info logged " + hostinfo)
            write_log(hostinfo, NewLogPath)
        End If

        ' OK, write out the encrypted log array
        If Encrypt Then
            '            Debug.WriteLine("writing encrypted log to: " + logPath)
            crypt_log_array(NewLogPath, Module1.CryptoAction.Encrypt)
        End If

        Me.spider_progress.Text = "Spider Ready"
        Me.run_button.Text = "Run Spider"
        Me.Text = "Spider " + SPIDER_VERSION
        spider_running = False
        Me.Refresh()

        Debug.WriteLine("exit when finished")
        Debug.WriteLine(ExitWhenFinished)

        If ExitWhenFinished Then
            System.Environment.Exit(0)
        End If

        If LaunchLogViewer Then
            ' do that
            Debug.WriteLine("launching log viewer")
            Dim oFile As System.IO.File
            Dim oRead As System.IO.StreamReader
            enc_log_plaintext = ""
            If Not Encrypt Then
                Try
                    oRead = oFile.OpenText(NewLogPath)
                Catch ex As Exception
                    Return
                End Try
                enc_log_plaintext = oRead.ReadToEnd
                oRead.Close()
            Else
                crypt_log_array(NewLogPath, Module1.CryptoAction.Decrypt)
                ' we've got a pile of text.
            End If
            ' Debug.WriteLine("will show: " + enc_log_plaintext)
            push_viewer(enc_log_plaintext, NewLogPath, True)
        End If

        Me.WindowState = Normal

    End Sub

    Private Sub run_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles run_button.Click
        ' change it to a stop button and do what we've been told to do

        If spider_running Then
            Debug.WriteLine("Stopping spider worker threads")

            spider_running = False

            Me.run_button.Text = "Run Spider"
            Me.spider_progress.Text = "Stopped"
            Me.Text = "Spider " + SPIDER_VERSION
            Me.Refresh()
            ' reset our counters
            files_processed = 0
            total_files = 0
            Return
        End If

        If Encrypt Then
            ' for now, can't do this unattended
            If Unattended Then
                MsgBox("Unable to prompt for password in unattended mode.", MsgBoxStyle.OKOnly, "Spider " + SPIDER_VERSION)
                System.Environment.Exit(0)
            End If
            ' need to get a password
            Dim resp As DialogResult
            Dim frm4 As New Form4
            frm4.password_label.Text = "Enter a password that will be used to encrypt the spider log."
            Dim screwup As Integer = 1
            While screwup <= 3
                resp = frm4.ShowDialog
                Debug.WriteLine("form4:" + frm4.password_box.Text)
                Debug.WriteLine("response: " + resp.ToString)
                '            Dim screwup As Integer = 1
                If resp = DialogResult.OK Then
                    If frm4.password_box.Text = frm4.password_confirm_box.Text Then
                        '          End If
                        PASSWORD = frm4.password_box.Text
                        Exit While
                    Else
                        PASSWORD = ""
                    End If
                Else
                    PASSWORD = ""
                    Exit While
                End If
                screwup += 1
            End While
            If PASSWORD = String.Empty Then
                MsgBox("Cannot start spider without a password to encrypt its log file.", MsgBoxStyle.OKOnly, "Spider " + SPIDER_VERSION)
                Return
            End If
        End If


        Debug.WriteLine("stopping any spider worker threads")

        Dim t As New Thread(AddressOf Me.run_spider)
        files_processed = 0

        If t.IsAlive Then
            t.Abort()
        End If

        If t.ThreadState = ThreadState.Stopped Then
            'Dim t As New Thread(AddressOf run_spider)
            Return
        End If

        t.IsBackground = True
        t.Start()

        Debug.WriteLine("spider started")

        If t.IsAlive Then
            Debug.WriteLine("spider alive")
        End If

        If Minimize Then
            Me.WindowState = Minimized
        End If

        If Priority = 1 Then
            Debug.WriteLine("setting low priority")
            t.Priority = ThreadPriority.BelowNormal
        End If

        If Priority = 2 Then
            Debug.WriteLine("setting high priority")
            t.Priority = ThreadPriority.AboveNormal
        End If

        ' hidden from the user; we can set it with regedit
        If Priority = 42 Then
            t.Priority = ThreadPriority.Highest
        End If

    End Sub

    Private Sub about_menu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles about_menu.Click
        Dim frm3 As New about_spider
        frm3.Show()
    End Sub

    Private Sub view_log_menu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles view_log_menu.Click
        Dim path As String
        Dim FBD As New System.Windows.Forms.OpenFileDialog
        Dim oFile As System.IO.File
        Dim oRead As System.IO.StreamReader
        Dim i As Integer
        Dim dStreams() As String
        Dim nStream As String = ""
        Dim fSelect As String

        '   Debug.WriteLine("starting path: " + NewLogPath)

        FBD.Title = "Select a spider log file"
        FBD.CheckFileExists = True

        ' FBD.FileName = "C:\SPIDER.LOG"

        ' see if we're dealing with an ADS log
        dStreams = NewLogPath.Split(System.IO.Path.DirectorySeparatorChar)
        fSelect = dStreams(dStreams.Length - 1)
        '       Debug.WriteLine("filename: " + fSelect)
        If InStr(fSelect, ":") Then
            dStreams = fSelect.Split(":")
            '       Debug.WriteLine("log path contains an ADS reference")
            fSelect = NewLogPath.TrimEnd(dStreams(1).ToCharArray)
            fSelect = fSelect.TrimEnd(":")
        Else
            fSelect = NewLogPath
        End If

        '      Debug.WriteLine("start path now: " + fSelect)

        If IO.File.Exists(fSelect) Then
            FBD.FileName = fSelect
        Else
            FBD.FileName = "C:\SPIDER.LOG"
        End If

        i = FBD.ShowDialog()

        path = FBD.FileName

        If i = DialogResult.Cancel Then
            Return
        End If

        If path = String.Empty Then
            Return
        End If

        ' read the first line of the file and see if it is "ENCRYPTED
        ' if so, pop up the password prompt and continue
        If Not (System.IO.File.Exists(path)) Then
            MsgBox("File does not exist.", MsgBoxStyle.Information, "Spider " + SPIDER_VERSION)
            Return
        End If

        ' check to see if there are alternate data streams beyond this file.
        ' if there are, pop up a dialog and allow the user to select the one they 
        ' want
        ' 
        ' at that point, open the ADS and proceed normally.
        Try
            dStreams = ADSFile.GetStreams(path)
        Catch ex As Exception
            ' don't know that there's much to worry about
        End Try

        '       Debug.WriteLine("LOG STREAMS: " + dStreams.Length.ToString)
        If dStreams.Length > 0 Then
            ' we have alternate data streams.
            ' pop up a selection box.
            Dim LB As New Form7
            For i = 0 To (dStreams.Length - 1)
                LB.ads_listbox.Items.Add(dStreams(i))
            Next
            LB.ads_listbox.SelectionMode = SelectionMode.One
            LB.ads_listbox.Sorted = True
            i = LB.ShowDialog()
            If i = DialogResult.OK Then
                nStream = LB.ads_listbox.SelectedItem
            End If
            If i = DialogResult.Cancel Then
                nStream = ""
                Return
            End If
            If i = DialogResult.Ignore Then
                nStream = ""
            End If
        End If

        ' if we've got an ADS selected, we'll have to deal with that.

        If Not Encrypt Then
            If nStream = String.Empty Then
                Try
                    oRead = oFile.OpenText(path)
                Catch ex As Exception
                    Return
                End Try
                enc_log_plaintext = oRead.ReadToEnd
                oRead.Close()
            Else
                ' ADS, deal with that
                Try
                    oRead = ADSFile.OpenText(path, nStream)
                Catch ex As Exception
                    Return
                End Try
                enc_log_plaintext = oRead.ReadToEnd
                oRead.Close()
            End If
        Else
            '           Debug.WriteLine("prepping password")
            ' display the file
            ' deal with the encrypted file;
            ' pop up the password prompt
            Dim frm4 As New Form4
            frm4.password_label.Text = "Please supply the password that was used to encrypt this log file"
            frm4.password_confirm_box.Enabled = False
            i = frm4.ShowDialog()
            If i = DialogResult.OK Then
                PASSWORD = frm4.password_box.Text
            Else
                PASSWORD = ""
            End If
            enc_log_plaintext = ""
            ' crypt_log_array will sort it out
            If nStream <> String.Empty Then
                path += ":" + nStream
            End If

            crypt_log_array(path, Module1.CryptoAction.Decrypt)
            ' we've got a pile of text.
        End If
        ' Debug.WriteLine("will show: " + enc_log_plaintext)
        push_viewer(enc_log_plaintext, path, False)

    End Sub
    Public Sub push_viewer(ByVal toDisplay As String, ByVal pPath As String, ByVal isDialog As Boolean)
        Dim line() As String
        Dim index As Integer = 0
        Dim newtext As String
        ' Dim encoding As New System.Text.ASCIIEncoding
        'Dim foo() As Byte


        Dim frm5 As New Form5

        If enc_log_plaintext = String.Empty Then
            MsgBox("Empty log or incorrect password for an encrypted log", MsgBoxStyle.Exclamation)
            Return
        End If

        frm5.log_box.Text = enc_log_plaintext
        frm5.Text = "Spider Log Viewer: " + pPath
        If isDialog Then
            frm5.ShowDialog()
        Else
            frm5.Show()
        End If
        '        frm5.Show()

        Return
    End Sub

    Private Sub full_help_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles full_help.Click
        Start(SPIDER_HELP_URL)
        Return
    End Sub

    Public Sub process_unallocated(ByVal drive As String)
        ' pretty simple.  we'll create a new zero-length temporary file
        ' we'll then extend that file in multiples of the blocksize, reading as we go
        ' Dim tempChunk = System.IO.Path.GetTempFileName
        Dim nRead As Integer
        Dim offT As Int64 = 0
        Dim scanT As Int64 = 0
        Dim cRead(1024 * 1024) As Char
        Dim hit As String
        Dim frag As String
        Dim READ_SIZE As Integer
        Dim mult As Integer
        '        Dim Loc As String = ""
        Dim Chunk As String

        drive = drive.TrimEnd("\")

        If DriveBlocksize.Contains(drive) Then
            HDD_BLOCKSIZE = DriveBlocksize.Item(drive)
            HDD_FREESPACE = DriveFreespace.Item(drive)
        Else
            '            Dim drive As String = pPath.Split(":")(0)
            get_disk_properties(drive)
        End If

        ' figure the largest multiple of the blocksize that'll fit within freespace and that's
        ' what we'll read
        mult = Int((1024 * 1024) / HDD_BLOCKSIZE)

        If mult = 0 Then
            Return
        End If

        Dim i As Integer

        For i = mult To 1 Step -1
            Debug.WriteLine("trying: " + (i * HDD_BLOCKSIZE).ToString)
            If (HDD_FREESPACE Mod (i * HDD_BLOCKSIZE) = 0) Then
                Exit For
            End If
        Next

        READ_SIZE = HDD_BLOCKSIZE * i

        Debug.WriteLine("READ SIZE: " + READ_SIZE.ToString)

        Chunk = drive + "\spider.unalloc_tmp"

        If System.IO.File.Exists(Chunk) Then
            System.IO.File.Delete(Chunk)
        End If

        Loc = ""

        Try
            ' basically works like this:
            ' extend the file by BLOCKSIZE,
            ' read the interval from the current location to EOF, scan it,
            ' repeat
            If Not SMVN_priv Then
                ' we need to elevate our privs, or throw an error if we can't.
                AdjustTokenDisk()
            End If
            Dim oP As New FileStream(Chunk, FileMode.Create, FileAccess.ReadWrite)
            Dim rP As New StreamReader(oP)

            Dim ret As Long
            ' unalloc to file
            If PullUnallocToFile Then
                If System.IO.File.Exists(UnallocFile) Then
                    wipe(UnallocFile, Module2.WipeTarget.File, Module2.WipeMethod.DoD_7Pass, Module2.WipeWhenDone.Truncate)
                End If
            End If
            Try
                While spider_running
                    ' inefficient to process such small chunks, but we're guaranteed of 
                    ' an integer number of blocks on the disk.
                    ' rP.BaseStream.Position = offT + HDD_BLOCKSIZE
                    '                    oP.SetLength(rP.BaseStream.Position + HDD_BLOCKSIZE)
                    '  Debug.WriteLine("setting file pointer to: " + offT.ToString)
                    ret = SetFilePointer(oP.Handle, offT, 0, Convert.ToUInt32(FILE_BEGIN))
                    ' Debug.WriteLine("setfilepointer: " + GetLastError.ToString)
                    ' rP.BaseStream.Position = offT

                    If Not SetEndOfFile(oP.Handle) Then
                        Debug.WriteLine("setendoffile failed: " + GetLastError.ToString)
                    End If


                    If Not SetFileValidData(oP.Handle, Convert.ToInt64(offT)) Then
                        Debug.WriteLine("setfilevaliddata failed: " + GetLastError.ToString)
                    End If

                    ' we've grabbed a block of the file; set our file pointer back one blocksize
                    ' to make way for reading
                    If offT >= READ_SIZE Then
                        rP.BaseStream.Position -= READ_SIZE
                    End If

                    offT += READ_SIZE

                    Me.spider_progress.Text = "Scanned: " + rP.BaseStream.Position.ToString + " of " + HDD_FREESPACE.ToString + " bytes"
                    Me.Refresh()
                    '                    oP.Close()
                    nRead = rP.ReadBlock(cRead, 0, READ_SIZE)
                    'Debug.WriteLine("read: " + nRead.ToString + " bytes")
                    scanT += nRead
                    'Debug.WriteLine("unalloc: " + Convert.ToString(cRead))
                    If (is_match(Convert.ToString(cRead), hit, frag)) Then
                        If PullUnallocToFile Then
                            Try
                                Dim uP As New FileStream(UnallocFile, FileMode.Append, FileAccess.Write)
                                Dim wU As New BinaryWriter(uP)
                                wU.Write(cRead)
                                wU.Close()
                            Catch ex As Exception
                                ' not much we can do if the above fails.
                            End Try
                        End If
                        If WipeUnallocMatchingBlocks Then
                            If Not wipe_block(oP.Handle, READ_SIZE, oP.Position, Module2.WipeMethod.Overwrite_with_nulls) Then
                                Debug.WriteLine("wipe_block failed")
                            End If
                        End If
                        If Loc = String.Empty Then
                            Loc = "Unallocated space"
                        End If
                        send_match("Unallocated Space", hit, frag)
                        ' want to rethink the following...
                        If FastMatch Then
                            oP.Close()
                            System.IO.File.Delete(Chunk)
                            spider_running = False
                            Me.spider_progress.Text = "Data found"
                            Me.Refresh()
                            Thread.CurrentThread.Abort()
                        End If
                    End If
                End While
                oP.Close()
                System.IO.File.Delete(Chunk)
                spider_running = False
                Me.spider_progress.Text = "Unallocated space scanned."
                Me.Refresh()
                Thread.CurrentThread.Abort()
            Catch ex As Exception
                Debug.WriteLine("inner exception: " + ex.ToString)
                oP.Close()
                System.IO.File.Delete(Chunk)
            End Try
        Catch ex As Exception
            Debug.WriteLine("Exception creating new file: " + ex.ToString)
        End Try

        ' Kill(oP)

        Return

    End Sub

    Public Sub process_files(ByVal targetlist As ArrayList)
        ' we'll iterate over the arraylist calling scan() for each file
        Dim pPath As String
        Dim finished As Integer
        Dim target_count As Integer = 0
        Dim origAtime As Date

        Debug.WriteLine("total files: " + targetlist.Count.ToString)
        total_files = targetlist.Count
        For Each pPath In targetlist
            Debug.WriteLine("processing: " + pPath.ToString)
            Debug.WriteLine("spider running: " + spider_running.ToString)
            If Not spider_running Then
                If CheckPoint Then
                    Debug.WriteLine("SETTING CHECKPOINT: " + pPath)
                    Dim regKey As RegistryKey
                    Try
                        regKey = Registry.LocalMachine.OpenSubKey("Software\Cornell University\Spider\Runtime", True)
                    Catch
                        MsgBox("Registry keys for spider not found.", MsgBoxStyle.OKOnly, "Spider " + SPIDER_VERSION)
                        System.Environment.Exit(0)
                    End Try
                    regKey.SetValue("CheckPointPath", pPath)
                    regKey.Close()
                    CheckPointFound = False
                End If
                Debug.WriteLine("aborting worker thread")
                Thread.CurrentThread.Abort()
            End If
            If files_processed Mod ModReport = 0 Then
                Me.Text = "Processing file " + files_processed.ToString + " of " + total_files.ToString
                Me.Refresh()
            End If
            Me.spider_progress.Text = pPath.ToString
            Me.Refresh()

            If NoAtime Then
                Try
                    origAtime = IO.File.GetLastAccessTime(pPath)
                Catch
                End Try
            End If

            scan(pPath)
            ' if scanslack is set, compute the next multiple of the blocksize larger than
            ' the file, set the file length to that, set our starting position to the old end of file, 
            ' and scan the difference.
            If ScanSlack Then
                process_slack(pPath)
            End If

            If NoAtime Then
                reset_atime(pPath, origAtime)
            End If

            target_count += 1

            If LogProgress Then
                If target_count Mod 1000 = 0 Then
                    finished = Int(target_count / targetlist.Count) * 100
                    Debug.WriteLine("finished: " + finished.ToString)
                    send_Evt_log("Spider is " + finished.ToString + "% finished with " + targetlist.Count.ToString + " files")
                End If
            End If
        Next

        Return
    End Sub
    Public Sub process_slack(ByVal pPath As String)
        Dim fSize As Int64 = 0
        Dim nSize As Int64 = 0
        Dim depth As Int64 = 0
        Dim deltaS As Int64 = 0
        Dim nRead As Integer = 0
        Dim cRead(65536) As Char
        Dim hit As String
        Dim frag As String
        Dim fAttr As FileAttribute
        Dim r As Integer
        Dim ret As Long

        Dim drive As String = pPath.Split(":")(0)
        drive += ":"
        If DriveBlocksize.Contains(drive) Then
            HDD_BLOCKSIZE = DriveBlocksize.Item(drive)
            HDD_FREESPACE = DriveFreespace.Item(drive)
        Else
            get_disk_properties(drive)
        End If

        fSize = FileLen(pPath)
        deltaS = fSize - (Int(fSize / HDD_BLOCKSIZE) * HDD_BLOCKSIZE)
        nSize = fSize - deltaS + HDD_BLOCKSIZE
        Debug.WriteLine("Old size: " + fSize.ToString + " new size: " + nSize.ToString)
        Loc = ""
        depth = 0
        'fAttr = System.IO.File.GetAttributes(pPath)
        If Not SMVN_priv Then
            AdjustTokenDisk()
        End If

        If fSize > 800 And deltaS <> 0 Then
            Try
                Dim oP As New FileStream(pPath, FileMode.Open, FileAccess.ReadWrite)
                '       System.IO.File.SetAttributes(pPath, FileAttributes.SparseFile)
                Dim rP As New StreamReader(oP)
                ' here's what we'll do:
                ' pad the file size off to the next multiple of the block size
                ' we'll call setfilevalidata and setendoffile to cement that,
                ' then we'll scan the slack space we've found.
                ret = SetFilePointer(oP.Handle, nSize, 0, Convert.ToUInt32(FILE_BEGIN))
                If ret = 0 Then
                    Debug.WriteLine("err: " + GetLastError.ToString)
                    oP.Close()
                    Return
                End If
                If Not SetEndOfFile(oP.Handle) Then
                    Debug.WriteLine("setEOF: " + GetLastError.ToString)
                    oP.Close()
                    Return
                End If
                If Not SetFileValidData(oP.Handle, Convert.ToInt64(nSize - 1)) Then
                    Debug.WriteLine("setfilevaliddata: " + GetLastError.ToString)
                    oP.Close()
                    Return
                End If
                ' set our start position to the previous EOF
                rP.BaseStream.Position = fSize
                While rP.Peek <> -1
                    nRead = rP.ReadBlock(cRead, 0, READ_SIZE)
                    If Not spider_running Then
                        ' close up and exit
                        '                      rP.Close()
                        Me.spider_progress.Text = "Stopped"
                        Me.Refresh()
                        spider_running = False
                        Exit While
                        '                        Thread.CurrentThread.Abort()
                        '                       Return
                    End If
                    Debug.WriteLine("Read: " + nRead.ToString)
                    depth += nRead
                    Debug.WriteLine("Slack: " + Convert.ToString(cRead))
                    If is_match(Convert.ToString(cRead), hit, frag) Then
                        If Loc = String.Empty Then
                            Loc = "Slack Offset: " + depth.ToString
                        End If
                        send_match(pPath, hit, frag)
                    End If
                End While
                ' restore the file size
                ret = SetFilePointer(oP.Handle, fSize, 0, Convert.ToUInt32(FILE_BEGIN))
                If ret = 0 Then
                    Debug.WriteLine("err: " + GetLastError.ToString)
                    oP.Close()
                    Return
                End If
                If Not SetEndOfFile(oP.Handle) Then
                    Debug.WriteLine("setEOF: " + GetLastError.ToString)
                    oP.Close()
                    Return
                End If
                If Not SetFileValidData(oP.Handle, Convert.ToInt64(fSize)) Then
                    Debug.WriteLine("setfilevaliddata: " + GetLastError.ToString)
                    oP.Close()
                    Return
                End If
                oP.Close()
            Catch ex As Exception
                Debug.WriteLine("Exception setting file size: " + ex.ToString)
            End Try
        Else
            Debug.WriteLine("skipping either resident data or integer blocksize file")
        End If
    End Sub
    Public Sub process_http(ByVal StartURL As String, ByVal Currentdepth As Integer, ByVal Currentdomain As String)
        Dim scanpath As String
        Dim currdomain As String
        Dim frag As String
        Dim hit As String
        Dim sRead As String
        Dim nRead As Integer
        Dim currdepth As Integer = 0
        Dim i As Integer
        Dim Links As New ArrayList

        Dim request As HttpWebRequest
        Dim response As HttpWebResponse

        ' Dim thisURI As Uri
        ' we'll spider the site, recursing to depth N off the main site
        ' we'll expect the following:
        ' MaxURLDepth int -- max distance from the main page
        ' JumpDomain bool -- whether or not to visit links that lead to other servers

        ' we'll start by pulling the first page and scanning it as though it were a file
        ' we'll then grab all the links off the page, using JumpDomain as a gauge for relevance
        ' we'll then pull and scan those, recursing as we go
        Debug.WriteLine("Scanning URL: " + StartURL)

        Dim thisURI As New Uri(StartURL)

        Me.spider_progress.Text = "Scanning: " + StartURL
        Me.Refresh()

        If (Currentdepth = MaxURLDepth) Then
            Return
        End If

        ' don't scan robots.txt
        If StartURL.EndsWith("obots.txt") Then
            Return
        End If

        ' check the domain of this URL against the Currentdomain and JumpDomain values
        If Not JumpDomain Then
            ' check
            ' if DomainDepth is 0, it means an exact match is required
            ' otherwise, we're just looking for tail-end
            If DomainDepth = 0 Then
                If thisURI.Host.ToLower <> Currentdomain.ToLower Then
                    Return
                End If
            Else
                If Not thisURI.Host.EndsWith(Currentdomain) Then
                    Return
                End If
            End If
        End If

        ' if RespectRobots is set, and we haven't pulled one for this host yet, try now
        ' we'll parse it, and set a skip disposition for this host
        ' if RespectRobots is set and we already have a skip dispo for this host, 
        ' process it now
        If RespectRobots Then
            ' Dim robotSkipList As New ArrayList
            If RobotsDone.Contains(thisURI.Host.ToLower) Then
                ' we've been here before
                If check_robots(StartURL) Then
                    ' nope
                    Me.spider_progress.Text = "Robots.txt skipping: " + StartURL
                    Me.Refresh()
                    Return
                End If
            Else
                populate_robots(StartURL)
                If check_robots(StartURL) Then
                    Me.spider_progress.Text = "Robots.txt skipping: " + StartURL
                    Me.Refresh()
                    Return
                End If
            End If
        End If

        ' for our various file-type scanners (ZIP, PDF, etc), we'll pull the contents of the URL
        ' down to a temporary file that's removed when Spider closes, then hand that 
        ' path off to process_whatever
        ' the trick is going to be getting the logfile to reflect the URL instead of the disk-path
        ' to the temporary file.  *sigh*  code bloat.

        Try
            request = DirectCast(WebRequest.Create(StartURL), HttpWebRequest)
            If UserAgent <> String.Empty Then
                request.UserAgent = UserAgent
            End If
            ' if we've got a username and password, ready those, too

            request.MaximumAutomaticRedirections = MaxRedirections

            ' fire away
            response = DirectCast(request.GetResponse(), HttpWebResponse)
            ' check our status codes

            ' redirects get checked for proper domain versus JumpDomain

            If MaxContentLength > 0 Then
                If response.ContentLength >= (MaxContentLength * 1024 * 1024) Then
                    Debug.WriteLine("Excessive content length, Max: " + MaxContentLength.ToString + "MB got: " + response.ContentLength.ToString)
                    response.Close()
                    Return
                End If
            End If

            If SkipContentRegex.Count <> 0 Then
                Dim en As IDictionaryEnumerator = SkipContentRegex.GetEnumerator
                Dim skipContentReg As String
                Dim re As Regex
                While en.MoveNext
                    If re.Matches(response.ContentType, SkipContentRegex.Item(en.Key)) Then
                        Debug.WriteLine("Skipping content-type for regex: " + en.Key)
                        response.Close()
                        Return
                    End If
                End While
            End If

            ' barf!  I hate doing it this way, with two while loops, one used, one not, depending
            ' on the circumstances.

            Dim readHTML As StreamReader = New StreamReader(response.GetResponseStream)
            If DelegateContentTypes.Contains(response.ContentType) Then
                ' dump to a temp file and scan that file
                Dim tempContent = System.IO.Path.GetTempFileName
                Dim content As String = response.ContentType
                Dim writeData As StreamWriter = New StreamWriter(System.IO.File.OpenWrite(tempContent))
                Dim cRead(65535) As Char
                While readHTML.Peek <> -1
                    If Not spider_running Then
                        readHTML.Close()
                        writeData.Close()
                        response.Close()
                        System.IO.File.Delete(tempContent)
                        Me.spider_progress.Text = "Stopped"
                        Me.Refresh()
                        spider_running = False
                        Thread.CurrentThread.Abort()
                        Return
                    End If
                    nRead = readHTML.Read(cRead, 0, READ_SIZE)
                    writeData.Write(cRead, 0, nRead)
                End While
                readHTML.Close()
                writeData.Close()
                response.Close()
                If content = "application/pdf" Then
                    process_pdf_file(tempContent, hit, frag)
                End If
                If content = "application/zip" Then
                    process_zip_archive(tempContent, hit, frag)
                End If
                If content = "application/vnd.ms-excel" Then
                    process_excel(tempContent, hit, frag)
                End If
                If content = "application/x-gzip" Then
                    process_gzip_archive(tempContent, hit, frag)
                End If
                ' none of these types natively contains links, so we're done
                System.IO.File.Delete(tempContent)
                Return
            End If

            While readHTML.Peek <> -1
                If Not spider_running Then
                    readHTML.Close()
                    response.Close()
                    Me.spider_progress.Text = "Stopped"
                    Me.Refresh()
                    spider_running = False
                    Thread.CurrentThread.Abort()
                    Return
                End If
                sRead = readHTML.ReadLine
                Debug.WriteLine("Scanning: " + sRead)
                ' if RecurseURL is set, grab the links from this page and add them to an array
                ' when we're finished with this page, we'll iterate over those
                If RecurseURL Then
                    Links = getHrefs(sRead, thisURI.Host, thisURI.ToString, Links)
                End If
                If (is_match(sRead, hit, frag)) Then
                    ' decide what to do
                    Debug.WriteLine("Found: " + hit)
                    Debug.WriteLine("Frag: " + frag)
                End If
            End While
            readHTML.Close()
            ' if there are any links on the page, pull those and recurse to this function
        Catch ex As Exception
            Debug.WriteLine("error pulling HTML from web page: " + ex.ToString)
            Return
        End Try

        If RecurseURL And Links.Count > 0 Then
            ' run through again
            For i = 0 To Links.Count - 1
                Debug.WriteLine("Descending: " + Links.Item(i).ToString)
                process_http(Links.Item(i).ToString, (Currentdepth + 1), Currentdomain)
            Next
        End If

        Return

    End Sub
End Class
