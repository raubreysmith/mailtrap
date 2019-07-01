using Newtonsoft.Json;

namespace Mailtrap.Model
{
    public class Inbox
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("emails_count")]
        public int MessageCount { get; set; }
    }
}
