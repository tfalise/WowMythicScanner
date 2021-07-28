using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
            var content = await apiReader.RequestAsync("https://eu.api.blizzard.com/data/wow/achievement-category/index");
            
            JsonDocument jsonDocument = JsonDocument.Parse(content);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            return JsonSerializer.Deserialize<List<AchievementCategory>>(jsonDocument.RootElement.GetProperty("categories").GetRawText(), options);
        }
    }
}
