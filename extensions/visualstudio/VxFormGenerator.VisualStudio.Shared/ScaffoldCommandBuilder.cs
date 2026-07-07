using System;
using System.Globalization;
using System.IO;

namespace VxFormGenerator.VisualStudio.Shared
{
    public static class ScaffoldCommandBuilder
    {
        public static ScaffoldCommand Build(string solutionDirectory, string assemblyPath, string typeName, string outputPath)
        {
            if (string.IsNullOrWhiteSpace(solutionDirectory))
            {
                throw new ArgumentException("A solution directory is required.", nameof(solutionDirectory));
            }

            if (string.IsNullOrWhiteSpace(assemblyPath))
            {
                throw new ArgumentException("An assembly path is required.", nameof(assemblyPath));
            }

            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentException("A model type name is required.", nameof(typeName));
            }

            if (string.IsNullOrWhiteSpace(outputPath))
            {
                throw new ArgumentException("An output path is required.", nameof(outputPath));
            }

            var toolProject = Path.Combine(solutionDirectory, "VxFormGenerator.Tools", "VxFormGenerator.Tools.csproj");
            if (File.Exists(toolProject))
            {
                return new ScaffoldCommand(
                    "dotnet",
                    string.Format(CultureInfo.InvariantCulture, "run --project \"{0}\" -- scaffold --assembly \"{1}\" --type \"{2}\" --output \"{3}\"", toolProject, assemblyPath, typeName.Trim(), outputPath));
            }

            return new ScaffoldCommand(
                "vxform",
                string.Format(CultureInfo.InvariantCulture, "scaffold --assembly \"{0}\" --type \"{1}\" --output \"{2}\"", assemblyPath, typeName.Trim(), outputPath));
        }
    }
}
