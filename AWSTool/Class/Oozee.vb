Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Data
Imports System.Security.Cryptography
Imports System.Threading
Imports System.Windows.Forms
Imports DevExpress.XtraEditors
Imports System.IO
Imports System.Drawing
Imports System.Net
Imports Microsoft.Win32


Public Class Oozee



#Region "Enumaration"

    Public Enum DateFormat
        MMDDYYYY = 0
        MMDDYY = 1
        DDMMYYYY = 2
        DDMMYY = 3
        YYYYDDMM = 4
        YYDDMM = 5
        YYYYMMDD = 6
        YYMMDD = 7
        DDMMMYYYY = 8
    End Enum

    Public Enum TimeFormat
        HHMMTT = 0
        HHMMSSTT = 1
        HHMMSS = 2
    End Enum

    Public Enum DateDiffEnum
        DAYS = 0
        HOURS = 3
        MINUTES = 4
        SECONDS = 5
        YEARS = 6
        MONTH = 7
    End Enum

#End Region

    Public Sub writeRegisteryCurrentUser(ByVal strKey As String, ByVal strSubKey As String, ByVal strValue As String)
        Dim regKey As RegistryKey = Nothing
        Try
            regKey = Registry.CurrentUser.OpenSubKey(strKey, True)
            ' strValue = regKey.GetValue(strSubKey)
            regKey.SetValue(strSubKey, strValue)
        Catch ex As Exception
        Finally
            regKey.Close()
        End Try
    End Sub

    Public Function IsBlank(ByVal obj As Object) As Boolean
        If IsDBNull(obj) OrElse obj Is Nothing OrElse obj.ToString = "" Then
            Return True
        End If
        Return False
    End Function

#Region "Conversion Utility"

    Public Function CheckInterNetConnection() As Boolean
        Try
            Dim request As HttpWebRequest = DirectCast(WebRequest.Create("http://www.google.co.in"), HttpWebRequest)
            request.Timeout = 4000
            request.Credentials = CredentialCache.DefaultNetworkCredentials
            Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)

            If response.StatusCode = HttpStatusCode.OK Then
                Return True
            Else
                Return False
            End If
        Catch
            Return False
        End Try
    End Function

    Public Function PingIP() As Boolean
        Try
            Dim request As HttpWebRequest = DirectCast(WebRequest.Create("http://www.google.co.in"), HttpWebRequest)
            request.Timeout = 4000
            request.Credentials = CredentialCache.DefaultNetworkCredentials
            Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)

            If response.StatusCode = HttpStatusCode.OK Then
                Return True
            Else
                Return False
            End If
        Catch
            Return False
        End Try
    End Function

    Public Function ToImageToByteArray(ByVal imageIn As System.Drawing.Image) As Byte()
        Dim ms = New MemoryStream()
        imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
        Return ms.ToArray()
    End Function

    Public Function ToBytesToImage(ByVal byteArrayIn As Byte()) As Image
        Dim ms = New MemoryStream(byteArrayIn)
        Dim returnImage As Image
        returnImage = Image.FromStream(ms)
        Return returnImage
    End Function

    Public Function ToDouble(ByVal pObj As Object) As Double
        Dim Answer As Double = 0
        If pObj Is Nothing Then
            Return Answer
        End If
        If pObj.ToString().ToUpper() = "NAN" Then
            Return 0.0
        End If
        If Double.TryParse(pObj.ToString(), Answer) Then
            Return Answer
        Else
            Return Answer
        End If
    End Function

    Public Function ToLong(ByVal pObj As Object) As Long
        Dim Answer As Long = 0
        If pObj Is Nothing Then
            Return Answer
        End If
        If pObj.ToString().ToUpper() = "NAN" Then
            Return 0
        End If
        If Long.TryParse(pObj.ToString(), Answer) Then
            Return Answer
        Else
            Return Answer
        End If
    End Function

    Public Function Val(ByVal pObj As Object) As Double
        Return ToDouble(pObj)
    End Function

    Public Function ToInt(ByVal pObj As Object) As Integer
        Dim Answer As Integer = 0
        If pObj Is Nothing Then
            Return Answer
        End If
        If Integer.TryParse(pObj.ToString(), Answer) Then
            Return Answer
        Else
            Return Answer
        End If
    End Function

    Public Function ToInt16(ByVal pObj As Object) As Int16
        Dim Answer As Int16 = 0
        If pObj Is Nothing Then
            Return Answer
        End If
        If Int16.TryParse(pObj.ToString(), Answer) Then
            Return Answer
        Else
            Return Answer
        End If
    End Function

    Public Function ToInt32(ByVal pObj As Object) As Int32
        Dim Answer As Int32 = 0
        If pObj Is Nothing Then
            Return Answer
        End If
        If Int32.TryParse(pObj.ToString(), Answer) Then
            Return Answer
        Else
            Return Answer
        End If
    End Function

    Public Function ToInt64(ByVal pObj As Object) As Int64
        Dim Answer As Int64 = 0
        If pObj Is Nothing Then
            Return Answer
        End If
        If Int64.TryParse(pObj.ToString(), Answer) Then
            Return Answer
        Else
            Return Answer
        End If
    End Function

    Public Function ToDecimal(ByVal pObj As Object) As Decimal
        Dim Answer As Decimal = 0
        If pObj Is Nothing Then
            Return Answer
        End If
        If Decimal.TryParse(pObj.ToString(), Answer) Then
            Return Answer
        Else
            Return Answer
        End If
    End Function

    Public Overloads Function ToString(ByVal pObj As Object) As String
        'string Answer = "";
        'if (pObj == null || pObj.ToString().ToUpper() == "NAN") 
        If pObj Is Nothing Then
            Return ""
        End If
        Return pObj.ToString().Trim()
    End Function

    Public Function ToDate(ByVal pObj As Object, ByVal pDateFormat As DateFormat) As String

        If pObj Is Nothing OrElse pObj.ToString() = "" Then
            Return ""
        End If

        Dim DT As DateTime

        If DateTime.TryParse(pObj.ToString(), DT) Then
            If pDateFormat = DateFormat.DDMMYY Then
                Return DateTime.Parse(pObj.ToString()).ToString("dd/MM/yy")
            ElseIf pDateFormat = DateFormat.DDMMYYYY Then
                Return DateTime.Parse(pObj.ToString()).ToString("dd/MM/yyyy")
            ElseIf pDateFormat = DateFormat.MMDDYY Then
                Return DateTime.Parse(pObj.ToString()).ToString("MM/dd/yy")
            ElseIf pDateFormat = DateFormat.MMDDYYYY Then
                Return DateTime.Parse(pObj.ToString()).ToString("MM/dd/yyyy")
            ElseIf pDateFormat = DateFormat.YYDDMM Then
                Return DateTime.Parse(pObj.ToString()).ToString("yy/dd/MM")
            ElseIf pDateFormat = DateFormat.YYMMDD Then
                Return DateTime.Parse(pObj.ToString()).ToString("yy/MM/dd")
            ElseIf pDateFormat = DateFormat.YYYYDDMM Then
                Return DateTime.Parse(pObj.ToString()).ToString("yyyy/dd/MM")
            ElseIf pDateFormat = DateFormat.YYYYMMDD Then
                Return DateTime.Parse(pObj.ToString()).ToString("yyyy/MM/dd")
            ElseIf pDateFormat = DateFormat.DDMMMYYYY Then
                Return DateTime.Parse(pObj.ToString()).ToString("dd MMM yyyy")
            Else
                Return ""
            End If
        Else
            Return ""
        End If
    End Function

    Public Function StringToDateTime(ByVal pObj As Object, ByVal pDateFormat As DateFormat) As DateTime
        Dim StrDate As String = ToDate(pObj, pDateFormat)
        Return DateTime.Parse(StrDate)
    End Function

    Public Function DBDate(ByVal pStrDate As String) As String
        Return ToDate(pStrDate, DateFormat.MMDDYY)
    End Function

    Public Function DBTime(ByVal pStrDate As String) As String
        Return ToTime(pStrDate, TimeFormat.HHMMSSTT)
    End Function

    Public Function ToTime(ByVal pObj As Object, ByVal pTimeFormat As TimeFormat) As String

        If pObj Is Nothing OrElse pObj.ToString() = "" Then
            Return ""
        End If

        Dim DT As DateTime

        If DateTime.TryParse(pObj.ToString(), DT) Then
            If pTimeFormat = TimeFormat.HHMMSS Then
                Return DateTime.Parse(pObj.ToString()).ToString("hh:mm:ss")

            ElseIf pTimeFormat = TimeFormat.HHMMSSTT Then
                Return DateTime.Parse(pObj.ToString()).ToString("hh:mm:ss tt")
            ElseIf pTimeFormat = TimeFormat.HHMMTT Then
                Return DateTime.Parse(pObj.ToString()).ToString("hh:mm tt")
            Else
                Return ""
            End If
        Else
            Return ""
        End If
    End Function

    Public Function ToBoolean(ByVal pObj As Object) As [Boolean]
        If pObj Is Nothing OrElse pObj.ToString() = "" Then
            Return False
        End If
        Dim Answer As Boolean = False
        Boolean.TryParse(pObj.ToString(), Answer)
        Return Answer
    End Function

    Public Function ToBooleanToInt(ByVal pObj As Object) As Integer
        If pObj Is Nothing OrElse pObj.ToString() = "" Then
            Return 0
        End If
        Dim Answer As Boolean = False
        Boolean.TryParse(pObj.ToString(), Answer)
        If Answer = False Then
            Return 0
        Else
            Return 1
        End If
    End Function

#End Region

#Region "Check Utility"

    Public Function ISDate(ByVal pObj As Object) As Boolean
        If pObj Is Nothing OrElse pObj.ToString() = "" Then
            Return False
        End If
        Dim Answer As DateTime
        If DateTime.TryParse(pObj.ToString(), Answer) Then
            Return True
        End If
        Return False
    End Function

    Public Function ISTime(ByVal pObj As Object) As Boolean
        If pObj Is Nothing OrElse pObj.ToString() = "" Then
            Return False
        End If
        Dim Answer As DateTime
        If DateTime.TryParse(pObj.ToString(), Answer) Then
            Return True
        End If
        Return False
    End Function

    Public Function ISNumeric(ByVal pObj As Object) As Boolean
        If pObj Is Nothing OrElse pObj.ToString() = "" Then
            Return False
        End If
        Dim Answer As Double
        If Double.TryParse(pObj.ToString(), Answer) Then
            Return True
        End If
        Return False
    End Function

    Public Shared Function IsAlphaNumeric(ByVal pStr As String) As Boolean
        Dim BlnValid As Boolean = True

        Dim StrAN As [String] = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"
        For i As Integer = 0 To pStr.Length - 1
            If StrAN.IndexOf(pStr(i)) < 0 Then
                BlnValid = False
                Exit For
            End If
        Next
        Return BlnValid
    End Function

#End Region

#Region "Encryption"

    Public Function Encrypt(ByVal ToEncrypt As String, ByVal Key As String) As String
        Dim keyArray As Byte()
        Dim toEncryptArray As Byte() = UTF8Encoding.UTF8.GetBytes(ToEncrypt)
        'System.Configuration.AppSettingsReader settingsReader = new     AppSettingsReader();

        Dim hashmd5 As New MD5CryptoServiceProvider()
        keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Key))
        hashmd5.Clear()

        Dim tDes As New TripleDESCryptoServiceProvider()
        tDes.Key = keyArray
        tDes.Mode = CipherMode.ECB
        tDes.Padding = PaddingMode.PKCS7
        Dim cTransform As ICryptoTransform = tDes.CreateEncryptor()
        Dim resultArray As Byte() = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length)
        tDes.Clear()
        Return Convert.ToBase64String(resultArray, 0, resultArray.Length)
    End Function

    Public Function Decrypt(ByVal cypherString As String, ByVal Key As String) As String
        Dim keyArray As Byte()
        Dim toDecryptArray As Byte() = Convert.FromBase64String(cypherString)
        'byte[] toEncryptArray = Convert.FromBase64String(cypherString);
        'System.Configuration.AppSettingsReader settingReader = new     AppSettingsReader();

        Dim hashmd As New MD5CryptoServiceProvider()
        keyArray = hashmd.ComputeHash(UTF8Encoding.UTF8.GetBytes(Key))
        hashmd.Clear()

        Dim tDes As New TripleDESCryptoServiceProvider()
        tDes.Key = keyArray
        tDes.Mode = CipherMode.ECB
        tDes.Padding = PaddingMode.PKCS7
        Dim cTransform As ICryptoTransform = tDes.CreateDecryptor()
        Try
            Dim resultArray As Byte() = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length)

            tDes.Clear()
            Return UTF8Encoding.UTF8.GetString(resultArray, 0, resultArray.Length)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#End Region

#Region "Datatable Search"

    Public Shared Function ZipString(textdata As String) As String
        Try
            Dim buffer As Byte() = System.Text.Encoding.Unicode.GetBytes(textdata)
            Dim ms As New MemoryStream()
            Using zipStream As New System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress, True)
                zipStream.Write(buffer, 0, buffer.Length)
            End Using

            ms.Position = 0
            Dim outStream As New MemoryStream()

            Dim compressed As Byte() = New Byte(ms.Length - 1) {}
            ms.Read(compressed, 0, compressed.Length)

            Dim gzBuffer As Byte() = New Byte(compressed.Length + 3) {}
            System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length)
            System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4)
            Return Convert.ToBase64String(gzBuffer)
        Catch ex As Exception
            Return "EX"
        End Try
    End Function

    Public Shared Function UnZipString(compressedText As String) As String
        Try
            Dim gzBuffer As Byte() = Convert.FromBase64String(compressedText)
            Using ms As New MemoryStream()
                Dim msgLength As Integer = BitConverter.ToInt32(gzBuffer, 0)
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4)

                Dim buffer As Byte() = New Byte(msgLength - 1) {}

                ms.Position = 0
                Using zipStream As New System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Decompress)
                    zipStream.Read(buffer, 0, buffer.Length)
                End Using

                Return System.Text.Encoding.Unicode.GetString(buffer, 0, buffer.Length)
            End Using
        Catch ex As Exception
            Return "EX"
        End Try
    End Function

    Public Function SearchText(ByVal DTab As DataTable, ByVal pStrSearchKeyField As String, ByVal pStrSearchValueField As String, ByVal pStrReturnField As String) As String
        If DTab Is Nothing Then
            Return ""
        End If
        Dim StrQuery As String = ""
        Dim StrResult As String = ""
        Dim Key As String() = pStrSearchKeyField.Split(","c)
        Dim Value As String() = pStrSearchValueField.Split(","c)

        For IntI As Integer = 0 To Key.Length - 1
            If IntI = 0 Then
                StrQuery = Key(IntI).ToString() & " = '" & Value(IntI).ToString() & "'"
            Else
                StrQuery += " AND " & Key(IntI).ToString() & " = '" & Value(IntI).ToString() & "'"
            End If
        Next

        Dim result As DataRow() = DTab.[Select](StrQuery)

        If result IsNot Nothing AndAlso result.Length <> 0 Then
            StrResult = result(0)(pStrReturnField).ToString()
        End If

        Key = Nothing
        Value = Nothing
        result = Nothing

        Return StrResult
    End Function

    Public Sub MsgError(ByVal ErrMsg As String, ByVal title As String)
        XtraMessageBox.Show(ErrMsg, title, MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Public Sub MsgSuccess(ByVal ErrMsg As String, ByVal title As String)
        XtraMessageBox.Show(ErrMsg, title, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Public Sub MsgStop(ByVal ErrMsg As String, ByVal title As String)
        XtraMessageBox.Show(ErrMsg, title, MessageBoxButtons.OK, MessageBoxIcon.Stop)
    End Sub

    Public Sub MsgWarning(ByVal ErrMsg As String, ByVal title As String)
        XtraMessageBox.Show(ErrMsg, title, MessageBoxButtons.OK, MessageBoxIcon.Warning)
    End Sub

    Public Sub MsgHand(ByVal ErrMsg As String, ByVal title As String)
        XtraMessageBox.Show(ErrMsg, title, MessageBoxButtons.OK, MessageBoxIcon.Hand)
    End Sub

    Public Sub MsgExclaim(ByVal ErrMsg As String, ByVal title As String)
        XtraMessageBox.Show(ErrMsg, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    End Sub

    Public Function MsgConfirmYesNo(ByVal ErrMsg As String, ByVal title As String) As DialogResult
        Dim Result As New DialogResult
        Result = XtraMessageBox.Show(ErrMsg, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        Return Result
    End Function

    Public Function MsgConfirmOkCancel(ByVal ErrMsg As String, ByVal title As String) As DialogResult
        Dim Result As New DialogResult
        Result = XtraMessageBox.Show(ErrMsg, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
        Return Result
    End Function

    Public Function MsgConfirmYesNoCancel(ByVal ErrMsg As String, ByVal title As String) As DialogResult
        Dim Result As New DialogResult
        Result = XtraMessageBox.Show(ErrMsg, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)
        Return Result
    End Function

#End Region


#Region "Format"

    Public Function Format(ByVal Expression As Object, ByVal Style As String) As String
        If Style.IndexOf("#"c) <> -1 Then
            If Style.IndexOf("."c) <> -1 Then
                Dim douval As Double = Me.ToDouble(Expression)
                Return Microsoft.VisualBasic.Strings.Format(douval, Style)
            Else
                Dim int64val As Int64 = ToInt64(Me.ToString(Expression))
                Return Microsoft.VisualBasic.Strings.Format(int64val, Style)
            End If
        Else
            Return Microsoft.VisualBasic.Strings.Format(Expression, Style)
        End If
    End Function

    Public Function Trim(ByVal Expression As Object) As String
        Return Microsoft.VisualBasic.Strings.Trim(ToString(Expression))
    End Function

#End Region

#Region "Form Events"

    Public Function CheckForm(ByVal objfrm As Form) As Boolean
        Dim IsOpen As Boolean = False
        For Each f As Form In Application.OpenForms
            If f.Name = objfrm.Name Then
                IsOpen = True
                f.Focus()
                Exit For
            End If
        Next
        Return IsOpen
    End Function

    Public Sub FormKeyDownEvent(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.[Return] Then
            If DirectCast(sender, Form).ActiveControl.[GetType]().FullName.ToString().IndexOf("UltraGrid") <> -1 Or DirectCast(sender, Form).ActiveControl.Parent.[GetType]().FullName.IndexOf("UltraGrid") <> -1 Then
            Else
                SendKeys.Send("{TAB}")
            End If
        End If
    End Sub

    Public Sub frmResize(ByVal pFrm As Form)
        pFrm.WindowState = FormWindowState.Normal
    End Sub

#End Region

    Public Function InputBox(ByVal pStrPrompt As String) As String
        Return Microsoft.VisualBasic.Interaction.InputBox(pStrPrompt, "", "", 300, 300)
    End Function

    Public Function Left(ByVal pStr As [String], ByVal pLength As Integer) As String
        If pStr Is Nothing OrElse pStr.Length = 0 Then
            Return ""
        End If
        Return Microsoft.VisualBasic.Strings.Left(pStr, pLength)
    End Function

    Public Function Right(ByVal pStr As [String], ByVal pLength As Integer) As String
        If pStr Is Nothing OrElse pStr.Length = 0 Then
            Return ""
        End If
        Return Microsoft.VisualBasic.Strings.Right(pStr, pLength)
    End Function

    Public Function DateDiff(ByVal pDateInterval As Microsoft.VisualBasic.DateInterval, ByVal pStrStartDateTime As String, ByVal pStrEndDateTime As String) As Double
        Dim Result As Long = 0
        Try
            If pStrStartDateTime = "" OrElse pStrEndDateTime = "" Then
                Return 0
            End If
            Dim StartDate As DateTime = DateTime.Parse(pStrStartDateTime)
            Dim EndDate As DateTime = DateTime.Parse(pStrEndDateTime)
            Result = Microsoft.VisualBasic.DateAndTime.DateDiff(pDateInterval, StartDate, EndDate)
        Catch

            Result = -1
        End Try
        Return Val(Result)
    End Function

End Class
