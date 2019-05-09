namespace OCRSDKTest
{
    partial class EraceParamSetting
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numMinLength = new System.Windows.Forms.NumericUpDown();
            this.numRatio = new System.Windows.Forms.NumericUpDown();
            this.numMinStep = new System.Windows.Forms.NumericUpDown();
            this.numMaxSpace = new System.Windows.Forms.NumericUpDown();
            this.numHStep = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabTableErase = new System.Windows.Forms.TabPage();
            this.tabNoiseErase = new System.Windows.Forms.TabPage();
            this.numMaxNoiseSize = new System.Windows.Forms.NumericUpDown();
            this.chkAutoSize = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbNoiseType = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbFastMode = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbLevel = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbDocType = new System.Windows.Forms.ComboBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.tabExtraFrame = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.numExtraFrameMargin = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numMinLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRatio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHStep)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabTableErase.SuspendLayout();
            this.tabNoiseErase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxNoiseSize)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.tabExtraFrame.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numExtraFrameMargin)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(212, 266);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(312, 266);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(84, 29);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "罫線長さの最小閾値";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "罫線矩形の縦横比の閾値";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "走査長さの最小閾値";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "点線の最大間隔";
            // 
            // numMinLength
            // 
            this.numMinLength.Location = new System.Drawing.Point(168, 15);
            this.numMinLength.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numMinLength.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numMinLength.Name = "numMinLength";
            this.numMinLength.Size = new System.Drawing.Size(120, 19);
            this.numMinLength.TabIndex = 6;
            this.numMinLength.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numRatio
            // 
            this.numRatio.Location = new System.Drawing.Point(168, 39);
            this.numRatio.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRatio.Name = "numRatio";
            this.numRatio.Size = new System.Drawing.Size(120, 19);
            this.numRatio.TabIndex = 7;
            this.numRatio.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numMinStep
            // 
            this.numMinStep.Location = new System.Drawing.Point(168, 64);
            this.numMinStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMinStep.Name = "numMinStep";
            this.numMinStep.Size = new System.Drawing.Size(120, 19);
            this.numMinStep.TabIndex = 8;
            this.numMinStep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numMaxSpace
            // 
            this.numMaxSpace.Location = new System.Drawing.Point(168, 89);
            this.numMaxSpace.Name = "numMaxSpace";
            this.numMaxSpace.Size = new System.Drawing.Size(120, 19);
            this.numMaxSpace.TabIndex = 9;
            // 
            // numHStep
            // 
            this.numHStep.Location = new System.Drawing.Point(168, 114);
            this.numHStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numHStep.Name = "numHStep";
            this.numHStep.Size = new System.Drawing.Size(120, 19);
            this.numHStep.TabIndex = 11;
            this.numHStep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "高速走査のステップ";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.numHStep);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.numMaxSpace);
            this.panel1.Controls.Add(this.numMinStep);
            this.panel1.Controls.Add(this.numRatio);
            this.panel1.Controls.Add(this.numMinLength);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(43, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(324, 170);
            this.panel1.TabIndex = 12;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabTableErase);
            this.tabControl1.Controls.Add(this.tabNoiseErase);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabExtraFrame);
            this.tabControl1.Location = new System.Drawing.Point(14, 11);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(419, 249);
            this.tabControl1.TabIndex = 13;
            // 
            // tabTableErase
            // 
            this.tabTableErase.Controls.Add(this.panel1);
            this.tabTableErase.Location = new System.Drawing.Point(4, 22);
            this.tabTableErase.Name = "tabTableErase";
            this.tabTableErase.Padding = new System.Windows.Forms.Padding(3);
            this.tabTableErase.Size = new System.Drawing.Size(411, 223);
            this.tabTableErase.TabIndex = 0;
            this.tabTableErase.Text = "罫線処理";
            this.tabTableErase.UseVisualStyleBackColor = true;
            // 
            // tabNoiseErase
            // 
            this.tabNoiseErase.Controls.Add(this.numMaxNoiseSize);
            this.tabNoiseErase.Controls.Add(this.chkAutoSize);
            this.tabNoiseErase.Controls.Add(this.label10);
            this.tabNoiseErase.Controls.Add(this.label9);
            this.tabNoiseErase.Controls.Add(this.cmbNoiseType);
            this.tabNoiseErase.Controls.Add(this.label8);
            this.tabNoiseErase.Controls.Add(this.cmbFastMode);
            this.tabNoiseErase.Controls.Add(this.label7);
            this.tabNoiseErase.Controls.Add(this.cmbLevel);
            this.tabNoiseErase.Controls.Add(this.label6);
            this.tabNoiseErase.Controls.Add(this.cmbDocType);
            this.tabNoiseErase.Location = new System.Drawing.Point(4, 22);
            this.tabNoiseErase.Name = "tabNoiseErase";
            this.tabNoiseErase.Padding = new System.Windows.Forms.Padding(3);
            this.tabNoiseErase.Size = new System.Drawing.Size(411, 223);
            this.tabNoiseErase.TabIndex = 1;
            this.tabNoiseErase.Text = "ノイズ除去";
            this.tabNoiseErase.UseVisualStyleBackColor = true;
            // 
            // numMaxNoiseSize
            // 
            this.numMaxNoiseSize.Location = new System.Drawing.Point(205, 122);
            this.numMaxNoiseSize.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numMaxNoiseSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxNoiseSize.Name = "numMaxNoiseSize";
            this.numMaxNoiseSize.Size = new System.Drawing.Size(120, 19);
            this.numMaxNoiseSize.TabIndex = 12;
            this.numMaxNoiseSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkAutoSize
            // 
            this.chkAutoSize.AutoSize = true;
            this.chkAutoSize.Location = new System.Drawing.Point(116, 125);
            this.chkAutoSize.Name = "chkAutoSize";
            this.chkAutoSize.Size = new System.Drawing.Size(48, 16);
            this.chkAutoSize.TabIndex = 9;
            this.chkAutoSize.Text = "自動";
            this.chkAutoSize.UseVisualStyleBackColor = true;
            this.chkAutoSize.CheckedChanged += new System.EventHandler(this.chkAutoSize_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 125);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 12);
            this.label10.TabIndex = 8;
            this.label10.Text = "最大ノイズサイズ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 92);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 12);
            this.label9.TabIndex = 7;
            this.label9.Text = "ノイズタイプ";
            // 
            // cmbNoiseType
            // 
            this.cmbNoiseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNoiseType.FormattingEnabled = true;
            this.cmbNoiseType.Location = new System.Drawing.Point(116, 89);
            this.cmbNoiseType.Name = "cmbNoiseType";
            this.cmbNoiseType.Size = new System.Drawing.Size(209, 20);
            this.cmbNoiseType.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 66);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 5;
            this.label8.Text = "ファストモード";
            // 
            // cmbFastMode
            // 
            this.cmbFastMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFastMode.FormattingEnabled = true;
            this.cmbFastMode.Location = new System.Drawing.Point(116, 63);
            this.cmbFastMode.Name = "cmbFastMode";
            this.cmbFastMode.Size = new System.Drawing.Size(209, 20);
            this.cmbFastMode.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 40);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "レベル";
            // 
            // cmbLevel
            // 
            this.cmbLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLevel.FormattingEnabled = true;
            this.cmbLevel.Location = new System.Drawing.Point(116, 37);
            this.cmbLevel.Name = "cmbLevel";
            this.cmbLevel.Size = new System.Drawing.Size(209, 20);
            this.cmbLevel.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "ドキュメントタイプ";
            // 
            // cmbDocType
            // 
            this.cmbDocType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDocType.FormattingEnabled = true;
            this.cmbDocType.Location = new System.Drawing.Point(116, 11);
            this.cmbDocType.Name = "cmbDocType";
            this.cmbDocType.Size = new System.Drawing.Size(209, 20);
            this.cmbDocType.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.checkBox4);
            this.tabPage1.Controls.Add(this.checkBox3);
            this.tabPage1.Controls.Add(this.checkBox2);
            this.tabPage1.Controls.Add(this.checkBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(411, 223);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "バーコード認識";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(11, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(385, 149);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "バーコード種別";
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(19, 29);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(89, 16);
            this.checkBox4.TabIndex = 3;
            this.checkBox4.Text = "PureBarcode";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(205, 29);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(75, 16);
            this.checkBox3.TabIndex = 2;
            this.checkBox3.Text = "TryHarder";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(205, 6);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(82, 16);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "TryInverted";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(19, 7);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(82, 16);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "AutoRotate";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // tabExtraFrame
            // 
            this.tabExtraFrame.Controls.Add(this.panel2);
            this.tabExtraFrame.Location = new System.Drawing.Point(4, 22);
            this.tabExtraFrame.Name = "tabExtraFrame";
            this.tabExtraFrame.Padding = new System.Windows.Forms.Padding(3);
            this.tabExtraFrame.Size = new System.Drawing.Size(411, 223);
            this.tabExtraFrame.TabIndex = 3;
            this.tabExtraFrame.Text = "領域抽出";
            this.tabExtraFrame.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.numExtraFrameMargin);
            this.panel2.Controls.Add(this.label15);
            this.panel2.Location = new System.Drawing.Point(29, 21);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(324, 170);
            this.panel2.TabIndex = 13;
            // 
            // numExtraFrameMargin
            // 
            this.numExtraFrameMargin.Location = new System.Drawing.Point(168, 15);
            this.numExtraFrameMargin.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numExtraFrameMargin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numExtraFrameMargin.Name = "numExtraFrameMargin";
            this.numExtraFrameMargin.Size = new System.Drawing.Size(120, 19);
            this.numExtraFrameMargin.TabIndex = 6;
            this.numExtraFrameMargin.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(19, 18);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(139, 12);
            this.label15.TabIndex = 2;
            this.label15.Text = "枠領域の誤差修正しきい値";
            // 
            // EraceParamSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 310);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Name = "EraceParamSetting";
            this.Text = "パラメタ設定";
            this.Load += new System.EventHandler(this.EraceParamSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numMinLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRatio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMinStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHStep)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabTableErase.ResumeLayout(false);
            this.tabNoiseErase.ResumeLayout(false);
            this.tabNoiseErase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxNoiseSize)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabExtraFrame.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numExtraFrameMargin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numMinLength;
        private System.Windows.Forms.NumericUpDown numRatio;
        private System.Windows.Forms.NumericUpDown numMinStep;
        private System.Windows.Forms.NumericUpDown numMaxSpace;
        private System.Windows.Forms.NumericUpDown numHStep;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabTableErase;
        private System.Windows.Forms.TabPage tabNoiseErase;
        private System.Windows.Forms.ComboBox cmbDocType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbNoiseType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbFastMode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbLevel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numMaxNoiseSize;
        private System.Windows.Forms.CheckBox chkAutoSize;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage tabExtraFrame;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown numExtraFrameMargin;
        private System.Windows.Forms.Label label15;
    }
}