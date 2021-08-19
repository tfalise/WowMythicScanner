using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WowMythicScanner.Core;
using WowMythicScanner.Wow;

namespace WowMythicScanner
{
    class WowApiReader
    {
        private readonly ApiReader apiReader;

        public WowApiReader(ApiReader apiReader)
        {
            this.apiReader = apiReader;
        }

        public async Task<List<AchievementCategory>> GetAchievementCategoriesAsync()
        {
            var content = await apiReader.RequestAsync("/data/wow/achievement-category/index", "static");
            
            JsonDocument jsonDocument = JsonDocument.Parse(content);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            return JsonSerializer.Deserialize<List<AchievementCategory>>(jsonDocument.RootElement.GetProperty("categories").GetRawText(), options);
        }

        public async Task<List<ConnectedRealm>> GetConnectedRealmsList()
        {
            var content = await apiReader.RequestAsync("/data/wow/connected-realm/index", "dynamic");
            JsonDocument jsonDocument = JsonDocument.Parse(content);

            var regex = new Regex(@"connected-realm/(\d+)");

            return regex.Matches(content).Select(match =>
            {
                var connectedRealmId = match.Groups[1].Value;
                var connectedRealmInfo = apiReader.RequestAsync(@$"/data/wow/connected-realm/{connectedRealmId}", "dynamic").Result;
                return JsonSerializer.Deserialize<ConnectedRealm>(connectedRealmInfo, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            ).ToList();
        }
    }
}
