Imports System.IO 'File access

Public Class TestingTools
    Public Shared Function RunScan_Details(ByVal arguments As String, ByVal ReportLocation As String, ByVal ScanLocation As String) As String
        Dim ReportContents As String
        Console.WriteLine("Arguments: " + arguments)
        '' Run the firefly scan here.
        File.Delete(ReportLocation)
        Dim x As Process
        Dim args As String
        args = "/unattended /path """ + ScanLocation + """ " + arguments
        x = Process.Start("C:\Documents and Settings\delaport\My Documents\ISE Projects\07-02-04SSNFinder\spider-4.0\spider-2.0\bin\Firefly SSN Scanner.exe", args)
        x.WaitForExit()
        '' Read the contents of the report.
        Dim objReader As StreamReader
        Try
            objReader = New StreamReader(ReportLocation)
            ReportContents = objReader.ReadToEnd()
            objReader.Close()
        Catch ex As Exception
            Throw ex
        End Try

        Return ReportContents
    End Function
End Class
