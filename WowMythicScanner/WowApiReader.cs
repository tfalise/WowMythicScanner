using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WowMythicScanner.Core;

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

        public async Task<List<string>> GetConnectedRealmsList()
        {
            var content = await apiReader.RequestAsync("/data/wow/connected-realm/index", "dynamic");
            JsonDocument jsonDocument = JsonDocument.Parse(content);

            var realmsList = JsonSerializer.Deserialize<List<HrefItem>>(
                jsonDocument.RootElement.GetProperty("connected_realms").GetRawText(), 
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return realmsList.Select(item => item.Href).ToList();
        }
    }
}
