<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AWSLogin
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.LabelControl4 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl3 = New DevExpress.XtraEditors.LabelControl()
        Me.txt_UserId = New DevExpress.XtraEditors.TextEdit()
        Me.txt_Password = New DevExpress.XtraEditors.TextEdit()
        Me.btnClose = New DevExpress.XtraEditors.SimpleButton()
        Me.btnLogin = New DevExpress.XtraEditors.SimpleButton()
        Me.labelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.marqueeProgressBarControl1 = New DevExpress.XtraEditors.MarqueeProgressBarControl()
        Me.PictureEdit1 = New DevExpress.XtraEditors.PictureEdit()
        Me.PictureEdit2 = New DevExpress.XtraEditors.PictureEdit()
        Me.LabelControl5 = New DevExpress.XtraEditors.LabelControl()
        Me.PictureEdit4 = New DevExpress.XtraEditors.PictureEdit()
        Me.PictureEdit5 = New DevExpress.XtraEditors.PictureEdit()
        Me.lbl_VersionDetails = New DevExpress.XtraEditors.LabelControl()
        Me.BehaviorManager1 = New DevExpress.Utils.Behaviors.BehaviorManager(Me.components)
        Me.LabelControl8 = New DevExpress.XtraEditors.LabelControl()
        Me.PictureEdit3 = New DevExpress.XtraEditors.PictureEdit()
        CType(Me.txt_UserId.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txt_Password.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.marqueeProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureEdit2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureEdit4.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureEdit5.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BehaviorManager1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureEdit3.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LabelControl4
        '
        Me.LabelControl4.Appearance.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl4.Appearance.ForeColor = System.Drawing.Color.DarkGreen
        Me.LabelControl4.Appearance.Options.UseFont = True
        Me.LabelControl4.Appearance.Options.UseForeColor = True
        Me.LabelControl4.Location = New System.Drawing.Point(179, 337)
        Me.LabelControl4.Name = "LabelControl4"
        Me.LabelControl4.Size = New System.Drawing.Size(112, 16)
        Me.LabelControl4.TabIndex = 28
        Me.LabelControl4.Text = "UserId/EmailId"
        '
        'LabelControl3
        '
        Me.LabelControl3.Appearance.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl3.Appearance.ForeColor = System.Drawing.Color.DarkGreen
        Me.LabelControl3.Appearance.Options.UseFont = True
        Me.LabelControl3.Appearance.Options.UseForeColor = True
        Me.LabelControl3.Location = New System.Drawing.Point(179, 394)
        Me.LabelControl3.Name = "LabelControl3"
        Me.LabelControl3.Size = New System.Drawing.Size(70, 16)
        Me.LabelControl3.TabIndex = 27
        Me.LabelControl3.Text = "Password"
        '
        'txt_UserId
        '
        Me.txt_UserId.EnterMoveNextControl = True
        Me.txt_UserId.Location = New System.Drawing.Point(179, 356)
        Me.txt_UserId.Name = "txt_UserId"
        Me.txt_UserId.Properties.Appearance.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_UserId.Properties.Appearance.Options.UseFont = True
        Me.txt_UserId.Size = New System.Drawing.Size(227, 22)
        Me.txt_UserId.TabIndex = 0
        '
        'txt_Password
        '
        Me.txt_Password.EnterMoveNextControl = True
        Me.txt_Password.Location = New System.Drawing.Point(179, 413)
        Me.txt_Password.Name = "txt_Password"
        Me.txt_Password.Properties.Appearance.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txt_Password.Properties.Appearance.Options.UseFont = True
        Me.txt_Password.Properties.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txt_Password.Size = New System.Drawing.Size(227, 22)
        Me.txt_Password.TabIndex = 1
        '
        'btnClose
        '
        Me.btnClose.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Appearance.Options.UseFont = True
        Me.btnClose.Location = New System.Drawing.Point(266, 447)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(81, 28)
        Me.btnClose.TabIndex = 3
        Me.btnClose.Text = "Close"
        '
        'btnLogin
        '
        Me.btnLogin.Appearance.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLogin.Appearance.Options.UseFont = True
        Me.btnLogin.Location = New System.Drawing.Point(179, 447)
        Me.btnLogin.Name = "btnLogin"
        Me.btnLogin.Size = New System.Drawing.Size(81, 28)
        Me.btnLogin.TabIndex = 2
        Me.btnLogin.Text = "Login"
        '
        'labelControl1
        '
        Me.labelControl1.Appearance.BackColor = System.Drawing.Color.DimGray
        Me.labelControl1.Appearance.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelControl1.Appearance.ForeColor = System.Drawing.Color.Azure
        Me.labelControl1.Appearance.Options.UseBackColor = True
        Me.labelControl1.Appearance.Options.UseFont = True
        Me.labelControl1.Appearance.Options.UseForeColor = True
        Me.labelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.labelControl1.Location = New System.Drawing.Point(421, 524)
        Me.labelControl1.Name = "labelControl1"
        Me.labelControl1.Size = New System.Drawing.Size(115, 13)
        Me.labelControl1.TabIndex = 22
        Me.labelControl1.Text = "Copyright © 2019-2020"
        '
        'marqueeProgressBarControl1
        '
        Me.marqueeProgressBarControl1.EditValue = 0
        Me.marqueeProgressBarControl1.Location = New System.Drawing.Point(11, 499)
        Me.marqueeProgressBarControl1.Name = "marqueeProgressBarControl1"
        Me.marqueeProgressBarControl1.Size = New System.Drawing.Size(941, 12)
        Me.marqueeProgressBarControl1.TabIndex = 21
        Me.marqueeProgressBarControl1.Visible = False
        '
        'PictureEdit1
        '
        Me.PictureEdit1.Cursor = System.Windows.Forms.Cursors.Default
        Me.PictureEdit1.EditValue = Global.AWSTool.My.Resources.Resources.pngfind_com_conga_png_6579796
        Me.PictureEdit1.Location = New System.Drawing.Point(581, 89)
        Me.PictureEdit1.Name = "PictureEdit1"
        Me.PictureEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PictureEdit1.Properties.Appearance.Options.UseBackColor = True
        Me.PictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.[Auto]
        Me.PictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch
        Me.PictureEdit1.Properties.ZoomAccelerationFactor = 1.0R
        Me.PictureEdit1.Size = New System.Drawing.Size(335, 329)
        Me.PictureEdit1.TabIndex = 30
        '
        'PictureEdit2
        '
        Me.PictureEdit2.Cursor = System.Windows.Forms.Cursors.Default
        Me.PictureEdit2.EditValue = Global.AWSTool.My.Resources.Resources.login
        Me.PictureEdit2.Location = New System.Drawing.Point(259, 246)
        Me.PictureEdit2.Name = "PictureEdit2"
        Me.PictureEdit2.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PictureEdit2.Properties.Appearance.Options.UseBackColor = True
        Me.PictureEdit2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PictureEdit2.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.[Auto]
        Me.PictureEdit2.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch
        Me.PictureEdit2.Properties.ZoomAccelerationFactor = 1.0R
        Me.PictureEdit2.Size = New System.Drawing.Size(44, 46)
        Me.PictureEdit2.TabIndex = 32
        '
        'LabelControl5
        '
        Me.LabelControl5.Appearance.Font = New System.Drawing.Font("Arial Narrow", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl5.Appearance.ForeColor = System.Drawing.Color.MediumSeaGreen
        Me.LabelControl5.Appearance.Options.UseFont = True
        Me.LabelControl5.Appearance.Options.UseForeColor = True
        Me.LabelControl5.Location = New System.Drawing.Point(237, 295)
        Me.LabelControl5.Name = "LabelControl5"
        Me.LabelControl5.Size = New System.Drawing.Size(93, 25)
        Me.LabelControl5.TabIndex = 33
        Me.LabelControl5.Text = "User Login"
        '
        'PictureEdit4
        '
        Me.PictureEdit4.Cursor = System.Windows.Forms.Cursors.Default
        Me.PictureEdit4.EditValue = Global.AWSTool.My.Resources.Resources.hiclipart_com__2_
        Me.PictureEdit4.Location = New System.Drawing.Point(140, 348)
        Me.PictureEdit4.Name = "PictureEdit4"
        Me.PictureEdit4.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(239, Byte), Integer))
        Me.PictureEdit4.Properties.Appearance.Options.UseBackColor = True
        Me.PictureEdit4.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PictureEdit4.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.[Auto]
        Me.PictureEdit4.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch
        Me.PictureEdit4.Properties.ZoomAccelerationFactor = 1.0R
        Me.PictureEdit4.Size = New System.Drawing.Size(28, 28)
        Me.PictureEdit4.TabIndex = 36
        '
        'PictureEdit5
        '
        Me.PictureEdit5.Cursor = System.Windows.Forms.Cursors.Default
        Me.PictureEdit5.EditValue = Global.AWSTool.My.Resources.Resources.hiclipart_com2
        Me.PictureEdit5.Location = New System.Drawing.Point(140, 407)
        Me.PictureEdit5.Name = "PictureEdit5"
        Me.PictureEdit5.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PictureEdit5.Properties.Appearance.Options.UseBackColor = True
        Me.PictureEdit5.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PictureEdit5.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.[Auto]
        Me.PictureEdit5.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze
        Me.PictureEdit5.Properties.ZoomAccelerationFactor = 1.0R
        Me.PictureEdit5.Size = New System.Drawing.Size(30, 30)
        Me.PictureEdit5.TabIndex = 35
        '
        'lbl_VersionDetails
        '
        Me.lbl_VersionDetails.Appearance.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_VersionDetails.Appearance.Options.UseFont = True
        Me.lbl_VersionDetails.Location = New System.Drawing.Point(252, 143)
        Me.lbl_VersionDetails.Name = "lbl_VersionDetails"
        Me.lbl_VersionDetails.Size = New System.Drawing.Size(48, 16)
        Me.lbl_VersionDetails.TabIndex = 37
        Me.lbl_VersionDetails.Text = "Version"
        '
        'LabelControl8
        '
        Me.LabelControl8.Appearance.BackColor = System.Drawing.Color.DimGray
        Me.LabelControl8.Appearance.Options.UseBackColor = True
        Me.LabelControl8.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        Me.LabelControl8.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.LabelControl8.Location = New System.Drawing.Point(0, 487)
        Me.LabelControl8.Name = "LabelControl8"
        Me.LabelControl8.Size = New System.Drawing.Size(928, 30)
        Me.LabelControl8.TabIndex = 41
        '
        'PictureEdit3
        '
        Me.PictureEdit3.Cursor = System.Windows.Forms.Cursors.Default
        Me.PictureEdit3.EditValue = Global.AWSTool.My.Resources.Resources.PriceAnalyzer_Logo
        Me.PictureEdit3.Location = New System.Drawing.Point(138, 22)
        Me.PictureEdit3.Name = "PictureEdit3"
        Me.PictureEdit3.Properties.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.PictureEdit3.Properties.Appearance.Options.UseBackColor = True
        Me.PictureEdit3.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
        Me.PictureEdit3.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom
        Me.PictureEdit3.Properties.ZoomAccelerationFactor = 1.0R
        Me.PictureEdit3.Size = New System.Drawing.Size(312, 115)
        Me.PictureEdit3.TabIndex = 42
        '
        'AWSLogin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Stretch
        Me.BackgroundImageStore = Global.AWSTool.My.Resources.Resources.background
        Me.ClientSize = New System.Drawing.Size(928, 517)
        Me.ControlBox = False
        Me.Controls.Add(Me.PictureEdit3)
        Me.Controls.Add(Me.labelControl1)
        Me.Controls.Add(Me.LabelControl8)
        Me.Controls.Add(Me.lbl_VersionDetails)
        Me.Controls.Add(Me.PictureEdit4)
        Me.Controls.Add(Me.PictureEdit5)
        Me.Controls.Add(Me.LabelControl5)
        Me.Controls.Add(Me.PictureEdit2)
        Me.Controls.Add(Me.PictureEdit1)
        Me.Controls.Add(Me.LabelControl4)
        Me.Controls.Add(Me.LabelControl3)
        Me.Controls.Add(Me.txt_UserId)
        Me.Controls.Add(Me.txt_Password)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnLogin)
        Me.Controls.Add(Me.marqueeProgressBarControl1)
        Me.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AWSLogin"
        Me.ShowIcon = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.txt_UserId.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txt_Password.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.marqueeProgressBarControl1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureEdit1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureEdit2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureEdit4.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureEdit5.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BehaviorManager1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureEdit3.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LabelControl4 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl3 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents txt_UserId As DevExpress.XtraEditors.TextEdit
    Friend WithEvents txt_Password As DevExpress.XtraEditors.TextEdit
    Friend WithEvents btnClose As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnLogin As DevExpress.XtraEditors.SimpleButton
    Private WithEvents labelControl1 As DevExpress.XtraEditors.LabelControl
    Private WithEvents marqueeProgressBarControl1 As DevExpress.XtraEditors.MarqueeProgressBarControl
    Friend WithEvents PictureEdit1 As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents PictureEdit2 As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents LabelControl5 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents PictureEdit4 As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents PictureEdit5 As DevExpress.XtraEditors.PictureEdit
    Friend WithEvents lbl_VersionDetails As DevExpress.XtraEditors.LabelControl
    Friend WithEvents BehaviorManager1 As DevExpress.Utils.Behaviors.BehaviorManager
    Friend WithEvents LabelControl8 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents PictureEdit3 As DevExpress.XtraEditors.PictureEdit
End Class
