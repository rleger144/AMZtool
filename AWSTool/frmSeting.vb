Imports System.Data.SQLite

Public Class frmSeting
    Dim dtMas As DataTable
    Dim dtDet As DataTable
    Dim objval As New Oozee
    Dim sqlCon As SQLiteConnection
    Dim da As SQLiteDataAdapter
    Dim cmb As SQLiteCommandBuilder

    Private Sub chkIsSendMail_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsSendMail.CheckedChanged
        If chkIsSendMail.Checked Then
            pnlMail.Enabled = True
            radSendmailType.Enabled = True
            'txtAlertMails.Enabled = True
            'txtSenderMailID.Enabled = True
            'txtSenderMailPWD.Enabled = True
            'txtSenderPORT.Enabled = True
            'txtSenderSMTP.Enabled = True
        Else
            pnlMail.Enabled = False
            radSendmailType.Enabled = False
            'txtAlertMails.Properties.ReadOnly = False
            'txtSenderMailID.Properties.ReadOnly = False
            'txtSenderMailPWD.Properties.ReadOnly = False
            'txtSenderPORT.Properties.ReadOnly = False
            'txtSenderSMTP.Properties.ReadOnly = False
        End If
    End Sub

    Private Sub frmSeting_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FILLData()
    End Sub

    Public Sub FILLData()
        Try

            'Dim st As Integer = 0

            Fill_ComboListAndData()

            txtAwsKey.Text = GblAwsAccessKey
            txtAwsScrretKey.Text = GblAwsSecretKey
            txtRefMin.Text = GblRefMinu
            txtAlertMails.Text = GblAlertMailIDs
            txtSenderMailID.Text = _GblSenderMailID
            txtSenderMailPWD.Text = _GblSenderMailPwd
            txtSenderPORT.Text = _GblSenderPortNo
            txtSenderSMTP.Text = _GblSenderSMTP
            radSendmailType.SelectedIndex = IIf(_GblSendMailonEachscan = True, 1, 0)
            txtAWSSellerID.Text = GblAwsSellerID

            'txtMWSAccesskey.Text = GblMWSAccessKey
            'txtMWSMarketPlaceID.Text = GblMWSMarketplaceID
            'txtMWSSellerID.Text = GblMWSSellerID
            'txtMWSSecretKey.Text = GblMWSSecretKey

            'ebAppID.Text = GblebAppID
            'ebDevID.Text = GblebDevID
            'ebCerID.Text = GblebCerID
            txtAPIIntAWSMin.Text = GblApiCalAWSInterval
            txtAPIIntMWSMin.Text = GblApiCalMWSInterval
            chkShowNotification.Checked = GblShowNotification
            'txtKeepaAccessKey.Text = GblKeepaAccessKey
            'chkEnabledEbay.Checked = GblEnabledEbay
            chkEnableSchedule.Checked = GblIsScheularEnabled
            txtAutoStarttime.Text = GblScheularTime
            'chkBrowserEbayACT.Checked = GblEnabledEbayBrowserACTIVE
            'chkBrowserComp.Checked = GblEnabledEbayBrowserCOMPLETE
            radNotification_Type.SelectedIndex = GblShowNotification_type

            If GblShowNotification = True Then
                radNotification_Type.Enabled = True
            Else
                radNotification_Type.Enabled = False
            End If

            If GblIsAlertMail = True Then
                chkIsSendMail.Checked = True
                pnlMail.Enabled = True
                radSendmailType.Enabled = True
            Else
                chkIsSendMail.Checked = False
                pnlMail.Enabled = False
                radSendmailType.Enabled = False
            End If

            'If chkEnabledEbay.Checked = True Then
            '    ebAppID.Enabled = True
            '    ebCerID.Enabled = True
            '    ebDevID.Enabled = True
            'Else
            '    ebAppID.Enabled = False
            '    ebCerID.Enabled = False
            '    ebDevID.Enabled = False
            'End If

            If chkEnableSchedule.Checked = True Then
                txtAutoStarttime.Enabled = True
            Else
                txtAutoStarttime.Enabled = False
            End If

            chkSenderSSL.Checked = _GblSenderIsSSL

        Catch ex As Exception
            objval.MsgError("Erro while loading data from Database.", "Exception")
        End Try
    End Sub

    Private Sub Fill_ComboListAndData(Optional ByVal vMarketId As Integer = 0)

        dtMas = New DataTable

        lue_MarketList.Properties.DataSource = Nothing

        dtMas = dbOperation.GetDataTable("Select MarketId, MarketName from db_MarketPlaceMas", 0).Copy()

        lue_MarketList.Properties.DataSource = dtMas
        lue_MarketList.Properties.DisplayMember = "MarketName"
        lue_MarketList.Properties.ValueMember = "MarketId"
        lue_MarketList.Properties.PopulateColumns()

        lue_MarketList.Properties.Columns("MarketId").Visible = False
        If dtMas.Rows.Count > 0 Then
            If dtMas.Compute("Count(MarketId)", "MarketId=" & vMarketId) = 1 Then
                lue_MarketList.EditValue = vMarketId
            Else
                lue_MarketList.EditValue = dtMas.Compute("Min(MarketId)", "")
            End If
        End If

        Fill_MWSGridData()

    End Sub

    Private Sub Fill_MWSGridData()

        dgv_MarketDet.DataSource = Nothing
        dtDet = New DataTable

        If da IsNot Nothing Then da = Nothing
        If cmb IsNot Nothing Then cmb = Nothing

        'dtDet = dbOperation.GetDataTable("Select MarketId,KeyId,KeyName,KeyValue,KeyDisplayOrder from db_MarketPlaceDet Where MarketId=" & IIf(lue_MarketList.EditValue IsNot Nothing, lue_MarketList.EditValue, 0) & " Order By KeyDisplayOrder", 0).Copy()
        sqlCon = New SQLiteConnection(dbOperation.DBconnStr)
        sqlCon.Open()

        da = New SQLiteDataAdapter("Select MarketId,KeyId,KeyName,KeyValue,KeyDisplayOrder from db_MarketPlaceDet Where MarketId=" & IIf(lue_MarketList.EditValue IsNot Nothing, lue_MarketList.EditValue, 0) & " Order By KeyDisplayOrder", sqlCon)
        cmb = New SQLiteCommandBuilder(da)
        da.Fill(dtDet)

        dtDet.Columns("KeyId").DefaultValue = 0
        dtDet.Columns("KeyDisplayOrder").DefaultValue = 0
        dtDet.Columns("MarketId").DefaultValue = lue_MarketList.EditValue

        dgv_MarketDet.DataSource = dtDet
        dgv_MarketDet.Columns("MarketId").Visible = False
        dgv_MarketDet.Columns("KeyId").Visible = False
        dgv_MarketDet.Columns("KeyDisplayOrder").Visible = False
        dgv_MarketDet.Columns("KeyName").Width = 180
        dgv_MarketDet.Columns("KeyValue").Width = 300
        'dtDet.Columns("KeyId").AutoIncrement = True
        'dtDet.Columns("KeyId").AutoIncrementSeed = -1
        'dtDet.Columns("KeyId").AutoIncrementStep = -1

    End Sub

    Private Sub LookUpEdit1_EditValueChanged(sender As Object, e As EventArgs) Handles lue_MarketList.EditValueChanged
        Fill_MWSGridData()
    End Sub

    Private Sub btmClose_Click(sender As Object, e As EventArgs) Handles btmClose.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Dim Errstring As String = ""

        If txtAWSSellerID.Text.Trim = "" Then
            Errstring += "AWS Seller ID" & vbCrLf
        End If

        If txtAwsKey.Text.Trim = "" Then
            Errstring += "AWS Access Key" & vbCrLf
        End If
        If txtAwsScrretKey.Text.Trim = "" Then
            Errstring += "AWS Secret Key" & vbCrLf
        End If

        If objval.ToInt32(txtRefMin.Text.Trim) = 0 Then
            Errstring += "Auto Refresh Minute" & vbCrLf
        End If
        'If ebAppID.Text.Trim() = "" Then
        '    Errstring += "eBay App ID" & vbCrLf
        'End If

        'If ebDevID.Text.Trim() = "" Then
        '    Errstring += "eBay Dev ID" & vbCrLf
        'End If
        'If ebCerID.Text.Trim() = "" Then
        '    Errstring += "eBay Cer ID" & vbCrLf
        'End If

        If chkEnableSchedule.Enabled = True Then
            If txtAlertMails.Text.Trim <> "" Then
                If IsValidTimeString() = False Then
                    Errstring += "Schedule monitoring time have invalid time value" & vbCrLf
                End If
            Else
                Errstring += "Please enter time for Scheduled monitoring" & vbCrLf
            End If
        End If

        'If chkEnabledEbay.Checked = True Then
        '    If ebAppID.Text.Trim = "" OrElse ebCerID.Text.Trim = "" OrElse ebDevID.Text.Trim = "" Then
        '        Errstring += "Please provide Ebay APIs parameter" & vbCrLf
        '    End If
        'End If

        If chkIsSendMail.Checked = True Then
            If txtAlertMails.Text.Trim = "" Then
                Errstring += "Enter Email id for alert mail" & vbCrLf
            End If
            If txtSenderMailID.Text.Trim = "" Then
                Errstring += "Sender Email ID" & vbCrLf
            End If
            If txtSenderMailPWD.Text.Trim = "" Then
                Errstring += "Sender Email PWD" & vbCrLf
            End If
            If txtSenderSMTP.Text.Trim = "" Then
                Errstring += "Sender SMTP" & vbCrLf
            End If
            If objval.ToInt32(txtSenderPORT.Text.Trim) = 0 OrElse txtSenderPORT.Text.Trim = "" Then
                Errstring += "Sender PORT" & vbCrLf
            End If
        End If

        If Errstring.Trim.Length > 0 Then
            objval.MsgWarning("Please enter below all required fields" & vbCrLf & Errstring, "Enter Required Fields")
            Exit Sub
        End If

        Dim cmd As New SQLiteCommand
        cmd.CommandText = " update db_settings " &
                            " set aws_sellerid=@aws_sellerid,access_key=@access_key,secret_key=@secret_key,refresh_minute=@refresh_minute,is_alert_mail=@is_alert_mail," &
                            " email_ids=@email_ids,sender_emailid=@sender_emailid,sender_emailpwd=@sender_emailpwd,send_mail_on_eachscan=@send_mail_on_eachscan," &
                            " sender_smtp=@sender_smtp,sender_port =@sender_port,	sender_chkssl = @sender_chkssl,is_show_notification=@is_show_notification,is_show_notification_type=@is_show_notification_type,api_call_interval=@api_call_intervalAWS,api_call_intervalMWS=@api_call_intervalMWS," &
                            " enable_schedule=@enable_schedule,schedule_time=@schedule_time where setting_id= 1"

        cmd.Parameters.AddWithValue("@send_mail_on_eachscan", radSendmailType.SelectedIndex)

        If chkEnableSchedule.Checked = True Then
            cmd.Parameters.AddWithValue("@enable_schedule", "1")
        Else
            cmd.Parameters.AddWithValue("@enable_schedule", "0")
        End If

        cmd.Parameters.AddWithValue("@schedule_time", txtAutoStarttime.Text.Trim)

        'If chkEnabledEbay.Checked = True Then
        '    cmd.Parameters.AddWithValue("@enable_ebay", "1")
        'Else
        '    cmd.Parameters.AddWithValue("@enable_ebay", "0")
        'End If

        'If chkBrowserComp.Checked = True Then
        '    cmd.Parameters.AddWithValue("@enable_ebay_comp_browser", "1")
        'Else
        '    cmd.Parameters.AddWithValue("@enable_ebay_comp_browser", "0")
        'End If

        'If chkBrowserEbayACT.Checked = True Then
        '    cmd.Parameters.AddWithValue("@enable_ebay_act_browser", "1")
        'Else
        '    cmd.Parameters.AddWithValue("@enable_ebay_act_browser", "0")
        'End If

        'enable_ebay_act_browser  enable_ebay_comp_browser

        'cmd.Parameters.AddWithValue("@mws_secretkey", txtMWSSecretKey.Text)
        'cmd.Parameters.AddWithValue("@mws_accesskey", txtMWSAccesskey.Text)
        'cmd.Parameters.AddWithValue("@mws_sellerid", txtMWSSellerID.Text)
        'cmd.Parameters.AddWithValue("@mws_marketplaceid", txtMWSMarketPlaceID.Text)

        cmd.Parameters.AddWithValue("@access_key", txtAwsKey.Text.Trim)
        cmd.Parameters.AddWithValue("@secret_key", txtAwsScrretKey.Text.Trim)
        cmd.Parameters.AddWithValue("@refresh_minute", objval.ToInt32(txtRefMin.Text.Trim))
        cmd.Parameters.AddWithValue("@aws_sellerid", txtAWSSellerID.Text.Trim)
        'cmd.Parameters.AddWithValue("@ebAppID", ebAppID.Text.Trim())
        'cmd.Parameters.AddWithValue("@ebDevID", ebDevID.Text.Trim())
        'cmd.Parameters.AddWithValue("@ebCerID", ebCerID.Text.Trim())
        cmd.Parameters.AddWithValue("@api_call_intervalAWS", objval.ToInt32(txtAPIIntAWSMin.Text.Trim))
        cmd.Parameters.AddWithValue("@api_call_intervalMWS", objval.ToInt32(txtAPIIntMWSMin.Text.Trim))
        'cmd.Parameters.AddWithValue("@keepa_access_key", objval.ToString(txtKeepaAccessKey.Text))

        If chkShowNotification.Checked = True Then
            cmd.Parameters.AddWithValue("@is_show_notification", 1)
        Else
            cmd.Parameters.AddWithValue("@is_show_notification", 0)
        End If
        cmd.Parameters.AddWithValue("@is_show_notification_type", radNotification_Type.SelectedIndex)

        If chkIsSendMail.Checked = True Then
            cmd.Parameters.AddWithValue("@is_alert_mail", 1)
        Else
            cmd.Parameters.AddWithValue("@is_alert_mail", 0)
        End If

        cmd.Parameters.AddWithValue("@email_ids", txtAlertMails.Text.Trim)
        cmd.Parameters.AddWithValue("@sender_emailid", txtSenderMailID.Text.Trim)
        cmd.Parameters.AddWithValue("@sender_emailpwd", txtSenderMailPWD.Text.Trim)
        cmd.Parameters.AddWithValue("@sender_smtp", txtSenderSMTP.Text.Trim)
        cmd.Parameters.AddWithValue("@sender_port", objval.ToInt32(txtSenderPORT.Text.Trim))
        If chkSenderSSL.Checked = True Then
            cmd.Parameters.AddWithValue("@sender_chkssl", 1)
        Else
            cmd.Parameters.AddWithValue("@sender_chkssl", 0)
        End If

        If dbOperation.ExecuteQuery(cmd) > 0 Then
            'sqlCon.Open()
            Dim maxKeyId As Integer = 0
            For Each dr As DataRow In dtDet.Select()
                If dr("MarketId") Is DBNull.Value Then
                    dr("MarketId") = lue_MarketList.EditValue
                End If
                If dr("KeyId") <= 0 Then
                    dr("KeyId") = dtDet.Compute("Max(KeyId)", "") + 1
                End If
                If dr("KeyDisplayOrder") <= 0 Then
                    dr("KeyDisplayOrder") = dtDet.Compute("Max(KeyDisplayOrder)", "") + 1
                End If
            Next

            da.Update(dtDet)

            objval.MsgSuccess("Setting updated successfully.", "Success")

            Me.Close()
        Else
            objval.MsgError("Some error occured while updating setting.", "Error")
        End If

    End Sub




    Private Sub chkEnableSchedule_CheckedChanged(sender As Object, e As EventArgs) Handles chkEnableSchedule.CheckedChanged
        If chkEnableSchedule.Checked = True Then
            txtAutoStarttime.Enabled = True
        Else
            txtAutoStarttime.Enabled = False
        End If
    End Sub

    Public Function IsValidTimeString() As Boolean
        Dim isValid As Boolean = True
        Dim TimeStr As String = txtAutoStarttime.Text.Trim  '  02:00,2:00,asdf
        Dim _arrMainItems() As String = TimeStr.Split(",")
        'Dim _arrDate() As DateTime = Nothing
        Dim _arrDate As New List(Of DateTime)
        For Each arrItem As String In _arrMainItems
            Dim _arrSub() As String = arrItem.Split(":")
            If _arrSub.Length <> 2 Then
                isValid = False
            Else
                If objval.ToInt32(_arrSub(0)) = 0 OrElse objval.ToInt32(_arrSub(0)) > 24 Then
                    isValid = False
                End If

                If objval.ToInt32(_arrSub(1)) > 59 Then
                    isValid = False
                End If

                If _arrSub(1).Length <> 2 Then
                    isValid = False
                ElseIf objval.ToInt32(_arrSub(1)) > 59 Then
                    isValid = False
                End If

                If isValid = True Then
                    Dim tempDt As New DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, objval.ToInt32(_arrSub(0)), objval.ToInt32(_arrSub(1)), 0)
                    _arrDate.Add(tempDt)
                End If

            End If
        Next

        If isValid = True Then
            _arrDate.Sort()

            Dim str As String = ""
            For Each item As DateTime In _arrDate
                str = str + item.ToString("HH:mm") + ","
            Next
            txtAutoStarttime.Text = str.Trim.Substring(0, str.Length - 1)

        End If

        Return isValid
    End Function

    'Private Sub chkEnabledEbay_CheckedChanged(sender As Object, e As EventArgs)
    '    If chkEnabledEbay.Checked = True Then
    '        ebAppID.Enabled = True
    '        ebCerID.Enabled = True
    '        ebDevID.Enabled = True
    '        chkBrowserComp.Enabled = True
    '        chkBrowserEbayACT.Enabled = True
    '    Else
    '        ebAppID.Enabled = False
    '        ebCerID.Enabled = False
    '        ebDevID.Enabled = False
    '        chkBrowserComp.Enabled = False
    '        chkBrowserEbayACT.Enabled = False
    '    End If
    'End Sub

    Private Sub chkShowNotification_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowNotification.CheckedChanged
        If chkShowNotification.Checked = True Then
            radNotification_Type.Enabled = True
        Else
            radNotification_Type.Enabled = False
        End If
    End Sub

    Private Sub dgv_MarketDet_CellValueNeeded(sender As Object, e As DataGridViewCellValueEventArgs) Handles dgv_MarketDet.CellValueNeeded
        If dgv_MarketDet.Columns("btnDelete").Name = dgv_MarketDet.Columns(e.ColumnIndex).Name Then
            e.Value = My.Resources.Delete
        End If
    End Sub

    Private Sub dgv_MarketDet_NewRowNeeded(sender As Object, e As DataGridViewRowEventArgs) Handles dgv_MarketDet.NewRowNeeded
        e.Row.Cells("MarketId").Value = lue_MarketList.EditValue
    End Sub

    
    Private Sub btn_MktEdit_Click(sender As Object, e As EventArgs) Handles btn_MktEdit.Click
        FlyoutPanel1.ShowPopup()
        txtMarketPlaceName.Tag = "E"
        txtMarketPlaceName.Text = dtMas.Compute("Max(MarketName)", "MarketId=" & lue_MarketList.EditValue)
        txtMarketPlaceName.Focus()
    End Sub

    Private Sub FlyoutPanel1_ButtonClick(sender As Object, e As DevExpress.Utils.FlyoutPanelButtonClickEventArgs) Handles FlyoutPanel1.ButtonClick
        If e.Button.Caption = "Save" Then
            If txtMarketPlaceName.Tag = "A" Then
                If dtMas.Compute("Count(MarketId)", "MarketName='" & txtMarketPlaceName.Text.ToUpper & "'") = 0 Then

                    Dim MaxId As Integer = Val(dtMas.Compute("Max(MarketId)", "")) + 1

                    Dim cmd As New SQLiteCommand
                    cmd.CommandText = "insert into db_MarketPlaceMas(MarketId,MarketName,IsActive) Values (@MarketId,@MarketName,@IsActive)"

                    cmd.Parameters.AddWithValue("@MarketId", MaxId)
                    cmd.Parameters.AddWithValue("@MarketName", txtMarketPlaceName.Text)
                    cmd.Parameters.AddWithValue("@IsActive", 1)

                    If dbOperation.ExecuteQuery(cmd) > 0 Then
                        objval.MsgSuccess("MarketPlace Added Successfully.", "Success")
                        Fill_ComboListAndData(MaxId)
                    Else
                        objval.MsgError("Some error occured while Adding New MarketPlace.", "Error")
                    End If

                Else
                    objval.MsgError("[" & txtMarketPlaceName.Text & "] name already exist ! Please enter different name.", "Error")
                End If
            Else
                If dtMas.Compute("Count(MarketId)", "Upper(MarketName)='" & txtMarketPlaceName.Text.ToUpper & "' And MarketId<>" & lue_MarketList.EditValue) Then

                    Dim cmd As New SQLiteCommand
                    cmd.CommandText = "update db_MarketPlaceMas Set MarketName=@MarketName Where MarketId=@MarketId"

                    cmd.Parameters.AddWithValue("@MarketName", txtMarketPlaceName.Text)
                    cmd.Parameters.AddWithValue("@MarketId", lue_MarketList.EditValue)

                    If dbOperation.ExecuteQuery(cmd) > 0 Then
                        objval.MsgSuccess("MarketPlace Updated Successfully.", "Success")
                        Fill_ComboListAndData(lue_MarketList.EditValue)
                    Else
                        objval.MsgError("Some error occured while updatng MarketPlace.", "Error")
                    End If

                Else
                    objval.MsgError("[" & txtMarketPlaceName.Text & "] name already exist ! Please enter different name.", "Error")
                End If
            End If
            MessageBox.Show("Saved")
        ElseIf e.Button.Caption = "Close" Then
            FlyoutPanel1.HidePopup()
        End If
    End Sub

    Private Sub btn_MktAdd_Click(sender As Object, e As EventArgs) Handles btn_MktAdd.Click
        FlyoutPanel1.ShowPopup()
        txtMarketPlaceName.Text = String.Empty
        txtMarketPlaceName.Tag = "A"
        txtMarketPlaceName.Focus()
    End Sub

    Private Sub btn_MktDelete_Click(sender As Object, e As EventArgs) Handles btn_MktDelete.Click

        If MessageBox.Show("Do you want to Delete current selected MarketPlace [" & txtMarketPlaceName.Text & "] ?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) = Windows.Forms.DialogResult.Yes Then

            Dim cmd As New SQLiteCommand
            cmd.CommandText = "delete from db_MarketPlaceDet Where MarketId=@MarketId"
            cmd.Parameters.AddWithValue("@MarketId", lue_MarketList.EditValue)
            If dbOperation.ExecuteQuery(cmd) >= 0 Then
                cmd.CommandText = "delete from db_MarketPlaceMas Where MarketId=@MarketId"
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("@MarketId", lue_MarketList.EditValue)
                If dbOperation.ExecuteQuery(cmd) > 0 Then
                    objval.MsgSuccess("MarketPlace Deleted Successfully.", "Success")
                    Fill_ComboListAndData(lue_MarketList.EditValue)
                Else
                    objval.MsgError("Some error occured while deleting MarketPlace.", "Error")
                End If
            Else
                objval.MsgError("Some error occured while deleting MarketPlace.", "Error")
            End If

        End If

    End Sub

    Private Sub dgv_MarketDet_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_MarketDet.CellClick
        If dgv_MarketDet.Columns(e.ColumnIndex).Name = dgv_MarketDet.Columns("btnDelete").Name Then
            If MessageBox.Show("Do you want to delete this entry ?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.Yes Then
                dtDet.Rows(e.RowIndex).Delete()
                MessageBox.Show("Entry deleted successfully.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub
End Class