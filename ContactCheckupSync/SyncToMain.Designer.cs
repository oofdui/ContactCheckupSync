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
            this.lvDefault = new System.Windows.Forms.ListView();
            this.clWhen = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clResult = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clHN = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clDetail = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bwDefault = new System.ComponentModel.BackgroundWorker();
            this.tmDefault = new System.Windows.Forms.Timer(this.components);
            this.bwTimer = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAddHours = new System.Windows.Forms.TextBox();
            this.tbExportPath = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSyncPathMobile = new System.Windows.Forms.Label();
            this.lblSyncPathMain = new System.Windows.Forms.Label();
            this.btSyncPathMobile = new System.Windows.Forms.Button();
            this.btSyncPathMain = new System.Windows.Forms.Button();
            this.tbDefault.SuspendLayout();
            this.tbCommand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.anLoading)).BeginInit();
            this.tbExportPath.SuspendLayout();
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
            this.tbDefault.Controls.Add(this.tbExportPath, 0, 6);
            this.tbDefault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDefault.Location = new System.Drawing.Point(0, 0);
            this.tbDefault.Margin = new System.Windows.Forms.Padding(0);
            this.tbDefault.Name = "tbDefault";
            this.tbDefault.RowCount = 8;
            this.tbDefault.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbDefault.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbDefault.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbDefault.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbDefault.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbDefault.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbDefault.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbDefault.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tbDefault.Size = new System.Drawing.Size(710, 417);
            this.tbDefault.TabIndex = 2;
            // 
            // tbCommand
            // 
            this.tbCommand.AutoSize = true;
            this.tbCommand.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tbCommand.BackColor = System.Drawing.Color.WhiteSmoke;
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
            this.tbCommand.Controls.Add(this.label1, 0, 0);
            this.tbCommand.Controls.Add(this.txtAddHours, 1, 0);
            this.tbCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCommand.Location = new System.Drawing.Point(0, 0);
            this.tbCommand.Margin = new System.Windows.Forms.Padding(0);
            this.tbCommand.Name = "tbCommand";
            this.tbCommand.Padding = new System.Windows.Forms.Padding(3);
            this.tbCommand.RowCount = 1;
            this.tbCommand.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbCommand.Size = new System.Drawing.Size(710, 35);
            this.tbCommand.TabIndex = 0;
            // 
            // btStart
            // 
            this.btStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btStart.Location = new System.Drawing.Point(548, 6);
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
            this.btStop.Enabled = false;
            this.btStop.Location = new System.Drawing.Point(629, 6);
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
            this.pbDefault.Location = new System.Drawing.Point(3, 137);
            this.pbDefault.Name = "pbDefault";
            this.pbDefault.Size = new System.Drawing.Size(704, 20);
            this.pbDefault.TabIndex = 3;
            this.pbDefault.Visible = false;
            // 
            // anLoading
            // 
            this.anLoading.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.anLoading.Image = global::_ContactCheckupSync.Properties.Resources.anLoading;
            this.anLoading.Location = new System.Drawing.Point(3, 38);
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
            this.lblDefault.Location = new System.Drawing.Point(3, 121);
            this.lblDefault.Name = "lblDefault";
            this.lblDefault.Size = new System.Drawing.Size(704, 13);
            this.lblDefault.TabIndex = 5;
            this.lblDefault.Text = "- รอดำเนินการ -";
            this.lblDefault.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lvDefault
            // 
            this.lvDefault.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clWhen,
            this.clResult,
            this.clHN,
            this.clDetail});
            this.lvDefault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvDefault.Location = new System.Drawing.Point(3, 163);
            this.lvDefault.Name = "lvDefault";
            this.lvDefault.Size = new System.Drawing.Size(704, 212);
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
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "จำนวนชั่วโมงที่ใช้ดึงย้อนหลัง";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtAddHours
            // 
            this.txtAddHours.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtAddHours.Location = new System.Drawing.Point(153, 7);
            this.txtAddHours.Name = "txtAddHours";
            this.txtAddHours.Size = new System.Drawing.Size(50, 20);
            this.txtAddHours.TabIndex = 6;
            // 
            // tbExportPath
            // 
            this.tbExportPath.AutoSize = true;
            this.tbExportPath.ColumnCount = 6;
            this.tbExportPath.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbExportPath.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbExportPath.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbExportPath.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbExportPath.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tbExportPath.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbExportPath.Controls.Add(this.label2, 0, 0);
            this.tbExportPath.Controls.Add(this.label3, 3, 0);
            this.tbExportPath.Controls.Add(this.lblSyncPathMobile, 1, 0);
            this.tbExportPath.Controls.Add(this.lblSyncPathMain, 4, 0);
            this.tbExportPath.Controls.Add(this.btSyncPathMobile, 2, 0);
            this.tbExportPath.Controls.Add(this.btSyncPathMain, 5, 0);
            this.tbExportPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbExportPath.Location = new System.Drawing.Point(0, 378);
            this.tbExportPath.Margin = new System.Windows.Forms.Padding(0);
            this.tbExportPath.Name = "tbExportPath";
            this.tbExportPath.RowCount = 1;
            this.tbExportPath.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbExportPath.Size = new System.Drawing.Size(710, 29);
            this.tbExportPath.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "SyncPath@Mobile";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(362, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "SyncPath@Main";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSyncPathMobile
            // 
            this.lblSyncPathMobile.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSyncPathMobile.AutoSize = true;
            this.lblSyncPathMobile.Location = new System.Drawing.Point(104, 8);
            this.lblSyncPathMobile.Name = "lblSyncPathMobile";
            this.lblSyncPathMobile.Size = new System.Drawing.Size(10, 13);
            this.lblSyncPathMobile.TabIndex = 5;
            this.lblSyncPathMobile.Text = "-";
            this.lblSyncPathMobile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSyncPathMain
            // 
            this.lblSyncPathMain.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSyncPathMain.AutoSize = true;
            this.lblSyncPathMain.Location = new System.Drawing.Point(455, 8);
            this.lblSyncPathMain.Name = "lblSyncPathMain";
            this.lblSyncPathMain.Size = new System.Drawing.Size(10, 13);
            this.lblSyncPathMain.TabIndex = 5;
            this.lblSyncPathMain.Text = "-";
            this.lblSyncPathMain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btSyncPathMobile
            // 
            this.btSyncPathMobile.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btSyncPathMobile.Location = new System.Drawing.Point(316, 3);
            this.btSyncPathMobile.Name = "btSyncPathMobile";
            this.btSyncPathMobile.Size = new System.Drawing.Size(40, 23);
            this.btSyncPathMobile.TabIndex = 6;
            this.btSyncPathMobile.Text = "View";
            this.btSyncPathMobile.UseVisualStyleBackColor = true;
            this.btSyncPathMobile.Click += new System.EventHandler(this.btSyncPathMobile_Click);
            // 
            // btSyncPathMain
            // 
            this.btSyncPathMain.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btSyncPathMain.Location = new System.Drawing.Point(667, 3);
            this.btSyncPathMain.Name = "btSyncPathMain";
            this.btSyncPathMain.Size = new System.Drawing.Size(40, 23);
            this.btSyncPathMain.TabIndex = 6;
            this.btSyncPathMain.Text = "View";
            this.btSyncPathMain.UseVisualStyleBackColor = true;
            this.btSyncPathMain.Click += new System.EventHandler(this.btSyncPathMain_Click);
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
            this.tbExportPath.ResumeLayout(false);
            this.tbExportPath.PerformLayout();
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAddHours;
        private System.Windows.Forms.TableLayoutPanel tbExportPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSyncPathMobile;
        private System.Windows.Forms.Label lblSyncPathMain;
        private System.Windows.Forms.Button btSyncPathMobile;
        private System.Windows.Forms.Button btSyncPathMain;
    }
}