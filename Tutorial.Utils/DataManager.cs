using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Tutorial.Utils
{
    public class DataManager
    {

        private readonly IConfiguration _configuration;
        public DataManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("Conn"));
        }


        public DataSet GetDataSet(string procedure, List<Parameter> par)
        {
            var ds = new DataSet();
            var cmd = new SqlCommand(procedure, CreateConnection())
                {CommandType = CommandType.StoredProcedure, CommandTimeout = 0};
            foreach (var a in par)
            {
                cmd.Parameters.AddWithValue("@"+a.Name.Trim(), a.Value);
            }
            var da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }

        public DataTable GetTable(string query)
        {
            var dt = new DataTable();
            var cmd = new SqlCommand(query, CreateConnection()) {CommandTimeout = 0};
            var da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public int GetNoneQuery(string query)
        {
            var cmd = new SqlCommand(query, CreateConnection()) { CommandTimeout = 0};
            return cmd.ExecuteNonQuery();
        }

    }

    public class Parameter
    {
        public  string Name { get; set; }
        public  object Value{ get; set; }
    }

}
