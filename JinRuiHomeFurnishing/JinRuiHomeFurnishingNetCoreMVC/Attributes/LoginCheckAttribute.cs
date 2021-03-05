using JinRuiHomeFurnishing.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinRuiHomeFurnishingNetCoreMVC.Attributes
{
    /// <summary>
    /// 验证登录的属性
    /// </summary>
    public class LoginCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var host = context.HttpContext.Request.Host.Value;
            if (host.StartsWith("localhost"))
            {
            }
            else
            {
                ApiResult<object> apiResult = new ApiResult<object>();
                string method = context.HttpContext.Request.Method;
                string userAuthKeyBase64 = "";
                int userGroupId = 0;
                if (method.Equals("POST") || method.Equals("GET"))
                {
                    userAuthKeyBase64 = context.HttpContext.Request.Form["ua"].ToString().Trim();
                    if (string.IsNullOrEmpty(userAuthKeyBase64))
                    {
                        userAuthKeyBase64 = context.HttpContext.Request.Query["ua"].ToString().Trim();
                    }
                    int.TryParse(context.HttpContext.Request.Form["groupId"].ToString().Trim(), out userGroupId);
                }
                else
                {
                    apiResult.code = 0;
                    apiResult.message = "不持支POST、GET以外其他请求方式";
                    context.Result = new JsonResult(apiResult);
                }
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                byte[] bytes = Convert.FromBase64String(userAuthKeyBase64);
                string userAuthKey = Encoding.GetEncoding(936).GetString(bytes);
                //4:表示app类型
                Guid authKey = new Guid();
                if (!authKey.ToString().Equals(userAuthKey))
                {
                    apiResult.code = 102;
                    apiResult.message = "请您重新登陆!";
                    context.Result = new JsonResult(apiResult);
                }
            }
        }
    }
}
