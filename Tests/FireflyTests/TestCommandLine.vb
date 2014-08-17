Imports NUnit.Framework 'Test Cases Exposed to NUnit
Imports System.IO 'File access

<TestFixture()> Public Class TestCommandLine

    Private ReportContents As String
    Private ReportLocation As String = "C:\Documents and Settings\delaport\Desktop\Firefly SSN Finder Report.htm"
    Private ScanLocation As String = "C:\Documents and Settings\delaport\Desktop\SSN\CommandLineTest"
    Private LogFolder As String
    Private ReportFolder As String

    Private Sub RunScan(ByVal args As String)
        ReportContents = TestingTools.RunScan_Details(args, ReportLocation, ScanLocation)
    End Sub

    <Test()> Public Sub ScanList()
        RunScan("/scanList ""C:\Documents and Settings\delaport\Desktop\SSN\CommandLineTest\List of Files.txt""")
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("C:\Documents and Settings\delaport\Desktop\SSN\CommandLineTest\List of Files.txt"), "Did not scan our list.")
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("scanned 28 files"), "Scanned the wrong nubmer of files.")
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("25 of the files scanned contain may contain sensitive information"), "Received the wrong number of results.")
    End Sub

    '<Test()> Public Sub Option_Path()
    '    RunScan("/path ""C:\Documents and Settings\delaport\Desktop\SSN\CommandLineTest\""")
    '    NUnit.Framework.Assert.IsTrue(ReportContents.Contains("RedirectedPath.txt"), "RedirectedPath.txt found.")
    '    NUnit.Framework.Assert.IsTrue(ReportContents.Contains("Just 2 SSNs.txt"), "Just 2 SSNs.txt not found.")
    'End Sub

#Region "Skip Rules"
    '' Rule of thumb: If we /skip it, it must be skipped!
    <Test()> Public Sub SkipMdb()
        RunScan("/skipExt ""mdb""")
        NUnit.Framework.Assert.IsFalse(ReportContents.Contains("Undelimited SSNs.mdb"), "Oops, scanned the Mdb file.")
        NUnit.Framework.Assert.IsFalse(ReportContents.Contains("List of undelimited SSNs.completelyUnknownExtension"), "Oops, we scanned the Unkown file.")
    End Sub
    <Test()> Public Sub ScanEverything_SkipMdb()
        RunScan("/se /skipExt ""mdb""")
        NUnit.Framework.Assert.IsFalse(ReportContents.Contains("Undelimited SSNs.mdb"), "Oops, scanned the Mdb file.")
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("List of undelimited SSNs.completelyUnknownExtension"), "Oops, we didn't scan the Unkown file.")
    End Sub
    <Test()> Public Sub ScanUnknown_SkipMdb()
        RunScan("/su /skipExt ""mdb""")
        NUnit.Framework.Assert.IsFalse(ReportContents.Contains("Undelimited SSNs.mdb"), "Oops, scanned the Mdb file.")
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("List of undelimited SSNs.completelyUnknownExtension"), "Oops, we didn't scan the Unkown file.")
    End Sub
#End Region

#Region "Redirections"
    <Test()> Public Sub Redirect_Log()
        LogFolder = "C:\Documents and Settings\delaport\Desktop\Firefly Files\TestRedirectLog"
        File.Delete(LogFolder + "\Firefly SSN Finder.log")
        RunScan("/logpath """ + LogFolder + """")
        NUnit.Framework.Assert.IsTrue(File.Exists(LogFolder + "\Firefly SSN Finder.log"))
    End Sub

    <Test()> Public Sub Redirect_Report()

        ReportFolder = "C:\Documents and Settings\delaport\Desktop\Firefly Files\TestRedirectReport"
        ReportLocation = ReportFolder + "\Firefly SSN Finder Report.htm"
        RunScan("/reportpath """ + ReportFolder + """")
        NUnit.Framework.Assert.IsTrue(File.Exists(ReportFolder + "\Firefly SSN Finder Report.htm"))
        ReportLocation = "C:\Documents and Settings\delaport\Desktop\Firefly SSN Finder Report.htm"
    End Sub

    <Test()> Public Sub Redirect_LogAndReport()
        LogFolder = "C:\Documents and Settings\delaport\Desktop\Firefly Files\TestRedirectLog"
        File.Delete(LogFolder + "\Firefly SSN Finder.log")

        ReportFolder = "C:\Documents and Settings\delaport\Desktop\Firefly Files\TestRedirectReport"
        ReportLocation = ReportFolder + "\Firefly SSN Finder Report.htm"

        Dim scanArgs As String = "/logpath """ + LogFolder + """" + " /reportpath """ + ReportFolder + """"
        RunScan(scanArgs)

        NUnit.Framework.Assert.IsTrue(File.Exists(LogFolder + "\Firefly SSN Finder.log"))
        NUnit.Framework.Assert.IsTrue(File.Exists(ReportFolder + "\Firefly SSN Finder Report.htm"))
        ReportLocation = "C:\Documents and Settings\delaport\Desktop\Firefly SSN Finder Report.htm"
    End Sub

    <Test()> Public Sub Redirect_ReportAndLog()
        LogFolder = "C:\Documents and Settings\delaport\Desktop\Firefly Files\TestRedirectLog"
        File.Delete(LogFolder + "\Firefly SSN Finder.log")

        ReportFolder = "C:\Documents and Settings\delaport\Desktop\Firefly Files\TestRedirectReport"
        ReportLocation = ReportFolder + "\Firefly SSN Finder Report.htm"

        Dim scanArgs As String = "/reportpath """ + ReportFolder + """" + " /logpath """ + LogFolder + """"
        RunScan(scanArgs)

        NUnit.Framework.Assert.IsTrue(File.Exists(LogFolder + "\Firefly SSN Finder.log"))
        NUnit.Framework.Assert.IsTrue(File.Exists(ReportFolder + "\Firefly SSN Finder Report.htm"))
        ReportLocation = "C:\Documents and Settings\delaport\Desktop\Firefly SSN Finder Report.htm"
    End Sub
#End Region

End Class

'cmdLineOpts.Add("/h or /? or /help outputs this text.")
'cmdLineOpts.Add("/path <directory> scans the directory specified and all subdirectories.")
'cmdLineOpts.Add("/priority <1,2,3 or 4> sets the run priority low(1) or high(4).")
'cmdLineOpts.Add("/reportpath <directory> places the report in the directory specified.")
'cmdLineOpts.Add("/logpath <directory> places the log in the directory specified.")
'cmdLineOpts.Add("/su scans unrecognized files as if they were plain text.")
'cmdLineOpts.Add("/se scans everything (takes a long time).")
'cmdLineOpts.Add("/skipExt 'ext1 ext2' adds extensions ext1 and ext2 to the skipped extensions list.")
'cmdLineOpts.Add("/scanList <text file> scans the files listed in the text file specified.")
'cmdLineOpts.Add("/threshold <number> or /t <number> reports only files with <number> or more potential matches.")
'cmdLineOpts.Add("/unattended causes " + My.Application.Info.ProductName + " to run unattended.")