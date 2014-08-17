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


Public Enum Validator
    cleared = 0
    None = 1
    SSN = 2
    CCN = 4
End Enum
Public Enum ValidatorTypes
    cleared = 0
    CanadianSIN = 1
    CreditCardPrefixes = 2
    US_SSN = 4
End Enum

Module Validators
    Public Function ValidateMatch(ByVal regExp As RegexEntry, ByVal match As String) As Boolean

        Select Case regExp.RegValidator
            Case Validator.cleared
                Return True
            Case Validator.None
                Return True
            Case Validator.SSN
                Return check_SSN(match)
            Case Validator.CCN
                Return check_Luhn(match, regExp.RegValidatorExtra)
        End Select

    End Function

    Public Function check_Luhn(ByVal CCnum As String, ByVal revalidextra As ValidatorTypes) As Boolean
        ' returns True if the Luhn check succeeds, false otherwise
        Dim digits(16) As Integer
        Dim i As Integer
        Dim index As Integer
        Dim start As Integer
        Dim twice As Integer
        Dim sum As Integer = 0
        Dim pchar(256) As Char

        ' even before first, prune non digits off the front/back
        If CCnum.Length = 0 Then
            Return False
        End If

        index = 0
        For i = 0 To 255
            If Not Char.IsDigit(Chr(i)) Then
                pchar(index) = Chr(i)
                index += 1
            End If
        Next

        CCnum.TrimStart(pchar)
        CCnum.TrimEnd(pchar)

        ' first, strip delimiters
        CCnum = CCnum.Replace("-", "")
        CCnum = CCnum.Replace(" ", "")
        CCnum = CCnum.Replace(".", "")

        Debug.WriteLine("LUHN: " + (CCnum.Length Mod 2).ToString)

        ' if this is supposed to be a credit card number, Diners doesn't
        ' use Luhn, but does have recognizable prefixes
        If revalidextra = ValidatorTypes.CreditCardPrefixes Then
            If CCnum.StartsWith("2014") Or CCnum.StartsWith("2149") Then
                ' Luhn doesn't work with Diner's, so we just have to hope ...
                Return True
            End If

            ' Check the hashtable of credit card prefixes.
            '   If we don't have a match, return false
            '   If we do have a match, continue with further validation.

            ' If we have a valid list to check against, make sure this item has a valid prefix.
            '  - If we don't have a list to check against, assume the prefix is valid and continue.
            Dim ccpref_match As Boolean = False
            If CCPrefixes.Count > 0 Then
                ' Only continue validating if we find a match.
                For i = 1 To 6
                    If CCPrefixes.Contains(CCnum.Substring(0, i)) Then
                        Debug.WriteLine("Testing: " + CCnum.Substring(0, i))
                        ccpref_match = True
                        Exit For
                    End If
                Next
                If Not ccpref_match Then
                    ' The prefix isn't in our list, this can't be a CCN.
                    Return False
                End If
            End If

        End If

        ' handle Luhn checks of arbitrary length
        ' even number of digits, start doubling at first digit
        ' odd number of digits, start doubling at second digit

        start = CCnum.Length Mod 2

        For i = 0 To (CCnum.Length - 1)
            digits(i) = Convert.ToInt32(CCnum.Substring(i, 1))
            ' Debug.WriteLine("digit: " + digits(i).ToString)
        Next

        ' SIN test
        If revalidextra = ValidatorTypes.CanadianSIN Then
            ' a first digit of 0 or 8 is an invalid SIN
            ' 9s are temporary residents, virtually none of which ought to be valid after 4/2004,
            ' but we'll assume they're valid nonetheless
            If digits(0) = 8 Or digits(0) = 0 Then
                Return False
            End If
        End If

        For i = start To (CCnum.Length - 1) Step 2
            twice = digits(i) * 2
            If twice >= 10 Then
                digits(i) = twice - 9
            Else
                digits(i) = twice
            End If
        Next

        For i = 0 To (CCnum.Length - 1)
            sum += digits(i)
        Next
        If (sum Mod 10) = 0 Then
            Return True
        End If

        Return False

    End Function
    Public Function LoadValidators(ByVal xmlPath As String, ByVal revalidextra As ValidatorTypes) As Boolean

        Try
            Dim xmld As XmlDocument
            Dim nodelist As XmlNodeList
            Dim node As XmlNode
            xmld = New XmlDocument
            If xmlPath = "ssn_area_codes" Then
                xmld.LoadXml(My.Resources.ssn_area_codes)
            ElseIf xmlPath = "ccn_area_codes" Then
                xmld.LoadXml(My.Resources.ccn_area_codes)
            Else
                Throw New Exception("Bad resource requested: " + xmlPath)
            End If

            ' xmld.Load(readPath)
            nodelist = xmld.SelectNodes("/areas/name")
            For Each node In nodelist
                Dim area As String = node.Attributes.GetNamedItem("area").Value
                Dim group As String = node.ChildNodes.Item(0).InnerText
                If revalidextra = ValidatorTypes.US_SSN Then
                    If Not Area2Group.Contains(area) Then
                        Area2Group.Add(area, group)
                    End If
                ElseIf revalidextra = ValidatorTypes.CreditCardPrefixes Then
                    If Not CCPrefixes.Contains(area) Then
                        CCPrefixes.Add(area, group)
                    End If
                End If
            Next
        Catch ex As Exception
            LogError(New Exception("Unable to load settings from " + xmlPath, ex), True)
        End Try

        Return True
    End Function
    Public Function check_SSN(ByVal SSN As String) As Boolean
        Try
            ' Returns False if we detect that this is not a valid SSN.
            ' Returns True otherwise.

            Dim area As String
            Dim group As String
            Dim sString As String

            SSN = SSN.Replace("-", "")
            SSN = SSN.Replace(" ", "")

            '' we've got to have at least 9 characters
            'If SSN.Length < 9 Then
            '    Return False
            'End If

            If Area2Group.Count < 1 Then
                ' We don't have a valid list to check against.
                ' For all we know, this is a valid SSN.
                Return True
            End If

            area = SSN.Substring(0, 3)

            group = SSN.Substring(3, 2)

            If Area2Group.Contains(area) Then
                sString = Area2Group.Item(area)
                Return (maxarea(Convert.ToInt32(sString), Convert.ToInt32(group)))
            End If
        Catch ex As Exception
            LogError(New Exception("An error occurred when checking the SSN for validity.", ex), True)
        End Try
        Return False

    End Function
    Public Function maxarea(ByVal govmax As Integer, ByVal testarea As Integer) As Boolean
        ' returns true if the testarea passed us fits within the pattern the SSA uses to 
        ' generate areas, false otherwise
        Dim ret As Boolean = False
        Dim i As Integer
        'Dim j As Integer

        ' areas of 99 are full, all others are used
        If govmax = 99 Then
            Debug.WriteLine("all groups issued")
            Return True
        End If

        i = 1

        ' odd areas, 1 through 9

        While (i <= 9)
            If testarea = i Then
                ret = True
                Return ret
            End If
            If i = govmax Then
                Return ret
            End If
            i += 2
        End While

        i = 10

        ' even areas, 10 through 98

        While (i <= 98)
            If testarea = i Then
                ret = True
                Return ret
            End If
            If i = govmax Then
                Return ret
            End If
            i += 2
        End While

        i = 2

        ' even areas 2 through 8

        While (i <= 8)
            If testarea = i Then
                ret = True
                Return ret
            End If
            If i = govmax Then
                Return ret
            End If
            i += 2
        End While

        ' odd areas 11 through 99

        i = 11

        While (i <= 99)
            If testarea = i Then
                ret = True
                Return ret
            End If
            If i = govmax Then
                Return ret
            End If
            i += 2
        End While

        Return ret

    End Function
End Module
