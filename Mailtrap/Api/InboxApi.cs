using System.Collections.Generic;
using Mailtrap.Model;
using Newtonsoft.Json;
using RestSharp;

namespace Mailtrap.Api
{
    public class InboxApi
    {
        private string resource = "inboxes";
        private MailtrapClient client;
        private JsonSerializerSettings settings;

        public InboxApi(MailtrapClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// Gets all inboxes for the account
        /// </summary>
        /// <returns>Returns a list of inboxes</returns>
        public IList<Inbox> GetInboxes()
        {
            var request = new RestRequest(resource);
            var response = client.Execute(request);

            return JsonConvert.DeserializeObject<IList<Inbox>>(response.Content);
        }

        /// <summary>
        /// Get a single inbox by Id
        /// </summary>
        /// <param name="inboxId">Inbox identifier</param>
        /// <returns>Returns a single inbox</returns>
        public Inbox GetInbox(int? inboxId)
        {
            var request = new RestRequest($"{resource}/{inboxId}");
            var response = client.Execute(request);

            return JsonConvert.DeserializeObject<Inbox>(response.Content);
        }

        /// <summary>
        /// Deletes all messages in an inbox
        /// </summary>
        /// <param name="inboxId">Inbox identifier</param>
        /// <returns>Returns the inbox as a response</returns>
        public Inbox EmptyInbox(int? inboxId)
        {
            var request = new RestRequest($"{resource}/{inboxId}/clean", Method.PATCH);
            var response = client.Execute(request);

            return JsonConvert.DeserializeObject<Inbox>(response.Content);
        }
    }
}