Public Class about_Firefly
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
    Friend WithEvents AboutFirefly As System.Windows.Forms.TextBox
    Friend WithEvents SecurityLogo As System.Windows.Forms.PictureBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents about_close As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(about_Firefly))
        Me.about_close = New System.Windows.Forms.Button
        Me.AboutFirefly = New System.Windows.Forms.TextBox
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.SecurityLogo = New System.Windows.Forms.PictureBox
        CType(Me.SecurityLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'about_close
        '
        Me.about_close.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.about_close.Location = New System.Drawing.Point(358, 271)
        Me.about_close.Name = "about_close"
        Me.about_close.Size = New System.Drawing.Size(52, 31)
        Me.about_close.TabIndex = 2
        Me.about_close.Text = "Close"
        '
        'AboutFirefly
        '
        Me.AboutFirefly.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AboutFirefly.BackColor = System.Drawing.SystemColors.Window
        Me.AboutFirefly.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.AboutFirefly.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AboutFirefly.Location = New System.Drawing.Point(12, 129)
        Me.AboutFirefly.Multiline = True
        Me.AboutFirefly.Name = "AboutFirefly"
        Me.AboutFirefly.Size = New System.Drawing.Size(398, 43)
        Me.AboutFirefly.TabIndex = 10
        Me.AboutFirefly.Text = "Firefly was developed by Edward Delaporte for CITES Security at the University of" & _
            " Illinois at Urbana-Champaign." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(12, 178)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(398, 39)
        Me.TextBox1.TabIndex = 12
        Me.TextBox1.Text = "Portions of Firefly are built on Cornell Spider's Version 3 Beta Release created " & _
            "at Cornell University." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'TextBox2
        '
        Me.TextBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox2.BackColor = System.Drawing.SystemColors.Window
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(12, 223)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(398, 42)
        Me.TextBox2.TabIndex = 13
        Me.TextBox2.Text = "The icons used in Firefly are courtesy of Ken Saunders at MouseRunner.com."
        '
        'SecurityLogo
        '
        Me.SecurityLogo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SecurityLogo.Image = Global.Firefly.My.Resources.Resources.securitylogo
        Me.SecurityLogo.Location = New System.Drawing.Point(12, 12)
        Me.SecurityLogo.Name = "SecurityLogo"
        Me.SecurityLogo.Size = New System.Drawing.Size(398, 111)
        Me.SecurityLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.SecurityLogo.TabIndex = 11
        Me.SecurityLogo.TabStop = False
        '
        'about_Firefly
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(422, 316)
        Me.ControlBox = False
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.SecurityLogo)
        Me.Controls.Add(Me.AboutFirefly)
        Me.Controls.Add(Me.about_close)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "about_Firefly"
        Me.Text = "About Firefly"
        CType(Me.SecurityLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub about_close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles about_close.Click
        Me.Close()
    End Sub

    Private Sub about_Firefly_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Me.Text = "About " + APP_NAME
        'Dim about As String = APP_NAME
        'about += " Created by Edward Delaporte for University of Illinois CITES Security."
        'about += " Based on Cornell Spider for Windows 3 Beta."
        'Me.AboutFirefly.Text = about
    End Sub

    Private Sub about_label_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub AboutFirefly_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutFirefly.TextChanged

    End Sub
End Class
