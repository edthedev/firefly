Imports NUnit.Framework 'Test Cases Exposed to NUnit
Imports System.IO 'File access

<TestFixture()> Public Class TestSkipLists

    Private ReportContents As String
    Private ReportLocation As String = "C:\Documents and Settings\delaport\Desktop\Firefly SSN Finder Report.htm"
    Private LogFolder As String
    Private ReportFolder As String

    Private Sub RunScan(Optional ByVal loc As String = "C:\Documents and Settings\delaport\Desktop\SSN\CommandLineTest", Optional ByVal args As String = "")
        ReportContents = TestingTools.RunScan_Details(args, ReportLocation, loc)
    End Sub


#Region "Skip Folder Rules"
    '' Rule of thumb: If we /skip it, it must be skipped!
    <Test()> Public Sub SkipWindowsFolder()
        RunScan("C:\WINDOWS")
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("scanned 0 files in C:\WINDOWS"), "Scanned some files in C:\WINDOWS.")
        ' NUnit.Framework.Assert.IsFalse(ReportContents.Contains("List of undelimited SSNs.completelyUnknownExtension"), "Oops, we scanned the Unkown file.")
    End Sub

    <Test()> Public Sub SkipVistaRecycleBin()
        RunScan("C:\$Recycle.bin")
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("scanned 0 files in C:\$Recycle.bin"), "Scanned some files in C:\$Recycle.bin.")
        ' NUnit.Framework.Assert.IsFalse(ReportContents.Contains("List of undelimited SSNs.completelyUnknownExtension"), "Oops, we scanned the Unkown file.")
    End Sub

    <Test()> Public Sub SkipQuarantine()
        RunScan("C:\QUARANTINE")
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("scanned 0 files in C:\QUARANTINE"), "Scanned some files in C:\QUARANTINE.")
        ' NUnit.Framework.Assert.IsFalse(ReportContents.Contains("List of undelimited SSNs.completelyUnknownExtension"), "Oops, we scanned the Unkown file.")
    End Sub

#End Region

End Class
