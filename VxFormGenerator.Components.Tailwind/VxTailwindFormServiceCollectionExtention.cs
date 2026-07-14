using Microsoft.Extensions.DependencyInjection;
using VxFormGenerator.Repository.Tailwind;

namespace VxFormGenerator.Settings.Tailwind
{
    public static class ServiceCollectionExtensions
    {
        public static void AddVxFormGenerator(this IServiceCollection services, Core.Layout.VxFormLayoutOptions vxFormLayoutOptions = null, VxTailwindRepository repository = null, VxTailwindFormOptions options = null)
        {
            FormGeneratorServiceServiceCollectionExtension.AddVxFormGenerator(services,
                vxFormLayoutOptions ?? new Core.Layout.VxFormLayoutOptions(),
                repository ?? new VxTailwindRepository(),
                options ?? new VxTailwindFormOptions()
                );
        }
    }
}
