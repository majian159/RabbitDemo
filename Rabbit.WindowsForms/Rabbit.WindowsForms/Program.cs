using Autofac;
using Rabbit.Kernel;
using Rabbit.Kernel.Caching;
using Rabbit.Kernel.Caching.Impl;
using Rabbit.Kernel.Environment;
using Rabbit.Kernel.Environment.Configuration;
using System;
using System.Windows.Forms;

namespace Rabbit.WindowsForms
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var kernelBuilder = new KernelBuilder();

            kernelBuilder.OnStarting(builder => builder.RegisterType<MainForm>().InstancePerLifetimeScope());
            kernelBuilder.UseCaching(c => c.UseMemoryCache());

            var hostContainer = kernelBuilder.Build();
            var host = hostContainer.Resolve<IHost>();
            host.Initialize();

            var work = host.CreateStandaloneEnvironment(new ShellSettings { Name = "Default" });

            var form = work.Resolve<MainForm>();
            Application.Run(form);
        }
    }
}