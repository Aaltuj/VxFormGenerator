using Microsoft.Extensions.DependencyInjection;
using VxFormGenerator.Repository.Plain;

namespace VxFormGenerator.Settings.Plain
{
    public static class ServiceCollectionExtensions
    {
        public static void AddVxFormGenerator(this IServiceCollection services, VxComponentsRepository repository = null, VxFormOptions options = null)
        {
            FormGeneratorServiceServiceCollectionExtension.AddVxFormGenerator(services, repository ?? new VxComponentsRepository(), options ?? new VxFormOptions());
        }
    }
}
