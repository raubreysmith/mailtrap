using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading;
using Mailtrap.Model;
using NUnit.Framework;

namespace Mailtrap.Tests
{
    [TestFixture]
    public class InboxTests
    {
        private MailtrapClient mailtrapClient;
        private SmtpClient smtpClient;
        private string url = "https://mailtrap.io/api/v1";
        private string token = "my-token";
        private int inboxId = 305150;

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
            smtpClient.Send("from@aubit.com", "to@aubit.com", "Dummy email 2", "Email for basic Mailtrap tests");
            Thread.Sleep(2000);
        }

        [Test]
        public void TestGetInboxes()
        {
            var inboxes = mailtrapClient.Inboxes().GetInboxes();
            Assert.That(inboxes, Is.TypeOf(typeof(List<Inbox>)));
        }

        [Test]
        public void TestGetInbox()
        {
            var inbox = mailtrapClient.Inboxes().GetInbox(inboxId);
            Assert.That(inbox, Is.TypeOf(typeof(Inbox)));
        }

        [Test]
        public void TestEmptyInbox()
        {
            var inbox = mailtrapClient.Inboxes().GetInbox(inboxId);

            if(inbox.MessageCount == 0)
                Assert.Inconclusive("No messages found in the inbox");

            inbox = mailtrapClient.Inboxes().EmptyInbox(inboxId);
            Assert.That(inbox.Id, Is.Not.Null, "Failed to empty inbox");
            Assert.That(inbox.MessageCount, Is.EqualTo(0), "Failed to empty inbox");
        }
    }
}
