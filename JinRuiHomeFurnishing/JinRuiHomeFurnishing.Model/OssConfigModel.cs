using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JinRuiHomeFurnishing.Model
{
    public class OssConfigModel
    {
        public string AccessKeyId { get; set; }
        public string AccessKeySecret { get; set; }
        public string Endpoint { get; set; }
        public string BucketName { get; set; }
        public string CDNUrl { get; set; }
        public string XiaoLvUrl { get; set; }
    }
}