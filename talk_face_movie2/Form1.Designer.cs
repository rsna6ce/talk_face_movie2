
namespace talk_face_movie2
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
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.buttonRun = new System.Windows.Forms.Button();
            this.buttonInputfile = new System.Windows.Forms.Button();
            this.buttonOutputfile = new System.Windows.Forms.Button();
            this.buttonImageDir = new System.Windows.Forms.Button();
            this.textBoxInputfile = new System.Windows.Forms.TextBox();
            this.textBoxOutputfile = new System.Windows.Forms.TextBox();
            this.textBoxImagedir = new System.Windows.Forms.TextBox();
            this.numericUpDownFramerate = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownBlinkInterval = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownLargeThreshold = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownSmallThreshold = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxFfmpeg = new System.Windows.Forms.TextBox();
            this.buttonFfmpeg = new System.Windows.Forms.Button();
            this.labelProgressbar = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFramerate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBlinkInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLargeThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSmallThreshold)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonRun
            // 
            this.buttonRun.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRun.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.buttonRun.Location = new System.Drawing.Point(428, 112);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(571, 44);
            this.buttonRun.TabIndex = 0;
            this.buttonRun.Text = "変換開始";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonInputfile
            // 
            this.buttonInputfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInputfile.Location = new System.Drawing.Point(971, 4);
            this.buttonInputfile.Name = "buttonInputfile";
            this.buttonInputfile.Size = new System.Drawing.Size(28, 23);
            this.buttonInputfile.TabIndex = 1;
            this.buttonInputfile.Text = "...";
            this.buttonInputfile.UseVisualStyleBackColor = true;
            this.buttonInputfile.Click += new System.EventHandler(this.buttonInputfile_Click);
            // 
            // buttonOutputfile
            // 
            this.buttonOutputfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOutputfile.Location = new System.Drawing.Point(971, 32);
            this.buttonOutputfile.Name = "buttonOutputfile";
            this.buttonOutputfile.Size = new System.Drawing.Size(28, 23);
            this.buttonOutputfile.TabIndex = 2;
            this.buttonOutputfile.Text = "...";
            this.buttonOutputfile.UseVisualStyleBackColor = true;
            this.buttonOutputfile.Click += new System.EventHandler(this.buttonOutputfile_Click);
            // 
            // buttonImageDir
            // 
            this.buttonImageDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonImageDir.Location = new System.Drawing.Point(971, 60);
            this.buttonImageDir.Name = "buttonImageDir";
            this.buttonImageDir.Size = new System.Drawing.Size(28, 23);
            this.buttonImageDir.TabIndex = 3;
            this.buttonImageDir.Text = "...";
            this.buttonImageDir.UseVisualStyleBackColor = true;
            this.buttonImageDir.Click += new System.EventHandler(this.buttonImageDir_Click);
            // 
            // textBoxInputfile
            // 
            this.textBoxInputfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInputfile.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxInputfile.Location = new System.Drawing.Point(128, 8);
            this.textBoxInputfile.Name = "textBoxInputfile";
            this.textBoxInputfile.Size = new System.Drawing.Size(839, 19);
            this.textBoxInputfile.TabIndex = 4;
            // 
            // textBoxOutputfile
            // 
            this.textBoxOutputfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutputfile.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxOutputfile.Location = new System.Drawing.Point(128, 36);
            this.textBoxOutputfile.Name = "textBoxOutputfile";
            this.textBoxOutputfile.Size = new System.Drawing.Size(839, 19);
            this.textBoxOutputfile.TabIndex = 5;
            // 
            // textBoxImagedir
            // 
            this.textBoxImagedir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxImagedir.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxImagedir.Location = new System.Drawing.Point(128, 60);
            this.textBoxImagedir.Name = "textBoxImagedir";
            this.textBoxImagedir.Size = new System.Drawing.Size(839, 19);
            this.textBoxImagedir.TabIndex = 6;
            this.textBoxImagedir.Text = "image\\";
            // 
            // numericUpDownFramerate
            // 
            this.numericUpDownFramerate.Location = new System.Drawing.Point(128, 112);
            this.numericUpDownFramerate.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDownFramerate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownFramerate.Name = "numericUpDownFramerate";
            this.numericUpDownFramerate.Size = new System.Drawing.Size(72, 19);
            this.numericUpDownFramerate.TabIndex = 7;
            this.numericUpDownFramerate.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // numericUpDownBlinkInterval
            // 
            this.numericUpDownBlinkInterval.Location = new System.Drawing.Point(128, 140);
            this.numericUpDownBlinkInterval.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownBlinkInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownBlinkInterval.Name = "numericUpDownBlinkInterval";
            this.numericUpDownBlinkInterval.Size = new System.Drawing.Size(72, 19);
            this.numericUpDownBlinkInterval.TabIndex = 8;
            this.numericUpDownBlinkInterval.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // numericUpDownLargeThreshold
            // 
            this.numericUpDownLargeThreshold.Location = new System.Drawing.Point(352, 112);
            this.numericUpDownLargeThreshold.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownLargeThreshold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownLargeThreshold.Name = "numericUpDownLargeThreshold";
            this.numericUpDownLargeThreshold.Size = new System.Drawing.Size(72, 19);
            this.numericUpDownLargeThreshold.TabIndex = 9;
            this.numericUpDownLargeThreshold.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // numericUpDownSmallThreshold
            // 
            this.numericUpDownSmallThreshold.Location = new System.Drawing.Point(352, 140);
            this.numericUpDownSmallThreshold.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDownSmallThreshold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownSmallThreshold.Name = "numericUpDownSmallThreshold";
            this.numericUpDownSmallThreshold.Size = new System.Drawing.Size(72, 19);
            this.numericUpDownSmallThreshold.TabIndex = 10;
            this.numericUpDownSmallThreshold.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "フレームレート [fps]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "瞬き間隔 [ms]";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(252, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "大きい口音量 [%]";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(252, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "小さい口音量 [%]";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "入力ファイル(wav/mp3)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 12);
            this.label6.TabIndex = 16;
            this.label6.Text = "出力ファイル(mp4)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 12);
            this.label7.TabIndex = 17;
            this.label7.Text = "顔画像フォルダ";
            // 
            // textBoxLog
            // 
            this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLog.Location = new System.Drawing.Point(8, 164);
            this.textBoxLog.MaxLength = 9999999;
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(991, 192);
            this.textBoxLog.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 88);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 19;
            this.label8.Text = "ffmpegパス";
            // 
            // textBoxFfmpeg
            // 
            this.textBoxFfmpeg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFfmpeg.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxFfmpeg.Location = new System.Drawing.Point(128, 84);
            this.textBoxFfmpeg.Name = "textBoxFfmpeg";
            this.textBoxFfmpeg.Size = new System.Drawing.Size(839, 19);
            this.textBoxFfmpeg.TabIndex = 20;
            this.textBoxFfmpeg.Text = "ffmpeg.exe";
            // 
            // buttonFfmpeg
            // 
            this.buttonFfmpeg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFfmpeg.Location = new System.Drawing.Point(971, 84);
            this.buttonFfmpeg.Name = "buttonFfmpeg";
            this.buttonFfmpeg.Size = new System.Drawing.Size(28, 23);
            this.buttonFfmpeg.TabIndex = 21;
            this.buttonFfmpeg.Text = "...";
            this.buttonFfmpeg.UseVisualStyleBackColor = true;
            this.buttonFfmpeg.Click += new System.EventHandler(this.buttonFfmpeg_Click);
            // 
            // labelProgressbar
            // 
            this.labelProgressbar.BackColor = System.Drawing.Color.Red;
            this.labelProgressbar.Location = new System.Drawing.Point(428, 156);
            this.labelProgressbar.Name = "labelProgressbar";
            this.labelProgressbar.Size = new System.Drawing.Size(568, 4);
            this.labelProgressbar.TabIndex = 22;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 361);
            this.Controls.Add(this.labelProgressbar);
            this.Controls.Add(this.buttonFfmpeg);
            this.Controls.Add(this.textBoxFfmpeg);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownSmallThreshold);
            this.Controls.Add(this.numericUpDownLargeThreshold);
            this.Controls.Add(this.numericUpDownBlinkInterval);
            this.Controls.Add(this.numericUpDownFramerate);
            this.Controls.Add(this.textBoxImagedir);
            this.Controls.Add(this.textBoxOutputfile);
            this.Controls.Add(this.textBoxInputfile);
            this.Controls.Add(this.buttonImageDir);
            this.Controls.Add(this.buttonOutputfile);
            this.Controls.Add(this.buttonInputfile);
            this.Controls.Add(this.buttonRun);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "talk_face_movie2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFramerate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownBlinkInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLargeThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSmallThreshold)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Button buttonInputfile;
        private System.Windows.Forms.Button buttonOutputfile;
        private System.Windows.Forms.Button buttonImageDir;
        private System.Windows.Forms.TextBox textBoxInputfile;
        private System.Windows.Forms.TextBox textBoxOutputfile;
        private System.Windows.Forms.TextBox textBoxImagedir;
        private System.Windows.Forms.NumericUpDown numericUpDownFramerate;
        private System.Windows.Forms.NumericUpDown numericUpDownBlinkInterval;
        private System.Windows.Forms.NumericUpDown numericUpDownLargeThreshold;
        private System.Windows.Forms.NumericUpDown numericUpDownSmallThreshold;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxFfmpeg;
        private System.Windows.Forms.Button buttonFfmpeg;
        private System.Windows.Forms.Label labelProgressbar;
    }
}

