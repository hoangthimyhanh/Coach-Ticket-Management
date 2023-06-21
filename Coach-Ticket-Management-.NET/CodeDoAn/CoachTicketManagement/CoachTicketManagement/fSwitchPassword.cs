using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using CoachTicketManagement.Models;
using CoachTicketManagement.Utility;

namespace CoachTicketManagement
{
    public partial class fSwitchPassword : Form
    {
        Account _Cur;
        public fSwitchPassword(Account account)
        {
            InitializeComponent();
            this._Cur = account;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if(txtNewPwd.Text != txtConfirmPwd.Text)
            {
                MessageBox.Show("Confirm không trùng khớp !!!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString.Instance.getConnectionString());
                SqlCommand cmd = new SqlCommand("sp_Change_Password", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", this._Cur.UserName);
                cmd.Parameters.AddWithValue("@old_pwd", txtOldPwd.Text.Trim());
                cmd.Parameters.AddWithValue("@new_pwd", txtNewPwd.Text.Trim());
                cmd.Parameters.Add("@Status", SqlDbType.Int);
                cmd.Parameters["@Status"].Direction = ParameterDirection.Output;
                con.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                int retVal = Convert.ToInt32(cmd.Parameters["@Status"].Value);
                if (retVal == 1)
                {
                    MessageBox.Show("Mật khẩu đã được thay đổi thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Tên người dùng hoặc mật khẩu cũ sai. Vui lòng nhập lại!!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                txtOldPwd.ResetText();
                txtNewPwd.ResetText();
                txtConfirmPwd.ResetText();
            }
            catch
            {
                MessageBox.Show("Hệ thống đã sảy ra sự cố vui lòng thực hiện lại thao tác!!!","Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                txtOldPwd.ResetText();
                txtNewPwd.ResetText();
                txtConfirmPwd.ResetText();
            }
        }

        private void fSwitchPassword_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult r = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.No)
                e.Cancel = true;
        }

        private void txtOldPwd_TextChanged(object sender, EventArgs e)
        {
            txtOldPwd.PasswordChar = '*';
        }

        private void txtNewPwd_TextChanged(object sender, EventArgs e)
        {
            txtNewPwd.PasswordChar = '*';
        }

        private void txtConfirmPwd_TextChanged(object sender, EventArgs e)
        {
            txtConfirmPwd.PasswordChar = '*';
        }
    }
}
