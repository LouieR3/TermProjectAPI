using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace _3342_TermProject_API.Classes
{
    public class Validation
{
        DBConnect db = new DBConnect();
        SqlCommand dbCommand = new SqlCommand();
        public Validation()
        {

        }
        public int checkAPIKey(string APIKey)
        {
            dbCommand.Parameters.Clear();
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.CommandText = "TP_CheckAPIKeyExists";

            SqlParameter inputEmail = new SqlParameter("@APIKey", APIKey);
            SqlParameter outputCount = new SqlParameter("@Count", 0);

            inputEmail.Direction = ParameterDirection.Input;
            inputEmail.SqlDbType = SqlDbType.VarChar;
            outputCount.Direction = ParameterDirection.Output;
            outputCount.SqlDbType = SqlDbType.Int;

            dbCommand.Parameters.Add(inputEmail);
            dbCommand.Parameters.Add(outputCount);

            db.GetDataSetUsingCmdObj(dbCommand);
            int count = int.Parse(dbCommand.Parameters["@Count"].Value.ToString());
            return count;
        }
}
}
