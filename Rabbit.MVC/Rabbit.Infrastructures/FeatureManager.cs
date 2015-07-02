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
            return new[] { "Rabbit.Kernel", "Rabbit.Components.Logging.NLog", "Rabbit.Web", "Rabbit.Web.Mvc", "Rabbit.Components.Security", "Rabbit.Components.Security.Web", "Rabbit.Infrastructures", "Shapes", "Default_TheThemeMachine", "TheAdmin", "TheThemeMachine", "Rabbit.ModuleManager" };
        }

        #endregion Implementation of IFeatureManager
    }
}