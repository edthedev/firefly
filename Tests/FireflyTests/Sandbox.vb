Imports NUnit.Framework 'Test Cases Exposed to NUnit
Imports System.IO 'File access

<TestFixture()> Public Class Sandbox

    Private FileLock As FileStream
    Private FileName As String = "C:\Documents and Settings\delaport\Desktop\SSN\FileLock.txt"
    '<TestFixtureSetUp()> Public Sub LockFile()
    '    ' Get file lock.
    '    FileLock = New FileStream(FileName, FileMode.Append, FileAccess.Write)
    '    FileLock.Lock(0, 100)
    'End Sub

    '<Test()> Public Sub WriteLockFile()
    '    Dim FL2 As New FileStream(FileName, FileMode.Append, FileAccess.Write)
    '    FL2.Lock(0, 100)
    '    NUnit.Framework.Assert.IsTrue(FL2.CanWrite)
    '    FL2.Unlock(0, 100)
    '    FL2.Dispose()
    'End Sub

    '<Test()> Public Sub ReadLockFile()
    '    Dim FL2 As New FileStream(FileName, FileMode.Open, FileAccess.Read)
    '    FL2.Lock(0, 100)
    '    NUnit.Framework.Assert.IsTrue(FL2.CanRead)
    '    FL2.Unlock(0, 100)
    '    FL2.Dispose()
    'End Sub

    '<Test()> Public Sub ReadLockedFile()
    '    Dim oread As StreamReader
    '    oread = File.OpenText(FileName)
    '    NUnit.Framework.Assert.IsTrue(oread.Peek())
    'End Sub

    '<TestFixtureTearDown()> Public Sub ReleaseLock()
    '    ' Release file lock.
    '    FileLock.Unlock(0, 100)
    '    FileLock.Dispose()
    'End Sub
End Class
