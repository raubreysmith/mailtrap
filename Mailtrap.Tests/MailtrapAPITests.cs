using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading;
using Mailtrap.Model;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;

namespace Mailtrap.Tests
{
    [TestFixture]
    public class MailtrapAPITests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            var smtpClient = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("username", "password"),
                EnableSsl = true
            };

            smtpClient.Send("from@aubit.com", "to@aubit.com", "Dummy email", "Email for basic Mailtrap tests");
            smtpClient.Send("from@aubit.com", "to@aubit.com", "Dummy email", "Email for basic Mailtrap tests");
            Thread.Sleep(2000);
        }

        [Test]
        public void MailTrap_GetInboxes()
        {
            var client = new RestClient("https://mailtrap.io/api/v1");
            client.AddDefaultHeader("Api-Token", "my-token");

            var request = new RestRequest("inboxes");
            var response = client.Execute(request);
            var inboxes =  JsonConvert.DeserializeObject<IList<Inbox>>(response.Content);

            Assert.That(inboxes.Count, Is.GreaterThan(0));
            Console.WriteLine(inboxes[0].Id);
        }

        [Test]
        public void Mailtrap_GetMessages()
        {
            var client = new RestClient("https://mailtrap.io/api/v1");
            client.AddDefaultHeader("Api-Token", "my-token");

            var request = new RestRequest("inboxes/305150/messages");
            var response = client.Execute(request);
            var messages = JsonConvert.DeserializeObject<IList<Message>>(response.Content);

            Assert.That(messages.Count, Is.GreaterThan(0));
            
            foreach (var message in messages)
            {
                Console.WriteLine($"{message.Id} {message.Subject} {message.SentAt}");
            }
        }

        [Test]
        public void Mailtrap_EmptyInbox()
        {
            var client = new RestClient("https://mailtrap.io/api/v1");
            client.AddDefaultHeader("Api-Token", "my-token");

            var request = new RestRequest("inboxes/305150/clean", Method.PATCH);
            var response = client.Execute(request);
            var inbox = JsonConvert.DeserializeObject<Inbox>(response.Content);

            Assert.That(inbox.MessageCount, Is.EqualTo(0));
        }
    }
}
