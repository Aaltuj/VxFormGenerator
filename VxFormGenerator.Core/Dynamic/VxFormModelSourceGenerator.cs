using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VxFormGenerator.Core.Dynamic
{
    public static class VxFormModelSourceGenerator
    {
        private static readonly HashSet<string> CSharpKeywords = new HashSet<string>
        {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked",
            "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else",
            "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for",
            "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock",
            "long", "namespace", "new", "null", "object", "operator", "out", "override", "params",
            "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short",
            "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true",
            "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual",
            "void", "volatile", "while"
        };

        public static string Generate(VxFormModelDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            if (string.IsNullOrWhiteSpace(definition.ClassName))
            {
                throw new ArgumentException("A class name is required.", nameof(definition));
            }

            var source = new StringBuilder();

            foreach (var @using in definition.Usings.Where(value => !string.IsNullOrWhiteSpace(value)).Distinct())
            {
                source.Append("using ").Append(@using).AppendLine(";");
            }

            source.AppendLine();
            source.Append("namespace ").AppendLine(string.IsNullOrWhiteSpace(definition.Namespace) ? "VxFormGenerator.Generated" : definition.Namespace);
            source.AppendLine("{");

            foreach (var attribute in definition.Attributes.Where(value => !string.IsNullOrWhiteSpace(value)))
            {
                AppendAttribute(source, attribute, 1);
            }

            source.Append("    public class ").AppendLine(ToIdentifier(definition.ClassName));
            source.AppendLine("    {");

            foreach (var property in definition.Properties)
            {
                AppendProperty(source, property);
            }

            source.AppendLine("    }");
            source.AppendLine("}");

            return source.ToString();
        }

        private static void AppendProperty(StringBuilder source, VxFormModelPropertyDefinition property)
        {
            if (property == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(property.Name))
            {
                throw new ArgumentException("Every generated property requires a name.");
            }

            foreach (var attribute in BuildAttributes(property))
            {
                AppendAttribute(source, attribute, 2);
            }

            source.Append("        public ")
                .Append(string.IsNullOrWhiteSpace(property.TypeName) ? "string" : property.TypeName)
                .Append(' ')
                .Append(ToIdentifier(property.Name))
                .Append(" { get; set; }");

            if (!string.IsNullOrWhiteSpace(property.DefaultValueExpression))
            {
                source.Append(" = ").Append(property.DefaultValueExpression).Append(';');
            }

            source.AppendLine();
            source.AppendLine();
        }

        private static IEnumerable<string> BuildAttributes(VxFormModelPropertyDefinition property)
        {
            if (!string.IsNullOrWhiteSpace(property.Label) || property.Order.HasValue)
            {
                var displayArgs = new List<string>();

                if (!string.IsNullOrWhiteSpace(property.Label))
                {
                    displayArgs.Add($"Name = \"{Escape(property.Label)}\"");
                }

                if (property.Order.HasValue)
                {
                    displayArgs.Add($"Order = {property.Order.Value}");
                }

                yield return $"Display({string.Join(", ", displayArgs)})";
            }

            var layoutArgs = new List<string>();

            if (property.RowId.HasValue)
            {
                layoutArgs.Add($"RowId = {property.RowId.Value}");
            }

            if (property.ColSpan.HasValue)
            {
                layoutArgs.Add($"ColSpan = {property.ColSpan.Value}");
            }

            if (!string.IsNullOrWhiteSpace(property.Label))
            {
                layoutArgs.Add($"Label = \"{Escape(property.Label)}\"");
            }

            if (!string.IsNullOrWhiteSpace(property.Placeholder))
            {
                layoutArgs.Add($"Placeholder = \"{Escape(property.Placeholder)}\"");
            }

            if (!string.IsNullOrWhiteSpace(property.Description))
            {
                layoutArgs.Add($"Description = \"{Escape(property.Description)}\"");
            }

            if (property.Order.HasValue)
            {
                layoutArgs.Add($"Order = {property.Order.Value}");
            }

            if (layoutArgs.Count > 0)
            {
                yield return $"VxFormElementLayout({string.Join(", ", layoutArgs)})";
            }

            if (property.IsRequired)
            {
                yield return "Required";
            }

            if (property.MaxLength.HasValue)
            {
                if (property.MinLength.HasValue)
                {
                    yield return $"StringLength({property.MaxLength.Value}, MinimumLength = {property.MinLength.Value})";
                }
                else
                {
                    yield return $"StringLength({property.MaxLength.Value})";
                }
            }
            else if (property.MinLength.HasValue)
            {
                yield return $"MinLength({property.MinLength.Value})";
            }

            if (!string.IsNullOrWhiteSpace(property.RangeMinimum) && !string.IsNullOrWhiteSpace(property.RangeMaximum))
            {
                yield return $"Range(typeof({property.TypeName}), \"{Escape(property.RangeMinimum)}\", \"{Escape(property.RangeMaximum)}\")";
            }

            foreach (var attribute in property.Attributes.Where(value => !string.IsNullOrWhiteSpace(value)))
            {
                yield return attribute;
            }
        }

        private static void AppendAttribute(StringBuilder source, string attribute, int indentLevel)
        {
            source.Append(new string(' ', indentLevel * 4));
            source.Append('[').Append(attribute.Trim().Trim('[', ']')).AppendLine("]");
        }

        private static string ToIdentifier(string value)
        {
            var identifier = new StringBuilder();

            foreach (var character in value.Trim())
            {
                identifier.Append(char.IsLetterOrDigit(character) || character == '_' ? character : '_');
            }

            if (identifier.Length == 0 || char.IsDigit(identifier[0]))
            {
                identifier.Insert(0, '_');
            }

            var output = identifier.ToString();
            return CSharpKeywords.Contains(output) ? "@" + output : output;
        }

        private static string Escape(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }
}
