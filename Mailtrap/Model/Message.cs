using System;
using Newtonsoft.Json;

namespace Mailtrap.Model
{
    public class Message
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("sent_at")]
        public DateTime SentAt { get; set; }

        [JsonProperty("from_email")]
        public string FromEmail { get; set; }

        [JsonProperty("to_email")]
        public string ToEmail { get; set; }

        [JsonProperty("text_body")]
        public string BodyText { get; set; }

        [JsonProperty("html_body")]
        public string BodyHtml { get; set; }
    }
}
