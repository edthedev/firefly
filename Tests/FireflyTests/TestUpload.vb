Imports NUnit.Framework 'Test Cases Exposed to NUnit
Imports System.IO 'File access

'<TestFixture()> Public Class TestUpload
'    Const serverPath As String = "\\cites-magnum.adtest.uiuc.edu\isetest"
'    Const minorVersion As String = "9"
'    '<TestFixtureSetUp()> Public Sub CopyFiles()
'    '    Shell("powershell.exe 'C:\Documents and Settings\delaport\My Documents\ISE Projects\07-02-04SSNFinder\Publish\SignFirefly.ps1'")
'    'End Sub
'    <Test()> Public Sub LatestManifest_Published()
'        NUnit.Framework.Assert.IsTrue(File.Exists(serverPath + "\Firefly SSN Scanner_1_0_" + minorVersion + "_0.application"))
'    End Sub

'    <Test()> Public Sub FilesDirectory_Published()
'        NUnit.Framework.Assert.IsTrue(Directory.Exists(serverPath + "\Firefly SSN Scanner_1_0_" + minorVersion + "_0"))
'    End Sub

'    <Test()> Public Sub Setup_Published()
'        NUnit.Framework.Assert.IsTrue(File.Exists(serverPath + "\setup.exe"))
'    End Sub
'    <Test()> Public Sub UnVersionedManifest_Published()
'        NUnit.Framework.Assert.IsTrue(File.Exists(serverPath + "\Firefly SSN Scanner.application"))
'    End Sub

'    <Test()> Public Sub MSI_Published()
'        NUnit.Framework.Assert.IsTrue(File.Exists(serverPath + "\InstallFirefly.1.0." + minorVersion + ".msi"))
'    End Sub
'    <Test()> Public Sub Firefly4WinPage_Updated()
'        Dim x As New StreamReader(serverPath + "\firefly4win.htm")
'        Dim contents As String = x.ReadToEnd()
'        NUnit.Framework.Assert.IsTrue(contents.Contains("InstallFirefly.1.0." + minorVersion + ".msi"))
'    End Sub



'    '<Test()> Public Sub Manifest_Recent()
'    '    NUnit.Framework.Assert.IsTrue(serverPath + "\Firefly SSN Scanner_1_0_" + minorVersion + "_0.application")
'    'End Sub


'    'Private Function Recent(ByVal filePath As String) As Boolean
'    '    If File.Exists(filePath) Then
'    '        Date.Compare(File.GetLastAccessTime(filePath), Now() - 60 * 5)
'    '    Else
'    '        Return False
'    '    End If
'    'End Function
'End Class
