Public Class AdsForm
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
    Friend WithEvents ads_listbox As System.Windows.Forms.ListBox
    Friend WithEvents open_ads As System.Windows.Forms.Button
    Friend WithEvents ads_cancel As System.Windows.Forms.Button
    Friend WithEvents named_stream As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.ads_listbox = New System.Windows.Forms.ListBox
        Me.open_ads = New System.Windows.Forms.Button
        Me.ads_cancel = New System.Windows.Forms.Button
        Me.named_stream = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'ads_listbox
        '
        Me.ads_listbox.ItemHeight = 16
        Me.ads_listbox.Location = New System.Drawing.Point(8, 8)
        Me.ads_listbox.Name = "ads_listbox"
        Me.ads_listbox.Size = New System.Drawing.Size(240, 260)
        Me.ads_listbox.TabIndex = 0
        '
        'open_ads
        '
        Me.open_ads.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.open_ads.Location = New System.Drawing.Point(256, 176)
        Me.open_ads.Name = "open_ads"
        Me.open_ads.Size = New System.Drawing.Size(106, 27)
        Me.open_ads.TabIndex = 1
        Me.open_ads.Text = "Open"
        '
        'ads_cancel
        '
        Me.ads_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ads_cancel.Location = New System.Drawing.Point(256, 240)
        Me.ads_cancel.Name = "ads_cancel"
        Me.ads_cancel.Size = New System.Drawing.Size(106, 26)
        Me.ads_cancel.TabIndex = 2
        Me.ads_cancel.Text = "Cancel"
        '
        'named_stream
        '
        Me.named_stream.DialogResult = System.Windows.Forms.DialogResult.Ignore
        Me.named_stream.Location = New System.Drawing.Point(256, 208)
        Me.named_stream.Name = "named_stream"
        Me.named_stream.Size = New System.Drawing.Size(106, 26)
        Me.named_stream.TabIndex = 3
        Me.named_stream.Text = "Named Stream"
        '
        'AdsForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(368, 280)
        Me.Controls.Add(Me.named_stream)
        Me.Controls.Add(Me.ads_cancel)
        Me.Controls.Add(Me.open_ads)
        Me.Controls.Add(Me.ads_listbox)
        Me.MaximizeBox = False
        Me.Name = "AdsForm"
        Me.Text = "Select an alternate data stream"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub ads_cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ads_cancel.Click
        Me.Close()
    End Sub
End Class
