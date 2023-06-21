using CoachTicketManagement.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoachTicketManagement
{
    public partial class fTicket : Form
    {
        int idBill;
        public fTicket()
        {
            InitializeComponent();
            cboIDBill.DisplayMember = "IDBILL";
            cboIDBill.ValueMember = "IDBILL";
            cboIDBill.DataSource = ADOHelper.Instance.ExecuteReader("select IDBILL from TBL_BILL");
        }
        public fTicket(int idBill)
        {
            InitializeComponent();
            cboIDBill.DisplayMember = "IDBILL";
            cboIDBill.ValueMember = "IDBILL";
            cboIDBill.DataSource = ADOHelper.Instance.ExecuteReader("select IDBILL from TBL_BILL");
            
            this.idBill = idBill;
            cboIDBill.SelectedValue = this.idBill;
        }

        private void btnFindTicketBill_Click(object sender, EventArgs e)
        {
            if(cboIDBill.SelectedItem != null)
            {
                rp_Ve rpVe = new rp_Ve();
                rpVe.SetDatabaseLogon("sa", "123", ".", "CoachTicketManagementCNPM");
                rpVe.SetDataSource(ADOHelper.Instance.ExecuteReader("select * from view_BILL"));
                reportTicket.ReportSource = rpVe;
                reportTicket.SelectionFormula = "{view_BILL.IDBILL} = " + cboIDBill.SelectedValue.ToString();
                //crystalReportViewerBai1.DisplayStatusBar = false;
                //crystalReportViewerBai1.DisplayToolbar = true;
                reportTicket.Refresh();
            }
        }
    }
}
