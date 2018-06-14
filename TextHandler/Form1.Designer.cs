namespace TextHandler
{
    partial class Form1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.VoiceList = new System.Windows.Forms.CheckedListBox();
            this.OpenXlxs = new System.Windows.Forms.Button();
            this.LoadScr = new System.Windows.Forms.Button();
            this.ScrCount = new System.Windows.Forms.Label();
            this.CountS = new System.Windows.Forms.Label();
            this.Start = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.OpenXlxs);
            this.panel1.Controls.Add(this.VoiceList);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 191);
            this.panel1.TabIndex = 0;
            // 
            // VoiceList
            // 
            this.VoiceList.CheckOnClick = true;
            this.VoiceList.FormattingEnabled = true;
            this.VoiceList.Location = new System.Drawing.Point(3, 36);
            this.VoiceList.Name = "VoiceList";
            this.VoiceList.Size = new System.Drawing.Size(194, 148);
            this.VoiceList.TabIndex = 0;
            // 
            // OpenXlxs
            // 
            this.OpenXlxs.Location = new System.Drawing.Point(13, 7);
            this.OpenXlxs.Name = "OpenXlxs";
            this.OpenXlxs.Size = new System.Drawing.Size(75, 23);
            this.OpenXlxs.TabIndex = 1;
            this.OpenXlxs.Text = "载入语音表";
            this.OpenXlxs.UseVisualStyleBackColor = true;
            this.OpenXlxs.Click += new System.EventHandler(this.OpenXlxs_Click);
            // 
            // LoadScr
            // 
            this.LoadScr.Location = new System.Drawing.Point(12, 209);
            this.LoadScr.Name = "LoadScr";
            this.LoadScr.Size = new System.Drawing.Size(75, 23);
            this.LoadScr.TabIndex = 2;
            this.LoadScr.Text = "载入脚本";
            this.LoadScr.UseVisualStyleBackColor = true;
            this.LoadScr.Click += new System.EventHandler(this.LoadScr_Click);
            // 
            // ScrCount
            // 
            this.ScrCount.AutoSize = true;
            this.ScrCount.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ScrCount.Location = new System.Drawing.Point(12, 239);
            this.ScrCount.Name = "ScrCount";
            this.ScrCount.Size = new System.Drawing.Size(139, 20);
            this.ScrCount.TabIndex = 3;
            this.ScrCount.Text = "已载入脚本数:";
            // 
            // CountS
            // 
            this.CountS.AutoSize = true;
            this.CountS.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CountS.Location = new System.Drawing.Point(143, 239);
            this.CountS.Name = "CountS";
            this.CountS.Size = new System.Drawing.Size(0, 20);
            this.CountS.TabIndex = 4;
            // 
            // Start
            // 
            this.Start.Location = new System.Drawing.Point(12, 262);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(131, 27);
            this.Start.TabIndex = 5;
            this.Start.Text = "运行";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 301);
            this.Controls.Add(this.Start);
            this.Controls.Add(this.CountS);
            this.Controls.Add(this.ScrCount);
            this.Controls.Add(this.LoadScr);
            this.Controls.Add(this.panel1);
            this.MaximumSize = new System.Drawing.Size(250, 340);
            this.MinimumSize = new System.Drawing.Size(250, 340);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckedListBox VoiceList;
        private System.Windows.Forms.Button OpenXlxs;
        private System.Windows.Forms.Button LoadScr;
        private System.Windows.Forms.Label ScrCount;
        private System.Windows.Forms.Label CountS;
        private System.Windows.Forms.Button Start;
    }
}