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

namespace StudentManager.UI_Layer.Branches
{
    public partial class ManageBranchForm : TemplateForm
    {
        public ManageBranchForm()
        {
            InitializeComponent();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void addNewBranchToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            ShowBranchInfoScreen(0, false);
        }

        private void ShowBranchInfoScreen(int branchId, bool isUpdate)
        {
            BranchInfoForm bif = new BranchInfoForm();
            bif.BranchId = branchId;
            bif.IsUpdate = isUpdate;
            bif.ShowDialog();

            LoadDataIntoGridView();

        }

        private void ManageBranchForm_Load(object sender, EventArgs e)
        {
            LoadDataIntoGridView();
        }

        private void LoadDataIntoGridView()
        {
            ListData.LoadDataIntoGridView(BranchesDataGridView, "usp_BranchesGetAllBranches");
        }

        private void BranchesDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = BranchesDataGridView.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            int branchId = Convert.ToInt32(BranchesDataGridView.Rows[rowIndex].Cells["BranchId"].Value);
            ShowBranchInfoScreen(branchId, true);
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDocument1.Print();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bm = new Bitmap(this.BranchesDataGridView.Width, this.BranchesDataGridView.Height);
            BranchesDataGridView.DrawToBitmap(bm, new Rectangle(0, 0, this.BranchesDataGridView.Width, this.BranchesDataGridView.Height));
            e.Graphics.DrawImage(bm, 10, 10);
        }

        private void BranchesDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ButtonEnables();
        }

        private void ButtonEnables()
        {
            deleteToolStripMenuItem.Enabled = true;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (BranchesDataGridView.SelectedRows[0].Cells[0].Value.ToString() == "")
            {
                MessageBox.Show("Please Enter Valid ID number");
            }
            else
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-GII0V3R;Initial Catalog=StudentManager;Integrated Security=True"))
                {

                    string str = "DELETE FROM Branches WHERE BranchId = '" + BranchesDataGridView.SelectedRows[0].Cells[0].Value + "'";
                    SqlCommand cmd = new SqlCommand(str, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    BranchesDataGridView.DataSource = new BindingSource(dt, null);
                }

            }
            using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-GII0V3R;Initial Catalog=StudentManager;Integrated Security=True"))
            {

                string str = "SELECT * FROM Branches";
                SqlCommand cmd = new SqlCommand(str, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                BranchesDataGridView.DataSource = new BindingSource(dt, null);
            }
            LoadDataIntoGridView();
        }
    }
}
