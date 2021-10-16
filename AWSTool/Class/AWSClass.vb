Imports System.Net
Imports System.Text

Public Class AWSToolClass
    Private UpdateVersionURL As String = "http://www.diyashinfotech.com/AwsAuth.ashx?AwsUpdateVersion=Get"
    Private AccountURL As String = "http://www.diyashinfotech.com/AwsAuth.ashx?AuthKey={0}"

    Private Lic As New AWSLicenseKey
    Public Sub WriteToEventLog(ByVal sSource As String, ByVal message As String, ByVal level As EventLogEntryType)
        Dim sLog As String = "AWSTool"
        If Not EventLog.SourceExists(sSource) Then EventLog.CreateEventSource(sSource, sLog)
        EventLog.WriteEntry(sSource, message, level)
    End Sub
    Public Function CheckForInternetConnection() As Boolean
        Try

            Using client = New WebClient()

                Using client.OpenRead("http://www.google.com")
                    Return True
                End Using
            End Using

        Catch ex As Exception
            WriteToEventLog("CheckForInternetConnection", ex.Message, EventLogEntryType.[Error])
            Return False
        End Try
    End Function
    Public Function Signin(ByVal signinId As String, ByVal Password As String, ByVal HardwareId As String) As String
        Dim RetMsg As String = ""
        If CheckForInternetConnection() = False Then
            RetMsg = "Erros : Internet not working"
        Else
            Try
                RetMsg += signinId & "|"
                RetMsg += Password & "|"
                RetMsg += HardwareId
                RetMsg = AWSLicenseKey.LicenseKey.Encrypt(RetMsg, True, "AwsSignin")
                Dim webClient As WebClient = New WebClient()
                Dim uriString As String = String.Format(AccountURL, 2)
                Dim byteArray As Byte() = Encoding.ASCII.GetBytes(RetMsg)
                Dim responseArray As Byte() = webClient.UploadData(New Uri(uriString), "POST", byteArray)
                RetMsg = Encoding.ASCII.GetString(responseArray)
            Catch ex As Exception
                WriteToEventLog("Signin", ex.Message, EventLogEntryType.Warning)
                RetMsg = "Invalid Request"
            End Try
        End If

        Return RetMsg
    End Function

    Public Function WebLogin(ByVal signinId As String, ByVal Password As String) As DataSet

        Dim RetMsg As String = ""
        Dim ds As DataSet = New DataSet()

        If CheckForInternetConnection() = False Then
            RetMsg = "Erros : Internet not working"
        Else
            Try
                Dim vLoginJSon As String = My.Resources.LoginJSON

                vLoginJSon = vLoginJSon.Replace("#CustomerProductVersion#", My.Application.Info.Version.ToString)
                vLoginJSon = vLoginJSon.Replace("#CustomerEmailId#", signinId)
                vLoginJSon = vLoginJSon.Replace("#CustomerPassword#", Password)
                vLoginJSon = vLoginJSon.Replace("#TokenNo#", "")

                Dim vRequestData As String = vLoginJSon
                Dim vResultData As String = String.Empty

                Dim webClient As WebClient = New WebClient()
                Dim uriString As String = String.Format(AccountURL, "Login")
                Dim byteArray As Byte() = Encoding.ASCII.GetBytes(vRequestData)
                Dim responseArray As Byte() = webClient.UploadData(New Uri(uriString), "POST", byteArray)

                vResultData = Encoding.ASCII.GetString(responseArray)

                ds = Newtonsoft.Json.JsonConvert.DeserializeObject(Of DataSet)(vResultData)

            Catch ex As Exception
                WriteToEventLog("Signin", ex.Message, EventLogEntryType.Warning)
                RetMsg = "Invalid Request"
            End Try
        End If

        Return ds
    End Function

    Public Function Check_LicenseValidation(ByVal signinId As String, ByVal vTokenNo As String) As DataSet

        Dim RetMsg As String = ""
        Dim ds As DataSet = New DataSet()

        If CheckForInternetConnection() = False Then
            RetMsg = "Erros : Internet not working"
        Else
            Try
                Dim vLoginJSon As String = My.Resources.LoginJSON

                vLoginJSon = vLoginJSon.Replace("#CustomerProductVersion#", My.Application.Info.Version.ToString)
                vLoginJSon = vLoginJSon.Replace("#CustomerEmailId#", signinId)
                vLoginJSon = vLoginJSon.Replace("#CustomerPassword#", "")
                vLoginJSon = vLoginJSon.Replace("#TokenNo#", vTokenNo)

                Dim vRequestData As String = vLoginJSon
                Dim vResultData As String = String.Empty

                Dim webClient As WebClient = New WebClient()
                Dim uriString As String = String.Format(AccountURL, "Login")
                Dim byteArray As Byte() = Encoding.ASCII.GetBytes(vRequestData)
                Dim responseArray As Byte() = webClient.UploadData(New Uri(uriString), "POST", byteArray)

                vResultData = Encoding.ASCII.GetString(responseArray)

                ds = Newtonsoft.Json.JsonConvert.DeserializeObject(Of DataSet)(vResultData)

            Catch ex As Exception
                WriteToEventLog("Signin", ex.Message, EventLogEntryType.Warning)
                RetMsg = "Invalid Request"
            End Try
        End If

        Return ds
    End Function


    Public Function UserDatabaseUpdate() As String
        Dim RetMsg As String = ""

        'If CheckForInternetConnection() = False Then
        '    RetMsg = "Erros : Internet not working"
        'Else

        '    Try
        '        Dim dt As DataTable = New DataTable()
        '        dt = GetDatatable("Select D02_01_UserID,D02_01_Password From D02_01_FilterProfile where D02_01_FeedType <> 4 Group by D02_01_UserID,D02_01_Password")

        '        If dt.Rows.Count > 0 Then

        '            For Each dr As DataRow In dt.Rows
        '                dr("D02_01_UserID") = UserAndPasswordEncryptDecrypt(dr("D02_01_UserID").ToString(), "D")
        '                dr("D02_01_Password") = UserAndPasswordEncryptDecrypt(dr("D02_01_Password").ToString(), "D")
        '            Next

        '            dt.AcceptChanges()
        '            RetMsg = JsonConvert.SerializeObject(dt)
        '            RetMsg = DiaAnalysis.LicenseKey.Encrypt(RetMsg, True, "UserDatabaseUpdate")
        '            Dim webClient As WebClient = New WebClient()
        '            Dim uriString As String = String.Format(AccountURL, 99)
        '            Dim byteArray As Byte() = Encoding.ASCII.GetBytes(RetMsg)
        '            Dim responseArray As Byte() = webClient.UploadData(New Uri(uriString), "POST", byteArray)
        '            RetMsg = Encoding.ASCII.GetString(responseArray)
        '        End If

        '    Catch ex As Exception
        '        Common.WriteToEventLog("Signin", ex.Message, EventLogEntryType.Warning)
        '        RetMsg = "Invalid Request"
        '    End Try
        'End If

        Return RetMsg
    End Function

End Class
