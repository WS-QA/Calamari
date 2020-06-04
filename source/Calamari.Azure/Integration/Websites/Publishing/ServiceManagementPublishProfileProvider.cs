﻿using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Calamari.Azure.Accounts;
using Calamari.Commands.Support;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Management.WebSites;
using Microsoft.WindowsAzure.Management.WebSites.Models;

namespace Calamari.Azure.Integration.Websites.Publishing
{
    public class ServiceManagementPublishProfileProvider  
    {
        public static SitePublishProfile GetPublishProperties(AzureAccount account, AzureTargetSite targetSite)
        {
            Log.Verbose($"Service Management endpoint is {account.ServiceManagementEndpointBaseUri}");
            Log.Verbose($"Retrieving publishing profile for {targetSite.SiteAndSlotLegacy}");
            using (var cloudClient = new WebSiteManagementClient(
                new CertificateCloudCredentials(account.SubscriptionNumber, new X509Certificate2(account.CertificateBytes)),new Uri(account.ServiceManagementEndpointBaseUri)))
            {
                var webApp = cloudClient.WebSpaces.List()
                    .SelectMany( webSpace => cloudClient.WebSpaces.ListWebSites(webSpace.Name, new WebSiteListParameters()))
                    .FirstOrDefault(webSite => webSite.Name.Equals(targetSite.SiteAndSlotLegacy, StringComparison.OrdinalIgnoreCase));

                if (webApp == null)
                    throw new CommandException($"Could not find Azure WebSite '{targetSite.SiteAndSlotLegacy}' in subscription '{account.SubscriptionNumber}'");

                Log.Verbose("Retrieving publishing profile...");
                var publishProfile = cloudClient.WebSites.GetPublishProfile(webApp.WebSpace, targetSite.SiteAndSlotLegacy)
                    .PublishProfiles.First(x => x.PublishMethod.StartsWith("MSDeploy"));

                Log.Verbose($"Retrieved publishing profile: URI: {publishProfile.PublishUrl}  UserName: {publishProfile.UserName}");
                return new SitePublishProfile(publishProfile.UserName, publishProfile.UserPassword,
                    new Uri("https://" + publishProfile.PublishUrl));
            }
        }
    }
}