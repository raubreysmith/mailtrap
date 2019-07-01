using System;
using System.Diagnostics;
using Mailtrap.Api;
using RestSharp;

namespace Mailtrap
{
    public class MailtrapClient
    {
        private RestClient client;

        /// <summary>
        /// REST client for the Mailtrap API service
        /// </summary>
        /// <param name="baseUrl">Url of the service e.g. https://mailtrap.io/api/v1</param>
        /// <param name="apiToken">API token from your Mailtrap account</param>
        public MailtrapClient(string baseUrl, string apiToken)
        {
            client = new RestClient(baseUrl);
            client.AddDefaultHeader("Api-Token", apiToken);
        }

        /// <summary>
        /// Executes the REST request, throwing an exception for any response errors
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        internal IRestResponse Execute(RestRequest request)
        {
            var response = client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response.  Check inner details for more info.";
                throw new ApplicationException(message, response.ErrorException);
            }

            Debug.WriteLine($"{(int)response.StatusCode}: {response.StatusDescription}");
            Debug.WriteLine(response.Content);

            return response;
        }

        /// <summary>
        /// Inbox operations
        /// </summary>
        /// <returns>Inboxes API</returns>
        public InboxApi Inboxes()
        {
            return new InboxApi(this);
        }

        /// <summary>
        /// Message operations, defaults to the first available inbox for the account
        /// </summary>
        /// <returns>Messages API</returns>
        public MessageApi Messages()
        {
            var inbox = new InboxApi(this);
            var id = inbox.GetInboxes()[0].Id;

            return Messages(id);
        }

        /// <summary>
        /// Message operations for a specified inbox
        /// </summary>
        /// <param name="inboxId">Inbox identifier</param>
        /// <returns>Messages API</returns>
        public MessageApi Messages(int? inboxId)
        {
            return new MessageApi(this, inboxId);
        }
    }
}
