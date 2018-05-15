﻿namespace VstsModuleManagementCore.Cmdlets
{
    using System.Collections.Generic;
    using System.Management.Automation;

    using VstsModuleManagementCore.Extensions;
    using VstsModuleManagementCore.Resources;
    using VstsModuleManagementCore.Utilities;

    [Cmdlet("Remove","VstsPackageSource")]
    public class RemoveVstsPackageSource : PSCmdlet
    {
        [Parameter]
        public string AccountName { get; set; }

        [Parameter]
        public string PackageRepositoryName { get; set; }

        protected override void ProcessRecord()
        {
            var providerName = $"{this.AccountName}-{this.PackageRepositoryName}-{CommonStrings.VstsProviderSufix}";

            var parameters = new Dictionary<string, object>
                                 {
                                     { "Name", providerName },
                                     { "Confirm", false }
                                 };

            PSUtils.InvokePSCommand("Unregister-PackageSource", parameters);

            var moduleSettings = this.GetModuleSettings();

            if (moduleSettings.KnownVstsProviders.ContainsKey(providerName))
            {
                moduleSettings.KnownVstsProviders.Remove(providerName);
                this.SaveModuleConfiguration(moduleSettings);
            }
            else
            {
                this.WriteWarning("A package source matching the input parameters was not found and was not removed from the known providers list.");
            }
        }
    }
}