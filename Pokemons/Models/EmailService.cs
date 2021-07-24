using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;

namespace Pokemons.Models
{
    public class EmailService
    {
        public void SendEmail(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация интернет магазина Pokemons", "pokemonshoptest@yandex.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.yandex.ru", 465, true);
                client.Authenticate("pokemonshoptest@yandex.ru", "Q78perW4");
                client.Send(emailMessage);
                client.Disconnect(true);

            };
        }
    }
}
