using Rabbit.Kernel;

namespace Rabbit.Infrastructures
{
    public interface IFeatureManager : IDependency
    {
        string[] GetRequiredFeatures();
    }

    internal sealed class FeatureManager : IFeatureManager
    {
        #region Implementation of IFeatureManager

        public string[] GetRequiredFeatures()
        {
            return new[] { "Rabbit.ModuleManager" };
        }

        #endregion Implementation of IFeatureManager
    }
}