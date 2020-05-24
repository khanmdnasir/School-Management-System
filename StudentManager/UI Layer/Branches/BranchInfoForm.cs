using DbFramework;
using StudentManager.Model.Branches;
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

namespace StudentManager.UI_Layer.Branches
{
    public partial class BranchInfoForm : TemplateForm
    {
        public BranchInfoForm()
        {
            InitializeComponent();
        }

        public int BranchId { get; set; }

        private void BranchNametextBox_TextChanged(object sender, EventArgs e)
        {
            TopPanellabel.Text = BranchNametextBox.Text;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void BranchInfoForm_Load(object sender, EventArgs e)
        {

            LoadDataIntoComboBox();
            LoadDataAndBindToControlIfUpdate();
        }

        private void LoadDataIntoComboBox()
        {
            ListData.LoadDataIntoComboBox(CityNameComboBox, new DbParameter() { Parameter = "@ListTypeId", Value = ListTypes.City });
            ListData.LoadDataIntoComboBox(DistrictNameComboBox, new DbParameter() { Parameter = "@ListTypeId", Value=ListTypes.District });
        }

        private void LoadDataAndBindToControlIfUpdate()
        {
            if (this.IsUpdate)
            {
                DbSqlServer db = new DbSqlServer(AppSettings.ConnectionString());
                DataTable dtBranch = db.GetDataList("usp_BranchesGetBranchDetailsByBranchId", new DbParameter() { Parameter = "@BranchId", Value = this.BranchId });
                DataRow row = dtBranch.Rows[0];

                BranchNametextBox.Text = row["BranchName"].ToString();
                EmailAddressTextBox.Text = row["Email"].ToString();
                TelephoneTextBox.Text = row["Telephone"].ToString();
                WebsiteTextBox.Text = row["Website"].ToString();
                AddressLineTextBox.Text = row["AddressLine"].ToString();
                LogoPictureBox.Image = (row["BranchImage"] is DBNull)? null:ImageManipulation.PutPhoto((byte[])row["BranchImage"]);
                CityNameComboBox.SelectedValue = row["CityId"];
                DistrictNameComboBox.SelectedValue = row["DistrictId"];
                PostCodeTextBox.Text = row["PostCode"].ToString();

            }
        }

        private void savedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(isFormValidate())
            {
                if(this.IsUpdate)
                {
                    SaveOrUpdateRecord("usp_BranchesUpdateBranchDetails");
                    MessageBox.Show("Record is Update successfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    SaveOrUpdateRecord("usp_BranchesAddNewBranch");
                    MessageBox.Show("Record is added successfully", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Close();
               

            }
        }

        private void SaveOrUpdateRecord(string storeProceName)
        {
            DbSqlServer db = new DbSqlServer(AppSettings.ConnectionString());
            db.SaveOrUpdatRecord(storeProceName, GetObject());

        }

        private Branch GetObject()
        {
            Branch branch = new Branch();
            branch.BranchId = (this.IsUpdate) ? this.BranchId : 0;
            branch.BranchName = BranchNametextBox.Text;
            branch.Email = EmailAddressTextBox.Text;
            branch.Telephone = TelephoneTextBox.Text;
            branch.Website = WebsiteTextBox.Text;
            branch.AddressLine = AddressLineTextBox.Text;
            branch.CityId = Convert.ToInt32(CityNameComboBox.SelectedValue);
            branch.DistrictId = Convert.ToInt32(DistrictNameComboBox.SelectedValue);
            branch.PostCode = PostCodeTextBox.Text;
            branch.BranchImage =(LogoPictureBox.Image==null)? null:ImageManipulation.GetPhoto(LogoPictureBox);
            branch.CreatedBy =LoggedInUsers.UserName;

            return branch;
            
        }

       

        private bool isFormValidate()
        {
            if (BranchNametextBox.Text.Trim() == "")
            {
                MessageBox.Show("Branch Name is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BranchNametextBox.Clear();
                BranchNametextBox.Focus();
                return false;
            }
            if (EmailAddressTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Email is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                EmailAddressTextBox.Clear();
                EmailAddressTextBox.Focus();
                return false;
            }
            if (TelephoneTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Telephone is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TelephoneTextBox.Clear();
                TelephoneTextBox.Focus();
                return false;
            }
            if (AddressLineTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Address is required", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AddressLineTextBox.Clear();
                AddressLineTextBox.Focus();
                return false;
            }
           
            return true;
        }

        private void LogoPictureBox_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Logo";
            ofd.Filter = "Logo File(*.png;*.jpg;*.bmp;*.gif)|*.png;*.jpg;*.bmp;*.gif";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                LogoPictureBox.Image = new Bitmap(ofd.FileName);
            }
        }

       
    }
}
