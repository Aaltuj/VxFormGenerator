using Microsoft.Extensions.DependencyInjection;
using VxFormGenerator.Core;
using VxFormGenerator.Core.Repository;

namespace VxFormGenerator.Settings
{
    public static class FormGeneratorServiceServiceCollectionExtension
    {
        public static void AddVxFormGenerator(IServiceCollection services, Core.Layout.VxFormLayoutOptions vxFormLayoutOptions = null, IFormGeneratorComponentsRepository repository = null, IFormGeneratorOptions options = null)
        {

            if (vxFormLayoutOptions == null)
                throw new System.Exception("No layout options provided, please refer to the documentation.");

            if (repository == null)
                throw new System.Exception("No repository provided, please refer to the documentation.");

            if (options == null)
                throw new System.Exception("No options provided, please refer to the documentation.");


            services.AddSingleton(typeof(IFormGeneratorComponentsRepository), repository);
            services.AddSingleton(typeof(IFormGeneratorOptions), options);
            services.AddSingleton(typeof(Core.Layout.VxFormLayoutOptions), vxFormLayoutOptions);
        }
    }

}
