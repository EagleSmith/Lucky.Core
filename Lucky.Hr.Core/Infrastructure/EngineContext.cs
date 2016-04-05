using System;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Lucky.Hr.Core.Configuration;

namespace Lucky.Hr.Core.Infrastructure
{
    /// <summary>
    /// 提供单例引擎上下文
    /// </summary>
    public class EngineContext 
    {
        #region Initialization Methods
        /// <summary>初始化引擎上下文</summary>
        /// <param name="forceRecreate">造成即使出厂时已预先初始化一个新的工厂实例</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Initialize(bool forceRecreate)
        {
            if (Singleton<IEngine>.Instance == null || forceRecreate)
            {
                var config = ConfigurationManager.GetSection("HrConfig") as HrConfig;
                Debug.WriteLine("构建引擎 " + DateTime.Now);
                Singleton<IEngine>.Instance = CreateEngineInstance(config);
                Debug.WriteLine("初始化引擎 " + DateTime.Now);
                Singleton<IEngine>.Instance.Initialize(config);
            }
            return Singleton<IEngine>.Instance;
        }

        /// <summary>设置为所提供的静态引擎实例。使用这个方法来提供自己的引擎实现</summary>
        /// <param name="engine">引擎实例</param>
        /// <remarks>仅使用此方法，如果你知道自己在做什么</remarks>
        public static void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }

        /// <summary>
        /// 创建一个实例，并添加一个HTTP应用反射
        /// </summary>
        /// <returns>A new factory</returns>
        public static IEngine CreateEngineInstance(HrConfig config)
        {
            if (config != null && !string.IsNullOrEmpty(config.EngineType))
            {
                var engineType = Type.GetType(config.EngineType);
                if (engineType == null)
                    throw new ConfigurationErrorsException("The type '" + engineType + "' could not be found. Please check the configuration at /configuration/nop/engine[@engineType] or check for missing assemblies.");
                if (!typeof(IEngine).IsAssignableFrom(engineType))
                    throw new ConfigurationErrorsException("The type '" + engineType + "' doesn't implement 'Lucky.Core.Infrastructure.IEngine' and cannot be configured in /configuration/nop/engine[@engineType] for that purpose.");
                return Activator.CreateInstance(engineType) as IEngine;
            }

            return new HrEngine();
        }

        #endregion

        /// <summary>得到的单例NOP引擎用来访问NOP服务</summary>
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Initialize(false);
                }
                return Singleton<IEngine>.Instance;
            }
        }
    }
}
