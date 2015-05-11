using Rabbit.Kernel;

namespace Rabbit.Infrastructures
{
    public static class BuilderExtensions
    {
        public static void UseInfrastructures(this IKernelBuilder kernelBuilder)
        {
            kernelBuilder.RegisterExtension(typeof(BuilderExtensions).Assembly);
        }
    }
}