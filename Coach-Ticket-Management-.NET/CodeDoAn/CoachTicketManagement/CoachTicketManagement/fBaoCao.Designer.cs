
namespace CoachTicketManagement
{
    partial class fBaoCao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fBaoCao));
            this.btnXuat = new System.Windows.Forms.Button();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.crystalReportViewerBaoCao = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.cboDriver = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnXuat
            // 
            this.btnXuat.AutoSize = true;
            this.btnXuat.Location = new System.Drawing.Point(1097, 11);
            this.btnXuat.Name = "btnXuat";
            this.btnXuat.Size = new System.Drawing.Size(99, 34);
            this.btnXuat.TabIndex = 0;
            this.btnXuat.Text = "Xuất Report";
            this.btnXuat.UseVisualStyleBackColor = true;
            this.btnXuat.Click += new System.EventHandler(this.btnXuat_Click);
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.CustomFormat = "dd/MM/yyyy";
            this.dateTimePickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerStart.Location = new System.Drawing.Point(218, 11);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.Size = new System.Drawing.Size(200, 27);
            this.dateTimePickerStart.TabIndex = 1;
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.CustomFormat = "dd/MM/yyyy";
            this.dateTimePickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEnd.Location = new System.Drawing.Point(517, 11);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(200, 27);
            this.dateTimePickerEnd.TabIndex = 2;
            // 
            // crystalReportViewerBaoCao
            // 
            this.crystalReportViewerBaoCao.ActiveViewIndex = -1;
            this.crystalReportViewerBaoCao.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewerBaoCao.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewerBaoCao.Location = new System.Drawing.Point(12, 52);
            this.crystalReportViewerBaoCao.Name = "crystalReportViewerBaoCao";
            this.crystalReportViewerBaoCao.Size = new System.Drawing.Size(1183, 517);
            this.crystalReportViewerBaoCao.TabIndex = 3;
            this.crystalReportViewerBaoCao.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // cboDriver
            // 
            this.cboDriver.FormattingEnabled = true;
            this.cboDriver.Location = new System.Drawing.Point(825, 14);
            this.cboDriver.Name = "cboDriver";
            this.cboDriver.Size = new System.Drawing.Size(213, 28);
            this.cboDriver.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(187, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Từ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(476, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Đến";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(771, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Tài xế";
            // 
            // fBaoCao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1207, 584);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboDriver);
            this.Controls.Add(this.crystalReportViewerBaoCao);
            this.Controls.Add(this.dateTimePickerEnd);
            this.Controls.Add(this.dateTimePickerStart);
            this.Controls.Add(this.btnXuat);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "fBaoCao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Báo Cáo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnXuat;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewerBaoCao;
        private System.Windows.Forms.ComboBox cboDriver;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}