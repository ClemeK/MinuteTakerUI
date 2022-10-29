using FluentEmail.Core;
using FluentEmail.Smtp;
using System.Collections.Generic;
using System.Net.Mail;

namespace MinuteTaker
{
    public static class FluentEmailLogic
    {
        public static void SendEmailToGroupWithAttachment(SmtpsModel s, List<PersonModel> toAddress, string subject, string body, string file)
        {
            foreach (var a in toAddress)
            {
                SendEmailWithAttachment(s, a.EmailAddress, a.FirstName, subject, body, file);
            }
        }

        public static void SendEmailWithAttachment(SmtpsModel s, string toAddress, string toName, string subject, string body, string file)
        {
            bool Ssl = false;

            if (s.Ssl == "SSL")
            {
                Ssl = true;
            }

            var sender = new SmtpSender(() => new SmtpClient(s.Url)
            {
                EnableSsl = Ssl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Port = s.Port
                //DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                //PickupDirectoryLocation = @".\Docs"
            });

            Email.DefaultSender = sender;

            string senderEmail = MinuteTakerLibary.AppKeyLookup("senderEmail");
            string senderName = MinuteTakerLibary.AppKeyLookup("senderName");

            var email = Email
                .From(senderEmail, senderName)
                .To(toAddress, toName)
                .Subject(subject)
                .Body(body)
                .AttachFromFilename(file)
                .Send();
        }

        // **********************************************
        // **********************************************

        public static void SendEmailToGroup(SmtpsModel s, List<PersonModel> toAddress, string subject, string body)
        {
            foreach (var a in toAddress)
            {
                SendEmail(s, a.EmailAddress, a.FirstName, subject, body);
            }
        }

        public static void SendEmail(SmtpsModel s, string toAddress, string toName, string subject, string body)
        {
            bool Ssl = false;

            if (s.Ssl != "SSL")
            {
                Ssl = true;
            }

            var sender = new SmtpSender(() => new SmtpClient(s.Url)
            {
                EnableSsl = Ssl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Port = s.Port
                //DeliveryMethod =SmtpDeliveryMethod.SpecifiedPickupDirectory,
                //PickupDirectoryLocation=@".\Docs"
            });

            Email.DefaultSender = sender;

            string senderEmail = MinuteTakerLibary.AppKeyLookup("senderEmail");
            string senderName = MinuteTakerLibary.AppKeyLookup("senderName");

            var email = Email
                .From(senderEmail, senderName)
                .To(toAddress, toName)
                .Subject(subject)
                .Body(body)
                .Send();
        }
    }
}