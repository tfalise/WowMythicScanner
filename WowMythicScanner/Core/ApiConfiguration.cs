using System;
using System.Collections.Generic;
using System.Text;

namespace WowMythicScanner
{
    class ApiConfiguration
    {
        public ApiRegion Region { get; set; }
        public ApiLocale Locale { get; set; }

        private static readonly Dictionary<ApiRegion, string> RegionUrlPrefixes = new Dictionary<ApiRegion, string>()
        {
            [ApiRegion.America] = "us",
            [ApiRegion.Europe] = "eu",
            [ApiRegion.Korea] = "kr",
            [ApiRegion.Taiwan] = "tw"
        };

        private static readonly Dictionary<ApiLocale, string> LocaleStrings = new Dictionary<ApiLocale, string>()
        {
            [ApiLocale.en_US] = "en_US",
            [ApiLocale.fr_FR] = "fr_FR"
        };

        public string GetRegionUrlPrefix() => RegionUrlPrefixes[Region];
        public string GetLocale() => LocaleStrings[Locale];
    }
}
