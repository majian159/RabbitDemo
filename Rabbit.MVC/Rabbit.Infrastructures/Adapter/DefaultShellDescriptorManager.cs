using Rabbit.Kernel.Environment.Configuration;
using Rabbit.Kernel.Environment.Descriptor;
using Rabbit.Kernel.Environment.Descriptor.Models;
using Rabbit.Kernel.Extensions;
using Rabbit.Kernel.Localization;
using System.Collections.Generic;
using System.Linq;

namespace Rabbit.Infrastructures.Adapter
{
    [SuppressDependency("Rabbit.Kernel.Environment.Descriptor.Impl.DefaultShellDescriptorManager")]
    internal sealed class ShellDescriptorManager : IShellDescriptorManager
    {
        #region Field

        private readonly IExtensionManager _extensionManager;
        private readonly IEnumerable<IShellDescriptorManagerEventHandler> _events;
        private readonly ShellSettings _settings;
        private readonly IShellDescriptorCache _shellDescriptorCache;

        private static readonly object SyncLock = new object();

        private static int _serialNumber;

        #endregion Field

        #region Property

        public Localizer T { get; set; }

        #endregion Property

        #region Constructor

        public ShellDescriptorManager(IExtensionManager extensionManager, IEnumerable<IShellDescriptorManagerEventHandler> events, ShellSettings settings, IShellDescriptorCache shellDescriptorCache)
        {
            _extensionManager = extensionManager;
            _events = events;
            _settings = settings;
            _shellDescriptorCache = shellDescriptorCache;

            T = NullLocalizer.Instance;
        }

        #endregion Constructor

        #region Implementation of IShellDescriptorManager

        /// <summary>
        /// 获取当前外壳描述符。
        /// </summary>
        /// <returns>外壳描述符。</returns>
        public ShellDescriptor GetShellDescriptor()
        {
            //得到当前租户的外壳描述符。
            var descriptor = _shellDescriptorCache.Fetch(_settings.Name);

            //如果之前的记录与最新的功能描述符相等直接返回。
            if (descriptor != null)
                return descriptor;

            //得到最新的功能描述符名称集合。
            var features = GetFeatures();

            //添加一个新的描述符。
            return new ShellDescriptor
            {
                Features = features.Select(i => new ShellFeature { Name = i }).ToArray(),
                SerialNumber = GetSerialNumber()
            };
        }

        /// <summary>
        /// 更新外壳描述符。
        /// </summary>
        /// <param name="serialNumber">序列号。</param>
        /// <param name="enabledFeatures">需要开启的特性。</param>
        public void UpdateShellDescriptor(int serialNumber, IEnumerable<ShellFeature> enabledFeatures)
        {
            var descriptor = new ShellDescriptor
            {
                SerialNumber = serialNumber,
                Features = enabledFeatures
            };
            _shellDescriptorCache.Store(_settings.Name, descriptor);

            foreach (var handler in _events)
                handler.Changed(descriptor, _settings.Name);
        }

        #endregion Implementation of IShellDescriptorManager

        #region Private Method

        private IEnumerable<string> GetFeatures()
        {
            var features =
                new[] { "Rabbit.Kernel" }.Concat(
                    _extensionManager.AvailableFeatures().Select(i => i.Id)).ToArray();

            return features;
        }

        private static int GetSerialNumber()
        {
            lock (SyncLock)
            {
                _serialNumber = _serialNumber + 1;
                return _serialNumber;
            }
        }

        #endregion Private Method
    }
}