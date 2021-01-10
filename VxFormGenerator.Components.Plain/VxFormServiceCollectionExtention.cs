using Microsoft.Extensions.DependencyInjection;
using VxFormGenerator.Repository.Plain;

namespace VxFormGenerator.Settings.Plain
{
    public static class ServiceCollectionExtensions
    {
        public static void AddVxFormGenerator(this IServiceCollection services, Core.Layout.VxFormLayoutOptions vxFormLayoutOptions = null, VxComponentsRepository repository = null, VxFormOptions options = null)
        {
            FormGeneratorServiceServiceCollectionExtension.AddVxFormGenerator(services, 
                vxFormLayoutOptions ?? new Core.Layout.VxFormLayoutOptions(),
                repository ?? new VxComponentsRepository(), 
                options ?? new VxFormOptions());
        }

    }
}
