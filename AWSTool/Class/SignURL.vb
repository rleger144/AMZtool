
Imports System.Collections.Generic
Imports System.Text
Imports System.Web
Imports System.Security.Cryptography


Class SignedRequestHelper
    Private endPoint As String
    Private akid As String
    Private secret As Byte()
    Private signer As HMAC

    Private Const REQUEST_URI As String = "/onca/xml"
    Private Const REQUEST_METHOD As String = "GET"

    '
    '         * Use this constructor to create the object. The AWS credentials are available on
    '         * http://aws.amazon.com
    '         * 
    '         * The destination is the service end-point for your application:
    '         *  US: ecs.amazonaws.com
    '         *  JP: ecs.amazonaws.jp
    '         *  UK: ecs.amazonaws.co.uk
    '         *  DE: ecs.amazonaws.de
    '         *  FR: ecs.amazonaws.fr
    '         *  CA: ecs.amazonaws.ca
    '         

    Public Sub New(awsAccessKeyId As String, awsSecretKey As String, destination As String)
        Me.endPoint = destination.ToLower()
        Me.akid = awsAccessKeyId
        Me.secret = Encoding.UTF8.GetBytes(awsSecretKey)
        Me.signer = New HMACSHA256(Me.secret)
    End Sub

    '
    '         * Sign a request in the form of a Dictionary of name-value pairs.
    '         * 
    '         * This method returns a complete URL to use. Modifying the returned URL
    '         * in any way invalidates the signature and Amazon will reject the requests.
    '         

    Public Function Sign(request As IDictionary(Of String, String)) As String
        ' Use a SortedDictionary to get the parameters in naturual byte order, as
        ' required by AWS.
        Dim pc As New ParamComparer()
        Dim sortedMap As New SortedDictionary(Of String, String)(request, pc)

        ' Add the AWSAccessKeyId and Timestamp to the requests.
        sortedMap("AWSAccessKeyId") = Me.akid
        sortedMap("Timestamp") = Me.GetTimestamp()

        ' Get the canonical query string
        Dim canonicalQS As String = Me.ConstructCanonicalQueryString(sortedMap)

        ' Derive the bytes needs to be signed.
        Dim builder As New StringBuilder()
        builder.Append(REQUEST_METHOD).Append(vbLf).Append(Me.endPoint).Append(vbLf).Append(REQUEST_URI).Append(vbLf).Append(canonicalQS)

        Dim stringToSign As String = builder.ToString()
        Dim toSign As Byte() = Encoding.UTF8.GetBytes(stringToSign)

        ' Compute the signature and convert to Base64.
        Dim sigBytes As Byte() = signer.ComputeHash(toSign)
        Dim signature As String = Convert.ToBase64String(sigBytes)

        ' now construct the complete URL and return to caller.
        Dim qsBuilder As New StringBuilder()
        qsBuilder.Append("http://").Append(Me.endPoint).Append(REQUEST_URI).Append("?").Append(canonicalQS).Append("&Signature=").Append(Me.PercentEncodeRfc3986(signature))

        Return qsBuilder.ToString()
    End Function

    '
    '         * Sign a request in the form of a query string.
    '         * 
    '         * This method returns a complete URL to use. Modifying the returned URL
    '         * in any way invalidates the signature and Amazon will reject the requests.
    '         

    Public Function Sign(queryString As String) As String
        Dim request As IDictionary(Of String, String) = Me.CreateDictionary(queryString)
        Return Me.Sign(request)
    End Function

    '
    '         * Current time in IS0 8601 format as required by Amazon
    '         

    Private Function GetTimestamp() As String
        Dim currentTime As DateTime = DateTime.UtcNow
        Dim timestamp As String = currentTime.ToString("yyyy-MM-ddTHH:mm:ssZ")
        Return timestamp
    End Function

    '
    '         * Percent-encode (URL Encode) according to RFC 3986 as required by Amazon.
    '         * 
    '         * This is necessary because .NET's HttpUtility.UrlEncode does not encode
    '         * according to the above standard. Also, .NET returns lower-case encoding
    '         * by default and Amazon requires upper-case encoding.
    '         

    Private Function PercentEncodeRfc3986(str As String) As String
        str = HttpUtility.UrlEncode(str, System.Text.Encoding.UTF8)
        str = str.Replace("'", "%27").Replace("(", "%28").Replace(")", "%29").Replace("*", "%2A").Replace("!", "%21").Replace("%7e", "~").Replace("+", "%20")

        Dim sbuilder As New StringBuilder(str)
        For i As Integer = 0 To sbuilder.Length - 1
            If sbuilder(i) = "%"c Then
                If [Char].IsLetter(sbuilder(i + 1)) OrElse [Char].IsLetter(sbuilder(i + 2)) Then
                    sbuilder(i + 1) = [Char].ToUpper(sbuilder(i + 1))
                    sbuilder(i + 2) = [Char].ToUpper(sbuilder(i + 2))
                End If
            End If
        Next
        Return sbuilder.ToString()
    End Function

    '
    '         * Convert a query string to corresponding dictionary of name-value pairs.
    '         

    Private Function CreateDictionary(queryString As String) As IDictionary(Of String, String)
        Dim map As New Dictionary(Of String, String)()

        Dim requestParams As String() = queryString.Split("&"c)

        For i As Integer = 0 To requestParams.Length - 1
            If requestParams(i).Length < 1 Then
                Continue For
            End If

            Dim sep As Char() = {"="c}
            Dim param As String() = requestParams(i).Split(sep, 2)
            For j As Integer = 0 To param.Length - 1
                param(j) = HttpUtility.UrlDecode(param(j), System.Text.Encoding.UTF8)
            Next
            Select Case param.Length
                Case 1
                    If True Then
                        If requestParams(i).Length >= 1 Then
                            If requestParams(i).ToCharArray()(0) = "="c Then
                                map("") = param(0)
                            Else
                                map(param(0)) = ""
                            End If
                        End If
                        Exit Select
                    End If
                Case 2
                    If True Then
                        If Not String.IsNullOrEmpty(param(0)) Then
                            map(param(0)) = param(1)
                        End If
                    End If
                    Exit Select
            End Select
        Next

        Return map
    End Function

    '
    '         * Consttuct the canonical query string from the sorted parameter map.
    '         

    Private Function ConstructCanonicalQueryString(sortedParamMap As SortedDictionary(Of String, String)) As String
        Dim builder As New StringBuilder()

        If sortedParamMap.Count = 0 Then
            builder.Append("")
            Return builder.ToString()
        End If

        For Each kvp As KeyValuePair(Of String, String) In sortedParamMap
            builder.Append(Me.PercentEncodeRfc3986(kvp.Key))
            builder.Append("=")
            builder.Append(Me.PercentEncodeRfc3986(kvp.Value))
            builder.Append("&")
        Next
        Dim canonicalString As String = builder.ToString()
        canonicalString = canonicalString.Substring(0, canonicalString.Length - 1)
        Return canonicalString
    End Function
End Class

'
'     * To help the SortedDictionary order the name-value pairs in the correct way.
'     

Class ParamComparer
    Implements IComparer(Of String)
    Public Function Compare(p1 As String, p2 As String) As Integer Implements IComparer(Of String).Compare
        Return String.CompareOrdinal(p1, p2)
    End Function
End Class


