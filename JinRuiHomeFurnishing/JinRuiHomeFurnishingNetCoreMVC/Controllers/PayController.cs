using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using JinRuiHomeFurnishing.ExtensionMethod;
using JinRuiHomeFurnishing.ExtensionMethod.util;
using JinRuiHomeFurnishing.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JinRuiHomeFurnishingNetCoreMVC.Controllers
{
    [Route("payment/[controller]/[action]")]  //路由：例 payment/Pay/GetWeiXinPay
    [EnableCors("AllowAll")]  //跨域
    [ApiController]
    public class PayController : ControllerBase
    {
        public static string APPIDNEW = "wx5f9736e31ec0ecf4";  //新的小程序的
        public static string APPSECRECTNEW = "535564a972b584884c1d9b6544056f7f"; //新的小程序的
        public static string WeiXinShangHuHao = "1577111701";  //商户号
        public static string WeiXinShangHuApiKey = "XingWei2021JiaoYushigeyifadacaia";  //商户秘钥

        [HttpGet]
        /// <summary> 
        /// 小程序微信支付  统一下单预支付订单    2020年
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWeiXinPay(string OpendId)
        {
            ApiResult<object> result = new ApiResult<object>();
            try
            {
                //byte[] resultbyte = Convert.FromBase64String(HttpUtility.UrlDecode(GetParam("req").ToString()).Replace(" ", "+"));
                //JObject reqJObject = (JObject)JsonConvert.DeserializeObject(Encoding.GetEncoding(936).GetString(resultbyte));
                //int UserGroupId = 1700328;
                string openId = "oE8lW4zlWqL8n4t6nNkStE3hfRXU";  //小程序需要的  openId

                int OrderId = 123;//后台订单id
                decimal Cash = 1; //支付金额

                #region //微信统一下单支付
                Dictionary<string, string> nativeObj = new Dictionary<string, string>();

                //组成的商户订单号
                string out_trade_no = string.Format("{0}-{1}-{2}-{3}-{4}", 1, 2, 3, 4, 5);  //商户
                nativeObj.Add("openid", openId); //小程序openid (用户唯一表示)
                nativeObj.Add("appid", APPIDNEW); //小程序appid 
                nativeObj.Add("mch_id", WeiXinShangHuHao); //小程序商户号
                nativeObj.Add("nonce_str", PayUtil.CreateNoncestr());  //随机字符串
                nativeObj.Add("body", "中业网校微信小程序,订单号:" + OrderId.ToString()); //商品描述
                nativeObj.Add("out_trade_no", out_trade_no);  //商户订单号
                nativeObj.Add("total_fee", Convert.ToInt64(Cash * 100).ToString());  //标价金额
                nativeObj.Add("spbill_create_ip", "127.0.0.1");  //终端IP
                nativeObj.Add("trade_type", "JSAPI"); //交易类型 小程序取值：JSAPI  其他的请看文档
                nativeObj.Add("sign_type", "MD5"); //签名类型
                nativeObj.Add("notify_url", "http://www.zhongyewx.com/JinRuiHomeFurnishingWebForm/ashxFile/AppNotify.ashx");//通知地址
                string content = PayUtil.FormatBizQueryParaMapForUnifiedPay(nativeObj);//转成url参数格式
                string signStr = content + "&key=" + WeiXinShangHuApiKey;//拼接签名
                nativeObj.Add("sign", PayUtil.GetMD5(signStr)); //生成Md5摘要

                string xml = NetTools.GetResponsePost("https://api.mch.weixin.qq.com/pay/unifiedorder", PayUtil.DictionaryToXmlString(nativeObj));  //请求微信 统一下单URL  

                //微信返回xml格式信息
                if (xml.Length > 0)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);
                    if (doc != null)
                    {
                        XmlNode xmlNode = doc["xml"];
                        if (xmlNode != null)
                        {
                            if (xmlNode["return_code"].InnerText.Equals("SUCCESS") && xmlNode["result_code"].InnerText.Equals("SUCCESS"))  //成功
                            {
                                #region 再次签名（二次签名）
                                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                                Dictionary<string, string> signObj = new Dictionary<string, string>();
                                signObj.Add("appId", APPIDNEW);
                                signObj.Add("timeStamp", Convert.ToInt64(ts.TotalSeconds).ToString());//当前时间戳
                                signObj.Add("nonceStr", PayUtil.CreateNoncestr()); //再次生成一次随机数
                                signObj.Add("package", "prepay_id=" + xmlNode["prepay_id"].InnerText);  //统一下单接口返回的 prepay_id 参数值，提交格式如：prepay_id=***
                                signObj.Add("signType", "MD5");  //写MD5
                                string content2 = PayUtil.FormatBizQueryParaMapForUnifiedPay(signObj);//转成url参数格式
                                string signStr2 = content2 + "&key=" + WeiXinShangHuApiKey;//拼接签名
                                string sign2 = PayUtil.GetMD5(signStr2);//生成Md5摘要
                                #endregion

                                var info = new
                                {
                                    timeStamp = signObj["timeStamp"],
                                    nonceStr = signObj["nonceStr"],
                                    package = signObj["package"],   //统一下单接口返回的 prepay_id 参数值，提交格式如：prepay_id=***
                                    signType = signObj["signType"], //签名方式
                                    notify_url = "http://www.zhongyewx.com/JinRuiHomeFurnishingWebForm/ashxFile/AppNotify.ashx",
                                    paySign = sign2,
                                };

                                result.code = 0;
                                result.message = "统一下单成功";
                                result.data = info;
                                return new JsonResult(result);
                            }
                            else
                            {
                                result.code = 101;
                                result.message = xmlNode["return_msg"].InnerText;
                                return new JsonResult(result);
                            }
                        }
                    }
                }
                #endregion

            }
            catch
            {
                result.code = 101;
                result.message = "异常";
                return new JsonResult(result);
            }

            return new JsonResult(result);
        }



    }
}