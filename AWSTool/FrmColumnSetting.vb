Imports System.IO
Imports System.Xml

Public Class FrmColumnSetting
    Dim columnfilepath As String = Application.StartupPath & "\columnsetting.xml"
    Dim objval As New Oozee

    Dim gblColumnDT As DataTable = New DataTable()

    Private Sub FrmColumnSetting_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        gblColumnDT.Columns.Add("col_def", GetType(String))
        gblColumnDT.Columns.Add("col_cap", GetType(String))
        gblColumnDT.Columns.Add("col_field", GetType(String))
        gblColumnDT.Columns.Add("col_width", GetType(Int32))
        gblColumnDT.Columns.Add("col_order", GetType(Int32))
        gblColumnDT.Columns.Add("col_show", GetType(Boolean))

        If Not File.Exists(columnfilepath) Then
            File.Delete(columnfilepath)
            Dim doc As XDocument = XDocument.Parse(My.Resources.def_view)
            Dim points = doc.Descendants("Column")

            For Each current As XElement In points
                If Convert.ToString(current.Element("col_def").Value) = "YES" Then
                    gblColumnDT.Rows.Add(Convert.ToString(current.Element("col_def").Value), Convert.ToString(current.Element("col_cap").Value), Convert.ToString(current.Element("col_field").Value), Convert.ToString(current.Element("col_width").Value), Convert.ToString(current.Element("col_order").Value), True)
                Else
                    gblColumnDT.Rows.Add(Convert.ToString(current.Element("col_def").Value), Convert.ToString(current.Element("col_cap").Value), Convert.ToString(current.Element("col_field").Value), Convert.ToString(current.Element("col_width").Value), Convert.ToString(current.Element("col_order").Value), False)
                End If
            Next

            GridControl1.DataSource = gblColumnDT

        End If

    End Sub


    Public Sub CreateSmartLib(ByVal dtx As DataTable)
        If File.Exists(columnfilepath) Then
            File.Delete(columnfilepath)
        End If

        Dim settings As XmlWriterSettings = New XmlWriterSettings()
        settings.Indent = True
        Dim writer As XmlWriter = XmlWriter.Create("columnsetting.xml", settings)
        writer.WriteStartDocument()
        writer.WriteStartElement("table")

        For Each dr As DataRow In dtx.Rows
            writer.WriteStartElement("Column")
            writer.WriteElementString("rap", Convert.ToString(dr("colMRapfield")))
            writer.WriteElementString("xls", Convert.ToString(dr("colExcelField")))
            writer.WriteElementString("bak", Convert.ToString(dr("colMBackFieldName")))
            writer.WriteEndElement()
        Next

        writer.WriteEndDocument()
        writer.Flush()
        writer.Close()
        objval.MsgSuccess("Column view setting updated successfully", "Success")
        Me.Close()
    End Sub


End Class