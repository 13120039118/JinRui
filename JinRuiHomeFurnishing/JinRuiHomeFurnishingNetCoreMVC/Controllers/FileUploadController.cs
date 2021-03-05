using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using JinRuiHomeFurnishing.ExtensionMethod;
using JinRuiHomeFurnishing.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace JinRuiHomeFurnishingNetCoreMVC.Controllers
{
    [Route("product/[controller]/[action]")]
    [EnableCors("AllowAll")]
    [ApiController]
    public class FileUploadController : CustomController
    {
        /// <summary>
        /// 通用文件上传接口
        /// </summary>
        /// <param name="files">文件</param>
        /// <param name="groupId">文件所属用户</param>
        /// <param name="company">文件所属公司 0表示中业</param>
        /// <param name="directory">文件所属目录 0用户头像 1在线答疑 2作业在线答疑 3试题图片 4APP广告 5一对一预约课程 6微信裂变 7沟通记录</param>       
        /// <param name="fileType">文件类型 fileType大于0时表示上传经base64的图片上传</param>
        /// <param name="fileCount">文件数量</param>
        /// <param name="pic_i">经base64的图片文件 i表示文件索引 i从1开始计数   好像没用到传空串吧</param>
        /// <returns></returns>       
        [HttpPost("UploadFile")]
        public object UploadFile([FromForm][Required] IFormFileCollection files, [FromForm][Required] int groupId, [FromForm][Required] int company, [FromForm][Required] int directory, [FromForm][Required] int fileType, [FromForm] int fileCount, [FromForm] string pic_i)
        {
            ApiResult<object> result = new ApiResult<object>();
            List<object> list = new List<object>();
            List<string> objectNameList = new List<string>();
            int i = 1;
            var keys = Request.Query.Keys;
            if (!(keys.Contains("groupId") && keys.Contains("company") && keys.Contains("directory")))
            {
                //如果query中没有需要的参数，则去form中取
                keys = Request.Form.Keys;
                if (!(keys.Contains("groupId") && keys.Contains("company") && keys.Contains("directory")))
                {
                    result.code = 101;
                    result.message = "参数错误";
                    return result;
                }
            }
            var host = Request.Host.Value;
            // 如果上传的文件是经base64的图片
            if (fileType > 0)
            {
                for (int n = 1; n <= fileCount; n++)
                {
                    var (stream, fileSuffix) = OSSHelper.GetImageStreamFromBase64(Request.Form["pic_" + n].ToString());
                    if (stream == null)
                    {
                        result.code = 101;
                        result.message = "图片数据格式错误";
                        return result;
                    }

                    var (url, status, msg) = OSSHelper.PutObject(groupId, (ALiOssCompany)company, (ALiOssDirectory)directory, stream, fileSuffix, FileSizeLimit);
                    if (status == OssFileUploadStatus.Failed)
                    {
                        foreach (var item in objectNameList)
                        {
                            OSSHelper.DeleteObject(item);
                        }
                        result.message = msg;
                        return result;
                    }
                    var obj = new
                    {
                        Num = n,
                        Name = "",
                        Url = "/" + url
                    };
                    objectNameList.Add(url);
                    list.Add(obj);
                }
                result.code = 0;
                result.message = "success";
                result.data = list;
                return result;
            }

            foreach (var file in files)
            {
                string url = "", msg = "";
                OssFileUploadStatus status = OssFileUploadStatus.Failed;
                // 兼容语音答疑上传
                if (file.FileName.Trim().EndsWith(".aac"))
                {
                    using (var stream = file.OpenReadStream())
                    {
                        (url, status, msg) = OSSHelper.PutObject(file.FileName, stream);
                    }
                }
                else
                {
                    (url, status, msg) = OSSHelper.PutObject(groupId, (ALiOssCompany)company, (ALiOssDirectory)directory, file, FileSizeLimit);
                }

                if (status == OssFileUploadStatus.Failed)
                {
                    // 多文件上传,如果一个其中一个失败，则删除已经成功上传的文件
                    foreach (var item in objectNameList)
                    {
                        OSSHelper.DeleteObject(item);
                    }
                    result.code = 101;
                    result.message = msg;
                    return result;
                }

                var obj = new
                {
                    Num = i,
                    Name = file.FileName,
                    Url = "/" + url
                };
                i++;
                objectNameList.Add(url);
                list.Add(obj);
            }
            result.code = 0;
            result.data = list;
            return result;
        }

        private bool FileSizeLimit(long fileSize, string suffix)
        {
            // 图片格式的文件大小限制
            var suffixArr = new string[] { ".png", ".jpg", "bmp", "gif", ".jpeg" };
            if (suffixArr.Contains(suffix.ToLower()))
            {
                var kb = Math.Round((double)(fileSize / 1024), 2);
                //单位MB
                int maxSize = AppSettingsReader.GetValue<int>("OSSFileSizeLimit:Image:UserIamge");
                if (kb > maxSize * 1024)
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 修改用户头像
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateUsersImage()
        {
            JObject reqJObject = GetReqParam();
            ApiResult<object> result = new ApiResult<object>();
            if (Version == 1)
            {
                result.code = 100;
                //判断接收的数据是否包好指定的键

                string UserImageUrl = "";
                if (reqJObject.ContainsKey("UImg"))
                {
                    UserImageUrl = reqJObject["UImg"].ToString();
                }
                else
                {
                    result.code = 100;
                    result.message = "参数错误";
                    return new JsonResult(result);
                }


                if (UserImageUrl.Length > 0)
                {
                    var imageTuple = OSSHelper.GetImageStreamFromBase64(UserImageUrl);

                    var tuple = OSSHelper.PutObject(1700328, ALiOssCompany.ZhongYe, ALiOssDirectory.UserImage, imageTuple.Item1, imageTuple.Item2, null);
                    if (tuple.Item2 == OssFileUploadStatus.Success)
                    {
                        result.code = 0;
                        result.message = "保存成功";
                        //result.data = OSSHelper.GetImageUrl(UserInfo.ImageUrl, true, true);
                        return new JsonResult(result);
                    }
                }
                result.code = 100;
                result.message = "保存失败";
                return new JsonResult(result);
            }
            return new JsonResult(result);
        }






    }
}