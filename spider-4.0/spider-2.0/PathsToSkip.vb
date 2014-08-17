Public Class PathsToSkipForm
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
    Friend WithEvents path_skip_box As System.Windows.Forms.ListBox
    Friend WithEvents add_path_skip As System.Windows.Forms.Button
    Friend WithEvents remove_path_skip As System.Windows.Forms.Button
    Friend WithEvents save_path_skip As System.Windows.Forms.Button
    Friend WithEvents cancel_path_skip As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.path_skip_box = New System.Windows.Forms.ListBox
        Me.add_path_skip = New System.Windows.Forms.Button
        Me.remove_path_skip = New System.Windows.Forms.Button
        Me.save_path_skip = New System.Windows.Forms.Button
        Me.cancel_path_skip = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'path_skip_box
        '
        Me.path_skip_box.HorizontalScrollbar = True
        Me.path_skip_box.ItemHeight = 16
        Me.path_skip_box.Location = New System.Drawing.Point(10, 9)
        Me.path_skip_box.Name = "path_skip_box"
        Me.path_skip_box.ScrollAlwaysVisible = True
        Me.path_skip_box.Size = New System.Drawing.Size(374, 260)
        Me.path_skip_box.TabIndex = 0
        '
        'add_path_skip
        '
        Me.add_path_skip.Location = New System.Drawing.Point(392, 104)
        Me.add_path_skip.Name = "add_path_skip"
        Me.add_path_skip.Size = New System.Drawing.Size(90, 26)
        Me.add_path_skip.TabIndex = 1
        Me.add_path_skip.Text = "Add New"
        '
        'remove_path_skip
        '
        Me.remove_path_skip.Location = New System.Drawing.Point(392, 136)
        Me.remove_path_skip.Name = "remove_path_skip"
        Me.remove_path_skip.Size = New System.Drawing.Size(90, 27)
        Me.remove_path_skip.TabIndex = 2
        Me.remove_path_skip.Text = "Remove"
        '
        'save_path_skip
        '
        Me.save_path_skip.Location = New System.Drawing.Point(392, 208)
        Me.save_path_skip.Name = "save_path_skip"
        Me.save_path_skip.Size = New System.Drawing.Size(90, 27)
        Me.save_path_skip.TabIndex = 3
        Me.save_path_skip.Text = "Save"
        '
        'cancel_path_skip
        '
        Me.cancel_path_skip.Location = New System.Drawing.Point(392, 240)
        Me.cancel_path_skip.Name = "cancel_path_skip"
        Me.cancel_path_skip.Size = New System.Drawing.Size(90, 27)
        Me.cancel_path_skip.TabIndex = 4
        Me.cancel_path_skip.Text = "Cancel"
        '
        'PathsToSkipForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(488, 280)
        Me.Controls.Add(Me.cancel_path_skip)
        Me.Controls.Add(Me.save_path_skip)
        Me.Controls.Add(Me.remove_path_skip)
        Me.Controls.Add(Me.add_path_skip)
        Me.Controls.Add(Me.path_skip_box)
        Me.MaximizeBox = False
        Me.Name = "PathsToSkipForm"
        Me.Text = "Paths to skip"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub cancel_path_skip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cancel_path_skip.Click
        Me.Close()
    End Sub

    Private Sub Form8_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' poplate the list box with the contents of the SkipPaths hashtable
        'Dim en As IDictionaryEnumerator
        'If Form8_State = Scanner.Form8_Dispo.SkipPaths Then
        '    en = SkipPaths.GetEnumerator
        '    Me.Text = "Paths to skip"
        'Else
        '    en = SkipContentTypes.GetEnumerator
        '    Me.Text = "MIME types to skip"
        'End If

        'Dim aList(1) As String
        'Dim i As Integer = 0

        'While en.MoveNext
        '    aList(i) = en.Key
        '    i += 1
        '    ReDim Preserve aList(i)
        'End While

        'Array.Sort(aList)

        'For i = 0 To (aList.GetLength(0) - 1)
        '    If aList(i) <> String.Empty Then
        '        Me.path_skip_box.Items.Add(aList(i))
        '    End If
        'Next

        'Me.cancel_path_skip.Text = "Cancel"
        'Me.add_path_skip.Select()

    End Sub

    Private Sub save_path_skip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles save_path_skip.Click
        ' read the contents of the listbox and populate a new SkipPaths hashtable
        'Dim i As Integer = 0

        'If Form8_State = Scanner.Form8_Dispo.SkipPaths Then
        '    SkipPaths.Clear()
        'Else
        '    SkipContentTypes.Clear()
        'End If

        'For i = 0 To (Me.path_skip_box.Items.Count - 1)
        '    If Me.path_skip_box.Items.Item(i) <> String.Empty Then
        '        If Form8_State = Scanner.Form8_Dispo.SkipPaths Then
        '            SkipPaths.Add(Me.path_skip_box.Items.Item(i), "1")
        '        Else
        '            SkipContentTypes.Add(Me.path_skip_box.Items(i), "1")
        '        End If
        '    End If
        'Next

        'If Form8_State = Scanner.Form8_Dispo.SkipContent Then
        '    ' paths2globs(SkipContentTypes, SkipContentRegex)
        '    SkipContentRegex = SkipContentTypes
        'End If

        'Me.cancel_path_skip.Text = "Close"
        'Me.cancel_path_skip.Select()

    End Sub

    Private Sub add_path_skip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles add_path_skip.Click
        ' pop up an inputbox to solicit this from the user
        Dim newskip As String
        newskip = InputBox("Provide a path that should be skipped during scanning", "Add path")

        If newskip <> String.Empty Then
            Me.path_skip_box.Items.Add(newskip)
        End If
        Me.Refresh()

    End Sub

    Private Sub remove_path_skip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles remove_path_skip.Click

        Me.path_skip_box.Items.Remove(Me.path_skip_box.SelectedItem)
        Me.Refresh()

    End Sub
End Class
