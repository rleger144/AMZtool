Imports System.Data.SQLite
Imports System.Data.SqlClient
Imports System.Globalization
Imports AmazonMWS1
Public Class dbOperation
    Public Shared DBconnStr As String = "Data Source=|DataDirectory|\awsdb.db; Version=3;"
    Public Shared Sub FillGloabalSetting()
        Dim objval As New Oozee
        Try
            Dim dt As DataTable
            Dim st As Integer = 0
            dt = dbOperation.GetDataTable("select * from db_settings", st)

            Dim dt_MWS As DataTable
            Dim st_MWS As Integer = 0
            dt_MWS = dbOperation.GetDataTable("Select * from db_MarketPlaceDet", st_MWS)

            If st_MWS > 0 Then
                If dt_MWS.Rows.Count > 0 Then
                    For Each dr As DataRow In dt_MWS.Select("", "MarketId")
                        Select Case dr("MarketId")
                            Case 1  ' MWS
                                Select Case dr("KeyId")
                                    Case 1  ' [MWS Seller Id]
                                        GblMWSSellerID = dr("KeyValue")
                                    Case 2  ' [MWS AccessKey ID]
                                        GblMWSAccessKey = dr("KeyValue")
                                    Case 3  ' [MWS Secret Key]
                                        GblMWSSecretKey = dr("KeyValue")
                                    Case 4  ' [MWS MarketPlace ID]
                                        GblMWSMarketplaceID = dr("KeyValue")
                                End Select
                            Case 2  ' eBAY
                                Select Case dr("KeyId")
                                    Case 1  ' [eBay AppID (ClientId)]
                                        GblebAppID = dr("KeyValue")
                                    Case 2  ' [eBay Dev ID]
                                        GblebDevID = dr("KeyValue")
                                    Case 3  ' [eBay Cert ID]
                                        GblebCerID = dr("KeyValue")
                                    Case 4
                                        If dr("KeyValue") = "0" Then
                                            GblEnabledEbay = False
                                        Else
                                            GblEnabledEbay = True
                                        End If
                                    Case 5
                                        If dr("KeyValue") = "1" Then
                                            GblEnabledEbayBrowserACTIVE = True
                                        Else
                                            GblEnabledEbayBrowserACTIVE = False
                                        End If
                                    Case 6
                                        If dr("KeyValue") = "1" Then
                                            GblEnabledEbayBrowserCOMPLETE = True
                                        Else
                                            GblEnabledEbayBrowserCOMPLETE = False
                                        End If
                                End Select
                            Case 3  ' Keepa
                                Select Case dr("KeyId")
                                    Case 1  ' [eBay AppID (ClientId)]
                                        GblKeepaAccessKey = dr("KeyValue")
                                End Select
                        End Select
                        
                    Next
                End If
            End If

            If st > 0 Then
                If dt.Rows.Count > 0 Then
                    GblAwsAccessKey = objval.ToString(dt.Rows(0)("access_key"))
                    GblAwsSecretKey = objval.ToString(dt.Rows(0)("secret_key"))
                    GblRefMinu = objval.ToInt32(dt.Rows(0)("refresh_minute"))
                    GblApiCalAWSInterval = objval.ToInt32(dt.Rows(0)("api_call_interval"))
                    GblApiCalMWSInterval = objval.ToInt32(dt.Rows(0)("api_call_intervalMWS"))
                    GblAwsSellerID = objval.ToString(dt.Rows(0)("aws_sellerid"))
                    'GblebAppID = objval.ToString(dt.Rows(0)("Eb_App_ID"))
                    'GblebDevID = objval.ToString(dt.Rows(0)("Eb_Dev_ID"))
                    'GblebCerID = objval.ToString(dt.Rows(0)("Eb_Cer_ID"))
                    'GblKeepaAccessKey = objval.ToString(dt.Rows(0)("keepa_access_key"))
                    GblAlertMailIDs = objval.ToString(dt.Rows(0)("email_ids"))
                    _GblSenderMailID = objval.ToString(dt.Rows(0)("sender_emailid"))
                    _GblSenderMailPwd = objval.ToString(dt.Rows(0)("sender_emailpwd"))
                    _GblSenderPortNo = objval.ToString(dt.Rows(0)("sender_port"))
                    _GblSenderSMTP = objval.ToString(dt.Rows(0)("sender_smtp"))

                    If objval.ToString(dt.Rows(0)("send_mail_on_eachscan")) = "1" Then
                        _GblSendMailonEachscan = True
                    Else
                        _GblSendMailonEachscan = False
                    End If

                    'GblMWSAccessKey = objval.ToString(dt.Rows(0)("mws_accesskey"))
                    'GblMWSMarketplaceID = objval.ToString(dt.Rows(0)("mws_marketplaceid"))
                    'GblMWSSecretKey = objval.ToString(dt.Rows(0)("mws_secretkey"))
                    'GblMWSSellerID = objval.ToString(dt.Rows(0)("mws_sellerid"))

                    If objval.ToString(dt.Rows(0)("enable_schedule")) = "1" Then
                        GblIsScheularEnabled = True
                        GblScheularTime = objval.ToString(dt.Rows(0)("schedule_time"))
                    Else
                        GblIsScheularEnabled = False
                        GblScheularTime = objval.ToString(dt.Rows(0)("schedule_time"))
                    End If
                    'If objval.ToInt32(dt.Rows(0)("enable_ebay")) = 0 Then
                    '    GblEnabledEbay = False
                    'Else
                    '    GblEnabledEbay = True
                    'End If

                    'If objval.ToInt32(dt.Rows(0)("enable_ebay_act_browser")) = 1 Then
                    '    GblEnabledEbayBrowserACTIVE = True
                    'Else
                    '    GblEnabledEbayBrowserACTIVE = False
                    'End If

                    'If objval.ToInt32(dt.Rows(0)("enable_ebay_comp_browser")) = 1 Then
                    '    GblEnabledEbayBrowserCOMPLETE = True
                    'Else
                    '    GblEnabledEbayBrowserCOMPLETE = False
                    'End If


                    GblMWS.SetMWS_AccessKey(GblMWSAccessKey)
                    GblMWS.SetMWS_MerchantId(GblMWSSellerID)
                    GblMWS.SetMWS_SecretKey(GblMWSSecretKey)
                    GblMWS.SetMWS_MarketplaceId(GblMWSMarketplaceID)

                    If objval.ToInt32(dt.Rows(0)("is_alert_mail")) = 1 Then
                        GblIsAlertMail = True
                    Else
                        GblIsAlertMail = False
                    End If

                    If objval.ToInt32(dt.Rows(0)("is_show_notification")) = 1 Then
                        GblShowNotification = True

                    Else
                        GblShowNotification = False
                    End If
                    GblShowNotification_type = objval.ToInt32(dt.Rows(0)("is_show_notification_type"))

                    If objval.ToInt32(dt.Rows(0)("sender_chkssl")) = 1 Then
                        _GblSenderIsSSL = True
                    Else
                        _GblSenderIsSSL = False
                    End If
                End If
            End If

        Catch ex As Exception
            objval.MsgError("Erro while loading data from Database.", "Exception")
        End Try


    End Sub

    Public Shared Function GetDataTable(ByVal Qry As [String], ByRef _status As Integer) As DataTable
        Dim dt As DataTable = Nothing
        Try
            _status = 1
            Using con As New SQLiteConnection(DBconnStr)
                con.Open()
                Dim cmd As New SQLiteCommand(Qry, con)
                cmd.CommandText = Qry

                Dim da As New SQLiteDataAdapter(cmd)
                Dim ds As New DataSet()
                da.Fill(ds, "data")
                con.Close()
                dt = ds.Tables("data")
                Return dt
            End Using
        Catch
            _status = -1
            Return dt
        End Try
    End Function

    Public Shared Function GetDataTable(ByVal cmd As SQLiteCommand, ByRef _status As Integer) As DataTable
        Dim dt As DataTable = Nothing
        Try
            _status = 1
            Using con As New SQLiteConnection(DBconnStr)
                con.Open()
                cmd.Connection = con

                Dim da As New SQLiteDataAdapter(cmd)
                Dim ds As New DataSet()
                da.Fill(ds, "data")
                con.Close()
                dt = ds.Tables("data")
                Return dt
            End Using
        Catch
            _status = -1
            Return dt
        End Try
    End Function

    Public Shared Function ChkASIN(ByVal asin As String, ByVal tprice As Double, ByVal usedprice As Double) As Integer
        Try
            Dim dt As DataTable = Nothing
            Dim cmd As New SQLiteCommand
            cmd.CommandText = "select count(*) from db_products where product_asin = @asin"
            cmd.Parameters.AddWithValue("@asin", asin)

            Using con As New SQLiteConnection(DBconnStr)
                con.Open()
                cmd.Connection = con

                Dim da As New SQLiteDataAdapter(cmd)
                Dim ds As New DataSet()
                da.Fill(ds, "data")
                dt = ds.Tables("data")

                cmd = New SQLiteCommand(con)
                If Convert.ToInt32(dt.Rows(0)(0)) > 0 Then
                    'Update
                    cmd.CommandText = "update db_products set target_used_price=@target_used_price,target_price= @target_price,live_price=0,live_newprice=0,live_usedprice=0  where product_asin=@product_asin"
                Else
                    'Insert
                    cmd.CommandText = "insert into db_products(product_asin,target_price,target_used_price) values (@product_asin,@target_price,@target_used_price)"
                End If
                cmd.Parameters.AddWithValue("@product_asin", asin)
                cmd.Parameters.AddWithValue("@target_price", Math.Round(tprice, 2))
                cmd.Parameters.AddWithValue("@target_used_price", Math.Round(usedprice, 2))
                cmd.ExecuteNonQuery()

                con.Close()

            End Using
            Return 1
        Catch
            Return -1
        End Try
    End Function

    Public Shared Function UpdateProcessPrice(ByVal ASIN As String, ByVal title As String, ByVal Price As Double, ByVal Nprice As Double, ByVal Uprice As Double) As Integer
        Try
            Dim i As Integer = 0
            Dim con As New SQLiteConnection(DBconnStr)
            Dim cmd As New SQLiteCommand()
            cmd.CommandText = "update db_products set product_name=@product_name,last_updated=@last_updated,live_price=@live_price,live_newprice=@live_newprice,live_usedprice=@live_usedprice  where product_asin=@product_asin"
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            cmd.Parameters.AddWithValue("@product_name", title)
            cmd.Parameters.AddWithValue("@live_price", Price)
            cmd.Parameters.AddWithValue("@live_newprice", Nprice)
            cmd.Parameters.AddWithValue("@live_usedprice", Uprice)
            cmd.Parameters.AddWithValue("@product_asin", ASIN)
            cmd.Parameters.AddWithValue("@last_updated", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture))

            con.Open()
            i = cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            con.Close()
            Return i

        Catch ex As Exception
            Return -1
        End Try
    End Function

    Public Shared Function ExecuteQuery(cmd As SQLiteCommand) As Int32
        Try
            Dim i As Integer = 0
            Dim con As New SQLiteConnection(DBconnStr)
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            con.Open()
            i = cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            con.Close()
            Return i
        Catch ex As Exception

            Return -1
        End Try
    End Function

    Public Shared Function ExecuteQuery(qry As [String]) As Int32
        Try
            Dim i As Integer = 0
            Dim con As New SQLiteConnection(DBconnStr)
            Dim cmd As New SQLiteCommand(qry)
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            con.Open()
            Using transaction = con.BeginTransaction()
                i = cmd.ExecuteNonQuery()
                transaction.Commit()
            End Using

            cmd.Parameters.Clear()
            con.Close()
            Return i
        Catch
            Return -1
        End Try
    End Function

    Public Shared Function _updCreateTempDatabase(ByVal connection As SqlConnection, ByVal dbname As String, ByRef excption As String) As Boolean
        Dim cmd As SqlCommand = connection.CreateCommand()
        cmd.CommandText = "IF EXISTS(SELECT 1 FROM sys.databases WHERE name ='" & dbname & "') " & _
                            "BEGIN " & _
                            "  ALTER DATABASE [" & dbname & "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; " & _
                            "  DROP DATABASE [" & dbname & "] ; " & _
                            "  CREATE DATABASE [" & dbname & "]; " & _
                            "end " & _
                            "ELSE " & _
                            "BEGIN " & _
                            "  CREATE DATABASE [" & dbname & "]; " & _
                            "END "
        Try
            connection.Open()
            cmd.ExecuteNonQuery()
            excption = ""
            Return True
        Catch ex As SqlException
            excption = ex.ToString()
            Return False
        Catch ex As Exception
            excption = ex.ToString()
            Return False
        Finally
            If connection.State = ConnectionState.Open Then
                connection.Close()
            End If
            cmd.Dispose()
            connection.Dispose()
        End Try
        Return True
    End Function



End Class
