using CoachTicketManagement.Utility;
using CrystalDecisions.Shared;
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
    public partial class fBaoCao : Form
    {
        public fBaoCao()
        {
            InitializeComponent();
            cboDriver.DisplayMember = "NAMEDRIVER";
            cboDriver.ValueMember = "IDDRIVER";
            cboDriver.DataSource = ADOHelper.Instance.ExecuteReader("select * from TBL_DRIVER");
        }

        private void btnXuat_Click(object sender, EventArgs e)
        {
            if (cboDriver.SelectedItem != null)
            {
                DateTime start = dateTimePickerStart.Value;
                DateTime end = dateTimePickerEnd.Value;
                DataTable data = ADOHelper.Instance.ExecuteReader("select * from view_BaoCao where IDDRIVER = @para_2 and DATEDIFF(D, CONVERT(datetime,@para_0,103),  DEPARTUREDAY) >= 0 and DATEDIFF(D, CONVERT(datetime,@para_1,103),  DEPARTUREDAY) <= 0", new object[] { start.ToString("dd/MM/yyyy"), end.ToString("dd/MM/yyyy"), cboDriver.SelectedValue });
                if(data.Rows.Count <= 0)
                {
                    MessageBox.Show("Tài xế "+ cboDriver.Text +" trong khoảng ngày từ " + start.ToString("dd/MM/yyyy") + " đến " + end.ToString("dd/MM/yyyy") + " không chạy chuyến nào !!!", "Thông báo", MessageBoxButtons.OK);
                    return;
                }    
                Rpt_BaoCaoThang rpBaoCao = new Rpt_BaoCaoThang();
                rpBaoCao.SetDatabaseLogon("sa", "123", ".", "CoachTicketManagementCNPM");
                rpBaoCao.SetDataSource(data); /*ADOHelper.Instance.ExecuteReader("select * from view_BaoCao")*/

                ParameterDiscreteValue pStart = new ParameterDiscreteValue();
                pStart.Value = dateTimePickerStart.Value;
                ParameterDiscreteValue pEnd = new ParameterDiscreteValue();
                pEnd.Value = dateTimePickerEnd.Value;

                ParameterValues pv = new ParameterValues();
                pv.Add(pStart);
                rpBaoCao.DataDefinition.ParameterFields["DayStart"].ApplyCurrentValues(pv);

                ParameterValues pE = new ParameterValues();
                pE.Add(pEnd);
                rpBaoCao.DataDefinition.ParameterFields["DayEnd"].ApplyCurrentValues(pE);

                crystalReportViewerBaoCao.ReportSource = rpBaoCao;
                crystalReportViewerBaoCao.SelectionFormula = @"{view_BaoCao.DEPARTUREDAY} in DateTime ("+start.Year+","+start.Month+","+start.Day+ ", 00, 00, 00) to DateTime (" + end.Year + "," + end.Month + "," + end.Day + ", 00, 00, 00) and {view_BaoCao.IDDRIVER} = " + cboDriver.SelectedValue.ToString();
                crystalReportViewerBaoCao.Refresh();
            }
        }

    }
}
