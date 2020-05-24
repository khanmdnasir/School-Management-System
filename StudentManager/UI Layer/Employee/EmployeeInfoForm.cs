using DbFramework;
using StudentManager.Model.Employees;
using StudentManager.Model.Users;
using StudentManager.UI_Layer.Templates;
using StudentManager.Utilities;
using StudentManager.Utilities.List;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManager.UI_Layer.Employee
{
    public partial class EmployeeInfoForm : TemplateForm
    {
        public EmployeeInfoForm()
        {
            InitializeComponent();
        }

        public int EmployeeId { get; set; }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EmployeeInfoForm_Load(object sender, EventArgs e)
        {
            LoadDataIntoComboBoxes();
            if(this.IsUpdate)
            {
                 DbSqlServer db = new DbSqlServer(AppSettings.ConnectionString());
                DataTable dtBranch = db.GetDataList("usp_EmployeesGetAllEmployeesByEmployeeId", new DbParameter() { Parameter = "@EmployeeId", Value = this.EmployeeId });
                DataRow row = dtBranch.Rows[0];

                EmployeeIdTextBox.Text = row["EmployeeId"].ToString();
                FullNameTextBox.Text = row["FullName"].ToString();
                DOBdateTimePicker.Text = row["DateOfBirth"].ToString();
                NICTextBox.Text = row["NICNumber"].ToString();
                EmailAddressTextBox.Text = row["EmailAddress"].ToString();
                MobileTextBox.Text = row["Mobile"].ToString();  
                TelephoneTextBox.Text = row["Telephone"].ToString();
                GenderComboBox.SelectedValue = row["GenderId"];
                EmploymentDatedateTimePicker.Text = row["EmploymentDate"].ToString();
                
                
                
                BranchComboBox.SelectedValue = row["BranchId"];
                EmployeePictureBox.Image = (row["Photo"] is DBNull) ? null : ImageManipulation.PutPhoto((byte[])row["Photo"]);
                AddressLineTextBox.Text = row["AddressLine"].ToString();
                CityComboBox.SelectedValue = row["CityId"];
                DistrictComboBox.SelectedValue = row["DistrictId"];
                PostCodeTextBox.Text = row["PostCode"].ToString();
                JobTitleComboBox.SelectedValue = row["JObTitleId"];
                CurrentSalaryTextBox1.Text = row["CurrentSalary"].ToString();
                StartingSalaryTextBox.Text = row["StartingSalary"].ToString();
                HasLeftComboBox.SelectedValue = row["HasLeft"];
                DateLeftDateTimePicker.Text = row["DateLeft"].ToString();
                ReasonLeftComboBox.SelectedValue = row["ReasonLeftId"];
                
                
                
                CommentsTextBox.Text = row["Comments"].ToString();
               



            }
            else
            {
                GenerateEmployeeId();
            }
        }

        private void LoadDataIntoComboBoxes()
        {
            ListData.LoadDataIntoComboBox(BranchComboBox, "usp_BranchesGetAllBranchName");
            ListData.LoadDataIntoComboBox(GenderComboBox, new DbParameter { Parameter = "@ListTypeId", Value = ListTypes.Gender });
            ListData.LoadDataIntoComboBox(CityComboBox, new DbParameter { Parameter = "@ListTypeId", Value = ListTypes.City });
            ListData.LoadDataIntoComboBox(DistrictComboBox, new DbParameter { Parameter = "@ListTypeId", Value = ListTypes.District });
            ListData.LoadDataIntoComboBox(JobTitleComboBox, new DbParameter { Parameter = "@ListTypeId", Value = ListTypes.EmployeeJobTitle });
            ListData.LoadDataIntoComboBox(HasLeftComboBox, new DbParameter { Parameter = "@ListTypeId", Value = ListTypes.YesOrNo });
            ListData.LoadDataIntoComboBox(ReasonLeftComboBox, new DbParameter { Parameter = "@ListTypeId", Value = ListTypes.EmployeeReasonLeft });
        }

        private void GenerateEmployeeId()
        {
            DbSqlServer db = new DbSqlServer(AppSettings.ConnectionString());
            EmployeeIdTextBox.Text = db.GetScalerValue("usp_EmployeesGenerateNewEmployeeId").ToString();

        }

        private void EmployeePictureBox_DoubleClick(object sender, EventArgs e)
        {
            GetPhoto();
        }

        private void GetPhoto()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Photo";
            ofd.Filter = "Photo File(*.png;*.jpg;*.bmp;*.gif)|*.png;*.jpg;*.bmp;*.gif";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                EmployeePictureBox.Image = new Bitmap(ofd.FileName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetPhoto();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            EmployeePictureBox.Image = null;
        }

        private void saveRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(IsFormValidate())
            {
                if (this.IsUpdate)
                {
                    SaveOrUpdateRecord("usp_EmployeesUpdateEmployeeDetails");
                    MessageBox.Show("Record is Update successfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    SaveOrUpdateRecord("usp_EmployeesAddNewEmployee");
                    MessageBox.Show("Record is added successfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.IsUpdate = true;
                    ButtonEnables();
                }
                this.Close();


            }

         
        }

        private void ButtonEnables()
        {
            addToolStripMenuItem.Enabled = true;
            sendToolStripMenuItem.Enabled = true;
            printToolStripMenuItem.Enabled = true;
        }

        private void SaveOrUpdateRecord(string storeProceName)
        {
            DbSqlServer db = new DbSqlServer(AppSettings.ConnectionString());
            db.SaveOrUpdatRecord(storeProceName, GetObject());
        }

        private Employees GetObject()
        {
            Employees employee = new Employees();
            employee.EmployeeId = (this.IsUpdate)?this.EmployeeId:(Convert.ToInt32(EmployeeIdTextBox.Text.Trim()));
            employee.FullName = FullNameTextBox.Text.Trim();
            employee.DateOfBirth = DOBdateTimePicker.Value.Date;
            employee.NICNumber = NICTextBox.Text.Trim();
            employee.EmailAddress = EmailAddressTextBox.Text.Trim();
            employee.Mobile = MobileTextBox.Text.Trim();
            employee.Telephone = TelephoneTextBox.Text.Trim();
            employee.GenderId = (GenderComboBox.SelectedIndex == -1) ? 0 : Convert.ToInt32(GenderComboBox.SelectedValue);
            employee.BranchId = (BranchComboBox.SelectedIndex == -1) ? 0 : Convert.ToInt32(BranchComboBox.SelectedValue);
            employee.EmploymentDate = EmploymentDatedateTimePicker.Value.Date;
            employee.Photo = (EmployeePictureBox.Image == null) ? null : ImageManipulation.GetPhoto(EmployeePictureBox); 
            employee.AddressLine = AddressLineTextBox.Text.Trim();
            employee.CityId = (CityComboBox.SelectedIndex == -1) ? 0 : Convert.ToInt32(CityComboBox.SelectedValue);
            employee.DistrictId = (DistrictComboBox.SelectedIndex == -1) ? 0 : Convert.ToInt32(DistrictComboBox.SelectedValue);
            employee.PostCode = PostCodeTextBox.Text.Trim();
            employee.JObTitleId = (JobTitleComboBox.SelectedIndex == -1) ? 0 : Convert.ToInt32(JobTitleComboBox.SelectedValue);
            employee.CurrentSalary = Convert.ToDecimal(CurrentSalaryTextBox1.Text);
            employee.StartingSalary = Convert.ToDecimal(StartingSalaryTextBox.Text);
            employee.HasLeft = (HasLeftComboBox.Text == "Yes") ? true : false;
            employee.DateLeft = DateLeftDateTimePicker.Value.Date;
            employee.ReasonLeftId = (ReasonLeftComboBox.SelectedIndex == -1) ? 0 : Convert.ToInt32(ReasonLeftComboBox.SelectedValue);
            employee.Comments = CommentsTextBox.Text.Trim();
            employee.CreatedBy = LoggedInUsers.UserName;


            return employee;
        }

        private bool IsFormValidate()
        {
            if (FullNameTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Full Name is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FullNameTextBox.Clear();
                FullNameTextBox.Focus();
                return false;
            }
            if (EmailAddressTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Email is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                EmailAddressTextBox.Clear();
                EmailAddressTextBox.Focus();
                return false;
            }
            if ((TelephoneTextBox.Text.Trim() == "")&&(MobileTextBox.Text.Trim()==""))
            {
                MessageBox.Show("Mobile or Telephone Number is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TelephoneTextBox.Clear();
                TelephoneTextBox.Focus();
                return false;
            }
            if (NICTextBox.Text.Trim() == "")
            {
                MessageBox.Show("NIC is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                NICTextBox.Clear();
                NICTextBox.Focus();
                return false;
            }
            if(GenderComboBox.SelectedIndex==-1)
            {
                MessageBox.Show("Gender is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
                
            }
            
            if (AddressLineTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Address is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AddressLineTextBox.Clear();
                AddressLineTextBox.Focus();
                return false;
            }
            if (CityComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("City is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;

            }
            if (DistrictComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("District is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;

            }
            if (PostCodeTextBox.Text.Trim() == "")
            {
                MessageBox.Show("PostCode is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PostCodeTextBox.Clear();
                PostCodeTextBox.Focus();
                return false;
            }
            if (JobTitleComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Job Title is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;

            }
            if (CurrentSalaryTextBox1.Text.Trim() == "")
            {
                MessageBox.Show("Current Salary is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CurrentSalaryTextBox1.Clear();
                CurrentSalaryTextBox1.Focus();
                return false;
            }
            else
            {
                if(Convert.ToDecimal(CurrentSalaryTextBox1.Text.Trim())<1)
                {
                    MessageBox.Show("Current Salary can not be Zero or less than zero");
                    CurrentSalaryTextBox1.Focus();
                    return false;
                }
            }
            if (StartingSalaryTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Starting Salary is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                StartingSalaryTextBox.Clear();
                StartingSalaryTextBox.Focus();
                return false;
            }
            else
            {
                if (Convert.ToDecimal(StartingSalaryTextBox.Text.Trim()) < 1)
                {
                    MessageBox.Show("Starting Salary can not be Zero or less than zero");
                    StartingSalaryTextBox.Focus();
                    return false;
                }
            }

            return true;
        }
    }
}
