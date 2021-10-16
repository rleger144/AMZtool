<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmKeepaScan
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
        Me.btnStart = New DevExpress.XtraEditors.SimpleButton()
        Me.btnStop = New DevExpress.XtraEditors.SimpleButton()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl2 = New DevExpress.XtraEditors.LabelControl()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.chkWithPrevious = New DevExpress.XtraEditors.CheckEdit()
        Me.pnlUpdating = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblPercentage = New DevExpress.XtraEditors.LabelControl()
        Me.lblApproxTime = New DevExpress.XtraEditors.LabelControl()
        Me.lblScanComplete = New DevExpress.XtraEditors.LabelControl()
        Me.lblTotalProduct = New DevExpress.XtraEditors.LabelControl()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.lblprocessStopped = New DevExpress.XtraEditors.LabelControl()
        Me.LabelControl7 = New DevExpress.XtraEditors.LabelControl()
        Me.txtErrorLog = New DevExpress.XtraEditors.MemoEdit()
        Me.HDbackWork = New System.ComponentModel.BackgroundWorker()
        Me.Panel1.SuspendLayout()
        CType(Me.chkWithPrevious.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlUpdating.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        CType(Me.txtErrorLog.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnStart
        '
        Me.btnStart.Appearance.Font = New System.Drawing.Font("Trebuchet MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStart.Appearance.Options.UseFont = True
        Me.btnStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnStart.ImageOptions.Image = Global.AWSTool.My.Resources.Resources.play_button
        Me.btnStart.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter
        Me.btnStart.Location = New System.Drawing.Point(8, 23)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(58, 44)
        Me.btnStart.TabIndex = 95
        '
        'btnStop
        '
        Me.btnStop.Appearance.Font = New System.Drawing.Font("Trebuchet MS", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStop.Appearance.Options.UseFont = True
        Me.btnStop.Enabled = False
        Me.btnStop.ImageOptions.Image = Global.AWSTool.My.Resources.Resources.stop_button
        Me.btnStop.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter
        Me.btnStop.Location = New System.Drawing.Point(69, 23)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(58, 44)
        Me.btnStop.TabIndex = 96
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl1.Appearance.Options.UseFont = True
        Me.LabelControl1.Location = New System.Drawing.Point(142, 26)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(88, 14)
        Me.LabelControl1.TabIndex = 97
        Me.LabelControl1.Text = "Total Products :"
        '
        'LabelControl2
        '
        Me.LabelControl2.Appearance.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl2.Appearance.Options.UseFont = True
        Me.LabelControl2.Location = New System.Drawing.Point(336, 28)
        Me.LabelControl2.Name = "LabelControl2"
        Me.LabelControl2.Size = New System.Drawing.Size(67, 14)
        Me.LabelControl2.TabIndex = 98
        Me.LabelControl2.Text = "Completed :"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.chkWithPrevious)
        Me.Panel1.Controls.Add(Me.pnlUpdating)
        Me.Panel1.Controls.Add(Me.lblApproxTime)
        Me.Panel1.Controls.Add(Me.lblScanComplete)
        Me.Panel1.Controls.Add(Me.lblTotalProduct)
        Me.Panel1.Controls.Add(Me.btnStop)
        Me.Panel1.Controls.Add(Me.LabelControl2)
        Me.Panel1.Controls.Add(Me.btnStart)
        Me.Panel1.Controls.Add(Me.LabelControl1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(650, 71)
        Me.Panel1.TabIndex = 99
        '
        'chkWithPrevious
        '
        Me.chkWithPrevious.EditValue = True
        Me.chkWithPrevious.EnterMoveNextControl = True
        Me.chkWithPrevious.Location = New System.Drawing.Point(8, 2)
        Me.chkWithPrevious.Name = "chkWithPrevious"
        '
        '
        '
        Me.chkWithPrevious.Properties.Caption = "Scan from First item ?"
        Me.chkWithPrevious.Size = New System.Drawing.Size(123, 19)
        Me.chkWithPrevious.TabIndex = 104
        '
        'pnlUpdating
        '
        Me.pnlUpdating.Controls.Add(Me.PictureBox1)
        Me.pnlUpdating.Controls.Add(Me.lblPercentage)
        Me.pnlUpdating.Location = New System.Drawing.Point(530, 16)
        Me.pnlUpdating.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.pnlUpdating.Name = "pnlUpdating"
        Me.pnlUpdating.Size = New System.Drawing.Size(118, 35)
        Me.pnlUpdating.TabIndex = 103
        Me.pnlUpdating.Visible = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.AWSTool.My.Resources.Resources.processing
        Me.PictureBox1.Location = New System.Drawing.Point(4, 2)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(31, 30)
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'lblPercentage
        '
        Me.lblPercentage.Appearance.Font = New System.Drawing.Font("Tahoma", 15.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPercentage.Appearance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblPercentage.Appearance.Options.UseFont = True
        Me.lblPercentage.Appearance.Options.UseForeColor = True
        Me.lblPercentage.Location = New System.Drawing.Point(42, 6)
        Me.lblPercentage.Name = "lblPercentage"
        Me.lblPercentage.Size = New System.Drawing.Size(70, 24)
        Me.lblPercentage.TabIndex = 101
        Me.lblPercentage.Text = "50.20%"
        '
        'lblApproxTime
        '
        Me.lblApproxTime.Appearance.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblApproxTime.Appearance.ForeColor = System.Drawing.Color.Maroon
        Me.lblApproxTime.Appearance.Options.UseFont = True
        Me.lblApproxTime.Appearance.Options.UseForeColor = True
        Me.lblApproxTime.Location = New System.Drawing.Point(144, 48)
        Me.lblApproxTime.Name = "lblApproxTime"
        Me.lblApproxTime.Size = New System.Drawing.Size(123, 13)
        Me.lblApproxTime.TabIndex = 102
        Me.lblApproxTime.Text = "( approx 30 minute scan )"
        '
        'lblScanComplete
        '
        Me.lblScanComplete.Appearance.Font = New System.Drawing.Font("Tahoma", 15.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblScanComplete.Appearance.Options.UseFont = True
        Me.lblScanComplete.Location = New System.Drawing.Point(409, 22)
        Me.lblScanComplete.Name = "lblScanComplete"
        Me.lblScanComplete.Size = New System.Drawing.Size(66, 24)
        Me.lblScanComplete.TabIndex = 100
        Me.lblScanComplete.Text = "500000"
        '
        'lblTotalProduct
        '
        Me.lblTotalProduct.Appearance.Font = New System.Drawing.Font("Tahoma", 15.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalProduct.Appearance.Options.UseFont = True
        Me.lblTotalProduct.Location = New System.Drawing.Point(237, 22)
        Me.lblTotalProduct.Name = "lblTotalProduct"
        Me.lblTotalProduct.Size = New System.Drawing.Size(66, 24)
        Me.lblTotalProduct.TabIndex = 99
        Me.lblTotalProduct.Text = "500000"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.lblprocessStopped)
        Me.Panel2.Controls.Add(Me.LabelControl7)
        Me.Panel2.Controls.Add(Me.txtErrorLog)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 71)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Padding = New System.Windows.Forms.Padding(0, 20, 0, 0)
        Me.Panel2.Size = New System.Drawing.Size(650, 511)
        Me.Panel2.TabIndex = 100
        '
        'lblprocessStopped
        '
        Me.lblprocessStopped.Appearance.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblprocessStopped.Appearance.ForeColor = System.Drawing.Color.Maroon
        Me.lblprocessStopped.Appearance.Options.UseFont = True
        Me.lblprocessStopped.Appearance.Options.UseForeColor = True
        Me.lblprocessStopped.Location = New System.Drawing.Point(206, 2)
        Me.lblprocessStopped.Name = "lblprocessStopped"
        Me.lblprocessStopped.Size = New System.Drawing.Size(249, 16)
        Me.lblprocessStopped.TabIndex = 104
        Me.lblprocessStopped.Text = "Process stopped by user.. Please Wait"
        Me.lblprocessStopped.Visible = False
        '
        'LabelControl7
        '
        Me.LabelControl7.Appearance.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelControl7.Appearance.ForeColor = System.Drawing.Color.Maroon
        Me.LabelControl7.Appearance.Options.UseFont = True
        Me.LabelControl7.Appearance.Options.UseForeColor = True
        Me.LabelControl7.Location = New System.Drawing.Point(7, 4)
        Me.LabelControl7.Name = "LabelControl7"
        Me.LabelControl7.Size = New System.Drawing.Size(58, 13)
        Me.LabelControl7.TabIndex = 103
        Me.LabelControl7.Text = "ERROR LOG"
        '
        'txtErrorLog
        '
        Me.txtErrorLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtErrorLog.EnterMoveNextControl = True
        Me.txtErrorLog.Location = New System.Drawing.Point(0, 20)
        Me.txtErrorLog.Name = "txtErrorLog"
        '
        '
        '
        Me.txtErrorLog.Properties.Appearance.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtErrorLog.Properties.Appearance.Options.UseFont = True
        Me.txtErrorLog.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003
        Me.txtErrorLog.Size = New System.Drawing.Size(650, 491)
        Me.txtErrorLog.TabIndex = 4
        '
        'HDbackWork
        '
        Me.HDbackWork.WorkerReportsProgress = True
        '
        'FrmKeepaScan
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(650, 582)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "FrmKeepaScan"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Keepa Scan"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.chkWithPrevious.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlUpdating.ResumeLayout(False)
        Me.pnlUpdating.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.txtErrorLog.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnStart As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnStop As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents txtErrorLog As DevExpress.XtraEditors.MemoEdit
    Friend WithEvents lblPercentage As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblScanComplete As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblTotalProduct As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblApproxTime As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl7 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents pnlUpdating As System.Windows.Forms.Panel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents HDbackWork As System.ComponentModel.BackgroundWorker
    Friend WithEvents lblprocessStopped As DevExpress.XtraEditors.LabelControl
    Friend WithEvents chkWithPrevious As DevExpress.XtraEditors.CheckEdit
End Class
