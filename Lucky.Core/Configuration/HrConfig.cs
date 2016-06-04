using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lucky.Core.Configuration
{
    public class HrConfig : IConfigurationSectionHandler
    {
        /// <summary>
        /// 创建一个配置节处理程序
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="configContext">配置内容</param>
        /// <param name="section">配置节点</param>
        /// <returns>创建的配置节处理程序对象</returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            var config = this;
            var dynamicDiscoveryNode = section.SelectSingleNode("DynamicDiscovery");
            if (dynamicDiscoveryNode != null && dynamicDiscoveryNode.Attributes != null)
            {
                var attribute = dynamicDiscoveryNode.Attributes["Enabled"];
                if (attribute != null)
                    config.DynamicDiscovery = Convert.ToBoolean(attribute.Value);
            }

            var engineNode = section.SelectSingleNode("Engine");
            if (engineNode != null && engineNode.Attributes != null)
            {
                var attribute = engineNode.Attributes["Type"];
                if (attribute != null)
                    config.EngineType = attribute.Value;
            }

            var startupNode = section.SelectSingleNode("Startup");
            if (startupNode != null && startupNode.Attributes != null)
            {
                var attribute = startupNode.Attributes["IgnoreStartupTasks"];
                if (attribute != null)
                    config.IgnoreStartupTasks = Convert.ToBoolean(attribute.Value);
            }

            var themeNode = section.SelectSingleNode("Themes");
            if (themeNode != null && themeNode.Attributes != null)
            {
                var attribute = themeNode.Attributes["basePath"];
                if (attribute != null)
                    config.ThemeBasePath = attribute.Value;
            }

            return config;
        }

        /// <summary>
        /// 除了配置组件检查和在bin目录加载程序集
        /// </summary>
        public bool DynamicDiscovery { get; private set; }

        /// <summary>
        /// 自定义<see cref="IEngine"/>管理应用程序，而不是默认
        /// </summary>
        public string EngineType { get; private set; }

        /// <summary>
        /// 主题存储路径 (~/Themes/)
        /// </summary>
        public string ThemeBasePath { get; private set; }

        /// <summary>
        /// 指示是否应该忽略启动任务
        /// </summary>
        public bool IgnoreStartupTasks { get; private set; }
    }
}
