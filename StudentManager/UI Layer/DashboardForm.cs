using StudentManager.UI_Layer.Branches;
using StudentManager.UI_Layer.Employee;
using StudentManager.UI_Layer.Templates;
using StudentManager.Utilities.List;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManager.UI_Layer
{
    public partial class DashboardForm : TemplateForm
    {
        public DashboardForm()
        {
            InitializeComponent();
        }
        
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void NewStudentToolStripButton_Click(object sender, EventArgs e)
        {
            ShowBranchInfoScreen(0, false);
        }

        private void ShowBranchInfoScreen(int studentId, bool isUpdate)
        {
            StudentForm sf = new StudentForm();
            sf.StudentId = studentId;
            sf.IsUpdate = isUpdate;
            sf.ShowDialog();
            LoadDataIntoGridView();

        }

        private void ManageBranchesToolStripButton_Click(object sender, EventArgs e)
        {
            ManageBranchForm mbf = new ManageBranchForm();
                mbf.Show();
           
        }

        private void EditProfileToolStripButton_Click(object sender, EventArgs e)
        {
            Profile p = new Profile();
            p.ShowDialog();
        }

        private void ManageUsersToolStripButton_Click(object sender, EventArgs e)
        {
            ManageEmployeeForm mef = new ManageEmployeeForm();
            mef.ShowDialog();
        }

        private void ManagetoolsToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void SystemSetupToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void HelpandSupportToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginForm lf = new LoginForm();
            lf.Show();
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            LoadDataIntoGridView();
           
        }

        private void LoadDataIntoGridView()
        {
            ListData.LoadDataIntoGridView(StudentsDataGridView, "usp_StudentsGettAllStudents");
        }

        private void HelpandSupportToolStripButton_Click_1(object sender, EventArgs e)
        {
            HelpAndSupport hs = new HelpAndSupport();
            hs.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SystemSettingsForm ssf = new SystemSettingsForm();
            ssf.ShowDialog();
        }

        private void StudentsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = StudentsDataGridView.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            int studentId = Convert.ToInt32(StudentsDataGridView.Rows[rowIndex].Cells["StudentId"].Value);
            ShowBranchInfoScreen(studentId, true);
        }

        private void SearchTextbox_Enter(object sender, EventArgs e)
        {
            if(SearchTextbox.Text=="SearchById")
            {
                SearchTextbox.Text = null;
                SearchTextbox.ForeColor = Color.Black;
            }
        }

        private void SearchTextbox_Leave(object sender, EventArgs e)
        {
            if (SearchTextbox.Text == null)
            {
                SearchTextbox.Text = "SearchById";
                SearchTextbox.ForeColor = Color.Silver;
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (SearchTextbox.Text == "")
            {
                MessageBox.Show("Please Enter Valid Id number");
            }


            else
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-GII0V3R;Initial Catalog=StudentManager;Integrated Security=True"))
                {

                    string str = "SELECT * FROM Students WHERE StudentId LIKE '" + SearchTextbox.Text + "'";
                    SqlCommand cmd = new SqlCommand(str, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    StudentsDataGridView.DataSource = new BindingSource(dt, null);
                }
                SearchTextbox.Text = "";
            }
        }

        private void Loadbutton_Click(object sender, EventArgs e)
        {
            LoadDataIntoGridView();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDocument1.Print();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(this.StudentsDataGridView.Width, this.StudentsDataGridView.Height);
            StudentsDataGridView.DrawToBitmap(bm, new Rectangle(0, 0, this.StudentsDataGridView.Width, this.StudentsDataGridView.Height));
            e.Graphics.DrawImage(bm, 10, 10);
        }

        private void StudentsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ButtonEnables();
        }

        private void ButtonEnables()
        {
            deleteToolStripMenuItem.Enabled = true;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (StudentsDataGridView.SelectedRows[0].Cells[0].Value.ToString() == "")
            {
                MessageBox.Show("Please Enter Valid ID number");
            }
            else
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-GII0V3R;Initial Catalog=StudentManager;Integrated Security=True"))
                {

                    string str = "DELETE FROM Students WHERE StudentId = '" + StudentsDataGridView.SelectedRows[0].Cells[0].Value + "'";
                    SqlCommand cmd = new SqlCommand(str, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    StudentsDataGridView.DataSource = new BindingSource(dt, null);
                }

            }
            using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-GII0V3R;Initial Catalog=StudentManager;Integrated Security=True"))
            {

                string str = "SELECT * FROM Students";
                SqlCommand cmd = new SqlCommand(str, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                StudentsDataGridView.DataSource = new BindingSource(dt, null);
            }
            LoadDataIntoGridView();
        
    }
    }
}
