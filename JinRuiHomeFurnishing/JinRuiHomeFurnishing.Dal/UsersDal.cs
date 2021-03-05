using System;
using System.Collections.Generic;
using System.Text;
using JinRuiHomeFurnishing.Model;
using JinRuiHomeFurnishing.IDal;
using System.Data;
using System.Configuration.Assemblies;
using System.Data.SqlClient;

namespace JinRuiHomeFurnishing.Dal
{
    public class UsersDal : IUsers
    {
        public DataSet getUserList()
        {
            ConnectionPool connect = new ConnectionPool();
            DataSet ds = connect.ExecuteDataSet("select top 10 * from users", ConnectionPool.DataBaseType.JinRui);
            return ds;
        }

    }
}
