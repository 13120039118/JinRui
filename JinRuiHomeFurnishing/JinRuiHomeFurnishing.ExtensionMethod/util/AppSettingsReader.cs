
using System.IO;
using JinRuiHomeFurnishing.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

//using OSharp.Extensions;


namespace JinRuiHomeFurnishing.ExtensionMethod
{
    /// <summary>
    /// appsettings配置文件读取器
    /// </summary>
    public class AppSettingsReader
    {
        private static IConfiguration _configuration = (IConfiguration)ServiceLocator.Instance.GetService(typeof(IConfiguration));

        /// <summary>
        /// 读取指定节点信息
        /// </summary>
        /// <param name="key">节点名称，多节点以:分隔</param>
        public static string GetString(string key)
        {
            return _configuration[key];
        }

        /// <summary>
        /// 读取指定节点的简单数据类型的值
        /// </summary>
        /// <param name="key">节点名称，多节点以:分隔</param>
        /// <param name="defaultValue">默认值，读取失败时使用</param>
        public static T GetValue<T>(string key, T defaultValue = default)
        {
            string str = _configuration[key];
            return str.CastTo<T>(defaultValue);
        }

        /// <summary>
        /// 读取指定节点的复杂类型的值，并绑定到指定的空实例上
        /// </summary>
        /// <typeparam name="T">复杂类型</typeparam>
        /// <param name="key">节点名称，多节点以:分隔</param>
        /// <param name="instance">要绑定的空实例</param>
        /// <returns></returns>
        public static T GetInstance<T>(string key, T instance)
        {
            var config = _configuration.GetSection(key);
            if (!config.Exists())
            {
                return default(T);
            }
            config.Bind(instance);
            return instance;
        }

    }
}