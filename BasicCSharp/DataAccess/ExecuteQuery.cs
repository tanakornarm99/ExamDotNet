using BasicCSharp.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BasicCSharp.DataAccess
{
    public class ExecuteQuery
    {
        private readonly string _conString;

        public ExecuteQuery(string connectionString)
        {
            _conString = connectionString;
        }

        public void ExecuteNonQuery(string commandText, List<Param> parameters)
        {
            using (SqlConnection connection = new SqlConnection(_conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = connection,
                    CommandText = commandText,
                    CommandType = CommandType.Text
                };
                foreach (var item in parameters)
                {
                    cmd.Parameters.AddWithValue("@" + item.Key, item.value);
                }
                cmd.ExecuteNonQuery();
            }
        }   //Insert Update Delete 

        public object ExecuteQueryScalar(string commandText, List<Param> parameters)
        {
            using (SqlConnection connection = new SqlConnection(_conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = connection,
                    CommandText = commandText,
                    CommandType = CommandType.Text
                };
                foreach (var item in parameters)
                {
                    cmd.Parameters.AddWithValue("@" + item.Key, item.value);
                }
                return cmd.ExecuteScalar();
            }
        }   //get ExecuteScalar

        public DataTable ExecuteQueryWithResult(string commandText, List<Param> parameters)
        {
            using (SqlConnection connection = new SqlConnection(_conString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = connection,
                    CommandText = commandText,
                    CommandType = CommandType.Text
                };
                foreach (var item in parameters)
                {
                    cmd.Parameters.AddWithValue("@" + item.Key, item.value);
                }
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
        }   //get datatable

        public Param SetParam(string key, Object value)
        {
            return new Param
            {
                Key = key,
                value = value
            };
        } //Set Parameter

    }
}