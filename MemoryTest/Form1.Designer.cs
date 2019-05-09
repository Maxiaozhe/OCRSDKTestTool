namespace MemoryTest
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.btnFromFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtCount = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(30, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(163, 73);
            this.button1.TabIndex = 0;
            this.button1.Text = "IMAGE　Dispose";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(219, 31);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(163, 73);
            this.button2.TabIndex = 1;
            this.button2.Text = "IMAGE　Using";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(402, 31);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(163, 73);
            this.button3.TabIndex = 2;
            this.button3.Text = "IMAGE　Not Dispose";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(30, 136);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(163, 73);
            this.button4.TabIndex = 3;
            this.button4.Text = "IMAGE　From Stream";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(219, 136);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(163, 73);
            this.button5.TabIndex = 4;
            this.button5.Text = "IMAGE　二値化";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(402, 136);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(163, 73);
            this.button6.TabIndex = 5;
            this.button6.Text = "TIFFエンコーダ";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // btnFromFile
            // 
            this.btnFromFile.Location = new System.Drawing.Point(30, 234);
            this.btnFromFile.Name = "btnFromFile";
            this.btnFromFile.Size = new System.Drawing.Size(163, 73);
            this.btnFromFile.TabIndex = 7;
            this.btnFromFile.Text = "FromFile";
            this.btnFromFile.UseVisualStyleBackColor = true;
            this.btnFromFile.Click += new System.EventHandler(this.button8_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "画像ファイル|*.jpg;*.tif;*.bmp;*.png";
            // 
            // txtCount
            // 
            this.txtCount.Location = new System.Drawing.Point(219, 288);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(96, 19);
            this.txtCount.TabIndex = 8;
            this.txtCount.Text = "10000";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 607);
            this.Controls.Add(this.txtCount);
            this.Controls.Add(this.btnFromFile);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button btnFromFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtCount;
    }
}

