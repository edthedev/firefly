Public Class LogOptionsForm
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
    Friend WithEvents Firefly_loghostinfo As System.Windows.Forms.TextBox
    Friend WithEvents Firefly_log_label As System.Windows.Forms.Label
    Friend WithEvents loghostinfo_ok As System.Windows.Forms.Button
    Friend WithEvents loghostinfo_cancel As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Firefly_loghostinfo = New System.Windows.Forms.TextBox
        Me.Firefly_log_label = New System.Windows.Forms.Label
        Me.loghostinfo_ok = New System.Windows.Forms.Button
        Me.loghostinfo_cancel = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Firefly_loghostinfo
        '
        Me.Firefly_loghostinfo.Location = New System.Drawing.Point(8, 46)
        Me.Firefly_loghostinfo.Name = "Firefly_loghostinfo"
        Me.Firefly_loghostinfo.Size = New System.Drawing.Size(328, 22)
        Me.Firefly_loghostinfo.TabIndex = 0
        '
        'Firefly_log_label
        '
        Me.Firefly_log_label.Location = New System.Drawing.Point(16, 9)
        Me.Firefly_log_label.Name = "Firefly_log_label"
        Me.Firefly_log_label.Size = New System.Drawing.Size(278, 27)
        Me.Firefly_log_label.TabIndex = 1
        Me.Firefly_log_label.Text = "Enter string to append to log file"
        '
        'loghostinfo_ok
        '
        Me.loghostinfo_ok.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.loghostinfo_ok.Location = New System.Drawing.Point(152, 80)
        Me.loghostinfo_ok.Name = "loghostinfo_ok"
        Me.loghostinfo_ok.Size = New System.Drawing.Size(90, 27)
        Me.loghostinfo_ok.TabIndex = 2
        Me.loghostinfo_ok.Text = "OK"
        '
        'loghostinfo_cancel
        '
        Me.loghostinfo_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.loghostinfo_cancel.Location = New System.Drawing.Point(248, 80)
        Me.loghostinfo_cancel.Name = "loghostinfo_cancel"
        Me.loghostinfo_cancel.Size = New System.Drawing.Size(90, 27)
        Me.loghostinfo_cancel.TabIndex = 3
        Me.loghostinfo_cancel.Text = "Cancel"
        '
        'LogOptionsForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(344, 112)
        Me.Controls.Add(Me.loghostinfo_cancel)
        Me.Controls.Add(Me.loghostinfo_ok)
        Me.Controls.Add(Me.Firefly_log_label)
        Me.Controls.Add(Me.Firefly_loghostinfo)
        Me.MaximizeBox = False
        Me.Name = "LogOptionsForm"
        Me.Text = "Log Options"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub Form9_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Firefly_loghostinfo.Text = LogHostInfo
    End Sub
End Class
