Public Class AwsLogin_oLD
    Private AwsCls As New AWSToolClass
    Sub New()
        InitializeComponent()
    End Sub

    Public Overrides Sub ProcessCommand(ByVal cmd As System.Enum, ByVal arg As Object)
        MyBase.ProcessCommand(cmd, arg)
    End Sub

    Public Enum SplashScreenCommand
        SomeCommandId
    End Enum

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles SimpleButton2.Click
        Me.DialogResult = Windows.Forms.DialogResult.No
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK

        'If txt_UserId.Text.Length = 0 Then
        '    MessageBox.Show("UserId/EmailId Mustbe required...!!")
        'ElseIf txt_Password.Text.Length = 0 Then
        '    MessageBox.Show("Password Mustbe required...!!")
        'Else
        '    Dim Result As String = String.Empty
        '    Dim HardwareId As String = String.Empty
        '    Result = AwsCls.Signin(txt_UserId.Text, txt_Password.Text, HardwareId)
        '    If Not Result.Contains("Invalid") Then
        '        Me.DialogResult = Windows.Forms.DialogResult.OK
        '    Else
        '        MessageBox.Show("Invalid UserId/Emailid and password ...!!")
        '    End If
        'End If
    End Sub
End Class
