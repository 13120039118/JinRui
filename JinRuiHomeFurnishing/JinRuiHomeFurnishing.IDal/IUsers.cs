using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using JinRuiHomeFurnishing.Model;

namespace JinRuiHomeFurnishing.IDal
{
    public interface IUsers
    {
        DataSet getUserList();
    }
}
