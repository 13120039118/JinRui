using Aliyun.OSS;
using JinRuiHomeFurnishing.Bll;
using JinRuiHomeFurnishing.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JinRuiHomeFurnishing.ExtensionMethod
{
    /// <summary>
    /// 上传文件所属目录
    /// </summary>
    public enum ALiOssDirectory
    {
        /// <summary>
        /// 用户头像
        /// </summary>
        UserImage,
        /// <summary>
        /// 在线答疑
        /// </summary>
        QuestionOnline,
        /// <summary>
        /// 作业在线答疑
        /// </summary>
        TaskQuestionOnline,
        /// <summary>
        /// 试题图片
        /// </summary>
        TExamSubject,
        /// <summary>
        /// APP广告
        /// </summary>
        AppNews,
        /// <summary>
        /// 一对一预约课程
        /// </summary>
        Alone,
        /// <summary>
        /// 微信裂变
        /// </summary>
        WeChatSubscrip,
        /// <summary>
        /// 沟通记录
        /// </summary>
        CommunicationRecord
    }

    /// <summary>
    /// 上传文件所属公司
    /// </summary>
    public enum ALiOssCompany
    {
        /// <summary>
        /// 中业
        /// </summary>
        ZhongYe
    }

    /// <summary>
    /// Oss文件上传状态
    /// </summary>
    public enum OssFileUploadStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 失败
        /// </summary>
        Failed
    }


    public class OSSHelper
    {
        protected static ILogger<OSSHelper> _log = (ILogger<OSSHelper>)ServiceLocator.Instance.GetService(typeof(ILogger<OSSHelper>));

        //此处需注意要装nuget包 ：Microsoft.AspNetCore.Hosting2.2.7  和 Microsoft.Extensions.Configuration.Abstractions 2.2.0 和  Microsoft.Extensions.Configuration2.2.0
        private static AliOssConfig _aliOssConfig = AppSettingsReader.GetInstance("AliOssConfig", new AliOssConfig());

        internal class AliOssConfig
        {
            public string AccessKeyId { get; set; }

            public string AccessKeySecret { get; set; }

            public string Endpoint { get; set; }

            public string BucketName { get; set; }

            public string CDNUrl { get; set; }

            public string XiaoLvUrl { get; set; }
        }


        public static OssClient client = new OssClient(_aliOssConfig.Endpoint, _aliOssConfig.AccessKeyId, _aliOssConfig.AccessKeySecret);

        /// <summary>
        /// 通过IFormFile上传到阿里云OSS
        /// </summary>
        /// <param name="userGroupId">用户UserGroupId</param>
        /// <param name="company">文件所属公司</param>
        /// <param name="directory">文件存放路径</param>
        /// <param name="formFile">文件对象</param>
        /// <param name="fileSizeLimitFuc">判断文件大小的回调函数</param>
        /// <returns></returns>
        public static (string, OssFileUploadStatus, string) PutObject(int userGroupId, ALiOssCompany company, ALiOssDirectory directory, IFormFile formFile, Func<long, string, bool> fileSizeLimitFunc = null)
        {
            string url = string.Empty;
            string month = DateTime.Now.Month <= 9 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
            string path = $"StaticFile/{company.ToString()}/{directory.ToString()}/{ DateTime.Now.Year }{month}/";
            //判断用户是否存在
            //var user = BLLLogicHelper.UsersLogic.Get_Mysql(UsersInfo.Fields.GroupId, userGroupId, UsersInfo.Fields.GroupId);  //获取业务逻辑的 用户id
            var userId = 1700328;  //获取业务逻辑的 用户id 即可
            if (userId == 0)
            {
                return ("", OssFileUploadStatus.Failed, "UserGroupId参数错误，该用户不存在!");
            }
            try
            {

                using (var stream = formFile.OpenReadStream())
                {
                    var fileName = formFile.FileName;

                    //文件后缀
                    var tmpSuffix = fileName.Substring(fileName.LastIndexOf(".")).ToLower();
                    var unSafeFileSuffix = NetTools.UnSafeFileSuffix();
                    if (unSafeFileSuffix.Contains(tmpSuffix))
                    {
                        return ("", OssFileUploadStatus.Failed, "非法文件格式");
                    }
                    if (fileSizeLimitFunc != null)
                    {
                        bool flag = fileSizeLimitFunc(stream.Length, tmpSuffix);
                        if (!flag)
                        {
                            return ("", OssFileUploadStatus.Failed, "文件大小超出限制");
                        }
                    }

                    //构造文件名称 userGroupId_时间戳
                    TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    var timeStamp = ts.TotalMilliseconds;

                    string newFileName = $"{userGroupId}_{timeStamp}";
                    string filePath = path + newFileName + tmpSuffix;
                    var result = client.PutObject(_aliOssConfig.BucketName, filePath, stream, null).ETag;
                    if (!string.IsNullOrEmpty(result))
                    {
                        url = filePath;
                        _log.LogInformation("文件上传成功，文件名称：" + filePath);
                    }
                    else
                    {
                        return ("", OssFileUploadStatus.Failed, "文件上传失败");
                    }
                };
            }
            catch (Exception ex)
            {
                return ("", OssFileUploadStatus.Failed, ex.Message);
            }

            return (url, OssFileUploadStatus.Success, "文件上传成功!");
        }

        /// <summary>
        /// 通过stream上传文件到阿里云OSS
        /// </summary>
        /// <param name="userGroupId"></param>
        /// <param name="company"></param>
        /// <param name="directory"></param>
        /// <param name="stream"></param>
        /// <param name="suffix"></param>
        /// <param name="fileSizeLimitFunc"></param>
        /// <returns></returns>
        public static (string, OssFileUploadStatus, string) PutObject(int userGroupId, ALiOssCompany company, ALiOssDirectory directory, Stream stream, string suffix, Func<long, string, bool> fileSizeLimitFunc = null)
        {
            var unSafeFileSuffix = NetTools.UnSafeFileSuffix();
            if (unSafeFileSuffix.Contains(suffix.ToLower()))
            {
                return ("", OssFileUploadStatus.Failed, "非法文件格式");
            }
            string url = string.Empty;
            string month = DateTime.Now.Month <= 9 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
            string path = $"StaticFile/{company.ToString()}/{directory.ToString()}/{ DateTime.Now.Year }{month}/";
            //判断用户是否存在
            //var user = BLLLogicHelper.UsersLogic.Get_Mysql(UsersInfo.Fields.GroupId, userGroupId, UsersInfo.Fields.GroupId);
            //if (user.GroupId == 0)
            //{
            //    return ("", OssFileUploadStatus.Failed, "UserGroupId参数错误，该用户不存在!");
            //}
            try
            {
                if (fileSizeLimitFunc != null)
                {
                    bool flag = fileSizeLimitFunc(stream.Length, suffix);
                    if (!flag)
                    {
                        return ("", OssFileUploadStatus.Failed, "文件大小超出限制");
                    }
                }

                //构造文件名称 userGroupId_时间戳
                TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                var timeStamp = ts.TotalMilliseconds;

                string newFileName = $"{userGroupId}_{timeStamp}";
                string filePath = path + newFileName + suffix;
                var result = client.PutObject(_aliOssConfig.BucketName, filePath, stream, null).ETag;
                if (!string.IsNullOrEmpty(result))
                {
                    url = filePath;
                    _log.LogInformation("文件上传成功，文件名称：" + filePath);
                }
                else
                {
                    return ("", OssFileUploadStatus.Failed, "文件上传失败");
                }
            }
            catch (Exception ex)
            {
                return ("", OssFileUploadStatus.Failed, ex.Message);
            }

            return (url, OssFileUploadStatus.Success, "文件上传成功!");
        }

        /// <summary>
        /// 语音上传oss
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static (string, OssFileUploadStatus, string) PutObject(string filePath, Stream stream)
        {
            string url = string.Empty;
            try
            {
                var result = client.PutObject("hangzhou-dayiluyin", filePath, stream, null).ETag;
                if (!string.IsNullOrEmpty(result))
                {
                    url = filePath;
                    _log.LogInformation("文件上传成功，文件名称：" + filePath);
                }
                else
                {
                    return ("", OssFileUploadStatus.Failed, "文件上传失败");
                }
            }
            catch (Exception ex)
            {
                return ("", OssFileUploadStatus.Failed, ex.Message);
            }
            return (url, OssFileUploadStatus.Success, "文件上传成功!");

        }

        /// <summary>
        /// 根据objectName删除oss文件
        /// </summary>
        /// <param name="objectName"></param>
        public static void DeleteObject(string objectName)
        {
            try
            {
                client.DeleteObject(_aliOssConfig.BucketName, objectName);
            }
            catch (Exception ex)
            {
                _log.LogError("文件删除失败，异常信息：" + ex.ToString());
            }

        }

        /// <summary>
        /// 将base64的图片转成stream
        /// </summary>
        /// <param name="base64string"></param>
        /// <returns></returns>
        public static (Stream, string) GetImageStreamFromBase64(string base64string)
        {
            try
            {
                var match = Regex.Match(base64string, "data:image/(png|jpg|jpeg|gif|bmp);base64,([\\w\\W]*)$");
                string suffix = ".png";
                if (match.Success)
                {
                    base64string = match.Groups[2].Value;
                    suffix = "." + match.Groups[1].Value; //文件后缀
                }
                byte[] b = Convert.FromBase64String(base64string);
                MemoryStream ms = new MemoryStream(b);
                return (ms, suffix);
            }
            catch (Exception)
            {
                return (null, null);
            }

        }


        /// <summary>
        /// 获取图片完整url
        /// </summary>
        /// <param name="OriginImageUrl"></param>
        /// <param name="hasDomain"></param>
        /// <param name="isOss"></param>
        /// <returns></returns>
        public static string GetImageUrl(string originImageUrl, bool hasDomain = false, bool isOss = false)
        {

            string imageUrl = !string.IsNullOrEmpty(originImageUrl) ? originImageUrl : "";
            if (imageUrl.StartsWith("http"))
            {
                return imageUrl;
            }
            // 如果以 /StaticFile 或者 StaticFile开头的话，直接走oss域名          
            if (imageUrl.StartsWith("/StaticFile"))
            {
                imageUrl = _aliOssConfig.CDNUrl + imageUrl;
                return imageUrl;
            }
            else if (imageUrl.StartsWith("StaticFile"))
            {
                imageUrl = _aliOssConfig.CDNUrl + "/" + imageUrl;
                return imageUrl;
            }
            else if (imageUrl.StartsWith("/static"))
            {
                // 兼容效率部那边的图片拼接方式
                imageUrl = imageUrl.StartsWith("/") ? (_aliOssConfig.XiaoLvUrl + imageUrl) : (_aliOssConfig.XiaoLvUrl + "/" + imageUrl);
                return imageUrl;
            }
            else if (isOss)
            {
                // 兼容服务器静态资源上传oss后的情况
                imageUrl = imageUrl.StartsWith("/") ? _aliOssConfig.CDNUrl + imageUrl : (_aliOssConfig.CDNUrl + "/" + imageUrl);
                return imageUrl;
            }
            else
            {
                if (imageUrl.StartsWith("http") || !hasDomain)
                {
                    return imageUrl;
                }
                else
                {
                    return string.Format("http://{0}{1}{2}", "www.zhongyewx.com",
                        (imageUrl.StartsWith("/") ? "" : "/"), imageUrl);
                }
            }
        }

    }
}
