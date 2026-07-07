using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.Shell;

namespace VxFormGenerator.VisualStudio
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class VxFormGeneratorPackage : AsyncPackage
    {
        public const string PackageGuidString = "6e721f2d-5238-4787-9306-a80cd406d592";

        protected override async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await ScaffoldFormCommand.InitializeAsync(this);
        }
    }
}
