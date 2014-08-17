Option Explicit On 

Imports Microsoft.Win32
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Windows.Forms.FormWindowState
Imports System.IO
Imports System.Text
Imports System.Net
Imports System.Reflection
Imports System.Net.Sockets
Imports System.Security.Cryptography
Imports System.Xml
Imports System.Data.OleDb
Imports System.Diagnostics.Process

Imports Firefly.Org.Mentalis.Utilities


Module Module2
    ' public
    Public Enum WipeTarget
        File = 1
        Slack = 2
    End Enum
    Public Enum WipeMethod
        NOOP = 0
        Delete = 1
        Overwrite_with_nulls = 2
        DoD_3Pass = 3
        DoD_7Pass = 7
        Truncate = 64
    End Enum
    Public Enum WipeWhenDone
        NOOP = 0
        Delete = 1
        Truncate = 2
    End Enum
    Public Enum FillBytes
        NULLS = 0
        Random = 1
        Spec = 2
    End Enum

    Public Declare Ansi Function SetFileValidData Lib "kernel32" (ByVal handle As System.IntPtr, ByVal offT As Int64) As Boolean
    Public Declare Ansi Function SetFilePointer Lib "kernel32" (ByVal handle As System.IntPtr, ByVal offT As Int32, ByRef offTHigh As Int32, ByVal method As UInt32) As Long
    Public Declare Ansi Function SetEndOfFile Lib "kernel32" (ByVal handle As System.IntPtr) As Boolean
    Public Declare Ansi Function CloseHandle Lib "kernel32" (ByVal handle As System.IntPtr) As Boolean
    Public Declare Ansi Function GetLastError Lib "kernel32" () As UInt32
    Public Declare Ansi Function GetCurrentProcess Lib "kernel32" () As Long
    Public Declare Ansi Function GetCurrentProcessId Lib "kernel32" () As Long

    Public Sub AdjustTokenDisk()
        WindowsController.EnableToken("SeManageVolumePrivilege")
        SMVN_priv = True
    End Sub
    'Public Function wipe_block(ByVal hdl As System.IntPtr, ByVal length As Long, ByVal startpos As Int64, ByVal method As WipeMethod) As Boolean
    '    ' returns true if the wipe succeeded, false otherwise
    '    ' we currently only implement overwrite-with-nulls
    '    Dim cBuf(65536) As Byte
    '    Dim towrite As Long
    '    Dim writesize As Long

    '    If Not method = WipeMethod.Overwrite_with_nulls Then
    '        Return False
    '    End If

    '    fill_bytes(cBuf, 0, FillBytes.NULLS)

    '    Try
    '        Dim wS As New FileStream(hdl, FileAccess.Write)
    '        Dim bW As New BinaryWriter(wS)
    '        If Not wS.Position = startpos Then
    '            wS.Position = startpos
    '        End If
    '        wS.Lock(0, length)
    '        towrite = length
    '        While towrite > 0
    '            While towrite > 0
    '                If (towrite > 65536) Then
    '                    writesize = 65536
    '                Else
    '                    writesize = towrite
    '                End If
    '                bW.Write(cBuf, 0, writesize)
    '                ' wP.Write(b)
    '                towrite -= writesize
    '                bW.Flush()
    '            End While
    '        End While
    '        bW.Flush()
    '        wS.Close()
    '        Return True
    '    Catch ex As Exception
    '        Return False
    '    End Try

    '    Return False

    'End Function

    'Public Function wipe(ByVal pPath As String, ByVal target As WipeTarget, ByVal method As WipeMethod, ByVal whendone As WipeWhenDone) As Boolean
    '    ' we'll wipe the path provided according to the method specified
    '    ' what we really ought to do is simply import EraserDLL and use it's methods
    '    ' hmmm.
    '    Dim b(65536) As Byte
    '    Dim fSize As Long
    '    Dim newname As String = ""
    '    Dim writesize As Long = 0
    '    Dim towrite As Long = 0

    '    If method = WipeMethod.NOOP Then
    '        Return True
    '    End If

    '    If method = WipeMethod.Truncate Then
    '        Try
    '            Dim oP As New FileStream(pPath, FileMode.Truncate, FileAccess.ReadWrite)
    '            oP.SetLength(0)
    '            oP.Close()
    '        Catch ex As Exception
    '            Debug.WriteLine("failed to truncate: " + pPath)
    '            Return False
    '        End Try
    '        Return True
    '    End If

    '    If method = WipeMethod.Delete Then
    '        ' nothing more than a straight up file deletion
    '        Try
    '            System.IO.File.Delete(pPath)
    '        Catch ex As Exception
    '            ' something went wrong
    '            Return False
    '        End Try
    '        Return True
    '    End If

    '    ' for the actual overwrite methods, we have alot of ground to cover:
    '    ' overwrite the file according to the spec

    '    ' right now, we can do either slack or the whole file, but not both
    '    ' if the application requires us to get both, we'll have to get slack first,
    '    ' then the file.

    '    ' we've got a problem with MFT resident data that makes this ugly.  Basically, any 
    '    ' file under 800 bytes just gets a file-only wipe

    '    ' clobbering slack is pretty straightforward -- we won't have to resort to the SeManageVolumeName
    '    ' privs we need to actually scan it.  The OS will give us a free zeroing pass the instant we 
    '    ' lengthen the file to write to it.

    '    ' set its times back to the epoch
    '    ' rename the file to something random
    '    ' call this function with WipeMethod.Delete

    '    ' IFF *all* these steps succeed, return true
    '    Try
    '        fSize = FileLen(pPath)
    '        Debug.WriteLine("file size: " + fSize.ToString)
    '    Catch
    '        Return False
    '    End Try

    '    towrite = fSize

    '    If method = WipeMethod.Overwrite_with_nulls Then
    '        fill_bytes(b, 0, FillBytes.NULLS)
    '        ' fSize = FileLen(pPath)
    '        Try
    '            Dim oP As New System.IO.FileStream(pPath, IO.FileMode.Open)
    '            Dim wP As New BinaryWriter(oP)
    '            wP.BaseStream.Position = 0
    '            While towrite > 0
    '                If (towrite > 65536) Then
    '                    writesize = 65536
    '                Else
    '                    writesize = towrite
    '                End If
    '                wP.Write(b, 0, writesize)
    '                ' wP.Write(b)
    '                towrite -= writesize
    '                wP.Flush()
    '            End While
    '            wP.Flush()
    '            oP.Close()
    '        Catch ex As Exception
    '            Debug.WriteLine("exception: " + ex.ToString)
    '            Return False
    '        End Try
    '    End If

    '    If method = WipeMethod.DoD_3Pass Then
    '        ' this is three passes:
    '        ' all zeros
    '        ' all ones
    '        ' random data
    '        fill_bytes(b, 0, FillBytes.NULLS)
    '        'fSize = FileLen(pPath)
    '        Try
    '            Dim oP As New System.IO.FileStream(pPath, FileMode.Open)
    '            Dim wP As New BinaryWriter(oP)
    '            wP.BaseStream.Position = 0
    '            While towrite > 0
    '                If (towrite > 65536) Then
    '                    writesize = 65536
    '                Else
    '                    writesize = towrite
    '                End If
    '                wP.Write(b, 0, writesize)
    '                towrite -= writesize
    '                wP.Flush()
    '            End While
    '            ' reset our position
    '            wP.BaseStream.Position = 0
    '            towrite = fSize
    '            fill_bytes(b, 255, FillBytes.Spec)
    '            While towrite > 0
    '                If (towrite > 65536) Then
    '                    writesize = 65536
    '                Else
    '                    writesize = towrite
    '                End If
    '                wP.Write(b, 0, writesize)
    '                towrite -= writesize
    '                wP.Flush()
    '            End While
    '            ' random; this is hard because we want really random
    '            towrite = fSize
    '            fill_bytes(b, 0, FillBytes.Random)
    '            While towrite > 0
    '                If (towrite > 65536) Then
    '                    writesize = 65536
    '                Else
    '                    writesize = towrite
    '                End If
    '                wP.Write(b, 0, writesize)
    '                towrite -= writesize
    '                wP.Flush()
    '                If fSize > 65536 Then
    '                    fill_bytes(b, 0, FillBytes.Random)
    '                End If
    '            End While
    '            ' should be done.
    '            wP.Flush()
    '            oP.Close()
    '        Catch ex As Exception
    '            Debug.WriteLine("exception in DoD 3-pass: " + ex.ToString)
    '            Return False
    '        End Try

    '    End If

    '    If method = WipeMethod.DoD_7Pass Then
    '        ' 7 passes, as follows:
    '        ' random character (0x55)
    '        ' its ones complement (0xAA)
    '        ' random data (random)
    '        ' nulls (0x00)
    '        ' random character (0x55)
    '        ' its ones complement (0xAA)
    '        ' random data (random)
    '        fill_bytes(b, 85, FillBytes.Spec)
    '        Try
    '            Dim oP As New System.IO.FileStream(pPath, FileMode.Open)
    '            Dim wP As New BinaryWriter(oP)
    '            wP.BaseStream.Position = 0
    '            towrite = fSize
    '            ' first pass
    '            While towrite > 0
    '                If (towrite > 65536) Then
    '                    writesize = 65536
    '                Else
    '                    writesize = towrite
    '                End If
    '                Debug.WriteLine("writesize: " + writesize.ToString)
    '                wP.Write(b, 0, writesize)
    '                wP.Flush()
    '                Debug.WriteLine("towrite: " + towrite.ToString)
    '                towrite -= writesize
    '                Debug.WriteLine("new towrite: " + towrite.ToString)
    '            End While
    '            ' second pass
    '            wP.BaseStream.Position = 0
    '            fill_bytes(b, 170, FillBytes.Spec)
    '            towrite = fSize
    '            While towrite > 0
    '                If (towrite > 65536) Then
    '                    writesize = 65536
    '                Else
    '                    writesize = towrite
    '                End If
    '                wP.Write(b, 0, writesize)
    '                towrite -= writesize
    '                wP.Flush()
    '            End While
    '            ' third pass, random
    '            wP.BaseStream.Position = 0
    '            fill_bytes(b, 0, FillBytes.Random)
    '            towrite = fSize
    '            While towrite > 0
    '                If (towrite > 65536) Then
    '                    writesize = 65536
    '                Else
    '                    writesize = towrite
    '                End If
    '                wP.Write(b, 0, writesize)
    '                wP.Flush()
    '                towrite -= writesize
    '                If fSize > 65536 Then
    '                    fill_bytes(b, 0, FillBytes.Random)
    '                End If
    '            End While
    '            ' fourth pass, nulls
    '            wP.BaseStream.Position = 0
    '            towrite = fSize
    '            fill_bytes(b, 0, FillBytes.NULLS)
    '            While towrite > 0
    '                If (towrite > 65536) Then
    '                    writesize = 65536
    '                Else
    '                    writesize = towrite
    '                End If
    '                wP.Write(b, 0, writesize)
    '                wP.Flush()
    '                towrite -= writesize
    '            End While
    '            ' fifth pass, character
    '            wP.BaseStream.Position = 0
    '            towrite = fSize
    '            fill_bytes(b, 85, FillBytes.Spec)
    '            While towrite > 0
    '                If (towrite > 65536) Then
    '                    writesize = 65536
    '                Else
    '                    writesize = towrite
    '                End If
    '                wP.Write(b, 0, writesize)
    '                wP.Flush()
    '                towrite -= writesize
    '            End While
    '            ' sixth pass, ones complement
    '            wP.BaseStream.Position = 0
    '            fill_bytes(b, 170, FillBytes.Spec)
    '            towrite = fSize
    '            While towrite > 0
    '                If (towrite > 65536) Then
    '                    writesize = 65536
    '                Else
    '                    writesize = towrite
    '                End If
    '                wP.Write(b, 0, writesize)
    '                wP.Flush()
    '                towrite -= writesize
    '            End While
    '            ' last pass, random
    '            wP.BaseStream.Position = 0
    '            fill_bytes(b, 0, FillBytes.Random)
    '            towrite = fSize
    '            While towrite > 0
    '                If (towrite > 65536) Then
    '                    writesize = 65536
    '                Else
    '                    writesize = towrite
    '                End If
    '                wP.Write(b, 0, writesize)
    '                wP.Flush()
    '                towrite -= writesize
    '                If fSize > 65536 Then
    '                    fill_bytes(b, 0, FillBytes.Random)
    '                End If
    '            End While
    '            ' should be done
    '            wP.Flush()
    '            oP.Close()
    '        Catch ex As Exception
    '            Debug.WriteLine("exception in DoD 7-pass: " + ex.ToString)
    '        End Try
    '    End If

    '    ' slack ends here
    '    If target = WipeTarget.Slack Then
    '        Return True
    '    End If


    '    ' only relevant for deletes
    '    If whendone = WipeWhenDone.Delete Then
    '        'End If
    '        Try
    '            Debug.WriteLine("resetting dates on: " + pPath)

    '            ' date setting
    '            Dim oldest As DateTime = DateTime.Parse("01/01/1970")
    '            System.IO.File.SetCreationTime(pPath, oldest.ToUniversalTime)

    '            System.IO.File.SetLastWriteTime(pPath, oldest.ToUniversalTime)

    '            System.IO.File.SetLastAccessTime(pPath, oldest.ToUniversalTime)
    '        Catch ex As Exception
    '            Debug.WriteLine("error setting dates: " + ex.ToString)
    '            Return False
    '        End Try

    '        ' only really applies if we intend to delete the file as the other two
    '        ' methods leave something in place.

    '        ' If whendone = WipeWhenDone.Delete Then
    '        Try
    '            ' rename file to something random
    '            fill_bytes(b, 0, FillBytes.Random)
    '            Dim md5 As New MD5CryptoServiceProvider
    '            Dim md5byte() As Byte = md5.ComputeHash(b)
    '            Dim buff As StringBuilder = New StringBuilder
    '            Dim hashbyte As Byte
    '            For Each hashbyte In md5byte
    '                buff.Append(String.Format("{0:X1}", hashbyte))
    '            Next
    '            Dim basename As String = System.IO.Directory.GetDirectoryRoot(pPath)
    '            newname = basename + "\" + buff.ToString
    '            System.IO.File.Move(pPath, newname)
    '        Catch ex As Exception
    '            Debug.WriteLine("file rename: " + ex.ToString)
    '            Return False
    '        End Try
    '    End If

    '    ' and that's it; what to do when we're done with the file:
    '    ' nothing; leave wiped file in place under its new name
    '    If whendone = WipeWhenDone.NOOP Then
    '        Return wipe(pPath, WipeTarget.File, WipeMethod.NOOP, WipeWhenDone.NOOP)
    '    End If

    '    ' delete the file when done
    '    If whendone = WipeWhenDone.Delete Then
    '        Return wipe(newname, WipeTarget.File, WipeMethod.Delete, WipeWhenDone.NOOP)
    '    End If

    '    ' truncate the file when done
    '    If whendone = WipeWhenDone.Truncate Then
    '        Return wipe(pPath, WipeTarget.File, WipeMethod.Truncate, WipeWhenDone.NOOP)
    '    End If

    '    ' unrecognized method, something went wrong, etc.
    '    Return False

    'End Function

    Public Sub fill_bytes(ByRef barray() As Byte, ByVal S As Byte, ByVal FillType As FillBytes)
        Dim i As Integer
        Dim C As Byte

        If FillType = FillBytes.Random Then
            ' random fills the whole array
            System.Security.Cryptography.RandomNumberGenerator.Create.GetNonZeroBytes(barray)
            Return
        End If

        ' everything else
        If FillType = FillBytes.Spec Then
            C = S
        End If

        If FillType = FillBytes.NULLS Then
            C = 0
        End If

        For i = 0 To barray.Length - 1
            barray(i) = C
        Next

        Return

    End Sub

End Module
