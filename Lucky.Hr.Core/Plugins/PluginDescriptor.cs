using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Lucky.Hr.Core.Infrastructure;

namespace Lucky.Hr.Core.Plugins
{
    public class PluginDescriptor : IComparable<PluginDescriptor>
    {
        public PluginDescriptor()
        {
            this.SupportedVersions = new List<string>();
            this.LimitedToStores = new List<int>();
        }


        public PluginDescriptor(Assembly referencedAssembly, FileInfo originalAssemblyFile,
            Type pluginType)
            : this()
        {
            this.ReferencedAssembly = referencedAssembly;
            this.OriginalAssemblyFile = originalAssemblyFile;
            this.PluginType = pluginType;
        }
        /// <summary>
        /// 插件类型
        /// </summary>
        public virtual string PluginFileName { get; set; }

        /// <summary>
        /// 插件类型
        /// </summary>
        public virtual Type PluginType { get; set; }

        /// <summary>
        /// 已阴影复制是活跃在应用程序中的组件
        /// </summary>
        public virtual Assembly ReferencedAssembly { get; internal set; }

        /// <summary>
        /// 这卷影副本是由它制成的原装配文件
        /// </summary>
        public virtual FileInfo OriginalAssemblyFile { get; internal set; }

        /// <summary>
        /// 获取或设置插件组
        /// </summary>
        public virtual string Group { get; set; }

        /// <summary>
        /// 获取或设置友好名称
        /// </summary>
        public virtual string FriendlyName { get; set; }

        /// <summary>
        /// 获取或设置系统名称
        /// </summary>
        public virtual string SystemName { get; set; }

        /// <summary>
        /// 获取或设置版本
        /// </summary>
        public virtual string Version { get; set; }

        /// <summary>
        /// 获取或设置nopCommerce的受支持版本
        /// </summary>
        public virtual IList<string> SupportedVersions { get; set; }

        /// <summary>
        /// 获取或设置作者
        /// </summary>
        public virtual string Author { get; set; }

        /// <summary>
        /// 获取或设置显示顺序
        /// </summary>
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// 获取或设置标识符存储在这个插件是可用的列表。如果为空，那么这个插件适用于所有门店
        /// </summary>
        public virtual IList<int> LimitedToStores { get; set; }

        /// <summary>
        /// 获取或设置该值指示是否已安装插件
        /// </summary>
        public virtual bool Installed { get; set; }

        public virtual T Instance<T>() where T : class, IPlugin
        {
            object instance;
            if (!EngineContext.Current.ContainerManager.TryResolve(PluginType, null, out instance))
            {
                //not resolved
                instance = EngineContext.Current.ContainerManager.ResolveUnregistered(PluginType);
            }
            var typedInstance = instance as T;
            if (typedInstance != null)
                typedInstance.PluginDescriptor = this;
            return typedInstance;
        }

        public IPlugin Instance()
        {
            return Instance<IPlugin>();
        }

        public int CompareTo(PluginDescriptor other)
        {
            if (DisplayOrder != other.DisplayOrder)
                return DisplayOrder.CompareTo(other.DisplayOrder);
            else
                return FriendlyName.CompareTo(other.FriendlyName);
        }

        public override string ToString()
        {
            return FriendlyName;
        }

        public override bool Equals(object obj)
        {
            var other = obj as PluginDescriptor;
            return other != null &&
                SystemName != null &&
                SystemName.Equals(other.SystemName);
        }

        public override int GetHashCode()
        {
            return SystemName.GetHashCode();
        }
    }
}
