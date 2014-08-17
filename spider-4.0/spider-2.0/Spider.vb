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
Imports System.Collections.Specialized ' StringCollection
' Imports AlternateDataStream
Imports System.Deployment.Application ' Add a reference to System.Deployment, allows us to grab 'Publish' details.



Public Class MainForm
    Inherits System.Windows.Forms.Form

#Region "Member Variables"
    ' Status reporting methods
    Public Enum StatusMedia
        None = 0
        Progress = 1
        Status = 2
        ' LogFile = 4
        ' MainFormRunButton = 8
    End Enum
    'Public Enum RunState
    '    Cleared = 0
    '    Stopped = 64
    '    BuildingFileList = 1
    '    Scanning = 2
    '    SavingFileList = 4
    '    StopRequested = 8
    '    ExitRequested = 16
    'End Enum
    '' Public FireflyRunState As RunState



    Public LastStatusUpdate As Date = New Date(0)
    Public LastProgressUpdate As Date = New Date(0)
    ' Public LastProgressUpdate As Date = New Date(0)

    Public TimeBetweenUpdates As Double = 1
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents stop_button As System.Windows.Forms.Button
    Friend WithEvents ScanProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents SecurityLogo As System.Windows.Forms.PictureBox
    Friend WithEvents Firefly_Status As System.Windows.Forms.Label
    Friend WithEvents IntroText As System.Windows.Forms.Label
    Friend WithEvents MenuItem4 As System.Windows.Forms.MenuItem
    Friend WithEvents Button2 As System.Windows.Forms.Button
#End Region

    Public cmdLineOpts As Collections.Specialized.StringCollection

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
    Friend WithEvents DisplayReportMenu As System.Windows.Forms.MenuItem
    Friend WithEvents exit_menu As System.Windows.Forms.MenuItem
    Friend WithEvents configure_menu As System.Windows.Forms.MenuItem
    Friend WithEvents settings_menu As System.Windows.Forms.MenuItem
    Friend WithEvents help_menu As System.Windows.Forms.MenuItem
    Friend WithEvents about_menu As System.Windows.Forms.MenuItem
    Friend WithEvents full_help As System.Windows.Forms.MenuItem
    Friend WithEvents run_button As System.Windows.Forms.Button
    Friend WithEvents exit_button As System.Windows.Forms.Button
    Friend WithEvents Firefly_progress As System.Windows.Forms.Label
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents ConfigureSetRunPriority As System.Windows.Forms.MenuItem
    Friend WithEvents ConfigureFileTypes As System.Windows.Forms.MenuItem
    Friend WithEvents DisplayLogMenu As System.Windows.Forms.MenuItem
    Friend WithEvents RestoreDefaultsMenu As System.Windows.Forms.MenuItem
    Friend WithEvents NewSearchMenu As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.MainMenu1 = New System.Windows.Forms.MainMenu(Me.components)
        Me.file_menu = New System.Windows.Forms.MenuItem
        Me.DisplayReportMenu = New System.Windows.Forms.MenuItem
        Me.DisplayLogMenu = New System.Windows.Forms.MenuItem
        Me.exit_menu = New System.Windows.Forms.MenuItem
        Me.configure_menu = New System.Windows.Forms.MenuItem
        Me.settings_menu = New System.Windows.Forms.MenuItem
        Me.ConfigureFileTypes = New System.Windows.Forms.MenuItem
        Me.NewSearchMenu = New System.Windows.Forms.MenuItem
        Me.ConfigureSetRunPriority = New System.Windows.Forms.MenuItem
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.RestoreDefaultsMenu = New System.Windows.Forms.MenuItem
        Me.help_menu = New System.Windows.Forms.MenuItem
        Me.about_menu = New System.Windows.Forms.MenuItem
        Me.full_help = New System.Windows.Forms.MenuItem
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.MenuItem4 = New System.Windows.Forms.MenuItem
        Me.exit_button = New System.Windows.Forms.Button
        Me.Firefly_progress = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.ScanProgress = New System.Windows.Forms.ProgressBar
        Me.Firefly_Status = New System.Windows.Forms.Label
        Me.IntroText = New System.Windows.Forms.Label
        Me.Button2 = New System.Windows.Forms.Button
        Me.SecurityLogo = New System.Windows.Forms.PictureBox
        Me.stop_button = New System.Windows.Forms.Button
        Me.run_button = New System.Windows.Forms.Button
        CType(Me.SecurityLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.file_menu, Me.configure_menu, Me.help_menu})
        '
        'file_menu
        '
        Me.file_menu.Index = 0
        Me.file_menu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.DisplayReportMenu, Me.DisplayLogMenu, Me.exit_menu})
        Me.file_menu.Text = "&File"
        '
        'DisplayReportMenu
        '
        Me.DisplayReportMenu.Index = 0
        Me.DisplayReportMenu.Text = "View &Report"
        '
        'DisplayLogMenu
        '
        Me.DisplayLogMenu.Index = 1
        Me.DisplayLogMenu.Text = "View &Log"
        '
        'exit_menu
        '
        Me.exit_menu.Index = 2
        Me.exit_menu.Text = "E&xit"
        '
        'configure_menu
        '
        Me.configure_menu.Index = 1
        Me.configure_menu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.settings_menu, Me.ConfigureFileTypes, Me.NewSearchMenu, Me.ConfigureSetRunPriority, Me.MenuItem3, Me.RestoreDefaultsMenu})
        Me.configure_menu.Text = "&Configure"
        '
        'settings_menu
        '
        Me.settings_menu.Index = 0
        Me.settings_menu.Text = "&Settings"
        Me.settings_menu.Visible = False
        '
        'ConfigureFileTypes
        '
        Me.ConfigureFileTypes.Index = 1
        Me.ConfigureFileTypes.Text = "&File Types"
        Me.ConfigureFileTypes.Visible = False
        '
        'NewSearchMenu
        '
        Me.NewSearchMenu.Index = 2
        Me.NewSearchMenu.Text = "Scan &Location"
        '
        'ConfigureSetRunPriority
        '
        Me.ConfigureSetRunPriority.Index = 3
        Me.ConfigureSetRunPriority.Text = "Run &Priority"
        '
        'MenuItem3
        '
        Me.MenuItem3.Index = 4
        Me.MenuItem3.Text = "-"
        '
        'RestoreDefaultsMenu
        '
        Me.RestoreDefaultsMenu.Index = 5
        Me.RestoreDefaultsMenu.Text = "Restore &Defaults"
        '
        'help_menu
        '
        Me.help_menu.Index = 2
        Me.help_menu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.about_menu, Me.full_help, Me.MenuItem1, Me.MenuItem2, Me.MenuItem4})
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
        Me.full_help.Text = "&Firefly Help"
        '
        'MenuItem1
        '
        Me.MenuItem1.Index = 2
        Me.MenuItem1.Text = "License"
        Me.MenuItem1.Visible = False
        '
        'MenuItem2
        '
        Me.MenuItem2.Index = 3
        Me.MenuItem2.Text = "Command Line"
        '
        'MenuItem4
        '
        Me.MenuItem4.Index = 4
        Me.MenuItem4.Text = "About &Report to CITES Security"
        '
        'exit_button
        '
        Me.exit_button.AccessibleDescription = "Exits Firefly after pausing the running scan."
        Me.exit_button.AccessibleName = "Exit Button"
        Me.exit_button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.exit_button.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.exit_button.Location = New System.Drawing.Point(411, 508)
        Me.exit_button.Name = "exit_button"
        Me.exit_button.Size = New System.Drawing.Size(80, 49)
        Me.exit_button.TabIndex = 1
        Me.exit_button.Text = "Exit"
        '
        'Firefly_progress
        '
        Me.Firefly_progress.AccessibleDescription = "Displays the filename currently being scanned."
        Me.Firefly_progress.AccessibleName = "Progress text"
        Me.Firefly_progress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Firefly_progress.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Firefly_progress.Location = New System.Drawing.Point(12, 430)
        Me.Firefly_progress.Name = "Firefly_progress"
        Me.Firefly_progress.Size = New System.Drawing.Size(479, 46)
        Me.Firefly_progress.TabIndex = 2
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(296, 508)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(109, 49)
        Me.Button1.TabIndex = 3
        Me.Button1.Text = "Test Security Report"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'ScanProgress
        '
        Me.ScanProgress.AccessibleDescription = "Displays scan progress."
        Me.ScanProgress.AccessibleName = "Scan Progress Bar"
        Me.ScanProgress.AccessibleRole = System.Windows.Forms.AccessibleRole.ProgressBar
        Me.ScanProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ScanProgress.Location = New System.Drawing.Point(15, 404)
        Me.ScanProgress.Name = "ScanProgress"
        Me.ScanProgress.Size = New System.Drawing.Size(476, 23)
        Me.ScanProgress.TabIndex = 6
        '
        'Firefly_Status
        '
        Me.Firefly_Status.AccessibleDescription = "Displays Firefly's current operation."
        Me.Firefly_Status.AccessibleName = "Status Text"
        Me.Firefly_Status.AccessibleRole = System.Windows.Forms.AccessibleRole.StatusBar
        Me.Firefly_Status.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Firefly_Status.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Firefly_Status.Location = New System.Drawing.Point(12, 377)
        Me.Firefly_Status.Name = "Firefly_Status"
        Me.Firefly_Status.Size = New System.Drawing.Size(479, 24)
        Me.Firefly_Status.TabIndex = 8
        Me.Firefly_Status.Text = "Press the 'Scan' button to start scanning."
        '
        'IntroText
        '
        Me.IntroText.AccessibleDescription = "Provides instructions for using Firefly."
        Me.IntroText.AccessibleName = "Firefly Introduction Text"
        Me.IntroText.AccessibleRole = System.Windows.Forms.AccessibleRole.StaticText
        Me.IntroText.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.IntroText.Location = New System.Drawing.Point(12, 126)
        Me.IntroText.Name = "IntroText"
        Me.IntroText.Size = New System.Drawing.Size(476, 257)
        Me.IntroText.TabIndex = 10
        Me.IntroText.Text = resources.GetString("IntroText.Text")
        '
        'Button2
        '
        Me.Button2.AccessibleDescription = "Displays the help web page in your web browser."
        Me.Button2.AccessibleName = "Help Button"
        Me.Button2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Button2.Location = New System.Drawing.Point(176, 479)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(81, 78)
        Me.Button2.TabIndex = 11
        Me.Button2.Text = "Help"
        Me.Button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'SecurityLogo
        '
        Me.SecurityLogo.AccessibleDescription = "The CITES Security Logo "
        Me.SecurityLogo.AccessibleName = "CITES Security Logo"
        Me.SecurityLogo.AccessibleRole = System.Windows.Forms.AccessibleRole.Graphic
        Me.SecurityLogo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SecurityLogo.Image = Global.Firefly.My.Resources.Resources.securitylogo
        Me.SecurityLogo.Location = New System.Drawing.Point(12, 12)
        Me.SecurityLogo.Name = "SecurityLogo"
        Me.SecurityLogo.Size = New System.Drawing.Size(479, 111)
        Me.SecurityLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.SecurityLogo.TabIndex = 7
        Me.SecurityLogo.TabStop = False
        '
        'stop_button
        '
        Me.stop_button.AccessibleDescription = "Pauses a running scan."
        Me.stop_button.AccessibleName = "Pause Button"
        Me.stop_button.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.stop_button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.stop_button.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.stop_button.Image = Global.Firefly.My.Resources.Resources.Pause
        Me.stop_button.Location = New System.Drawing.Point(89, 479)
        Me.stop_button.Name = "stop_button"
        Me.stop_button.Size = New System.Drawing.Size(81, 78)
        Me.stop_button.TabIndex = 4
        Me.stop_button.Text = "Pause"
        Me.stop_button.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'run_button
        '
        Me.run_button.AccessibleDescription = "Starts Firefly scanning."
        Me.run_button.AccessibleName = "Run Button"
        Me.run_button.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.run_button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.run_button.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.run_button.Image = Global.Firefly.My.Resources.Resources.Play
        Me.run_button.Location = New System.Drawing.Point(12, 479)
        Me.run_button.Name = "run_button"
        Me.run_button.Size = New System.Drawing.Size(71, 78)
        Me.run_button.TabIndex = 0
        Me.run_button.Text = "Scan"
        Me.run_button.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'MainForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(503, 569)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Firefly_Status)
        Me.Controls.Add(Me.ScanProgress)
        Me.Controls.Add(Me.stop_button)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Firefly_progress)
        Me.Controls.Add(Me.exit_button)
        Me.Controls.Add(Me.run_button)
        Me.Controls.Add(Me.IntroText)
        Me.Controls.Add(Me.SecurityLogo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Menu = Me.MainMenu1
        Me.Name = "MainForm"
        Me.Text = "Firefly"
        CType(Me.SecurityLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "Click Event Hanlders"

    Private Sub HideIntroduction()
        If IntroText.Visible = True Then
            IntroText.Visible = False
            SecurityLogo.Visible = False
            Me.Height = Me.Height - IntroText.Height - SecurityLogo.Height
        End If
    End Sub

    Private Sub ShowIntroduction()
        If IntroText.Visible = False Then
            IntroText.Visible = True
            SecurityLogo.Visible = True
            Me.Height = Me.Height + IntroText.Height + SecurityLogo.Height
        End If

    End Sub

    Private Sub run_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles run_button.Click
        HideIntroduction()
        If FireflyScanStage = ScanStages.Stopped Then
            Scan_Start()
        Else ' Firefly is already running.
            DisplayAndLogError(New Exception("A scan was requested but did not run because " + My.Application.Info.ProductName + " is unable to start a new scan while another scan is in progress."))
        End If
    End Sub

    Private Sub stop_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stop_button.Click
        UpdateInterfaceMessage("Stopping worker thread...", StatusMedia.Status, Now(), True)
        SendStopSignal(False)
    End Sub

    Private Sub exit_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exit_button.Click
        If FireflyScanStage = ScanStages.Stopped Then
            System.Environment.Exit(0)
        Else
            SendStopSignal(True)
        End If
    End Sub

    Private Sub exit_menu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exit_menu.Click
        If FireflyScanStage = ScanStages.Stopped Then
            System.Environment.Exit(0)
        Else
            SendStopSignal(True)
        End If
    End Sub

    Private Sub settings_menu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles settings_menu.Click
        ' Dim frm2 As New Form2
        ' frm2.Show()
        Dim prefs As New Preferences
        prefs.Show()
    End Sub

    Private Sub about_menu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles about_menu.Click
        ShowIntroduction()
        Dim frm3 As New about_Firefly
        frm3.Show()
    End Sub
    Public Sub DisplayReport(ByVal pPath As String)
        '' ShowReportForm(enc_log_plaintext, pPath, True)
        If pPath = ReportPath Then
            If FireflyReportOptions And ReportOptions.HtmlFormat Then
                pPath += ".htm"
            ElseIf FireflyReportOptions And ReportOptions.CsvReport Then
                pPath += ".csv"
            Else
                pPath += ".txt"
            End If
        End If
        Try
            System.Diagnostics.Process.Start(pPath)
        Catch ex As Exception
            DisplayAndLogError(New Exception("Unable to display the " + My.Application.Info.ProductName + " report at " + pPath, ex))
        End Try
    End Sub
    'Public Sub DisplayReport_Old(ByVal pPath As String)
    '    Dim path As String
    '    Dim FBD As New System.Windows.Forms.OpenFileDialog
    '    ' Dim oFile As System.IO.File
    '    Dim oRead As System.IO.StreamReader
    '    Dim i As Integer
    '    Dim dStreams() As String
    '    Dim nStream As String = ""
    '    ' Dim fSelect As String

    '    '   Debug.WriteLine("starting path: " + ExpandedReportPath)

    '    FBD.Title = "Select a file"
    '    FBD.CheckFileExists = True

    '    ' FBD.FileName = "C:\Firefly.LOG"

    '    '' see if we're dealing with an ADS log
    '    'dStreams = ExpandedReportPath.Split(System.IO.Path.DirectorySeparatorChar)
    '    'fSelect = dStreams(dStreams.Length - 1)
    '    ''       Debug.WriteLine("filename: " + fSelect)
    '    'If InStr(fSelect, ":") Then
    '    '    dStreams = fSelect.Split(":")
    '    '    '       Debug.WriteLine("log path contains an ADS reference")
    '    '    fSelect = ExpandedReportPath.TrimEnd(dStreams(1).ToCharArray)
    '    '    fSelect = fSelect.TrimEnd(":")
    '    'Else
    '    '    fSelect = ExpandedReportPath
    '    'End If

    '    ''      Debug.WriteLine("start path now: " + fSelect)

    '    'If IO.File.Exists(fSelect) Then
    '    '    FBD.FileName = fSelect
    '    'Else
    '    '    FBD.FileName = "C:\Firefly.LOG"
    '    'End If

    '    'i = FBD.ShowDialog()

    '    'path = FBD.FileName

    '    'If i = Windows.Forms.DialogResult.Cancel Then
    '    '    Return
    '    'End If
    '    path = pPath

    '    If path = String.Empty Then
    '        MsgBox("Path to file is empty. Unable to open.")
    '        Return
    '    End If

    '    ' read the first line of the file and see if it is "ENCRYPTED
    '    ' if so, pop up the password prompt and continue
    '    If Not (System.IO.File.Exists(path)) Then
    '        MsgBox("File does not exist.", MsgBoxStyle.Information, My.Application.Info.ProductName)
    '        Return
    '    End If

    '    ' check to see if there are alternate data streams beyond this file.
    '    ' if there are, pop up a dialog and allow the user to select the one they 
    '    ' want
    '    ' 
    '    ' at that point, open the ADS and proceed normally.
    '    Try
    '        dStreams = ADSFile.GetStreams(path)
    '        If dStreams.Length > 0 Then
    '            ' we have alternate data streams.
    '            ' pop up a selection box.
    '            Dim LB As New AdsForm
    '            For i = 0 To (dStreams.Length - 1)
    '                LB.ads_listbox.Items.Add(dStreams(i))
    '            Next
    '            LB.ads_listbox.SelectionMode = SelectionMode.One
    '            LB.ads_listbox.Sorted = True
    '            i = LB.ShowDialog()
    '            If i = Windows.Forms.DialogResult.OK Then
    '                nStream = LB.ads_listbox.SelectedItem
    '            End If
    '            If i = Windows.Forms.DialogResult.Cancel Then
    '                nStream = ""
    '                Return
    '            End If
    '            If i = Windows.Forms.DialogResult.Ignore Then
    '                nStream = ""
    '            End If
    '        End If
    '    Catch ex As Exception
    '        LogError(New ScanFailedException("Unable to open AlternateDataStream.", ex, pPath))
    '    End Try

    '    ' if we've got an ADS selected, we'll have to deal with that.

    '    If Not (FireflyReportOptions And Configuration.ReportOptions.EncryptReport) Then
    '        If nStream = String.Empty Then
    '            Try
    '                oRead = File.OpenText(path)
    '            Catch ex As Exception
    '                LogError(New ScanFailedException("Unable to open file.", ex, path))
    '                Return
    '            End Try
    '            enc_log_plaintext = oRead.ReadToEnd
    '            oRead.Close()
    '        Else
    '            ' ADS, deal with that
    '            Try
    '                oRead = ADSFile.OpenText(path, nStream)
    '            Catch ex As Exception
    '                Return
    '            End Try
    '            enc_log_plaintext = oRead.ReadToEnd
    '            oRead.Close()
    '        End If
    '    Else
    '        '           Debug.WriteLine("prepping password")
    '        ' display the file
    '        ' deal with the encrypted file;
    '        ' pop up the password prompt
    '        Dim frm4 As New PasswordSetForm
    '        frm4.password_label.Text = "Please supply the password that was used to encrypt this log file"
    '        frm4.password_confirm_box.Enabled = False
    '        i = frm4.ShowDialog()
    '        If i = Windows.Forms.DialogResult.OK Then
    '            PASSWORD = frm4.password_box.Text
    '        Else
    '            PASSWORD = ""
    '        End If
    '        enc_log_plaintext = ""
    '        ' EncryptReport will sort it out
    '        If nStream <> String.Empty Then
    '            path += ":" + nStream
    '        End If

    '        EncryptReport(path, Scanner.CryptoAction.Decrypt)
    '        ' we've got a pile of text.
    '    End If
    '    ' Debug.WriteLine("will show: " + enc_log_plaintext)
    '    ShowReportForm(enc_log_plaintext, path, False)

    'End Sub
    Private Sub MenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem1.Click
        Start(Firefly_LICENSE_URL)
        Return
    End Sub
    Private Sub MenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem2.Click
        ' display a box containing Firefly's command-line arguments
        Dim helpInfo As String
        helpInfo = My.Application.Info.ProductName + " command line arguments:" + vbCrLf
        Dim item As String
        For Each item In cmdLineOpts
            helpInfo += item + vbCrLf
        Next
        MsgBox(helpInfo, MsgBoxStyle.OkOnly, My.Application.Info.ProductName)

    End Sub
    Private Sub full_help_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles full_help.Click
        Start(Firefly_HELP_URL)
        Return
    End Sub
    Private Sub configure_menu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles configure_menu.Click

    End Sub
    Private Sub ConfigureSetRunPriority_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConfigureSetRunPriority.Click
        Dim frmPriority As New Pref_RunPriority
        frmPriority.Show()
    End Sub
    Private Sub ConfigureFileTypes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConfigureFileTypes.Click
        Dim frmFileTypes As New FileTypesForm
        frmFileTypes.Show()
    End Sub
    Private Sub NewSearchMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewSearchMenu.Click
        Dim frmLocation As New SearchLocation()
        frmLocation.Show()
    End Sub
    Private Sub RestoreDefaultsMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RestoreDefaultsMenu.Click
        Configuration.default_config()
        Configuration.save_config()
    End Sub
    Private Sub DisplayReportMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisplayReportMenu.Click
        DisplayReport(ReportPath)
    End Sub
    Private Sub DisplayLogMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DisplayLogMenu.Click
        DisplayReport(AppLogPath)
    End Sub

#End Region

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ' MsgBox("Security report sent: " + SendSecurityReport().ToString())
        SendSecurityReport()
    End Sub

#Region "Scanning Functions"
    Public Sub scan_main()

        Dim filelist As New ArrayList

        Dim ScanWeb As Boolean = False

        Firefly_start = DateTime.Now

        DisplayLogMenu.Enabled = False
        DisplayReportMenu.Enabled = False
        NewSearchMenu.Enabled = False

        UpdateInterfaceMessage("Preparing report files.", StatusMedia.Status, Now())
        ' Initialize log and reports files.
        If Not PrepareReportFiles() Then
            UpdateInterfaceMessage("Unable to prepare report files.", StatusMedia.Status, Now(), True)
            Scan_Abort()
            Return
        End If

        UpdateInterfaceMessage("Selecting a scan mode.", StatusMedia.Status, Now())

        ' OK, now we delegate by scan type

        If (FireflyRunMode And Configuration.RunMode.Disk) Then
            UpdateInterfaceMessage("Starting local disk scan.", StatusMedia.Status, Now(), True)
            disk_mode()
            'ElseIf (FireflyRunMode And Configuration.RunMode.Hidden) Then
            '    WriteToLog("Scanning hidden sections of the disk.")
            '    UpdateInterfaceMessage("Scanning hidden sections of the disk...", StatusMedia.Status Or StatusMedia.Status, True)
            '    hidden_mode()
            'ElseIf (FireflyRunMode And Configuration.RunMode.Web) Then
            '    WriteToLog("Scanning in website mode.")
            '    UpdateInterfaceMessage("Scanning a Web location...", StatusMedia.Status Or StatusMedia.Status, True)
            '    web_mode()
        Else
            LogError(New Exception("Unknown scan mode. Scan aborted."))
            Firefly_end = DateTime.Now
            UpdateInterfaceMessage("Unknown scan mode. Scan aborted.", StatusMedia.Status, Now(), True)
            Scan_Abort()
            Return
        End If

        If (FireflyReportOptions And Configuration.ReportOptions.EncryptReport) Then
            EncryptReport(ReportPath, Scanner.CryptoAction.Encrypt)
        End If

        Firefly_end = DateTime.Now

    End Sub

    Public Sub SendStopSignal(ByVal exitWhenFinished As Boolean)
        ThreadTracking.StopRequested = True
        If exitWhenFinished Then ThreadTracking.ExitRequested = True
    End Sub


    Public Sub Scan_Abort()
        ' Do not attempt to resume this scan.
        My.Settings.ScanInProgress = False

        WriteToLog("Scan cancelled at " + Now() + ".")

        Scan_IsStopped()
    End Sub

    Public Sub Scan_Pause(ByVal targetList As ArrayList, ByVal ppath As String)
        ' The user has requested we stop.
        WriteToLog("The user interrupted the scan at " + Now())
        If (FireflyScanDiskOptions And DiskModeOptions.SaveAndResumeSearch) Then
            WriteToLog("Saving list of files to scan later.")
            UpdateInterfaceMessage("Saving scan progress...", StatusMedia.Status, Now(), True)
            SaveFileList(targetList, ppath)
            My.Settings.ScanInProgress = True
            My.Settings.ScanPaused = True
            UpdateInterfaceMessage("Scan progress saved.", StatusMedia.Status, Now(), True)
            WriteToLog("Scan paused at " + Now() + ".")
            UpdateInterfaceMessage("Scan paused.", StatusMedia.Status, Now(), True)
            Scan_IsStopped()
        Else
            UpdateInterfaceMessage("Scan cancelled. Saving progress is disabled.", StatusMedia.Status, Now(), True)
            Scan_Abort()
        End If

    End Sub

    Public Sub Scan_IsStopped()

        UpdateInterfaceMessage("", StatusMedia.Progress, Now(), True)

        DisplayLogMenu.Enabled = True
        DisplayReportMenu.Enabled = True
        NewSearchMenu.Enabled = True

        SetWindowStateNormal_ThreadSafe()

        ' files_processed = 0
        My.Settings.TotalFilesScanned = 0

        If ThreadTracking.ExitRequested Then
            If Not FireflyScanStage = ScanStages.SavingFileList Then
                My.Settings.Save()
                Application.Exit()
            Else
                LogError(New Exception("Firefly failed to close because it was busy saving the unscanned files list."))
            End If
        End If

        FireflyScanStage = ScanStages.Stopped

        UpdateInterfaceButtons_ThreadSafe()
        ThreadTracking.StopRequested = False

    End Sub

    Public Sub Scan_Completed()
        UpdateInterfaceMessage("Scan completed.", StatusMedia.Status, Now(), True)
        WriteToLog("Finished scanning files on " + Now())

        ' Do not attempt to resume this scan during the next run.
        My.Settings.ScanInProgress = False

        ' Write and send the security report.
        If FireflyReportOptions And ReportOptions.SendSecurityReport Then
            Try
                WriteSecurityReport_Footer()
            Catch ex As Exception
                LogError(New Exception("Unable to create the security report.", ex))
            End Try
            Try
                SecurityReportSent = SendSecurityReport()
            Catch ex As Exception
                DisplayAndLogError(New Exception("Unable to send the report to CITES Security.", ex), True)
            End Try
        Else
            WriteToLog("Firefly's is configured not to send the security report.")
        End If

        ' Write the local report.
        WriteReport_Footer()
        BackupReport()

        ' Display the report.
        If (FireflyRunOptions And Configuration.RunOptions.LaunchLogViewer) Then
            DisplayReport(ReportPath)
        End If

        ' Exit when scan completes.
        If (FireflyRunOptions And Configuration.RunOptions.ExitWhenDone) Then
            UpdateInterfaceMessage("Exiting...", StatusMedia.Status, Now(), True)
            System.Environment.Exit(0)
        Else
            UpdateInterfaceProgress(My.Settings.TotalFilesScanned, My.Settings.TotalFilesScanned, Now())
            UpdateInterfaceMessage("Scan completed.", StatusMedia.Status, Now(), True)
        End If

        Scan_IsStopped()
    End Sub

    Public Sub Scan_Start()

        '' Prompt for Password
        If (FireflyReportOptions And Configuration.ReportOptions.EncryptReport) Then
            ' for now, can't do this unattended
            If (FireflyRunOptions And Configuration.RunOptions.Unattended) Then
                ' MsgBox("Unable to prompt for password in unattended mode.", MsgBoxStyle.OkOnly, My.Application.Info.ProductName)
                DisplayAndLogError(New Exception("Unable to prompt for password in unattended mode."))
                System.Environment.Exit(0)
            End If
            ' need to get a password
            Dim resp As DialogResult
            Dim frm4 As New PasswordSetForm
            frm4.password_label.Text = "Enter a password that will be used to encrypt the Firefly log."
            Dim screwup As Integer = 1
            While screwup <= 3
                resp = frm4.ShowDialog
                ' Debug.WriteLine("PasswordSetForm:" + frm4.password_box.Text)
                ' Debug.WriteLine("response: " + resp.ToString)
                '            Dim screwup As Integer = 1
                If resp = Windows.Forms.DialogResult.OK Then
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
                MsgBox("Cannot run " + My.Application.Info.ProductName + " search without a password to encrypt its log file.", MsgBoxStyle.OkOnly, My.Application.Info.ProductName)
                Return
            End If
        End If

        '' Start a new Firefly thread.

        Dim t As New Thread(AddressOf Me.scan_main)
        t.SetApartmentState(ApartmentState.STA)
        ' t.SetApartmentState(ApartmentState.MTA)
        ' files_processed = 0
        If t.IsAlive Then
            LogError(New Exception("Scanning thread is already active."))
            t.Abort()
        End If
        ''TODO: Handle this check gracefully, rather than simply suppressing.
        'If t.ThreadState = Threading.ThreadState.Stopped Then
        '    'Dim t As New Thread(AddressOf scan_main)
        '    Return
        'End If
        t.IsBackground = True

        UpdateInterfaceButtons()

        '' Minimize Firefly
        If (FireflyRunOptions And Configuration.RunOptions.Minimize) Then
            Me.WindowState = Minimized
        End If

        '' Set Firefly run priority level
        If (My.Settings.FireflyRunPriority = RunPriority.Normal) Then
            t.Priority = ThreadPriority.Normal
        ElseIf (My.Settings.FireflyRunPriority = RunPriority.High) Then
            t.Priority = ThreadPriority.AboveNormal
            WriteToLog("Scan will start with above normal run priority.")
        ElseIf (My.Settings.FireflyRunPriority = RunPriority.Highest) Then
            t.Priority = ThreadPriority.Highest
            WriteToLog("Scan will start with highest run priority.")
        Else
            t.Priority = ThreadPriority.BelowNormal
            WriteToLog("Scan will start with lowest run priority.")
        End If

        t.Start()

    End Sub


    Public Sub process_files(ByVal targetlist As ArrayList)
        ' we'll iterate over the arraylist calling process_plain_text() for each file
        Dim pPath As String
        ' Dim origAtime As Date

        ' Update the status again after processing ReportAfter files.
        '  Don't set this number to zero. Bad things would happen.
        Dim ReportAfter As Integer = 1
        Dim files_processed As Integer = 1
        ' Now we are scanning.
        FireflyScanStage = ScanStages.Scanning
        UpdateInterfaceButtons_ThreadSafe()

        ' Display how many files we're scanning.
        My.Settings.TotalFilesScanned = targetlist.Count
        UpdateInterfaceMessage("Scanning " + My.Settings.TotalFilesScanned.ToString() + " files.", StatusMedia.Status, Now(), True)

        If FireflyReportOptions And ReportOptions.HtmlFormat Then
            WriteToLog(My.Application.Info.ProductName + " found " + My.Settings.TotalFilesScanned.ToString() + " files to scan.")
        End If

        While targetlist.Count > 0
            pPath = targetlist.Item(0)

            ' For Each pPath In targetlist

            If CheckForStopSignal() Then
                ' Save scan progress.
                Scan_Pause(targetlist, pPath)
                Exit Sub
            End If

            ' UpdateInterfaceMessage("Processing file " + files_processed.ToString + " of " + total_files.ToString, StatusMedia.MainFormCaption)
            UpdateInterfaceMessage(pPath.ToString, StatusMedia.Progress, Now())
            ' System.Threading.CoWaitForMultipleHandles(pPath) 

            'Try
            '    origAtime = IO.File.GetLastAccessTime(pPath)
            'Catch
            'End Try

            Dim startTime As Date = Now()
            Delegate_File_Ext(pPath)
            Dim finishTime As Date = Now()
            Dim longTime As New TimeSpan(0, 0, 30)
            Dim timeTaken As TimeSpan = finishTime - startTime
            If timeTaken > longTime Then
                WriteToLog("This file took greater than 30 seconds to scan: " + pPath)
            End If
            targetlist.Remove(pPath)


            'If (FireflyHiddenModeOptions And Configuration.HiddenModeOptions.ScanSlack) And (FireflyRunMode And RunMode.Hidden) Then
            '    process_slack(pPath)
            'End If

            'Try
            '    IO.File.SetLastAccessTime(pPath, origAtime)
            'Catch ex As Exception
            '    LogError(New Exception("Unable to reset last access time on " + pPath, ex))
            'End Try

            'If (FireflyLogOptions And Configuration.LogOptions.LogProgress) Then
            '    If target_count Mod 1000 = 0 Then
            '        finished = Int(target_count / targetlist.Count) * 100
            '        send_Evt_log(My.Application.Info.ProductName + " is " + finished.ToString + "% finished with " + targetlist.Count.ToString + " files")
            '    End If
            'End If

            files_processed += 1
            If files_processed Mod ReportAfter = 0 Then
                UpdateInterfaceProgress(files_processed, My.Settings.TotalFilesScanned, Now())
            End If
        End While

        Scan_Completed()
        Return
    End Sub
    'Public Sub process_slack(ByVal pPath As String)
    '    Dim fSize As Int64 = 0
    '    Dim nSize As Int64 = 0
    '    Dim depth As Int64 = 0
    '    Dim deltaS As Int64 = 0
    '    Dim nRead As Integer = 0
    '    Dim cRead(65536) As Char
    '    Dim hit As String = ""
    '    Dim frag As String = ""
    '    ' Dim fAttr As FileAttribute
    '    ' Dim r As Integer
    '    Dim ret As Long
    '    Dim HitItems As New Hashtable

    '    Dim matches As New Scanner.MatchesInFile(pPath)

    '    HitItems.Clear()

    '    Dim drive As String = pPath.Split(":")(0)
    '    drive += ":"
    '    If DriveBlocksize.Contains(drive) Then
    '        HDD_BLOCKSIZE = DriveBlocksize.Item(drive)
    '        HDD_FREESPACE = DriveFreespace.Item(drive)
    '    Else
    '        get_disk_properties(drive)
    '    End If

    '    fSize = FileLen(pPath)
    '    deltaS = fSize - (Int(fSize / HDD_BLOCKSIZE) * HDD_BLOCKSIZE)
    '    nSize = fSize - deltaS + HDD_BLOCKSIZE
    '    Debug.WriteLine("Old size: " + fSize.ToString + " new size: " + nSize.ToString)
    '    Loc = ""
    '    depth = 0
    '    'fAttr = System.IO.File.GetAttributes(pPath)
    '    If Not SMVN_priv Then
    '        AdjustTokenDisk()
    '    End If

    '    If fSize > 800 And deltaS <> 0 Then
    '        Try
    '            Dim oP As New FileStream(pPath, FileMode.Open, FileAccess.ReadWrite)
    '            '       System.IO.File.SetAttributes(pPath, FileAttributes.SparseFile)
    '            Dim rP As New StreamReader(oP)
    '            ' here's what we'll do:
    '            ' pad the file size off to the next multiple of the block size
    '            ' we'll call setfilevalidata and setendoffile to cement that,
    '            ' then we'll scan the slack space we've found.
    '            ret = SetFilePointer(oP.Handle, nSize, 0, Convert.ToUInt32(FILE_BEGIN))
    '            If ret = 0 Then
    '                Debug.WriteLine("err: " + GetLastError.ToString)
    '                oP.Close()
    '                Return
    '            End If
    '            If Not SetEndOfFile(oP.Handle) Then
    '                Debug.WriteLine("setEOF: " + GetLastError.ToString)
    '                oP.Close()
    '                Return
    '            End If
    '            If Not SetFileValidData(oP.Handle, Convert.ToInt64(nSize - 1)) Then
    '                Debug.WriteLine("setfilevaliddata: " + GetLastError.ToString)
    '                oP.Close()
    '                Return
    '            End If
    '            ' set our start position to the previous EOF
    '            rP.BaseStream.Position = fSize
    '            While rP.Peek <> -1
    '                nRead = rP.ReadBlock(cRead, 0, READ_SIZE)
    '                ' HandleStopSignal(Threading.Thread.CurrentThread, targetlist, pPath)
    '                If ThreadTracking.StopRequested Then
    '                    StopScanning(Thread.CurrentThread)
    '                    Exit While
    '                End If

    '                Debug.WriteLine("Read: " + nRead.ToString)
    '                depth += nRead
    '                Debug.WriteLine("Slack: " + Convert.ToString(cRead))
    '                GetMatches(Convert.ToString(cRead), matches)
    '                If matches.HasMatches Then
    '                    If Loc = String.Empty Then
    '                        Loc = "Slack Offset: " + depth.ToString
    '                    End If
    '                    If (FireflyRunOptions And Configuration.RunOptions.FastMatch) Then
    '                        hit = HitItems("0")
    '                    Else
    '                        ' Dim i As Integer
    '                        For i As Integer = 0 To 512
    '                            If Not HitItems.Contains(i.ToString) Then
    '                                Exit For
    '                            End If
    '                            hit += HitItems(i.ToString) + " # "
    '                        Next
    '                    End If
    '                    ReportMatch(matches)
    '                End If
    '            End While
    '            ' restore the file size
    '            ret = SetFilePointer(oP.Handle, fSize, 0, Convert.ToUInt32(FILE_BEGIN))
    '            If ret = 0 Then
    '                Debug.WriteLine("err: " + GetLastError.ToString)
    '                oP.Close()
    '                Return
    '            End If
    '            If Not SetEndOfFile(oP.Handle) Then
    '                Debug.WriteLine("setEOF: " + GetLastError.ToString)
    '                oP.Close()
    '                Return
    '            End If
    '            If Not SetFileValidData(oP.Handle, Convert.ToInt64(fSize)) Then
    '                Debug.WriteLine("setfilevaliddata: " + GetLastError.ToString)
    '                oP.Close()
    '                Return
    '            End If
    '            oP.Close()
    '        Catch ex As Exception
    '            Debug.WriteLine("Exception setting file size: " + ex.ToString)
    '        End Try
    '    Else
    '        Debug.WriteLine("skipping either resident data or integer blocksize file")
    '    End If
    'End Sub


    Public Sub disk_mode()
        ' Dim drivelist() As String
        Dim filelist As ArrayList = New ArrayList
        Dim tList As New ArrayList
        ' Dim i As Integer
        Dim recurse As Boolean
        recurse = FireflyScanDiskOptions And Configuration.DiskModeOptions.Recurse

        ' based on our disk mode options, accumulate a list of files to be 
        ' scanned, then scan them, 

        ' GUI status update only when not running 'unattended' or 'NoPrompts'.
        Try
            If FireflyScanDiskOptions And Configuration.DiskModeOptions.ScanFileList Then
                ScanList()
                Return
            End If

            ' If we have a list of unprocessed files, process them.
            If My.Settings.ScanInProgress Then
                UpdateInterfaceMessage("Resuming scan...", StatusMedia.Status, Now(), True)
                process_files(LoadFileList(UnprocessedFilesPath))
            Else

                Try
                    If System.IO.File.Exists(UnprocessedFilesPath) Then
                        System.IO.File.Delete(UnprocessedFilesPath)
                    End If
                Catch ex As Exception
                    DisplayAndLogError(New Exception(My.Application.Info.ProductName + " was unable to clear the unprocessed files list at " + UnprocessedFilesPath + ". " + My.Application.Info.ProductName + " may scan more files than intended if this scan is paused and resumed.", ex))
                End Try

                My.Settings.FilesWithMatches = 0

                ' Create the list of files to scan.
                FireflyScanStage = ScanStages.BuildingFileList
                UpdateInterfaceMessage("Building the list of files to scan.", StatusMedia.Status, Now(), True)

                Dim drivesList As String = ""
                Dim drivesToScan As Collection = New Collection
                If (FireflyScanDiskOptions And Configuration.DiskModeOptions.AllLocalDrives) Then
                    '' Scan all local Drives
                    '' drivelist = Directory.GetLogicalDrives()
                    Dim drives() As System.IO.DriveInfo = DriveInfo.GetDrives()
                    ' Dim driveName As String

                    For Each drive As DriveInfo In drives
                        If drive.DriveType = DriveType.Fixed Then
                            drivesToScan.Add(drive.Name)
                            drivesList += drive.Name + " "
                        End If
                    Next
                    'Else
                    '    ' Not scanning all drives, see if we were given a list of drive letters.
                    '    Dim rListOfDrives As Regex = New Regex("^(\w:?\s\\?)")
                    '    If rListOfDrives.Match(My.Settings.ScanDir).Success Then
                    '        For Each i In rListOfDrives.Match(My.Settings.ScanDir).Captures
                    '            drivesList += i.ToString
                    '            drivesToScan.Add(i.ToString)
                    '        Next
                    '    End If
                End If

                If drivesList <> "" Then
                    If (FireflyRunOptions And RunOptions.Unattended) = 0 Then
                        If MsgBox("The following drives will be scanned: " + drivesList, MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
                            UpdateInterfaceMessage("Scan cancelled by user request.", StatusMedia.Status, Now(), True)
                            Scan_Abort()
                            Exit Sub
                        End If
                    End If
                    If FireflyReportOptions And ReportOptions.HtmlFormat Then
                        WriteToLog(My.Application.Info.ProductName + " is scanning the following drives: " + drivesList)
                    End If

                    My.Settings.ScanDir = drivesList

                    For Each driveName As String In drivesToScan
                        'If drive.DriveType = DriveType.Fixed Then
                        'driveName = drive.Name
                        'For Each driveletter As String In drivelist
                        Try
                            UpdateInterfaceMessage("Creating a list of files to scan on drive " + driveName.ToString(), StatusMedia.Status, Now())
                            ' driveletter += "\"
                            ' Find all files on this drive.
                            filelist.AddRange(BuildFileList(driveName))
                            ' UpdateInterfaceMessage("Processing files from all local drives...", StatusMedia.Status)
                        Catch abortEx As ThreadAbortException
                            If FireflyScanStage = ScanStages.BuildingFileList Then
                                LogError(New Exception(My.Application.Info.ProductName + " was interupted while creating the list of files to scan.", abortEx))
                            Else
                                LogError(New Exception("An unexpected error occurred while scanning.", abortEx), True)
                            End If
                        Catch ex As Exception
                            LogError(New Exception("An error occurred while creating the list of files to scan.", ex))
                            Return
                        End Try

                        'End If

                        'Next
                    Next
                    Try
                        My.Settings.TotalFilesScanned = filelist.Count
                        If ThreadTracking.StopRequested Then
                            UpdateInterfaceMessage("Scan cancelled while building the file list.", StatusMedia.Status, Now(), True)
                            Scan_Abort()
                            Exit Sub
                        Else
                            process_files(filelist)
                        End If
                    Catch ex As Exception
                        ' We really hope this doesn't come up.
                        DisplayAndLogError(New Exception("An unhandled error occurred while processing files. Consult the log file for details."), True)
                    End Try

                Else ' Scan a single location.

                    If System.IO.File.Exists(My.Settings.ScanDir) And Not System.IO.Directory.Exists(My.Settings.ScanDir) Then
                        ' If it's a file rather than a directory, just scan that file.
                        filelist.Add(My.Settings.ScanDir)
                        process_files(filelist)
                    Else
                        ' Find all files in our start directory.
                        filelist = BuildFileList(My.Settings.ScanDir)
                        If ThreadTracking.StopRequested Then
                            UpdateInterfaceMessage("Scan cancelled while building the file list.", StatusMedia.Status, Now(), True)
                            Scan_Abort()
                            Exit Sub
                        End If
                        UpdateInterfaceMessage("Scanning all files in " + My.Settings.ScanDir, StatusMedia.Status, Now())
                        My.Settings.TotalFilesScanned = filelist.Count
                        process_files(filelist)
                    End If
                End If

            End If

            Return

        Catch ex As Exception
            LogError(New Exception("An unhandled error occurred while scanning.", ex), True)
        End Try

    End Sub

    Private Sub ScanList()
        ' Scan a list of files, rather than building a list.
        'Dim filelist As ArrayList = New ArrayList
        'Dim tList As New ArrayList
        'Dim i As Integer

        If System.IO.File.Exists(My.Settings.ScanDir) And Not System.IO.Directory.Exists(My.Settings.ScanDir) Then

            process_files(LoadFileList(My.Settings.ScanDir))

            '' We were passed a file instead of a directory: " + My.Settings.StartDir
            '' Assume each line names a directory and search each one.
            'Try
            '    Dim oRead As System.IO.StreamReader
            '    oRead = File.OpenText(My.Settings.ScanDir)
            '    While (oRead.Peek <> -1)
            '        tList.Add(oRead.ReadLine)
            '    End While
            '    oRead.Close()
            'Catch ex As Exception
            '    LogError(New Exception("Unable to read the list of files to search in " + My.Settings.ScanDir, ex))
            'End Try

            'WriteToLog("Scanning the drive locations listed in the file: " + My.Settings.ScanDir)

            'For i = 0 To tList.Count - 1
            '    UpdateInterfaceMessage("Acquiring file list from: " + tList.Item(i), StatusMedia.Status, Now())
            '    ' Find all files in this directory from the list.
            '    filelist = BuildFileList(tList.Item(i))
            '    ' UpdateInterfaceMessage(RunButton_Pause, StatusMedia.MainFormRunButton)
            '    My.Settings.TotalFilesScanned = filelist.Count
            '    process_files(filelist)
            '    filelist.Clear()
            'Next
        End If
    End Sub

    Public Sub web_mode()

        ' failsafe
        '  Return

        URLLoopDetect.Clear()
        UpdateInterfaceMessage("Scanning " + WebModeStartURL, StatusMedia.Status, Now(), True)

        ' http/https scanning here
        ' need to figure out the start domain
        Dim fooURI As New Uri(WebModeStartURL)
        ' if DomainDepth is set, craft a tmp host and send that over
        Dim tmpHost As String
        'Debug.WriteLine("DOMAINDEPTH: " + DomainDepth.ToString)
        tmpHost = ""
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
                    '          Debug.WriteLine("PIECE: " + tmpParts(j))
                    tmpHost += "." + tmpParts(j)
                Next
            End If
        End If
        tmpHost = tmpHost.TrimStart(".")
        ' Debug.WriteLine("TMPHOST: " + tmpHost)

        LogError(New Exception("A scan of a website was requested, but " + APP_NAME + " does not support website scanning."))
        ' process_http(WebModeStartURL, 0, tmpHost)
        Scan_Completed()

        Return

    End Sub
    Public Sub hidden_mode()

        'If (FireflyHiddenModeOptions And Configuration.HiddenModeOptions.ScanSlack) Then
        '    disk_mode()
        'End If

        LogError(New Exception("A scan of unallocated disk space was requested, but " + APP_NAME + " does not yet support this feature."))
    End Sub


    'Public Sub process_unallocated(ByVal drive As String)
    '    ' pretty simple.  we'll create a new zero-length temporary file
    '    ' we'll then extend that file in multiples of the blocksize, reading as we go
    '    ' Dim tempChunk = System.IO.Path.GetTempFileName
    '    Dim nRead As Integer
    '    Dim offT As Int64 = 0
    '    Dim scanT As Int64 = 0
    '    Dim cRead(1024 * 1024) As Char
    '    Dim hit As String = ""
    '    Dim frag As String = ""
    '    Dim READ_SIZE As Integer
    '    Dim mult As Integer
    '    '        Dim Loc As String = ""
    '    Dim Chunk As String
    '    Dim BlowChunk As String
    '    Dim HitItems As New Hashtable

    '    Dim matches As New Scanner.MatchesInFile(drive)

    '    HitItems.Clear()

    '    drive = drive.TrimEnd("\")

    '    If DriveBlocksize.Contains(drive) Then
    '        HDD_BLOCKSIZE = DriveBlocksize.Item(drive)
    '        HDD_FREESPACE = DriveFreespace.Item(drive)
    '    Else
    '        '            Dim drive As String = pPath.Split(":")(0)
    '        get_disk_properties(drive)
    '    End If

    '    ' figure the largest multiple of the blocksize that'll fit within freespace and that's
    '    ' what we'll read
    '    mult = Int((1024 * 1024) / HDD_BLOCKSIZE)

    '    If mult = 0 Then
    '        Return
    '    End If

    '    Dim i As Integer

    '    For i = mult To 1 Step -1
    '        Debug.WriteLine("trying: " + (i * HDD_BLOCKSIZE).ToString)
    '        If (HDD_FREESPACE Mod (i * HDD_BLOCKSIZE) = 0) Then
    '            Exit For
    '        End If
    '    Next

    '    READ_SIZE = HDD_BLOCKSIZE * i

    '    Debug.WriteLine("READ SIZE: " + READ_SIZE.ToString)

    '    Chunk = drive + "\" + My.Application.Info.ProductName + ".unalloc_tmp"
    '    BlowChunk = drive + UnallocFile

    '    If System.IO.File.Exists(Chunk) Then
    '        System.IO.File.Delete(Chunk)
    '    End If

    '    Loc = ""

    '    Try
    '        ' basically works like this:
    '        ' extend the file by BLOCKSIZE,
    '        ' read the interval from the current location to EOF, scan it,
    '        ' repeat
    '        If Not SMVN_priv Then
    '            ' we need to elevate our privs, or throw an error if we can't.
    '            AdjustTokenDisk()
    '        End If
    '        Dim oP As New FileStream(Chunk, FileMode.Create, FileAccess.ReadWrite)
    '        Dim rP As New StreamReader(oP)

    '        Dim ret As Long
    '        ' unalloc to file
    '        If (FireflyHiddenModeOptions And Configuration.HiddenModeOptions.PullUnalloctoFile) Then
    '            If System.IO.File.Exists(BlowChunk) Then
    '                wipe(BlowChunk, Module2.WipeTarget.File, Module2.WipeMethod.DoD_7Pass, Module2.WipeWhenDone.Truncate)
    '            End If
    '        End If
    '        Try
    '            While Not (ThreadTracking.StopRequested)
    '                ' inefficient to process such small chunks, but we're guaranteed of 
    '                ' an integer number of blocks on the disk.
    '                ' rP.BaseStream.Position = offT + HDD_BLOCKSIZE
    '                '                    oP.SetLength(rP.BaseStream.Position + HDD_BLOCKSIZE)
    '                '  Debug.WriteLine("setting file pointer to: " + offT.ToString)
    '                ret = SetFilePointer(oP.Handle, offT, 0, Convert.ToUInt32(FILE_BEGIN))
    '                ' Debug.WriteLine("setfilepointer: " + GetLastError.ToString)
    '                ' rP.BaseStream.Position = offT

    '                If Not SetEndOfFile(oP.Handle) Then
    '                    Debug.WriteLine("setendoffile failed: " + GetLastError.ToString)
    '                End If

    '                If Not SetFileValidData(oP.Handle, Convert.ToInt64(offT)) Then
    '                    Debug.WriteLine("setfilevaliddata failed: " + GetLastError.ToString)
    '                End If

    '                ' we've grabbed a block of the file; set our file pointer back one blocksize
    '                ' to make way for reading
    '                If offT >= READ_SIZE Then
    '                    rP.BaseStream.Position -= READ_SIZE
    '                End If

    '                offT += READ_SIZE

    '                Me.Firefly_progress.Text = "Scanned: " + rP.BaseStream.Position.ToString + " of " + HDD_FREESPACE.ToString + " bytes"
    '                Me.Refresh()
    '                '                    oP.Close()
    '                nRead = rP.ReadBlock(cRead, 0, READ_SIZE)
    '                'Debug.WriteLine("read: " + nRead.ToString + " bytes")
    '                scanT += nRead
    '                'Debug.WriteLine("unalloc: " + Convert.ToString(cRead))
    '                GetMatches(Convert.ToString(cRead), matches)
    '                If matches.HasMatches Then

    '                    'If (FireflyRunOptions And Configuration.RunOptions.FastMatch) Then
    '                    '    hit = HitItems("0")
    '                    'Else
    '                    '    For i = 0 To 512
    '                    '        If Not HitItems.Contains(i.ToString) Then
    '                    '            Exit For
    '                    '        End If
    '                    '        hit += HitItems(i.ToString) + " # "
    '                    '    Next
    '                    'End If
    '                    If (FireflyHiddenModeOptions And Configuration.HiddenModeOptions.PullUnalloctoFile) Then
    '                        Try
    '                            Dim uP As New FileStream(BlowChunk, FileMode.Append, FileAccess.Write)
    '                            Dim wU As New BinaryWriter(uP)
    '                            wU.Write(cRead)
    '                            wU.Close()
    '                        Catch ex As Exception
    '                            ' not much we can do if the above fails.
    '                            LogError(New Exception("Unable to copy unallocated space to a file for reference.", ex))
    '                        End Try
    '                    End If
    '                    If (FireflyHiddenModeOptions And Configuration.HiddenModeOptions.WipeUnallocMatchingBlocks) Then
    '                        If Not wipe_block(oP.Handle, READ_SIZE, oP.Position, Module2.WipeMethod.Overwrite_with_nulls) Then
    '                            Debug.WriteLine("wipe_block failed")
    '                        End If
    '                    End If
    '                    If Loc = String.Empty Then
    '                        Loc = "Unallocated space"
    '                    End If
    '                    matches.PathToFile = "Unallocated space"
    '                    ReportMatch(matches)
    '                    ' want to rethink the following...
    '                    If (FireflyRunOptions And Configuration.RunOptions.FastMatch) Then
    '                        oP.Close()
    '                        System.IO.File.Delete(Chunk)
    '                        UpdateInterfaceMessage("Match found in unallocated space.", StatusMedia.Status, True)
    '                        StopScanning(Thread.CurrentThread)
    '                    End If
    '                End If
    '            End While
    '            oP.Close()
    '            System.IO.File.Delete(Chunk)
    '            ' FireflyRunState = FireflyRunState Or RunState.FinishedScan
    '            ' StopScanning(Thread.CurrentThread)
    '        Catch ex As Exception
    '            Debug.WriteLine("inner exception: " + ex.ToString)
    '            oP.Close()
    '            System.IO.File.Delete(Chunk)
    '        End Try
    '    Catch ex As Exception
    '        Debug.WriteLine("Exception creating new file: " + ex.ToString)
    '    End Try

    '    ' Kill(oP)

    '    Return

    'End Sub

    Public Sub SaveFileList(ByVal fileList As ArrayList, ByVal pPath As String)
        Dim i As String
        ' Clear the list.
        Try
            System.IO.File.Delete(UnprocessedFilesPath)
            Dim unProcessed As Boolean = False
            ' List all the files we were planning to get to.
            For Each i In fileList
                'If i = pPath Then unProcessed = True
                'If unProcessed Then WriteToFile(i.ToString(), UnprocessedFilesPath)
                WriteToFile(i.ToString(), UnprocessedFilesPath)
            Next
            UpdateInterfaceMessage("Scan progress saved.", StatusMedia.Status, Now())
            ProgressSaved = True
        Catch ex As Exception
            LogError(New Exception("Failed to save scan progress."))
        End Try

    End Sub
    Public Function LoadFileList(ByVal pPath As String) As ArrayList
        Dim retVal As New ArrayList
        Try
            Dim sr As New StreamReader(pPath)
            Dim line As String = sr.ReadLine()

            Do While Not line Is Nothing
                retVal.Add(line)
                line = sr.ReadLine()
            Loop

        Catch ex As Exception
            LogError(New Exception("Unable to load unprocessed files list.", ex))
        End Try
        Return retVal

    End Function

    Public Function BuildFileList(ByVal path As String) As ArrayList
        FireflyScanStage = ScanStages.BuildingFileList
        UpdateInterfaceButtons_ThreadSafe()
        Dim listFiles As New ArrayList
        BuildFileListRecursive(path, listFiles, True)
        Return listFiles
    End Function

    Private Sub BuildFileListRecursive(ByVal folderPath As String, ByVal listFiles As ArrayList, ByVal recursive As Boolean)

        Try

            If Not ShouldScanPath(folderPath) Then
                Return
            End If

            Dim fAttributes As Long
            Dim epoch As Date = Convert.ToDateTime("01/01/0001")
            ' Dim ftime As Date

            If ThreadTracking.StopRequested Then
                Exit Sub
            End If

            UpdateInterfaceMessage(folderPath, StatusMedia.Progress, Now())

            Try
                ' directory exists?
                If Not Directory.Exists(folderPath) Then
                    Return
                End If
                Directory.GetDirectories(folderPath)
            Catch ioex As IOException
                LogError(New Exception("Unable to access: " + folderPath))
            Catch ex As Exception
                LogError(New Exception("Unable to access: " + folderPath, ex))
                Return
            End Try

            ' Add subdirectories to the list.
            If recursive Then
                For Each folder As String In Directory.GetDirectories(folderPath)
                    BuildFileListRecursive(folder, listFiles, recursive)
                Next
            End If

            ' Add files in this directory to the list.
            For Each pFile As String In Directory.GetFiles(folderPath)
                Try
                    ' check attributes
                    Dim unScannable As Boolean
                    Try
                        fAttributes = System.IO.File.GetAttributes(pFile)
                        If fAttributes And IO.FileAttributes.System Then
                            ' System file
                            unScannable = True
                        ElseIf fAttributes And IO.FileAttributes.SparseFile Then
                            ' Sparse File
                            unScannable = True
                        ElseIf fAttributes And IO.FileAttributes.Offline Then
                            ' Offline file
                            unScannable = True
                        ElseIf fAttributes And IO.FileAttributes.Encrypted Then
                            ' Encrypted file
                            unScannable = True
                            UnableToScan(pFile, UnableToScanReason.FileEncrypted)
                        ElseIf fAttributes And IO.FileAttributes.Device Then
                            ' Device
                            unScannable = True
                        ElseIf fAttributes And IO.FileAttributes.Directory Then
                            ' Directory
                            unScannable = True
                        ElseIf fAttributes And IO.FileAttributes.Hidden Then
                            ' Hidden File
                            unScannable = True
                            UnableToScan(pFile, UnableToScanReason.AccessDenied)
                        Else
                            unScannable = False
                        End If
                    Catch ex As Exception
                        LogError(New ScanFailedException("An error occurred while determining if this file could be scanned.", ex, pFile), True)
                        unScannable = True
                    End Try

                    If Not unScannable Then

                        ' Actually check whether we can scan this.

                        ' Is this a file extension that we can scan?
                        Dim fileExt As String
                        Dim skippedbypath As Boolean = False
                        fileExt = System.IO.Path.GetExtension(pFile).ToLower.TrimStart(".")

                        Dim reason As UnableToScanReason
                        If ShouldScanExt(fileExt, reason) Then
                            If ShouldScanFileName(pFile) Then listFiles.Add(pFile)
                            ' Files in SkippedPaths are not reported as 'unscanned'.
                        Else
                            ' Don't scan this extension.
                            '   Whitelisted files are not reported as 'unscanned'.
                            If reason <> UnableToScanReason.WhiteListed Then
                                UnableToScan(pFile, reason)
                            End If
                        End If
                    End If
                Catch ex As Exception
                    LogError(New Exception("An error occurred while building the list of files to scan. " + pFile + " may not have been scanned.", ex))
                End Try
            Next

        Catch ex As Exception
            LogError(New Exception("An error occurred while building the list of files to scan. Sub-directories of " + folderPath + " may have been missed.", ex), True)
        End Try
    End Sub

    Public Function ShouldScanPath(ByVal ppath As String) As Boolean

        ' Don't scan it if it's within a path to skip.
        If (SkipPaths.Count > 0) Then
            Dim en As IDictionaryEnumerator = SkipPaths.GetEnumerator
            Dim pathToSkip As String
            While en.MoveNext
                pathToSkip = en.Key
                If MatchRegex(ppath, pathToSkip) Then
                    Return False
                End If
            End While
        End If
        Return True
    End Function

    Public Function ShouldScanFileName(ByVal ppath As String) As Boolean

        ' Don't scan it if it's within a path to skip.
        If (SkipFileNames.Count > 0) Then
            Dim en As IDictionaryEnumerator = SkipFileNames.GetEnumerator
            Dim pathToSkip As String
            While en.MoveNext
                pathToSkip = en.Key
                If MatchRegex(ppath, pathToSkip) Then
                    Return False
                End If
            End While
        End If
        Return True
    End Function

#End Region

#Region "Stateless Support Functions"

    Public Sub ShowReportForm(ByVal contents As String, ByVal pPath As String, ByVal isDialog As Boolean)
        ' Dim line() As String
        Dim index As Integer = 0
        ' Dim newtext As String
        ' Dim encoding As New System.Text.ASCIIEncoding
        'Dim foo() As Byte

        Dim frm5 As New LogViewerForm

        If contents = String.Empty Then
            MsgBox("There are no results to display. (" + pPath + " is empty.)", MsgBoxStyle.Information, APP_NAME)
            Return
        End If

        frm5.log_box.Text = contents
        frm5.Text = My.Application.Info.ProductName + " Log Viewer: " + pPath
        If isDialog Then
            frm5.ShowDialog()
        Else
            frm5.Show()
        End If

        Return
    End Sub

#End Region


#Region "Firefly Startup"

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' read our environment
        ' read our config
        ' get going
        ' Dim reg As RegistryKey
        ' Dim foo As String

        ' Dim s As String
        ' Dim i As Integer

        'OUR_EPOCH = Convert.ToDateTime("01/01/0001")
        'Debug.WriteLine("EPOCH: " + OUR_EPOCH.ToString)

        ' only allow one instance
        Dim aModuleName As String = Diagnostics.Process.GetCurrentProcess.MainModule.ModuleName

        Dim aProcName As String = System.IO.Path.GetFileNameWithoutExtension(aModuleName)

        If Process.GetProcessesByName(aProcName).Length > 1 Then
            MsgBox(My.Application.Info.ProductName + " is already running.")
            Application.Exit()
        End If

        ' Worth a try...
        ' If ApplicationDeployment.IsNetworkDeployed Then
        ' APP_NAME = Application.ProductName + " " + ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString
        ' Else
        APP_NAME = Application.ProductName + " " + System.String.Format(" {0}.{1}.{2}", My.Application.Info.Version.Major, My.Application.Info.Version.Minor, My.Application.Info.Version.Build)
        ' End If


        Me.Text = APP_NAME
        RunButton_Play = "Run " + My.Application.Info.ProductName + " Search"
        RunButton_Stop = "Stop " + My.Application.Info.ProductName + " Search"
        RunButton_Pause = "Pause " + My.Application.Info.ProductName + " Search"
        ' UpdateButtonText(RunButton_Play)
        ' UpdateInterfaceButtons()

        ' Set the required Firefly settings.
        EnforceFireflySettings()

        LoadValidators("ssn_area_codes", ValidatorTypes.US_SSN)
        LoadValidators("ccn_area_codes", ValidatorTypes.CreditCardPrefixes)

        ProcessCommandLineOptions()

        ' Check the reports path
        If Not System.IO.Directory.Exists(ReportDirectory) Then
            ' reportsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
            DisplayAndLogError(New Exception("Report path specified does not exist. Parameter ignored."))
            ReportDirectory = "" ' Right next to the executable file.
        End If

        Try
            If Not System.IO.Directory.Exists(LogDirectory) Then
                System.IO.Directory.CreateDirectory(LogDirectory)
            End If
        Catch ex As Exception
            LogDirectory = ReportDirectory
            DisplayAndLogError(New Exception("Unable to create the log directory. Logs will be saved in the reports directory instead.", ex), True)
        End Try

        ReportFileName = My.Application.Info.ProductName + " Report"
        ReportPath = ReportDirectory + "\" + ReportFileName
        CsvReportPath = LogDirectory + "\" + ReportFileName + ".csv"

        ' Log files
        AppLogPath = LogDirectory + "\" + My.Application.Info.ProductName + ".log"
        SkippedFilesListPath = LogDirectory + "\" + My.Application.Info.ProductName + "_FilesSkipped.csv"
        SecurityReportPath = LogDirectory + "\" + My.Application.Info.ProductName + "_SecurityReport.xml"
        SecurityReportSummaryPath = LogDirectory + "\" + My.Application.Info.ProductName + "_SecurityReportSummary.txt"
        UnprocessedFilesPath = LogDirectory + "\" + My.Application.Info.ProductName + "FilesToProcess.log"



        If (FireflyRunOptions And RunOptions.Unattended) Then
            Dim t As New Thread(AddressOf Me.scan_main)
            t.IsBackground = True
            t.Start()
            If (My.Settings.FireflyRunPriority = RunPriority.Low) Then
                t.Priority = ThreadPriority.BelowNormal
            ElseIf (My.Settings.FireflyRunPriority = RunPriority.High) Then
                t.Priority = ThreadPriority.AboveNormal
            ElseIf (My.Settings.FireflyRunPriority = RunPriority.Highest) Then
                t.Priority = ThreadPriority.Highest
            Else
                t.Priority = ThreadPriority.Normal
            End If
        End If
        UpdateInterfaceButtons()

    End Sub

    Private Function GetCommandLineArguments() As Hashtable
        Dim args As Hashtable
        Dim rawArgs As Array
        args = New Hashtable
        rawArgs = Environment.GetCommandLineArgs()
        For Each arg As String In rawArgs
            Dim key As String = ""
            Dim val As String = ""
            If arg.StartsWith("--") Then
                key = arg.Remove(0, 2)
            ElseIf arg.StartsWith("-") Or arg.StartsWith("/") Then
                key = arg.Remove(0, 1)
            End If
            If key <> "" Then
                Dim x As Integer = Array.IndexOf(rawArgs, arg) + 1
                If rawArgs.Length > x Then
                    val = rawArgs(x)
                End If
                If val.StartsWith("-") Or val.StartsWith("--") Or val.StartsWith("/") Then
                    val = ""
                End If
                key = key.ToLower
                If Not args.Contains(key) Then args.Add(key, val)
            End If
        Next
        Return args
    End Function

    Private Sub ProcessCommandLineOptions()
        Dim argsHash As Hashtable
        Try
            argsHash = GetCommandLineArguments()

            ' Advertise the Firefly flags.
            cmdLineOpts = New StringCollection()

            cmdLineOpts.Add("/debug causes " + My.Application.Info.ProductName + " to log full details for all errors.")
            cmdLineOpts.Add("/h or /? or /help outputs this text.")
            cmdLineOpts.Add("/path <directory> scans the directory specified and all subdirectories.")
            cmdLineOpts.Add("/priority <1,2,3 or 4> sets the run priority low(1) or high(4).")
            cmdLineOpts.Add("/reportpath <directory> places the report in the directory specified.")
            cmdLineOpts.Add("/logpath <directory> places the log in the directory specified.")
            cmdLineOpts.Add("/su scans unrecognized files as if they were plain text.")
            cmdLineOpts.Add("/se scans everything (takes a long time).")
            cmdLineOpts.Add("/skipExt 'ext1 ext2' adds extensions ext1 and ext2 to the skipped extensions list.")
            cmdLineOpts.Add("/scanList <text file> scans the files listed in the text file specified.")
            cmdLineOpts.Add("/threshold <number> or /t <number> reports only files with <number> or more potential matches.")
            cmdLineOpts.Add("/unattended causes " + My.Application.Info.ProductName + " to run unattended.")

            ' Debug mode
            If argsHash.Contains("debug") Then
                FireflyRunOptions = FireflyRunOptions Or Configuration.RunOptions.Debug
            End If

            If argsHash.ContainsKey("unattended") Or argsHash.ContainsKey("run") Then
                FireflyRunOptions = FireflyRunOptions Or Configuration.RunOptions.Unattended
                '                 Unattended = True
                FireflyRunOptions = FireflyRunOptions Or Configuration.RunOptions.ExitWhenDone
                '                 ExitWhenFinished = True
                If (FireflyRunOptions And Configuration.RunOptions.Restore) Then
                    FireflyRunOptions = FireflyRunOptions Xor Configuration.RunOptions.Restore
                End If
                '                Restorewindow = False
                If (FireflyRunOptions And Configuration.RunOptions.LaunchLogViewer) Then
                    FireflyRunOptions = FireflyRunOptions Xor Configuration.RunOptions.LaunchLogViewer
                End If
                '                LaunchLogViewer = False
                Me.WindowState = Minimized
                Me.Visible = False
                Me.Firefly_progress.Text = "Unattended"
                Me.Refresh()
            End If

            If argsHash.ContainsKey("path") Then
                My.Settings.ScanDir = argsHash("path")
                ' Don't resume a search in progress.
                My.Settings.ScanInProgress = False
                ' Don't scan all local drives
                FireflyScanDiskOptions = FireflyScanDiskOptions Xor DiskModeOptions.AllLocalDrives
            End If

            If argsHash.ContainsKey("su") Then
                FireflyScanDiskOptions = FireflyScanDiskOptions Or DiskModeOptions.ScanUnknownFileTypes
                AddSkipExts()
                ScanExts.Clear()
            End If

            If argsHash.ContainsKey("se") Then
                FireflyScanDiskOptions = FireflyScanDiskOptions Or DiskModeOptions.ScanEverything
                FireflyScanDiskOptions = FireflyScanDiskOptions Or DiskModeOptions.ScanUnknownFileTypes
                SkipExts.Clear()
                SkipPaths.Clear()
                ScanExts.Clear()
            End If

            If argsHash.ContainsKey("reportpath") Then
                ReportDirectory = argsHash("reportpath")
            End If

            If argsHash.ContainsKey("logpath") Then
                LogDirectory = argsHash("logpath")
            End If

            If argsHash.ContainsKey("skipext") Then
                Dim myExtList As String = argsHash("skipext")
                Try
                    Dim ext As String
                    For Each ext In myExtList.Split()
                        ext = ext.Replace(".", "")
                        If Not SkipExts.Contains(ext) Then SkipExts.Add(ext, "1")
                    Next
                Catch ex As Exception
                    Throw New Exception("An error occurred while adding extensions to the skip list.", ex)
                End Try
            End If

            If argsHash.ContainsKey("t") Then
                My.Settings.ReportAfterXMatches = argsHash("t")
            End If

            If argsHash.ContainsKey("threshhold") Then
                My.Settings.ReportAfterXMatches = argsHash("threshhold")
            End If

            If argsHash.ContainsKey("scanlist") Then
                FireflyScanDiskOptions = FireflyScanDiskOptions Or DiskModeOptions.ScanFileList
                My.Settings.ScanDir = argsHash("scanlist")
                ' Don't resume a search in progress.
                My.Settings.ScanInProgress = False
                ' Don't scan all local drives
                FireflyScanDiskOptions = FireflyScanDiskOptions Xor DiskModeOptions.AllLocalDrives
            End If


            If argsHash.Contains("priority") Then
                Dim priority As String
                priority = argsHash("priority")
                Select Case priority
                    Case "1" ' RunPriority.Low.ToString
                        My.Settings.FireflyRunPriority = RunPriority.Low
                    Case "2" ' RunPriority.Normal.ToString
                        My.Settings.FireflyRunPriority = RunPriority.Normal
                    Case "3" ' RunPriority.High.ToString
                        My.Settings.FireflyRunPriority = RunPriority.High
                    Case "4" ' RunPriority.Highest.ToString
                        My.Settings.FireflyRunPriority = RunPriority.Highest
                End Select
            End If

            ' Print the command line help.
            If argsHash.Contains("help") Or argsHash.Contains("?") Or argsHash.Contains("h") Then
                Try
                    ' Dim sw As System.IO.TextWriter = Console.
                    ' Dim sw As StreamWriter = New StreamWriter(os)
                    Try
                        Console.WriteLine("Usage: " + vbCrLf + My.Application.Info.ProductName + ".exe")
                        Dim item As String
                        For Each item In cmdLineOpts
                            Console.WriteLine(item)
                        Next
                    Catch ex As Exception
                        LogError(New Exception(My.Application.Info.ProductName + "failed to display the help text to the command line.", ex))
                    Finally
                        System.Environment.Exit(0)
                    End Try
                Catch ex As Exception
                    DisplayAndLogError(ex)
                End Try
            End If

        Catch ex As Exception
            DisplayAndLogError(New Exception("An error occurred while processing command line options. The command line settings may not have been applied. See the error log for more details.", ex), True)
        End Try

        '' Match the Spider flags, but don't advertise them.
        '' /L: - report file location
        '' /R: - recursive (is default, so ignore)
        '' /D: - non-recursive
        '' /P: - password path
        '' /F - log all fragments (not allowed in Firefly, throw an error)
        '' /run - run unattended.

    End Sub
#End Region


#Region "Update the interface Safely Across Threads"
    '' All cross-thread updates to the GUI must go through this function.
    Public Sub UpdateInterfaceMessage(ByVal message As String, ByVal media As StatusMedia, ByVal sent As Date, Optional ByVal forceDisplay As Boolean = False)
        If media And StatusMedia.Status Then
            ' Updating too regularly can freeze the interface.
            If sent >= DateAndTime.DateAdd(DateInterval.Second, TimeBetweenUpdates, LastStatusUpdate) Or forceDisplay Then
                Me.BeginInvoke(New InvokeUpdateStatus(AddressOf UpdateStatusLabel), message)
                LastStatusUpdate = Now()
            End If
        End If
        If media And StatusMedia.Progress Then
            If sent >= DateAndTime.DateAdd(DateInterval.Second, TimeBetweenUpdates, LastProgressUpdate) Or forceDisplay Then
                Me.BeginInvoke(New InvokeUpdateStatus(AddressOf UpdateProgressLabel), message)
                LastProgressUpdate = Now()
            End If
        End If

    End Sub



    '' Each Update... function must have an argument list that matches the delegate below.
    Delegate Sub InvokeUpdateStatus(ByVal text As String)
    Public Sub UpdateStatusLabel(ByVal text As String)
        Me.Firefly_Status.Text = text
        Me.Refresh()
    End Sub

    Public Sub UpdateInterfaceProgress(ByVal done As Integer, ByVal total As Integer, ByVal sent As Date)
        ' If this interface update was sent within one second of the previous, just skip it.
        If sent >= DateAndTime.DateAdd(DateInterval.Second, TimeBetweenUpdates, LastProgressUpdate) Then
            Dim args() As Integer = {done, total}
            Me.BeginInvoke(New InvokeUpdateProgress(AddressOf UpdateInterfaceProgress_Queued), args)
        Else
            Return
        End If
    End Sub
    Delegate Sub InvokeUpdateProgress(ByVal values As Integer())
    Public Sub UpdateInterfaceProgress_Queued(ByVal values As Integer())
        Try
            Dim max As Integer = values(1)
            Dim value As Integer = values(0)
            Me.ScanProgress.Maximum = max

            If value < max Then
                Me.ScanProgress.Value = value
            Else
                Me.ScanProgress.Value = max
            End If

            Me.Refresh()
        Catch
            ' Supress
        End Try
    End Sub

    Public Sub UpdateInterfaceButtons_ThreadSafe()
        Me.BeginInvoke(New InvokeUpdateInterfaceButtons(AddressOf UpdateInterfaceButtons))
    End Sub
    Delegate Sub InvokeUpdateInterfaceButtons()
    Public Sub UpdateInterfaceButtons()
        Select Case FireflyScanStage
            Case ScanStages.Stopped
                Me.run_button.Enabled = True
                Me.stop_button.Enabled = False
            Case ScanStages.Scanning
                Me.run_button.Enabled = False
                Me.stop_button.Enabled = True
            Case ScanStages.BuildingFileList
                Me.run_button.Enabled = False
                Me.stop_button.Enabled = True
            Case ScanStages.SavingFileList
                Me.run_button.Enabled = False
                Me.stop_button.Enabled = False
        End Select
        Me.Refresh()

    End Sub

    'Public Sub UpdateButtonText(ByVal text As String)
    '    Select Case text
    '        Case RunButton_Play
    '            Me.run_button.Text = ""
    '            Me.run_button.Image = My.Resources.play
    '        Case RunButton_Stop
    '            Me.run_button.Text = ""
    '            Me.run_button.Image = My.Resources._stop
    '        Case RunButton_Pause
    '            Me.run_button.Text = ""
    '            Me.run_button.Image = My.Resources.pause
    '    End Select
    '    ' Me.run_button.Text = text
    '    Me.Refresh()
    'End Sub

    Public Sub UpdateFormCaption(ByVal text As String)
        Me.Text = text
        Me.Refresh()
    End Sub

    Public Sub UpdateProgressLabel(ByVal text As String)
        Me.Firefly_progress.Text = text
        Me.Refresh()
    End Sub

    '' Change the window state
    Private Sub SetWindowStateNormal_ThreadSafe()
        Me.BeginInvoke(New InvokeUpdateWindowState(AddressOf SetWindowStateNormal))
    End Sub
    Delegate Sub InvokeUpdateWindowState()
    Public Sub SetWindowStateNormal()
        'Here is the bit that actually does something!
        Me.WindowState = Normal
    End Sub
#End Region


#Region "Scanning Functions"

#End Region

    Private Sub TextBox1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs)
        If IntroText.Height > 0 Then
            IntroText.Height = 0
        Else
            IntroText.Height = 600
        End If

    End Sub


    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)


    End Sub

    Private Sub TestWordFix_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub IntroText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IntroText.Click

    End Sub

    Private Sub MenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem4.Click
        Dim aboutsec As New AboutSecurityReport()
        aboutsec.Show()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Start(Firefly_HELP_URL)
    End Sub
End Class