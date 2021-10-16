Imports DevExpress.Skins
Imports System.IO
Imports System.Data.OleDb
Imports System.Data.SQLite
Imports System.Xml
Imports DevExpress.XtraGrid.Views.Grid
Imports System.Globalization
Imports System.Text
Imports System.Net
Imports System.Threading
Imports System.Net.Mail
Imports DevExpress.XtraSplashScreen
Imports System.Threading.Tasks
Imports DevExpress.XtraGrid.Columns
Imports AmazonMWS1
Imports IOEx

'Public Class RetLowestPrice

'    Public Property asin_key As String

'    Public Property new_price As Double

'    Public Property used_price As Double
'End Class

Public Class MainRibbon
    Dim objval As New Oozee
    Dim gblisPausemode As Boolean = False
    Dim gblisPauseSTATUS As String = "pause"
    Dim gblisPauseLASTiNDEX As Int32 = 0
    Dim glbXlsTotal As Integer = 0
    Dim glbXlsUploaded As Integer = 0
    Dim glbXlsUploadedError As Boolean = False
    Dim glbXlsDT As DataTable
    Dim glbFinalGridDT As DataTable
    Dim glbProcessDT As DataTable
    Dim glbMailDT As DataTable
    Dim glbEbayDT As DataTable
    Dim glbErroOnProcess As Boolean = False
    Dim _totalProcessCnt As Integer = 0
    Dim _totalProcessTotalMWSInfoCnt As Integer = 0
    Dim _totalProcessSuccMWSinfo As Integer = 0
    Dim _totalProcessSucc As Integer = 0
    Dim gblIsprocessByManual As Boolean = False
    Public DBconnStr As String = "Data Source=|DataDirectory|\awsdb.db; Version=3;"
    Public DBconnStrKeepa As String = "Data Source=|DataDirectory|\keepadb.db; Version=3;"

    Dim glbErroLog As New StringBuilder
    Dim DESTINATION As String = "webservices.amazon.com"
    Dim NAMESPACEx As String = "http://webservices.amazon.com/AWSECommerceService/2009-03-31"

    Dim gblISWrongSettig As Boolean = False
    Dim gblHashMailSent As New Hashtable
    Dim oxml As String
    Dim nxml As String
    Dim EAN_oxml As String
    Dim EAN_nxml As String
    Dim key_nxml As String
    Dim key_oxml As String
    Dim request As System.Net.HttpWebRequest
    Dim selectionStock As New DevExpressGrid()
    Dim TotRC As Int64 = 0
    Dim HDtasks As New List(Of Task(Of Long))()
    Dim columnfilepath As String = Application.StartupPath & "\columnsetting.xml"
    Dim glbNextSchedule As DateTime = Date.Now

    Public Sub New()
        'Dim obj = New ozlib.FrmSplash(My.Resources.toolLogo, 2075511, "AWSTool.exe", "1.0.1")
        'obj.ShowDialog()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Visual Studio 2013 Light")
        'Metropolis,Sharp,'Office 2010 Silver
        InitializeComponent()
    End Sub

    Private Sub MainRibbon_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        mnuMWSScan.Checked = True
        selectionStock = New DevExpressGrid(dgvGrid, True)
        selectionStock.CheckMarkColumn.VisibleIndex = 0
        selectionStock.CheckMarkColumn.AbsoluteIndex = 0


        glbEbayDT = New DataTable
        glbEbayDT.Columns.Add("price", GetType(Double))
        glbEbayDT.Columns.Add("state", GetType(String))


        glbMailDT = New DataTable
        glbMailDT.Columns.Add("asin", GetType(String))
        glbMailDT.Columns.Add("title", GetType(String))
        glbMailDT.Columns.Add("newprice", GetType(Double))
        glbMailDT.Columns.Add("newprice_eb", GetType(Double))
        glbMailDT.Columns.Add("Tnewprice", GetType(Double))
        glbMailDT.Columns.Add("isNpriceHigh", GetType(Boolean))
        glbMailDT.Columns.Add("usedprice", GetType(Double))
        glbMailDT.Columns.Add("usedprice_eb", GetType(Double))
        glbMailDT.Columns.Add("Tusedprice", GetType(Double))
        glbMailDT.Columns.Add("isUpriceHigh", GetType(Boolean))
        glbMailDT.Columns.Add("link", GetType(String))
        glbMailDT.Columns.Add("notes", GetType(String))
        glbMailDT.Columns.Add("upc", GetType(String))
        gblHashMailSent.Clear()


        LoadGridColumnSetting()



        dbOperation.FillGloabalSetting()
        autoTimer.Interval = GblRefMinu * 60000
        ReloadGrid()
        ScheduleTimer()
        '   CompletedsITEM("037429200223", "B0002JELNQ")
    End Sub

    'Private Sub MainMdi_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
    '    Application.Exit()
    'End Sub

    'Private Sub MainMdi_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
    '    Dim res As DialogResult = objval.MsgConfirmYesNo("Do you want to Logout?", "Confirmation")
    '    If res = System.Windows.Forms.DialogResult.Yes Then
    '        Application.Exit()
    '    Else
    '        e.Cancel = True
    '    End If
    'End Sub

    Private Sub mnuLogout_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnuLogout.ItemClick
        If GblIsProcessStart = True Then
            objval.MsgWarning("Please stop Monitoring process first.", "Access Denied")
            Exit Sub
        End If

        Dim res As DialogResult = objval.MsgConfirmYesNo("Do you want to Logout?", "Confirmation")
        If res = System.Windows.Forms.DialogResult.Yes Then
            'End
            'Else
            Application.Exit()
        End If
    End Sub

    Private Sub mnuSetting_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnuSetting.ItemClick
        If GblIsProcessStart = True Then
            objval.MsgWarning("Please stop Monitoring process first.", "Access Denied")
            Exit Sub
        End If

        Dim frmobj As New frmSeting
        frmobj.ShowDialog()
        dbOperation.FillGloabalSetting()
        autoTimer.Interval = GblRefMinu * 60000

        ScheduleTimer()
    End Sub

    Private Sub mnuStart_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnuStart.ItemClick

        If mnuAWSScan.Checked = True And GblAwsSellerID = "" Then
            objval.MsgWarning("AWS Setting is not ready. Please complete settings in setting menu", "Warning")
            Exit Sub
        ElseIf mnuMWSScan.Checked = True And GblMWSSellerID = "" Then
            objval.MsgWarning("MWS Setting is not ready. Please complete settings in setting menu", "Warning")
            Exit Sub
        End If


        gblisPauseLASTiNDEX = 0
        gblHashMailSent.Clear()
        'chk_Target_Price.Checked = True
        'chk_TargetUsedPrice.Checked = True
        mnuStart.Enabled = False
        mnuStop.Enabled = True
        mnuPause.Enabled = True
        mnuAWSScan.Enabled = False
        mnuMWSScan.Enabled = False
        mnuUpdate.Enabled = False
        GblIsProcessStart = True
        glbErroOnProcess = False
        lblProcessType.Text = "Updateing Price (Auto)"
        autoTimer_Tick(Nothing, Nothing)
        autoTimer.Start()
    End Sub

    Private Sub mnuStop_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnuStop.ItemClick

        mnuStop.Enabled = False
        mnuStart.Enabled = True
        mnuUpdate.Enabled = True
        mnuPause.Enabled = False
        GblIsProcessStart = False
        gblisPausemode = False
        gblisPauseLASTiNDEX = 0
        gblisPauseSTATUS = "pause"
        mnuPause.ImageOptions.LargeImage = My.Resources.pause_button
        mnuPause.Caption = "Pause Monitoring"
        picPaused.Visible = False
        mnuPause.Enabled = False
        gblIsprocessByManual = False
        mnuAWSScan.Enabled = True
        mnuMWSScan.Enabled = True
        autoTimer.Stop()
    End Sub

    Private Sub mnuImportExcel_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnuImportExcel.ItemClick
        If gblisPausemode = True Then
            objval.MsgWarning("Import not allowed dusing 'Pause Monitoring'." & vbCrLf & "Please stop monitoring and try again.", "Access Denied")
            Exit Sub
        End If
        'If GblIsProcessStart = True Then
        '    objval.MsgWarning("Please stop Monitoring process first.", "Access Denied")
        '    Exit Sub
        'End If

        Dim frmobj As New FrmImport
        frmobj.ShowDialog()
        ReloadGrid()

    End Sub

    Private Sub autoTimer_Tick(sender As Object, e As EventArgs) Handles autoTimer.Tick
        'If mnuAWSScan.Checked = True Then
        '    If HDbackWork.IsBusy = False Then
        '        pnlUpdating.Visible = True
        '        HDbackWork.RunWorkerAsync()
        '    End If
        'Else
        If MWSBackWork.IsBusy = False And gblisPausemode = False Then
            pnlUpdating.Visible = True
            MWSBackWork.RunWorkerAsync()
        End If
        'End If
    End Sub

    Private Sub HDBackImportXLS_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles HDBackImportXLS.DoWork
        HDBackImportXLS.ReportProgress(1)
        Dim dt As DataTable = Nothing

        glbXlsUploadedError = False
        glbXlsUploaded = 0

        Try
            Dim strExLog As Integer = 0
            Dim dtItems As DataTable = dbOperation.GetDataTable("select product_asin from db_products", strExLog)

            Dim QryStr As New StringBuilder
            QryStr.AppendLine("BEGIN;")

            For Each dr As DataRow In glbXlsDT.Rows
                If Convert.ToString(dr("ASIN")).Trim.Replace("'", "").Trim.Length = 0 Then
                    Continue For
                End If
                If dtItems.Select("product_asin='" & Convert.ToString(dr("ASIN")).Trim.Replace("'", "") & "'").Length > 0 Then
                    If glbXlsDT.Columns.Contains("notes") Then
                        QryStr.AppendLine("update db_products set target_price=" & Math.Round(objval.ToDouble(dr("Price")), 2) & ",target_used_price=" & Math.Round(objval.ToDouble(dr("UsedPrice")), 2) & ",notes='" & Convert.ToString(dr("notes")).Trim.Replace("'", "") & "' where product_asin='" & Convert.ToString(dr("ASIN")).Trim.Replace("'", "") & "';")
                    Else
                        QryStr.AppendLine("update db_products set target_price=" & Math.Round(objval.ToDouble(dr("Price")), 2) & ",target_used_price=" & Math.Round(objval.ToDouble(dr("UsedPrice")), 2) & " where product_asin='" & Convert.ToString(dr("ASIN")).Trim.Replace("'", "") & "';")
                    End If

                Else
                    If glbXlsDT.Columns.Contains("notes") Then
                        QryStr.AppendLine("insert into db_products(product_asin,target_price,target_used_price,notes) values ('" & Convert.ToString(dr("ASIN")).Trim.Replace("'", "") & "'," & Math.Round(objval.ToDouble(dr("Price")), 2) & "," & Math.Round(objval.ToDouble(dr("UsedPrice")), 2) & "," & objval.ToString(dr("notes")).Replace("'", "") & ");")
                    Else
                        QryStr.AppendLine("insert into db_products(product_asin,target_price,target_used_price,notes) values ('" & Convert.ToString(dr("ASIN")).Trim.Replace("'", "") & "'," & Math.Round(objval.ToDouble(dr("Price")), 2) & "," & Math.Round(objval.ToDouble(dr("UsedPrice")), 2) & ",'');")
                    End If

                End If
            Next
            QryStr.AppendLine("COMMIT;")
            If File.Exists(Application.StartupPath & "\import.sql") Then
                File.Delete(Application.StartupPath & "\import.sql")
            End If

            Using sw As StreamWriter = File.CreateText(Application.StartupPath & "\import.sql")
                sw.Write(Convert.ToString(QryStr))
            End Using

            If File.Exists(Application.StartupPath & "\import.bat") Then
                File.Delete(Application.StartupPath & "\import.bat")
            End If

            Using sw As StreamWriter = File.CreateText(Application.StartupPath & "\import.bat")
                sw.Write("cd " & Application.StartupPath & vbCrLf & "sqlite3 awsdb.db < import.sql")
            End Using

            Dim proc As New Process()
            proc.StartInfo.FileName = "import.bat"
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            proc.Start()
            System.Threading.Thread.Sleep(1000)
        Catch ex As Exception
            glbXlsUploadedError = True
        End Try

    End Sub

    Private Sub HDBackImportXLS_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles HDBackImportXLS.RunWorkerCompleted
        pnlLoadingImport.Visible = False
        If glbXlsUploadedError = True Then
            objval.MsgError("Some Error occured while Import file from excel." & vbCrLf & "Please check your excel > ASIN,Price,Usedprice columns are compulsory and notes remark which is optional.", "Error On Import")
        Else
            objval.MsgSuccess("Data import from Excel Successfully completed." & vbCrLf & "Total Items : " & glbXlsTotal, "Import Summary")
            ReloadGrid()
        End If

    End Sub

    Public Sub ReloadGrid()
        ShowWaitForm()

        Dim qry As String = "select *,(case when target_price>0 and live_newprice>0 then ((live_newprice/target_price)*100)-100  else 0.00 end) as new_per,(case when target_used_price>0 and live_usedprice>0 then ((live_usedprice/target_used_price)*100)-100  else 0.00 end) as used_per,(case when target_price>0 and ebay_live_new_price>0 then ((ebay_live_new_price/target_price)*100)-100  else 0.00 end) as new_per_eb,(case when target_used_price>0 and ebay_live_used_price>0 then ((ebay_live_used_price/target_used_price)*100)-100  else 0.00 end) as used_per_eb,is_active  from db_products "
        Dim itemActiveFilter As String = ""
        If cmbFilter.SelectedIndex = 1 Then
            itemActiveFilter = " and is_active='Yes'"
        ElseIf cmbFilter.SelectedIndex = 2 Then
            itemActiveFilter = " and is_active='No'"
        End If

        If chk_Target_Price.Checked = True And chk_TargetUsedPrice.Checked = True Then

            qry = qry + " where  ((target_price > live_price and live_price > 0)  or  (target_price>live_newprice and live_newprice > 0 ) )  "
            qry = qry + " or ( live_usedprice > 0 and  target_used_price > live_usedprice) " & itemActiveFilter
        ElseIf chk_Target_Price.Checked = True Then
            qry = qry + " where  (target_price > live_price and live_price > 0)  or  (target_price>live_newprice and live_newprice > 0 )   " & itemActiveFilter
        ElseIf chk_TargetUsedPrice.Checked = True Then
            qry = qry + " where  live_usedprice > 0 and  target_used_price > live_usedprice " & itemActiveFilter
        ElseIf itemActiveFilter <> "" Then
            qry = qry + " where " & itemActiveFilter.Replace("and", "")
        End If


        Dim exLog As Integer = 0
        glbFinalGridDT = dbOperation.GetDataTable(qry, exLog)

        'Dim glbKeepaDT As DataTable = dbOperation_keepa.GetDataTable("select product_asin,day_30_new,day_90_new,day_180_new,day_365_new,year_3_new,year_5_new,day_30_used,day_90_used,day_180_used,day_365_used,year_3_used, " &
        '                     " year_5_used,day_30_salerank,day_90_salerank,day_180_salerank,day_365_salerank,year_3_salerank from keepa_price", exLog)


        'Dim dtGrid As New DataTable

        'Dim aa As New StringBuilder
        'For Each dc As DataColumn In glbFinalGridDT.Columns
        '    dtGrid.Columns.Add(dc.Caption, dc.DataType)
        'Next

        'For Each dc2 As DataColumn In glbKeepaDT.Columns
        '    If dc2.Caption <> "product_asin" Then
        '        aa.AppendLine("DrIns(""" & dc2.Caption & """) =  objval.ToDouble(dtemp.Rows(0)(""" & dc2.Caption & """))")
        '        dtGrid.Columns.Add(dc2.Caption, dc2.DataType)
        '    End If

        'Next

        'Dim DrIns As DataRow
        'For Each dr As DataRow In glbFinalGridDT.Rows
        '    DrIns = dtGrid.NewRow()
        '    DrIns("product_id") = dr("product_id")
        '    DrIns("product_name") = dr("product_name")
        '    DrIns("product_asin") = dr("product_asin")
        '    DrIns("product_category") = dr("product_category")
        '    DrIns("target_price") = dr("target_price")
        '    DrIns("live_price") = dr("live_price")
        '    DrIns("live_newprice") = dr("live_newprice")
        '    DrIns("live_usedprice") = dr("live_usedprice")
        '    DrIns("last_updated") = dr("last_updated")
        '    DrIns("target_used_price") = dr("target_used_price")
        '    DrIns("trade_in_price") = dr("trade_in_price")
        '    DrIns("product_url") = dr("product_url")
        '    DrIns("notes") = dr("notes")
        '    DrIns("upc") = dr("upc")
        '    DrIns("rank") = dr("rank")
        '    DrIns("ebay_live_new_price") = dr("ebay_live_new_price")
        '    DrIns("ebay_live_used_price") = dr("ebay_live_used_price")
        '    DrIns("ebay_new_comprice_avg") = dr("ebay_new_comprice_avg")
        '    DrIns("ebay_used_comprice_avg") = dr("ebay_used_comprice_avg")
        '    DrIns("ebay_new_comprice_per") = dr("ebay_new_comprice_per")
        '    DrIns("ebay_used_comprice_per") = dr("ebay_used_comprice_per")
        '    DrIns("live_newprice_pre") = dr("live_newprice_pre")
        '    DrIns("live_usedprice_pre") = dr("live_usedprice_pre")
        '    DrIns("num_of_pages") = dr("num_of_pages")
        '    DrIns("num_of_disc") = dr("num_of_disc")
        '    DrIns("num_of_review") = dr("num_of_review")
        '    DrIns("ebay_live_new_price_multi") = dr("ebay_live_new_price_multi")
        '    DrIns("ebay_live_used_price_multi") = dr("ebay_live_used_price_multi")
        '    DrIns("ebay_sold_new_price_high_multi") = dr("ebay_sold_new_price_high_multi")
        '    DrIns("ebay_sold_used_price_high_multi") = dr("ebay_sold_used_price_high_multi")
        '    DrIns("ebay_sold_new_price_lowest_multi") = dr("ebay_sold_new_price_lowest_multi")
        '    DrIns("ebay_sold_used_price_lowest_multi") = dr("ebay_sold_used_price_lowest_multi")
        '    DrIns("new_per") = dr("new_per")
        '    DrIns("used_per") = dr("used_per")
        '    DrIns("new_per_eb") = dr("new_per_eb")
        '    DrIns("used_per_eb") = dr("used_per_eb")

        '    Dim dv As New DataView(glbKeepaDT)
        '    dv.RowFilter = "product_asin = '" & Convert.ToString(dr("product_asin")) & "'"
        '    Dim dtemp As DataTable = dv.ToTable()
        '    If dtemp.Rows.Count = 1 Then
        '        DrIns("day_30_new") = objval.ToDouble(dtemp.Rows(0)("day_30_new"))
        '        DrIns("day_90_new") = objval.ToDouble(dtemp.Rows(0)("day_90_new"))
        '        DrIns("day_180_new") = objval.ToDouble(dtemp.Rows(0)("day_180_new"))
        '        DrIns("day_365_new") = objval.ToDouble(dtemp.Rows(0)("day_365_new"))
        '        DrIns("year_3_new") = objval.ToDouble(dtemp.Rows(0)("year_3_new"))
        '        DrIns("year_5_new") = objval.ToDouble(dtemp.Rows(0)("year_5_new"))
        '        DrIns("day_30_used") = objval.ToDouble(dtemp.Rows(0)("day_30_used"))
        '        DrIns("day_90_used") = objval.ToDouble(dtemp.Rows(0)("day_90_used"))
        '        DrIns("day_180_used") = objval.ToDouble(dtemp.Rows(0)("day_180_used"))
        '        DrIns("day_365_used") = objval.ToDouble(dtemp.Rows(0)("day_365_used"))
        '        DrIns("year_3_used") = objval.ToDouble(dtemp.Rows(0)("year_3_used"))
        '        DrIns("year_5_used") = objval.ToDouble(dtemp.Rows(0)("year_5_used"))
        '        DrIns("day_30_salerank") = objval.ToInt64(dtemp.Rows(0)("day_30_salerank"))
        '        DrIns("day_90_salerank") = objval.ToInt64(dtemp.Rows(0)("day_90_salerank"))
        '        DrIns("day_180_salerank") = objval.ToInt64(dtemp.Rows(0)("day_180_salerank"))
        '        DrIns("day_365_salerank") = objval.ToInt64(dtemp.Rows(0)("day_365_salerank"))
        '        DrIns("year_3_salerank") = objval.ToInt64(dtemp.Rows(0)("year_3_salerank"))
        '    End If


        '    dtGrid.Rows.Add(DrIns)
        'Next

        If exLog = 1 Then
            If Not glbFinalGridDT Is Nothing Then

                ConvertColumnType(glbFinalGridDT, "is_watch", GetType(Boolean))
                'If glbFinalGridDT.Columns.Contains("is_watch") Then
                '    glbFinalGridDT.Columns("is_watch").DataType = GetType(Boolean)
                'End If
            End If
            GridConn.DataSource = glbFinalGridDT
            lblTotalProduct.Text = glbFinalGridDT.Rows.Count
        Else
            lblTotalProduct.Text = "0"
        End If
        CloseWaitForm()
    End Sub

    Public Shared Sub ConvertColumnType(ByVal dt As DataTable, ByVal columnName As String, ByVal newType As Type)
        Using dc As DataColumn = New DataColumn(columnName & "_new", newType)
            Dim ordinal As Integer = dt.Columns(columnName).Ordinal
            dt.Columns.Add(dc)
            dc.SetOrdinal(ordinal)

            For Each dr As DataRow In dt.Rows
                dr(dc.ColumnName) = Convert.ChangeType(dr(columnName), newType)
            Next

            dt.Columns.Remove(columnName)
            dc.ColumnName = columnName
        End Using
    End Sub

    Private Sub _DeleteBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles repoGridRemoveBtn.Click
        Try
            Dim proid As Integer = objval.ToInt32(dgvGrid.GetRowCellValue(dgvGrid.FocusedRowHandle, "product_id"))
            If objval.MsgConfirmYesNo("Are you sure you want to remove this item from monitoring?", "Confirm") = Windows.Forms.DialogResult.Yes Then
                Dim cmd As New SQLiteCommand
                cmd.CommandText = "delete from db_products where product_id=@product_id"
                cmd.Parameters.AddWithValue("@product_id", proid)
                If dbOperation.ExecuteQuery(cmd) > 0 Then
                    _totalProcessCnt = _totalProcessCnt - 1
                    objval.MsgSuccess("Item Removed Successfully.", "Success")
                    ReloadGrid()
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub _URLBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles repoItemURL.Click
        Try
            Dim proid As String = objval.ToString(dgvGrid.GetRowCellValue(dgvGrid.FocusedRowHandle, "product_url"))
            If proid.Length > 0 Then
                Process.Start(proid)
            Else
                objval.MsgWarning("Item URL does not exists.", "No URL Found")
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnAtoFit_Click(sender As Object, e As EventArgs) Handles btnAtoFit.Click
        dgvGrid.BestFitColumns()
    End Sub

    Private Sub btnRefreshGrid_Click(sender As Object, e As EventArgs) Handles btnRefreshGrid.Click
        ReloadGrid()
    End Sub

    Private Sub btnDeleteALL_Click(sender As Object, e As EventArgs) Handles btnDeleteALL.Click
        'If GblIsProcessStart = True Then
        '    objval.MsgWarning("Please stop Monitoring process first.", "Access Denied")
        '    Exit Sub
        'End If
        Dim arr As String() = GetStockSelection()
        Dim itemsStr As String = String.Join(",", arr)
        If itemsStr.Trim.Length = 0 Then
            objval.MsgWarning("Please Select item to remove from monitoring.", "Selection Required")
            Exit Sub
        End If

        If objval.MsgConfirmYesNo("Are you sure you want to remove ALL item from monitoring?", "Confirm") = Windows.Forms.DialogResult.Yes Then

            Dim cmd As New SQLiteCommand
            cmd.CommandText = "delete from db_products where product_id in (" & itemsStr & ")"
            If dbOperation.ExecuteQuery(cmd) > 0 Then
                objval.MsgSuccess("ALL Item removed from monitoring Successfully.", "Success")
                _totalProcessCnt = _totalProcessCnt - arr.Length
                ReloadGrid()
                selectionStock.ClearSelection()
            End If
        End If


        ''''''' FOR DELETE ALL DATA
        'If GblIsProcessStart = True Then
        '    objval.MsgWarning("Please stop Monitoring process first.", "Access Denied")
        '    Exit Sub
        'End If

        'If objval.MsgConfirmYesNo("Are you sure you want to remove ALL item from monitoring?", "Confirm") = Windows.Forms.DialogResult.Yes Then
        '    Dim cmd As New SQLiteCommand
        '    cmd.CommandText = "delete from db_products"
        '    If dbOperation.ExecuteQuery(cmd) > 0 Then
        '        objval.MsgSuccess("ALL Item removed from monitoring Successfully.", "Success")
        '        ReloadGrid()
        '    End If
        'End If

    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click

        Dim svDialog As New SaveFileDialog()

        svDialog.DefaultExt = "xls"
        svDialog.Title = "Export to Excel"
        svDialog.FileName = "Amzone_Result_" & DateTime.Now.ToString("ddMMyyyy-hhmmsstt") & ".xls"
        svDialog.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*"
        svDialog.ShowDialog()

        If svDialog.FileName <> "" Then
            dgvGrid.ExportToXls(svDialog.FileName)
            If objval.MsgConfirmYesNo("Excel file Exported successfully." & vbCrLf & "Are you want to Open Excel file after export?", "Export successful") = Windows.Forms.DialogResult.Yes Then
                Process.Start(svDialog.FileName)
            End If
        End If
    End Sub

    Private Sub RunProcess(ByVal asin As String, ByVal newPrice As Double, ByVal UsedPrice As Double, ByVal newPrice_eb As Double, ByVal usedPrice_eb As Double, ByVal title As String, ByVal url As String, ByVal upc As String)
        If InvokeRequired Then
            BeginInvoke(New MethodInvoker(Sub() Me.RunProcess(asin, newPrice, UsedPrice, newPrice_eb, usedPrice_eb, title, url, upc)))
        Else

            Dim dr() As DataRow = glbProcessDT.Select("product_asin='" & asin & "'")
            If dr.Length > 0 Then
                Dim Nprice As String = ""
                Dim Uprice As String = ""
                Dim ebNprice As String = ""
                Dim ebUprice As String = ""
                Dim showAlert As Boolean = False
                Dim ebShowAlert As Boolean = False
                Dim isNewhighlight As Boolean = False
                Dim isUsedhighlight As Boolean = False

                'If objval.ToDouble(dr("live_price")) > 0 AndAlso objval.ToDouble(dr("live_price")) < objval.ToDouble(dr("target_price")) Then
                '    Nprice = vbCrLf & "Price: <color=#d41111>$" & objval.ToDouble(dr("live_price")) & "</color>  T: <color=#095077>$" & objval.ToDouble(dr("target_price")) & "</color>"
                '    showAlert = True
                'End If
                'If newPrice_eb > 0 AndAlso newPrice_eb < objval.ToDouble(dr(0).ItemArray(1)) Then
                '    ebNprice = vbCrLf & "NEW Eb. Price: <color=#d41111>$" & newPrice_eb & "</color>  T: <color=#095077>$" & objval.ToDouble(dr(0).ItemArray(1)) & "</color>"

                '    ebShowAlert = True
                '    isNewhighlight = True

                'ElseIf newPrice_eb > 0 Then
                '    ebNprice = vbCrLf & "NEW Eb.Price: $" & newPrice_eb & "  T: $" & objval.ToDouble(dr(0).ItemArray(1))

                'End If

                'If usedPrice_eb > 0 AndAlso usedPrice_eb < objval.ToDouble(dr(0).ItemArray(2)) Then
                '    ebUprice = vbCrLf & "Used Eb.Price: <color=#d41111>$" & usedPrice_eb & "</color>  T: <color=#095077>$" & objval.ToDouble(dr(0).ItemArray(2)) & "</color>"
                '    ebShowAlert = True
                '    isUsedhighlight = True
                'ElseIf usedPrice_eb > 0 Then
                '    ebUprice = vbCrLf & "Used Eb.Price: $" & usedPrice_eb & " T: $" & objval.ToDouble(dr(0).ItemArray(2))
                'End If


                If newPrice > 0 AndAlso newPrice < objval.ToDouble(dr(0).ItemArray(1)) Then
                    Nprice = vbCrLf & "NEW Am. Price: <color=#d41111>$" & newPrice & "</color>  T: <color=#095077>$" & objval.ToDouble(dr(0).ItemArray(1)) & "</color>"
                    showAlert = True
                    isNewhighlight = True
                ElseIf newPrice > 0 Then
                    Nprice = vbCrLf & "NEW Am.Price: $" & newPrice & "  T: $" & objval.ToDouble(dr(0).ItemArray(1))
                End If


                If UsedPrice > 0 AndAlso UsedPrice < objval.ToDouble(dr(0).ItemArray(2)) Then
                    Uprice = vbCrLf & "Used Am.Price: <color=#d41111>$" & UsedPrice & "</color>  T: <color=#095077>$" & objval.ToDouble(dr(0).ItemArray(2)) & "</color>"
                    showAlert = True
                    isUsedhighlight = True
                ElseIf UsedPrice > 0 Then
                    Uprice = vbCrLf & "Used Am.Price: $" & UsedPrice & " T: $" & objval.ToDouble(dr(0).ItemArray(2))
                End If

                'select product_asin,target_price,target_used_price,upc,notes from db_products
                'If ebShowAlert = True Then
                '    hdAlert.Show(Me, "<b>" & title & "</b>", asin & ebNprice & ebUprice)
                'End If

                If showAlert = True Then
                    If GblShowNotification = True Then
                        hdAlert.Show(Me, "<b>" & title & "</b>", asin & Nprice & Uprice)
                    End If

                    ' Task.Run(Sub() CompletedsITEM(upc, asin))

                    'HDtasks(HDtasks.Count + 1) = Task.Factory.StartNew(Function() Me.CompletedsITEM(upc, asin))

                    'uncomment after work''''''''''''''''''''''''''''''  
                    If upc.Trim <> "" AndAlso GblEnabledEbay = True Then
                        HDtasks.Add(Task.Factory.StartNew(Function() Me.CompletedsITEM(upc, asin)))
                    End If


                    'Dim tts As Task = Task.Factory.StartNew(Sub() Me.CompletedsITEM(upc, asin))
                    'tts.Start()


                    'Dim threadEbay As Thread
                    'threadEbay = New Thread(Sub() Me.CompletedsITEM(upc, asin))
                    'threadEbay.IsBackground = True
                    'threadEbay.Start()
                    'threadEbay.Join()

                    If gblHashMailSent.ContainsKey(asin) = False Then
                        'glbErroLog.AppendLine("Added to hashtable :" & asin)
                        Dim axxx As String = objval.ToString(dr(0).ItemArray(3))
                        glbMailDT.Rows.Add(asin, title, newPrice, newPrice_eb, objval.ToDouble(dr(0).ItemArray(1)), isNewhighlight, UsedPrice, usedPrice_eb, objval.ToDouble(dr(0).ItemArray(2)), isUsedhighlight, url, objval.ToString(dr(0).ItemArray(4)), objval.ToString(dr(0).ItemArray(3)))
                        gblHashMailSent.Add(asin, DateTime.Now.ToString("ddMMyyyyhhmmsstt"))
                    Else
                        'glbErroLog.AppendLine("Key Exists to hashtable :" & asin)
                    End If
                End If
            End If
        End If
    End Sub

    Public Function CompletedsITEM(ByVal UPCcode As String, ByVal ASIN As String) As Int64
        'If ASIN <> "B008PDAF7C" Then
        '    Return 0
        'End If

        'If InvokeRequired Then
        '    BeginInvoke(New MethodInvoker(Sub() Me.CompletedsITEM(UPCcode, ASIN)))
        'Else

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
            glbErroLog.AppendLine("Exception in Complete NEW LOWEST PRICE FETCH " & vbCrLf & ex.Message.ToString())
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
            glbErroLog.AppendLine("Exception in Comlete List NEW " & vbCrLf & ex.Message.ToString())
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
            glbErroLog.AppendLine("Exception in USED LOWEST EBAY FETCH " & vbCrLf & ex.Message.ToString())
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
            glbErroLog.AppendLine("Exception in Comlete List NEW " & vbCrLf & ex.Message.ToString())
            _UsedAVGprice = 0
            _UsedAVGPercent = 0
        End Try

        '' UPDATE INTO DATABASE

        Try
            Dim con As New SQLiteConnection(DBconnStr)
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
            glbErroLog.AppendLine("Exception in UPDATE DB for Ebay Effect" & vbCrLf & ex.Message.ToString())
        End Try
        ' End If

        Return 0
    End Function

    Private Sub HDbackWork_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles HDbackWork.DoWork
        gblISWrongSettig = False
        Try
            glbMailDT.Rows.Clear()
            Dim exLog As Integer = 0
            glbProcessDT = dbOperation.GetDataTable("select product_asin,target_price,target_used_price,upc,notes from db_products where is_active='Yes'", exLog)
            _totalProcessCnt = 0
            _totalProcessSucc = 0

            If glbProcessDT.Rows.Count > 0 Then
                _totalProcessCnt = glbProcessDT.Rows.Count
            Else
                Exit Sub
            End If

            Dim _lastIndex As Integer = 0
            If exLog = 1 Then
                glbErroLog.Clear()
                HDbackWork.ReportProgress(1)

NextLotofItems:
                If GblIsProcessStart = False Then
                    GoTo ProcessCompleted
                End If

                Dim ItemCntToSearch As Integer = 0

                Dim DtRead As DataTable = dbOperation.GetDataTable("select product_id,upc,product_asin from db_products where product_id >" & _lastIndex & " and is_active='Yes' order by product_id LIMIT 20;", exLog)
                Dim _produtArr1 As New List(Of String)()
                'Dim _productUPC1 As New List(Of String)()
                Dim _produtArr2 As New List(Of String)()
                'Dim _productUPC2 As New List(Of String)()

                If Not DtRead Is Nothing Then
                    ItemCntToSearch = DtRead.Rows.Count
                Else
                    GoTo ProcessCompleted
                End If

                If ItemCntToSearch = 0 Then
                    GoTo ProcessCompleted
                End If

                For rwcnt As Integer = 0 To DtRead.Rows.Count - 1
                    If _produtArr1.Count = 10 Then
                        _produtArr2.Add(DtRead.Rows(rwcnt)("product_asin"))
                        '_productUPC1.Add(DtRead.Rows(rwcnt)("upc"))
                    Else
                        _produtArr1.Add(DtRead.Rows(rwcnt)("product_asin"))
                        '_productUPC1.Add(DtRead.Rows(rwcnt)("upc"))
                    End If
                    If rwcnt = DtRead.Rows.Count - 1 Then
                        _lastIndex = objval.ToInt32(DtRead.Rows(rwcnt)("product_id"))
                    End If
                Next

                Dim url As String = "http://svcs.ebay.com/services/search/FindingService/v1"
                Dim threadNotification2 As Thread

                Dim _searchItems1 As String = String.Join(",", _produtArr1)
                Dim _searchItems2 As String = String.Join(",", _produtArr2)
                Dim TryCnt As Integer = 1

                Dim _RequestURL As String = ""
                Dim threadNotification As Thread
tryAgain:
                Try

                    If GblApiCalAWSInterval > 0 Then
                        System.Threading.Thread.Sleep(GblApiCalAWSInterval)
                    End If

                    Dim helper As New SignedRequestHelper(GblAwsAccessKey, GblAwsSecretKey, DESTINATION)

                    Dim r1 As IDictionary(Of String, String) = New Dictionary(Of String, [String])()
                    r1("Service") = "AWSECommerceService"
                    r1("Operation") = "ItemLookup"
                    If _searchItems2.Length > 0 Then
                        r1("ItemLookup.1.ItemId") = _searchItems1
                        r1("ItemLookup.2.ItemId") = _searchItems2
                    Else
                        r1("ItemLookup.1.ItemId") = _searchItems1
                    End If
                    'r1("ResponseGroup") = "Full"
                    r1("ResponseGroup") = "ItemAttributes,Offers,OfferFull,OfferListings,SalesRank"
                    r1("ItemLookup.Shared.IdType") = "ASIN"
                    r1("IdType") = "ASIN"
                    r1("AssociateTag") = GblAwsSellerID


                    _RequestURL = helper.Sign(r1)
                    Dim urlx As [String] = ""
                    Dim request As WebRequest = HttpWebRequest.Create(_RequestURL)
                    Dim response As WebResponse = request.GetResponse()

                    Dim reader As New XmlTextReader(response.GetResponseStream())
                    reader.WhitespaceHandling = WhitespaceHandling.None

                    Dim RewardXmlDoc As New XmlDocument()
                    Dim isItemLookExist As Boolean = False

                    While reader.Read()
                        If reader.Name = "ItemLookupResponse" Then
                            RewardXmlDoc.LoadXml(reader.ReadOuterXml())
                            reader.Close()
                            isItemLookExist = True
                        End If
                    End While

                    If isItemLookExist = True Then
                        Dim nsmgr As New XmlNamespaceManager(RewardXmlDoc.NameTable)
                        nsmgr.AddNamespace("rate", "http://webservices.amazon.com/AWSECommerceService/2011-08-01")

                        Dim nd As XmlNodeList = RewardXmlDoc.SelectNodes("//rate:Item", nsmgr)
                        For Each node As XmlNode In nd
                            Dim dd As String = node.InnerXml
                            Dim _iAsin As XmlNode = node.SelectSingleNode("./rate:ASIN", nsmgr)
                            Dim _iDetailURL As XmlNode = node.SelectSingleNode("./rate:DetailPageURL", nsmgr)
                            Dim _iTitle As XmlNode = node.SelectSingleNode("./rate:ItemAttributes/rate:Title", nsmgr)
                            Dim _iCategory As XmlNode = node.SelectSingleNode("./rate:ItemAttributes/rate:ProductGroup", nsmgr)
                            Dim _iNewprice As XmlNode = node.SelectSingleNode("./rate:OfferSummary/rate:LowestNewPrice/rate:FormattedPrice", nsmgr)
                            Dim _iUsedprice As XmlNode = node.SelectSingleNode("./rate:OfferSummary/rate:LowestUsedPrice/rate:FormattedPrice", nsmgr)
                            Dim _iLivePrice As XmlNode = node.SelectSingleNode("./rate:Offers/rate:OfferListing/rate:Price/rate:FormattedPrice", nsmgr)
                            Dim _iSaleRank As XmlNode = node.SelectSingleNode("./rate:SalesRank", nsmgr)
                            Dim _iTradeinValue As XmlNode = node.SelectSingleNode("./rate:ItemAttributes/rate:TradeInValue/rate:FormattedPrice", nsmgr)
                            Dim _iNumOfPages As XmlNode = node.SelectSingleNode("./rate:ItemAttributes/rate:NumberOfPages", nsmgr)
                            Dim _iNumOfDisc As XmlNode = node.SelectSingleNode("./rate:ItemAttributes/rate:NumberOfDiscs", nsmgr)
                            Dim _iUPC As XmlNode = node.SelectSingleNode("./rate:ItemAttributes/rate:UPC", nsmgr)

                            Dim con As New SQLiteConnection(DBconnStr)


                            Dim cmd As New SQLiteCommand()
                            cmd.CommandText = "update db_products set product_name=@product_name,last_updated=@last_updated,live_price=@live_price," &
                                "live_newprice=@live_newprice,live_usedprice=@live_usedprice,product_url=@product_url,rank=@rank,trade_in_price=@trade_in_price,product_category=@product_category, " &
                                "live_newprice_pre=live_newprice,live_usedprice_pre=live_usedprice,num_of_pages=@num_of_pages,num_of_disc=@num_of_disc, " &
                                "upc = CASE upc WHEN '' THEN  @upc  ELSE upc END " &
                                " where product_asin=@product_asin"
                            cmd.Connection = con
                            cmd.CommandType = CommandType.Text

                            ' glbErroLog.AppendLine("----------------")
                            Dim newPrice As Double = 0
                            Dim usedPrice As Double = 0
                            Dim TradePrice As Double = 0
                            Dim ebnprice As Double = 0
                            Dim ebuprice As Double = 0
                            Dim itmTitle As String = ""
                            Dim itmCategory As String = ""
                            Dim itmASIN As String = ""
                            Dim itmURL As String = ""
                            Dim numberOfPage As String = ""
                            Dim numberOfDisc As String = ""
                            Dim iupccode As String = ""


                            If _iTitle Is Nothing Then
                                cmd.Parameters.AddWithValue("@product_name", "")
                            Else
                                itmTitle = objval.ToString(_iTitle.InnerText)
                                cmd.Parameters.AddWithValue("@product_name", itmTitle)
                            End If

                            If _iUPC Is Nothing Then
                                cmd.Parameters.AddWithValue("@upc", "")
                            Else
                                iupccode = objval.ToString(_iUPC.InnerText)
                                cmd.Parameters.AddWithValue("@upc", iupccode)
                            End If

                            If _iCategory Is Nothing Then
                                cmd.Parameters.AddWithValue("@product_category", "")
                            Else
                                itmCategory = objval.ToString(_iCategory.InnerText)
                                cmd.Parameters.AddWithValue("@product_category", itmCategory)
                            End If

                            If _iSaleRank Is Nothing Then
                                cmd.Parameters.AddWithValue("@rank", "")
                            Else
                                cmd.Parameters.AddWithValue("@rank", objval.ToString(_iSaleRank.InnerText))
                            End If

                            If IsNothing(_iLivePrice) Then
                                cmd.Parameters.AddWithValue("@live_price", 0)
                            Else
                                cmd.Parameters.AddWithValue("@live_price", objval.ToDouble(_iLivePrice.InnerText.Replace("$", "")))
                            End If

                            If IsNothing(_iNewprice) Then
                                cmd.Parameters.AddWithValue("@live_newprice", 0.0)
                            Else
                                newPrice = objval.ToDouble(_iNewprice.InnerText.Replace("$", ""))
                                cmd.Parameters.AddWithValue("@live_newprice", newPrice)
                            End If

                            If IsNothing(_iUsedprice) Then
                                cmd.Parameters.AddWithValue("@live_usedprice", 0.0)
                            Else
                                usedPrice = objval.ToDouble(_iUsedprice.InnerText.Replace("$", ""))
                                cmd.Parameters.AddWithValue("@live_usedprice", usedPrice)
                            End If

                            If IsNothing(_iTradeinValue) Then
                                cmd.Parameters.AddWithValue("@trade_in_price", 0.0)
                            Else
                                TradePrice = objval.ToDouble(_iTradeinValue.InnerText.Replace("$", ""))
                                cmd.Parameters.AddWithValue("@trade_in_price", TradePrice)
                            End If

                            If IsNothing(_iNumOfPages) Then
                                cmd.Parameters.AddWithValue("@num_of_pages", 0.0)
                            Else
                                numberOfPage = objval.ToString(_iNumOfPages.InnerText)
                                cmd.Parameters.AddWithValue("@num_of_pages", numberOfPage)
                            End If

                            If IsNothing(_iNumOfDisc) Then
                                cmd.Parameters.AddWithValue("@num_of_disc", 0.0)
                            Else
                                numberOfDisc = objval.ToString(_iNumOfDisc.InnerText)
                                cmd.Parameters.AddWithValue("@num_of_disc", numberOfDisc)
                            End If

                            If IsNothing(_iAsin) Then
                                cmd.Parameters.AddWithValue("@product_asin", "")
                            Else
                                itmASIN = objval.ToString(_iAsin.InnerText)
                                cmd.Parameters.AddWithValue("@product_asin", itmASIN)
                            End If

                            If IsNothing(_iDetailURL) Then
                                cmd.Parameters.AddWithValue("@product_url", "")
                            Else
                                itmURL = objval.ToString(_iDetailURL.InnerText)
                                cmd.Parameters.AddWithValue("@product_url", itmURL)
                            End If

                            cmd.Parameters.AddWithValue("@last_updated", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture))

                            Dim dr() As DataRow = glbProcessDT.Select("product_asin='" & itmASIN & "'")

                            Dim target_price As Double = objval.ToDouble(dr(0).ItemArray(1))
                            Dim target_used_price As Double = objval.ToDouble(dr(0).ItemArray(2))
                            Dim upcf As String = objval.ToString(dr(0).ItemArray(3))

                            con.Open()
                            Try
                                Dim ddssdd As Integer = cmd.ExecuteNonQuery()
                                _totalProcessSucc = _totalProcessSucc + 1
                                HDbackWork.ReportProgress(1)

                                threadNotification = New Thread(Sub() Me.RunProcess(itmASIN, newPrice, usedPrice, ebnprice, ebuprice, itmTitle, itmURL, upcf))
                                threadNotification.IsBackground = True
                                threadNotification.Start()

                                'ChkThread:
                                '                                If threadNotification.IsAlive = True Then
                                '                                    System.Threading.Thread.Sleep(1000)
                                '                                    GoTo ChkThread
                                '                                End If
                            Catch ex As Exception
                                Dim xsddd As Integer
                                xsddd = 0
                            End Try
                            cmd.Parameters.Clear()
                            con.Close()
                        Next
                    Else
                        ' xmls is not proper .. so try again 
                        If TryCnt < 4 Then
                            TryCnt = TryCnt + 1
                        End If
                        System.Threading.Thread.Sleep(1000)
                        GoTo tryAgain
                    End If

                    GoTo NextLotofItems

                Catch ex As Exception
                    If ex.Message.ToString.Contains("(403)") Then
                        gblISWrongSettig = True
                        GoTo ProcessCompleted
                    Else
                        glbErroLog.AppendLine(_RequestURL & vbCrLf & ex.Message.ToString() & vbCrLf & "------------------------------------------------")
                        GoTo tryAgain
                    End If

                    'exLog = ex.Message.ToString() + vbLf + requestUrl
                End Try

                GoTo NextLotofItems

ProcessCompleted:

                Dim taskCnt As Int64 = HDtasks.Where(Function(x) x.Status = TaskStatus.Running).ToList().Count()

                If taskCnt > 0 Then
                    Threading.Thread.Sleep(3000)
                    GoTo ProcessCompleted
                End If

                HDtasks.Clear()
            End If
        Catch ex As Exception
            objval.MsgWarning(ex.Message.ToString(), "Error")
            GblIsProcessStart = False
            glbErroOnProcess = True
            gblIsprocessByManual = False
        End Try
    End Sub

    Private Sub HDbackWork_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles HDbackWork.RunWorkerCompleted

        If glbMailDT.Rows.Count > 0 And _GblSenderMailID.Length > 0 And GblIsAlertMail = True Then
            SplashScreenManager.ShowForm(GetType(ozLoading))
            SplashScreenManager.Default.SetWaitFormDescription("Sending Mail..")
            Dim mailaddr As String = GblAlertMailIDs
            Dim SmtpServer As New SmtpClient()
            SmtpServer.Credentials = New Net.NetworkCredential(_GblSenderMailID, _GblSenderMailPwd)
            SmtpServer.Port = _GblSenderPortNo
            SmtpServer.Host = _GblSenderSMTP
            SmtpServer.EnableSsl = _GblSenderIsSSL
            SmtpServer.Timeout = 600000
            mailaddr = GblAlertMailIDs



            Dim Mail As New MailMessage()
            Dim addr() As String = mailaddr.Split(",")
            Try
                Mail.From = New MailAddress(_GblSenderMailID, "AWS Item Analysis", System.Text.Encoding.UTF8)
                Dim i As Byte
                For i = 0 To addr.Length - 1
                    Mail.To.Add(addr(i))
                Next

                Dim sendDate As String = DateTime.Now.ToString("dd MMM yyyy hh:mm tt")

                Mail.Subject = "Amazon Analysis Report : " & sendDate

                Mail.IsBodyHtml = True

                Dim AsinCodes As String = "'" & String.Join("','", (From row In glbMailDT.AsEnumerable Select row("asin")).ToArray) & "'"
                Dim DtMail As DataTable

                Dim qry As String = "select *, round( (case when target_price>0 and live_newprice>0 then ((live_newprice/target_price)*100)-100  else 0.00 end),2) as new_per," &
                                    "round( (case when target_used_price>0 and live_usedprice>0 then ((live_usedprice/target_used_price)*100)-100  else 0.00 end),2) as used_per, " &
                                    "round( (case when target_price>0 and ebay_live_new_price>0 then ((ebay_live_new_price/target_price)*100)-100  else 0.00 end),2) as new_per_eb, " &
                                    "round( (case when target_used_price>0 and ebay_live_used_price>0 then ((ebay_live_used_price/target_used_price)*100)-100  else 0.00 end),2) as used_per_eb, " &
                                    "(case when (live_newprice>0 and target_price>0) and (target_price>=live_newprice) then 1  else 0 end) as isNpriceHigh, " &
                                    "(case when (live_usedprice>0 and target_used_price>0) and (target_used_price>=live_usedprice) then 1  else 0 end) as isUpriceHigh " &
                                    " from db_products " &
                                    " where is_active='Yes' and product_asin in (" & AsinCodes & ")"

                Dim exLog As Integer = 0
                DtMail = dbOperation.GetDataTable(qry, exLog)

                Dim mailStr As New StringBuilder
                mailStr.AppendLine("")
                mailStr.AppendLine("<html><head><title>Amazon item price analysis report</title></head>")
                mailStr.AppendLine("<body style='padding:6px;'><div style='border:1px solid #808080;padding:5px;border-radius:5px;margin:5px;' >")
                mailStr.AppendLine("<table style='font-family: Arial, Helvetica, sans-serif;font-size:15px;color: #131212;'>")
                mailStr.AppendLine("<tr><td>Amazon item price analysis report</td></tr><tr><td>" & sendDate & "</td></tr></table><br />")
                mailStr.AppendLine("<table cellspacing='0' cellpadding='7' width='1325px'  style='border: 1px solid #808080;font-family: Arial, Helvetica, sans-serif;font-size: 13px;color: #131212;'>")

                If exLog = 1 Then

                    For Each dr As DataRow In DtMail.Rows
                        mailStr.AppendLine("<tr><td width='380' style='font-size:12px;'>" & objval.ToString(dr("product_name")) & "</td><td width='70' style='background-color:#ddd;'></td>" &
                                            "<td width='60' style='background-color:#ddd;font-size:12px;font-weight:bold;'>Amazon $</td> " &
                                            "<td width='60' style='background-color:#ddd;font-size:12px;font-weight:bold;'>Target $</td>" &
                                            "<td width='60' style='background-color:#ddd;font-size:12px;font-weight:bold;'>Am.Diff%</td>" &
                                            "<td width='60' style='background-color:#ddd;font-size:12px;font-weight:bold;'>Ebay$</td>" &
                                            "<td width='80' style='background-color:#ddd;font-size:12px;font-weight:bold;'>Ebay.Comp$</td>" &
                                            "<td width='60' style='background-color:#ddd;font-size:12px;font-weight:bold;'>Ebay.Avg%</td>" &
                                            "<td width='60' style='background-color:#ddd;font-size:12px;font-weight:bold;'>TradeIN</td>" &
                                            "<td style='background-color:#ddd;font-size:12px;font-weight:bold;'>Rank</td></tr>")

                        mailStr.AppendLine("<tr><td style='font-weight:bold;'>" & objval.ToString(dr("product_category")) & "</td><td style='background-color:#ddd;font-size:12px;font-weight:bold;'>New Price</td>" &
                                           "<td style='background-color:#f9eee7;'>$" & objval.ToString(dr("live_newprice")) & "</td>" &
                                           "<td " & IIf(objval.ToInt32(dr("isNpriceHigh")) = 1, "style='font-weight:bold;color:#f21717;'", "") & ">$" & objval.ToString(dr("target_price")) & "</td>" &
                                           "<td >" & objval.ToString(dr("new_per")) & "%</td>" &
                                           "<td style='background-color:#e2f9ef;'>$" & objval.ToString(dr("ebay_live_new_price")) & "</td>" &
                                           "<td>$" & objval.ToString(dr("ebay_new_comprice_avg")) & "</td>" &
                                           "<td>" & objval.ToString(dr("ebay_new_comprice_per")) & "%</td>" &
                                           "<td>" & IIf(objval.ToInt32(dr("trade_in_price")) > 0, "$" & objval.ToString(dr("trade_in_price")), "") & "</td>" &
                                           "<td>" & objval.ToString(dr("rank")) & "</td></tr>")

                        mailStr.AppendLine("<tr><td><b>ASIN | UPC : </b> " & objval.ToString(dr("product_asin")) & " | " & objval.ToString(dr("upc")) & "</td> " &
                                           "<td style='background-color:#ddd;font-size:12px;font-weight:bold;'>Used Price</td>" &
                                           "<td style='background-color:#f9eee7;'>$" & objval.ToString(dr("live_usedprice")) & "</td><td " & IIf(objval.ToInt32(dr("isUpriceHigh")) = 1, "style='font-weight:bold;color:#f21717;'", "") & ">$" & objval.ToString(dr("target_used_price")) & "</td>" &
                                           "<td>" & objval.ToString(dr("used_per")) & "%</td><td style='background-color:#e2f9ef;'>$" & objval.ToString(dr("ebay_live_used_price")) & "</td><td>$" & objval.ToString(dr("ebay_used_comprice_avg")) & "</td><td>" & objval.ToString(dr("ebay_new_comprice_per")) & "%</td><td></td>" &
                                           "<td style='font-size:11px;'><a href='" & objval.ToString(dr("product_url")) & "'>Go to Amazon</a> | ")
                        mailStr.AppendLine("<a href='http://www.ebay.com/sch/i.html?LH_Complete=1&_nkw=" & objval.ToString(dr("upc")) & "&rt=nc&LH_BIN=1'>Ebay Completed List</a> | ")
                        mailStr.AppendLine("<a href='http://www.ebay.com/sch/i.html?_nkw=" & objval.ToString(dr("upc")) & "'>Ebay Active List</a> ")
                        mailStr.AppendLine("<tr><td colspan='10' style='border-top:1px solid #ddd;' ></td></tr>")
                    Next
                Else
                    glbErroLog.AppendLine("MAIL Sending Datatable error")
                End If

                mailStr.AppendLine("</table></div></body></html>")

                Mail.Body = objval.ToString(mailStr)
                SmtpServer.Send(Mail)
                Mail.Dispose()
                SmtpServer.Dispose()

            Catch ex As Exception
                Mail.Dispose()
                glbErroLog.AppendLine("MAIL Sending ERROR" & vbCrLf & ex.Message.ToString() & vbCrLf & "------------------------------------------------")
            End Try

            SplashScreenManager.CloseForm()
        End If

        pnlUpdating.Visible = False
        Try

            If Not Directory.Exists(Application.StartupPath & "\logs") Then
                Directory.CreateDirectory(Application.StartupPath & "\logs")
            End If
            Dim logfilname As String = "Log_" & DateTime.Now.ToString("dd MMM YYYY hhmmss tt") & ".txt"
            If File.Exists(Application.StartupPath & "\logs\" & logfilname) Then
                File.Delete(Application.StartupPath & "\logs\" & logfilname)
            End If

            Using sw As StreamWriter = File.CreateText(Application.StartupPath & "\logs\" & logfilname)
                sw.Write(Convert.ToString(glbErroLog))
            End Using
        Catch ex As Exception
            objval.MsgError(ex.Message.ToString(), "Error while Saving Logs")
        End Try
        If gblISWrongSettig = True Then
            objval.MsgStop("AWS settings seems invalid. Please re-check AWS Accesskey,Secretkey and SellerID in setting and trygain.", "Invalid AWS parameters")
            mnuStart.Enabled = True
            mnuUpdate.Enabled = True
            mnuStop.Enabled = False
            GblIsProcessStart = False
            mnuAWSScan.Enabled = True
            mnuMWSScan.Enabled = True
            Exit Sub
        End If

        If glbErroOnProcess = True Then
            mnuStart.Enabled = True
            mnuUpdate.Enabled = True
            GblIsProcessStart = False
            mnuAWSScan.Enabled = True
            mnuMWSScan.Enabled = True
            objval.MsgWarning("Monitoring Process has been stopped", "ERROR")
        End If
        If gblIsprocessByManual = True Then
            mnuStop.Enabled = False
            mnuStart.Enabled = True
            GblIsProcessStart = False
            mnuAWSScan.Enabled = True
            mnuMWSScan.Enabled = True
        End If
        mnuUpdate.Enabled = True
        ReloadGrid()
    End Sub

    Private Sub HDbackWork_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles HDbackWork.ProgressChanged
        lblProcessingcnt.Text = _totalProcessSucc & "/" & _totalProcessCnt
    End Sub

    Private Sub mnuUpdate_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnuUpdate.ItemClick
        gblHashMailSent.Clear()

        If GblIsProcessStart = True Then
            objval.MsgWarning("Please stop Monitoring process first.", "Access Denied")
            Exit Sub
        End If

        If mnuAWSScan.Checked = True And GblAwsSellerID = "" Then
            objval.MsgWarning("AWS Setting is not ready. Please complete settings in setting menu", "Warning")
            Exit Sub
        ElseIf mnuMWSScan.Checked = True And GblMWSSellerID = "" Then
            objval.MsgWarning("MWS Setting is not ready. Please complete settings in setting menu", "Warning")
            Exit Sub
        End If

        'chk_Target_Price.Checked = True
        'chk_TargetUsedPrice.Checked = True
        gblisPausemode = False
        gblisPauseLASTiNDEX = 0
        gblisPauseSTATUS = "pause"
        mnuPause.ImageOptions.LargeImage = My.Resources.pause_button
        mnuPause.Caption = "Pause Monitoring"
        picPaused.Visible = False
        mnuPause.Enabled = False

        gblIsprocessByManual = True
        GblIsProcessStart = True
        pnlUpdating.Visible = True
        glbErroOnProcess = False
        mnuUpdate.Enabled = False
        mnuStart.Enabled = False
        mnuStop.Enabled = True
        lblProcessType.Text = "Updateing Price (Manual)"
        If mnuAWSScan.Checked = True Then
            If HDbackWork.IsBusy = False Then
                HDbackWork.RunWorkerAsync()
            End If
        Else
            If MWSBackWork.IsBusy = False Then
                MWSBackWork.RunWorkerAsync()
            End If
        End If


    End Sub

    Private Sub dgvGrid_RowCellStyle(ByVal sender As Object, ByVal e As RowCellStyleEventArgs) Handles dgvGrid.RowCellStyle
        Dim View As GridView = sender
        If e.Column.FieldName = "target_price" Then
            ' Dim livprice As Double = objval.ToDouble(View.GetRowCellDisplayText(e.RowHandle, View.Columns("live_price")))
            Dim livNewprice As Double = objval.ToDouble(View.GetRowCellDisplayText(e.RowHandle, View.Columns("live_newprice")))
            Dim Targetprice As Double = objval.ToDouble(View.GetRowCellDisplayText(e.RowHandle, View.Columns("target_price")))

            If Targetprice > 0 Then
                'If livprice > 0 And livprice < Targetprice Then
                '    e.Appearance.ForeColor = Color.Maroon
                '    e.Appearance.BackColor = Color.FromArgb(248, 233, 233)
                '    e.Appearance.BackColor2 = Color.FromArgb(248, 233, 233)
                'Else
                If livNewprice > 0 And livNewprice < Targetprice Then
                    e.Appearance.ForeColor = Color.Maroon
                    e.Appearance.BackColor = Color.FromArgb(248, 233, 233)
                    e.Appearance.BackColor2 = Color.FromArgb(248, 233, 233)
                End If
            End If

        ElseIf e.Column.FieldName = "target_used_price" Then
            Dim livUsedprice As Double = objval.ToDouble(View.GetRowCellDisplayText(e.RowHandle, View.Columns("live_usedprice")))
            Dim TargetUSedprice As Double = objval.ToDouble(View.GetRowCellDisplayText(e.RowHandle, View.Columns("target_used_price")))

            If TargetUSedprice > 0 Then
                If livUsedprice > 0 And livUsedprice < TargetUSedprice Then
                    e.Appearance.ForeColor = Color.Maroon
                    e.Appearance.BackColor = Color.FromArgb(248, 233, 233)
                    e.Appearance.BackColor2 = Color.FromArgb(248, 233, 233)
                End If
            End If

        End If
    End Sub

    Private Sub dgvGrid_CustomColumnDisplayText(sender As Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs) Handles dgvGrid.CustomColumnDisplayText
        If e.Column.FieldName = "live_price" OrElse e.Column.FieldName = "live_newprice" OrElse e.Column.FieldName = "trade_in_price" OrElse e.Column.FieldName = "live_usedprice" OrElse e.Column.FieldName = "new_per" OrElse e.Column.FieldName = "used_per" Then
            If objval.ToDouble(e.Value) = 0 Then
                e.DisplayText = ""
            End If
        End If
    End Sub

    Private Sub dgvGrid_DoubleClick(sender As Object, e As EventArgs) Handles dgvGrid.DoubleClick
        Try
            Dim hi As DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo = dgvGrid.CalcHitInfo(TryCast(e, MouseEventArgs).Location)
            If hi.InRowCell Then
                Dim Rowhandel As Integer = hi.RowHandle
                Dim asin As String = objval.ToString(dgvGrid.GetRowCellValue(dgvGrid.FocusedRowHandle, "product_asin"))
                Dim proid As Int32 = objval.ToInt32(dgvGrid.GetRowCellValue(dgvGrid.FocusedRowHandle, "product_id"))
                If asin.Length > 0 Then

                    Dim dv As New DataView
                    dv = dgvGrid.DataSource

                    Using frmObjPopup As New FrmItemDetail(proid, asin, dv.ToTable)
                        frmObjPopup.ShowDialog()
                        frmObjPopup.BringToFront()
                    End Using

                    'Using frmObj As New FrmItemDetail(asin)
                    '    frmObj.ShowDialog()
                    '    frmObj.BringToFront()
                    'End Using
                    ReloadGrid()
                End If
            End If
        Catch ex As Exception
            objval.MsgError(ex.Message, "dgvDevice_DoubleClick")
        End Try
    End Sub

#Region "SERVICE Sync Code"

    Public Function GetAWSItem(AWSSellerID As [String], AWSAccessKey As [String], AWSSecretKey As [String], ASINNo As [String], ByRef _price As [Double], ByRef _newprice As [Double], _
        ByRef _usedprice As [Double], ByRef _itemtitle As [String], ByRef _itemWEBUrL As [String], ByRef ExLog As [String]) As [Boolean]
        Dim requestUrl As [String] = ""
        Try
            _price = 0.0
            _newprice = 0.0
            _usedprice = 0.0
            _itemtitle = ""

            Dim helper As New SignedRequestHelper(AWSAccessKey, AWSSecretKey, DESTINATION)

            Dim r1 As IDictionary(Of String, String) = New Dictionary(Of String, [String])()
            r1("Service") = "AWSECommerceService"
            r1("Operation") = "ItemLookup"
            r1("ItemId") = "B018FK66TU"
            r1("ItemLookup.1.ItemId") = "B00C6F60NI,B000FQISSU,B0009HI5KQ,B000O7864G,B0027UY8B8,B00DXLO086,B00KVS6YZQ,B00007M5HU,B001BN4WA4,B000002TSV"
            r1("ItemLookup.2.ItemId") = "B00002CF5X,B0000541WJ,B00005NX1J,B00006BSGS,B00007IG1M,B000083C6T,B0000AYL45,B0000B1OC4,B0001I5592"
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
            ElseIf tagName = "DetailPageURL" OrElse tagName = "ASIN" Then
                Return RetVal
            Else
                Return priceStr.Replace("$", "")
            End If
        Catch ex As Exception

            If tagName = "Title" OrElse tagName = "Price" OrElse tagName = "DetailPageURL" OrElse tagName = "ASIN" Then
                Return "-1"
            Else
                Return "-1|-1"
            End If
        End Try
    End Function

#End Region

    Private Sub BarButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick

    End Sub

    Private Sub hdAlert_BeforeFormShow(sender As Object, e As DevExpress.XtraBars.Alerter.AlertFormEventArgs) Handles hdAlert.BeforeFormShow
        e.AlertForm.OpacityLevel = 1
    End Sub

    Private Sub repCompleteUPCLink_Click(sender As Object, e As EventArgs) Handles repCompleteUPCLink.Click
        Try
            Dim proid As String = objval.ToString(dgvGrid.GetRowCellValue(dgvGrid.FocusedRowHandle, "upc"))
            If proid.Length > 0 Then
                Process.Start("http://www.ebay.com/sch/i.html?_nkw=" & proid)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub repUPCActiveLink_Click(sender As Object, e As EventArgs) Handles repUPCActiveLink.Click
        Try
            Dim proid As String = objval.ToString(dgvGrid.GetRowCellValue(dgvGrid.FocusedRowHandle, "upc"))
            If proid.Length > 0 Then
                Process.Start("http://www.ebay.com/sch/i.html?LH_Complete=1&_nkw=" & proid & "&rt=nc&LH_BIN=1")
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Function GetStockSelection() As String()
        Dim selectedArray As Integer() = New Integer(Convert.ToInt16(selectionStock.SelectedCount) - 1) {}
        Dim returnArray(selectionStock.SelectedCount - 1) As String
        For i As Integer = 0 To selectionStock.SelectedCount - 1
            'selectedArray(i) = Convert.ToInt16(TryCast(selection.GetSelectedRow(i), DataRowView)(0))
            Dim ColId As Integer = dgvGrid.Columns("product_id").VisibleIndex
            returnArray(i) = Convert.ToString(TryCast(selectionStock.GetSelectedRow(i), DataRowView)("product_id"))
            TotRC = TotRC + 1
        Next
        Return returnArray
    End Function

    Private Sub mnuKeepaScan_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnuKeepaScan.ItemClick
        If GblIsKeepaOpen = False Then
            Dim objFrm As New FrmKeepaScan
            objFrm.Show()
        Else
            objval.MsgWarning("Keepa Winows is already open.", "Check Minimized Apps")
        End If
    End Sub


    Public Sub ShowWaitForm()

        If (Not SplashScreenManager1.IsSplashFormVisible = True) Then
            SplashScreenManager1.ShowWaitForm()
            SplashScreenManager1.SetWaitFormCaption("Please Wait")
            SplashScreenManager1.SetWaitFormDescription("Loading Data...")
        End If

    End Sub

    Public Sub CloseWaitForm()
        If SplashScreenManager1.IsSplashFormVisible Then
            SplashScreenManager1.CloseWaitForm()
        End If
    End Sub


    Private Sub mnuColumnSetting_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnuColumnSetting.ItemClick
        SaveResetColumnView(False)
        LoadGridColumnSetting()
    End Sub

    Private Sub mnuResetView_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnuResetView.ItemClick
        SaveResetColumnView(True)
        LoadGridColumnSetting()
    End Sub

    Public Sub SaveResetColumnView(ByVal isReset As Boolean)
        Try
            If isReset = True Then
                If File.Exists(columnfilepath) Then
                    File.Delete(columnfilepath)
                End If

                Using sw As StreamWriter = File.CreateText(columnfilepath)
                    sw.Write(My.Resources.def_view)
                    sw.Close()
                End Using
                LoadGridColumnSetting()
                objval.MsgSuccess("Column view setting updated successfully", "Success")
            Else

                If File.Exists(columnfilepath) Then
                    File.Delete(columnfilepath)
                End If
                Dim xxx As New StringBuilder

                Dim settings As XmlWriterSettings = New XmlWriterSettings()
                settings.Indent = True
                Dim writer As XmlWriter = XmlWriter.Create("columnsetting.xml", settings)
                writer.WriteStartDocument()
                writer.WriteStartElement("table")
                For Each dd As GridColumn In dgvGrid.Columns
                    If dd.FieldName = "CheckMarkSelection" OrElse dd.FieldName = "product_id" Then
                        Continue For
                    End If
                    writer.WriteStartElement("Column")
                    writer.WriteElementString("col_field", dd.FieldName)
                    writer.WriteElementString("col_width", dd.Width)
                    writer.WriteElementString("col_order", dd.VisibleIndex)
                    If dd.Visible = False Then
                        writer.WriteElementString("col_show", "NO")
                    Else
                        writer.WriteElementString("col_show", "YES")
                    End If
                    writer.WriteEndElement()
                Next

                writer.WriteEndDocument()
                writer.Flush()
                writer.Close()
                objval.MsgSuccess("Column view setting updated successfully", "Success")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub LoadGridColumnSetting()
        If File.Exists(columnfilepath) Then

            Dim doc As XDocument = XDocument.Load(columnfilepath)
            Dim points = doc.Descendants("Column")
            dgvGrid.Columns("CheckMarkSelection").VisibleIndex = 0
            For Each current As XElement In points

                If Convert.ToString(current.Element("col_show").Value) = "YES" Then
                    ' dgvGrid.Columns(Convert.ToString(current.Element("col_field").Value)).Width = Convert.ToInt32(current.Element("col_width").Value)
                    'dgvGrid.Columns(Convert.ToString(current.Element("col_field").Value)).Caption = Convert.ToString(current.Element("col_cap").Value)
                    If Not dgvGrid.Columns(Convert.ToString(current.Element("col_field").Value)) Is Nothing Then
                        dgvGrid.Columns(Convert.ToString(current.Element("col_field").Value)).Visible = True
                        dgvGrid.Columns(Convert.ToString(current.Element("col_field").Value)).VisibleIndex = Convert.ToInt32(current.Element("col_order").Value) + 1
                    End If

                Else
                    If Not dgvGrid.Columns(Convert.ToString(current.Element("col_field").Value)) Is Nothing Then
                        dgvGrid.Columns(Convert.ToString(current.Element("col_field").Value)).Visible = False
                    End If
                End If
            Next

            dgvGrid.Columns("product_id").Visible = False
            'dgvGrid.Columns("CheckMarkSelection").VisibleIndex = 0
        End If
    End Sub

    Private Sub mnuReport_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnuReport.ItemClick
        Dim frmobj As New FrmReport
        frmobj.Show()
    End Sub

    Private Sub mnuMWSScan_CheckedChanged(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnuMWSScan.CheckedChanged
        If mnuMWSScan.Checked = True Then
            mnuAWSScan.Checked = False
            mnuRefProductInfo.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        Else
            mnuAWSScan.Checked = True
            mnuRefProductInfo.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        End If
    End Sub

    Private Sub mnuAWSScan_CheckedChanged(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnuAWSScan.CheckedChanged
        If mnuAWSScan.Checked = True Then
            mnuMWSScan.Checked = False
            mnuRefProductInfo.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
        Else
            mnuMWSScan.Checked = True
            mnuRefProductInfo.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
        End If
    End Sub

    Private Sub MWSBackWork_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles MWSBackWork.DoWork
        gblISWrongSettig = False
        Try
            glbMailDT.Rows.Clear()
            Dim exLog As Integer = 0
            glbProcessDT = dbOperation.GetDataTable("select product_asin,target_price,target_used_price,upc,notes,product_name from db_products  where is_active='Yes'", exLog)

            If gblisPausemode = False Then
                _totalProcessCnt = 0
                _totalProcessSucc = 0
            End If


            If glbProcessDT.Rows.Count > 0 Then
                If gblisPausemode = False Then
                    _totalProcessCnt = glbProcessDT.Rows.Count
                End If
            Else
                Exit Sub
            End If

            Dim _lastIndex As Integer = 0
            If gblisPausemode = True Then
                _lastIndex = gblisPauseLASTiNDEX
                gblisPausemode = False
                _totalProcessSucc = _totalProcessSucc - 3
            End If
            If exLog = 1 Then
                glbErroLog.Clear()
                MWSBackWork.ReportProgress(1)

NextLotofItems:
                If GblIsProcessStart = False OrElse gblisPausemode = True Then
                    GoTo ProcessCompleted
                End If

                Dim ItemCntToSearch As Integer = 0
                gblisPauseLASTiNDEX = _lastIndex

                Dim DtRead As DataTable = dbOperation.GetDataTable("select product_id,upc,product_asin from db_products where  is_active='Yes' and product_id >" & _lastIndex & " order by product_id LIMIT 20;", exLog)
                Dim _produtArr1 As New List(Of String)()
                'Dim _productUPC1 As New List(Of String)()
                'Dim _produtArr2 As New List(Of String)()
                'Dim _productUPC2 As New List(Of String)()
                ' Dim aa2 As String = MWSProductsMain.ProductInfo()
                If Not DtRead Is Nothing Then
                    ItemCntToSearch = DtRead.Rows.Count
                Else
                    GoTo ProcessCompleted
                End If

                If ItemCntToSearch = 0 Then
                    GoTo ProcessCompleted
                End If

                For rwcnt As Integer = 0 To DtRead.Rows.Count - 1
                    _produtArr1.Add(DtRead.Rows(rwcnt)("product_asin"))

                    If rwcnt = DtRead.Rows.Count - 1 Then
                        _lastIndex = objval.ToInt32(DtRead.Rows(rwcnt)("product_id"))
                    End If
                Next

                Dim TryCnt As Integer = 1

tryAgain:
                If GblApiCalMWSInterval > 0 Then
                    System.Threading.Thread.Sleep(GblApiCalMWSInterval)
                End If

                Dim ab As New List(Of AmazonMWS1.RetLowestPrice)()
                ab = MWSProductsMain.GetLowestPrice2(_produtArr1)

                If ab Is Nothing Then
                    If TryCnt < 4 Then
                        TryCnt = TryCnt + 1
                    End If
                    System.Threading.Thread.Sleep(1000)
                    GoTo tryAgain
                End If

                For Each lstVar As AmazonMWS1.RetLowestPrice In ab
                    'xxeads.asin_key
                    Dim ebnprice As Double = 0
                    Dim ebuprice As Double = 0
                    Dim itmURL As String = "https://www.amazon.com/dp/" & lstVar.asin_key

                    Dim con As New SQLiteConnection(DBconnStr)

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

                    Dim dr() As DataRow = glbProcessDT.Select("product_asin='" & lstVar.asin_key & "'")

                    Dim target_price As Double = objval.ToDouble(dr(0).ItemArray(1))
                    Dim target_used_price As Double = objval.ToDouble(dr(0).ItemArray(2))
                    Dim upcf As String = objval.ToString(dr(0).ItemArray(3))
                    Dim itmTitle As String = objval.ToString(dr(0).ItemArray(5))

                    con.Open()
                    Try
                        Dim ddssdd As Integer = cmd.ExecuteNonQuery()
                        _totalProcessSucc = _totalProcessSucc + 1
                        MWSBackWork.ReportProgress(1)
                        Dim threadNotification As Thread
                        threadNotification = New Thread(Sub() Me.RunProcess(lstVar.asin_key, lstVar.new_price, lstVar.used_price, ebnprice, ebuprice, itmTitle, itmURL, upcf))
                        threadNotification.IsBackground = True
                        threadNotification.Start()

                    Catch ex As Exception
                        Dim xsddd As Integer
                        xsddd = 0
                    End Try
                    cmd.Parameters.Clear()
                    con.Close()

                Next
                GoTo NextLotofItems

ProcessCompleted:

                Dim taskCnt As Int64 = HDtasks.Where(Function(x) x.Status = TaskStatus.Running).ToList().Count()

                If taskCnt > 0 Then
                    Threading.Thread.Sleep(3000)
                    GoTo ProcessCompleted
                End If

                HDtasks.Clear()
            End If
        Catch ex As Exception
            objval.MsgWarning(ex.Message.ToString(), "Error")
            GblIsProcessStart = False
            glbErroOnProcess = True
            gblIsprocessByManual = False
        End Try

    End Sub

    Private Sub MWSBackWork_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles MWSBackWork.ProgressChanged
        lblProcessingcnt.Text = _totalProcessSucc & "/" & _totalProcessCnt
    End Sub

    Private Sub MWSBackWork_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles MWSBackWork.RunWorkerCompleted
        If gblisPausemode = True Then

            Exit Sub
        End If

        If glbMailDT.Rows.Count > 0 And _GblSenderMailID.Length > 0 And GblIsAlertMail = True Then
            SplashScreenManager.ShowForm(GetType(ozLoading))
            SplashScreenManager.Default.SetWaitFormDescription("Sending Mail..")
            Dim mailaddr As String = GblAlertMailIDs
            Dim SmtpServer As New SmtpClient()
            SmtpServer.Credentials = New Net.NetworkCredential(_GblSenderMailID, _GblSenderMailPwd)
            SmtpServer.Port = _GblSenderPortNo
            SmtpServer.Host = _GblSenderSMTP
            SmtpServer.EnableSsl = _GblSenderIsSSL
            SmtpServer.Timeout = 600000
            mailaddr = GblAlertMailIDs



            Dim Mail As New MailMessage()
            Dim addr() As String = mailaddr.Split(",")
            Try
                Mail.From = New MailAddress(_GblSenderMailID, "AWS Item Analysis", System.Text.Encoding.UTF8)
                Dim i As Byte
                For i = 0 To addr.Length - 1
                    Mail.To.Add(addr(i))
                Next

                Dim sendDate As String = DateTime.Now.ToString("dd MMM yyyy hh:mm tt")

                Mail.Subject = "Amazon Analysis Report : " & sendDate

                Mail.IsBodyHtml = True

                Dim AsinCodes As String = "'" & String.Join("','", (From row In glbMailDT.AsEnumerable Select row("asin")).ToArray) & "'"
                Dim DtMail As DataTable

                Dim qry As String = "select *, round( (case when target_price>0 and live_newprice>0 then ((live_newprice/target_price)*100)-100  else 0.00 end),2) as new_per," &
                                    "round( (case when target_used_price>0 and live_usedprice>0 then ((live_usedprice/target_used_price)*100)-100  else 0.00 end),2) as used_per, " &
                                    "round( (case when target_price>0 and ebay_live_new_price>0 then ((ebay_live_new_price/target_price)*100)-100  else 0.00 end),2) as new_per_eb, " &
                                    "round( (case when target_used_price>0 and ebay_live_used_price>0 then ((ebay_live_used_price/target_used_price)*100)-100  else 0.00 end),2) as used_per_eb, " &
                                    "(case when (live_newprice>0 and target_price>0) and (target_price>=live_newprice) then 1  else 0 end) as isNpriceHigh, " &
                                    "(case when (live_usedprice>0 and target_used_price>0) and (target_used_price>=live_usedprice) then 1  else 0 end) as isUpriceHigh " &
                                    " from db_products " &
                                    " where product_asin in (" & AsinCodes & ")"

                Dim exLog As Integer = 0
                DtMail = dbOperation.GetDataTable(qry, exLog)

                Dim mailStr As New StringBuilder
                mailStr.AppendLine("")
                mailStr.AppendLine("<html><head><title>Amazon item price analysis report</title></head>")
                mailStr.AppendLine("<body style='padding:6px;'><div style='border:1px solid #808080;padding:5px;border-radius:5px;margin:5px;' >")
                mailStr.AppendLine("<table style='font-family: Arial, Helvetica, sans-serif;font-size:15px;color: #131212;'>")
                mailStr.AppendLine("<tr><td>Amazon item price analysis report</td></tr><tr><td>" & sendDate & "</td></tr></table><br />")
                mailStr.AppendLine("<table cellspacing='0' cellpadding='7' width='1325px'  style='border: 1px solid #808080;font-family: Arial, Helvetica, sans-serif;font-size: 13px;color: #131212;'>")

                If exLog = 1 Then

                    For Each dr As DataRow In DtMail.Rows
                        mailStr.AppendLine("<tr><td width='380' style='font-size:12px;'>" & objval.ToString(dr("product_name")) & "</td><td width='70' style='background-color:#ddd;'></td>" &
                                            "<td width='60' style='background-color:#ddd;font-size:12px;font-weight:bold;'>Amazon $</td> " &
                                            "<td width='60' style='background-color:#ddd;font-size:12px;font-weight:bold;'>Target $</td>" &
                                            "<td width='60' style='background-color:#ddd;font-size:12px;font-weight:bold;'>Am.Diff%</td>" &
                                            "<td width='60' style='background-color:#ddd;font-size:12px;font-weight:bold;'>Ebay$</td>" &
                                            "<td width='80' style='background-color:#ddd;font-size:12px;font-weight:bold;'>Ebay.Comp$</td>" &
                                            "<td width='60' style='background-color:#ddd;font-size:12px;font-weight:bold;'>Ebay.Avg%</td>" &
                                            "<td width='60' style='background-color:#ddd;font-size:12px;font-weight:bold;'>TradeIN</td>" &
                                            "<td style='background-color:#ddd;font-size:12px;font-weight:bold;'>Rank</td></tr>")

                        mailStr.AppendLine("<tr><td style='font-weight:bold;'>" & objval.ToString(dr("product_category")) & "</td><td style='background-color:#ddd;font-size:12px;font-weight:bold;'>New Price</td>" &
                                           "<td style='background-color:#f9eee7;'>$" & objval.ToString(dr("live_newprice")) & "</td>" &
                                           "<td " & IIf(objval.ToInt32(dr("isNpriceHigh")) = 1, "style='font-weight:bold;color:#f21717;'", "") & ">$" & objval.ToString(dr("target_price")) & "</td>" &
                                           "<td >" & objval.ToString(dr("new_per")) & "%</td>" &
                                           "<td style='background-color:#e2f9ef;'>$" & objval.ToString(dr("ebay_live_new_price")) & "</td>" &
                                           "<td>$" & objval.ToString(dr("ebay_new_comprice_avg")) & "</td>" &
                                           "<td>" & objval.ToString(dr("ebay_new_comprice_per")) & "%</td>" &
                                           "<td>" & IIf(objval.ToInt32(dr("trade_in_price")) > 0, "$" & objval.ToString(dr("trade_in_price")), "") & "</td>" &
                                           "<td>" & objval.ToString(dr("rank")) & "</td></tr>")

                        mailStr.AppendLine("<tr><td><b>ASIN | UPC : </b> " & objval.ToString(dr("product_asin")) & " | " & objval.ToString(dr("upc")) & "</td> " &
                                           "<td style='background-color:#ddd;font-size:12px;font-weight:bold;'>Used Price</td>" &
                                           "<td style='background-color:#f9eee7;'>$" & objval.ToString(dr("live_usedprice")) & "</td><td " & IIf(objval.ToInt32(dr("isUpriceHigh")) = 1, "style='font-weight:bold;color:#f21717;'", "") & ">$" & objval.ToString(dr("target_used_price")) & "</td>" &
                                           "<td>" & objval.ToString(dr("used_per")) & "%</td><td style='background-color:#e2f9ef;'>$" & objval.ToString(dr("ebay_live_used_price")) & "</td><td>$" & objval.ToString(dr("ebay_used_comprice_avg")) & "</td><td>" & objval.ToString(dr("ebay_new_comprice_per")) & "%</td><td></td>" &
                                           "<td style='font-size:11px;'><a href='" & objval.ToString(dr("product_url")) & "'>Go to Amazon</a> | ")
                        mailStr.AppendLine("<a href='http://www.ebay.com/sch/i.html?LH_Complete=1&_nkw=" & objval.ToString(dr("upc")) & "&rt=nc&LH_BIN=1'>Ebay Completed List</a> | ")
                        mailStr.AppendLine("<a href='http://www.ebay.com/sch/i.html?_nkw=" & objval.ToString(dr("upc")) & "'>Ebay Active List</a> ")
                        mailStr.AppendLine("<tr><td colspan='10' style='border-top:1px solid #ddd;' ></td></tr>")
                    Next
                Else
                    glbErroLog.AppendLine("MAIL Sending Datatable error")
                End If

                mailStr.AppendLine("</table></div></body></html>")

                Mail.Body = objval.ToString(mailStr)
                SmtpServer.Send(Mail)
                Mail.Dispose()
                SmtpServer.Dispose()

            Catch ex As Exception
                Mail.Dispose()
                glbErroLog.AppendLine("MAIL Sending ERROR" & vbCrLf & ex.Message.ToString() & vbCrLf & "------------------------------------------------")
            End Try

            SplashScreenManager.CloseForm()
        End If

        pnlUpdating.Visible = False
        Try

            If Not Directory.Exists(Application.StartupPath & "\logs") Then
                Directory.CreateDirectory(Application.StartupPath & "\logs")
            End If
            Dim logfilname As String = "Log_" & DateTime.Now.ToString("dd MMM YYYY hhmmss tt") & ".txt"
            If File.Exists(Application.StartupPath & "\logs\" & logfilname) Then
                File.Delete(Application.StartupPath & "\logs\" & logfilname)
            End If

            Using sw As StreamWriter = File.CreateText(Application.StartupPath & "\logs\" & logfilname)
                sw.Write(Convert.ToString(glbErroLog))
            End Using
        Catch ex As Exception
            objval.MsgError(ex.Message.ToString(), "Error while Saving Logs")
        End Try
        If gblISWrongSettig = True Then
            objval.MsgStop("AWS settings seems invalid. Please re-check AWS Accesskey,Secretkey and SellerID in setting and trygain.", "Invalid AWS parameters")
            mnuStart.Enabled = True
            mnuUpdate.Enabled = True
            mnuPause.Enabled = False
            mnuStop.Enabled = False
            GblIsProcessStart = False
            mnuAWSScan.Enabled = True
            mnuMWSScan.Enabled = True
            Exit Sub
        End If

        If glbErroOnProcess = True Then
            mnuStart.Enabled = True
            mnuUpdate.Enabled = True
            mnuPause.Enabled = False
            GblIsProcessStart = False
            mnuAWSScan.Enabled = True
            mnuMWSScan.Enabled = True
            objval.MsgWarning("Monitoring Process has been stopped", "ERROR")
        End If
        If gblIsprocessByManual = True Then
            mnuStop.Enabled = False
            mnuStart.Enabled = True
            mnuPause.Enabled = False
            GblIsProcessStart = False
            mnuAWSScan.Enabled = True
            mnuMWSScan.Enabled = True
        End If
        mnuUpdate.Enabled = True

        gblisPausemode = False
        gblisPauseLASTiNDEX = 0
        gblisPauseSTATUS = "pause"
        mnuPause.ImageOptions.LargeImage = My.Resources.pause_button
        mnuPause.Caption = "Pause Monitoring"
        picPaused.Visible = False
        mnuPause.Enabled = False

        ReloadGrid()

        If GblIsScheularEnabled = True Then
            ResetSchedule()
        End If
    End Sub

    Private Sub repoKeepaLink_Click(sender As Object, e As EventArgs) Handles repoKeepaLink.Click
        Try
            Dim proid As String = objval.ToString(dgvGrid.GetRowCellValue(dgvGrid.FocusedRowHandle, "product_asin"))
            If proid.Length > 0 Then
                Process.Start("https://keepa.com/#!product/1-" & proid)
            Else
                objval.MsgWarning("Item URL does not exists.", "No URL Found")
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub mnuRefProductInfo_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnuRefProductInfo.ItemClick
        If HDMWSBackwork.IsBusy = False Then
            pnlUpdatingInfoMWS.Visible = True
            HDMWSBackwork.RunWorkerAsync()
        End If
    End Sub

    Private Sub HDMWSBackwork_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles HDMWSBackwork.DoWork
        gblISWrongSettig = False
        Try
            Dim exLog As Integer = 0
            Dim dtcnt As DataTable = dbOperation.GetDataTable("select product_asin,target_price,target_used_price,upc,notes from db_products", exLog)
            _totalProcessTotalMWSInfoCnt = 0
            _totalProcessSuccMWSinfo = 0

            If dtcnt.Rows.Count > 0 Then
                _totalProcessTotalMWSInfoCnt = dtcnt.Rows.Count
            Else
                Exit Sub
            End If
            HDMWSBackwork.ReportProgress(1)
            Dim _lastIndex As Integer = 0
            If exLog = 1 Then
                glbErroLog.Clear()
                HDMWSBackwork.ReportProgress(1)

NextLotofItems:

                Dim ItemCntToSearch As Integer = 0

                Dim DtRead As DataTable = dbOperation.GetDataTable("select product_id,product_asin from db_products where product_id >" & _lastIndex & " order by product_id LIMIT 5;", exLog)
                Dim _produtArr1 As New List(Of String)()

                If Not DtRead Is Nothing Then
                    ItemCntToSearch = DtRead.Rows.Count
                Else
                    GoTo ProcessCompleted
                End If

                If ItemCntToSearch = 0 Then
                    GoTo ProcessCompleted
                End If

                For rwcnt As Integer = 0 To DtRead.Rows.Count - 1
                    _produtArr1.Add(DtRead.Rows(rwcnt)("product_asin"))

                    If rwcnt = DtRead.Rows.Count - 1 Then
                        _lastIndex = objval.ToInt32(DtRead.Rows(rwcnt)("product_id"))
                    End If
                Next
                Dim _searchItems1 As String = String.Join(",", _produtArr1)
                Dim TryCnt As Integer = 1
tryAgain:
                Try

                    'Thread.Sleep(100)

                    Dim ab As New List(Of RetProductInfo)()
                    ab = MWSProductsMain.ProductInfo(_produtArr1)

                    If ab Is Nothing Then
                        If TryCnt < 4 Then
                            TryCnt = TryCnt + 1
                        End If
                        System.Threading.Thread.Sleep(1000)
                        GoTo tryAgain
                    End If


                    For Each lstVar As RetProductInfo In ab

                        Dim con As New SQLiteConnection(DBconnStr)
                        Dim cmd As New SQLiteCommand()
                        cmd.CommandText = "update db_products set product_name=@product_name,product_url=@product_url,rank=@rank,product_category=@product_category,num_of_pages=@num_of_pages,num_of_disc=@num_of_disc  where product_asin=@product_asin"
                        cmd.Connection = con
                        cmd.CommandType = CommandType.Text

                        cmd.Parameters.AddWithValue("@product_name", lstVar.itemname)
                        cmd.Parameters.AddWithValue("@rank", lstVar.salerank)
                        cmd.Parameters.AddWithValue("@product_category", lstVar.category)
                        cmd.Parameters.AddWithValue("@num_of_pages", lstVar.numberOfPages)
                        cmd.Parameters.AddWithValue("@num_of_disc", lstVar.numberOfDisc)
                        'cmd.Parameters.AddWithValue("@live_price", objval.ToDouble(_iLivePrice.InnerText.Replace("$", "")))
                        'cmd.Parameters.AddWithValue("@live_newprice", newPrice)
                        'cmd.Parameters.AddWithValue("@live_usedprice", usedPrice)
                        cmd.Parameters.AddWithValue("@product_asin", lstVar.product_code)
                        cmd.Parameters.AddWithValue("@product_url", "https://www.amazon.com/dp/" & lstVar.product_code)
                        'cmd.Parameters.AddWithValue("@last_updated", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture))

                        con.Open()



                        Try
                            Dim ddssdd As Integer = cmd.ExecuteNonQuery()
                            _totalProcessSuccMWSinfo = _totalProcessSuccMWSinfo + 1
                            HDMWSBackwork.ReportProgress(1)

                        Catch ex As Exception
                            Dim xsddd As Integer
                            xsddd = 0
                        End Try
                        cmd.Parameters.Clear()
                        con.Close()
                        cmd.Dispose()
                        con.Dispose()

                    Next

                    GoTo NextLotofItems

                Catch ex As Exception
                    If ex.Message.ToString.Contains("(403)") OrElse ex.Message.ToString.Contains("(400)") Then
                        gblISWrongSettig = True
                        GoTo ProcessCompleted
                    Else
                        glbErroLog.AppendLine(vbCrLf & ex.Message.ToString() & vbCrLf & "------------------------------------------------")
                        GoTo tryAgain
                    End If
                    'exLog = ex.Message.ToString() + vbLf + requestUrl
                End Try

                GoTo NextLotofItems
ProcessCompleted:
            End If

        Catch ex As Exception
            objval.MsgWarning(ex.Message.ToString(), "Error")
        End Try
    End Sub

    Private Sub HDMWSBackwork_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles HDMWSBackwork.ProgressChanged
        lblProcessingcntMWS.Text = _totalProcessSuccMWSinfo & "/" & _totalProcessTotalMWSInfoCnt
    End Sub

    Private Sub HDMWSBackwork_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles HDMWSBackwork.RunWorkerCompleted
        pnlUpdatingInfoMWS.Visible = False
    End Sub


    'Private Sub ribbonControl1_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles RibbonControl.Paint
    '    'If IsCurrentAbout Then
    '    '    Return
    '    'End If
    '    Dim ribbonViewInfo As DevExpress.XtraBars.Ribbon.ViewInfo.RibbonViewInfo = RibbonControl.ViewInfo
    '    If ribbonViewInfo Is Nothing Then
    '        Return
    '    End If
    '    Dim panelViewInfo As DevExpress.XtraBars.Ribbon.ViewInfo.RibbonPanelViewInfo = ribbonViewInfo.Panel
    '    If panelViewInfo Is Nothing Then
    '        Return
    '    End If
    '    Dim bounds As Rectangle = panelViewInfo.Bounds
    '    Dim minX As Integer = bounds.X
    '    Dim groups As DevExpress.XtraBars.Ribbon.ViewInfo.RibbonPageGroupViewInfoCollection = panelViewInfo.Groups
    '    If groups Is Nothing Then
    '        Return
    '    End If
    '    If groups.Count > 0 Then
    '        minX = groups(groups.Count - 1).Bounds.Right
    '    End If
    '    Dim image As Image = DevExpress.Utils.Frames.ApplicationCaption8_1.GetImageLogoEx(LookAndFeel)
    '    If bounds.Height < image.Height Then
    '        Return
    '    End If
    '    Dim offset As Integer = (bounds.Height - image.Height) / 2
    '    Dim width As Integer = image.Width + 15
    '    bounds.X = bounds.Width - width
    '    If bounds.X < minX Then
    '        Return
    '    End If
    '    bounds.Width = width
    '    bounds.Y += offset
    '    bounds.Height = image.Height
    '    e.Graphics.DrawImage(image, bounds.Location)
    'End Sub


    Private Sub cmbFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFilter.SelectedIndexChanged
        ReloadGrid()
    End Sub

    Private Sub btnSetActive_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles btnSetActive.LinkClicked
        setActive(True)
    End Sub

    Public Sub setActive(ByVal ActiveStatus As Boolean)
        If ActiveStatus = True Then
            If objval.MsgConfirmYesNo("Are you sure you want to set selected Items as ACTIVE?", "Confirm") = Windows.Forms.DialogResult.No Then
                Exit Sub
            End If
        Else
            If objval.MsgConfirmYesNo("Are you sure you want to set selected Items as INACTIVE?", "Confirm") = Windows.Forms.DialogResult.No Then
                Exit Sub
            End If
        End If


        Dim arr As String() = GetStockSelection()
        Dim itemsStr As String = String.Join(",", arr)
        If itemsStr.Trim.Length = 0 Then
            objval.MsgWarning("Please Select item to change item status from monitoring.", "Selection Required")
            Exit Sub
        End If

        Dim cmd As New SQLiteCommand
        If ActiveStatus = True Then
            cmd.CommandText = "update db_products set is_active='Yes' where product_id in (" & itemsStr & ")"
        Else
            cmd.CommandText = "update db_products set is_active='No' where product_id in (" & itemsStr & ")"
        End If

        If dbOperation.ExecuteQuery(cmd) > 0 Then
            objval.MsgSuccess("selected Item's status updated successfully.", "Success")
            _totalProcessCnt = _totalProcessCnt - arr.Length
            ReloadGrid()
            selectionStock.ClearSelection()
        End If
        
    End Sub

    Private Sub btnSetInActive_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles btnSetInActive.LinkClicked
        setActive(False)
    End Sub

    Public Sub ScheduleTimer()
        If GblIsScheularEnabled = True Then
            timerSchedule.Start()
            glbNextSchedule = DateTime.Now
            ResetSchedule()
        Else
            lblNextScan.Caption = ""
            lblNextScanBy.Caption = ""
        End If

    End Sub

    Public Sub ResetSchedule()
        Dim TimeStr As String = GblScheularTime
        Dim _arrMainItems() As String = TimeStr.Split(",")
        'Dim _arrDate() As DateTime = Nothing
        Dim varDate As DateTime = DateTime.Now
rechek:
        Dim _arrDate As New List(Of DateTime)
        For Each arrItem As String In _arrMainItems
            Dim _arrSub() As String = arrItem.Split(":")
            If _arrSub.Length <> 2 Then

            Else
                If objval.ToInt32(_arrSub(0)) = 0 OrElse objval.ToInt32(_arrSub(0)) > 24 Then
                    Continue For
                End If

                If objval.ToInt32(_arrSub(1)) > 59 Then
                    Continue For
                End If

                If _arrSub(1).Length <> 2 Then
                    Continue For
                ElseIf objval.ToInt32(_arrSub(1)) > 59 Then
                    Continue For
                End If

                Dim tempDt As New DateTime(varDate.Year, varDate.Month, varDate.Day, objval.ToInt32(_arrSub(0)), objval.ToInt32(_arrSub(1)), 0)
                _arrDate.Add(tempDt)
            End If
        Next

        _arrDate.Sort()
        Dim timeSet As Boolean = False
        For Each item As DateTime In _arrDate
            If item <= glbNextSchedule Then
                Continue For
            Else
                glbNextSchedule = item
                timeSet = True
                Exit For
            End If
        Next

        If timeSet = False Then
            varDate = varDate.AddDays(1)
            Dim tempDt As New DateTime(varDate.Year, varDate.Month, varDate.Day, 0, 0, 1)
            varDate = tempDt
            GoTo rechek
        End If
        lblNextScan.Caption = "Next Scan : " & glbNextSchedule.ToString("dd MMM yyyy hh:mm tt")
    End Sub

    Private Sub timerSchedule_Tick(sender As Object, e As EventArgs) Handles timerSchedule.Tick
        lblNextScanBy.Caption = " (" & DateDiff(DateInterval.Minute, DateTime.Now, glbNextSchedule) & " minute left for next scan )"

        Dim diff As Int32 = DateDiff(DateInterval.Second, DateTime.Now, glbNextSchedule)

        If diff > 0 And diff < 5 Then
            If MWSBackWork.IsBusy = False AndAlso HDbackWork.IsBusy = False Then
                gblHashMailSent.Clear()
                If mnuAWSScan.Checked = True And GblAwsSellerID = "" Then
                    objval.MsgWarning("AWS Setting is not ready. Please complete settings in setting menu", "Warning")
                    Exit Sub
                ElseIf mnuMWSScan.Checked = True And GblMWSSellerID = "" Then
                    objval.MsgWarning("MWS Setting is not ready. Please complete settings in setting menu", "Warning")
                    Exit Sub
                End If

                gblIsprocessByManual = True
                GblIsProcessStart = True
                pnlUpdating.Visible = True
                glbErroOnProcess = False
                mnuUpdate.Enabled = False
                mnuStart.Enabled = False
                mnuStop.Enabled = True
                mnuPause.Enabled = False

                lblProcessType.Text = "Updateing Price (Manual)"
                If mnuAWSScan.Checked = True Then
                    If HDbackWork.IsBusy = False Then
                        HDbackWork.RunWorkerAsync()
                    End If
                Else
                    If MWSBackWork.IsBusy = False Then
                        MWSBackWork.RunWorkerAsync()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub mnuPause_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles mnuPause.ItemClick
        If mnuStart.Enabled = True Then
            Exit Sub
        End If

        If gblisPauseSTATUS = "pause" Then
            mnuPause.ImageOptions.LargeImage = My.Resources.pause_play_button
            gblisPausemode = True
            gblisPauseSTATUS = "play"
            picPaused.Visible = True
            mnuPause.Caption = "Resume Monitoring"
            autoTimer.Stop()
        Else
            If MWSBackWork.IsBusy = False Then
                MWSBackWork.RunWorkerAsync()
            Else
                objval.MsgWarning("Please wait.. completing previous action", "Work in progress")
                Exit Sub
            End If
            autoTimer.Start()
            mnuPause.ImageOptions.LargeImage = My.Resources.pause_button
            'gblisPausemode = False
            gblisPauseSTATUS = "pause"
            picPaused.Visible = False
            mnuPause.Caption = "Pause Monitoring"

        End If
    End Sub


End Class
