Imports System.io
Imports System.Runtime.InteropServices



Public Class LogViewerForm
    Inherits System.Windows.Forms.Form
    Public HyperLinkViewer As Boolean = False
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
    Friend WithEvents close_button As System.Windows.Forms.Button
    Friend WithEvents cp_clipboard As System.Windows.Forms.Button
    Friend WithEvents log_box As System.Windows.Forms.RichTextBox
    Friend WithEvents save_to_file As System.Windows.Forms.Button
    Friend WithEvents view_links As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.close_button = New System.Windows.Forms.Button
        Me.cp_clipboard = New System.Windows.Forms.Button
        Me.log_box = New System.Windows.Forms.RichTextBox
        Me.save_to_file = New System.Windows.Forms.Button
        Me.view_links = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'close_button
        '
        Me.close_button.Location = New System.Drawing.Point(690, 528)
        Me.close_button.Name = "close_button"
        Me.close_button.Size = New System.Drawing.Size(90, 26)
        Me.close_button.TabIndex = 1
        Me.close_button.Text = "Close"
        '
        'cp_clipboard
        '
        Me.cp_clipboard.Location = New System.Drawing.Point(17, 528)
        Me.cp_clipboard.Name = "cp_clipboard"
        Me.cp_clipboard.Size = New System.Drawing.Size(153, 26)
        Me.cp_clipboard.TabIndex = 2
        Me.cp_clipboard.Text = "Copy to Clipboard"
        '
        'log_box
        '
        Me.log_box.AutoSize = True
        Me.log_box.Location = New System.Drawing.Point(8, 8)
        Me.log_box.Name = "log_box"
        Me.log_box.ReadOnly = True
        Me.log_box.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth
        Me.log_box.Size = New System.Drawing.Size(772, 514)
        Me.log_box.TabIndex = 3
        Me.log_box.Text = "log"
        '
        'save_to_file
        '
        Me.save_to_file.Location = New System.Drawing.Point(176, 528)
        Me.save_to_file.Name = "save_to_file"
        Me.save_to_file.Size = New System.Drawing.Size(153, 26)
        Me.save_to_file.TabIndex = 4
        Me.save_to_file.Text = "Save to File"
        '
        'view_links
        '
        Me.view_links.Location = New System.Drawing.Point(335, 528)
        Me.view_links.Name = "view_links"
        Me.view_links.Size = New System.Drawing.Size(154, 26)
        Me.view_links.TabIndex = 5
        Me.view_links.Text = "View Links"
        '
        'LogViewerForm
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(792, 560)
        Me.Controls.Add(Me.view_links)
        Me.Controls.Add(Me.save_to_file)
        Me.Controls.Add(Me.log_box)
        Me.Controls.Add(Me.cp_clipboard)
        Me.Controls.Add(Me.close_button)
        Me.Name = "LogViewerForm"
        Me.Text = "Log Viewer"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub close_button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles close_button.Click
        Me.log_box.Text = ""
        enc_log_plaintext = ""
        PASSWORD = ""
        Me.Close()
    End Sub

    Private Sub cp_clipboard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cp_clipboard.Click
        Clipboard.SetDataObject(log_box.Text)
    End Sub

    Private Sub export_excel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
    Private Sub write_file(ByVal oPath As String)
        ' grab the contents of the RichTextBox to a file
        Me.log_box.SaveFile(oPath, RichTextBoxStreamType.PlainText)
        Return
    End Sub

    Private Sub save_to_file_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles save_to_file.Click
        ' open a file select dialog and get a path
        ' save it.
        Dim sfD As New SaveFileDialog
        Dim outPath As String

        sfD.InitialDirectory = System.IO.Directory.GetCurrentDirectory
        sfD.Filter = "Text files (*.txt)|*.txt|" & "All files|*.*"

        sfD.ShowDialog()

        outPath = sfD.FileName

        If outPath <> String.Empty Then
            write_file(outPath)
        End If

        Return
    End Sub
    Private Sub log_box_LinkClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkClickedEventArgs) Handles log_box.LinkClicked
        Dim foo() As String
        Dim pPath As String

        ' the link is everything up to the first comma
        ' we'll prune the file:/ off the front and verify the file exists, so we don't throw an exception
        foo = e.LinkText.Split(",")

        pPath = foo(0)
        ' we'll have to URL decode the rest
        pPath = URLDecode(pPath)
        Debug.WriteLine("Starting (" + pPath + ")")
        Try
            Process.Start(pPath)
        Catch ex As Exception
            Debug.WriteLine("can't start default browser: " + ex.ToString)
            Return
        End Try

    End Sub


    Private Sub view_links_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles view_links.Click
        ' we'll take the current text, chunk it up on lines, and re-write it as file:// links
        Dim oldText As String
        Dim newText As String = ""
        Dim line() As String
        Dim index As Integer

        If HyperLinkViewer Then
            Me.view_links.Text = "View Links"
            Me.log_box.Text = enc_log_plaintext
            Me.Refresh()
            HyperLinkViewer = False
            Return
        End If

        HyperLinkViewer = True
        Me.view_links.Text = "View Normal"

        oldText = Me.log_box.Text
        line = enc_log_plaintext.Split(vbCrLf)

        For index = 0 To (line.GetLength(0) - 1)
            If InStr(line(index), ":\") Then
                newText += "file://" + URLEncode(line(index)) + vbCrLf
                ' newText += "file://" + line(index) + vbCrLf
            Else
                newText += line(index) + vbCrLf
            End If
            If line(index) <> String.Empty Then
                Debug.WriteLine("(" + line(index) + ")")
            End If
            'End If
        Next

        Me.log_box.Text = newText
        Me.Refresh()

        Return

    End Sub
    Public Function URLDecode(ByVal inStr As String) As String

        ' what a craptastic way to do this

        inStr = inStr.Replace("file://", "")
        inStr = inStr.Replace("%20", " ")
        inStr = inStr.Replace("%3A", ":")
        inStr = inStr.Replace("%21", "!")
        inStr = inStr.Replace("%2A", "*")
        inStr = inStr.Replace("%27", "'")
        inStr = inStr.Replace("%28", "(")
        inStr = inStr.Replace("%29", ")")
        inStr = inStr.Replace("%3B", ";")
        inStr = inStr.Replace("%40", "@")
        inStr = inStr.Replace("%26", "&")
        inStr = inStr.Replace("%3D", "=")
        inStr = inStr.Replace("%2B", "+")
        inStr = inStr.Replace("%24", "$")
        inStr = inStr.Replace("%2F", "/")
        inStr = inStr.Replace("%3F", "?")
        inStr = inStr.Replace("%25", "%")
        inStr = inStr.Replace("%23", "#")
        inStr = inStr.Replace("%5C", "\")

        Return inStr

    End Function
    Public Function URLEncode(ByVal inStr As String) As String
        'we'll parse through the string character-wise, replacing things as we need to
        Dim c() As Char
        Dim i As Integer
        Dim ch As Integer
        Dim newStr As String
        Dim encoding As New System.Text.ASCIIEncoding
        Dim b(2) As Byte
        Dim Done_encoding As Boolean

        c = inStr.ToCharArray
        newStr = ""
        ' very limited encoding
        Done_encoding = False

        For i = 0 To (c.GetLength(0) - 1)
            ch = Asc(c(i))
            If (ch = 44) Or Done_encoding Then
                If Not Done_encoding Then
                    ' it's a comma, probably the first
                    Done_encoding = True
                    newStr += ", "
                Else
                    newStr += c(i)
                    Done_encoding = True
                End If
            End If
            If Not Done_encoding Then
                Select Case ch
                    Case 10
                        newStr += ""
                    Case 13
                        newStr += ""
                    Case 32
                        newStr += "%20"
                    Case 37
                        newStr += "%25"
                    Case 34
                        newStr += "%22"
                    Case 60
                        newStr += "%3C"
                    Case 62
                        newStr += "%3E"
                    Case 35
                        newStr += "%23"
                    Case 94
                        newStr += "%5E"
                    Case 91
                        newStr += "%5B"
                    Case 92
                        newStr += "%5C"
                    Case 93
                        newStr += "%5D"
                    Case 38
                        newStr += "%26"
                    Case 43
                        newStr += "%2B"
                    Case 44
                        newStr += "%2C"
                    Case 47
                        newStr += "%2F"
                    Case 58
                        newStr += "%3A"
                    Case 59
                        newStr += "%3B"
                    Case 61
                        newStr += "%3D"
                    Case 63
                        newStr += "%3F"
                    Case 64
                        newStr += "%40"
                    Case 36
                        newStr += "%24"
                    Case Else
                        newStr += c(i)
                End Select
            End If
        Next
        Return newStr
    End Function

    Private Sub log_box_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles log_box.TextChanged

    End Sub
End Class
