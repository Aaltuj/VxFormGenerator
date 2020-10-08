
using Microsoft.AspNetCore.Builder;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VxBootstrapFormComponents;
using VxFormGenerator;
using VxFormGenerator.Repository;

namespace FormGeneratorDemo
{
    public class Startup
    {

        /* public FormGeneratorComponentsRepository PlainFormMapping = new FormGeneratorComponentsRepository(
                  new Dictionary<string, Type>()
                  {
                         {typeof(string).ToString(), typeof(InputText) },
                         {typeof(DateTime).ToString(), typeof(InputDate<>) },
                         {typeof(bool).ToString(), typeof(InputCheckbox) },
                         {typeof(FoodKind).ToString(), typeof(InputSelectWithOptions<>) },
                         {typeof(ValueReferences<FoodKind>).ToString(), typeof(InputCheckboxMultiple<>) },
                         {typeof(decimal).ToString(), typeof(InputNumber<>) },
                         {typeof(Color).ToString(), typeof(InputColor) }
                  }, null, typeof(FormElement<>));*/

        /*     public FormGeneratorComponentsRepository BootstrapPlainFormMapping = new FormGeneratorComponentsRepository(
                    new Dictionary<string, Type>()
                    {
                             {typeof(string).ToString(), typeof(InputText) },
                             {typeof(DateTime).ToString(), typeof(InputDate<>) },
                             {typeof(bool).ToString(), typeof(BootstrapInputCheckbox) },
                             {typeof(FoodKind).ToString(), typeof(InputSelectWithOptions<>) },
                             {typeof(ValueReferences<FoodKind>).ToString(), typeof(InputCheckboxMultipleWithChildren<>) },
                             {typeof(decimal).ToString(), typeof(InputNumber<>) },
                             {typeof(Color).ToString(), typeof(InputColor) }
                    }, null, typeof(BootstrapFormElement<>));*/


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton(typeof(IFormGeneratorComponentsRepository), new VxBootstrapFormComponentsRepository());
            services.AddSingleton(typeof(IFormGeneratorOptions), new VxBootstrapFormOptions());


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
