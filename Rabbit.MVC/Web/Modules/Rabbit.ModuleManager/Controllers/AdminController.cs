using Rabbit.Infrastructures;
using Rabbit.Kernel.Environment.Descriptor;
using Rabbit.Kernel.Environment.Descriptor.Models;
using Rabbit.Kernel.Extensions;
using Rabbit.ModuleManager.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Rabbit.ModuleManager.Controllers
{
    public class AdminController : Controller
    {
        private readonly IExtensionManager _extensionManager;
        private readonly IFeatureManager _featureManager;
        private readonly IShellDescriptorManager _shellDescriptorManager;
        private readonly ShellDescriptor _shellDescriptor;

        public AdminController(IExtensionManager extensionManager, IFeatureManager featureManager, IShellDescriptorManager shellDescriptorManager, ShellDescriptor shellDescriptor)
        {
            _extensionManager = extensionManager;
            _featureManager = featureManager;
            _shellDescriptorManager = shellDescriptorManager;
            _shellDescriptor = shellDescriptor;
        }

        public ActionResult Index()
        {
            var availableFeatures = _extensionManager.AvailableFeatures().Select(i => i.Id).ToArray();
            var currentFeatures = _shellDescriptor.Features.Select(i => i.Name).ToArray();

            var model = availableFeatures.Select(i => new FeatureViewModel
            {
                Name = i,
                Enable = currentFeatures.Contains(i)
            }).ToArray();

            return View(model);
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost()
        {
            var cbk = Request["cbk"] ?? string.Empty;

            var newFeatures = _featureManager.GetRequiredFeatures().Concat(cbk.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
            _shellDescriptorManager.UpdateShellDescriptor(_shellDescriptor.SerialNumber + 1, newFeatures.Select(i => new ShellFeature { Name = i }));

            return RedirectToAction("Index");
        }
    }
}