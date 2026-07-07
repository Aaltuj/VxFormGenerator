using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using VxFormGenerator.Core.Layout;

namespace VxFormGenerator.Core.Scaffolding
{
    public static class VxFormRazorComponentSourceGenerator
    {
        public static string Generate(Type modelType, VxFormRazorComponentScaffoldOptions options = null)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            options ??= new VxFormRazorComponentScaffoldOptions();
            options.ModelNamespace ??= modelType.Namespace;
            options.ModelTypeName ??= GetTypeName(modelType);
            options.ComponentName ??= modelType.Name.EndsWith("Model", StringComparison.Ordinal)
                ? modelType.Name.Substring(0, modelType.Name.Length - "Model".Length)
                : modelType.Name + "Form";

            var source = new StringBuilder();

            AppendUsings(source, options);
            AppendEditForm(source, options);
            AppendCodeBlock(source, modelType, options);

            return source.ToString();
        }

        private static void AppendUsings(StringBuilder source, VxFormRazorComponentScaffoldOptions options)
        {
            source.AppendLine("@using Microsoft.AspNetCore.Components.Forms");
            source.AppendLine("@using Microsoft.AspNetCore.Components");
            source.AppendLine("@using System.Threading.Tasks");
            source.AppendLine("@using VxFormGenerator.Core");
            source.AppendLine("@using VxFormGenerator.Core.Layout");

            if (!string.IsNullOrWhiteSpace(options.ModelNamespace))
            {
                source.Append("@using ").AppendLine(options.ModelNamespace);
            }

            source.AppendLine();
        }

        private static void AppendEditForm(StringBuilder source, VxFormRazorComponentScaffoldOptions options)
        {
            source.Append("<EditForm Model=\"").Append(options.ModelPropertyName).Append("\" OnValidSubmit=\"HandleValidSubmit\">").AppendLine();

            if (options.UseObjectGraphValidator)
            {
                source.AppendLine("    <ObjectGraphDataAnnotationsValidator />");
            }
            else
            {
                source.AppendLine("    <DataAnnotationsValidator />");
            }

            if (options.IncludeFieldTemplate)
            {
                AppendTemplatedRenderFormElements(source);
            }
            else
            {
                source.AppendLine("    <RenderFormElements FormLayoutOptions=\"Options\" />");
            }

            source.AppendLine("    <button class=\"btn btn-primary\" type=\"submit\">Save</button>");
            source.AppendLine("</EditForm>");
            source.AppendLine();
        }

        private static void AppendTemplatedRenderFormElements(StringBuilder source)
        {
            source.AppendLine("    <RenderFormElements FormLayoutOptions=\"Options\">");
            source.AppendLine("        <FieldTemplate Context=\"field\">");
            source.AppendLine("            <div class=\"vx-generated-field\" data-field-name=\"@field.Name\">");
            source.AppendLine("                @if (field.ShowLabel)");
            source.AppendLine("                {");
            source.AppendLine("                    <label for=\"@field.Id\">@field.Label</label>");
            source.AppendLine("                }");
            source.AppendLine();
            source.AppendLine("                <div class=\"vx-generated-input\">");
            source.AppendLine("                    @field.Input");
            source.AppendLine("                </div>");
            source.AppendLine();
            source.AppendLine("                <div class=\"vx-generated-validation\">");
            source.AppendLine("                    @field.ValidationMessage");
            source.AppendLine("                </div>");
            source.AppendLine("            </div>");
            source.AppendLine("        </FieldTemplate>");
            source.AppendLine("    </RenderFormElements>");
        }

        private static void AppendCodeBlock(StringBuilder source, Type modelType, VxFormRazorComponentScaffoldOptions options)
        {
            source.AppendLine("@code {");
            source.AppendLine("    [Parameter]");
            source.Append("    public ").Append(options.ModelTypeName).Append(' ').Append(options.ModelPropertyName).Append(" { get; set; } = new ").Append(options.ModelTypeName).AppendLine("();");
            source.AppendLine();
            source.AppendLine("    [Parameter]");
            source.Append("    public EventCallback<").Append(options.ModelTypeName).Append("> ").Append(options.SubmitCallbackName).AppendLine(" { get; set; }");
            source.AppendLine();
            source.AppendLine("    private VxFormLayoutOptions Options { get; } = new VxFormLayoutOptions");
            source.AppendLine("    {");
            source.AppendLine("        LabelOrientation = LabelOrientation.TOP,");
            source.AppendLine("        ShowPlaceholder = PlaceholderPolicy.EXPLICIT_LABEL_FALLBACK");
            source.AppendLine("    };");
            source.AppendLine();
            source.AppendLine("    private Task HandleValidSubmit(EditContext context)");
            source.AppendLine("    {");
            source.Append("        return ").Append(options.SubmitCallbackName).Append(".InvokeAsync(").Append(options.ModelPropertyName).AppendLine(");");
            source.AppendLine("    }");
            source.AppendLine();
            AppendModelSummary(source, modelType);
            source.AppendLine("}");
        }

        private static void AppendModelSummary(StringBuilder source, Type modelType)
        {
            var properties = modelType.GetProperties()
                .Where(property => property.GetCustomAttribute<VxIgnoreAttribute>() == null)
                .ToArray();

            if (properties.Length == 0)
            {
                return;
            }

            source.AppendLine("    // Generated from model properties:");

            foreach (var property in properties)
            {
                var display = property.GetCustomAttribute<DisplayAttribute>();
                var label = display?.GetName() ?? property.Name;
                source.Append("    // - ").Append(property.Name).Append(" (").Append(GetTypeName(property.PropertyType)).Append("): ").AppendLine(label);
            }
        }

        private static string GetTypeName(Type type)
        {
            if (!type.IsGenericType)
            {
                return type.FullName ?? type.Name;
            }

            if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return GetTypeName(type.GetGenericArguments()[0]) + "?";
            }

            return type.Name.Substring(0, type.Name.IndexOf('`')) + "<" + string.Join(", ", type.GetGenericArguments().Select(GetTypeName)) + ">";
        }
    }
}
