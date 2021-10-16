Imports System.Data.SQLite
Imports System.Data
Imports System.Globalization
Imports System.Xml
Imports System.Net
Imports System.IO
Imports DevExpress.XtraGrid.Views.Grid
Imports AmazonMWS1
Imports System.Threading

Public Class FrmItemDetail

    Dim gblDTAllItem As DataTable
    Dim gblASIN As String = ""
    Dim gblCurrID As Int32 = 0
    'Dim gblNextID As Int32 = 0
    'Dim gblPreviousID As Int32 = 0

    Dim objval As New Oozee
    Dim gblDTamazon As DataTable
    Dim gblDTebay As DataTable
    Dim gblDTkeepa As DataTable
    Dim gblProductID As Int32 = 0
    Dim gblUPC As String = ""
    Dim gblAmazonURL As String = ""

    Dim gblDTNew As DataTable
    Dim gblDTUsed As DataTable
    Dim gblDTSaleRank As DataTable
    Public DBconnStrKeepa As String = "Data Source=|DataDirectory|\keepadb.db; Version=3;"

    Public Sub New()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("McSkin")
        InitializeComponent()
    End Sub

    Public Sub New(ByVal itemID As Int32, ByVal ASIN As String, ByVal dt As DataTable)
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("McSkin")
        InitializeComponent()
        gblASIN = ASIN.Replace("'", "")
        gblDTAllItem = dt
        gblCurrID = itemID
    End Sub

    Private Sub FrmItemDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        gblDTamazon = New DataTable
        gblDTamazon.Columns.Add("colitem", GetType(String))
        gblDTamazon.Columns.Add("colvalue", GetType(String))
        gblDTamazon.Columns.Add("colcode", GetType(String))

        gblDTebay = gblDTamazon.Clone
        gblDTkeepa = gblDTamazon.Clone
        webBrowserCurrent.ScriptErrorsSuppressed = True
        WebBrowserCompleted.ScriptErrorsSuppressed = True
        FillDATA()

    End Sub

    Public Sub FillDATA()
        Try



            gblDTebay.Rows.Clear()
            gblDTkeepa.Rows.Clear()

            Dim retval As Integer = 0
            Dim dt As DataTable = dbOperation.GetDataTable("select * from db_products where product_asin='" & gblASIN & "'", retval)
            If retval > 0 Then
                If dt.Rows.Count > 0 Then
                    lblASIN.Text = gblASIN
                    gblProductID = objval.ToInt32(dt.Rows(0)("product_id"))
                    txtItemName.Text = objval.ToString(dt.Rows(0)("product_name"))
                    lblURL.Text = objval.ToString(dt.Rows(0)("product_url"))
                    gblAmazonURL = objval.ToString(dt.Rows(0)("product_url"))
                    txtNotes.Text = objval.ToString(dt.Rows(0)("notes"))
                    gblUPC = objval.ToString(dt.Rows(0)("upc"))
                    If objval.ToDouble(dt.Rows(0)("live_price")) > 0 Then
                        lblLiveprice.Text = "$" & objval.ToDouble(dt.Rows(0)("live_price"))
                    Else
                        lblLiveprice.Text = "-"
                    End If

                    If objval.ToDouble(dt.Rows(0)("live_newprice")) > 0 Then
                        lblNewPrice.Text = "$" & objval.ToDouble(dt.Rows(0)("live_newprice"))
                    Else
                        lblNewPrice.Text = "-"
                    End If

                    If objval.ToDouble(dt.Rows(0)("live_usedprice")) > 0 Then
                        lblUsedPrice.Text = "$" & objval.ToDouble(dt.Rows(0)("live_usedprice"))
                    Else
                        lblUsedPrice.Text = "-"
                    End If

                    txtTargetNewPrice.Text = objval.ToDouble(dt.Rows(0)("target_price"))
                    txtTargetUSEDPrice.Text = objval.ToDouble(dt.Rows(0)("target_used_price"))

                    If (objval.ToDouble(dt.Rows(0)("live_price")) > 0 And objval.ToDouble(dt.Rows(0)("target_price")) > objval.ToDouble(dt.Rows(0)("live_price"))) _
                    OrElse (objval.ToDouble(dt.Rows(0)("live_newprice")) > 0 And objval.ToDouble(dt.Rows(0)("target_price")) > objval.ToDouble(dt.Rows(0)("live_newprice"))) Then
                        txtTargetNewPrice.ForeColor = Color.Maroon
                        txtTargetNewPrice.BackColor = Color.FromArgb(248, 233, 233)
                    Else
                        txtTargetNewPrice.ForeColor = Color.Black
                        txtTargetNewPrice.BackColor = Color.White
                    End If

                    If objval.ToDouble(dt.Rows(0)("live_usedprice")) > 0 And objval.ToDouble(dt.Rows(0)("live_usedprice")) < objval.ToDouble(dt.Rows(0)("target_used_price")) Then
                        txtTargetUSEDPrice.ForeColor = Color.Maroon
                        txtTargetUSEDPrice.BackColor = Color.FromArgb(248, 233, 233)
                    Else
                        txtTargetUSEDPrice.ForeColor = Color.Black
                        txtTargetUSEDPrice.BackColor = Color.White
                    End If

                    If gblUPC <> "" Then
                        TabPane1.SelectedPageIndex = 0
                        If GblEnabledEbayBrowserACTIVE = True And GblEnabledEbayBrowserCOMPLETE = True Then
                            webBrowserCurrent.Navigate("http://www.ebay.com/sch/i.html?_nkw=" & gblUPC & "&_sop=15")
                            WebBrowserCompleted.Navigate("https://www.ebay.com/sch/i.html?_nkw=" & gblUPC & "&rt=nc&LH_BIN=1&_sop=16&_fsrp=1&LH_Sold=1&LH_Complete=1")
                            SplitContainer.SplitterPosition = SplitContainer.Width / 2
                        ElseIf GblEnabledEbayBrowserACTIVE = True And GblEnabledEbayBrowserCOMPLETE = False Then
                            webBrowserCurrent.Navigate("http://www.ebay.com/sch/i.html?_nkw=" & gblUPC & "&_sop=15")

                            SplitContainer.SplitterPosition = SplitContainer.Size.Width - 10
                        ElseIf GblEnabledEbayBrowserACTIVE = False And GblEnabledEbayBrowserCOMPLETE = True Then
                            WebBrowserCompleted.Navigate("https://www.ebay.com/sch/i.html?_nkw=" & gblUPC & "&rt=nc&LH_BIN=1&_sop=16&_fsrp=1&LH_Sold=1&LH_Complete=1")
                            SplitContainer.SplitterPosition = 10
                        Else
                            TabPane1.SelectedPageIndex = 1
                        End If

                        'WebBrowserCompleted.Navigate("http://www.ebay.com/sch/i.html?LH_Complete=1&_nkw=" & gblUPC & "&rt=nc&LH_BIN=1&_sop=16")
                    End If



                    '''''''''''''''' AMAZON Table data
                    gblDTamazon.Rows.Clear()
                    gblDTamazon.Rows.Add("Lowest Used Price", objval.ToString(dt.Rows(0)("live_usedprice")), "0")
                    gblDTamazon.Rows.Add("Lowest NEW Price", objval.ToString(dt.Rows(0)("live_newprice")), "0")
                    gblDTamazon.Rows.Add("Pre. Used Price", objval.ToString(dt.Rows(0)("live_usedprice_pre")), "0")
                    gblDTamazon.Rows.Add("Pre. NEW Price", objval.ToDouble(dt.Rows(0)("live_newprice_pre")), "0")
                    gblDTamazon.Rows.Add("Sale Rank", objval.ToString(dt.Rows(0)("rank")), "0")
                    gblDTamazon.Rows.Add("Trade IN", objval.ToString(dt.Rows(0)("trade_in_price")), "0")
                    gblDTamazon.Rows.Add("No.Of Pages", objval.ToString(dt.Rows(0)("num_of_pages")), "0")
                    gblDTamazon.Rows.Add("No.Of Disc", objval.ToString(dt.Rows(0)("num_of_disc")), "0")

                    GridAmazon.DataSource = gblDTamazon

                    '''''''''''''''' AMAZON Table data
                    gblDTebay.Rows.Clear()
                    gblDTebay.Rows.Add("Lowest Used Offers", objval.ToString(dt.Rows(0)("ebay_live_used_price_multi")), "0")
                    gblDTebay.Rows.Add("Lowest New Offers", objval.ToString(dt.Rows(0)("ebay_live_new_price_multi")), "0")
                    gblDTebay.Rows.Add("Lowest Used Sold List.", objval.ToString(dt.Rows(0)("ebay_sold_used_price_lowest_multi")), "0")
                    gblDTebay.Rows.Add("Highest Used Sold List.", objval.ToString(dt.Rows(0)("ebay_sold_used_price_high_multi")), "0")
                    gblDTebay.Rows.Add("AVG Used Sold List.", IIf(objval.ToDouble(dt.Rows(0)("ebay_used_comprice_avg")) > 0, "$" & objval.ToString(dt.Rows(0)("ebay_used_comprice_avg")), ""), "0")
                    gblDTebay.Rows.Add("Lowest New Sold List.", objval.ToString(dt.Rows(0)("ebay_sold_new_price_lowest_multi")), "0")
                    gblDTebay.Rows.Add("Highest New Sold List.", objval.ToString(dt.Rows(0)("ebay_sold_new_price_high_multi")), "0")
                    gblDTebay.Rows.Add("AVG New Sold List.", IIf(objval.ToDouble(dt.Rows(0)("ebay_new_comprice_avg")) > 0, "$" & objval.ToString(dt.Rows(0)("ebay_new_comprice_avg")), ""), "0")
                    GridEbay.DataSource = gblDTebay

                    FillKeepaGrid()



                Else
                    objval.MsgWarning("Fail to get item Data", "No Data")
                    Me.Close()
                End If


            Else
                objval.MsgWarning("Fail to get item Data", "Error")
            End If
        Catch ex As Exception
            btnSave.Enabled = False
            btnUpdatePrice.Enabled = False

        End Try
    End Sub


    'Private Sub BR_CurrentListing(ByVal UPCURL)
    '    webBrowser1.Navigate(UPCURL)
    '    AddHandler webBrowser1.ProgressChanged, AddressOf WebBrowserProgressChangedEventHandler(webpage_ProgressChanged)
    '    AddHandler webBrowser1.StatusTextChanged, AddressOf EventHandler(webpage_StatusTextChanged)

    '    'webBrowser1.ProgressChanged += New WebBrowserProgressChangedEventHandler(webpage_ProgressChanged)
    '    ' webBrowser1.DocumentTitleChanged += New EventHandler(webpage_DocumentTitleChanged)
    '    'webBrowser1.StatusTextChanged += New EventHandler(webpage_StatusTextChanged)
    '    ' webBrowser1.Navigated += New WebBrowserNavigatedEventHandler(webpage_Navigated)
    '    ' webBrowser1.DocumentCompleted += New WebBrowserDocumentCompletedEventHandler(webpage_DocumentCompleted)
    'End Sub





    Private Sub btmClose_Click(sender As Object, e As EventArgs) Handles btmClose.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If objval.ToDouble(txtTargetNewPrice.Text) = 0 AndAlso objval.ToDouble(txtTargetUSEDPrice.Text) = 0 Then
            objval.MsgWarning("Please enter 'Target Price' OR 'Used Target' to proceed.", "Invalid")
        Else
            Dim cmd As New SQLiteCommand
            cmd.CommandText = "update db_products set target_price= @target_price,target_used_price=@target_used_price,notes=@notes,last_price_updated=@last_price_updated where product_asin=@product_asin"
            cmd.Parameters.AddWithValue("@target_price", objval.ToDouble(txtTargetNewPrice.Text))
            cmd.Parameters.AddWithValue("@target_used_price", objval.ToDouble(txtTargetUSEDPrice.Text))
            cmd.Parameters.AddWithValue("@product_asin", gblASIN)
            cmd.Parameters.AddWithValue("@notes", txtNotes.Text.Trim)
            cmd.Parameters.AddWithValue("@last_price_updated", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture))

            If dbOperation.ExecuteQuery(cmd) > 0 Then
                objval.MsgSuccess("Price updated successfully", "Success")
                Me.Close()
            End If

        End If

    End Sub

    Private Sub btnUpdatePrice_Click(sender As Object, e As EventArgs) Handles btnUpdatePrice.Click
        Try
            ShowWaitForm()

            Dim _produtArr1 As New List(Of String)()
            _produtArr1.Add(gblASIN)

            Dim ab As New List(Of AmazonMWS1.RetLowestPrice)()
            ab = MWSProductsMain.GetLowestPrice2(_produtArr1)

            For Each lstVar As AmazonMWS1.RetLowestPrice In ab
                Dim ebnprice As Double = 0
                Dim ebuprice As Double = 0
                Dim itmURL As String = "https://www.amazon.com/dp/" & lstVar.asin_key

                Dim con As New SQLiteConnection("Data Source=|DataDirectory|\awsdb.db; Version=3;")

                Dim cmd As New SQLiteCommand()
                cmd.CommandText = "update db_products set last_updated=@last_updated," &
                    "live_newprice=@live_newprice,live_usedprice=@live_usedprice,product_url=@product_url," &
                    "live_newprice_pre=live_newprice,live_usedprice_pre=live_usedprice " &
                    " where product_asin=@product_asin"
                cmd.Connection = con
                cmd.CommandType = CommandType.Text

                cmd.Parameters.AddWithValue("@live_newprice", lstVar.new_price)
                cmd.Parameters.AddWithValue("@live_usedprice", lstVar.used_price)
                cmd.Parameters.AddWithValue("@product_asin", lstVar.asin_key)
                cmd.Parameters.AddWithValue("@product_url", itmURL)
                cmd.Parameters.AddWithValue("@last_updated", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture))

                'Dim dr() As DataRow = glbProcessDT.Select("product_asin='" & lstVar.asin_key & "'")


                'Dim target_price As Double = objval.ToDouble(dr(0).ItemArray(1))
                'Dim target_used_price As Double = objval.ToDouble(dr(0).ItemArray(2))
                Dim upcf As String = gblUPC
                Dim itmTitle As String = txtItemName.Text.Trim

                con.Open()
                Try
                    Dim ddssdd As Integer = cmd.ExecuteNonQuery()

                    If gblUPC <> "" And GblEnabledEbay = True Then
                        Dim HDtasks As New List(Of Task(Of Long))()

                        HDtasks.Add(Task.Factory.StartNew(Function() Me.CompletedsITEM(gblUPC, gblASIN)))
checkAgain:
                        Dim taskCnt As Int64 = HDtasks.Where(Function(x) x.Status = TaskStatus.Running).ToList().Count()

                        If taskCnt > 0 Then
                            Threading.Thread.Sleep(3000)
                            GoTo checkAgain
                        End If

                        HDtasks.Clear()
                    End If

                    'Dim threadNotification As Thread
                    'threadNotification = New Thread(Sub() Me.RunProcess(lstVar.asin_key, lstVar.new_price, lstVar.used_price, ebnprice, ebuprice, itmTitle, itmURL, upcf))
                    'threadNotification.IsBackground = True
                    'threadNotification.Start()

                Catch ex As Exception
                    Dim xsddd As Integer
                    xsddd = 0
                End Try
                cmd.Parameters.Clear()
                con.Close()

                Exit For
            Next


            Try
                ShowWaitForm()
                gblDTNew = New DataTable
                gblDTNew.Columns.Add("idate", GetType(DateTime))
                gblDTNew.Columns.Add("iprice", GetType(Double))

                gblDTUsed = gblDTNew.Clone
                gblDTSaleRank = gblDTNew.Clone

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

                Dim _ProductASIN As String = gblASIN

                Dim URL As String = "https://api.keepa.com/product?key=" & GblKeepaAccessKey & "&domain=1&asin=" & _ProductASIN

                Dim streamText As String = ""
                Dim request As HttpWebRequest = CType(WebRequest.Create(URL), HttpWebRequest)
                request.AutomaticDecompression = DecompressionMethods.Deflate Or DecompressionMethods.GZip
                request.Method = "GET"

                Dim res As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)

                If res.StatusCode = HttpStatusCode.OK Then
                    Dim receiveStream As Stream = res.GetResponseStream()
                    Dim readStream As StreamReader = Nothing
                    readStream = New StreamReader(receiveStream)
                    streamText = readStream.ReadToEnd()
                    res.Close()
                    readStream.Close()
                Else
                    CloseWaitForm()
                    objval.MsgWarning("Failed to get Data from keepa.Server down may down.", "Fail")
                    Exit Sub
                End If

                '  Dim search_response As String = System.IO.File.ReadAllText(Application.StartupPath & "\keepa.txt")
                Dim jObject As RootObject

                Try
                    jObject = CType(Newtonsoft.Json.JsonConvert.DeserializeObject(streamText, GetType(RootObject)), RootObject)
                Catch ex As Exception
                    CloseWaitForm()
                    objval.MsgWarning("API response converstion failed.", "Fail 1.1")
                    Exit Sub
                End Try

                Try


                    For Each proObj As Product In jObject.products
                        Dim abc As Int64()() = proObj.csv
                        Dim aas As Int32 = abc(1).Length
                        ' 1 index is for New items
                        For index As Int32 = 0 To abc(1).Length - 1 Step 2
                            If objval.ToDouble(abc(1)(index + 1)) > 0 Then
                                gblDTNew.Rows.Add(KeepaToDateTime(abc(1)(index)), objval.ToDouble(abc(1)(index + 1)) / 100.0)
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
                    Next
                Catch ex As Exception
                    CloseWaitForm()
                    objval.MsgWarning("API response converstion failed.", "Fail 1.1")
                    Exit Sub
                End Try

                '' Update To DB

                _Used30Avg = GetKeepaAvg(gblDTUsed, 30, 0)
                _Used90Avg = GetKeepaAvg(gblDTUsed, 90, 0)
                _Used180Avg = GetKeepaAvg(gblDTUsed, 180, 0)
                _Used365Avg = GetKeepaAvg(gblDTUsed, 365, 0)
                _Used3YrAvg = GetKeepaAvg(gblDTUsed, 3, 3)
                _Used5YrAvg = GetKeepaAvg(gblDTUsed, 5, 3)

                _New30Avg = GetKeepaAvg(gblDTNew, 30, 0)
                _New90Avg = GetKeepaAvg(gblDTNew, 90, 0)
                _New180Avg = GetKeepaAvg(gblDTNew, 180, 0)
                _New365Avg = GetKeepaAvg(gblDTNew, 365, 0)
                _New3YrAvg = GetKeepaAvg(gblDTNew, 3, 3)
                _New5YrAvg = GetKeepaAvg(gblDTNew, 5, 3)

                _SaleR30Avg = GetKeepaRankAvg(gblDTSaleRank, 30, 0)
                _SaleR90Avg = GetKeepaRankAvg(gblDTSaleRank, 90, 0)
                _SaleR180Avg = GetKeepaRankAvg(gblDTSaleRank, 180, 0)
                _SaleR365Avg = GetKeepaRankAvg(gblDTSaleRank, 365, 0)
                _SaleR3YrAvg = GetKeepaRankAvg(gblDTSaleRank, 3, 3)

                Dim retVal As Int32 = 0
                Dim DtProChk As DataTable = dbOperation_keepa.GetDataTable("select * from keepa_price where product_asin='" & _ProductASIN & "'", retVal)
                If retVal = 1 Then
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
                    CloseWaitForm()
                    'objval.MsgWarning("Sych Successfully completed.", "Success")
                Else
                    CloseWaitForm()
                    objval.MsgWarning("Scan Fail Product Doesn't exists.", "Scan Failed")
                End If
            Catch ex As Exception
                CloseWaitForm()
                objval.MsgError("Some Error occured while Save data to Database.", "Sync Failed")
            End Try

            CloseWaitForm()
            '  objval.MsgSuccess("Price updated from Amazon", "Success")
            FillDATA()
        Catch ex As Exception
            CloseWaitForm()
            objval.MsgError(ex.Message.ToString(), "Error on Update")
        End Try
    End Sub

    Public Function CompletedsITEM(ByVal UPCcode As String, ByVal ASIN As String) As Int64
        'If ASIN <> "B008PDAF7C" Then
        '    Return 0
        'End If

        'If InvokeRequired Then
        '    BeginInvoke(New MethodInvoker(Sub() Me.CompletedsITEM(UPCcode, ASIN)))
        'Else
        Dim nxml As String = ""
        Dim glbEbayDT As New DataTable
        glbEbayDT.Columns.Add("price", GetType(Double))
        glbEbayDT.Columns.Add("state", GetType(String))

        Dim url As String = "http://svcs.ebay.com/services/search/FindingService/v1"
        Dim request2 As System.Net.HttpWebRequest = System.Net.WebRequest.Create(url)
        Dim dtEbayPrices As DataTable = glbEbayDT.Clone

        Dim _NewAVGprice As Double = 0.0
        Dim _NewAVGPercent As Double = 0.0
        Dim _NewLowestPrice As Double = 0.0

        Dim _UsedAVGprice As Double = 0.0
        Dim _UsedAVGPercent As Double = 0.0
        Dim _UsedLowestPrice As Double = 0.0
        Dim _New_LowestMulti As String = ""
        Dim _Used_LowestMulti As String = ""
        Dim _New_Sold_LowestMulti As String = ""
        Dim _Used_Sold_LowestMulti As String = ""
        Dim _New_Sold_HighMulti As String = ""
        Dim _Used_Sold_HighMulti As String = ""
        Dim _gblCnt As Int32 = 1


        '`ebay_live_new_price_multi`	TEXT,
        '`ebay_live_used_price_multi`	TEXT,
        '`ebay_sold_new_price_high_multi`	TEXT,
        '`ebay_sold_used_price_high_multi`	TEXT,
        '`ebay_sold_new_price_lowest_multi`	TEXT,
        '`ebay_sold_used_price_lowest_multi`	TEXT
        ''''''''''''''' GET NEW LOWEST

        Try
            'findItemsByProductRequest findItemsByProduct
            nxml = "<?xml version=""1.0"" encoding=""UTF-8""?> " &
                    " <findItemsByProductRequest xmlns=""http://www.ebay.com/marketplace/search/v1/services""> " &
                    " <productId type=""UPC"">" & UPCcode & "</productId> " &
                    "  <itemFilter>" &
                    "      <name>Condition</name> <value>1000</value> <value>1500</value> <value>1750</value> <value>2000</value> <value>2500</value> </itemFilter> " &
                    "   <paginationInput>" &
                    "       <entriesPerPage>3</entriesPerPage>" &
                    "   </paginationInput>" &
                    "  <sortOrder> PricePlusShippingLowest </sortOrder>" &
                    " </findItemsByProductRequest>"

            Dim Xml_bytes() As Byte = System.Text.Encoding.UTF8.GetBytes(nxml)
            request2.Headers.Add("X-EBAY-SOA-SERVICE-NAME", "FindingService")
            request2.Headers.Add("X-EBAY-SOA-OPERATION-NAME", "findItemsByProduct")
            request2.Headers.Add("X-EBAY-SOA-SERVICE-VERSION", "1.12.0")
            'request2.Headers.Add("X-EBAY-SOA-GLOBAL-ID", "EBAY-US")
            request2.Headers.Add("X-EBAY-SOA-SECURITY-APPNAME", GblebAppID)
            request2.Headers.Add("X-EBAY-SOA-REQUEST-DATA-FORMAT", "XML")
            request2.Method = "POST"

            request2.ContentLength = Xml_bytes.Length
            request2.ContentType = "text/xml"
            Dim requestStream As System.IO.Stream = request2.GetRequestStream()
            requestStream.Write(Xml_bytes, 0, Xml_bytes.Length)
            requestStream.Close()

            Dim response2 As System.Net.HttpWebResponse = request2.GetResponse()
            Dim responseReader As IO.StreamReader = New IO.StreamReader(response2.GetResponseStream(), System.Text.Encoding.UTF8)
            Dim strResponse = responseReader.ReadToEnd()

            Dim reader As New XmlTextReader(New System.IO.StringReader(strResponse))
            reader.WhitespaceHandling = WhitespaceHandling.None

            Dim RewardXmlDoc As New XmlDocument()
            Dim isItemLookExist As Boolean = False

            While reader.Read()
                If reader.Name = "searchResult" Then
                    RewardXmlDoc.LoadXml(reader.ReadOuterXml())
                    reader.Close()
                    isItemLookExist = True
                End If
            End While

            response2.Close()
            responseReader.Dispose()
            reader.Close()

            If isItemLookExist = True Then
                Dim nsmgr As New XmlNamespaceManager(RewardXmlDoc.NameTable)
                nsmgr.AddNamespace("rate", "http://www.ebay.com/marketplace/search/v1/services")
                Dim nd As XmlNodeList = RewardXmlDoc.SelectNodes("//rate:item", nsmgr)
                _gblCnt = 1
                For Each node As XmlNode In nd
                    Dim _currentPrice As XmlNode = node.SelectSingleNode("./rate:sellingStatus/rate:currentPrice", nsmgr)
                    If Not _currentPrice Is Nothing Then
                        If _gblCnt = 1 Then
                            _NewLowestPrice = objval.ToDouble(_currentPrice.InnerText)
                            If _NewLowestPrice > 0 Then
                                _New_LowestMulti = "$" & _NewLowestPrice
                            End If

                        ElseIf _gblCnt > 3 Then
                            Exit For
                        Else
                            If objval.ToDouble(_currentPrice.InnerText) > 0 Then
                                _New_LowestMulti = _New_LowestMulti & " | $" & objval.ToDouble(_currentPrice.InnerText)
                            End If
                        End If
                        _gblCnt = _gblCnt + 1
                    End If
                Next
            End If

        Catch ex As Exception

            _NewLowestPrice = 0
        End Try

        ''''''''''''''' GET NEW AVG PRICE

        Try
            request2 = System.Net.WebRequest.Create(url)
            nxml = "<?xml version=""1.0"" encoding=""UTF-8""?> " &
                    " <findCompletedItemsRequest xmlns=""http://www.ebay.com/marketplace/search/v1/services""> " &
                    " <productId type=""UPC"">" & UPCcode & "</productId> " &
                    "  <itemFilter>" &
                    "      <name>Condition</name> <value>1000</value> <value>1500</value> <value>1750</value> <value>2000</value> <value>2500</value> </itemFilter> " &
                    "   <paginationInput>" &
                    "       <entriesPerPage>100</entriesPerPage>" &
                    "   </paginationInput>" &
                    "  <sortOrder> PricePlusShippingLowest </sortOrder>" &
                    " </findCompletedItemsRequest>"

            Dim Xml_bytes() As Byte = System.Text.Encoding.UTF8.GetBytes(nxml)
            request2.Headers.Add("X-EBAY-SOA-SERVICE-NAME", "FindingService")
            request2.Headers.Add("X-EBAY-SOA-OPERATION-NAME", "findCompletedItems")
            request2.Headers.Add("X-EBAY-SOA-SERVICE-VERSION", "1.12.0")
            'request2.Headers.Add("X-EBAY-SOA-GLOBAL-ID", "EBAY-US")
            request2.Headers.Add("X-EBAY-SOA-SECURITY-APPNAME", GblebAppID)
            request2.Headers.Add("X-EBAY-SOA-REQUEST-DATA-FORMAT", "XML")
            request2.Method = "POST"

            request2.ContentLength = Xml_bytes.Length
            request2.ContentType = "text/xml"
            Dim requestStream As System.IO.Stream = request2.GetRequestStream()
            requestStream.Write(Xml_bytes, 0, Xml_bytes.Length)
            requestStream.Close()

            Dim response2 As System.Net.HttpWebResponse = request2.GetResponse()
            Dim responseReader As IO.StreamReader = New IO.StreamReader(response2.GetResponseStream(), System.Text.Encoding.UTF8)
            Dim strResponse = responseReader.ReadToEnd()

            Dim reader As New XmlTextReader(New System.IO.StringReader(strResponse))
            reader.WhitespaceHandling = WhitespaceHandling.None

            Dim RewardXmlDoc As New XmlDocument()
            Dim isItemLookExist As Boolean = False

            While reader.Read()
                If reader.Name = "searchResult" Then
                    RewardXmlDoc.LoadXml(reader.ReadOuterXml())
                    reader.Close()
                    isItemLookExist = True
                End If
            End While

            response2.Close()
            responseReader.Dispose()
            reader.Close()

            If isItemLookExist = True Then
                Dim nsmgr As New XmlNamespaceManager(RewardXmlDoc.NameTable)
                nsmgr.AddNamespace("rate", "http://www.ebay.com/marketplace/search/v1/services")

                Dim nd As XmlNodeList = RewardXmlDoc.SelectNodes("//rate:item", nsmgr)
                dtEbayPrices.Rows.Clear()

                For Each node As XmlNode In nd
                    Dim _currentPrice As XmlNode = node.SelectSingleNode("./rate:sellingStatus/rate:currentPrice", nsmgr)
                    Dim _sellingState As XmlNode = node.SelectSingleNode("./rate:sellingStatus/rate:sellingState", nsmgr)

                    Dim curprice As Double = 0
                    Dim curState As String = ""

                    If Not _currentPrice Is Nothing Then
                        curprice = objval.ToDouble(_currentPrice.InnerText)
                    End If
                    If Not _sellingState Is Nothing Then
                        curState = objval.ToString(_sellingState.InnerText)
                    End If

                    If curprice <> 0 And curState.Trim.Length > 0 Then
                        dtEbayPrices.Rows.Add(curprice, curState)
                    End If
                Next
            End If



            Dim totalPrice As Double = objval.ToDouble(dtEbayPrices.Compute("SUM(price)", "state='EndedWithSales'"))
            Dim soldCount As Double = objval.ToInt32(dtEbayPrices.Compute("COUNT(price)", "state='EndedWithSales'"))
            Dim totalCount As Double = objval.ToInt32(dtEbayPrices.Compute("COUNT(price)", ""))

            ' price,state
            If soldCount > 0 Then
                _NewAVGprice = totalPrice / soldCount
            Else
                _NewAVGprice = 0
            End If

            If totalCount > 0 Then
                _NewAVGPercent = (soldCount / totalCount) * 100.0
            Else
                _NewAVGPercent = 0
            End If

            Dim lowDv As New DataView(dtEbayPrices)
            lowDv.RowFilter = "state='EndedWithSales'"
            lowDv.Sort = "price ASC"
            dtEbayPrices = lowDv.ToTable
            For lowcnt As Integer = 0 To dtEbayPrices.Rows.Count - 1
                If lowcnt = 0 Then
                    _New_Sold_LowestMulti = "$" & objval.ToDouble(dtEbayPrices.Rows(lowcnt)("price"))
                ElseIf lowcnt = 3 Then
                    Exit For
                Else
                    _New_Sold_LowestMulti = _New_Sold_LowestMulti & " | $" & objval.ToDouble(dtEbayPrices.Rows(lowcnt)("price"))
                End If
            Next

            lowDv = New DataView(dtEbayPrices)
            lowDv.RowFilter = "state='EndedWithSales'"
            lowDv.Sort = "price DESC"
            dtEbayPrices = lowDv.ToTable

            For lowcnt As Integer = 0 To dtEbayPrices.Rows.Count - 1
                If lowcnt = 0 Then
                    _New_Sold_HighMulti = "$" & objval.ToDouble(dtEbayPrices.Rows(lowcnt)("price"))
                ElseIf lowcnt = 3 Then
                    Exit For
                Else
                    _New_Sold_HighMulti = _New_Sold_HighMulti & " | $" & objval.ToDouble(dtEbayPrices.Rows(lowcnt)("price"))
                End If
            Next


        Catch ex As Exception

            _NewAVGprice = 0
            _NewAVGPercent = 0
        End Try

        ''''''''''''''' GET USED LOWEST PRICE

        Try
            'findItemsByProductRequest findItemsByProduct
            request2 = System.Net.WebRequest.Create(url)
            nxml = "<?xml version=""1.0"" encoding=""UTF-8""?> " &
                    " <findItemsByProductRequest xmlns=""http://www.ebay.com/marketplace/search/v1/services""> " &
                    " <productId type=""UPC"">" & UPCcode & "</productId> " &
                    " <itemFilter> " &
                      "  <name>Condition</name>" &
                      "  <value>3000</value>" &
                      "  <value>4000</value>" &
                      "  <value>5000</value>" &
                      "  <value>6000</value>" &
                      "  <value>7000</value>" &
                      " </itemFilter>" &
                    "   <paginationInput>" &
                    "       <entriesPerPage>3</entriesPerPage>" &
                    "   </paginationInput>" &
                    "  <sortOrder> PricePlusShippingLowest </sortOrder>" &
                    " </findItemsByProductRequest>"

            Dim Xml_bytes() As Byte = System.Text.Encoding.UTF8.GetBytes(nxml)
            request2.Headers.Add("X-EBAY-SOA-SERVICE-NAME", "FindingService")
            request2.Headers.Add("X-EBAY-SOA-OPERATION-NAME", "findItemsByProduct")
            request2.Headers.Add("X-EBAY-SOA-SERVICE-VERSION", "1.12.0")
            request2.Headers.Add("X-EBAY-SOA-GLOBAL-ID", "EBAY-US")
            request2.Headers.Add("X-EBAY-SOA-SECURITY-APPNAME", GblebAppID)
            request2.Headers.Add("X-EBAY-SOA-REQUEST-DATA-FORMAT", "XML")
            request2.Method = "POST"

            request2.ContentLength = Xml_bytes.Length
            request2.ContentType = "text/xml"
            Dim requestStream As System.IO.Stream = request2.GetRequestStream()
            requestStream.Write(Xml_bytes, 0, Xml_bytes.Length)
            requestStream.Close()

            Dim response2 As System.Net.HttpWebResponse = request2.GetResponse()
            Dim responseReader As IO.StreamReader = New IO.StreamReader(response2.GetResponseStream(), System.Text.Encoding.UTF8)
            Dim strResponse = responseReader.ReadToEnd()

            Dim reader As New XmlTextReader(New System.IO.StringReader(strResponse))
            reader.WhitespaceHandling = WhitespaceHandling.None

            Dim RewardXmlDoc As New XmlDocument()
            Dim isItemLookExist As Boolean = False

            While reader.Read()
                If reader.Name = "searchResult" Then
                    RewardXmlDoc.LoadXml(reader.ReadOuterXml())
                    reader.Close()
                    isItemLookExist = True
                End If
            End While

            response2.Close()
            responseReader.Dispose()
            reader.Close()

            If isItemLookExist = True Then
                Dim nsmgr As New XmlNamespaceManager(RewardXmlDoc.NameTable)
                nsmgr.AddNamespace("rate", "http://www.ebay.com/marketplace/search/v1/services")

                Dim nd As XmlNodeList = RewardXmlDoc.SelectNodes("//rate:item", nsmgr)
                _gblCnt = 1

                For Each node As XmlNode In nd
                    Dim _currentPrice As XmlNode = node.SelectSingleNode("./rate:sellingStatus/rate:currentPrice", nsmgr)

                    If Not _currentPrice Is Nothing Then
                        '_UsedLowestPrice = objval.ToDouble(_currentPrice.InnerText)
                        'Exit For
                        If _gblCnt = 1 Then
                            _UsedLowestPrice = objval.ToDouble(_currentPrice.InnerText)
                            If _UsedLowestPrice > 0 Then
                                _Used_LowestMulti = "$" & _UsedLowestPrice
                            End If
                        ElseIf _gblCnt > 3 Then
                            Exit For
                        Else
                            If objval.ToDouble(_currentPrice.InnerText) > 0 Then
                                _Used_LowestMulti = _Used_LowestMulti & " | $" & objval.ToDouble(_currentPrice.InnerText)
                            End If
                        End If
                        _gblCnt = _gblCnt + 1

                    End If
                Next
            End If

        Catch ex As Exception

            _UsedLowestPrice = 0
        End Try

        ''''''''''''''' GET USED AVG PRICE
        Try
            request2 = System.Net.WebRequest.Create(url)
            nxml = "<?xml version=""1.0"" encoding=""UTF-8""?> " &
                    " <findCompletedItemsRequest xmlns=""http://www.ebay.com/marketplace/search/v1/services""> " &
                    " <productId type=""UPC"">" & UPCcode & "</productId> " &
                    " <itemFilter> " &
                      "  <name>Condition</name>" &
                      "  <value>3000</value>" &
                      "  <value>4000</value>" &
                      "  <value>5000</value>" &
                      "  <value>6000</value>" &
                      "  <value>7000</value>" &
                      " </itemFilter>" &
                    "   <paginationInput>" &
                    "       <entriesPerPage>100</entriesPerPage>" &
                    "   </paginationInput>" &
                    "  <sortOrder> PricePlusShippingLowest </sortOrder>" &
                    " </findCompletedItemsRequest>"

            Dim Xml_bytes() As Byte = System.Text.Encoding.UTF8.GetBytes(nxml)
            request2.Headers.Add("X-EBAY-SOA-SERVICE-NAME", "FindingService")
            request2.Headers.Add("X-EBAY-SOA-OPERATION-NAME", "findCompletedItems")
            request2.Headers.Add("X-EBAY-SOA-SERVICE-VERSION", "1.12.0")
            request2.Headers.Add("X-EBAY-SOA-GLOBAL-ID", "EBAY-US")
            request2.Headers.Add("X-EBAY-SOA-SECURITY-APPNAME", GblebAppID)
            request2.Headers.Add("X-EBAY-SOA-REQUEST-DATA-FORMAT", "XML")
            request2.Method = "POST"

            request2.ContentLength = Xml_bytes.Length
            request2.ContentType = "text/xml"
            Dim requestStream As System.IO.Stream = request2.GetRequestStream()
            requestStream.Write(Xml_bytes, 0, Xml_bytes.Length)
            requestStream.Close()

            Dim response2 As System.Net.HttpWebResponse = request2.GetResponse()
            Dim responseReader As IO.StreamReader = New IO.StreamReader(response2.GetResponseStream(), System.Text.Encoding.UTF8)
            Dim strResponse = responseReader.ReadToEnd()

            Dim reader As New XmlTextReader(New System.IO.StringReader(strResponse))
            reader.WhitespaceHandling = WhitespaceHandling.None

            Dim RewardXmlDoc As New XmlDocument()
            Dim isItemLookExist As Boolean = False

            While reader.Read()
                If reader.Name = "searchResult" Then
                    RewardXmlDoc.LoadXml(reader.ReadOuterXml())
                    reader.Close()
                    isItemLookExist = True
                End If
            End While

            response2.Close()
            responseReader.Dispose()
            reader.Close()

            If isItemLookExist = True Then
                Dim nsmgr As New XmlNamespaceManager(RewardXmlDoc.NameTable)
                nsmgr.AddNamespace("rate", "http://www.ebay.com/marketplace/search/v1/services")

                Dim nd As XmlNodeList = RewardXmlDoc.SelectNodes("//rate:item", nsmgr)
                dtEbayPrices.Rows.Clear()

                For Each node As XmlNode In nd
                    Dim _currentPrice As XmlNode = node.SelectSingleNode("./rate:sellingStatus/rate:currentPrice", nsmgr)
                    Dim _sellingState As XmlNode = node.SelectSingleNode("./rate:sellingStatus/rate:sellingState", nsmgr)

                    Dim curprice As Double = 0
                    Dim curState As String = ""

                    If Not _currentPrice Is Nothing Then
                        curprice = objval.ToDouble(_currentPrice.InnerText)
                    End If
                    If Not _sellingState Is Nothing Then
                        curState = objval.ToString(_sellingState.InnerText)
                    End If

                    If curprice <> 0 And curState.Trim.Length > 0 Then
                        dtEbayPrices.Rows.Add(curprice, curState)
                    End If
                Next
            End If

            Dim totalPrice As Double = objval.ToDouble(dtEbayPrices.Compute("SUM(price)", "state='EndedWithSales'"))
            Dim soldCount As Double = objval.ToInt32(dtEbayPrices.Compute("COUNT(price)", "state='EndedWithSales'"))
            Dim totalCount As Double = objval.ToInt32(dtEbayPrices.Compute("COUNT(price)", ""))

            ' price,state
            If soldCount > 0 Then
                _UsedAVGprice = totalPrice / soldCount
            Else
                _UsedAVGprice = 0
            End If

            If totalCount > 0 Then
                _UsedAVGPercent = (soldCount / totalCount) * 100.0
            Else
                _UsedAVGPercent = 0
            End If


            Dim lowDv As New DataView(dtEbayPrices)
            lowDv.RowFilter = "state='EndedWithSales'"
            lowDv.Sort = "price ASC"
            dtEbayPrices = lowDv.ToTable
            For lowcnt As Integer = 0 To dtEbayPrices.Rows.Count - 1
                If lowcnt = 0 Then
                    _Used_Sold_LowestMulti = "$" & objval.ToDouble(dtEbayPrices.Rows(lowcnt)("price"))
                ElseIf lowcnt = 3 Then
                    Exit For
                Else
                    _Used_Sold_LowestMulti = _Used_Sold_LowestMulti & " | $" & objval.ToDouble(dtEbayPrices.Rows(lowcnt)("price"))
                End If
            Next

            lowDv = New DataView(dtEbayPrices)
            lowDv.RowFilter = "state='EndedWithSales'"
            lowDv.Sort = "price DESC"
            dtEbayPrices = lowDv.ToTable

            For lowcnt As Integer = 0 To dtEbayPrices.Rows.Count - 1
                If lowcnt = 0 Then
                    _Used_Sold_HighMulti = "$" & objval.ToDouble(dtEbayPrices.Rows(lowcnt)("price"))
                ElseIf lowcnt = 3 Then
                    Exit For
                Else
                    _Used_Sold_HighMulti = _Used_Sold_HighMulti & " | $" & objval.ToDouble(dtEbayPrices.Rows(lowcnt)("price"))
                End If
            Next

        Catch ex As Exception

            _UsedAVGprice = 0
            _UsedAVGPercent = 0
        End Try

        '' UPDATE INTO DATABASE

        Try
            Dim con As New SQLiteConnection("Data Source=|DataDirectory|\awsdb.db; Version=3;")
            Dim cmd As New SQLiteCommand()
            cmd.CommandText = "update db_products set " &
                "ebay_live_new_price=@ebay_live_new_price,ebay_live_used_price=@ebay_live_used_price,ebay_new_comprice_avg=@ebay_new_comprice_avg,ebay_used_comprice_avg=@ebay_used_comprice_avg,  " &
                "ebay_new_comprice_per=@ebay_new_comprice_per,ebay_used_comprice_per=@ebay_used_comprice_per, " &
                "ebay_live_new_price_multi=@ebay_live_new_price_multi,ebay_live_used_price_multi=@ebay_live_used_price_multi," &
                "ebay_sold_new_price_high_multi=@ebay_sold_new_price_high_multi,ebay_sold_used_price_high_multi=@ebay_sold_used_price_high_multi," &
                "ebay_sold_new_price_lowest_multi=@ebay_sold_new_price_lowest_multi,ebay_sold_used_price_lowest_multi=@ebay_sold_used_price_lowest_multi " &
                " where product_asin=@product_asin"
            cmd.Connection = con
            cmd.CommandType = CommandType.Text


            'Dim _New_LowestMulti As String = ""
            'Dim _Used_LowestMulti As String = ""
            'Dim _New_Sold_LowestMulti As String = ""
            'Dim _Used_Sold_LowestMulti As String = ""
            'Dim _New_Sold_HighMulti As String = ""
            'Dim _Used_Sold_HighMulti As String = ""
            'Dim _gblCnt As Int32 = 1


            ''`ebay_live_new_price_multi`	TEXT,
            ''`ebay_live_used_price_multi`	TEXT,
            ''`ebay_sold_new_price_high_multi`	TEXT,
            ''`ebay_sold_used_price_high_multi`	TEXT,
            ''`ebay_sold_new_price_lowest_multi`	TEXT,
            ''`ebay_sold_used_price_lowest_multi`	TEXT

            cmd.Parameters.AddWithValue("@ebay_live_new_price", Math.Round(_NewLowestPrice, 2))
            cmd.Parameters.AddWithValue("@ebay_live_used_price", Math.Round(_UsedLowestPrice, 2))
            cmd.Parameters.AddWithValue("@ebay_new_comprice_avg", Math.Round(_NewAVGprice, 2))
            cmd.Parameters.AddWithValue("@ebay_used_comprice_avg", Math.Round(_UsedAVGprice, 2))
            cmd.Parameters.AddWithValue("@ebay_new_comprice_per", Math.Round(_NewAVGPercent, 2))
            cmd.Parameters.AddWithValue("@ebay_used_comprice_per", Math.Round(_UsedAVGPercent, 2))
            cmd.Parameters.AddWithValue("@product_asin", ASIN)
            ''' Logs of price
            cmd.Parameters.AddWithValue("@ebay_live_new_price_multi", _New_LowestMulti)
            cmd.Parameters.AddWithValue("@ebay_live_used_price_multi", _Used_LowestMulti)
            cmd.Parameters.AddWithValue("@ebay_sold_new_price_high_multi", _New_Sold_HighMulti)
            cmd.Parameters.AddWithValue("@ebay_sold_used_price_high_multi", _Used_Sold_HighMulti)
            cmd.Parameters.AddWithValue("@ebay_sold_new_price_lowest_multi", _New_Sold_LowestMulti)
            cmd.Parameters.AddWithValue("@ebay_sold_used_price_lowest_multi", _Used_Sold_LowestMulti)


            con.Open()
            Dim ddssdd As Integer = cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            con.Close()
        Catch ex As Exception

        End Try
        ' End If

        Return 0
    End Function


    Public Function GetAWSItem(AWSSellerID As [String], AWSAccessKey As [String], AWSSecretKey As [String], ASINNo As [String], ByRef _price As [Double], ByRef _newprice As [Double], _
        ByRef _usedprice As [Double], ByRef _itemtitle As [String], ByRef _itemWEBUrL As [String], ByRef ExLog As [String]) As [Boolean]
        Dim requestUrl As [String] = ""
        Try
            _price = 0.0
            _newprice = 0.0
            _usedprice = 0.0
            _itemtitle = ""

            Dim helper As New SignedRequestHelper(AWSAccessKey, AWSSecretKey, "webservices.amazon.com")

            Dim r1 As IDictionary(Of String, String) = New Dictionary(Of String, [String])()
            r1("Service") = "AWSECommerceService"
            r1("Operation") = "ItemLookup"
            r1("ItemId") = ASINNo
            'r1("ItemLookup.1.ItemId") = "B00C6F60NI,B000FQISSU,B0009HI5KQ,B000O7864G,B0027UY8B8,B00DXLO086,B00KVS6YZQ,B00007M5HU,B001BN4WA4,B000002TSV"
            'r1("ItemLookup.2.ItemId") = "B00002CF5X,B0000541WJ,B00005NX1J,B00006BSGS,B00007IG1M,B000083C6T,B0000AYL45,B0000B1OC4,B0001I5592"
            r1("ResponseGroup") = "ItemAttributes,Offers,OfferFull,OfferListings"
            r1("ItemLookup.Shared.IdType") = "ASIN"
            r1("IdType") = "ASIN"
            r1("AssociateTag") = AWSSellerID

            'r1["Service"] = "AWSECommerceService";
            'r1["Operation"] = "ItemLookup";
            'r1["ItemId"] = "B018FK66TU";
            'r1["ResponseGroup"] = "ItemAttributes,Offers,OfferFull,OfferListings";
            'r1["ItemLookup.Shared.IdType"] = "ASIN";
            'r1["IdType"] = "ASIN";
            'r1["AssociateTag"] = AWSSellerID;


            requestUrl = helper.Sign(r1)
            Dim urlx As [String] = ""
            Dim request As WebRequest = HttpWebRequest.Create(requestUrl)
            Dim response As WebResponse = request.GetResponse()

            Dim reader As New XmlTextReader(response.GetResponseStream())
            reader.WhitespaceHandling = WhitespaceHandling.None

            While reader.Read()
                Dim xxss As String = reader.Name

                If reader.Name = "ItemLookupResponse" OrElse reader.Name = "Items" OrElse reader.Name = "Item" OrElse reader.Name = "DetailPageURL" Then
                    If reader.LocalName = "DetailPageURL" Then
                        If reader.NodeType = XmlNodeType.Element AndAlso reader.LocalName = "DetailPageURL" AndAlso reader.IsStartElement() = True Then
                            _itemWEBUrL = ProcessRewardNode(reader, "DetailPageURL")
                        End If
                    End If
                Else
                    'if (reader.LocalName == "DetailPageURL")
                    '{
                    '    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "DetailPageURL" && reader.IsStartElement() == true)
                    '    {
                    '        urlx = ProcessRewardNode(reader, "DetailPageURL");
                    '    }
                    '}

                    If reader.Name = "ItemAttributes" OrElse reader.Name = "OfferSummary" Then
                        If reader.NodeType = XmlNodeType.Element AndAlso reader.LocalName = "ItemAttributes" AndAlso reader.IsStartElement() = True Then
                            _itemtitle = ProcessRewardNode(reader, "Title")
                        End If
                        If reader.NodeType = XmlNodeType.Element AndAlso reader.LocalName = "OfferSummary" AndAlso reader.IsStartElement() = True Then
                            Dim xx As String = ProcessRewardNode(reader, "LowestNewPrice")
                            Dim aa As [String]() = xx.Split("|"c)
                            If aa.Length = 2 Then
                                If aa(0) = "-1" OrElse aa(0) = "-2" Then
                                    _newprice = 0.0
                                Else
                                    _newprice = Convert.ToDouble(aa(0))
                                End If

                                If aa(1) = "-1" OrElse aa(1) = "-2" Then
                                    _usedprice = 0.0
                                Else
                                    _usedprice = Convert.ToDouble(aa(1))
                                End If
                            Else
                                _newprice = 0.0
                                _usedprice = 0.0
                            End If
                        End If

                        If reader.NodeType = XmlNodeType.Element AndAlso reader.LocalName = "Offers" AndAlso reader.IsStartElement() = True Then
                            Dim xx As String = ProcessRewardNode(reader, "Price")
                            If xx = "-1" OrElse xx = "-2" Then
                                _price = 0.0
                            Else
                                _price = Convert.ToDouble(xx)

                                ' reader.Close();
                            End If
                        End If
                        'reader.Skip();
                    Else
                    End If
                End If
            End While

            ExLog = requestUrl
            Return True
        Catch ex As Exception
            _price = 0.0
            _newprice = 0.0
            _usedprice = 0.0
            _itemtitle = ""
            ExLog = ex.Message.ToString() + vbLf + requestUrl
            Return False
        End Try
    End Function

    Public Function ProcessRewardNode(RewardReader As XmlTextReader, tagName As String) As [String]
        Try
            Dim RewardXmlDoc As New XmlDocument()
            RewardXmlDoc.LoadXml(RewardReader.ReadOuterXml())

            Dim nsmgr As New XmlNamespaceManager(RewardXmlDoc.NameTable)
            nsmgr.AddNamespace("rate", "http://webservices.amazon.com/AWSECommerceService/2011-08-01")

            Dim RetVal As String = ""
            Dim priceStr As String = ""

            If tagName = "LowestNewPrice" OrElse tagName = "LowestUsedPrice" Then
                Try
                    Dim nd As XmlNode = RewardXmlDoc.SelectSingleNode("//rate:LowestNewPrice", nsmgr)

                    If nd.ChildNodes.Count = 3 Then
                        priceStr = nd.ChildNodes(2).InnerText
                    Else
                        priceStr = "-2"
                    End If
                Catch ex As Exception
                    priceStr = "-1"
                End Try

                Try
                    Dim nd As XmlNode = RewardXmlDoc.SelectSingleNode("//rate:LowestUsedPrice", nsmgr)

                    If nd.ChildNodes.Count = 3 Then
                        priceStr = (priceStr & Convert.ToString("|")) + nd.ChildNodes(2).InnerText
                    Else
                        priceStr = priceStr & Convert.ToString("|-2")
                    End If
                Catch ex As Exception
                    priceStr = priceStr & Convert.ToString("|-1")

                End Try
            ElseIf tagName = "Title" Then
                RetVal = RewardXmlDoc.SelectSingleNode(Convert.ToString("//rate:") & tagName, nsmgr).InnerText
            ElseIf tagName = "Price" Then
                ' price
                Try
                    Dim nd As XmlNode = RewardXmlDoc.SelectSingleNode("//rate:Price", nsmgr)

                    If nd.ChildNodes.Count = 3 Then
                        RetVal = nd.ChildNodes(2).InnerText
                    Else
                        RetVal = "-2"
                    End If
                Catch ex As Exception
                    RetVal = "-1"
                End Try
            Else
                RetVal = RewardXmlDoc.SelectSingleNode(Convert.ToString("//rate:") & tagName, nsmgr).InnerText
            End If

            If tagName = "Title" OrElse tagName = "Price" Then
                Return RetVal.Replace("$", "")
            ElseIf tagName = "DetailPageURL" Then
                Return RetVal
            Else
                Return priceStr.Replace("$", "")
            End If
        Catch ex As Exception

            If tagName = "Title" OrElse tagName = "Price" Then
                Return "-1"
            Else
                Return "-1|-1"
            End If
        End Try
    End Function

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            Dim proid As Integer = gblProductID
            If objval.MsgConfirmYesNo("Are you sure you want to remove this item from monitoring?", "Confirm") = Windows.Forms.DialogResult.Yes Then
                Dim cmd As New SQLiteCommand
                cmd.CommandText = "delete from db_products where product_id=@product_id"
                cmd.Parameters.AddWithValue("@product_id", proid)
                If dbOperation.ExecuteQuery(cmd) > 0 Then
                    objval.MsgSuccess("Item Removed Successfully.", "Success")

                    Dim tempAsin As String = ""
                    tempAsin = gblASIN
                    Dim dvNext As New DataView(gblDTAllItem)
                    dvNext.RowFilter = "product_id < " & gblCurrID
                    dvNext.Sort = "product_id DESC"

                    Dim dt As DataTable = dvNext.ToTable()
                    If dt.Rows.Count > 0 Then
                        gblCurrID = objval.ToInt32(dt.Rows(0)("product_id"))
                        gblASIN = objval.ToString(dt.Rows(0)("product_asin"))
                        FillDATA()

                        Dim dvNext2 As New DataView(gblDTAllItem)
                        dvNext2.RowFilter = "product_id <> '" & tempAsin & "'"
                        dvNext2.Sort = "product_id ASC"
                        gblDTAllItem = dvNext2.ToTable
                    Else
                        objval.MsgWarning("No more previous record available", "No Data avaiable")
                    End If

                    'Me.Close()
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        If gblUPC = "" Then
            objval.MsgWarning("UPC code missing for current item", "Can not open link")
        Else
            Dim aa As String = "http://www.ebay.com/sch/i.html?LH_Complete=1&_nkw=" & gblUPC & "&rt=nc&LH_BIN=1"
            Try
                Process.Start(aa)
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        If gblUPC = "" Then
            objval.MsgWarning("UPC code missing for current item", "Can not open link")
        Else
            Dim aa As String = "http://www.ebay.com/sch/i.html?_nkw=" & gblUPC
            Try
                Process.Start(aa)
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked


        If gblAmazonURL = "" Then
            objval.MsgWarning("Scan is pending for this item.", "Can not open link")
        Else
            Try
                Process.Start(gblAmazonURL)
            Catch ex As Exception

            End Try
        End If
    End Sub


    Private Sub btnSyncKeepa_Click(sender As Object, e As EventArgs) Handles btnSyncKeepa.Click
        Try
            ShowWaitForm()
            gblDTNew = New DataTable
            gblDTNew.Columns.Add("idate", GetType(DateTime))
            gblDTNew.Columns.Add("iprice", GetType(Double))

            gblDTUsed = gblDTNew.Clone
            gblDTSaleRank = gblDTNew.Clone

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

            Dim _ProductASIN As String = gblASIN

            Dim URL As String = "https://api.keepa.com/product?key=" & GblKeepaAccessKey & "&domain=1&asin=" & _ProductASIN

            Dim streamText As String = ""
            Dim request As HttpWebRequest = CType(WebRequest.Create(URL), HttpWebRequest)
            request.AutomaticDecompression = DecompressionMethods.Deflate Or DecompressionMethods.GZip
            request.Method = "GET"

            Dim res As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)

            If res.StatusCode = HttpStatusCode.OK Then
                Dim receiveStream As Stream = res.GetResponseStream()
                Dim readStream As StreamReader = Nothing
                readStream = New StreamReader(receiveStream)
                streamText = readStream.ReadToEnd()
                res.Close()
                readStream.Close()
            Else
                CloseWaitForm()
                objval.MsgWarning("Failed to get Data from keepa.Server down may down.", "Fail")
                Exit Sub
            End If

            '  Dim search_response As String = System.IO.File.ReadAllText(Application.StartupPath & "\keepa.txt")
            Dim jObject As RootObject

            Try
                jObject = CType(Newtonsoft.Json.JsonConvert.DeserializeObject(streamText, GetType(RootObject)), RootObject)
            Catch ex As Exception
                CloseWaitForm()
                objval.MsgWarning("API response converstion failed.", "Fail 1.1")
                Exit Sub
            End Try

            Try


                For Each proObj As Product In jObject.products
                    Dim abc As Int64()() = proObj.csv
                    Dim aas As Int32 = abc(1).Length
                    ' 1 index is for New items
                    For index As Int32 = 0 To abc(1).Length - 1 Step 2
                        If objval.ToDouble(abc(1)(index + 1)) > 0 Then
                            gblDTNew.Rows.Add(KeepaToDateTime(abc(1)(index)), objval.ToDouble(abc(1)(index + 1)) / 100.0)
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
                Next
            Catch ex As Exception
                CloseWaitForm()
                objval.MsgWarning("API response converstion failed.", "Fail 1.1")
                Exit Sub
            End Try

            '' Update To DB

            _Used30Avg = GetKeepaAvg(gblDTUsed, 30, 0)
            _Used90Avg = GetKeepaAvg(gblDTUsed, 90, 0)
            _Used180Avg = GetKeepaAvg(gblDTUsed, 180, 0)
            _Used365Avg = GetKeepaAvg(gblDTUsed, 365, 0)
            _Used3YrAvg = GetKeepaAvg(gblDTUsed, 3, 3)
            _Used5YrAvg = GetKeepaAvg(gblDTUsed, 5, 3)

            _New30Avg = GetKeepaAvg(gblDTNew, 30, 0)
            _New90Avg = GetKeepaAvg(gblDTNew, 90, 0)
            _New180Avg = GetKeepaAvg(gblDTNew, 180, 0)
            _New365Avg = GetKeepaAvg(gblDTNew, 365, 0)
            _New3YrAvg = GetKeepaAvg(gblDTNew, 3, 3)
            _New5YrAvg = GetKeepaAvg(gblDTNew, 5, 3)

            _SaleR30Avg = GetKeepaRankAvg(gblDTSaleRank, 30, 0)
            _SaleR90Avg = GetKeepaRankAvg(gblDTSaleRank, 90, 0)
            _SaleR180Avg = GetKeepaRankAvg(gblDTSaleRank, 180, 0)
            _SaleR365Avg = GetKeepaRankAvg(gblDTSaleRank, 365, 0)
            _SaleR3YrAvg = GetKeepaRankAvg(gblDTSaleRank, 3, 3)

            Dim retVal As Int32 = 0
            Dim DtProChk As DataTable = dbOperation_keepa.GetDataTable("select * from keepa_price where product_asin='" & _ProductASIN & "'", retVal)
            If retVal = 1 Then
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

                FillKeepaGrid()
                CloseWaitForm()
                'objval.MsgWarning("Sych Successfully completed.", "Success")
            Else
                CloseWaitForm()
                objval.MsgWarning("Scan Fail Product Doesn't exists.", "Scan Failed")
            End If
        Catch ex As Exception
            CloseWaitForm()
            objval.MsgError("Some Error occured while Save data to Database.", "Sync Failed")
        End Try
    End Sub

#Region "Keepa Funcitons"

    Public Sub FillKeepaGrid()
        Try


            If gblASIN <> "" Then
                Dim retval As Int32 = 0
                Dim dt As DataTable = dbOperation_keepa.GetDataTable("select * from keepa_price where product_asin='" & gblASIN & "'", retval)
                If retval = 1 Then
                    If dt.Rows.Count > 0 Then

                        gblDTkeepa.Rows.Clear()
                        gblDTkeepa.Rows.Add("30  Days USED Avg", IIf(objval.ToDouble(dt.Rows(0)("day_30_used")) > 0, "$" & objval.ToString(dt.Rows(0)("day_30_used")), objval.ToString(dt.Rows(0)("day_30_used"))), "0")
                        gblDTkeepa.Rows.Add("90  Days USED Avg", IIf(objval.ToDouble(dt.Rows(0)("day_90_used")) > 0, "$" & objval.ToString(dt.Rows(0)("day_90_used")), objval.ToString(dt.Rows(0)("day_90_used"))), "0")
                        gblDTkeepa.Rows.Add("180 Days USED Avg", IIf(objval.ToDouble(dt.Rows(0)("day_180_used")) > 0, "$" & objval.ToString(dt.Rows(0)("day_180_used")), objval.ToString(dt.Rows(0)("day_180_used"))), "0")
                        gblDTkeepa.Rows.Add("365 Days USED Avg", IIf(objval.ToDouble(dt.Rows(0)("day_365_used")) > 0, "$" & objval.ToString(dt.Rows(0)("day_365_used")), objval.ToString(dt.Rows(0)("day_365_used"))), "0")
                        gblDTkeepa.Rows.Add("3yr Days USED Avg", IIf(objval.ToDouble(dt.Rows(0)("year_3_used")) > 0, "$" & objval.ToString(dt.Rows(0)("year_3_used")), objval.ToString(dt.Rows(0)("year_3_used"))), "0")
                        gblDTkeepa.Rows.Add("5yr Days USED Avg", IIf(objval.ToDouble(dt.Rows(0)("year_5_used")) > 0, "$" & objval.ToString(dt.Rows(0)("year_5_used")), objval.ToString(dt.Rows(0)("year_5_used"))), "0")
                        gblDTkeepa.Rows.Add("", "", "")
                        gblDTkeepa.Rows.Add("30  Days NEW Avg", IIf(objval.ToDouble(dt.Rows(0)("day_30_new")) > 0, "$" & objval.ToString(dt.Rows(0)("day_30_new")), objval.ToString(dt.Rows(0)("day_30_new"))), "1")
                        gblDTkeepa.Rows.Add("90  Days NEW Avg", IIf(objval.ToDouble(dt.Rows(0)("day_90_new")) > 0, "$" & objval.ToString(dt.Rows(0)("day_90_new")), objval.ToString(dt.Rows(0)("day_90_new"))), "1")
                        gblDTkeepa.Rows.Add("180 Days NEW Avg", IIf(objval.ToDouble(dt.Rows(0)("day_180_new")) > 0, "$" & objval.ToString(dt.Rows(0)("day_180_new")), objval.ToString(dt.Rows(0)("day_180_new"))), "1")
                        gblDTkeepa.Rows.Add("365 Days NEW Avg", IIf(objval.ToDouble(dt.Rows(0)("day_365_new")) > 0, "$" & objval.ToString(dt.Rows(0)("day_365_new")), objval.ToString(dt.Rows(0)("day_365_new"))), "1")
                        gblDTkeepa.Rows.Add("3yr Days NEW Avg", IIf(objval.ToDouble(dt.Rows(0)("year_3_new")) > 0, "$" & objval.ToString(dt.Rows(0)("year_3_new")), objval.ToString(dt.Rows(0)("year_3_new"))), "1")
                        gblDTkeepa.Rows.Add("5yr Days NEW Avg", IIf(objval.ToDouble(dt.Rows(0)("year_5_new")) > 0, "$" & objval.ToString(dt.Rows(0)("year_5_new")), objval.ToString(dt.Rows(0)("year_5_new"))), "1")
                        gblDTkeepa.Rows.Add("", "", "")
                        gblDTkeepa.Rows.Add("30  Days SaleRank Avg", objval.ToString(dt.Rows(0)("day_30_salerank")), "2")
                        gblDTkeepa.Rows.Add("90  Days SaleRank Avg", objval.ToString(dt.Rows(0)("day_90_salerank")), "2")
                        gblDTkeepa.Rows.Add("180 Days SaleRank Avg", objval.ToString(dt.Rows(0)("day_180_salerank")), "2")
                        gblDTkeepa.Rows.Add("365 Days SaleRank Avg", objval.ToString(dt.Rows(0)("day_365_salerank")), "2")
                        gblDTkeepa.Rows.Add("3yr Days SaleRank Avg", objval.ToString(dt.Rows(0)("year_3_salerank")), "2")
                        GridKeepa.DataSource = gblDTkeepa
                    Else
                        GridKeepa.DataSource = Nothing
                    End If
                End If
            End If
        Catch ex As Exception
            objval.MsgWarning("Error while fill data to Keepa Grid.", "Error")
        End Try
    End Sub

    Public Function GetKeepaAvg(ByVal dt As DataTable, ByVal days As Int32, ByVal deductType As Boolean) As Double
        Try
            Dim total As Double = 0
            Dim cnt As Double = 0

            If deductType = 0 Then ' BY DAY
                Dim aa As String = DateTime.Now.AddDays(-1 * days).ToString(System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern).ToString()
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

    Private Sub dgvKeepa_RowStyle(sender As Object, e As RowStyleEventArgs) Handles dgvKeepa.RowStyle
        Dim View As GridView = sender
        If (e.RowHandle >= 0) Then
            Dim typ As String = View.GetRowCellDisplayText(e.RowHandle, View.Columns("colcode"))
            If typ = "0" Then
                e.Appearance.BackColor = Color.FromArgb(254, 243, 243)
                e.Appearance.BackColor2 = Color.FromArgb(254, 243, 243)
            ElseIf typ = "1" Then
                e.Appearance.BackColor = Color.FromArgb(243, 254, 247)
                e.Appearance.BackColor2 = Color.FromArgb(243, 254, 247)
            ElseIf typ = "2" Then
                e.Appearance.BackColor = Color.FromArgb(250, 243, 254)
                e.Appearance.BackColor2 = Color.FromArgb(250, 243, 254)
            End If
        End If
    End Sub

#End Region

    Public Sub ShowWaitForm()

        If (Not SplashScreenManager1.IsSplashFormVisible = True) Then
            SplashScreenManager1.ShowWaitForm()
            SplashScreenManager1.SetWaitFormCaption("Please Wait")
            SplashScreenManager1.SetWaitFormDescription("Fetching Data from Keepa")
        End If

    End Sub

    Public Sub CloseWaitForm()
        If SplashScreenManager1.IsSplashFormVisible Then
            SplashScreenManager1.CloseWaitForm()
        End If
    End Sub

    Private Sub FrmItemDetail_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        ' SplitContainer.SplitterPosition = SplitContainer.Width / 2
    End Sub

    Private Sub FrmItemDetail_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged

        ' SplitContainer.SplitterPosition = (SplitContainer.Size.Width / 2)
    End Sub

    Private Sub WebBrowserCompleted_ProgressChanged(sender As Object, e As WebBrowserProgressChangedEventArgs) Handles WebBrowserCompleted.ProgressChanged
        ToolStripProgressBar2.Maximum = CInt(e.MaximumProgress)
        ToolStripProgressBar2.Value = If((CInt(e.CurrentProgress) < 0 OrElse CInt(e.MaximumProgress) < CInt(e.CurrentProgress)), CInt(e.MaximumProgress), CInt(e.CurrentProgress))
    End Sub

    Private Sub WebBrowserCompleted_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowserCompleted.DocumentCompleted
        ToolStripStatusLabel2.Text = "Done"
    End Sub

    Private Sub webpage_DocumentCompleted(ByVal sender As Object, ByVal e As WebBrowserDocumentCompletedEventArgs) Handles webBrowserCurrent.DocumentCompleted
        toolStripStatusLabel1.Text = "Done"
    End Sub

    Private Sub webpage_ProgressChanged(ByVal sender As Object, ByVal e As WebBrowserProgressChangedEventArgs) Handles webBrowserCurrent.ProgressChanged
        toolStripProgressBar1.Maximum = CInt(e.MaximumProgress)
        toolStripProgressBar1.Value = If((CInt(e.CurrentProgress) < 0 OrElse CInt(e.MaximumProgress) < CInt(e.CurrentProgress)), CInt(e.MaximumProgress), CInt(e.CurrentProgress))
    End Sub

    Private Sub btnEqual_Click(sender As Object, e As EventArgs) Handles btnEqual.Click, btnEqual2.Click
        SplitContainer.SplitterPosition = (SplitContainer.Size.Width / 2)
    End Sub

    Private Sub btnCurrListingFull_Click(sender As Object, e As EventArgs) Handles btnCurrListingFull.Click, btnComListingHide.Click
        SplitContainer.SplitterPosition = SplitContainer.Size.Width - 10
    End Sub

    Private Sub btnCurrListingHide_Click(sender As Object, e As EventArgs) Handles btnCurrListingHide.Click, btnComListingFull.Click
        SplitContainer.SplitterPosition = 10
    End Sub

    Private Sub btnNextitem_Click(sender As Object, e As EventArgs) Handles btnNextitem.Click
        Dim dvNext As New DataView(gblDTAllItem)
        dvNext.RowFilter = "product_id > " & gblCurrID
        dvNext.Sort = "product_id ASC"

        Dim dt As DataTable = dvNext.ToTable()
        If dt.Rows.Count > 0 Then
            gblCurrID = objval.ToInt32(dt.Rows(0)("product_id"))
            gblASIN = objval.ToString(dt.Rows(0)("product_asin"))
            FillDATA()
        Else
            objval.MsgWarning("No more next record available", "No Data avaiable")
        End If

    End Sub

    Private Sub btnPreviouositem_Click(sender As Object, e As EventArgs) Handles btnPreviouositem.Click
        Dim dvNext As New DataView(gblDTAllItem)
        dvNext.RowFilter = "product_id < " & gblCurrID
        dvNext.Sort = "product_id DESC"

        Dim dt As DataTable = dvNext.ToTable()
        If dt.Rows.Count > 0 Then
            gblCurrID = objval.ToInt32(dt.Rows(0)("product_id"))
            gblASIN = objval.ToString(dt.Rows(0)("product_asin"))
            FillDATA()
        Else
            objval.MsgWarning("No more previous record available", "No Data avaiable")
        End If
    End Sub

    Private Sub lnkKeepa_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkKeepa.LinkClicked
        Try
            Process.Start("https://keepa.com/#!product/1-" & gblASIN)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub LinkLabel4_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        Try
            Process.Start("https://www.amazon.com/gp/offer-listing/" & gblASIN & "/")
        Catch ex As Exception

        End Try
    End Sub
End Class