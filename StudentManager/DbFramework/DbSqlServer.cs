using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DbFramework
{
    public class DbSqlServer
    {
        private string _connString;
        public DbSqlServer(string connString)
        {
            _connString = connString;
        }

        public DataTable GetDataList(string storedProceName)
        {
            DataTable dtData = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                using (SqlCommand cmd = new SqlCommand(storedProceName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    dtData.Load(reader);

                }
            }
            return dtData;

        }
        public DataTable GetDataList(string storedProceName,DbParameter parameter)
        {
            DataTable dtData = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                using (SqlCommand cmd = new SqlCommand(storedProceName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    cmd.Parameters.AddWithValue(parameter.Parameter, parameter.Value);
                    SqlDataReader reader = cmd.ExecuteReader();
                    dtData.Load(reader);

                }
            }
            return dtData;

        }
        public DataTable GetDataList(string storedProceName,DbParameter[] parameters)
        {
            DataTable dtData = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                using (SqlCommand cmd = new SqlCommand(storedProceName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    foreach (var para in parameters)
                    {
                        cmd.Parameters.AddWithValue(para.Parameter, para.Value);
                    }

                        SqlDataReader reader = cmd.ExecuteReader();
                        dtData.Load(reader);
                    
                    

                }
            }
            return dtData;

        }

        public void SaveOrUpdatRecord(string storeProceName,object obj)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                using (SqlCommand cmd = new SqlCommand(storeProceName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    Type type = obj.GetType();
                    BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
                    PropertyInfo[] properties = type.GetProperties(flags);

                    foreach(var property in properties)
                    {
                        cmd.Parameters.AddWithValue("@" + property.Name, property.GetValue(obj, null));

                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public object GetScalerValue(string storedProcedureName)
        {
            object value = null;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedureName,conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conn.Open();
                    value = cmd.ExecuteScalar();
                }
            }
            return value;
        }

        public object GetScalerValue(string storedProcedureName,DbParameter parameter)
        {
            object value = null;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conn.Open();
                    cmd.Parameters.AddWithValue(parameter.Parameter, parameter.Value);
                    value = cmd.ExecuteScalar();
                }
            }
            return value;
        }

        public object GetScalerValue(string storeProcedureName, DbParameter[] parameters)
        {
            object value = null;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                using (SqlCommand cmd = new SqlCommand(storeProcedureName, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conn.Open();
                    foreach (var para in parameters)
                    {
                        cmd.Parameters.AddWithValue(para.Parameter, para.Value);
                        
                    }
                    value = cmd.ExecuteScalar();
                }
            }
            return value;
        }
    }
}
