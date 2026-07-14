using Microsoft.Extensions.DependencyInjection;
using VxFormGenerator.Repository.Bulma;

namespace VxFormGenerator.Settings.Bulma
{
    public static class ServiceCollectionExtensions
    {
        public static void AddVxFormGenerator(this IServiceCollection services, Core.Layout.VxFormLayoutOptions vxFormLayoutOptions = null, VxBulmaRepository repository = null, VxBulmaFormOptions options = null)
        {
            FormGeneratorServiceServiceCollectionExtension.AddVxFormGenerator(services,
                vxFormLayoutOptions ?? new Core.Layout.VxFormLayoutOptions(),
                repository ?? new VxBulmaRepository(),
                options ?? new VxBulmaFormOptions()
                );
        }
    }
}
