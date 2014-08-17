Imports System.Array ' Sort

Public Class FileTypesForm
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
    Friend WithEvents scan_group As System.Windows.Forms.GroupBox
    Friend WithEvents skip_group As System.Windows.Forms.GroupBox
    Friend WithEvents exts_save As System.Windows.Forms.Button
    Friend WithEvents close_exts As System.Windows.Forms.Button
    Friend WithEvents keep_exts_box As System.Windows.Forms.ListBox
    Friend WithEvents skip_exts_box As System.Windows.Forms.ListBox
    Friend WithEvents keep_add_button As System.Windows.Forms.Button
    Friend WithEvents keep_remove_button As System.Windows.Forms.Button
    Friend WithEvents skip_add_button As System.Windows.Forms.Button
    Friend WithEvents skip_remove_button As System.Windows.Forms.Button
    Friend WithEvents scan_to_skip As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents skip_to_scan As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FileTypesForm))
        Me.scan_group = New System.Windows.Forms.GroupBox
        Me.keep_remove_button = New System.Windows.Forms.Button
        Me.keep_add_button = New System.Windows.Forms.Button
        Me.keep_exts_box = New System.Windows.Forms.ListBox
        Me.skip_group = New System.Windows.Forms.GroupBox
        Me.skip_remove_button = New System.Windows.Forms.Button
        Me.skip_add_button = New System.Windows.Forms.Button
        Me.skip_exts_box = New System.Windows.Forms.ListBox
        Me.exts_save = New System.Windows.Forms.Button
        Me.close_exts = New System.Windows.Forms.Button
        Me.scan_to_skip = New System.Windows.Forms.Button
        Me.skip_to_scan = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.scan_group.SuspendLayout()
        Me.skip_group.SuspendLayout()
        Me.SuspendLayout()
        '
        'scan_group
        '
        Me.scan_group.Controls.Add(Me.keep_remove_button)
        Me.scan_group.Controls.Add(Me.keep_add_button)
        Me.scan_group.Controls.Add(Me.keep_exts_box)
        Me.scan_group.Location = New System.Drawing.Point(3, 65)
        Me.scan_group.Name = "scan_group"
        Me.scan_group.Size = New System.Drawing.Size(264, 344)
        Me.scan_group.TabIndex = 0
        Me.scan_group.TabStop = False
        Me.scan_group.Text = "File Extensions to Scan"
        '
        'keep_remove_button
        '
        Me.keep_remove_button.Location = New System.Drawing.Point(160, 72)
        Me.keep_remove_button.Name = "keep_remove_button"
        Me.keep_remove_button.Size = New System.Drawing.Size(90, 26)
        Me.keep_remove_button.TabIndex = 2
        Me.keep_remove_button.Text = "Remove"
        '
        'keep_add_button
        '
        Me.keep_add_button.Location = New System.Drawing.Point(168, 24)
        Me.keep_add_button.Name = "keep_add_button"
        Me.keep_add_button.Size = New System.Drawing.Size(90, 27)
        Me.keep_add_button.TabIndex = 1
        Me.keep_add_button.Text = "Add"
        '
        'keep_exts_box
        '
        Me.keep_exts_box.ItemHeight = 16
        Me.keep_exts_box.Location = New System.Drawing.Point(8, 24)
        Me.keep_exts_box.Name = "keep_exts_box"
        Me.keep_exts_box.Size = New System.Drawing.Size(144, 308)
        Me.keep_exts_box.TabIndex = 0
        '
        'skip_group
        '
        Me.skip_group.Controls.Add(Me.skip_remove_button)
        Me.skip_group.Controls.Add(Me.skip_add_button)
        Me.skip_group.Controls.Add(Me.skip_exts_box)
        Me.skip_group.Location = New System.Drawing.Point(315, 65)
        Me.skip_group.Name = "skip_group"
        Me.skip_group.Size = New System.Drawing.Size(264, 344)
        Me.skip_group.TabIndex = 1
        Me.skip_group.TabStop = False
        Me.skip_group.Text = "File Extensions to Skip"
        '
        'skip_remove_button
        '
        Me.skip_remove_button.Location = New System.Drawing.Point(160, 64)
        Me.skip_remove_button.Name = "skip_remove_button"
        Me.skip_remove_button.Size = New System.Drawing.Size(90, 26)
        Me.skip_remove_button.TabIndex = 3
        Me.skip_remove_button.Text = "Remove"
        '
        'skip_add_button
        '
        Me.skip_add_button.Location = New System.Drawing.Point(160, 24)
        Me.skip_add_button.Name = "skip_add_button"
        Me.skip_add_button.Size = New System.Drawing.Size(90, 27)
        Me.skip_add_button.TabIndex = 2
        Me.skip_add_button.Text = "Add"
        '
        'skip_exts_box
        '
        Me.skip_exts_box.ItemHeight = 16
        Me.skip_exts_box.Location = New System.Drawing.Point(8, 24)
        Me.skip_exts_box.Name = "skip_exts_box"
        Me.skip_exts_box.Size = New System.Drawing.Size(144, 308)
        Me.skip_exts_box.TabIndex = 1
        '
        'exts_save
        '
        Me.exts_save.Location = New System.Drawing.Point(395, 417)
        Me.exts_save.Name = "exts_save"
        Me.exts_save.Size = New System.Drawing.Size(90, 26)
        Me.exts_save.TabIndex = 2
        Me.exts_save.Text = "Save"
        '
        'close_exts
        '
        Me.close_exts.Location = New System.Drawing.Point(491, 417)
        Me.close_exts.Name = "close_exts"
        Me.close_exts.Size = New System.Drawing.Size(90, 26)
        Me.close_exts.TabIndex = 3
        Me.close_exts.Text = "Close"
        '
        'scan_to_skip
        '
        Me.scan_to_skip.Location = New System.Drawing.Point(275, 89)
        Me.scan_to_skip.Name = "scan_to_skip"
        Me.scan_to_skip.Size = New System.Drawing.Size(29, 27)
        Me.scan_to_skip.TabIndex = 4
        Me.scan_to_skip.Text = "->"
        '
        'skip_to_scan
        '
        Me.skip_to_scan.Location = New System.Drawing.Point(275, 137)
        Me.skip_to_scan.Name = "skip_to_scan"
        Me.skip_to_scan.Size = New System.Drawing.Size(29, 26)
        Me.skip_to_scan.TabIndex = 5
        Me.skip_to_scan.Text = "<-"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(424, 17)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "The filetypes listed below are in addition to the standard filetypes. "
        '
        'FileTypesForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(592, 453)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.skip_to_scan)
        Me.Controls.Add(Me.scan_to_skip)
        Me.Controls.Add(Me.close_exts)
        Me.Controls.Add(Me.exts_save)
        Me.Controls.Add(Me.skip_group)
        Me.Controls.Add(Me.scan_group)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FileTypesForm"
        Me.Text = "Custom File Types"
        Me.scan_group.ResumeLayout(False)
        Me.skip_group.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub Form6_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Dim en As IDictionaryEnumerator = KeepExts.GetEnumerator
        'Dim sen As IDictionaryEnumerator = SkipExts.GetEnumerator
        'Dim aList(1) As String
        'Dim i As Integer
        ''        Dim newList() As String

        '' we'll sort these for the convenience of the user
        'i = 0
        'While en.MoveNext
        '    aList(i) = en.Key
        '    i += 1
        '    ReDim Preserve aList(i)
        '    '            keep_exts_box.Items.Add(en.Key)
        'End While

        'Sort(aList)
        'For i = 0 To (aList.GetLength(0) - 1)
        '    If aList(i) <> String.Empty Then
        '        keep_exts_box.Items.Add(aList(i))
        '    End If
        'Next

        'i = 0
        'ReDim aList(1)
        'While sen.MoveNext
        '    aList(i) = sen.Key
        '    i += 1
        '    ReDim Preserve aList(i)
        '    '    skip_exts_box.Items.Add(sen.Key)
        'End While
        'Array.Sort(aList)
        'Debug.WriteLine("array size: " + aList.GetLength(0).ToString)
        '' newList.Sort(aList)
        'For i = 0 To (aList.GetLength(0) - 1)
        '    Debug.WriteLine("adding: " + aList(i))
        '    If aList(i) <> String.Empty Then
        '        skip_exts_box.Items.Add(aList(i))
        '    End If
        'Next

    End Sub

    Private Sub close_exts_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles close_exts.Click
        Me.Close()
    End Sub

    Private Sub exts_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles exts_save.Click
        ' clear the KeepExts and SkipExts hashes and repopulate.  The Save button
        ' on Form2 will set things permanently.  we'll have to doctor the cancel button to
        ' clear the hashes
        Dim i As Integer
        ' KeepExts.Clear()
        SkipExts.Clear()

        'If Me.keep_exts_box.Items.Count > 0 Then
        '    For i = 1 To Me.keep_exts_box.Items.Count
        '        KeepExts.Add(Me.keep_exts_box.Items.Item(i - 1), "1")
        '    Next
        'End If

        If Me.skip_exts_box.Items.Count > 0 Then
            For i = 1 To Me.skip_exts_box.Items.Count
                SkipExts.Add(Me.skip_exts_box.Items.Item(i - 1), "1")
            Next
        End If

        ' Updated - just save right now, please.
        Configuration.save_config()
        Me.Close()
    End Sub
    Private Sub keep_add_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles keep_add_button.Click
        ' pop up an inputbox requesting a new extension
        Dim newExt As String
        newExt = InputBox("Enter a new file extension.  No wildcards, no punctuation.", "")
        If newExt.ToString = String.Empty Then
            Return
        End If
        If newExt.IndexOf("*") > 0 Then
            Return
        End If

        If newExt.IndexOf(".") > 0 Then
            Return
        End If
        '   Debug.WriteLine("other: " + Me.skip_exts_box.Items.(newExt.ToUpper).ToString)
        If Me.skip_exts_box.Items.Contains(newExt.ToUpper) Then
            MsgBox("Duplicate item in other extension list.", MsgBoxStyle.OkOnly, My.Application.Info.ProductName)
            Return
        End If
        Me.keep_exts_box.Items.Add(newExt.ToUpper)
    End Sub

    Private Sub skip_add_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles skip_add_button.Click
        Dim newExt As String
        newExt = InputBox("Enter a new file extension.  No wildcards, no punctuation.", "")
        If newExt.ToString = String.Empty Then
            Return
        End If
        If newExt.IndexOf("*") > 0 Then
            Return
        End If
        If newExt.IndexOf(".") > 0 Then
            Return
        End If
        If Me.keep_exts_box.Items.Contains(newExt.ToUpper) Then
            MsgBox("Duplicate item in other extension list.", MsgBoxStyle.OkOnly, My.Application.Info.ProductName)
            Return
        End If
        Me.skip_exts_box.Items.Add(newExt.ToUpper)
    End Sub

    Private Sub keep_remove_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles keep_remove_button.Click
        Dim index As Integer
        ' Dim ext As String

        index = Me.keep_exts_box.SelectedIndex

        If index >= 0 Then
            Me.keep_exts_box.Items.RemoveAt(index)
        End If
        Return
    End Sub

    Private Sub skip_remove_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles skip_remove_button.Click
        Dim index As Integer
        ' Dim ext As String

        index = Me.skip_exts_box.SelectedIndex

        If index >= 0 Then
            Me.skip_exts_box.Items.RemoveAt(index)
        End If
        Return
    End Sub

    Private Sub scan_to_skip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles scan_to_skip.Click
        ' copy the extension from the scan box to the skip box and remove 
        Dim index As Integer
        Dim ext As String

        index = Me.keep_exts_box.SelectedIndex

        If index < 0 Then
            Return
        End If

        ext = Me.keep_exts_box.Items.Item(index)

        Me.skip_exts_box.Items.Add(ext)
        Me.keep_exts_box.Items.RemoveAt(index)

    End Sub

    Private Sub skip_to_scan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles skip_to_scan.Click
        Dim index As Integer
        Dim ext As String

        index = Me.skip_exts_box.SelectedIndex

        If index < 0 Then
            Return
        End If

        ext = Me.skip_exts_box.Items.Item(index)

        Me.keep_exts_box.Items.Add(ext)
        Me.skip_exts_box.Items.RemoveAt(index)
    End Sub
End Class