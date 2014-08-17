Public Class PasswordSetForm
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
    Friend WithEvents password_label As System.Windows.Forms.Label
    Friend WithEvents password_box As System.Windows.Forms.TextBox
    Friend WithEvents password_ok As System.Windows.Forms.Button
    Friend WithEvents password_cancel As System.Windows.Forms.Button
    Friend WithEvents char_count_box As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents password_confirm_box As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.password_label = New System.Windows.Forms.Label
        Me.password_box = New System.Windows.Forms.TextBox
        Me.password_ok = New System.Windows.Forms.Button
        Me.password_cancel = New System.Windows.Forms.Button
        Me.char_count_box = New System.Windows.Forms.Label
        Me.password_confirm_box = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'password_label
        '
        Me.password_label.Location = New System.Drawing.Point(19, 18)
        Me.password_label.Name = "password_label"
        Me.password_label.Size = New System.Drawing.Size(317, 84)
        Me.password_label.TabIndex = 0
        Me.password_label.Text = "Password"
        '
        'password_box
        '
        Me.password_box.Location = New System.Drawing.Point(19, 148)
        Me.password_box.Name = "password_box"
        Me.password_box.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.password_box.Size = New System.Drawing.Size(307, 22)
        Me.password_box.TabIndex = 1
        Me.password_box.Text = ""
        '
        'password_ok
        '
        Me.password_ok.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.password_ok.Location = New System.Drawing.Point(58, 249)
        Me.password_ok.Name = "password_ok"
        Me.password_ok.Size = New System.Drawing.Size(90, 27)
        Me.password_ok.TabIndex = 3
        Me.password_ok.Text = "OK"
        '
        'password_cancel
        '
        Me.password_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.password_cancel.Location = New System.Drawing.Point(202, 249)
        Me.password_cancel.Name = "password_cancel"
        Me.password_cancel.Size = New System.Drawing.Size(90, 27)
        Me.password_cancel.TabIndex = 4
        Me.password_cancel.Text = "Cancel"
        '
        'char_count_box
        '
        Me.char_count_box.Location = New System.Drawing.Point(19, 111)
        Me.char_count_box.Name = "char_count_box"
        Me.char_count_box.Size = New System.Drawing.Size(240, 26)
        Me.char_count_box.TabIndex = 4
        Me.char_count_box.Text = "Need 16 characters"
        '
        'password_confirm_box
        '
        Me.password_confirm_box.Location = New System.Drawing.Point(22, 212)
        Me.password_confirm_box.Name = "password_confirm_box"
        Me.password_confirm_box.PasswordChar = Microsoft.VisualBasic.ChrW(42)
        Me.password_confirm_box.Size = New System.Drawing.Size(307, 22)
        Me.password_confirm_box.TabIndex = 2
        Me.password_confirm_box.Text = ""
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(19, 185)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 18)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Confirm"
        '
        'PasswordSetForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(350, 288)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.password_confirm_box)
        Me.Controls.Add(Me.char_count_box)
        Me.Controls.Add(Me.password_cancel)
        Me.Controls.Add(Me.password_ok)
        Me.Controls.Add(Me.password_box)
        Me.Controls.Add(Me.password_label)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PasswordSetForm"
        Me.Text = "Encrypt log file"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub password_ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles password_ok.Click
        PASSWORD = ""
        If password_box.Text <> String.Empty Then
            PASSWORD = password_box.Text
        Else
            PASSWORD = ""
        End If

        If password_box.Text <> password_confirm_box.Text And password_confirm_box.Enabled = True Then
            MsgBox("Passwords do not match", MsgBoxStyle.Exclamation, "Mismatched passwords")
            Return
            Debug.WriteLine("mismatched password")
        Else
            Me.Close()
        End If

        '        Me.Close()
    End Sub

    Private Sub password_cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles password_cancel.Click
        Me.Close()
    End Sub

    Private Sub password_box_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles password_box.TextChanged
        Dim need As Integer

        need = 16 - password_box.Text.Length
        If need >= 0 Then
            Me.char_count_box.Text = "Need " + need.ToString + " characters."
        Else
            Me.char_count_box.Text = "Need 0 characters."
        End If
        If password_box.Text.Length < 16 Then
            password_ok.Enabled = False
        Else
            password_ok.Enabled = True
            ' password_ok.Focus()
        End If
    End Sub

    Private Sub password_box_KeyPressed(ByVal sender As System.Object, ByVal e As KeyPressEventArgs) Handles password_box.KeyPress
        Debug.WriteLine("KEYchar: (" + e.KeyChar.ToString + ")")
        If e.KeyChar = Chr(13) Then
            Debug.WriteLine("newline")
            Me.password_ok.PerformClick()
        Else
            '  e.Handled = True
        End If
    End Sub

    Private Sub password_confirm_box_KeyPressed(ByVal sender As System.Object, ByVal e As KeyPressEventArgs) Handles password_confirm_box.KeyPress
        If e.KeyChar = Chr(13) Then
            Me.password_ok.PerformClick()
        Else

        End If
    End Sub
    Private Sub password_confirm_box_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles password_confirm_box.TextChanged

        If password_box.Text.Length = password_confirm_box.Text.Length Then
            password_ok.Focus()
        End If

    End Sub

    Private Sub Form4_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PASSWORD = ""
        If password_box.Text.Length < 16 Then
            password_ok.Enabled = False
        End If
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub
End Class
