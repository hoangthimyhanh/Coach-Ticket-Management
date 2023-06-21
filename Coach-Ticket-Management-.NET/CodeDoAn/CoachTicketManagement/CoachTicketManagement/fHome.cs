using CoachTicketManagement.Core;
using CoachTicketManagement.Models;
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
    public partial class fHome : Form
    {

        List<Employee> _employees;
        List<Account> _accounts;
        Employee _curEmployee;
        public fHome(Employee cur, List<Employee> employees, List<Account> accounts)
        {
            InitializeComponent();
            this._employees = employees;
            this._accounts = accounts;
            this._curEmployee = cur;
            fLoad();
        }

        void fLoad()
        {
            if (String.Compare(EmployeeService.Instance.CheckPermissionByID(_curEmployee.Id), "Admin", true) == 0)
            {
                ToolStripAdmin.Visible = true;
                ToolStripBill.Visible = true;
                ToolStripTicket.Visible = true;
                ToolStripBaoCao.Visible = true;
            }
            else if (String.Compare(EmployeeService.Instance.CheckPermissionByID(_curEmployee.Id), "Kế Toán", true) == 0)
            {
                ToolStripBill.Visible = true;
                ToolStripBaoCao.Visible = true;
            }
            else if (String.Compare(EmployeeService.Instance.CheckPermissionByID(_curEmployee.Id), "Bán Vé", true) == 0)
            {
                ToolStripBill.Visible = true;
                ToolStripTicket.Visible = true;
            }
            else
            {
                this.Close();
                MessageBox.Show("Tài khoản của bạn hiện đang bị khóa !!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void ToolStripAdmin_Click(object sender, EventArgs e)
        {
            fAdmin admin = new fAdmin(this._curEmployee);
            this.Hide();
            admin.ShowDialog();
            this.Show();
            fLoad();
        }

        private void ToolStripLogOut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ToolStripInfoAccount_Click(object sender, EventArgs e)
        {
            fAccountInfo accountInfo = new fAccountInfo(_curEmployee);
            accountInfo.ShowDialog();
        }

        private void ToolStripSwitchPassword_Click(object sender, EventArgs e)
        {
            Account account = this._accounts.SingleOrDefault(x => x.IdEmployee == this._curEmployee.Id);
            if (account == null)
            {
                MessageBox.Show("Không tìm thấy tài khoản hệ thống !!!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            fSwitchPassword f = new fSwitchPassword(account);
            f.ShowDialog();
        }

        private void ToolStripBaoCao_Click(object sender, EventArgs e)
        {
            fBaoCao f = new fBaoCao();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void ToolStripTicket_Click(object sender, EventArgs e)
        {
            fTicket f = new fTicket();
            this.Hide();
            f.ShowDialog();
            this.Show();
        }

        private void ToolStripBill_Click(object sender, EventArgs e)
        {
            fBill f = new fBill(_curEmployee);
            this.Hide();
            f.ShowDialog();
            this.Show();
        }
    }
}
