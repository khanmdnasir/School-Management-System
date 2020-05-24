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

namespace StudentManager.UI_Layer.Employee
{
    public partial class ManageEmployeeForm : TemplateForm
    {
        public ManageEmployeeForm()
        {
            InitializeComponent();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowEmployeeInfoScreen(0, false);
        }

        private void ShowEmployeeInfoScreen(int employeeId, bool isUpdate)
        {
            EmployeeInfoForm eif = new EmployeeInfoForm();
            eif.EmployeeId = employeeId;
            eif.IsUpdate = isUpdate;
            eif.ShowDialog();
            LoadDataIntoGridView();
        }

        private void ManageEmployeeForm_Load(object sender, EventArgs e)
        {
            LoadDataIntoGridView();
        }

        private void LoadDataIntoGridView()
        {
            ListData.LoadDataIntoGridView(EmployeesDataGridView, "usp_EmployeesGetEmployee");
        }

        private void EmployeesDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = EmployeesDataGridView.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            int employeeId = Convert.ToInt32(EmployeesDataGridView.Rows[rowIndex].Cells["EmployeeId"].Value);
            ShowEmployeeInfoScreen(employeeId, true);
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDocument1.Print();
        }

        private void EmployeesDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ButtonEnables();
        }

        private void ButtonEnables()
        {
            deleteToolStripMenuItem.Enabled = true;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(this.EmployeesDataGridView.Width, this.EmployeesDataGridView.Height);
            EmployeesDataGridView.DrawToBitmap(bm, new Rectangle(0, 0, this.EmployeesDataGridView.Width, this.EmployeesDataGridView.Height));
            e.Graphics.DrawImage(bm, 10, 10);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (EmployeesDataGridView.SelectedRows[0].Cells[0].Value.ToString() == "")
            {
                MessageBox.Show("Please Enter Valid ID number");
            }
            else
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-GII0V3R;Initial Catalog=StudentManager;Integrated Security=True"))
                {

                    string str = "DELETE FROM Employees WHERE EmployeeId = '" + EmployeesDataGridView.SelectedRows[0].Cells[0].Value + "'";
                    SqlCommand cmd = new SqlCommand(str, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    EmployeesDataGridView.DataSource = new BindingSource(dt, null);
                }

            }
            using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-GII0V3R;Initial Catalog=StudentManager;Integrated Security=True"))
            {

                string str = "SELECT * FROM Employees";
                SqlCommand cmd = new SqlCommand(str, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                EmployeesDataGridView.DataSource = new BindingSource(dt, null);
            }
            LoadDataIntoGridView();
        
    }
    }
}
