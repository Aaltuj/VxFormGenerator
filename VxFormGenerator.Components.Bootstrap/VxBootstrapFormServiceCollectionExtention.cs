using Microsoft.Extensions.DependencyInjection;
using VxFormGenerator.Repository.Bootstrap;

namespace VxFormGenerator.Settings.Bootstrap
{
    public static class ServiceCollectionExtensions
    {
        public static void AddVxFormGenerator(this IServiceCollection services, Core.Layout.VxFormLayoutOptions vxFormLayoutOptions = null, VxBootstrapRepository repository = null, VxBootstrapFormOptions options = null)
        {
            FormGeneratorServiceServiceCollectionExtension.AddVxFormGenerator(services,
                vxFormLayoutOptions ?? new Core.Layout.VxFormLayoutOptions(),
                repository ?? new VxBootstrapRepository(),
                options ?? new VxBootstrapFormOptions()
                );
        }
    }
}
