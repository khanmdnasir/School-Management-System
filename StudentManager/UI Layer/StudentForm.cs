using DbFramework;
using StudentManager.Model;
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

namespace StudentManager
{
    public partial class StudentForm : TemplateForm
    {
        public StudentForm()
        {
            InitializeComponent();
        }
        public int StudentId { get; set; }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Logo";
            ofd.Filter = "Logo File(*.png;*.jpg;*.bmp;*.gif)|*.png;*.jpg;*.bmp;*.gif";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ImagepictureBox.Image = new Bitmap(ofd.FileName);
            }
        }

        private void StudentForm_Load(object sender, EventArgs e)
        {
            LoadDataIntoComboBoxes();
            if (this.IsUpdate)
            {
                DbSqlServer db = new DbSqlServer(AppSettings.ConnectionString());
                DataTable dtBranch = db.GetDataList("usp_StudentsGetAllStudentByStudentId", new DbParameter() { Parameter = "@StudentId", Value = this.StudentId });
                DataRow row = dtBranch.Rows[0];
                StudentIdTextBox.Text = row["StudentId"].ToString();
                FullNameTextBox.Text = row["FullName"].ToString();
                FatherNameTextBox.Text = row["FatherName"].ToString();
                MotherNameTextBox.Text = row["MotherName"].ToString();
                DOBDateTimePicker.Text = row["DOB"].ToString();
                AddressTextBox.Text = row["Address"].ToString();
                EmailTextBox.Text = row["Email"].ToString();
                AdmissionDateDateTimePicker.Text = row["AdmissionDate"].ToString();
                MobileTextBox.Text = row["Mobile"].ToString();
                BloodGroupTextBox.Text = row["BloodGroup"].ToString();
                GenderComboBox.SelectedValue = row["GenderId"];
                BranchComboBox.SelectedValue = row["BranchId"];
                ImagepictureBox.Image = (row["Photo"] is DBNull) ? null : ImageManipulation.PutPhoto((byte[])row["Photo"]);
            }
            else
            {
                GenerateStudentId();
            }
        }

        private void LoadDataIntoComboBoxes()
        {
            ListData.LoadDataIntoComboBox(BranchComboBox, "usp_BranchesGetAllBranchName");
            ListData.LoadDataIntoComboBox(GenderComboBox, new DbParameter { Parameter = "@ListTypeId", Value = ListTypes.Gender });
        }

        private void GenerateStudentId()
        {
            DbSqlServer db = new DbSqlServer(AppSettings.ConnectionString());
            StudentIdTextBox.Text = db.GetScalerValue("usp_StudentsGenerateNewStudentId").ToString();
        }

        private void saveRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsFormValidate())
            {
                if (this.IsUpdate)
                {
                    SaveOrUpdateRecord("usp_StudentsUpdateStudent");
                    MessageBox.Show("Record is Update successfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    SaveOrUpdateRecord("usp_StudentsAddNewStudent");
                    MessageBox.Show("Record is added successfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.IsUpdate = true;

                }
                this.Close();
            }
        }

       

        private void SaveOrUpdateRecord(string storeProceName)
        {
            DbSqlServer db = new DbSqlServer(AppSettings.ConnectionString());
            db.SaveOrUpdatRecord(storeProceName, GetObject());

        }

        private object GetObject()
        {
            Students s = new Students();
            s.StudentId = (this.IsUpdate)?this.StudentId:(Convert.ToInt32(StudentIdTextBox.Text.Trim()));
            s.FullName = FullNameTextBox.Text.Trim();
            s.FatherName = FatherNameTextBox.Text.Trim();
            s.MotherName = MotherNameTextBox.Text.Trim();
            s.DOB = DOBDateTimePicker.Value.Date;
            s.Address = AddressTextBox.Text.Trim();
            s.Email = EmailTextBox.Text.Trim();
            s.AdmissionDate = AdmissionDateDateTimePicker.Value.Date;
            s.Mobile = MobileTextBox.Text.Trim();
            s.BloodGroup = BloodGroupTextBox.Text.Trim();
            s.GenderId = (GenderComboBox.SelectedIndex == -1) ? 0 : Convert.ToInt32(GenderComboBox.SelectedValue);
            s.BranchId = (BranchComboBox.SelectedIndex == -1) ? 0 : Convert.ToInt32(BranchComboBox.SelectedValue);
            s.Photo = (ImagepictureBox.Image == null) ? null : ImageManipulation.GetPhoto(ImagepictureBox);
            s.CreatedBy = LoggedInUsers.UserName;
            return s;
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
            if (FatherNameTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Father Name is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FatherNameTextBox.Clear();
                FatherNameTextBox.Focus();
                return false;
            }
            if (MotherNameTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Mother Name is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MotherNameTextBox.Clear();
                MotherNameTextBox.Focus();
                return false;
            }
            if (AddressTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Address is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AddressTextBox.Clear();
                AddressTextBox.Focus();
                return false;
            }
            if (EmailTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Email Name is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                EmailTextBox.Clear();
                EmailTextBox.Focus();
                return false;
            }
            if (MobileTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Mobile number is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MobileTextBox.Clear();
                MobileTextBox.Focus();
                return false;
            }
            if (BloodGroupTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Blood Group is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BloodGroupTextBox.Clear();
                BloodGroupTextBox.Focus();
                return false;
            }

            if (GenderComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Gender is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;

            }
            if (BranchComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Branch is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;

            }

            return true;
        }
    }
 }
