using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace JinRuiHomeFurnishing.Dal
{
    /// <summary>
    /// 连接池
    /// </summary>
    public class ConnectionPool
    {
        private string sqljinrui = GetConnString("JinRui");

        /// <summary>
        /// 数据库
        /// </summary>
        public enum DataBaseType
        {
            JinRui = 0,
            MYSQL_JINRUI = 1
        };

        SqlConnection conn; //sql链接
        SqlCommand cmd;  //sql执行命令

        public ConnectionPool()
        {
            cmd = new SqlCommand();
            cmd.CommandTimeout = 36000;
        }

        #region GetConnection
        private SqlConnection GetConnection(DataBaseType db)
        {
            if (conn == null)
            {
                conn = new SqlConnection();
                if (db == DataBaseType.JinRui)
                {
                    conn.ConnectionString = sqljinrui;
                }

                try
                {
                    conn.Open();
                }
                catch (Exception er)
                {
                    //ErrorMessages.SendErrorAlert(er.Message);
                    throw er;
                }
            }
            return conn;
        }
        #endregion

        //获取数据库链接
        private static string GetConnString(string DataName)
        {
            //netFramework
            //if (null == System.Configuration.ConfigurationManager.ConnectionStrings[DataName])
            //{
            //    return string.Empty;
            //}
            //else
            //{
            //    return System.Configuration.ConfigurationManager.ConnectionStrings[DataName].ConnectionString;
            //}

            ///netcore
            if (null == ConfigurationManager.ConnectionStrings[DataName])
            {
                return string.Empty;
            }
            else
            {
                return ConfigurationManager.ConnectionStrings[DataName];
            }
        }

        #region DataSet
        public DataSet ExecuteDataSet(string sqlStr, DataBaseType enumDBType)
        {
            cmd.Connection = GetConnection(enumDBType);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sqlStr;
            SqlDataAdapter daGetData = new SqlDataAdapter(cmd);
            DataSet dsReturnData = new DataSet();

            try
            {
                daGetData.Fill(dsReturnData);
                return dsReturnData;
            }
            catch (Exception er)
            {
                throw er;
            }
            finally
            {
                cmd.Dispose();
                conn.Dispose();
            }
        }
        #endregion

    }
}
