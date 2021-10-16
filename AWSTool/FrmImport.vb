Imports System.IO
Imports DevExpress.Spreadsheet
Imports DevExpress.XtraSplashScreen
Imports System.Text
Imports DevExpress.Spreadsheet.Export
Imports System.Globalization

Public Class FrmImport
    Dim objval As New Oozee
    Dim glbXlsUploadedError As Boolean = False
    Dim glbXlsDT As DataTable
    Dim glbErrType As Integer = 0

    Private Sub FrmImport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SplashScreenManager.ShowForm(GetType(ozLoading))

        Dim worksheet As Worksheet = HDSpreadSheet.Document.Worksheets.ActiveWorksheet

        worksheet.Cells("A1").Value = "ASIN"
        worksheet.Cells("B1").Value = "Price"
        worksheet.Cells("C1").Value = "UsedPrice"
        worksheet.Cells("D1").Value = "UPC"
        worksheet.Cells("E1").Value = "Notes"

        worksheet.Columns("D").NumberFormat = "@"
        worksheet.Columns("A").NumberFormat = "@"


        worksheet.Cells("A1").ColumnWidth = 350
        worksheet.Cells("B1").ColumnWidth = 250
        worksheet.Cells("C1").ColumnWidth = 250
        worksheet.Cells("D1").ColumnWidth = 350
        worksheet.Cells("E1").ColumnWidth = 900

        worksheet.Rows(0).FillColor = Color.LightGray
        worksheet.Rows(0).Font.Bold = True

        SplashScreenManager.CloseForm()


    End Sub

    Private Sub btnSelectXls_Click(sender As Object, e As EventArgs) Handles btnSelectXls.Click
        Try
            Dim SourcePath As String = ""
            Dim OpenFileDialog As New OpenFileDialog()
            OpenFileDialog.Filter = "Excel files (*.xls,*.xlsx)|*.xls;*.xlsx|All files (*.*)|*.*"
            If OpenFileDialog.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                SourcePath = OpenFileDialog.FileName
            Else
                Return
            End If

            OpenFileDialog.Dispose()
            OpenFileDialog = Nothing

            If SourcePath.Length = 0 Then
                objval.MsgError("File Path Not Found", "Error")
                Return
            End If

            If File.Exists(SourcePath) = False Then
                objval.MsgError("File Is Not Exists To The Path", "Error")
                Return
            End If

            Dim filename As String = Path.GetFileName(SourcePath)
            Dim extension As String = Path.GetExtension(SourcePath)

            If extension <> ".xls" AndAlso extension <> ".xlsx" AndAlso extension <> ".csv" Then
                objval.MsgWarning("Please select .xls or .xlsx file only for Item Import", "Error")
                Return
            End If

            Dim DestinationPath As String = System.Windows.Forms.Application.StartupPath + "\xls\" + filename
            Try


                If Not Directory.Exists(System.Windows.Forms.Application.StartupPath + "\xls") Then
                    Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + "\xls")
                End If

                If System.IO.File.Exists(DestinationPath) Then
                    System.IO.File.Delete(DestinationPath)
                End If
                System.IO.File.Copy(SourcePath, DestinationPath)
            Catch ex As Exception
                objval.MsgError(Convert.ToString(ex.Message), "Error Occured while temp Directory")
                Exit Sub
            End Try

            SplashScreenManager.ShowForm(GetType(ozLoading))

            Using stream As New FileStream(DestinationPath, FileMode.Open)
                If extension = ".xls" Then
                    HDSpreadSheet.LoadDocument(stream, DocumentFormat.Xls)
                ElseIf extension = ".xlsx" Then
                    HDSpreadSheet.LoadDocument(stream, DocumentFormat.Xlsx)
                ElseIf extension = ".csv" Then
                    HDSpreadSheet.LoadDocument(stream, DocumentFormat.Csv)
                Else
                    SplashScreenManager.CloseForm()
                    objval.MsgWarning("Invalid FileFormat found.", "Invalid File")
                    Exit Sub
                End If
            End Using

            SplashScreenManager.CloseForm()


            'Dim excelConnectionString As String = ""
            'If extension = ".xls" Then
            '    excelConnectionString = (Convert.ToString("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=") & DestinationPath) + "; Extended Properties='Excel 8.0;Mode=Read;ReadOnly=true;HDR=YES;IMEX=1;ImportMixedTypes=Text'"
            'ElseIf extension = ".xlsx" Then
            '    excelConnectionString = (Convert.ToString("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=") & DestinationPath) + "; Extended Properties='Excel 12.0;Mode=Read;ReadOnly=true;HDR=YES;IMEX=1;ImportMixedTypes=Text'"
            'End If

            'Using cn As New OleDbConnection(excelConnectionString)
            '    cn.Open()

            '    Dim dbSchema As DataTable = cn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)
            '    If dbSchema Is Nothing OrElse dbSchema.Rows.Count < 1 Then
            '        objval.MsgWarning("Exception on reading excel file.", "Exception")
            '        Exit Sub
            '        ''Throw New Exception("Error: Could not determine the name of the first worksheet.")
            '    End If

            '    Dim WorkSheetName As String = dbSchema.Rows(0)("TABLE_NAME").ToString()

            '    Dim da As New OleDbDataAdapter((Convert.ToString("SELECT * FROM [") & WorkSheetName) + "]", cn)
            '    Dim dt As New DataTable(WorkSheetName)

            '    da.Fill(dt)
            'ASIN ,Price,UsedPrice
            '    If dt.Rows.Count > 0 Then
            '        If dt.Columns.Contains("ASIN") And dt.Columns.Contains("Price") And dt.Columns.Contains("UsedPrice") Then
            '            glbXlsTotal = dt.Rows.Count
            '            glbXlsUploaded = 0
            '            glbXlsDT = dt
            '            If HDBackImportXLS.IsBusy = False Then
            '                pnlLoadingImport.Visible = True
            '                pnlLoadingImport.Location = New System.Drawing.Point((Me.Width / 2 - pnlLoadingImport.Width / 2), (Me.Height / 2) - (pnlLoadingImport.Height / 2) - 100)
            '                HDBackImportXLS.RunWorkerAsync()
            '            End If

            '        Else
            '            objval.MsgWarning("'ASIN' or 'Price' or 'UsedPrice' column not found in excel." & vbCrLf & "No data imported", "Import Item Failed")
            '            Exit Sub
            '        End If
            '    End If

            'End Using
        Catch ex As Exception
            objval.MsgError(Convert.ToString(ex.Message), "Error Occured on file selection")
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        glbXlsUploadedError = False
        glbErrType = 0
        pnlLoadingImport.Visible = True
        HDBackImportXLS.RunWorkerAsync()

    End Sub

    Private Sub HDBackImportXLS_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles HDBackImportXLS.DoWork
        glbXlsUploadedError = False

        Try

            Dim worksheet As Worksheet = HDSpreadSheet.Document.Worksheets.ActiveWorksheet
            Dim range As Range = worksheet.GetDataRange ' Selection
            Dim rangeHasHeaders As Boolean = True
            'gblValidatedSheet = HDSpreadSheet.ActiveWorksheet.Name
            ' Create a data table with column names obtained from the first row in a range if it has headers.
            ' Column data types are obtained from cell value types of cells in the first data row of the worksheet range.
            glbXlsDT = worksheet.CreateDataTable(range, rangeHasHeaders)

            'Validate cell value types. If cell value types in a column are different, the column values are exported as text.
            For col As Integer = 0 To range.ColumnCount - 1
                Dim cellType As CellValueType = range(0, col).Value.Type
                For r As Integer = 1 To range.RowCount - 1
                    If cellType <> range(r, col).Value.Type Then
                        glbXlsDT.Columns(col).DataType = GetType(String)
                        Exit For
                    End If
                Next r
            Next col

            ' Create the exporter that obtains data from the specified range, 
            ' skips the header row (if required) and populates the previously created data table. 
            Dim exporter As DataTableExporter = worksheet.CreateDataTableExporter(range, glbXlsDT, rangeHasHeaders)
            ' Handle value conversion errors.
            AddHandler exporter.CellValueConversionError, AddressOf exporter_CellValueConversionError

            ' Perform the export.
            exporter.Export()

        Catch ex As Exception
            glbXlsUploadedError = True
            Exit Sub
        End Try

        If glbXlsDT Is Nothing Then
            glbXlsUploadedError = True
            Exit Sub
        Else
            'ASIN ,Price,UsedPrice
            'If glbXlsDT.Rows.Count = 1 Then
            '    glbXlsUploadedError = True
            '    glbErrType = 2
            '    Exit Sub
            'End If

            For Each dc As DataColumn In glbXlsDT.Columns
                dc.ColumnName = dc.ColumnName.ToUpper
            Next


            If glbXlsDT.Columns.Contains("ASIN") AndAlso glbXlsDT.Columns.Contains("PRICE") AndAlso glbXlsDT.Columns.Contains("USEDPRICE") Then
                HDBackImportXLS.ReportProgress(1)
                Dim dt As DataTable = Nothing

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
                            If glbXlsDT.Columns.Contains("NOTES") Then
                                If glbXlsDT.Columns.Contains("UPC") Then
                                    QryStr.AppendLine("update db_products set last_price_updated='" & DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture) & "',target_price=" & Math.Round(objval.ToDouble(dr("PRICE")), 2) & ",target_used_price=" & Math.Round(objval.ToDouble(dr("USEDPRICE")), 2) & ",notes='" & Convert.ToString(dr("NOTES")).Trim.Replace("'", "") & "',upc='" & Convert.ToString(dr("UPC")).Trim.Replace("'", "") & "' where product_asin='" & Convert.ToString(dr("ASIN")).Trim.Replace("'", "") & "';")
                                Else
                                    QryStr.AppendLine("update db_products set last_price_updated='" & DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture) & "',target_price=" & Math.Round(objval.ToDouble(dr("PRICE")), 2) & ",target_used_price=" & Math.Round(objval.ToDouble(dr("USEDPRICE")), 2) & ",notes='" & Convert.ToString(dr("NOTES")).Trim.Replace("'", "") & "' where product_asin='" & Convert.ToString(dr("ASIN")).Trim.Replace("'", "") & "';")
                                End If
                            Else
                                If glbXlsDT.Columns.Contains("UPC") Then
                                    QryStr.AppendLine("update db_products set last_price_updated='" & DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture) & "',target_price=" & Math.Round(objval.ToDouble(dr("PRICE")), 2) & ",target_used_price=" & Math.Round(objval.ToDouble(dr("USEDPRICE")), 2) & ",upc='" & Convert.ToString(dr("UPC")).Trim.Replace("'", "") & "' where product_asin='" & Convert.ToString(dr("ASIN")).Trim.Replace("'", "") & "';")
                                Else
                                    QryStr.AppendLine("update db_products set last_price_updated='" & DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture) & "',target_price=" & Math.Round(objval.ToDouble(dr("PRICE")), 2) & ",target_used_price=" & Math.Round(objval.ToDouble(dr("USEDPRICE")), 2) & " where product_asin='" & Convert.ToString(dr("ASIN")).Trim.Replace("'", "") & "';")
                                End If
                            End If

                        Else
                            If glbXlsDT.Columns.Contains("NOTES") Then
                                If glbXlsDT.Columns.Contains("UPC") Then
                                    QryStr.AppendLine("insert into db_products(product_asin,target_price,target_used_price,notes,upc,is_active,last_price_updated,is_watch) values ('" & Convert.ToString(dr("ASIN")).Trim.Replace("'", "") & "'," & Math.Round(objval.ToDouble(dr("PRICE")), 2) & "," & Math.Round(objval.ToDouble(dr("USEDPRICE")), 2) & ",'" & objval.ToString(dr("NOTES")).Replace("'", "") & "','" & objval.ToString(dr("UPC")).Replace("'", "") & "','Yes','" & DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture) & "',0);")
                                Else
                                    QryStr.AppendLine("insert into db_products(product_asin,target_price,target_used_price,notes,upc,is_active,last_price_updated,is_watch) values ('" & Convert.ToString(dr("ASIN")).Trim.Replace("'", "") & "'," & Math.Round(objval.ToDouble(dr("PRICE")), 2) & "," & Math.Round(objval.ToDouble(dr("USEDPRICE")), 2) & "," & objval.ToString(dr("NOTES")).Replace("'", "") & ",'','Yes','" & DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture) & "',0);")
                                End If
                            Else
                                If glbXlsDT.Columns.Contains("UPC") Then
                                    QryStr.AppendLine("insert into db_products(product_asin,target_price,target_used_price,notes,upc,is_active,last_price_updated,is_watch) values ('" & Convert.ToString(dr("ASIN")).Trim.Replace("'", "") & "'," & Math.Round(objval.ToDouble(dr("PRICE")), 2) & "," & Math.Round(objval.ToDouble(dr("USEDPRICE")), 2) & ",'','" & objval.ToString(dr("UPC")).Replace("'", "") & "','Yes','" & DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture) & "',0);")
                                Else
                                    QryStr.AppendLine("insert into db_products(product_asin,target_price,target_used_price,notes,upc,is_active,last_price_updated,is_watch) values ('" & Convert.ToString(dr("ASIN")).Trim.Replace("'", "") & "'," & Math.Round(objval.ToDouble(dr("PRICE")), 2) & "," & Math.Round(objval.ToDouble(dr("USEDPRICE")), 2) & ",'','','Yes','" & DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture) & "',0);")
                                End If
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
            Else
                glbErrType = 1
                glbXlsUploadedError = True
                ' column does not match
            End If
        End If
    End Sub

    Private Sub HDBackImportXLS_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles HDBackImportXLS.RunWorkerCompleted
        pnlLoadingImport.Visible = False
        If glbXlsUploadedError = True Then
            If glbErrType = 1 Then
                objval.MsgError("Excel Columns did not match. File upload failed.", "Invalid File")
            ElseIf glbErrType = 2 Then
                objval.MsgWarning("No Date found to upload. Please Enter items details.", "No Data Found")
            Else
                objval.MsgError("Some Error occured while Import file from excel." & vbCrLf & "Please check your excel > ASIN,Price,Usedprice columns are compulsory and notes remark which is optional.", "Error On Import")
            End If
        Else
            objval.MsgSuccess("Data import from Excel Successfully completed." & vbCrLf & "Total Items : " & glbXlsDT.Rows.Count, "Import Summary")
        End If
    End Sub

    Private Sub exporter_CellValueConversionError(ByVal sender As Object, ByVal e As CellValueConversionErrorEventArgs)
        'MessageBox.Show("Error in cell " & e.Cell.GetReferenceA1())
        '  gblErr = gblErr + e.Cell.GetReferenceA1() & vbCrLf
        e.DataTableValue = Nothing
        e.Action = DataTableExporterAction.Continue
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        HDSpreadSheet.CreateNewDocument()
        glbXlsDT = Nothing
        glbXlsUploadedError = False


        Dim worksheet As Worksheet = HDSpreadSheet.Document.Worksheets.ActiveWorksheet

        worksheet.Cells("A1").Value = "ASIN"
        worksheet.Cells("B1").Value = "Price"
        worksheet.Cells("C1").Value = "UsedPrice"
        worksheet.Cells("D1").Value = "UPC"
        worksheet.Cells("E1").Value = "Notes"

        worksheet.Columns("D").NumberFormat = "@"
        worksheet.Columns("A").NumberFormat = "@"

        worksheet.Cells("A1").ColumnWidth = 350
        worksheet.Cells("B1").ColumnWidth = 250
        worksheet.Cells("C1").ColumnWidth = 250
        worksheet.Cells("D1").ColumnWidth = 350
        worksheet.Cells("E1").ColumnWidth = 900

        worksheet.Rows(0).FillColor = Color.LightGray
        worksheet.Rows(0).Font.Bold = True




    End Sub

End Class