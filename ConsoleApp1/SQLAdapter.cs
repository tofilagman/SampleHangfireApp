using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ConsoleApp1
{
    public class SQLAdapter : ISQLAdapter
    {
        private SqlConnection connection;

        public SQLAdapter(string connection)
        {
            this.connection = new SqlConnection(connection);
        }

        public DataTable GetData(string Command)
        {
            using(var dt = new DataTable())
            {
                using(var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = Command;
                    using(var adp = new SqlDataAdapter())
                    {
                        adp.SelectCommand = cmd;
                        adp.Fill(dt);
                        return dt;
                    }
                }
            }
        } 
    }

    public interface ISQLAdapter
    {
        DataTable GetData(string Command);
    }
}
