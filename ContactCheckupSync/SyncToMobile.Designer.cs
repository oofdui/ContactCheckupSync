namespace _ContactCheckupSync
{
    partial class SyncToMobile
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
            this.tbSyncToMobile = new System.Windows.Forms.TableLayoutPanel();
            this.lblSyncToMobile = new System.Windows.Forms.Label();
            this.tbSyncToMobileSearch = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.dtDOEFrom = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtDOETo = new System.Windows.Forms.DateTimePicker();
            this.ddlCompany = new System.Windows.Forms.ComboBox();
            this.btSearch = new System.Windows.Forms.Button();
            this.btSync = new System.Windows.Forms.Button();
            this.gvSyncToMobile = new System.Windows.Forms.DataGridView();
            this.pbSyncToMobile = new System.Windows.Forms.ProgressBar();
            this.backgroundWorkerSyncToMobile = new System.ComponentModel.BackgroundWorker();
            this.tbSyncToMobile.SuspendLayout();
            this.tbSyncToMobileSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvSyncToMobile)).BeginInit();
            this.SuspendLayout();
            // 
            // tbSyncToMobile
            // 
            this.tbSyncToMobile.AutoSize = true;
            this.tbSyncToMobile.ColumnCount = 1;
            this.tbSyncToMobile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbSyncToMobile.Controls.Add(this.lblSyncToMobile, 0, 1);
            this.tbSyncToMobile.Controls.Add(this.tbSyncToMobileSearch, 0, 0);
            this.tbSyncToMobile.Controls.Add(this.gvSyncToMobile, 0, 3);
            this.tbSyncToMobile.Controls.Add(this.pbSyncToMobile, 0, 2);
            this.tbSyncToMobile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSyncToMobile.Location = new System.Drawing.Point(0, 0);
            this.tbSyncToMobile.Margin = new System.Windows.Forms.Padding(0);
            this.tbSyncToMobile.Name = "tbSyncToMobile";
            this.tbSyncToMobile.RowCount = 4;
            this.tbSyncToMobile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbSyncToMobile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbSyncToMobile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbSyncToMobile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbSyncToMobile.Size = new System.Drawing.Size(746, 402);
            this.tbSyncToMobile.TabIndex = 1;
            // 
            // lblSyncToMobile
            // 
            this.lblSyncToMobile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSyncToMobile.AutoSize = true;
            this.lblSyncToMobile.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblSyncToMobile.Location = new System.Drawing.Point(3, 29);
            this.lblSyncToMobile.Name = "lblSyncToMobile";
            this.lblSyncToMobile.Padding = new System.Windows.Forms.Padding(5);
            this.lblSyncToMobile.Size = new System.Drawing.Size(740, 23);
            this.lblSyncToMobile.TabIndex = 2;
            this.lblSyncToMobile.Text = "- โปรดเลือกช่วงวัน และ บริษัทก่อน -";
            this.lblSyncToMobile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbSyncToMobileSearch
            // 
            this.tbSyncToMobileSearch.AutoSize = true;
            this.tbSyncToMobileSearch.ColumnCount = 7;
            this.tbSyncToMobileSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbSyncToMobileSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbSyncToMobileSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbSyncToMobileSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbSyncToMobileSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tbSyncToMobileSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbSyncToMobileSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tbSyncToMobileSearch.Controls.Add(this.label1, 0, 0);
            this.tbSyncToMobileSearch.Controls.Add(this.dtDOEFrom, 1, 0);
            this.tbSyncToMobileSearch.Controls.Add(this.label2, 2, 0);
            this.tbSyncToMobileSearch.Controls.Add(this.dtDOETo, 3, 0);
            this.tbSyncToMobileSearch.Controls.Add(this.ddlCompany, 4, 0);
            this.tbSyncToMobileSearch.Controls.Add(this.btSearch, 5, 0);
            this.tbSyncToMobileSearch.Controls.Add(this.btSync, 6, 0);
            this.tbSyncToMobileSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSyncToMobileSearch.Location = new System.Drawing.Point(0, 0);
            this.tbSyncToMobileSearch.Margin = new System.Windows.Forms.Padding(0);
            this.tbSyncToMobileSearch.Name = "tbSyncToMobileSearch";
            this.tbSyncToMobileSearch.RowCount = 1;
            this.tbSyncToMobileSearch.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tbSyncToMobileSearch.Size = new System.Drawing.Size(746, 29);
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
            this.ddlCompany.Size = new System.Drawing.Size(248, 21);
            this.ddlCompany.TabIndex = 2;
            // 
            // btSearch
            // 
            this.btSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btSearch.Location = new System.Drawing.Point(587, 3);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(75, 23);
            this.btSearch.TabIndex = 3;
            this.btSearch.Text = "Search";
            this.btSearch.UseVisualStyleBackColor = true;
            this.btSearch.Click += new System.EventHandler(this.btSearch_Click);
            // 
            // btSync
            // 
            this.btSync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btSync.Enabled = false;
            this.btSync.Location = new System.Drawing.Point(668, 3);
            this.btSync.Name = "btSync";
            this.btSync.Size = new System.Drawing.Size(75, 23);
            this.btSync.TabIndex = 4;
            this.btSync.Text = "Sync";
            this.btSync.UseVisualStyleBackColor = true;
            this.btSync.Click += new System.EventHandler(this.btSync_Click);
            // 
            // gvSyncToMobile
            // 
            this.gvSyncToMobile.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.gvSyncToMobile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gvSyncToMobile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvSyncToMobile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvSyncToMobile.GridColor = System.Drawing.Color.Silver;
            this.gvSyncToMobile.Location = new System.Drawing.Point(3, 81);
            this.gvSyncToMobile.MultiSelect = false;
            this.gvSyncToMobile.Name = "gvSyncToMobile";
            this.gvSyncToMobile.ShowEditingIcon = false;
            this.gvSyncToMobile.Size = new System.Drawing.Size(740, 318);
            this.gvSyncToMobile.TabIndex = 1;
            // 
            // pbSyncToMobile
            // 
            this.pbSyncToMobile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbSyncToMobile.Location = new System.Drawing.Point(3, 55);
            this.pbSyncToMobile.Name = "pbSyncToMobile";
            this.pbSyncToMobile.Size = new System.Drawing.Size(740, 20);
            this.pbSyncToMobile.TabIndex = 3;
            this.pbSyncToMobile.Visible = false;
            // 
            // backgroundWorkerSyncToMobile
            // 
            this.backgroundWorkerSyncToMobile.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerSyncToMobile_DoWork);
            // 
            // SyncToMobile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(746, 402);
            this.Controls.Add(this.tbSyncToMobile);
            this.Name = "SyncToMobile";
            this.Text = "SyncToMobile";
            this.Load += new System.EventHandler(this.SyncToMobile_Load);
            this.tbSyncToMobile.ResumeLayout(false);
            this.tbSyncToMobile.PerformLayout();
            this.tbSyncToMobileSearch.ResumeLayout(false);
            this.tbSyncToMobileSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvSyncToMobile)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tbSyncToMobile;
        private System.Windows.Forms.Label lblSyncToMobile;
        private System.Windows.Forms.TableLayoutPanel tbSyncToMobileSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtDOEFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtDOETo;
        private System.Windows.Forms.ComboBox ddlCompany;
        private System.Windows.Forms.Button btSearch;
        private System.Windows.Forms.Button btSync;
        private System.Windows.Forms.DataGridView gvSyncToMobile;
        private System.Windows.Forms.ProgressBar pbSyncToMobile;
        private System.ComponentModel.BackgroundWorker backgroundWorkerSyncToMobile;
    }
}