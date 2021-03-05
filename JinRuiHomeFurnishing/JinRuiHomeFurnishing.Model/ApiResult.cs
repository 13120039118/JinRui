using System;
using System.Collections.Generic;
using System.Text;

namespace JinRuiHomeFurnishing.Model
{
    public class ApiResult<T>
    {
        public int code { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}
