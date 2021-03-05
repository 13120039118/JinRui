using System;
using JinRuiHomeFurnishing.IDal;
using JinRuiHomeFurnishing.Dal;
using System.Data;
using System.Reflection;

namespace JinRuiHomeFurnishing.Bll
{
    public class UsersBll
    {
        public DataSet getUsersList()
        {
            IUsers i = Create();
            return i.getUserList();
        }

        public static IUsers Create()
        {
            object objType = Assembly.Load("JinRuiHomeFurnishing.Dal").CreateInstance("JinRuiHomeFurnishing.Dal.UsersDal");
            return (IUsers)objType;
        }

    }
}
