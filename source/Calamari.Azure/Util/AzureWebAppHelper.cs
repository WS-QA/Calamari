﻿using System;
using Calamari.Azure.Integration.Websites.Publishing;

namespace Calamari.Azure.Util
{
    public static class AzureWebAppHelper
    {
        public static AzureTargetSite GetAzureTargetSite(string siteAndMaybeSlotName, string slotName)
        {
            AzureTargetSite targetSite = new AzureTargetSite {RawSite = siteAndMaybeSlotName};

            if (siteAndMaybeSlotName.Contains("("))
            {
                // legacy site and slot "site(slot)"
                var parenthesesIndex = siteAndMaybeSlotName.IndexOf("(", StringComparison.Ordinal);
                targetSite.Site = siteAndMaybeSlotName.Substring(0, parenthesesIndex).Trim();
                targetSite.Slot = siteAndMaybeSlotName.Substring(parenthesesIndex + 1).Replace(")", string.Empty).Trim();
                return targetSite;
            }

            if (siteAndMaybeSlotName.Contains("/"))
            {
                // "site/slot"
                var slashIndex = siteAndMaybeSlotName.IndexOf("/", StringComparison.Ordinal);
                targetSite.Site = siteAndMaybeSlotName.Substring(0, slashIndex).Trim();
                targetSite.Slot = siteAndMaybeSlotName.Substring(slashIndex + 1).Trim();
                return targetSite;
            }

            targetSite.Site = siteAndMaybeSlotName;
            targetSite.Slot = slotName;
            return targetSite;
        }
    }
}