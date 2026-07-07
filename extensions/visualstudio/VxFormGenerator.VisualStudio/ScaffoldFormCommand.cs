using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VxFormGenerator.VisualStudio.Shared;
using Task = System.Threading.Tasks.Task;

namespace VxFormGenerator.VisualStudio
{
    internal sealed class ScaffoldFormCommand
    {
        private static readonly Guid CommandSet = new Guid("720d45e0-1647-4543-9c64-b35c509c8c31");
        private const int CommandId = 0x0100;
        private readonly AsyncPackage package;

        private ScaffoldFormCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package;
            var commandId = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(Execute, commandId);
            commandService.AddCommand(menuItem);
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                _ = new ScaffoldFormCommand(package, commandService);
            }
        }

        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var solutionDirectory = GetSolutionDirectory();
            if (string.IsNullOrWhiteSpace(solutionDirectory))
            {
                ShowMessage("Open a solution before scaffolding a VxFormGenerator form.");
                return;
            }

            var assemblyPath = PickAssembly(solutionDirectory);
            if (string.IsNullOrWhiteSpace(assemblyPath))
            {
                return;
            }

            var typeName = Interaction.InputBox("Full model type name", "VxFormGenerator Scaffold", "MyApp.Forms.CustomerFormModel");
            if (string.IsNullOrWhiteSpace(typeName))
            {
                return;
            }

            var outputPath = PickOutput(solutionDirectory, typeName);
            if (string.IsNullOrWhiteSpace(outputPath))
            {
                return;
            }

            RunScaffold(solutionDirectory, assemblyPath, typeName, outputPath);
        }

        private string GetSolutionDirectory()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var dte = (DTE)Package.GetGlobalService(typeof(DTE));
            var solutionPath = dte?.Solution?.FullName;
            return string.IsNullOrWhiteSpace(solutionPath) ? null : Path.GetDirectoryName(solutionPath);
        }

        private static string PickAssembly(string solutionDirectory)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Select compiled model assembly";
                dialog.InitialDirectory = solutionDirectory;
                dialog.Filter = "Assemblies (*.dll)|*.dll";
                return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
            }
        }

        private static string PickOutput(string solutionDirectory, string typeName)
        {
            using (var dialog = new SaveFileDialog())
            {
                var shortName = typeName.Substring(typeName.LastIndexOf('.') + 1).Replace("Model", string.Empty);
                dialog.Title = "Save generated Razor component";
                dialog.InitialDirectory = solutionDirectory;
                dialog.FileName = shortName + "Form.razor";
                dialog.Filter = "Razor components (*.razor)|*.razor";
                return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
            }
        }

        private void RunScaffold(string solutionDirectory, string assemblyPath, string typeName, string outputPath)
        {
            var command = ScaffoldCommandBuilder.Build(solutionDirectory, assemblyPath, typeName, outputPath);
            var startInfo = new ProcessStartInfo
            {
                FileName = command.FileName,
                WorkingDirectory = solutionDirectory,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = command.Arguments
            };

            var process = Process.Start(startInfo);
            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                ShowMessage("Generated " + outputPath);
                return;
            }

            ShowMessage(process.StandardError.ReadToEnd());
        }

        private void ShowMessage(string message)
        {
            VsShellUtilities.ShowMessageBox(
                package,
                message,
                "VxFormGenerator Scaffold",
                OLEMSGICON.OLEMSGICON_INFO,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }
    }
}
