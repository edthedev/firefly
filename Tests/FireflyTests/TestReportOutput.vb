Imports NUnit.Framework 'Test Cases Exposed to NUnit
Imports System.IO 'File access

<TestFixture()> Public Class TestReportOutput

    Private ReportContents As String
    Private ReportLocation As String = "C:\Documents and Settings\delaport\Desktop\Firefly SSN Finder Report.htm"
    Private FileLock As FileStream
    <TestFixtureSetUp()> Public Sub RunScan()
        '' Get file lock.
        'FileLock = New FileStream("C:\Documents and Settings\delaport\Desktop\SSN\FileLock.txt", FileMode.Append, FileAccess.Write)
        'FileLock.Lock(0, 100)

        '' Run the firefly scan here.
        File.Delete(ReportLocation)
        Dim x As Process
        Dim arguments As String
        arguments = "/unattended /path ""C:\Documents and Settings\delaport\Desktop\SSN"" "
        x = Process.Start("C:\Documents and Settings\delaport\My Documents\ISE Projects\07-02-04SSNFinder\spider-4.0\spider-2.0\bin\Firefly SSN Scanner.exe", arguments)
        x.WaitForExit()

        ' Shell(, AppWinStyle.MinimizedNoFocus, True)
        Dim objReader As StreamReader
        Try
            objReader = New StreamReader(ReportLocation)
            ReportContents = objReader.ReadToEnd()
            objReader.Close()
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    <TestFixtureTearDown()> Public Sub ReleaseLock()
        ' Release file lock.
        ' FileLock.Unlock(0, 100)
    End Sub

    '<Test()> Public Sub LockedFile()
    '    NUnit.Framework.Assert.IsTrue(ReportContents.Contains("LockedFile.txt"))
    'End Sub

    <Test()> Public Sub ReportExists()
        NUnit.Framework.Assert.IsTrue(File.Exists(ReportLocation))
    End Sub

    <Test()> Public Sub Access_UnDelimitedSsns()
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("Undelimited SSNs.mdb"))
    End Sub

    <Test()> Public Sub Word_DelimitedSsns()
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("List of delimited SSNs.doc"))
    End Sub

    <Test()> Public Sub Word_UnDelimitedSsns()
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("List of undelimited SSNs.doc"))
    End Sub

    <Test()> Public Sub Word2007_DelimitedSsns()
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("List of delimited SSNs.docx"))
    End Sub

    <Test()> Public Sub Word2007_UnDelimitedSsns()
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("List of undelimited SSNs.docx"))
    End Sub

    <Test()> Public Sub Word2007_DelimitedCcns()
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("List of delimited CCNs.docx"))
    End Sub

    <Test()> Public Sub Word2007_UnDelimitedCcns()
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("List of undelimited CCNs.docx"))
    End Sub

    <Test()> Public Sub Text_DelimitedSsns()
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("List of delimited SSNs.txt"))
    End Sub

    <Test()> Public Sub Text_UnDelimitedSsns()
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("List of undelimited SSNs.txt"))
    End Sub

    <Test()> Public Sub Text_JustTwoSsns()
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("Just 2 SSNs.txt"))
    End Sub

    ' Todo: Break this into a few different tests: Mastercard, Visa, etc.
    <Test()> Public Sub Text_UnDelimitedCcns()
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("List of undelimited CCNs.txt"))
    End Sub

    <Test()> Public Sub Text_DelimitedCcns()
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("List of delimited CCNs.txt"))
    End Sub

    <Test()> Public Sub Excel_UnDelimitedSsns()
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("List of undelimited SSNs.xls"))
    End Sub

    <Test()> Public Sub Excel2007_UnDelimitedSsns()
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("List of undelimited SSNs.xlsx"))
    End Sub

    <Test()> Public Sub Archive_Tar()
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("SSN.tar"))
    End Sub

    <Test()> Public Sub Archive_Zip()
        NUnit.Framework.Assert.IsTrue(ReportContents.Contains("SSN.zip"))
    End Sub

    <Test()> Public Sub SkipList_WithinZip()
        NUnit.Framework.Assert.IsFalse(ReportContents.Contains("ShouldSkip.zip"))
    End Sub

    <Test()> Public Sub Skip_TenDigitNumbers()
        NUnit.Framework.Assert.IsFalse(ReportContents.Contains("List of ten digit numbers.txt"))
    End Sub

    <Test()> Public Sub Skip_Images()
        NUnit.Framework.Assert.IsFalse(ReportContents.Contains("List of undelimited SSNs.png"))
        NUnit.Framework.Assert.IsFalse(ReportContents.Contains("List of undelimited SSNs.jpg"))
        NUnit.Framework.Assert.IsFalse(ReportContents.Contains("List of undelimited SSNs.bmp"))
    End Sub

End Class
