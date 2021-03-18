using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JinRuiHomeFurnishingNetCoreMVC.Models;
using JinRuiHomeFurnishing.Bll;
using JinRuiHomeFurnishing.Model;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;

namespace JinRuiHomeFurnishingNetCoreMVC.Controllers
{
    [Route("product/[controller]/[action]")]
    [EnableCors("AllowAll")]
    [ApiController]
    public class CustomController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            int i = 1;
            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult userList()
        {

            UsersBll u = new UsersBll();
            DataSet ds = u.getUsersList();

            //方法一
            List<UsersInfo> t2 = new List<UsersInfo>();
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                t2.Add(new UsersInfo
                {
                    UserName = r["UserName"].ToString(),
                    UserTableId = Convert.ToInt32(r["UserTableId"])
                });
            }

            ViewBag.haha = 123;

            return View(t2);

        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        /// <summary>
        /// 获取请求参数req  (base64编码 http://tools.jb51.net/tools/base64_decode-gb2312.php)
        /// </summary>
        /// <returns></returns>
        protected virtual JObject GetReqParam()
        {
            string reqBase64 = "";
            string req = "";
            reqBase64 = Request.Form["req"].ToString().Trim();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            try
            {
                byte[] bytes = Convert.FromBase64String(HttpUtility.UrlDecode(reqBase64).Replace(" ", "+"));
                req = Encoding.GetEncoding(936).GetString(bytes);
            }
            catch
            {
                try
                {
                    if (reqBase64.Contains("\\u003d") || !req.EndsWith("="))
                    {
                        if (reqBase64.Contains("\\u003d"))
                        {
                            reqBase64 = reqBase64.Replace("\\u003d", "=");
                        }
                        else
                        {
                            reqBase64 = reqBase64 + "==";
                        }
                        byte[] bytes = Convert.FromBase64String(HttpUtility.UrlDecode(reqBase64).Replace(" ", "+"));
                        req = Encoding.GetEncoding(936).GetString(bytes);
                    }
                }
                catch
                {
                    try
                    {
                        reqBase64 = reqBase64.Replace("==", "=");
                        byte[] bytes = Convert.FromBase64String(HttpUtility.UrlDecode(reqBase64).Replace(" ", "+"));
                        req = Encoding.GetEncoding(936).GetString(bytes);
                    }
                    catch (Exception e)
                    {

                    }
                }

            }

            var result = (JObject)JsonConvert.DeserializeObject(req);
            return result;
        }

        /// <summary>
        /// 接口方法版本号
        /// </summary>
        protected virtual int Version
        {
            get
            {
                int version = 1;
                if (Request.Form.ContainsKey("v"))
                {
                    int.TryParse(Request.Form["v"].ToString().Trim(), out version);
                }
                return version;
            }
        }

    }
}
