using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JinRuiHomeFurnishing.ExtensionMethod.util
{
    /// <summary>
    /// 支付的一些使用方法
    /// </summary>
    public class PayUtil
    {
        /// <summary>
        /// 获取微信的随机数
        /// </summary>
        /// <returns></returns>
        public static string CreateNoncestr()
        {
            String chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            String res = "";
            Random rd = new Random();
            for (int i = 0; i < 16; i++)
            {
                res += chars[rd.Next(chars.Length - 1)];
            }
            return res;
        }

        /// <summary>
        /// 把字典内的参数拼接成微信的xml
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string DictionaryToXmlString(Dictionary<string, string> dic)
        {
            StringBuilder xmlString = new StringBuilder();
            xmlString.Append("<xml>");
            foreach (string key in dic.Keys)
            {
                xmlString.Append(string.Format("<{0}><![CDATA[{1}]]></{0}>", key, dic[key]));
            }
            xmlString.Append("</xml>");
            return xmlString.ToString();
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="encypStr"></param>
        /// <returns></returns>
        public static string GetMD5(string encypStr)
        {
            char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                    'A', 'B', 'C', 'D', 'E', 'F' };
            try
            {

                byte[] btInput = System.Text.Encoding.UTF8.GetBytes(encypStr);
                // 获得MD5摘要算法的 MessageDigest 对象
                MD5 mdInst = System.Security.Cryptography.MD5.Create();
                // 使用指定的字节更新摘要
                mdInst.ComputeHash(btInput);
                // 获得密文
                byte[] md = mdInst.Hash;
                // 把密文转换成十六进制的字符串形式
                int j = md.Length;
                char[] str = new char[j * 2];
                int k = 0;
                for (int i = 0; i < j; i++)
                {
                    byte byte0 = md[i];
                    str[k++] = hexDigits[(int)(((byte)byte0) >> 4) & 0xf];
                    str[k++] = hexDigits[byte0 & 0xf];
                }
                return new string(str);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// 把字典内的数据拼接成  url参数  例：  ?par1=1&par2=2
        /// </summary>
        /// <param name="paraMap"></param>
        /// <returns></returns>
        public static string FormatBizQueryParaMapForUnifiedPay(Dictionary<string, string> paraMap)
        {
            string buff = "";
            try
            {
                var result = from pair in paraMap orderby pair.Key select pair;
                foreach (KeyValuePair<string, string> pair in result)
                {
                    if (pair.Key != "")
                    {

                        string key = pair.Key;
                        string val = pair.Value;
                        buff += key + "=" + val + "&";
                    }
                }

                if (buff.Length == 0 == false)
                {
                    buff = buff.Substring(0, (buff.Length - 1) - (0));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return buff;
        }

        /// <summary>
        /// 从网银返回订单号中提取本地订单号
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public static int GetOrderId(string SendOrderId)
        {
            string[] OrderIdFormat = SendOrderId.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            int _OrderId = 0;
            if (OrderIdFormat.Length > 0)
                int.TryParse(OrderIdFormat[1], out _OrderId);
            return _OrderId;
        }

        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string str)
        {
            byte[] b = Encoding.UTF8.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');

            return ret;
        }
    }
}
