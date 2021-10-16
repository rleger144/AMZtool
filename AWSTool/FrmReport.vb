Imports System.Text
Imports DevExpress.XtraGrid.Views.Grid

Public Class FrmReport
    Dim glbFinalGridDT As DataTable
    Dim objval As New Oozee

    Private Sub FrmReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ShowWaitForm()
        ReloadGrid()

    End Sub

    Public Sub ReloadGrid()
        ShowWaitForm()

        Dim qry As String = "select *,(case when target_price>0 and live_newprice>0 then ((live_newprice/target_price)*100)-100  else 0.00 end) as new_per,(case when target_used_price>0 and live_usedprice>0 then ((live_usedprice/target_used_price)*100)-100  else 0.00 end) as used_per,(case when target_price>0 and ebay_live_new_price>0 then ((ebay_live_new_price/target_price)*100)-100  else 0.00 end) as new_per_eb,(case when target_used_price>0 and ebay_live_used_price>0 then ((ebay_live_used_price/target_used_price)*100)-100  else 0.00 end) as used_per_eb  from db_products "

        If chk_Target_Price.Checked = True And chk_TargetUsedPrice.Checked = True Then

            qry = qry + " where  ((target_price > live_price and live_price > 0)  or  (target_price>live_newprice and live_newprice > 0 ) )  "
            qry = qry + " or ( live_usedprice > 0 and  target_used_price > live_usedprice)  and product_name<>''"
        ElseIf chk_Target_Price.Checked = True Then
            qry = qry + " where  (target_price > live_price and live_price > 0)  or  (target_price>live_newprice and live_newprice > 0 )  and product_name<>'' "
        ElseIf chk_TargetUsedPrice.Checked = True Then
            qry = qry + " where  live_usedprice > 0 and  target_used_price > live_usedprice and product_name<>''"
        Else
            qry = qry + " where  product_name<>''"
        End If


        Dim exLog As Integer = 0
        glbFinalGridDT = dbOperation.GetDataTable(qry, exLog)

        Dim glbKeepaDT As DataTable = dbOperation_keepa.GetDataTable("select product_asin,day_30_new,day_90_new,day_180_new,day_365_new,year_3_new,year_5_new,day_30_used,day_90_used,day_180_used,day_365_used,year_3_used, " &
                                                                    " year_5_used,day_30_salerank,day_90_salerank,day_180_salerank,day_365_salerank,year_3_salerank from keepa_price", exLog)


        Dim dtGrid As New DataTable

        Dim aa As New StringBuilder
        For Each dc As DataColumn In glbFinalGridDT.Columns
            dtGrid.Columns.Add(dc.Caption, dc.DataType)
        Next

        For Each dc2 As DataColumn In glbKeepaDT.Columns
            If dc2.Caption <> "product_asin" Then
                aa.AppendLine("DrIns(""" & dc2.Caption & """) =  objval.ToDouble(dtemp.Rows(0)(""" & dc2.Caption & """))")
                dtGrid.Columns.Add(dc2.Caption, dc2.DataType)
            End If

        Next

        Dim DrIns As DataRow
        For Each dr As DataRow In glbFinalGridDT.Rows
            DrIns = dtGrid.NewRow()
            DrIns("product_id") = dr("product_id")
            DrIns("product_name") = dr("product_name")
            DrIns("product_asin") = dr("product_asin")
            DrIns("product_category") = dr("product_category")
            DrIns("target_price") = dr("target_price")
            DrIns("live_price") = dr("live_price")
            DrIns("live_newprice") = dr("live_newprice")
            DrIns("live_usedprice") = dr("live_usedprice")
            DrIns("last_updated") = dr("last_updated")
            DrIns("target_used_price") = dr("target_used_price")
            DrIns("trade_in_price") = dr("trade_in_price")
            DrIns("product_url") = dr("product_url")
            DrIns("notes") = dr("notes")
            DrIns("upc") = dr("upc")
            DrIns("rank") = dr("rank")
            DrIns("ebay_live_new_price") = dr("ebay_live_new_price")
            DrIns("ebay_live_used_price") = dr("ebay_live_used_price")
            DrIns("ebay_new_comprice_avg") = dr("ebay_new_comprice_avg")
            DrIns("ebay_used_comprice_avg") = dr("ebay_used_comprice_avg")
            DrIns("ebay_new_comprice_per") = dr("ebay_new_comprice_per")
            DrIns("ebay_used_comprice_per") = dr("ebay_used_comprice_per")
            DrIns("live_newprice_pre") = dr("live_newprice_pre")
            DrIns("live_usedprice_pre") = dr("live_usedprice_pre")
            DrIns("num_of_pages") = dr("num_of_pages")
            DrIns("num_of_disc") = dr("num_of_disc")
            DrIns("num_of_review") = dr("num_of_review")
            DrIns("ebay_live_new_price_multi") = dr("ebay_live_new_price_multi")
            DrIns("ebay_live_used_price_multi") = dr("ebay_live_used_price_multi")
            DrIns("ebay_sold_new_price_high_multi") = dr("ebay_sold_new_price_high_multi")
            DrIns("ebay_sold_used_price_high_multi") = dr("ebay_sold_used_price_high_multi")
            DrIns("ebay_sold_new_price_lowest_multi") = dr("ebay_sold_new_price_lowest_multi")
            DrIns("ebay_sold_used_price_lowest_multi") = dr("ebay_sold_used_price_lowest_multi")
            DrIns("new_per") = dr("new_per")
            DrIns("used_per") = dr("used_per")
            DrIns("new_per_eb") = dr("new_per_eb")
            DrIns("used_per_eb") = dr("used_per_eb")

            Dim dv As New DataView(glbKeepaDT)
            dv.RowFilter = "product_asin = '" & Convert.ToString(dr("product_asin")) & "'"
            Dim dtemp As DataTable = dv.ToTable()
            If dtemp.Rows.Count = 1 Then
                DrIns("day_30_new") = objval.ToDouble(dtemp.Rows(0)("day_30_new"))
                DrIns("day_90_new") = objval.ToDouble(dtemp.Rows(0)("day_90_new"))
                DrIns("day_180_new") = objval.ToDouble(dtemp.Rows(0)("day_180_new"))
                DrIns("day_365_new") = objval.ToDouble(dtemp.Rows(0)("day_365_new"))
                DrIns("year_3_new") = objval.ToDouble(dtemp.Rows(0)("year_3_new"))
                DrIns("year_5_new") = objval.ToDouble(dtemp.Rows(0)("year_5_new"))
                DrIns("day_30_used") = objval.ToDouble(dtemp.Rows(0)("day_30_used"))
                DrIns("day_90_used") = objval.ToDouble(dtemp.Rows(0)("day_90_used"))
                DrIns("day_180_used") = objval.ToDouble(dtemp.Rows(0)("day_180_used"))
                DrIns("day_365_used") = objval.ToDouble(dtemp.Rows(0)("day_365_used"))
                DrIns("year_3_used") = objval.ToDouble(dtemp.Rows(0)("year_3_used"))
                DrIns("year_5_used") = objval.ToDouble(dtemp.Rows(0)("year_5_used"))
                DrIns("day_30_salerank") = objval.ToInt64(dtemp.Rows(0)("day_30_salerank"))
                DrIns("day_90_salerank") = objval.ToInt64(dtemp.Rows(0)("day_90_salerank"))
                DrIns("day_180_salerank") = objval.ToInt64(dtemp.Rows(0)("day_180_salerank"))
                DrIns("day_365_salerank") = objval.ToInt64(dtemp.Rows(0)("day_365_salerank"))
                DrIns("year_3_salerank") = objval.ToInt64(dtemp.Rows(0)("year_3_salerank"))
            End If


            dtGrid.Rows.Add(DrIns)
        Next

        If exLog = 1 Then
            GridConn.DataSource = dtGrid 'glbFinalGridDT
        End If

        CloseWaitForm()




    End Sub

    Public Sub ShowWaitForm()

        If (Not SplashScreenManager1.IsSplashFormVisible = True) Then
            'SplashScreenManager1.ShowWaitForm()
            'SplashScreenManager1.SetWaitFormCaption("Please Wait")
            'SplashScreenManager1.SetWaitFormDescription("Loading Data...")
        End If

    End Sub

    Public Sub CloseWaitForm()
        If SplashScreenManager1.IsSplashFormVisible Then
            SplashScreenManager1.CloseWaitForm()
        End If
    End Sub



    Private Sub btnAtoFit_Click(sender As Object, e As EventArgs) Handles btnAtoFit.Click
        dgvGrid.BestFitColumns()
    End Sub

    Private Sub btnRefreshGrid_Click(sender As Object, e As EventArgs) Handles btnRefreshGrid.Click
        ShowWaitForm()
        ReloadGrid()
    End Sub


    Private Sub dgvGrid_CustomColumnDisplayText(sender As Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs) Handles dgvGrid.CustomColumnDisplayText
        If e.Column.FieldName = "live_price" OrElse e.Column.FieldName = "live_newprice" OrElse e.Column.FieldName = "trade_in_price" OrElse e.Column.FieldName = "live_usedprice" OrElse e.Column.FieldName = "new_per" OrElse e.Column.FieldName = "used_per" Then
            If objval.ToDouble(e.Value) = 0 Then
                e.DisplayText = ""
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


End Class