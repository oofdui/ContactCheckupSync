namespace _ContactCheckupSync
{
    partial class SyncToMain
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
            this.tbDefault = new System.Windows.Forms.TableLayoutPanel();
            this.tbCommand = new System.Windows.Forms.TableLayoutPanel();
            this.btStart = new System.Windows.Forms.Button();
            this.btStop = new System.Windows.Forms.Button();
            this.pbDefault = new System.Windows.Forms.ProgressBar();
            this.anLoading = new System.Windows.Forms.PictureBox();
            this.lblDefault = new System.Windows.Forms.Label();
            this.bwDefault = new System.ComponentModel.BackgroundWorker();
            this.tmDefault = new System.Windows.Forms.Timer(this.components);
            this.bwTimer = new System.ComponentModel.BackgroundWorker();
            this.lvDefault = new System.Windows.Forms.ListView();
            this.clWhen = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clResult = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clHN = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clDetail = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbDefault.SuspendLayout();
            this.tbCommand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.anLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // tbDefault
            // 
            this.tbDefault.AutoSize = true;
            this.tbDefault.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tbDefault.ColumnCount = 1;
            this.tbDefault.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbDefault.Controls.Add(this.tbCommand, 0, 0);
            this.tbDefault.Controls.Add(this.pbDefault, 0, 4);
            this.tbDefault.Controls.Add(this.anLoading, 0, 1);
            this.tbDefault.Controls.Add(this.lblDefault, 0, 2);
            this.tbDefault.Controls.Add(this.lvDefault, 0, 5);
            this.tbDefault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDefault.Location = new System.Drawing.Point(0, 0);
            this.tbDefault.Margin = new System.Windows.Forms.Padding(0);
            this.tbDefault.Name = "tbDefault";
            this.tbDefault.RowCount = 6;
            this.tbDefault.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbDefault.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbDefault.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbDefault.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbDefault.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbDefault.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbDefault.Size = new System.Drawing.Size(710, 417);
            this.tbDefault.TabIndex = 2;
            // 
            // tbCommand
            // 
            this.tbCommand.AutoSize = true;
            this.tbCommand.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tbCommand.ColumnCount = 7;
            this.tbCommand.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbCommand.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbCommand.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbCommand.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbCommand.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbCommand.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbCommand.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbCommand.Controls.Add(this.btStart, 5, 0);
            this.tbCommand.Controls.Add(this.btStop, 6, 0);
            this.tbCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCommand.Location = new System.Drawing.Point(0, 0);
            this.tbCommand.Margin = new System.Windows.Forms.Padding(0);
            this.tbCommand.Name = "tbCommand";
            this.tbCommand.RowCount = 1;
            this.tbCommand.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbCommand.Size = new System.Drawing.Size(710, 29);
            this.tbCommand.TabIndex = 0;
            // 
            // btStart
            // 
            this.btStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btStart.AutoSize = true;
            this.btStart.Location = new System.Drawing.Point(551, 3);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(75, 23);
            this.btStart.TabIndex = 3;
            this.btStart.Text = "START";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // btStop
            // 
            this.btStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btStop.AutoSize = true;
            this.btStop.Enabled = false;
            this.btStop.Location = new System.Drawing.Point(632, 3);
            this.btStop.Name = "btStop";
            this.btStop.Size = new System.Drawing.Size(75, 23);
            this.btStop.TabIndex = 4;
            this.btStop.Text = "STOP";
            this.btStop.UseVisualStyleBackColor = true;
            this.btStop.Click += new System.EventHandler(this.btStop_Click);
            // 
            // pbDefault
            // 
            this.pbDefault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbDefault.Location = new System.Drawing.Point(3, 131);
            this.pbDefault.Name = "pbDefault";
            this.pbDefault.Size = new System.Drawing.Size(704, 20);
            this.pbDefault.TabIndex = 3;
            this.pbDefault.Visible = false;
            // 
            // anLoading
            // 
            this.anLoading.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.anLoading.Image = global::_ContactCheckupSync.Properties.Resources.anLoading;
            this.anLoading.Location = new System.Drawing.Point(3, 32);
            this.anLoading.Name = "anLoading";
            this.anLoading.Size = new System.Drawing.Size(704, 80);
            this.anLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.anLoading.TabIndex = 4;
            this.anLoading.TabStop = false;
            this.anLoading.Visible = false;
            // 
            // lblDefault
            // 
            this.lblDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDefault.AutoSize = true;
            this.lblDefault.Location = new System.Drawing.Point(3, 115);
            this.lblDefault.Name = "lblDefault";
            this.lblDefault.Size = new System.Drawing.Size(704, 13);
            this.lblDefault.TabIndex = 5;
            this.lblDefault.Text = "- รอดำเนินการ -";
            this.lblDefault.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bwDefault
            // 
            this.bwDefault.WorkerSupportsCancellation = true;
            this.bwDefault.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwDefault_DoWork);
            // 
            // tmDefault
            // 
            this.tmDefault.Interval = 1000;
            this.tmDefault.Tick += new System.EventHandler(this.tmDefault_Tick);
            // 
            // bwTimer
            // 
            this.bwTimer.WorkerSupportsCancellation = true;
            this.bwTimer.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwTimer_DoWork);
            // 
            // lvDefault
            // 
            this.lvDefault.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clWhen,
            this.clResult,
            this.clHN,
            this.clDetail});
            this.lvDefault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvDefault.Location = new System.Drawing.Point(3, 157);
            this.lvDefault.Name = "lvDefault";
            this.lvDefault.Size = new System.Drawing.Size(704, 257);
            this.lvDefault.TabIndex = 6;
            this.lvDefault.UseCompatibleStateImageBehavior = false;
            this.lvDefault.View = System.Windows.Forms.View.Details;
            // 
            // clWhen
            // 
            this.clWhen.Text = "When";
            // 
            // clResult
            // 
            this.clResult.Text = "Result";
            this.clResult.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // clHN
            // 
            this.clHN.Text = "HN";
            this.clHN.Width = 100;
            // 
            // clDetail
            // 
            this.clDetail.Text = "Detail";
            // 
            // SyncToMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(710, 417);
            this.Controls.Add(this.tbDefault);
            this.Name = "SyncToMain";
            this.Text = "SyncToMain";
            this.Load += new System.EventHandler(this.SyncToMain_Load);
            this.tbDefault.ResumeLayout(false);
            this.tbDefault.PerformLayout();
            this.tbCommand.ResumeLayout(false);
            this.tbCommand.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.anLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tbDefault;
        private System.Windows.Forms.TableLayoutPanel tbCommand;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Button btStop;
        private System.Windows.Forms.ProgressBar pbDefault;
        private System.Windows.Forms.PictureBox anLoading;
        private System.Windows.Forms.Label lblDefault;
        private System.ComponentModel.BackgroundWorker bwDefault;
        private System.Windows.Forms.Timer tmDefault;
        private System.ComponentModel.BackgroundWorker bwTimer;
        private System.Windows.Forms.ListView lvDefault;
        private System.Windows.Forms.ColumnHeader clWhen;
        private System.Windows.Forms.ColumnHeader clResult;
        private System.Windows.Forms.ColumnHeader clHN;
        private System.Windows.Forms.ColumnHeader clDetail;
    }
}