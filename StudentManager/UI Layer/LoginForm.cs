using DbFramework;
using StudentManager.Model.Users;
using StudentManager.UI_Layer.Templates;
using StudentManager.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManager.UI_Layer
{
    public partial class LoginForm : TemplateForm
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SignInButton_Click(object sender, EventArgs e)
        {
            if (IsFormValidate())
            {
                DbSqlServer db = new DbSqlServer(AppSettings.ConnectionString());
                bool isLoginDetailsCorrect =Convert.ToBoolean(db.GetScalerValue("usp_UsersCheckLoginDetails",GetParameters()));
                if (isLoginDetailsCorrect)
                {
                    GetLoggedInUserSetting();
                    this.Hide();
                    DashboardForm dashboardForm = new DashboardForm();
                    dashboardForm.Show();
                }
                else
                {
                    MessageBox.Show("User Name/Password is not correct", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void GetLoggedInUserSetting()
        {
            LoggedInUsers.UserName = UserNametextBox.Text.Trim();
        }

        private DbParameter[] GetParameters()
        {
            List<DbParameter> parameters =new List<DbParameter>();

            DbParameter dbPara1 = new DbParameter();
            dbPara1.Parameter = "@UserName";
            dbPara1.Value = UserNametextBox.Text;
            parameters.Add(dbPara1);

            DbParameter dbPara2 = new DbParameter();
            dbPara2.Parameter = "@Password";
            dbPara2.Value = PasswordtextBox.Text;
            parameters.Add(dbPara2);

            return parameters.ToArray();

        }

        private bool IsFormValidate()
        {
            if(UserNametextBox.Text.Trim()=="")
            {
                MessageBox.Show("User Name is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UserNametextBox.Clear();
                UserNametextBox.Focus();
                return false;
            }
            if (PasswordtextBox.Text.Trim() == "")
            {
                MessageBox.Show("Password is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PasswordtextBox.Clear();
                PasswordtextBox.Focus();
                return false;
            }
            return true;
        }
    }
}
