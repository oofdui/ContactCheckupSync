namespace _ContactCheckupSync
{
    partial class Report
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
            this.tbReport = new System.Windows.Forms.TableLayoutPanel();
            this.lblDefault = new System.Windows.Forms.Label();
            this.tbSyncToMobileSearch = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.dtDOEFrom = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtDOETo = new System.Windows.Forms.DateTimePicker();
            this.ddlCompany = new System.Windows.Forms.ComboBox();
            this.btSearch = new System.Windows.Forms.Button();
            this.btExport = new System.Windows.Forms.Button();
            this.btLabExport = new System.Windows.Forms.Button();
            this.gvDefault = new System.Windows.Forms.DataGridView();
            this.pbDefault = new System.Windows.Forms.ProgressBar();
            this.anWaiting = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.wbSearch = new System.ComponentModel.BackgroundWorker();
            this.tbReport.SuspendLayout();
            this.tbSyncToMobileSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvDefault)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.anWaiting)).BeginInit();
            this.SuspendLayout();
            // 
            // tbReport
            // 
            this.tbReport.AutoSize = true;
            this.tbReport.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tbReport.ColumnCount = 1;
            this.tbReport.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbReport.Controls.Add(this.lblDefault, 0, 1);
            this.tbReport.Controls.Add(this.tbSyncToMobileSearch, 0, 0);
            this.tbReport.Controls.Add(this.gvDefault, 0, 4);
            this.tbReport.Controls.Add(this.pbDefault, 0, 2);
            this.tbReport.Controls.Add(this.anWaiting, 0, 3);
            this.tbReport.Controls.Add(this.label3, 0, 5);
            this.tbReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbReport.Location = new System.Drawing.Point(0, 0);
            this.tbReport.Margin = new System.Windows.Forms.Padding(0);
            this.tbReport.Name = "tbReport";
            this.tbReport.RowCount = 6;
            this.tbReport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbReport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbReport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbReport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbReport.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbReport.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbReport.Size = new System.Drawing.Size(826, 424);
            this.tbReport.TabIndex = 2;
            // 
            // lblDefault
            // 
            this.lblDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDefault.AutoSize = true;
            this.lblDefault.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(201)))), ((int)(((byte)(14)))));
            this.lblDefault.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblDefault.Location = new System.Drawing.Point(3, 29);
            this.lblDefault.Name = "lblDefault";
            this.lblDefault.Padding = new System.Windows.Forms.Padding(5);
            this.lblDefault.Size = new System.Drawing.Size(820, 27);
            this.lblDefault.TabIndex = 2;
            this.lblDefault.Text = "- โปรดเลือกช่วงวัน และ บริษัทก่อน -";
            this.lblDefault.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbSyncToMobileSearch
            // 
            this.tbSyncToMobileSearch.AutoSize = true;
            this.tbSyncToMobileSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tbSyncToMobileSearch.ColumnCount = 8;
            this.tbSyncToMobileSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbSyncToMobileSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbSyncToMobileSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbSyncToMobileSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbSyncToMobileSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbSyncToMobileSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbSyncToMobileSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbSyncToMobileSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbSyncToMobileSearch.Controls.Add(this.label1, 0, 0);
            this.tbSyncToMobileSearch.Controls.Add(this.dtDOEFrom, 1, 0);
            this.tbSyncToMobileSearch.Controls.Add(this.label2, 2, 0);
            this.tbSyncToMobileSearch.Controls.Add(this.dtDOETo, 3, 0);
            this.tbSyncToMobileSearch.Controls.Add(this.ddlCompany, 4, 0);
            this.tbSyncToMobileSearch.Controls.Add(this.btSearch, 5, 0);
            this.tbSyncToMobileSearch.Controls.Add(this.btExport, 6, 0);
            this.tbSyncToMobileSearch.Controls.Add(this.btLabExport, 7, 0);
            this.tbSyncToMobileSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSyncToMobileSearch.Location = new System.Drawing.Point(0, 0);
            this.tbSyncToMobileSearch.Margin = new System.Windows.Forms.Padding(0);
            this.tbSyncToMobileSearch.Name = "tbSyncToMobileSearch";
            this.tbSyncToMobileSearch.RowCount = 1;
            this.tbSyncToMobileSearch.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbSyncToMobileSearch.Size = new System.Drawing.Size(826, 29);
            this.tbSyncToMobileSearch.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "DOE";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtDOEFrom
            // 
            this.dtDOEFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.dtDOEFrom.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtDOEFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDOEFrom.Location = new System.Drawing.Point(39, 3);
            this.dtDOEFrom.Name = "dtDOEFrom";
            this.dtDOEFrom.Size = new System.Drawing.Size(133, 20);
            this.dtDOEFrom.TabIndex = 1;
            this.dtDOEFrom.ValueChanged += new System.EventHandler(this.dtDOEFrom_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(178, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(10, 29);
            this.label2.TabIndex = 0;
            this.label2.Text = "-";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtDOETo
            // 
            this.dtDOETo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.dtDOETo.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtDOETo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDOETo.Location = new System.Drawing.Point(194, 3);
            this.dtDOETo.Name = "dtDOETo";
            this.dtDOETo.Size = new System.Drawing.Size(133, 20);
            this.dtDOETo.TabIndex = 1;
            this.dtDOETo.ValueChanged += new System.EventHandler(this.dtDOETo_ValueChanged);
            // 
            // ddlCompany
            // 
            this.ddlCompany.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlCompany.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCompany.FormattingEnabled = true;
            this.ddlCompany.Location = new System.Drawing.Point(333, 3);
            this.ddlCompany.Name = "ddlCompany";
            this.ddlCompany.Size = new System.Drawing.Size(247, 21);
            this.ddlCompany.TabIndex = 2;
            // 
            // btSearch
            // 
            this.btSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btSearch.Location = new System.Drawing.Point(586, 3);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(75, 23);
            this.btSearch.TabIndex = 3;
            this.btSearch.Text = "Search";
            this.btSearch.UseVisualStyleBackColor = true;
            this.btSearch.Click += new System.EventHandler(this.btSearch_Click);
            // 
            // btExport
            // 
            this.btExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btExport.Enabled = false;
            this.btExport.Location = new System.Drawing.Point(667, 3);
            this.btExport.Name = "btExport";
            this.btExport.Size = new System.Drawing.Size(75, 23);
            this.btExport.TabIndex = 4;
            this.btExport.Text = "Export";
            this.btExport.UseVisualStyleBackColor = true;
            this.btExport.Click += new System.EventHandler(this.btExport_Click);
            // 
            // btLabExport
            // 
            this.btLabExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btLabExport.Location = new System.Drawing.Point(748, 3);
            this.btLabExport.Name = "btLabExport";
            this.btLabExport.Size = new System.Drawing.Size(75, 23);
            this.btLabExport.TabIndex = 4;
            this.btLabExport.Text = "LabExport";
            this.btLabExport.UseVisualStyleBackColor = true;
            this.btLabExport.Click += new System.EventHandler(this.btLabExport_Click);
            // 
            // gvDefault
            // 
            this.gvDefault.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gvDefault.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.gvDefault.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gvDefault.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvDefault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvDefault.GridColor = System.Drawing.Color.Silver;
            this.gvDefault.Location = new System.Drawing.Point(3, 149);
            this.gvDefault.MultiSelect = false;
            this.gvDefault.Name = "gvDefault";
            this.gvDefault.ShowEditingIcon = false;
            this.gvDefault.Size = new System.Drawing.Size(820, 253);
            this.gvDefault.TabIndex = 1;
            // 
            // pbDefault
            // 
            this.pbDefault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbDefault.Location = new System.Drawing.Point(3, 59);
            this.pbDefault.Name = "pbDefault";
            this.pbDefault.Size = new System.Drawing.Size(820, 20);
            this.pbDefault.TabIndex = 3;
            this.pbDefault.Visible = false;
            // 
            // anWaiting
            // 
            this.anWaiting.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.anWaiting.Image = global::_ContactCheckupSync.Properties.Resources.anLoading;
            this.anWaiting.Location = new System.Drawing.Point(0, 82);
            this.anWaiting.Margin = new System.Windows.Forms.Padding(0);
            this.anWaiting.Name = "anWaiting";
            this.anWaiting.Size = new System.Drawing.Size(826, 64);
            this.anWaiting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.anWaiting.TabIndex = 4;
            this.anWaiting.TabStop = false;
            this.anWaiting.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label3.Location = new System.Drawing.Point(3, 405);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(3);
            this.label3.Size = new System.Drawing.Size(405, 19);
            this.label3.TabIndex = 5;
            this.label3.Text = "Highlight สีส้ม : ซิงค์สถานะการลงทะเบียนสู่ระบบหลัก เพื่อรอทำ ConvertPreOrder แล้" +
    "ว";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // wbSearch
            // 
            this.wbSearch.DoWork += new System.ComponentModel.DoWorkEventHandler(this.wsSearch_DoWork);
            // 
            // Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(826, 424);
            this.Controls.Add(this.tbReport);
            this.Name = "Report";
            this.Text = "Report";
            this.Load += new System.EventHandler(this.Report_Load);
            this.tbReport.ResumeLayout(false);
            this.tbReport.PerformLayout();
            this.tbSyncToMobileSearch.ResumeLayout(false);
            this.tbSyncToMobileSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvDefault)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.anWaiting)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tbReport;
        private System.Windows.Forms.Label lblDefault;
        private System.Windows.Forms.TableLayoutPanel tbSyncToMobileSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtDOEFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtDOETo;
        private System.Windows.Forms.ComboBox ddlCompany;
        private System.Windows.Forms.Button btSearch;
        private System.Windows.Forms.Button btExport;
        private System.Windows.Forms.DataGridView gvDefault;
        private System.Windows.Forms.ProgressBar pbDefault;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btLabExport;
        private System.Windows.Forms.PictureBox anWaiting;
        private System.ComponentModel.BackgroundWorker wbSearch;
        private System.Windows.Forms.Label label3;
    }
}