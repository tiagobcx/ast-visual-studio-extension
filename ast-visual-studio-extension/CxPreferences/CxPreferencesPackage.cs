﻿using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace ast_visual_studio_extension.CxPreferences
{
    /**
     * Checkmarx Preferences page main class
     */
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(CxPreferencesPackage.PackageGuidString)]
    [ProvideOptionPage(typeof(CxPreferencesModule), "Checkmarx", "Checkmarx settings", 0, 0, true)]
    public sealed class CxPreferencesPackage : AsyncPackage
    {
        public const string PackageGuidString = "e2527bed-dc52-4188-9e62-c8037a3fc796";

        public CxPreferencesPackage(){ }

        #region Package Members

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
        }

        #endregion
    }
}
