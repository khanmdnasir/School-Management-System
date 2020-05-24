using DbFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentManager.Utilities.List
{
    public class ListData
    {
        public static void LoadDataIntoGridView(DataGridView dgv,string storedProceName)
        {
            DbSqlServer db = new DbSqlServer(AppSettings.ConnectionString());
            dgv.DataSource = db.GetDataList(storedProceName);

            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        public static void LoadDataIntoComboBox(ComboBox cb,DbParameter parameter)
        {
            DbSqlServer db = new DbSqlServer(AppSettings.ConnectionString());
            cb.DataSource = db.GetDataList("usp_ListTypeDataGetDataByListTypeId", parameter);
            cb.DisplayMember = "Description";
            cb.ValueMember = "Id";
            cb.SelectedIndex = -1;
            cb.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        public static void LoadDataIntoComboBox(ComboBox cb,string storeProceName)
        {
            DbSqlServer db = new DbSqlServer(AppSettings.ConnectionString());
            cb.DataSource = db.GetDataList(storeProceName);
            cb.DisplayMember = "Description";
            cb.ValueMember = "Id";
            cb.SelectedIndex = -1;
            cb.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        public static void LoadDataIntoComboBox(ComboBox cb,string storeProceName, DbParameter parameter)
        {
            DbSqlServer db = new DbSqlServer(AppSettings.ConnectionString());
            cb.DataSource = db.GetDataList(storeProceName, parameter);
            cb.DisplayMember = "Description";
            cb.ValueMember = "Id";
            cb.SelectedIndex = -1;
            cb.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        public static void LoadDataIntoComboBox(ComboBox cb,String storeProceName, DbParameter[] parameters)
        {
            DbSqlServer db = new DbSqlServer(AppSettings.ConnectionString());
            cb.DataSource = db.GetDataList(storeProceName, parameters);
            cb.DisplayMember = "Description";
            cb.ValueMember = "Id";
            cb.SelectedIndex = -1;
            cb.DropDownStyle = ComboBoxStyle.DropDownList;

        }
    }
}
