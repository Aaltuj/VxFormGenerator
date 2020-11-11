using Microsoft.Extensions.DependencyInjection;
using VxFormGenerator.Repository.Bootstrap;

namespace VxFormGenerator.Settings.Bootstrap
{
    public static class ServiceCollectionExtensions 
    {
        public static void AddVxFormGenerator(this IServiceCollection services, VxBootstrapRepository repository = null, VxBootstrapFormOptions options = null)
        {
            FormGeneratorServiceServiceCollectionExtension.AddVxFormGenerator(services, repository ?? new VxBootstrapRepository(), options ?? new VxBootstrapFormOptions());
        }
    }
}
