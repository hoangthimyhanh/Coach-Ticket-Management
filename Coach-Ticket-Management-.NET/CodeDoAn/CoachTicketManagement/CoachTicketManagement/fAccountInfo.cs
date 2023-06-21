using CoachTicketManagement.Core;
using CoachTicketManagement.Models;
using CoachTicketManagement.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CoachTicketManagement
{

    public partial class fAccountInfo : Form
    {
        private Employee _Employee;
        private int _IDAcount;
        List<Employee> employees_ = EmployeeService.Instance.GetEmployees();
        public fAccountInfo(Employee employee)
        {
            InitializeComponent();
            this._Employee = employee;
            this._IDAcount = employee.IdAccount;
        }


        private void Load_FAcount()
        {
            _Employee = employees_.FirstOrDefault(x => x.IdAccount == _IDAcount);
            TxtIdAccount.Text = _Employee.IdAccount.ToString();
            TxtIdEmployee.Text = _Employee.Id.ToString();
            TxtName.Text = _Employee.Name;
            ControlHelper.Instance.loadCity(CboCity);
            ControlHelper.Instance.loadDistrict(CboDistrict);
            ControlHelper.Instance.loadWard(CboWard);
            CboWard.SelectedValue = _Employee.IdWard;
            int idDistrict = ADOHelper.Instance.ExecuteScalar("select IDDistrict from tbl_Ward where IDWard = @para_0", new object[] { _Employee.IdWard });
            CboDistrict.SelectedValue = idDistrict;
            int idCity = ADOHelper.Instance.ExecuteScalar("select IDCity from tbl_District where IDDistrict = @para_0", new object[] { idDistrict });
            CboCity.SelectedValue = idCity;
            ControlHelper.Instance.loadTypeEmployee(CboTypeOfEmployee);
            DtpDateOfBirth.Value = _Employee.DateOfBirth;

            ControlHelper.Instance.loadGender(CboGender);
            CboGender.Text = _Employee.Gender;

            TxtIdentityCard.Text = _Employee.IdentityCard;
            TxtPhone.Text = _Employee.Phone;
            TxtEmail.Text = _Employee.Email;
        }
        private void fAccountInfo_Load(object sender, EventArgs e)
        {
            Load_FAcount();
            panelMain.Enabled = false;
            btnSave.Enabled = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            panelMain.Enabled = btnSave.Enabled = true;
            CboTypeOfEmployee.Enabled = false;
            TxtName.Focus();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtName.Text))
            {
                MessageBox.Show("Không được bỏ trống tên !!!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(TxtIdentityCard.Text))
            {
                MessageBox.Show("Không được bỏ trống CMND!!!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(TxtPhone.Text))
            {
                MessageBox.Show("Không được bỏ trống SĐT !!!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(TxtEmail.Text))
            {
                MessageBox.Show("Không được bỏ trống MAIL !!!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (DateTime.Now.Year - DtpDateOfBirth.Value.Year < 15)
            {
                MessageBox.Show("Tuổi không đủ !!!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (CboCity.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Tỉnh/Thành Phố !!!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (CboDistrict.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn quận huyện !!!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (CboWard.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn phường/xã !!!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }



            if (_Employee == null)
            {
                MessageBox.Show("Chưa load được dữ liệu Tài Khoản !!!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            else
            {
                if (TxtName.Text != _Employee.Name || (int)CboWard.SelectedValue != _Employee.IdWard || CboGender.SelectedItem.ToString() != _Employee.Gender || DtpDateOfBirth.Value != _Employee.DateOfBirth)
                {
                    DialogResult r = MessageBox.Show("Thông tin có sự thay đổi. Bạn có muốn cập nhật lại dữ liệu?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                    if (r == DialogResult.Yes)
                    {
                        try
                        {
                            using (SqlConnection connection = new SqlConnection(ConnectionString.Instance.getConnectionString()))
                            {
                                string query = string.Format(@"SET DATEFORMAT DMY exec sp_UpdateEMPLOYEE {0},{1},{2},{3},N'{4}','{5}',N'{6}','{7}','{8}','{9}'",
                                                  _Employee.Id, (int)CboWard.SelectedValue, _Employee.IdTypeOfEmployee, _Employee.IdPermissionGroup,
                                                   TxtName.Text, DtpDateOfBirth.Value, CboGender.Text,
                                                    TxtIdentityCard.Text, TxtPhone.Text, TxtEmail.Text);
                                connection.Open();
                                SqlCommand cmd = new SqlCommand(query, connection);
                                if (query != null)
                                {
                                    cmd.ExecuteNonQuery();
                                    MessageBox.Show("Cập Nhật Thành Công!!!", "Hoàn Thành", MessageBoxButtons.OK);
                                    //Load_FAcount();
                                }

                                connection.Close();
                                this.Hide();
                                fAccountInfo accountInfo = new fAccountInfo(_Employee);
                                accountInfo.ShowDialog();
                            }

                        }
                        catch
                        {
                            MessageBox.Show("Thông tin bị trùng !!!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                }
            }
        }

        private void CboCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox control = (ComboBox)sender;
            if (control.SelectedValue != null)
            {
                ControlHelper.Instance.loadDistrict(CboDistrict, (int)control.SelectedValue);
            }
        }

        private void CboDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox control = (ComboBox)sender;
            if (control.SelectedValue != null)
            {
                ControlHelper.Instance.loadWard(CboWard, (int)control.SelectedValue);
            }
        }

        private void TxtIdentityCard_KeyPress(object sender, KeyPressEventArgs e)
        {
            Control ctr = (Control)sender;
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void TxtIdentityCard_Leave(object sender, EventArgs e)
        {
            if (!Utilities.Instance.CheckIdentityCard(TxtIdentityCard.Text))
            {
                MessageBox.Show("CMND/CCCD không đúng định dạng!!!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                TxtIdentityCard.Focus();
                return;
            }
            string IDCard = TxtIdentityCard.Text;
            List<Employee> employees = EmployeeService.Instance.GetEmployees();
            Employee employee = employees.SingleOrDefault(x => x.IdentityCard == IDCard);

            if (employee != null)
            {
                if (employee.Id != Convert.ToInt32(TxtIdEmployee.Text))
                {
                    MessageBox.Show("CMND/CCCD bị trùng!!!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    TxtIdentityCard.Focus();
                    return;
                }
            }

        }

        private void TxtPhone_Leave(object sender, EventArgs e)
        {
            if (!Utilities.Instance.CheckPhone(TxtPhone.Text))
            {
                MessageBox.Show("SDT không đúng định dạng!!!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                TxtPhone.Focus();
                return;
            }
            string phone = TxtPhone.Text;
            List<Employee> employees = EmployeeService.Instance.GetEmployees();
            Employee employee = employees.SingleOrDefault(x => x.Phone == phone);

            if (employee != null)
            {
                if (employee.Id != Convert.ToInt32(TxtIdEmployee.Text))
                {
                    MessageBox.Show("SĐT bị trùng!!!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    TxtPhone.Focus();
                    return;
                }
            }
        }

        private void TxtEmail_Leave(object sender, EventArgs e)
        {
            if (!Utilities.Instance.CheckGmail(TxtEmail.Text))
            {
                MessageBox.Show("Mail không đúng định dạng!!!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                TxtEmail.Focus();
                return;
            }
            string mail = TxtEmail.Text;
            List<Employee> employees = EmployeeService.Instance.GetEmployees();
            Employee employee = employees.SingleOrDefault(x => x.Email == mail);

            if (employee != null)
            {
                if (employee.Id != Convert.ToInt32(TxtIdEmployee.Text))
                {
                    MessageBox.Show("Mail bị trùng!!!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    TxtEmail.Focus();
                    return;
                }
            }
        }


    }
}
