using System.Collections.Generic;
using Mailtrap.Model;
using Newtonsoft.Json;
using RestSharp;

namespace Mailtrap.Api
{
    public class MessageApi
    {
        private string resource;
        private int? inboxId;
        private MailtrapClient client;

        public MessageApi(MailtrapClient client, int? inboxId)
        {
            this.client = client;
            this.inboxId = inboxId;
            resource = $"inboxes/{inboxId}/messages";
        }

        /// <summary>
        /// Gets all messages in an inbox
        /// </summary>
        /// <returns>Returns a list of messages</returns>
        public IList<Message> GetMessages()
        {
            var request = new RestRequest(resource);
            var response = client.Execute(request);

            return JsonConvert.DeserializeObject<IList<Message>>(response.Content);
        }

        /// <summary>
        /// Gets all messages matching the search query.
        /// The query searches the from_email, to_email and subject properties for each message
        /// </summary>
        /// <param name="searchQuery">Substring to filter on</param>
        /// <returns>Returns a list of filter messages</returns>
        public IList<Message> GetMessages(string searchQuery)
        {
            var request = new RestRequest(resource);
            request.AddQueryParameter("search", searchQuery);
            var response = client.Execute(request);

            return JsonConvert.DeserializeObject<IList<Message>>(response.Content);
        }

        /// <summary>
        /// Gets a single message by Id
        /// </summary>
        /// <param name="messageId">Message identifier</param>
        /// <returns>Returns a single message</returns>
        public Message GetMessage(int? messageId)
        {
            var request = new RestRequest($"resource/{messageId}");
            var response = client.Execute(request);

            return JsonConvert.DeserializeObject<Message>(response.Content);
        }

        /// <summary>
        /// Deletes a message for the inbox
        /// </summary>
        /// <param name="messageId">Message identifier</param>
        public void DeleteMessage(int? messageId)
        {
            var request = new RestRequest($"resource/{messageId}", Method.DELETE);
            var response = client.Execute(request);
        }
    }
}