Public Class AWSLogin

    Private AwsCls As New AWSToolClass
    Sub New()
        InitializeComponent()

    End Sub

    'Public Overrides Sub ProcessCommand(ByVal cmd As System.Enum, ByVal arg As Object)
    '    MyBase.ProcessCommand(cmd, arg)
    'End Sub
    Dim m_CustVersion As String = String.Empty
    Public Enum SplashScreenCommand
        SomeCommandId
    End Enum

    Private Sub AWSLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        m_CustVersion = "Version " & My.Application.Info.Version.ToString
        lbl_VersionDetails.Text = m_CustVersion
    End Sub

    Private Sub AWSLogin_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        txt_UserId.Focus()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles btnLogin.Click


        If txt_UserId.Text.Length = 0 Then
            MessageBox.Show("UserId/EmailId Mustbe required...!!")
        ElseIf txt_Password.Text.Length = 0 Then
            MessageBox.Show("Password Mustbe required...!!")
        Else
            Dim dsResult As New DataSet
            Dim HardwareId As String = String.Empty
            dsResult = AwsCls.WebLogin(txt_UserId.Text, txt_Password.Text)

            If dsResult.Tables.Count >= 2 AndAlso dsResult.Tables("Errors") IsNot Nothing AndAlso dsResult.Tables("Errors").Rows(0)("ErrorNo") = "0" AndAlso dsResult.Tables("Errors").Rows(0)("ErrorStatus") = "Success" Then
                Dim vDiffDays As Integer = 0
                For Each dr As DataRow In dsResult.Tables("CustomerDetails").Select()
                    ProductLicDetails.CustVersion = m_CustVersion
                    ProductLicDetails.ProductType = dr("ProductType")
                    ProductLicDetails.CustId = dr("CustId")
                    ProductLicDetails.CustName = dr("CustName")
                    ProductLicDetails.CustAdd = dr("CustAdd")
                    ProductLicDetails.CustContact = dr("CustContact")
                    ProductLicDetails.CustMobileNo = dr("CustMobileNo")
                    ProductLicDetails.CustEmail = dr("CustEmail")
                    ProductLicDetails.CustRegisterDate = dr("CustRegisterDate")
                    ProductLicDetails.CustUserType = dr("CustUserType")
                    ProductLicDetails.CustUserTypeCode = dr("CustUserTypeCode")
                    ProductLicDetails.CustExpireDate = dr("CustExpireDate")
                    vDiffDays = DateDiff(DateInterval.Day, dr("CustExpireDate"), Now)
                    If vDiffDays <= 15 Then
                        ProductLicDetails.CustExpireDateDisplay = "License Expire in " & vDiffDays & " Day(s)."
                    Else
                        ProductLicDetails.CustExpireDateDisplay = dr("CustExpireDate")
                    End If
                    ProductLicDetails.CustAllowRole = dr("CustAllowRole")
                    ProductLicDetails.CustAllowFeature = dr("CustAllowFeature")
                    ProductLicDetails.CustTokenNo = dr("CustTokenNo")
                Next

                Me.DialogResult = Windows.Forms.DialogResult.OK
            Else
                MessageBox.Show("Invalid UserId/Emailid and password ...!!")
            End If
        End If
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.DialogResult = Windows.Forms.DialogResult.No
    End Sub


End Class