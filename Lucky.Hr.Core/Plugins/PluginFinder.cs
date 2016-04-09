using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucky.Core.Plugins
{
    /// <summary>
    /// 插件查找者
    /// </summary>
    public class PluginFinder : IPluginFinder
    {
        #region Fields

        private IList<PluginDescriptor> _plugins;

        private bool _arePluginsLoaded = false;

        #endregion

        #region Utilities

        /// <summary>
        /// 确保插件的加载
        /// </summary>
        protected virtual void EnsurePluginsAreLoaded()
        {
            if (!_arePluginsLoaded)
            {
                var foundPlugins = PluginManager.ReferencedPlugins.ToList();
                foundPlugins.Sort(); //sort
                _plugins = foundPlugins.ToList();

                _arePluginsLoaded = true;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 检查插件是否可用
        /// </summary>
        /// <param name="pluginDescriptor">Plugin descriptor to check</param>
        /// <param name="storeId">Store identifier to check</param>
        /// <returns>true - available; false - no</returns>
        public virtual bool AuthenticateStore(PluginDescriptor pluginDescriptor, int storeId)
        {
            if (pluginDescriptor == null)
                throw new ArgumentNullException("pluginDescriptor");

            //no validation required
            if (storeId == 0)
                return true;

            if (pluginDescriptor.LimitedToStores.Count == 0)
                return true;

            return pluginDescriptor.LimitedToStores.Contains(storeId);
        }

        /// <summary>
        /// 获取插件
        /// </summary>
        /// <typeparam name="T">The type of plugins to get.</typeparam>
        /// <param name="installedOnly">A value indicating whether to load only installed plugins</param>
        /// <param name="storeId">Load records allowed only in a specified store; pass 0 to load all records</param>
        /// <returns>Plugins</returns>
        public virtual IEnumerable<T> GetPlugins<T>(bool installedOnly = true, int storeId = 0) where T : class, IPlugin
        {
            EnsurePluginsAreLoaded();

            foreach (var plugin in _plugins)
                if (typeof(T).IsAssignableFrom(plugin.PluginType))
                    if (!installedOnly || plugin.Installed)
                        if (AuthenticateStore(plugin, storeId))
                            yield return plugin.Instance<T>();
        }

        /// <summary>
        /// Get plugin descriptors
        /// </summary>
        /// <param name="installedOnly">A value indicating whether to load only installed plugins</param>
        /// <param name="storeId">Load records allowed only in a specified store; pass 0 to load all records</param>
        /// <returns>Plugin descriptors</returns>
        public virtual IEnumerable<PluginDescriptor> GetPluginDescriptors(bool installedOnly = true, int storeId = 0)
        {
            EnsurePluginsAreLoaded();

            foreach (var plugin in _plugins)
                if (!installedOnly || plugin.Installed)
                    if (AuthenticateStore(plugin, storeId))
                        yield return plugin;
        }

        /// <summary>
        /// 获取插件描述符
        /// </summary>
        /// <typeparam name="T">The type of plugin to get.</typeparam>
        /// <param name="installedOnly">A value indicating whether to load only installed plugins</param>
        /// <param name="storeId">Load records allowed only in a specified store; pass 0 to load all records</param>
        /// <returns>Plugin descriptors</returns>
        public virtual IEnumerable<PluginDescriptor> GetPluginDescriptors<T>(bool installedOnly = true, int storeId = 0)
            where T : class, IPlugin
        {
            EnsurePluginsAreLoaded();

            foreach (var plugin in _plugins)
                if (typeof(T).IsAssignableFrom(plugin.PluginType))
                    if (!installedOnly || plugin.Installed)
                        if (AuthenticateStore(plugin, storeId))
                            yield return plugin;
        }

        /// <summary>
        /// 获取一个插件描述符由它的系统名称
        /// </summary>
        /// <param name="systemName">Plugin system name</param>
        /// <param name="installedOnly">A value indicating whether to load only installed plugins</param>
        /// <returns>>Plugin descriptor</returns>
        public virtual PluginDescriptor GetPluginDescriptorBySystemName(string systemName, bool installedOnly = true)
        {
            return GetPluginDescriptors(installedOnly)
                .SingleOrDefault(p => p.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// 获取一个插件描述符由它的系统名称
        /// </summary>
        /// <typeparam name="T">The type of plugin to get.</typeparam>
        /// <param name="systemName">Plugin system name</param>
        /// <param name="installedOnly">A value indicating whether to load only installed plugins</param>
        /// <returns>>Plugin descriptor</returns>
        public virtual PluginDescriptor GetPluginDescriptorBySystemName<T>(string systemName, bool installedOnly = true) where T : class, IPlugin
        {
            return GetPluginDescriptors<T>(installedOnly)
                .SingleOrDefault(p => p.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// 重新加载插件
        /// </summary>
        public virtual void ReloadPlugins()
        {
            _arePluginsLoaded = false;
            EnsurePluginsAreLoaded();
        }

        #endregion
    }
}
