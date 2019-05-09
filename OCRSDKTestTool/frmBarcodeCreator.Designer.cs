namespace OCRSDKTest
{
    partial class frmBarcodeCreator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.canvasPicture1 = new OCRSDKTest.CanvasPicture2();
            this.pnlCreate = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEncoderContent = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbEncoderType = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpenImage = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.ツールToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEncode = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClear = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.trackBarLeft = new System.Windows.Forms.TrackBar();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.mnuTifCreater = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.canvasPicture1)).BeginInit();
            this.pnlCreate.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLeft)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.canvasPicture1);
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(705, 732);
            this.panel1.TabIndex = 0;
            // 
            // canvasPicture1
            // 
            this.canvasPicture1.AllowAddFrame = false;
            this.canvasPicture1.Location = new System.Drawing.Point(0, 0);
            this.canvasPicture1.Name = "canvasPicture1";
            this.canvasPicture1.SelectedFrames = null;
            this.canvasPicture1.Selection = null;
            this.canvasPicture1.ShowFrames = false;
            this.canvasPicture1.Size = new System.Drawing.Size(690, 731);
            this.canvasPicture1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.canvasPicture1.TabIndex = 0;
            this.canvasPicture1.TabStop = false;
            this.canvasPicture1.TranAreas = null;
            this.canvasPicture1.Zoom = 100F;
            this.canvasPicture1.CreateBarcode += new System.EventHandler<OCRSDKTest.FrameEventArgs>(this.canvasPicture1_CreateBarcode);
            this.canvasPicture1.SelectedBarcode += new System.EventHandler<OCRSDKTest.FrameEventArgs>(this.canvasPicture1_SelectedBarcode);
            this.canvasPicture1.Click += new System.EventHandler(this.canvasPicture1_Click);
            // 
            // pnlCreate
            // 
            this.pnlCreate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCreate.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pnlCreate.Controls.Add(this.label5);
            this.pnlCreate.Controls.Add(this.textBox2);
            this.pnlCreate.Controls.Add(this.label3);
            this.pnlCreate.Controls.Add(this.propertyGrid1);
            this.pnlCreate.Controls.Add(this.button1);
            this.pnlCreate.Controls.Add(this.label2);
            this.pnlCreate.Controls.Add(this.txtEncoderContent);
            this.pnlCreate.Controls.Add(this.label1);
            this.pnlCreate.Controls.Add(this.cmbEncoderType);
            this.pnlCreate.Location = new System.Drawing.Point(711, 27);
            this.pnlCreate.Name = "pnlCreate";
            this.pnlCreate.Size = new System.Drawing.Size(296, 787);
            this.pnlCreate.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(18, 497);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "操作説明";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.Info;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2.Location = new System.Drawing.Point(16, 512);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(262, 180);
            this.textBox2.TabIndex = 7;
            this.textBox2.Text = "①ファイルメニューからTIFF画像を開く\r\n\r\n②画面でバーコードを作成する位置とサイズをマウスで選択する\r\n\r\n③バーコードの種別を選択して、バーコードの内容を" +
    "入力する\r\n\r\n④作成ボタンをクリックして、バーコードを作成する\r\n\r\n⑤バーコードを選択して、右クリックで、バーコードを削除できる\r\n\r\n⑥ファイルメニューで" +
    "編集した画像を保存する";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(20, 238);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "オプション設定";
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(16, 256);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(264, 211);
            this.propertyGrid1.TabIndex = 5;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(177, 188);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "作成";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(18, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "バーコード内容";
            // 
            // txtEncoderContent
            // 
            this.txtEncoderContent.Location = new System.Drawing.Point(16, 85);
            this.txtEncoderContent.Multiline = true;
            this.txtEncoderContent.Name = "txtEncoderContent";
            this.txtEncoderContent.Size = new System.Drawing.Size(251, 97);
            this.txtEncoderContent.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(16, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "バーコード種別";
            // 
            // cmbEncoderType
            // 
            this.cmbEncoderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEncoderType.FormattingEnabled = true;
            this.cmbEncoderType.Location = new System.Drawing.Point(16, 28);
            this.cmbEncoderType.Name = "cmbEncoderType";
            this.cmbEncoderType.Size = new System.Drawing.Size(251, 20);
            this.cmbEncoderType.TabIndex = 0;
            this.cmbEncoderType.SelectedIndexChanged += new System.EventHandler(this.cmbEncoderType_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルToolStripMenuItem,
            this.ツールToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1014, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルToolStripMenuItem
            // 
            this.ファイルToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpenImage,
            this.mnuSave,
            this.toolStripMenuItem1,
            this.mnuExit});
            this.ファイルToolStripMenuItem.Name = "ファイルToolStripMenuItem";
            this.ファイルToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.ファイルToolStripMenuItem.Text = "ファイル";
            // 
            // mnuOpenImage
            // 
            this.mnuOpenImage.Name = "mnuOpenImage";
            this.mnuOpenImage.Size = new System.Drawing.Size(158, 22);
            this.mnuOpenImage.Text = "TIFF画像を開く";
            this.mnuOpenImage.Click += new System.EventHandler(this.mnuOpenImage_Click);
            // 
            // mnuSave
            // 
            this.mnuSave.Name = "mnuSave";
            this.mnuSave.Size = new System.Drawing.Size(158, 22);
            this.mnuSave.Text = "TIFF画像を保存";
            this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(155, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(158, 22);
            this.mnuExit.Text = "終了";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // ツールToolStripMenuItem
            // 
            this.ツールToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEncode,
            this.mnuClear,
            this.mnuTifCreater});
            this.ツールToolStripMenuItem.Name = "ツールToolStripMenuItem";
            this.ツールToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.ツールToolStripMenuItem.Text = "ツール";
            // 
            // mnuEncode
            // 
            this.mnuEncode.Name = "mnuEncode";
            this.mnuEncode.Size = new System.Drawing.Size(154, 22);
            this.mnuEncode.Text = "バーコード認識";
            this.mnuEncode.Click += new System.EventHandler(this.mnuEncode_Click);
            // 
            // mnuClear
            // 
            this.mnuClear.Name = "mnuClear";
            this.mnuClear.Size = new System.Drawing.Size(154, 22);
            this.mnuClear.Text = "クリア";
            this.mnuClear.Click += new System.EventHandler(this.mnuClear_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "*.tif";
            this.openFileDialog1.Filter = "画像ファイル|*.png;*.jpg;*.gif;*.tif|領域ファイル|*.rng|画像辞書ファイル|*.dic";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Tiffファイル|*.tif";
            // 
            // trackBarLeft
            // 
            this.trackBarLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trackBarLeft.LargeChange = 20;
            this.trackBarLeft.Location = new System.Drawing.Point(1, 765);
            this.trackBarLeft.Maximum = 150;
            this.trackBarLeft.Minimum = 20;
            this.trackBarLeft.Name = "trackBarLeft";
            this.trackBarLeft.Size = new System.Drawing.Size(536, 45);
            this.trackBarLeft.SmallChange = 10;
            this.trackBarLeft.TabIndex = 3;
            this.trackBarLeft.TickFrequency = 10;
            this.trackBarLeft.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBarLeft.Value = 50;
            this.trackBarLeft.ValueChanged += new System.EventHandler(this.trackBarLeft_ValueChanged);
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.textBox1.Location = new System.Drawing.Point(543, 777);
            this.textBox1.Margin = new System.Windows.Forms.Padding(5);
            this.textBox1.MaxLength = 3;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(41, 19);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "100";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox1.WordWrap = false;
            this.textBox1.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(590, 780);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "%";
            // 
            // mnuTifCreater
            // 
            this.mnuTifCreater.Name = "mnuTifCreater";
            this.mnuTifCreater.Size = new System.Drawing.Size(154, 22);
            this.mnuTifCreater.Text = "TIFF作成ツール";
            this.mnuTifCreater.Click += new System.EventHandler(this.mnuTifCreater_Click);
            // 
            // frmBarcodeCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 818);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.trackBarLeft);
            this.Controls.Add(this.pnlCreate);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "frmBarcodeCreator";
            this.Text = "バーコード作成ツール";
            this.Load += new System.EventHandler(this.frmBarcodeCreator_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.canvasPicture1)).EndInit();
            this.pnlCreate.ResumeLayout(false);
            this.pnlCreate.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLeft)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private CanvasPicture2 canvasPicture1;
        private System.Windows.Forms.Panel pnlCreate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEncoderContent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbEncoderType;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ファイルToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuOpenImage;
        private System.Windows.Forms.ToolStripMenuItem mnuSave;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBarLeft;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ToolStripMenuItem ツールToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuEncode;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuClear;
        private System.Windows.Forms.ToolStripMenuItem mnuTifCreater;
    }
}