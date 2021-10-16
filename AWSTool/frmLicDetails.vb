Public Class frmLicDetails


    Private Sub frmLicDetails_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lbl_VersionDetails.Text = ProductLicDetails.CustVersion
        lbl_LicensedTo_CustName.Text = ProductLicDetails.CustName
        lbl_ContactName.Text = ProductLicDetails.CustContact
        lbl_Address.Text = ProductLicDetails.CustAdd
        lbl_RegisteredEmail.Text = ProductLicDetails.CustEmail
        lbl_MobileNo.Text = ProductLicDetails.CustMobileNo
        lbl_License_Type.Text = ProductLicDetails.CustUserTypeCode

        lbl_RegisterDate.Text = ProductLicDetails.CustRegisterDate
        lbl_ExpiredOn.Text = ProductLicDetails.CustExpireDate
    End Sub

    Private Sub btnOK_Click_1(sender As Object, e As EventArgs) Handles btnOK.Click
        Me.Close()
    End Sub

End Class