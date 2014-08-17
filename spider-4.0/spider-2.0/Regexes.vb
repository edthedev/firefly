Module Regexes
    Public Const Break_desc As String = "The expression will match if offset by word breaks."
    Public Const SSN324_regex As String = "\d{3}-\d{2}-\d{4}"
    Public Const SSN9_regex As String = "\d{9}"
    Public Const SIN9_regex As String = SSN9_regex
    Public Const SIN333_regex As String = "\d{3}-\d{3}-\d{3}"
    Public Const VMCD_regex As String = "\d{4}-\d{4}-\d{4}-\d{4}"
    Public Const AMEX_regex As String = "\d{4}-\d{6}-\d{5}"
    Public Const NINO_regex As String = "NINO"
    Public Const SSN324_desc As String = "A regular expression to find data whose pattern matches a US Social Security Number"
    Public Const SSN9_desc As String = "A regular expression to find data whose pattern is 9 consecutive digits"
    Public Const SIN9_desc As String = SSN9_desc
    Public Const SIN333_desc As String = "A regular expression to find data whose pattern matches a Canadian SIN written in groups of 3 digits"
    Public Const VMCD_desc As String = "A regular expression to match common 16-digit credit card numbers"
    Public Const AMEX_desc As String = "A regular expression to match common 15-digit credit card numbers"
    Public Const NINO_desc As String = "A regular expression to match UK NINOs"
    Public Const UID_regex As String = ""

End Module
