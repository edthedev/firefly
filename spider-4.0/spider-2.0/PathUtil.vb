Imports Microsoft.Win32
' Imports mshtml
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Text
Imports System.Net
Imports System.Net.Sockets
Imports System.Security.Cryptography
Imports System.Reflection
Imports System.Xml
Imports System.Data.OleDb
Imports System.Threading

Imports ICSharpCode.SharpZipLib
' Imports AlternateDataStream
Imports iTextSharp

Module PathUtil
    Public Function MatchRegex(ByVal pPath As String, ByVal pattern As String) As Boolean
        Dim reGlob As New Regex(pattern, RegexOptions.IgnoreCase)

        If reGlob.IsMatch(pPath) Then
            Return True
        Else
            Return False
        End If
    End Function
    'Public Sub paths2globs(ByVal inHash As Hashtable, ByRef outHash As Hashtable)
    '    Dim newglob As String
    '    Dim i As Integer
    '    Dim glob As String

    '    If outHash.Count <> 0 Then
    '        outHash.Clear()
    '    End If

    '    newglob = ""

    '    Dim en As IDictionaryEnumerator = inHash.GetEnumerator
    '    While en.MoveNext
    '        glob = en.Key
    '        For i = 0 To glob.Length - 1
    '            Select Case glob.Substring(i, 1)
    '                Case Chr(46)  ' .
    '                    ' Escape the character with \
    '                    newglob += Chr(92) + glob.Substring(i, 1)
    '                Case Chr(40)  ' (
    '                    ' Escape the character with \
    '                    newglob += Chr(92) + glob.Substring(i, 1)
    '                Case Chr(41)  ' )
    '                    ' Escape the character with \
    '                    newglob += Chr(92) + glob.Substring(i, 1)
    '                Case Chr(94)  ' ^
    '                    ' Escape the character with \
    '                    newglob += Chr(92) + glob.Substring(i, 1)
    '                Case Chr(43)  ' +
    '                    ' Escape the character with \
    '                    newglob += Chr(92) + glob.Substring(i, 1)
    '                Case Chr(123) ' {
    '                    ' Escape the character with \
    '                    newglob += Chr(92) + glob.Substring(i, 1)
    '                Case Chr(125) ' }
    '                    ' Escape the character with \
    '                    newglob += Chr(92) + glob.Substring(i, 1)
    '                Case Chr(36)  ' $
    '                    ' Escape the character with \
    '                    newglob += Chr(92) + glob.Substring(i, 1)
    '                Case Chr(63)  ' ?
    '                    ' Replace ? with .
    '                    newglob += Chr(46)
    '                Case Chr(42)  ' *
    '                    ' Replace * with .*
    '                    newglob += Chr(46) + Chr(42)
    '                Case Chr(92) ' \
    '                    ' Escape the directory separator with \
    '                    newglob += Chr(92) + Chr(92)
    '                Case Chr(58) ' :
    '                    ' Escape the directory separator with \
    '                    newglob += Chr(92) + Chr(58)
    '                Case Else
    '                    ' Everything else just gets put back where it was.
    '                    newglob += glob.Substring(i, 1)
    '            End Select
    '        Next
    '        outHash.Add(glob, newglob)
    '        newglob = ""
    '    End While

    '    Return

    'End Sub

    'Public Function expand_path(ByVal inPath As String) As String
    '    Dim i As Integer
    '    Dim aChars(1) As Char
    '    Dim now As Date = System.DateTime.Now
    '    Dim returnString As String

    '    ReDim aChars(inPath.Length - 1)

    '    aChars = inPath.ToCharArray
    '    returnString = ""

    '    For i = 0 To aChars.GetUpperBound(0)
    '        '  Debug.WriteLine("here: " + aChars(i))
    '        Select Case aChars(i)
    '            Case "%"
    '                i += 1
    '                Select Case aChars(i)
    '                    Case ""
    '                        Exit For
    '                    Case "F"
    '                        returnString += vbCrLf
    '                    Case "r"
    '                        returnString += System.Byte.Parse("13")
    '                    Case "l"
    '                        returnString += System.Byte.Parse("10")
    '                    Case "H"
    '                        returnString += TotalHits.ToString
    '                    Case "C"
    '                        returnString += files_processed.ToString
    '                    Case "i"
    '                        ' IP address
    '                        ' Dim ipE As IPHostEntry = Dns.GetHostByName(Dns.GetHostName())
    '                        Dim ipE As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName())
    '                        returnString += ipE.AddressList(0).ToString
    '                    Case "N"
    '                        ' add hostname
    '                        returnString += Dns.GetHostName.ToLower()
    '                    Case "n"
    '                        ' computername
    '                        returnString += SystemInformation.ComputerName
    '                    Case "R"
    '                        ' run time; difference in Firefly_start and Firefly_end
    '                        returnString += DateDiff(DateInterval.Second, Firefly_start, Firefly_end).ToString
    '                    Case "u"
    '                        ' username
    '                        returnString += SystemInformation.UserName
    '                    Case "%"
    '                        returnString += "%"
    '                    Case "d"
    '                        ' day of week, numeric
    '                        returnString += now.Day.ToString
    '                    Case "D"
    '                        ' day of week, english
    '                        Select Case now.DayOfWeek
    '                            Case DayOfWeek.Monday
    '                                returnString += "Mon"
    '                            Case DayOfWeek.Tuesday
    '                                returnString += "Tue"
    '                            Case DayOfWeek.Wednesday
    '                                returnString += "Wed"
    '                            Case DayOfWeek.Thursday
    '                                returnString += "Thu"
    '                            Case DayOfWeek.Friday
    '                                returnString += "Fri"
    '                            Case DayOfWeek.Saturday
    '                                returnString += "Sat"
    '                            Case DayOfWeek.Sunday
    '                                returnString += "Sun"
    '                        End Select
    '                    Case "P"
    '                        returnString += now.DayOfYear.ToString
    '                    Case "m"
    '                        ' month, numeric
    '                        returnString += now.Month.ToString
    '                    Case "M"
    '                        ' month, english
    '                        Select Case now.Month
    '                            Case 1
    '                                returnString += "Jan"
    '                            Case 2
    '                                returnString += "Feb"
    '                            Case 3
    '                                returnString += "Mar"
    '                            Case 4
    '                                returnString += "Apr"
    '                            Case 5
    '                                returnString += "May"
    '                            Case 6
    '                                returnString += "Jun"
    '                            Case 7
    '                                returnString += "Jul"
    '                            Case 8
    '                                returnString += "Aug"
    '                            Case 9
    '                                returnString += "Sep"
    '                            Case 10
    '                                returnString += "Oct"
    '                            Case 11
    '                                returnString += "Nov"
    '                            Case 12
    '                                returnString += "Dec"
    '                        End Select
    '                    Case "y"
    '                        ' four digit year
    '                        returnString += now.Year.ToString
    '                    Case "T"
    '                        ' time in HHMMSS format
    '                        If now.Hour <= 9 Then
    '                            returnString += "0" + now.Hour.ToString
    '                        Else
    '                            returnString += now.Hour.ToString
    '                        End If
    '                        If now.Minute <= 9 Then
    '                            returnString += "0" + now.Minute.ToString
    '                        Else
    '                            returnString += now.Minute.ToString
    '                        End If
    '                        If now.Second <= 9 Then
    '                            returnString += "0" + now.Second.ToString
    '                        Else
    '                            returnString += now.Second.ToString
    '                        End If
    '                End Select
    '            Case Else
    '                returnString += aChars(i)
    '        End Select
    '    Next
    '    '   Debug.WriteLine("new log path: " + returnString)

    '    Return returnString
    'End Function

    Public Function quote_path(ByVal pPath As String) As String
        Dim i As Integer = 0

        Debug.WriteLine("inside quote_path")
        If InStr(pPath, ",") Or InStr(pPath, System.Byte.Parse("34")) Then
            Debug.WriteLine("path needs fixing")
            '  Debug.WriteLine("here; quote_path")
            quote_path = ""
            quote_path += Chr(34)
            For i = 0 To pPath.Length - 1
                '                Debug.WriteLine("CSV: " + pPath.Substring(i, 1))
                If pPath.Substring(i, 1) = Chr(34) Then
                    quote_path += Chr(34)
                Else
                    quote_path += pPath.Substring(i, 1)
                End If
            Next
            If Not quote_path.EndsWith(Chr(34)) Then
                quote_path += Chr(34)
            End If
            Return quote_path
        End If

        Return pPath

    End Function
End Module
