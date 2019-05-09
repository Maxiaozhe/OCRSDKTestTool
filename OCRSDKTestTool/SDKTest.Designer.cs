namespace OCRSDKTest
{
    partial class SDKTest
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
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.chkNoRcFlag = new System.Windows.Forms.CheckBox();
            this.chkUpperCaseRcFlag = new System.Windows.Forms.CheckBox();
            this.chkLowerCaseRcFlag = new System.Windows.Forms.CheckBox();
            this.chkKatakanaRcFlag = new System.Windows.Forms.CheckBox();
            this.chkHiraganaRcFlag = new System.Windows.Forms.CheckBox();
            this.chkJISLevel1KanjiSetRcFlag = new System.Windows.Forms.CheckBox();
            this.chkJISLevel2KanjiSetRcFlag = new System.Windows.Forms.CheckBox();
            this.chkSymbol1RcFlag = new System.Windows.Forms.CheckBox();
            this.chkSymbol2RcFlag = new System.Windows.Forms.CheckBox();
            this.chkPunctuationRcFlag = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.chkJOCR = new System.Windows.Forms.CheckBox();
            this.chkHCOR = new System.Windows.Forms.CheckBox();
            this.button5 = new System.Windows.Forms.Button();
            this.chkEraseLine = new System.Windows.Forms.CheckBox();
            this.button6 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 62);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(239, 41);
            this.button1.TabIndex = 0;
            this.button1.Text = "OCR 実行";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "画像ファイル|*.png;*.jpg;*.gif;*.tif";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(289, 95);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(268, 11);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(446, 372);
            this.panel1.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(511, 405);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(68, 25);
            this.button2.TabIndex = 3;
            this.button2.Text = "拡大";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(585, 405);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(68, 25);
            this.button3.TabIndex = 4;
            this.button3.Text = "縮小";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 114);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(239, 19);
            this.textBox1.TabIndex = 5;
            // 
            // chkNoRcFlag
            // 
            this.chkNoRcFlag.AutoSize = true;
            this.chkNoRcFlag.Location = new System.Drawing.Point(21, 177);
            this.chkNoRcFlag.Name = "chkNoRcFlag";
            this.chkNoRcFlag.Size = new System.Drawing.Size(48, 16);
            this.chkNoRcFlag.TabIndex = 6;
            this.chkNoRcFlag.Text = "数字";
            this.chkNoRcFlag.UseVisualStyleBackColor = true;
            // 
            // chkUpperCaseRcFlag
            // 
            this.chkUpperCaseRcFlag.AutoSize = true;
            this.chkUpperCaseRcFlag.Location = new System.Drawing.Point(21, 199);
            this.chkUpperCaseRcFlag.Name = "chkUpperCaseRcFlag";
            this.chkUpperCaseRcFlag.Size = new System.Drawing.Size(72, 16);
            this.chkUpperCaseRcFlag.TabIndex = 7;
            this.chkUpperCaseRcFlag.Text = "英大文字";
            this.chkUpperCaseRcFlag.UseVisualStyleBackColor = true;
            // 
            // chkLowerCaseRcFlag
            // 
            this.chkLowerCaseRcFlag.AutoSize = true;
            this.chkLowerCaseRcFlag.Location = new System.Drawing.Point(21, 221);
            this.chkLowerCaseRcFlag.Name = "chkLowerCaseRcFlag";
            this.chkLowerCaseRcFlag.Size = new System.Drawing.Size(72, 16);
            this.chkLowerCaseRcFlag.TabIndex = 8;
            this.chkLowerCaseRcFlag.Text = "英小文字";
            this.chkLowerCaseRcFlag.UseVisualStyleBackColor = true;
            // 
            // chkKatakanaRcFlag
            // 
            this.chkKatakanaRcFlag.AutoSize = true;
            this.chkKatakanaRcFlag.Location = new System.Drawing.Point(21, 243);
            this.chkKatakanaRcFlag.Name = "chkKatakanaRcFlag";
            this.chkKatakanaRcFlag.Size = new System.Drawing.Size(60, 16);
            this.chkKatakanaRcFlag.TabIndex = 9;
            this.chkKatakanaRcFlag.Text = "カタカナ";
            this.chkKatakanaRcFlag.UseVisualStyleBackColor = true;
            // 
            // chkHiraganaRcFlag
            // 
            this.chkHiraganaRcFlag.AutoSize = true;
            this.chkHiraganaRcFlag.Location = new System.Drawing.Point(21, 265);
            this.chkHiraganaRcFlag.Name = "chkHiraganaRcFlag";
            this.chkHiraganaRcFlag.Size = new System.Drawing.Size(62, 16);
            this.chkHiraganaRcFlag.TabIndex = 10;
            this.chkHiraganaRcFlag.Text = "ひらがな";
            this.chkHiraganaRcFlag.UseVisualStyleBackColor = true;
            // 
            // chkJISLevel1KanjiSetRcFlag
            // 
            this.chkJISLevel1KanjiSetRcFlag.AutoSize = true;
            this.chkJISLevel1KanjiSetRcFlag.Location = new System.Drawing.Point(21, 287);
            this.chkJISLevel1KanjiSetRcFlag.Name = "chkJISLevel1KanjiSetRcFlag";
            this.chkJISLevel1KanjiSetRcFlag.Size = new System.Drawing.Size(96, 16);
            this.chkJISLevel1KanjiSetRcFlag.TabIndex = 11;
            this.chkJISLevel1KanjiSetRcFlag.Text = "第一水準漢字";
            this.chkJISLevel1KanjiSetRcFlag.UseVisualStyleBackColor = true;
            // 
            // chkJISLevel2KanjiSetRcFlag
            // 
            this.chkJISLevel2KanjiSetRcFlag.AutoSize = true;
            this.chkJISLevel2KanjiSetRcFlag.Location = new System.Drawing.Point(21, 309);
            this.chkJISLevel2KanjiSetRcFlag.Name = "chkJISLevel2KanjiSetRcFlag";
            this.chkJISLevel2KanjiSetRcFlag.Size = new System.Drawing.Size(96, 16);
            this.chkJISLevel2KanjiSetRcFlag.TabIndex = 12;
            this.chkJISLevel2KanjiSetRcFlag.Text = "第二水準漢字";
            this.chkJISLevel2KanjiSetRcFlag.UseVisualStyleBackColor = true;
            // 
            // chkSymbol1RcFlag
            // 
            this.chkSymbol1RcFlag.AutoSize = true;
            this.chkSymbol1RcFlag.Location = new System.Drawing.Point(21, 331);
            this.chkSymbol1RcFlag.Name = "chkSymbol1RcFlag";
            this.chkSymbol1RcFlag.Size = new System.Drawing.Size(56, 16);
            this.chkSymbol1RcFlag.TabIndex = 13;
            this.chkSymbol1RcFlag.Text = "記号１";
            this.chkSymbol1RcFlag.UseVisualStyleBackColor = true;
            // 
            // chkSymbol2RcFlag
            // 
            this.chkSymbol2RcFlag.AutoSize = true;
            this.chkSymbol2RcFlag.Location = new System.Drawing.Point(21, 353);
            this.chkSymbol2RcFlag.Name = "chkSymbol2RcFlag";
            this.chkSymbol2RcFlag.Size = new System.Drawing.Size(56, 16);
            this.chkSymbol2RcFlag.TabIndex = 14;
            this.chkSymbol2RcFlag.Text = "記号２";
            this.chkSymbol2RcFlag.UseVisualStyleBackColor = true;
            // 
            // chkPunctuationRcFlag
            // 
            this.chkPunctuationRcFlag.AutoSize = true;
            this.chkPunctuationRcFlag.Location = new System.Drawing.Point(21, 375);
            this.chkPunctuationRcFlag.Name = "chkPunctuationRcFlag";
            this.chkPunctuationRcFlag.Size = new System.Drawing.Size(60, 16);
            this.chkPunctuationRcFlag.TabIndex = 15;
            this.chkPunctuationRcFlag.Text = "句読点";
            this.chkPunctuationRcFlag.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 15);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(239, 41);
            this.button4.TabIndex = 16;
            this.button4.Text = "画像選択";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // chkJOCR
            // 
            this.chkJOCR.AutoSize = true;
            this.chkJOCR.Checked = true;
            this.chkJOCR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkJOCR.Location = new System.Drawing.Point(21, 140);
            this.chkJOCR.Name = "chkJOCR";
            this.chkJOCR.Size = new System.Drawing.Size(48, 16);
            this.chkJOCR.TabIndex = 17;
            this.chkJOCR.Text = "活字";
            this.chkJOCR.UseVisualStyleBackColor = true;
            // 
            // chkHCOR
            // 
            this.chkHCOR.AutoSize = true;
            this.chkHCOR.Checked = true;
            this.chkHCOR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHCOR.Location = new System.Drawing.Point(75, 139);
            this.chkHCOR.Name = "chkHCOR";
            this.chkHCOR.Size = new System.Drawing.Size(48, 16);
            this.chkHCOR.TabIndex = 18;
            this.chkHCOR.Text = "手書";
            this.chkHCOR.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 407);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(132, 23);
            this.button5.TabIndex = 19;
            this.button5.Text = "TIFF形式変換";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // chkEraseLine
            // 
            this.chkEraseLine.AutoSize = true;
            this.chkEraseLine.Checked = true;
            this.chkEraseLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEraseLine.Location = new System.Drawing.Point(154, 139);
            this.chkEraseLine.Name = "chkEraseLine";
            this.chkEraseLine.Size = new System.Drawing.Size(72, 16);
            this.chkEraseLine.TabIndex = 20;
            this.chkEraseLine.Text = "枠線除去";
            this.chkEraseLine.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(154, 407);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(132, 23);
            this.button6.TabIndex = 21;
            this.button6.Text = "EraseRuleLines";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // SDKTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 442);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.chkEraseLine);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.chkHCOR);
            this.Controls.Add(this.chkJOCR);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.chkPunctuationRcFlag);
            this.Controls.Add(this.chkSymbol2RcFlag);
            this.Controls.Add(this.chkSymbol1RcFlag);
            this.Controls.Add(this.chkJISLevel2KanjiSetRcFlag);
            this.Controls.Add(this.chkJISLevel1KanjiSetRcFlag);
            this.Controls.Add(this.chkHiraganaRcFlag);
            this.Controls.Add(this.chkKatakanaRcFlag);
            this.Controls.Add(this.chkLowerCaseRcFlag);
            this.Controls.Add(this.chkUpperCaseRcFlag);
            this.Controls.Add(this.chkNoRcFlag);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Name = "SDKTest";
            this.Text = "伝匠テストツール";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox chkNoRcFlag;
        private System.Windows.Forms.CheckBox chkUpperCaseRcFlag;
        private System.Windows.Forms.CheckBox chkLowerCaseRcFlag;
        private System.Windows.Forms.CheckBox chkKatakanaRcFlag;
        private System.Windows.Forms.CheckBox chkHiraganaRcFlag;
        private System.Windows.Forms.CheckBox chkJISLevel1KanjiSetRcFlag;
        private System.Windows.Forms.CheckBox chkJISLevel2KanjiSetRcFlag;
        private System.Windows.Forms.CheckBox chkSymbol1RcFlag;
        private System.Windows.Forms.CheckBox chkSymbol2RcFlag;
        private System.Windows.Forms.CheckBox chkPunctuationRcFlag;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox chkJOCR;
        private System.Windows.Forms.CheckBox chkHCOR;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.CheckBox chkEraseLine;
        private System.Windows.Forms.Button button6;
    }
}