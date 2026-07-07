using System;
using System.IO;
using VxFormGenerator.VisualStudio.Shared;
using Xunit;

namespace VxFormGenerator.VisualStudio.Tests
{
    public class ScaffoldCommandBuilderTests
    {
        [Fact]
        public void Build_UsesSourceToolProjectWhenPresent()
        {
            var solutionDirectory = CreateTemporaryDirectory();
            var toolProject = Path.Combine(solutionDirectory, "VxFormGenerator.Tools", "VxFormGenerator.Tools.csproj");
            Directory.CreateDirectory(Path.GetDirectoryName(toolProject)!);
            File.WriteAllText(toolProject, "<Project />");

            var command = ScaffoldCommandBuilder.Build(solutionDirectory, "models.dll", " Demo.CustomerModel ", "Forms/CustomerForm.razor");

            Assert.Equal("dotnet", command.FileName);
            Assert.Contains("run --project", command.Arguments);
            Assert.Contains("VxFormGenerator.Tools.csproj", command.Arguments);
            Assert.Contains("--type \"Demo.CustomerModel\"", command.Arguments);
        }

        [Fact]
        public void Build_UsesInstalledToolWhenProjectIsMissing()
        {
            var solutionDirectory = CreateTemporaryDirectory();

            var command = ScaffoldCommandBuilder.Build(solutionDirectory, "models.dll", "Demo.CustomerModel", "Forms/CustomerForm.razor");

            Assert.Equal("vxform", command.FileName);
            Assert.StartsWith("scaffold", command.Arguments, StringComparison.Ordinal);
            Assert.Contains("--assembly \"models.dll\"", command.Arguments);
        }

        private static string CreateTemporaryDirectory()
        {
            var directory = Path.Combine(Path.GetTempPath(), "vxform-vs-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(directory);
            return directory;
        }
    }
}
