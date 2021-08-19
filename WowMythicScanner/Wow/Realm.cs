using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WowMythicScanner.Wow
{
    class Realm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Timezone { get; set; }
        public string Category { get; set; }
        public string locale { get; set; }
        public string Slug { get; set; }
        [JsonPropertyName("is_tournament")]
        public bool IsTournament { get; set; }
    }
}
