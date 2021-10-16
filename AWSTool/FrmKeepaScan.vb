Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Web
Imports System.Data.SQLite
Imports System.Data.OleDb
Imports System.Security.Cryptography.X509Certificates

Public Class FrmKeepaScan

    Dim gblDTNew As DataTable
    'Dim gblDTUsed As DataTable
    '    Dim gblDTSaleRank As DataTable
    Dim objval As New Oozee
    Public DBconnStrKeepa As String = "Data Source=|DataDirectory|\keepadb.db; Version=3;"
    Dim gblProductDT As DataTable
    Dim _totalProductCnt As Int64 = 0
    Dim _totalProcessDone As Int64 = 0
    Dim _gblisKeepaStart As Boolean = False
    Dim gblErrorLog As New StringBuilder
    Dim _Keepatasks As New List(Of Task(Of Long))()
    Dim _gbltokenLeft As Int64 = -1
    Dim _gblLastProductID As Int64 = 0
    Dim _gblLastProductIDtoUpdate As Int64 = 0

    '`product_asin`	INTEGER,
    '`day_30_new`	TEXT,
    '`day_90_new`	TEXT,
    '`day_180_new`	TEXT,
    '`day_365_new`	TEXT,
    '`year_3_new`	TEXT,
    '`year_5_new`	TEXT,
    '`day_30_used`	TEXT,
    '`day_90_used`	TEXT,
    '`day_180_used`	TEXT,
    '`day_365_used`	TEXT,
    '`year_3_used`	TEXT,
    '`year_5_used`	TEXT,
    '`day_30_salerank`	TEXT,
    '`day_90_salerank`	TEXT,
    '`day_180_salerank`	TEXT,
    '`day_365_salerank`	TEXT,
    '`year_3_salerank`	TEXT,
    '`year_5_salerank`	TEXT

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        If GblKeepaAccessKey = "" Then
            objval.MsgWarning("Please set Keepa Accesskey into setting for scan.", "Warning")
            Exit Sub
        End If
        'ScanKeepa(329553, "B00000410S")


        If _totalProductCnt > 0 Then
            _totalProcessDone = 0
            _gblisKeepaStart = True
            btnStop.Enabled = True
            btnStart.Enabled = False
            pnlUpdating.Visible = True

            If chkWithPrevious.Checked = False Then

                Dim retval = 1
                Dim dt As DataTable = dbOperation_keepa.GetDataTable("select last_value from setting where id=1", retval)
                Dim StartFrom As Int64 = 0
                If dt.Rows.Count > 0 Then
                    StartFrom = dt.Rows(0)("last_value")
                End If

                Dim dv As New DataView(gblProductDT)
                dv.RowFilter = "product_id>" & StartFrom
                gblProductDT = dv.ToTable

                _totalProductCnt = gblProductDT.Rows.Count
                lblTotalProduct.Text = _totalProductCnt
                If _totalProductCnt > 0 Then
                    _gblLastProductID = objval.ToInt64(gblProductDT.Rows(_totalProductCnt - 1)("product_id"))
                Else
                    _gblLastProductID = 0
                End If
            End If

            chkWithPrevious.Enabled = False

            HDbackWork.RunWorkerAsync()
        End If
    End Sub

    Private Sub FrmKeepaScan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GblIsKeepaOpen = True
        lblApproxTime.Text = ""
        lblScanComplete.Text = "0"
        lblPercentage.Text = ""

        gblDTNew = New DataTable
        gblDTNew.Columns.Add("idate", GetType(DateTime))
        gblDTNew.Columns.Add("iprice", GetType(Double))

        'gblDTUsed = gblDTNew.Clone
        'gblDTSaleRank = gblDTNew.Clone

        Dim retval = 1
        gblProductDT = dbOperation.GetDataTable("select product_id,product_asin from db_products order by product_id", retval)
        If retval = 0 Then
            objval.MsgWarning("Fail to get product record.", "")
            btnStart.Enabled = False
            btnStop.Enabled = False
            lblTotalProduct.Text = "0"
            _totalProductCnt = 0
            _gblLastProductID = 0
        Else
            _totalProductCnt = gblProductDT.Rows.Count
            lblTotalProduct.Text = _totalProductCnt
            Dim t As New TimeSpan(0, _totalProductCnt / 20, 0)
            Dim timeElapsed As String = t.Hours & ":" & t.Minutes
            lblApproxTime.Text = "( approx " & timeElapsed & " minute scan )"
            If _totalProductCnt > 0 Then
                _gblLastProductID = objval.ToInt64(gblProductDT.Rows(_totalProductCnt - 1)("product_id"))
            Else
                _gblLastProductID = 0
            End If

        End If
    End Sub

    Private Sub HDbackWork_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles HDbackWork.DoWork

        gblErrorLog.AppendLine("Keepa Scan Started at :" & DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"))
        gblErrorLog.AppendLine(">> LAst ID" & _gblLastProductID)
        gblErrorLog.AppendLine(">> start ID" & gblProductDT.Rows(0)("product_id"))
        For Each dr As DataRow In gblProductDT.Rows

            If _gblisKeepaStart = False Then
                Exit For
            End If

            Dim _ProductASIN As String = Convert.ToString(dr("product_asin"))
            Dim _ProductID As Int64 = Convert.ToInt64(dr("product_id"))
            _gblLastProductIDtoUpdate = _ProductID
            gblErrorLog.AppendLine(">> " & _ProductID)
            HDbackWork.ReportProgress(1)

            _Keepatasks.Add(Task.Factory.StartNew(Function() Me.ScanKeepa(_ProductID, _ProductASIN)))
skipItem:
            If _gbltokenLeft = -1 Then
                System.Threading.Thread.Sleep(5000)
            End If
            System.Threading.Thread.Sleep(5000)
        Next

ChkStillTaskPending:

        Dim taskCnt As Int64 = _Keepatasks.Where(Function(x) x.Status = TaskStatus.Running).ToList().Count()



        If taskCnt > 0 Then
            Threading.Thread.Sleep(3000)
            GoTo ChkStillTaskPending
        End If

        _Keepatasks.Clear()

        gblErrorLog.AppendLine("Keepa Scan Ended at :" & DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"))
        HDbackWork.ReportProgress(1)
    End Sub

    Public Function ScanKeepa(ByVal productid As Int64, ByVal _ProductASIN As String) As Int64
        Try
            _totalProcessDone = _totalProcessDone + 1
            HDbackWork.ReportProgress(1)

            Dim gblDTNewsub As DataTable = gblDTNew.Clone
            Dim gblDTUsed As DataTable = gblDTNew.Clone
            Dim gblDTSaleRank As DataTable = gblDTNew.Clone


            Dim _Used30Avg As Double = 0.0
            Dim _Used90Avg As Double = 0.0
            Dim _Used180Avg As Double = 0.0
            Dim _Used365Avg As Double = 0.0
            Dim _Used3YrAvg As Double = 0.0
            Dim _Used5YrAvg As Double = 0.0

            Dim _New30Avg As Double = 0.0
            Dim _New90Avg As Double = 0.0
            Dim _New180Avg As Double = 0.0
            Dim _New365Avg As Double = 0.0
            Dim _New3YrAvg As Double = 0.0
            Dim _New5YrAvg As Double = 0.0

            Dim _SaleR30Avg As Double = 0.0
            Dim _SaleR90Avg As Double = 0.0
            Dim _SaleR180Avg As Double = 0.0
            Dim _SaleR365Avg As Double = 0.0
            Dim _SaleR3YrAvg As Double = 0.0

            Dim URL As String = "https://api.keepa.com/product?key=" & GblKeepaAccessKey & "&domain=1&asin=" & _ProductASIN

            Dim streamText As String = ""

            ServicePointManager.Expect100Continue = True
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 Or SecurityProtocolType.Tls
            ServicePointManager.SecurityProtocol = DirectCast(3072, SecurityProtocolType)
            'ServicePointManager.ServerCertificateValidationCallback = Function() True
            Dim request As HttpWebRequest = CType(WebRequest.Create(URL), HttpWebRequest)
            request.AutomaticDecompression = DecompressionMethods.Deflate Or DecompressionMethods.GZip
            request.Method = "GET"
            'request.ServicePoint.Expect100Continue = True
            'request.AllowWriteStreamBuffering = True
            'request.Credentials = CredentialCache.DefaultCredentials
            'request.ProtocolVersion = HttpVersion.Version10



            ' Dim cert As New X509Certificate2("Cert.der", "Password")
            ' request.ClientCertificates.Add(cert)
            'request.PreAuthenticate = True

            Dim res As HttpWebResponse
            Try
                res = CType(request.GetResponse(), HttpWebResponse)
            Catch ex As Exception
                gblErrorLog.AppendLine("Faild to get Response for : " & _ProductASIN)
                GoTo skippnow
            End Try


            If res.StatusCode = HttpStatusCode.OK Then
                Dim receiveStream As Stream = res.GetResponseStream()
                Dim readStream As StreamReader = Nothing
                readStream = New StreamReader(receiveStream)
                streamText = readStream.ReadToEnd()
                res.Close()
                readStream.Close()
            End If
            Dim jObject As RootObject = CType(Newtonsoft.Json.JsonConvert.DeserializeObject(streamText, GetType(RootObject)), RootObject)

            Try

                If _gbltokenLeft = -1 Then
                    _gbltokenLeft = jObject.tokensLeft
                    If _gbltokenLeft > 1000 Then
                        _gbltokenLeft = 1000
                    End If
                ElseIf _gbltokenLeft > 0 Then
                    _gbltokenLeft = _gbltokenLeft - 1
                End If


                For Each proObj As Product In jObject.products

                    Dim abc As Int64()() = proObj.csv
                    Dim aas As Int32 = abc(1).Length
                    ' 1 index is for New items
                    For index As Int32 = 0 To abc(1).Length - 1 Step 2
                        If objval.ToDouble(abc(1)(index + 1)) > 0 Then
                            gblDTNewsub.Rows.Add(KeepaToDateTime(abc(1)(index)), objval.ToDouble(abc(1)(index + 1)) / 100.0)
                        End If
                    Next
                    ' 2 index is for USED
                    For index As Int32 = 0 To abc(2).Length - 1 Step 2
                        If objval.ToDouble(abc(2)(index + 1)) > 0 Then
                            gblDTUsed.Rows.Add(KeepaToDateTime(abc(2)(index)), objval.ToDouble(abc(2)(index + 1)) / 100.0)
                        End If
                    Next
                    ' 3 index is for SALESRank
                    For index As Int32 = 0 To abc(3).Length - 1 Step 2
                        If objval.ToDouble(abc(3)(index + 1)) > 0 Then
                            gblDTSaleRank.Rows.Add(KeepaToDateTime(abc(3)(index)), objval.ToDouble(abc(3)(index + 1)))
                        End If
                    Next

                    Dim aa As String = proObj.manufacturer
                Next

            Catch ex As Exception
                gblErrorLog.AppendLine("Converstion Failed for :" & _ProductASIN)
                gblErrorLog.AppendLine(ex.Message.ToString())
                HDbackWork.ReportProgress(1)
                GoTo skippnow
            End Try
            '' Update To DB

            _Used30Avg = GetKeepaAvg(gblDTUsed, 30, 0)
            _Used90Avg = GetKeepaAvg(gblDTUsed, 90, 0)
            _Used180Avg = GetKeepaAvg(gblDTUsed, 180, 0)
            _Used365Avg = GetKeepaAvg(gblDTUsed, 365, 0)
            _Used3YrAvg = GetKeepaAvg(gblDTUsed, 3, 3)
            _Used5YrAvg = GetKeepaAvg(gblDTUsed, 5, 3)

            _New30Avg = GetKeepaAvg(gblDTNewsub, 30, 0)
            _New90Avg = GetKeepaAvg(gblDTNewsub, 90, 0)
            _New180Avg = GetKeepaAvg(gblDTNewsub, 180, 0)
            _New365Avg = GetKeepaAvg(gblDTNewsub, 365, 0)
            _New3YrAvg = GetKeepaAvg(gblDTNewsub, 3, 3)
            _New5YrAvg = GetKeepaAvg(gblDTNewsub, 5, 3)

            _SaleR30Avg = GetKeepaRankAvg(gblDTSaleRank, 30, 0)
            _SaleR90Avg = GetKeepaRankAvg(gblDTSaleRank, 90, 0)
            _SaleR180Avg = GetKeepaRankAvg(gblDTSaleRank, 180, 0)
            _SaleR365Avg = GetKeepaRankAvg(gblDTSaleRank, 365, 0)
            _SaleR3YrAvg = GetKeepaRankAvg(gblDTSaleRank, 3, 3)

            Dim retVal As Int32 = 0
            Dim DtProChk As DataTable = dbOperation_keepa.GetDataTable("select * from keepa_price where product_asin='" & _ProductASIN & "'", retVal)
            If retVal = 1 Then
                Try
                    Dim con As New SQLiteConnection(DBconnStrKeepa)
                    Dim cmd As New SQLiteCommand()
                    cmd.Connection = con
                    cmd.CommandType = CommandType.Text

                    If DtProChk.Rows.Count = 0 Then
                        ' Insert 

                        cmd.CommandText = "	insert into keepa_price (product_asin,day_30_new,day_90_new,day_180_new,day_365_new,year_3_new,year_5_new, " &
                                        "	day_30_used,day_90_used,day_180_used,day_365_used,year_3_used,year_5_used,	                   " &
                                        "	day_30_salerank,day_90_salerank,day_180_salerank,day_365_salerank,year_3_salerank)             " &
                                        "	values(@product_asin,@day_30_new,@day_90_new,@day_180_new,@day_365_new,@year_3_new,@year_5_new," &
                                        "		@day_30_used,@day_90_used,@day_180_used,@day_365_used,@year_3_used,@year_5_used,	      " &
                                        "		@day_30_salerank,@day_90_salerank,@day_180_salerank,@day_365_salerank,@year_3_salerank)"

                    Else
                        ' Update
                        cmd.CommandText = "update keepa_price set " &
                                        "day_30_new =@day_30_new,               " &
                                        "day_90_new=@day_90_new,                " &
                                        "day_180_new=@day_180_new,              " &
                                        "day_365_new=@day_365_new,              " &
                                        "year_3_new	=@year_3_new,               " &
                                        "year_5_new = @year_5_new,              " &
                                        "day_30_used=@day_30_used,              " &
                                        "day_90_used=@day_90_used,              " &
                                        "day_180_used=@day_180_used,            " &
                                        "day_365_used=@day_365_used,            " &
                                        "year_3_used=@year_3_used,              " &
                                        "year_5_used=@year_5_used,              " &
                                        "day_30_salerank=@day_30_salerank,      " &
                                        "day_90_salerank=@day_90_salerank,      " &
                                        "day_180_salerank=@day_180_salerank,    " &
                                        "day_365_salerank=@day_365_salerank,    " &
                                        "year_3_salerank	=@year_3_salerank	" &
                                        "where product_asin=@product_asin"
                    End If
                    cmd.Parameters.AddWithValue("@product_asin", _ProductASIN)
                    cmd.Parameters.AddWithValue("@day_30_new", _New30Avg)
                    cmd.Parameters.AddWithValue("@day_90_new", _New90Avg)
                    cmd.Parameters.AddWithValue("@day_180_new", _New180Avg)
                    cmd.Parameters.AddWithValue("@day_365_new", _New365Avg)
                    cmd.Parameters.AddWithValue("@year_3_new", _New3YrAvg)
                    cmd.Parameters.AddWithValue("@year_5_new", _New5YrAvg)
                    cmd.Parameters.AddWithValue("@day_30_used", _Used30Avg)
                    cmd.Parameters.AddWithValue("@day_90_used", _Used90Avg)
                    cmd.Parameters.AddWithValue("@day_180_used", _Used180Avg)
                    cmd.Parameters.AddWithValue("@day_365_used", _Used365Avg)
                    cmd.Parameters.AddWithValue("@year_3_used", _Used3YrAvg)
                    cmd.Parameters.AddWithValue("@year_5_used", _Used5YrAvg)
                    cmd.Parameters.AddWithValue("@day_30_salerank", _SaleR30Avg)
                    cmd.Parameters.AddWithValue("@day_90_salerank", _SaleR90Avg)
                    cmd.Parameters.AddWithValue("@day_180_salerank", _SaleR180Avg)
                    cmd.Parameters.AddWithValue("@day_365_salerank", _SaleR365Avg)
                    cmd.Parameters.AddWithValue("@year_3_salerank", _SaleR3YrAvg)

                    con.Open()
                    Dim ddssdd As Integer = cmd.ExecuteNonQuery()
                    cmd.Parameters.Clear()
                    con.Close()

                Catch ex As Exception
                    gblErrorLog.AppendLine("DB Entry Failed : " & _ProductASIN)
                    gblErrorLog.AppendLine(ex.Message.ToString())
                    HDbackWork.ReportProgress(1)
                    GoTo skippnow
                End Try
                HDbackWork.ReportProgress(1)
            Else
                gblErrorLog.AppendLine("Item not exists or may be deleted. Item Skipped : " & _ProductASIN)
                HDbackWork.ReportProgress(1)
            End If
            'gblErrorLog.AppendLine("" & _ProductASIN & " Completed")
            'HDbackWork.ReportProgress(1)
skippnow:



            Return 1
        Catch ex As Exception
            Return -1
        End Try
    End Function

    Private Sub HDbackWork_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles HDbackWork.ProgressChanged
        If _totalProcessDone > 0 Then
            lblPercentage.Text = Math.Round((_totalProcessDone / _totalProductCnt) * 100, 2) & "%"
            txtErrorLog.Text = Convert.ToString(gblErrorLog)
            lblScanComplete.Text = _totalProcessDone
        End If
    End Sub

    Private Sub HDbackWork_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles HDbackWork.RunWorkerCompleted
        pnlUpdating.Visible = False
        lblprocessStopped.Visible = False

        Try
            Dim con As New SQLiteConnection(DBconnStrKeepa)
            Dim cmd As New SQLiteCommand()
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "update setting set last_value =@last where id=1"
            If _gblLastProductIDtoUpdate = _gblLastProductID Then
                cmd.Parameters.AddWithValue("@last", 0)
            Else
                cmd.Parameters.AddWithValue("@last", _gblLastProductIDtoUpdate)
            End If

            con.Open()
            Dim ddssdd As Integer = cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            con.Close()
        Catch ex As Exception
            gblErrorLog.AppendLine("1. Error while updating last product id scanned.")
        End Try

        'Try
        '    Dim con As New SQLiteConnection(DBconnStrKeepa)
        '    Dim cmd As New SQLiteCommand()
        '    cmd.Connection = con
        '    cmd.CommandType = CommandType.Text
        '    cmd.CommandText = "update setting set last_value =@last where id=1"
        '    cmd.Parameters.AddWithValue("@last", _gblLastProductIDtoUpdate)
        '    con.Open()
        '    Dim ddssdd As Integer = cmd.ExecuteNonQuery()
        '    cmd.Parameters.Clear()
        '    con.Close()
        'Catch ex As Exception
        '    gblErrorLog.AppendLine("1. Error while updating last product id scanned.")
        'End Try


        If _gblisKeepaStart = True Then
            objval.MsgSuccess("KEEPA Scan completed successfully", "Scan Completed")
        Else
            objval.MsgSuccess("KEEPA Scan process stopped", "Process Stopped")
        End If
        btnStop.Enabled = False
        btnStart.Enabled = True
        _gblisKeepaStart = False
        _gbltokenLeft = -1
        _gblLastProductID = _gblLastProductIDtoUpdate
        chkWithPrevious.Enabled = True
    End Sub

#Region "General Functions"
    Public Function GetKeepaAvg(ByVal dt As DataTable, ByVal days As Int32, ByVal deductType As Boolean) As Double
        Try
            Dim total As Double = 0
            Dim cnt As Double = 0

            If deductType = 0 Then ' BY DAY
                total = objval.ToDouble(dt.Compute("Sum(iprice)", "[idate] >= #" & DateTime.Now.AddDays(-1 * days).ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) & "#"))
                cnt = objval.ToDouble(dt.Compute("count(iprice)", "[idate] >= #" & DateTime.Now.AddDays(-1 * days).ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) & "#"))
            ElseIf deductType = 1 Then ' By Month
                total = objval.ToDouble(dt.Compute("Sum(iprice)", "[idate] >= #" & DateTime.Now.AddMonths(-1 * days).ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) & "#"))
                cnt = objval.ToDouble(dt.Compute("count(iprice)", "[idate] >= #" & DateTime.Now.AddMonths(-1 * days).ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) & "#"))
            Else ' By Year
                total = objval.ToDouble(dt.Compute("Sum(iprice)", "[idate] >= #" & DateTime.Now.AddYears(-1 * days).ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) & "#"))
                cnt = objval.ToDouble(dt.Compute("count(iprice)", "[idate] >= #" & DateTime.Now.AddYears(-1 * days).ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) & "#"))
            End If

            If cnt > 0 Then
                Dim avg As Double = 0.0
                avg = Math.Round(total / cnt, 2)
                Return avg
            Else
                Return "0"
            End If
        Catch ex As Exception
            Return "-1"
        End Try
    End Function

    Public Function GetKeepaRankAvg(ByVal dt As DataTable, ByVal days As Int32, ByVal deductType As Boolean) As Double
        Try
            Dim total As Double = 0
            Dim cnt As Double = 0

            If deductType = 0 Then ' BY DAY
                total = objval.ToDouble(dt.Compute("Sum(iprice)", "[idate] >= #" & DateTime.Now.AddDays(-1 * days).ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) & "#"))
                cnt = objval.ToDouble(dt.Compute("count(iprice)", "[idate] >= #" & DateTime.Now.AddDays(-1 * days).ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) & "#"))
            ElseIf deductType = 1 Then ' By Month
                total = objval.ToDouble(dt.Compute("Sum(iprice)", "[idate] >= #" & DateTime.Now.AddMonths(-1 * days).ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) & "#"))
                cnt = objval.ToDouble(dt.Compute("count(iprice)", "[idate] >= #" & DateTime.Now.AddMonths(-1 * days).ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) & "#"))
            Else ' By Year
                total = objval.ToDouble(dt.Compute("Sum(iprice)", "[idate] >= #" & DateTime.Now.AddYears(-1 * days).ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) & "#"))
                cnt = objval.ToDouble(dt.Compute("count(iprice)", "[idate] >= #" & DateTime.Now.AddYears(-1 * days).ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern) & "#"))
            End If

            If cnt > 0 Then
                Dim avg As Double = 0.0
                avg = Math.Round(total / cnt, 0)
                Return avg
            Else
                Return "0"
            End If
        Catch ex As Exception
            Return "-1"
        End Try
    End Function

    Public Function KeepaToDateTime(ByVal keepaTime As String) As DateTime
        Try
            Dim strUnixTime As String = (objval.ToInt32(keepaTime) + 21564000) * 60
            Dim nTimestamp As Double = strUnixTime
            Dim nDateTime As System.DateTime = New System.DateTime(1970, 1, 1, 0, 0, 0, 0)
            nDateTime = nDateTime.AddSeconds(nTimestamp)
            Return nDateTime

        Catch ex As Exception
            Return New DateTime(1970, 1, 1, 0, 0, 0, 0)
        End Try
    End Function

#End Region

    Private Sub FrmKeepaScan_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        GblIsKeepaOpen = False
    End Sub

    Private Sub FrmKeepaScan_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        GblIsKeepaOpen = False
    End Sub

    Private Sub btnStop_Click(sender As Object, e As EventArgs) Handles btnStop.Click
        _gblisKeepaStart = False
        lblprocessStopped.Visible = True
    End Sub
End Class