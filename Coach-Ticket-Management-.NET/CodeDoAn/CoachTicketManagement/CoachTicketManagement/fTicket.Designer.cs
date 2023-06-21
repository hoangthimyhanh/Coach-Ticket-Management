
namespace CoachTicketManagement
{
    partial class fTicket
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
            this.reportTicket = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.cboIDBill = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFindTicketBill = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // reportTicket
            // 
            this.reportTicket.ActiveViewIndex = -1;
            this.reportTicket.Cursor = System.Windows.Forms.Cursors.Default;
            this.reportTicket.Location = new System.Drawing.Point(2, 42);
            this.reportTicket.Name = "reportTicket";
            this.reportTicket.Size = new System.Drawing.Size(887, 602);
            this.reportTicket.TabIndex = 0;
            this.reportTicket.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // cboIDBill
            // 
            this.cboIDBill.FormattingEnabled = true;
            this.cboIDBill.Location = new System.Drawing.Point(102, 10);
            this.cboIDBill.Name = "cboIDBill";
            this.cboIDBill.Size = new System.Drawing.Size(143, 24);
            this.cboIDBill.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Mã hóa đơn";
            // 
            // btnFindTicketBill
            // 
            this.btnFindTicketBill.Location = new System.Drawing.Point(261, 10);
            this.btnFindTicketBill.Name = "btnFindTicketBill";
            this.btnFindTicketBill.Size = new System.Drawing.Size(75, 23);
            this.btnFindTicketBill.TabIndex = 3;
            this.btnFindTicketBill.Text = "Tìm vé";
            this.btnFindTicketBill.UseVisualStyleBackColor = true;
            this.btnFindTicketBill.Click += new System.EventHandler(this.btnFindTicketBill_Click);
            // 
            // fTicket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 656);
            this.Controls.Add(this.btnFindTicketBill);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboIDBill);
            this.Controls.Add(this.reportTicket);
            this.Name = "fTicket";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "In vé";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer reportTicket;
        private System.Windows.Forms.ComboBox cboIDBill;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnFindTicketBill;
    }
}