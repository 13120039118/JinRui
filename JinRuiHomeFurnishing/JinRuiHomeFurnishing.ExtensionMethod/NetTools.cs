using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace JinRuiHomeFurnishing.ExtensionMethod
{
    public class NetTools
    {
        #region 取得Ip
        /// <summary> 
        /// 取得客户端真实IP。如果有代理则取第一个非内网地址 ，适用多层代理
        /// </summary> 
        public static string IPAddress
        {
            get
            {
                string result = String.Empty;
                if (HttpContext.Current == null)
                    return "192.168.199.253";

                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (result == null)
                    result = HttpContext.Current.Request.ServerVariables["X_FORWARDED_FOR"];
                if (result == null)
                    result = HttpContext.Current.Request.ServerVariables["X-Real-IP"];
                if (result != null && result != String.Empty)
                {
                    //可能有代理 
                    if (result.IndexOf(".") == -1)    //没有“.”肯定是非IPv4格式 
                        result = null;
                    else
                    {
                        if (result.IndexOf(",") != -1)
                        {
                            //有“,”，估计多个代理。取第一个不是内网的IP。 
                            result = result.Replace(" ", "").Replace("'", "");
                            string[] temparyip = result.Split(",;:".ToCharArray());
                            for (int i = 0; i < temparyip.Length; i++)
                            {
                                if (IsIPAddress(temparyip[i]) && temparyip[i].Substring(0, 3) != "10." && temparyip[i].Substring(0, 7) != "192.168" && temparyip[i].Substring(0, 7) != "172.16.")
                                {
                                    return temparyip[i];    //找到不是内网的地址 
                                }
                            }
                        }
                        else if (IsIPAddress(result)) //代理即是IP格式 
                            return result;
                    }

                }

                ///HTTP_X_REAL_IP为nginx代理用的
                //string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : (HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                if (null == result || result.Length == 0)
                    result = (HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_REAL_IP"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                if (result == null || result.Length == 0)
                    result = HttpContext.Current.Request.UserHostAddress;

                return string.IsNullOrEmpty(result) ? "192.168.199.253" : result;
            }
        }
        #region bool IsIPAddress(str1) 判断是否是IP格式
        /**/
        /// <summary>
        /// 判断是否是IP地址格式 0.0.0.0
        /// </summary>
        /// <param name="str1">待判断的IP地址</param>
        /// <returns>true or false</returns>
        private static Regex regformat = new Regex(@"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$", RegexOptions.IgnoreCase);
        public static bool IsIPAddress(string str1)
        {
            if (str1 == null || str1.Length == 0 || str1.Length < 7 || str1.Length > 15) return false;
            return regformat.IsMatch(str1);
        }
        #endregion
        #endregion


        #region 获取Url取值
        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="Url">请求url</param>
        /// <param name="TimeOut">请求超时（毫秒）</param>
        /// <returns></returns>
        public static string GetResponse(string Url, int TimeOut)
        {
            string responseJson = string.Empty;
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
                myRequest.Timeout = TimeOut;
                myRequest.Method = "GET";
                myRequest.ContentType = "application/requestJson";
                // Get response JSON string             
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                byte[] buffer = ReadInstreamIntoMemory(myResponse.GetResponseStream(), myResponse.ContentEncoding == "gzip");
                responseJson = ByteToHtml(buffer, myResponse.CharacterSet);
                myResponse.Close();
            }
            catch (Exception ex)
            {
            }
            return responseJson;
        }
        public static string GetResponseWithHeader(string Url, string Header, int TimeOut)
        {
            string responseJson = string.Empty;
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
                myRequest.Timeout = TimeOut;
                myRequest.Method = "GET";
                myRequest.Headers.Add(Header);
                myRequest.ContentType = "application/requestJson";
                // Get response JSON string             
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                byte[] buffer = ReadInstreamIntoMemory(myResponse.GetResponseStream(), myResponse.ContentEncoding == "gzip");
                responseJson = ByteToHtml(buffer, myResponse.CharacterSet);
                myResponse.Close();
            }
            catch (Exception ex)
            {
            }
            return responseJson;
        }
        public static string GetResponseWithHeader(string Url, WebHeaderCollection Header, int TimeOut)
        {
            string responseJson = string.Empty;
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                myRequest.Timeout = TimeOut;
                myRequest.Method = "GET";
                myRequest.Headers = Header;
                myRequest.ContentType = "application/requestJson";
                // Get response JSON string             
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                byte[] buffer = ReadInstreamIntoMemory(myResponse.GetResponseStream(), myResponse.ContentEncoding == "gzip");
                responseJson = ByteToHtml(buffer, myResponse.CharacterSet);
                myResponse.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return responseJson;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            HttpWebRequest httpWebrequest = (HttpWebRequest)sender;
            string domain = httpWebrequest.Address.Host;

            return true;
        }

        public static string GetResponse(string Url)
        {
            return GetResponse(Url, 10000);
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string GetResponsePost(string Url, string postData)
        {
            return GetResponsePost(Url, "application/xml", postData);
        }

        public static string GetResponsePost(string Url, string ContentType, string postData)
        {
            string responseXml = string.Empty;
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
                myRequest.Timeout = 10000;
                myRequest.Method = "POST";

                myRequest.ContentType = ContentType;
                byte[] postDatas = Encoding.UTF8.GetBytes(postData);
                myRequest.ContentLength = postDatas.Length;
                using (Stream reqStream = myRequest.GetRequestStream())
                {
                    reqStream.Write(postDatas, 0, postDatas.Length);
                }
                // Get response XML string             
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                byte[] buffer = ReadInstreamIntoMemory(myResponse.GetResponseStream(), myResponse.ContentEncoding == "gzip");
                responseXml = ByteToHtml(buffer, "utf-8");
                myResponse.Close();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return responseXml;
        }

        public static string MultipartFormDataPost(string postUrl, Dictionary<string, object> postParameters)
        {
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            return GetResponsePost(postUrl, contentType, GetMultipartFormData(postParameters, formDataBoundary));
        }

        private static string GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;

            foreach (var param in postParameters)
            {
                // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                // Skip it on the first parameter, add it to subsequent parameters.
                if (needsCLRF)
                    formDataStream.Write(Encoding.UTF8.GetBytes("\r\n"), 0, Encoding.UTF8.GetByteCount("\r\n"));

                needsCLRF = true;

                //if (param.Value is FileParameter)
                //{
                //    FileParameter fileToUpload = (FileParameter)param.Value;

                //    // Add just the first part of this param, since we will write the file data directly to the Stream
                //    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                //        boundary,
                //        param.Key,
                //        fileToUpload.FileName ?? param.Key,
                //        fileToUpload.ContentType ?? "application/octet-stream");

                //    formDataStream.Write(Encoding.UTF8.GetBytes(header), 0, Encoding.UTF8.GetByteCount(header));

                //    // Write the file data directly to the Stream, rather than serializing it to a string.
                //    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                //}
                //else
                //{
                string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                    boundary,
                    param.Key,
                    param.Value);
                formDataStream.Write(Encoding.UTF8.GetBytes(postData), 0, Encoding.UTF8.GetByteCount(postData));
                //}
            }

            // Add the end of the request.  Start with a newline
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(Encoding.UTF8.GetBytes(footer), 0, Encoding.UTF8.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();
            formDataStream.Dispose();

            return Encoding.UTF8.GetString(formData);
        }


        private static byte[] ReadInstreamIntoMemory(Stream stream, bool IsGzip)
        {
            Stream zipStream = stream;
            //如果是Gzip压缩过的页面，需要对流进行解压缩
            if (IsGzip)
                zipStream = new GZipStream(stream, CompressionMode.Decompress, false);
            int bufferSize = 16384;
            byte[] buffer = new byte[bufferSize];
            MemoryStream ms = new MemoryStream();
            while (true)
            {
                int numBytesRead = zipStream.Read(buffer, 0, bufferSize);
                if (numBytesRead <= 0) break;
                ms.Write(buffer, 0, numBytesRead);
            }
            return ms.ToArray();
        }
        static Regex Charset = new Regex("<meta[^>]*charset=(.+)\"?>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        static Regex CharsetContentRegex = new Regex("(?<=charset=[\"|']?).*?(?=[\"|'])", RegexOptions.IgnoreCase);
        private static string ByteToHtml(byte[] buffer, string CharacterSet)
        {
            string HTMLContent = string.Empty;
            if ("iso-8859-1|utf-8".IndexOf(CharacterSet.ToLower()) >= 0)
            {
                HTMLContent = System.Text.Encoding.UTF8.GetString(buffer);
                if ("iso-8859-1".IndexOf(CharacterSet.ToLower()) >= 0)
                {
                    if (CharsetContentRegex.Match(Charset.Match(HTMLContent).Value).Value.ToLower() != "utf-8")
                        HTMLContent = System.Text.Encoding.GetEncoding("gbk").GetString(buffer);
                }
            }
            else
            {
                HTMLContent = System.Text.Encoding.Default.GetString(buffer);
            }
            return HTMLContent;
        }
        #endregion

        public delegate string GetResponseAnsy(string Url);

        public static string ListToString(List<int> list)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (str.Length > 3300)
                {
                    str.Append(list[i]);
                    break;
                }
                if (i == list.Count - 1)//当循环到最后一个的时候 就不添加逗号,
                {
                    str.Append(list[i]);
                }
                else
                {
                    str.Append(list[i]);
                    str.Append(",");
                }
            }
            if (str.Length == 0)
                str.Append("0");
            return str.ToString();
        }

        public static string CutString(object objStr, int characterNum, string FormatString)
        {
            int realNum = characterNum * 2;
            if (objStr == null)
                return string.Empty;
            string str = objStr.ToString();
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            byte[] bs = Encoding.Default.GetBytes(str);
            if (bs.Length <= realNum)
            {
                return str;
            }
            List<byte> nbs = new List<byte>();
            realNum = characterNum * 2;
            for (int i = 0; i <= realNum; i++)
            {
                nbs.Add(bs[i]);
            }
            string rv = Encoding.Default.GetString(nbs.ToArray());
            if (rv.EndsWith("?"))
            {
                if (bs[bs.Length - 1] != Encoding.Default.GetBytes("?")[0])
                {
                    rv = rv.TrimEnd('?');
                }
            }
            return rv + FormatString;
        }
        public static string CutString(object objStr, int characterNum, int holdNum)
        {
            holdNum = 0;
            int realNum = (characterNum + holdNum) * 2;
            if (objStr == null)
                return string.Empty;
            string str = objStr.ToString();
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            byte[] bs = Encoding.Default.GetBytes(str);
            if (bs.Length <= realNum)
                return str;
            List<byte> nbs = new List<byte>();
            realNum = characterNum * 2;
            for (int i = 0; i <= realNum; i++)
            {
                nbs.Add(bs[i]);
            }
            string rv = Encoding.Default.GetString(nbs.ToArray());
            if (rv.EndsWith("?"))
            {
                if (bs[bs.Length - 1] != Encoding.Default.GetBytes("?")[0])
                {
                    rv = rv.TrimEnd('?');
                }
            }
            //return rv + "...";
            return rv;
        }
        public static string CutString(object objStr, int characterNum)
        {
            if (objStr == null)
                return string.Empty;
            string str = objStr.ToString();
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            if (str.Length <= characterNum)
                return str;
            //            return str.Substring(0, characterNum) + "...";
            return str.Substring(0, characterNum);
        }

        static Regex regEx_script = new Regex("<script[^>]*?>[\\s\\S]*?<\\/script>"); //定义script的正则表达式 
        static Regex regEx_style = new Regex("(<style[^>]*?>[\\s\\S]*?<\\/style>)"); //定义style的正则表达式 
        static Regex regEx_html = new Regex("(<[^>]+>)"); //定义HTML标签的正则表达式

        /// <summary>
        /// 删除HTML标签
        /// </summary>
        /// <param name="txtIn"></param>
        /// <returns></returns>
        public static string RemoveHTMLLabel(string txtIn)
        {
            string result = string.Empty;
            if (txtIn != null && txtIn != string.Empty)
            {
                string txt1 = txtIn.Trim().ToLower();
                txt1 = regEx_script.Replace(txt1, "");
                txt1 = regEx_style.Replace(txt1, "");
                txt1 = regEx_html.Replace(txt1, "");
                txt1 = txt1.Replace("&nbsp;", "");
                result = txt1.Trim();
            }
            return result;
        }
        static Regex haveBlankbp = new Regex("(<[aA-zZ-[bpBP]]+[^>]*>)");
        static Regex noBlankbp = new Regex("(</[aA-zZ-[bpBP]]+>)");
        public static string RemoveHtmlStylebp(object txtIn)
        {
            string result = string.Empty;
            if (null != txtIn && !string.IsNullOrEmpty(txtIn.ToString()))
            {
                string txt1 = txtIn.ToString().Trim();
                while (Regex.IsMatch(txt1, "(<[a-z-[bp]]+[^>]*>)", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture) || Regex.IsMatch(txt1, "(</[a-z-[bp]]+>)", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture))
                {
                    txt1 = noBlankbp.Replace(txt1, "");
                    txt1 = haveBlankbp.Replace(txt1, "");
                }
                result = txt1.Trim();
            }
            return result;
        }
        /// <summary>
        /// 修复未关闭的Html标签
        /// </summary>
        public static string RepairHtmlLabelNotClosed(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return string.Empty;
            }
            string oldHtml = html, temp = string.Empty, tabName = string.Empty, tabClose = string.Empty;
            StringBuilder newHtml = new StringBuilder();
            //取开始位置
            int index = html.IndexOf('<');
            int size = 0;
            while (index > -1)
            {
                if (index > 0)
                {
                    //截取第一个标签前的字符
                    newHtml.Append(oldHtml.Substring(0, index));
                    //将截取的从原有字符串中移除
                    oldHtml = oldHtml.Remove(0, index);
                }
                //取第一个结束标签（由于之前的处理，size必定大于index）
                size = oldHtml.IndexOf('>');
                if (oldHtml.IndexOf('<') > size)
                {
                    oldHtml = oldHtml.Remove(0, ++size);
                    continue;
                }
                if (oldHtml.IndexOf('<') >= 0)
                {
                    //将第一个标签放入临时字符串
                    temp = oldHtml.Substring(oldHtml.IndexOf('<'), ++size - oldHtml.IndexOf('<'));
                    oldHtml = oldHtml.Remove(oldHtml.IndexOf('<'), size - oldHtml.IndexOf('<'));
                }
                else
                {
                    temp = oldHtml.Substring(0, ++size);
                    //将截取的从原有字符串中移除
                    oldHtml = oldHtml.Remove(0, size);
                }
                if (temp.StartsWith("</"))
                {
                    continue;
                }
                //获取标签名
                else if (temp.Split(new char[] { ' ', '　' }).Length > 1)
                {
                    tabName = temp.Split(new char[] { ' ', '　' })[0].TrimStart('<').TrimEnd('>');
                }
                else
                {
                    tabName = temp.TrimStart('<').TrimEnd('>');
                }
                //拼出结束标签
                tabClose = string.Format("</{0}>", tabName);
                //取下一个标签
                index = oldHtml.IndexOf('<');
                //取最近的一个匹配的关闭标签
                size = oldHtml.IndexOf(tabClose);
                if (index > 0)
                {
                    //<a></a>的情况
                    if (index == size)
                    {
                        size = oldHtml.IndexOf('>');
                        newHtml.Append(temp);
                        newHtml.Append(oldHtml.Substring(0, ++size));
                        oldHtml = oldHtml.Remove(0, size);
                    }
                    //<a./>的情况
                    else if (temp.EndsWith("/>"))
                    {
                        newHtml.Append(temp);
                        newHtml.Append(oldHtml.Substring(0, index));
                        oldHtml = oldHtml.Remove(0, index);
                    }
                    //没有关闭
                    else
                    {
                        newHtml.Append(temp);
                        newHtml.Append(oldHtml.Substring(0, index));
                        //关闭标签
                        newHtml.Append(tabClose);
                        oldHtml = oldHtml.Remove(0, index);
                    }
                    index = oldHtml.IndexOf('<');
                }
            }
            //最后一段
            if (index < 0)
            {
                newHtml.Append(oldHtml);
            }
            return newHtml.ToString();
        }

        /// <summary>   
        /// 每隔n个字符插入一个字符   
        /// </summary>   
        /// <param name="input">源字符串</param>   
        /// <param name="interval">间隔字符数</param>   
        /// <param name="value">待插入值</param>   
        /// <returns>返回新生成字符串</returns>   
        public static string InsertFormat(string input, int interval, string value)
        {
            for (int i = interval; i < input.Length; i += value.Length + interval + 1)
                input = input.Insert(i, value);
            return input;
        }

        public static string Escape(string str)
        {
            if (str == null)
                return String.Empty;
            StringBuilder sb = new StringBuilder();
            byte[] ba = System.Text.Encoding.Unicode.GetBytes(str);
            for (int i = 0; i < ba.Length; i += 2)
            {
                sb.Append("%u");
                sb.Append(ba[i + 1].ToString("X2"));

                sb.Append(ba[i].ToString("X2"));
            }
            return sb.ToString();

        }
        public static string UNEscape(string s)
        {
            if (s.StartsWith("%u"))
            {
                string str = s.Remove(0, 2);//删除最前面两个＂%u＂
                string[] strArr = str.Split(new string[] { "%u" }, StringSplitOptions.None);//以子字符串＂%u＂分隔
                byte[] byteArr = new byte[strArr.Length * 2];
                for (int i = 0, j = 0; i < strArr.Length; i++, j += 2)
                {
                    byteArr[j + 1] = Convert.ToByte(strArr[i].Substring(0, 2), 16); //把十六进制形式的字串符串转换为二进制字节
                    byteArr[j] = Convert.ToByte(strArr[i].Substring(2, 2), 16);
                }
                str = System.Text.Encoding.Unicode.GetString(byteArr); //把字节转为unicode编码
                return str;
            }
            else
            {
                return s;
            }
        }
        public static string Hash(string url)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] bs = Encoding.UTF8.GetBytes(url);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }

            return s.ToString();
        }

        /// <summary>  
        /// SHA1 加密，返回大写字符串  
        /// </summary>  
        /// <param name="content">需要加密字符串</param>  
        /// <param name="encode">指定加密编码</param>  
        /// <returns>返回40位大写字符串</returns>  
        public static string SHA1(string content)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = Encoding.UTF8.GetBytes(content);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                bytes_in = null;
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                bytes_out = null;
                return result.ToLower();
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1加密出错：" + ex.Message);
            }
        }

        public static string SerializeObject(object value)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();

            StringBuilder sb = new StringBuilder(128);
            StringWriter sw = new StringWriter(sb, CultureInfo.InvariantCulture);
            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Formatting.None;

                jsonSerializer.Serialize(jsonWriter, value);
            }

            return sw.ToString();
        }

        /// <summary>
        /// 生成随机纯字母随机数
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string Str_char(int Length, bool Sleep)
        {
            if (Sleep) System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }

        /// <summary>
        /// 生成随机字母与数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string Str(int Length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }

        public static string GetTimestamp(DateTime CurrTime)
        {
            TimeSpan Ts = CurrTime - DateTime.Parse("1970-01-01 08:00");
            return Convert.ToInt64(Ts.TotalMilliseconds).ToString();
        }

        public static Int64 GetTimestampByZoneLocalTime(DateTime CurrTime)
        {
            TimeSpan Ts = CurrTime - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToInt64(Ts.TotalMilliseconds);
        }

        public static DateTime IntToDateTimeBySeconds(double timestamp)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(timestamp);
        }

        public static DateTime IntToDateTimeByMilliSeconds(double timestamp)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddMilliseconds(timestamp);
        }

        /// <summary>
        /// 数字金额转大写人民币金额
        /// </summary>
        /// <param name="LowerMoney"></param>
        /// <returns></returns>
        public static string MoneyToUpper(string moneyStr)
        {
            if (string.IsNullOrWhiteSpace(moneyStr))
            {
                moneyStr = "0";
            }
            string result = null;
            bool IsNegative = false; // 是否是负数
            if (moneyStr.Trim().Substring(0, 1) == "-")
            {
                // 是负数则先转为正数
                moneyStr = moneyStr.Trim().Remove(0, 1);
                IsNegative = true;
            }
            string strLower = null;
            string strUpart = null;
            string strUpper = null;
            int iTemp = 0;
            // 保留两位小数 123.489→123.49　　123.4→123.4
            double.TryParse(moneyStr, out var doubleMoney);
            moneyStr = Math.Round(doubleMoney, 2).ToString();
            if (moneyStr.IndexOf(".") > 0)
            {
                if (moneyStr.IndexOf(".") == moneyStr.Length - 2)
                {
                    moneyStr = moneyStr + "0";
                }
            }
            else
            {
                moneyStr = moneyStr + ".00";
            }
            strLower = moneyStr;
            iTemp = 1;
            strUpper = "";
            while (iTemp <= strLower.Length)
            {
                switch (strLower.Substring(strLower.Length - iTemp, 1))
                {
                    case ".":
                        strUpart = "圆";
                        break;
                    case "0":
                        strUpart = "零";
                        break;
                    case "1":
                        strUpart = "壹";
                        break;
                    case "2":
                        strUpart = "贰";
                        break;
                    case "3":
                        strUpart = "叁";
                        break;
                    case "4":
                        strUpart = "肆";
                        break;
                    case "5":
                        strUpart = "伍";
                        break;
                    case "6":
                        strUpart = "陆";
                        break;
                    case "7":
                        strUpart = "柒";
                        break;
                    case "8":
                        strUpart = "捌";
                        break;
                    case "9":
                        strUpart = "玖";
                        break;
                }

                switch (iTemp)
                {
                    case 1:
                        strUpart = strUpart + "分";
                        break;
                    case 2:
                        strUpart = strUpart + "角";
                        break;
                    case 3:
                        strUpart = strUpart + "";
                        break;
                    case 4:
                        strUpart = strUpart + "";
                        break;
                    case 5:
                        strUpart = strUpart + "拾";
                        break;
                    case 6:
                        strUpart = strUpart + "佰";
                        break;
                    case 7:
                        strUpart = strUpart + "仟";
                        break;
                    case 8:
                        strUpart = strUpart + "万";
                        break;
                    case 9:
                        strUpart = strUpart + "拾";
                        break;
                    case 10:
                        strUpart = strUpart + "佰";
                        break;
                    case 11:
                        strUpart = strUpart + "仟";
                        break;
                    case 12:
                        strUpart = strUpart + "亿";
                        break;
                    case 13:
                        strUpart = strUpart + "拾";
                        break;
                    case 14:
                        strUpart = strUpart + "佰";
                        break;
                    case 15:
                        strUpart = strUpart + "仟";
                        break;
                    case 16:
                        strUpart = strUpart + "万";
                        break;
                    default:
                        strUpart = strUpart + "";
                        break;
                }

                strUpper = strUpart + strUpper;
                iTemp = iTemp + 1;
            }

            strUpper = strUpper.Replace("零拾", "零");
            strUpper = strUpper.Replace("零佰", "零");
            strUpper = strUpper.Replace("零仟", "零");
            strUpper = strUpper.Replace("零零零", "零");
            strUpper = strUpper.Replace("零零", "零");
            strUpper = strUpper.Replace("零角零分", "整");
            strUpper = strUpper.Replace("零分", "整");
            strUpper = strUpper.Replace("零角", "零");
            strUpper = strUpper.Replace("零亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("亿零万零圆", "亿圆");
            strUpper = strUpper.Replace("零亿零万", "亿");
            strUpper = strUpper.Replace("零万零圆", "万圆");
            strUpper = strUpper.Replace("零亿", "亿");
            strUpper = strUpper.Replace("零万", "万");
            strUpper = strUpper.Replace("零圆", "圆");
            strUpper = strUpper.Replace("零零", "零");

            // 对壹圆以下的金额的处理
            if (strUpper.Substring(0, 1) == "圆")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "零")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "角")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "分")
            {
                strUpper = strUpper.Substring(1, strUpper.Length - 1);
            }
            if (strUpper.Substring(0, 1) == "整")
            {
                strUpper = "零圆整";
            }
            result = strUpper;

            if (IsNegative == true)
            {
                return "负" + result;
            }
            else
            {
                return result;
            }
        }


        /// <summary>
        /// 判断金额数值是否是最大值，如果是则为0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal JudgeIsIntMax(decimal value)
        {
            if (value == int.MaxValue)
            {
                return 0;
            }
            return value;
        }

        public static string ToJson(object obj)
        {
            string result = JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            return result;
        }

        /// <summary>
        /// 非法文件后缀
        /// </summary>
        /// <returns></returns>
        public static string[] UnSafeFileSuffix()
        {
            string[] arr = new string[]
            {
                ".aspx",
                ".ashx",
                ".bat",
                ".cwd",
                ".asp",
                ".sh",
                ".html",
                ".htm",
                ".php"
            };
            return arr;
        }

        /// <summary>
        /// 获取本地IPv4集合
        /// </summary>
        /// <returns></returns>
        public static List<string> GetLocalIPv4()
        {
            var r = new List<string>();
            try
            {
                string name = Dns.GetHostName();
                IPAddress[] ipadrlist = Dns.GetHostAddresses(name);


                foreach (IPAddress ipa in ipadrlist)
                {
                    if (ipa.AddressFamily == AddressFamily.InterNetwork)
                        r.Add(ipa.ToString());
                }
            }
            catch
            {
            }

            if (r.Count == 0)
                r.Add("0.0.0.0");

            return r;
        }

        /// <summary>
        /// 获取当前进程号
        /// </summary>
        public static string GetProcessId()
        {
            try
            {
                Process processes = Process.GetCurrentProcess();
                return processes.Id.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }

        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;

            Byte[] bComments = Encoding.UTF8.GetBytes(p_SrcString);
            foreach (char c in Encoding.UTF8.GetChars(bComments))
            {    //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
                if ((c > '\u0800' && c < '\u4e00') || (c > '\xAC00' && c < '\xD7A3'))
                {
                    //if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") || System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
                    //当截取的起始位置超出字段串长度时
                    if (p_StartIndex >= p_SrcString.Length)
                        return "";
                    else
                        return p_SrcString.Substring(p_StartIndex,
                                                       ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                }
            }

            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }

                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {
                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                                nFlag = 1;
                        }
                        else
                            nFlag = 0;

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                        nRealLength = p_Length + 1;

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);
                    myResult = myResult + p_TailString;
                }
            }

            return myResult;
        }

    }
}
