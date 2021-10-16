<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLicDetails
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lbl_RegisterDate = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lbl_ExpiredOn = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.lbl_License_Type = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lbl_MobileNo = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lbl_RegisteredEmail = New System.Windows.Forms.Label()
        Me.lbl_Address = New System.Windows.Forms.Label()
        Me.lbl_ContactName = New System.Windows.Forms.Label()
        Me.lbl_LicensedTo_CustName = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lbl_VersionDetails = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.btnOK = New DevExpress.XtraEditors.SimpleButton()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(6, 57)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(140, 14)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Registered On Date :"
        '
        'lbl_RegisterDate
        '
        Me.lbl_RegisterDate.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_RegisterDate.Location = New System.Drawing.Point(152, 57)
        Me.lbl_RegisterDate.Name = "lbl_RegisterDate"
        Me.lbl_RegisterDate.Size = New System.Drawing.Size(245, 17)
        Me.lbl_RegisterDate.TabIndex = 10
        Me.lbl_RegisterDate.Text = " "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(10, 86)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(136, 14)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "License Expired On :"
        '
        'lbl_ExpiredOn
        '
        Me.lbl_ExpiredOn.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_ExpiredOn.Location = New System.Drawing.Point(152, 86)
        Me.lbl_ExpiredOn.Name = "lbl_ExpiredOn"
        Me.lbl_ExpiredOn.Size = New System.Drawing.Size(235, 18)
        Me.lbl_ExpiredOn.TabIndex = 12
        Me.lbl_ExpiredOn.Text = " "
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(50, 28)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(96, 14)
        Me.Label7.TabIndex = 15
        Me.Label7.Text = "License Type :"
        '
        'lbl_License_Type
        '
        Me.lbl_License_Type.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_License_Type.ForeColor = System.Drawing.Color.Maroon
        Me.lbl_License_Type.Location = New System.Drawing.Point(152, 28)
        Me.lbl_License_Type.Name = "lbl_License_Type"
        Me.lbl_License_Type.Size = New System.Drawing.Size(247, 18)
        Me.lbl_License_Type.TabIndex = 14
        Me.lbl_License_Type.Text = " "
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.lbl_MobileNo)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.lbl_RegisteredEmail)
        Me.GroupBox1.Controls.Add(Me.lbl_Address)
        Me.GroupBox1.Controls.Add(Me.lbl_ContactName)
        Me.GroupBox1.Controls.Add(Me.lbl_LicensedTo_CustName)
        Me.GroupBox1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(71, 97)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(405, 154)
        Me.GroupBox1.TabIndex = 16
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Registered Details"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(69, 121)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(77, 14)
        Me.Label4.TabIndex = 17
        Me.Label4.Text = "Mobile No :"
        '
        'lbl_MobileNo
        '
        Me.lbl_MobileNo.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_MobileNo.ForeColor = System.Drawing.Color.Maroon
        Me.lbl_MobileNo.Location = New System.Drawing.Point(152, 121)
        Me.lbl_MobileNo.Name = "lbl_MobileNo"
        Me.lbl_MobileNo.Size = New System.Drawing.Size(247, 18)
        Me.lbl_MobileNo.TabIndex = 16
        Me.lbl_MobileNo.Text = " "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(80, 96)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(66, 14)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Email Id :"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(57, 26)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(89, 14)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Licensed To :"
        '
        'lbl_RegisteredEmail
        '
        Me.lbl_RegisteredEmail.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_RegisteredEmail.ForeColor = System.Drawing.Color.Blue
        Me.lbl_RegisteredEmail.Location = New System.Drawing.Point(152, 96)
        Me.lbl_RegisteredEmail.Name = "lbl_RegisteredEmail"
        Me.lbl_RegisteredEmail.Size = New System.Drawing.Size(247, 18)
        Me.lbl_RegisteredEmail.TabIndex = 13
        Me.lbl_RegisteredEmail.Text = " "
        '
        'lbl_Address
        '
        Me.lbl_Address.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_Address.Location = New System.Drawing.Point(152, 69)
        Me.lbl_Address.Name = "lbl_Address"
        Me.lbl_Address.Size = New System.Drawing.Size(247, 16)
        Me.lbl_Address.TabIndex = 12
        Me.lbl_Address.Text = " "
        '
        'lbl_ContactName
        '
        Me.lbl_ContactName.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_ContactName.ForeColor = System.Drawing.Color.DarkBlue
        Me.lbl_ContactName.Location = New System.Drawing.Point(152, 47)
        Me.lbl_ContactName.Name = "lbl_ContactName"
        Me.lbl_ContactName.Size = New System.Drawing.Size(247, 17)
        Me.lbl_ContactName.TabIndex = 11
        Me.lbl_ContactName.Text = " "
        '
        'lbl_LicensedTo_CustName
        '
        Me.lbl_LicensedTo_CustName.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_LicensedTo_CustName.Location = New System.Drawing.Point(152, 26)
        Me.lbl_LicensedTo_CustName.Name = "lbl_LicensedTo_CustName"
        Me.lbl_LicensedTo_CustName.Size = New System.Drawing.Size(245, 19)
        Me.lbl_LicensedTo_CustName.TabIndex = 10
        Me.lbl_LicensedTo_CustName.Text = " "
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lbl_License_Type)
        Me.GroupBox2.Controls.Add(Me.lbl_RegisterDate)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.lbl_ExpiredOn)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(71, 267)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(405, 118)
        Me.GroupBox2.TabIndex = 17
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "License Details"
        '
        'lbl_VersionDetails
        '
        Me.lbl_VersionDetails.Appearance.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_VersionDetails.Appearance.Options.UseFont = True
        Me.lbl_VersionDetails.Location = New System.Drawing.Point(182, 58)
        Me.lbl_VersionDetails.Name = "lbl_VersionDetails"
        Me.lbl_VersionDetails.Size = New System.Drawing.Size(43, 13)
        Me.lbl_VersionDetails.TabIndex = 39
        Me.lbl_VersionDetails.Text = "Version"
        '
        'LabelControl2
        '
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Arial Narrow", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl2.Appearance.ForeColor = System.Drawing.Color.DarkGreen
        Me.LabelControl2.Appearance.Options.UseFont = True
        Me.LabelControl2.Appearance.Options.UseForeColor = True
        Me.LabelControl2.Location = New System.Drawing.Point(182, 28)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(165, 33)
        Me.LabelControl2.TabIndex = 38
        Me.LabelControl2.Text = "Price Analyzer"
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(226, 404)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(84, 30)
        Me.btnOK.TabIndex = 40
        Me.btnOK.Text = "OK"
        '
        'frmLicDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(535, 446)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lbl_VersionDetails)
        Me.Controls.Add(Me.LabelControl2)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "frmLicDetails"
        Me.Text = "License Details"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lbl_RegisterDate As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lbl_ExpiredOn As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lbl_License_Type As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lbl_MobileNo As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lbl_RegisteredEmail As System.Windows.Forms.Label
    Friend WithEvents lbl_Address As System.Windows.Forms.Label
    Friend WithEvents lbl_ContactName As System.Windows.Forms.Label
    Friend WithEvents lbl_LicensedTo_CustName As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lbl_VersionDetails As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnOK As DevExpress.XtraEditors.SimpleButton
End Class
