using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading;
using Mailtrap.Model;
using NUnit.Framework;

namespace Mailtrap.Tests
{
    [TestFixture]
    public class MessageTests
    {
        private MailtrapClient mailtrapClient;
        private SmtpClient smtpClient;
        private string url = "https://mailtrap.io/api/v1";
        private string token = "my-token";

        [OneTimeSetUp]
        public void Setup()
        {
            mailtrapClient = new MailtrapClient(url, token);

            smtpClient = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("username", "password"),
                EnableSsl = true
            };

            smtpClient.Send("from@aubit.com", "to@aubit.com", "Dummy email", "Email for basic Mailtrap tests");
            smtpClient.Send("from@aubit.com", "to@aubit.com", "Dummy email", "Email for basic Mailtrap tests");
            Thread.Sleep(2000);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            var id = mailtrapClient.Inboxes().GetInboxes()[0].Id;
            mailtrapClient.Inboxes().EmptyInbox(id);
        }

        [Test]
        public void TestGetMessages()
        {
            var messages = mailtrapClient.Messages().GetMessages();

            if (messages.Count == 0)
                Assert.Inconclusive("No messages found in the inbox");

            Assert.That(messages, Is.TypeOf(typeof(List<Message>)));
        }

        [Test]
        public void TestGetMessage()
        {
            var messages = mailtrapClient.Messages().GetMessages();

            if (messages.Count == 0)
                Assert.Inconclusive("No messages found in the inbox");

            var id = messages[0].Id;

            var message = mailtrapClient.Messages().GetMessage(id);
            Assert.That(message, Is.TypeOf(typeof(Message)));
        }

        [Test]
        public void TestGetMessagesWithSearch()
        {
            var messages = mailtrapClient.Messages().GetMessages("Dummy");

            if (messages.Count == 0)
                Assert.Inconclusive("No messages found in the inbox");

            var id = messages[0].Id;

            var message = mailtrapClient.Messages().GetMessage(id);
            Assert.That(message, Is.TypeOf(typeof(Message)));
        }

        [Test]
        public void TestDeleteMessage()
        {
            var messages = mailtrapClient.Messages().GetMessages();

            if (messages.Count == 0)
                Assert.Inconclusive("No messages found in the inbox");

            var id = messages[0].Id;

            mailtrapClient.Messages().DeleteMessage(id);

            var message = mailtrapClient.Messages().GetMessage(id);
            Assert.That(message.Id, Is.Null);
        }
    }
}
