using Main.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Main.Class
{
	public class EmailSender : IEmailSender
	{
		public Task SendEmailAsync(string email, string subject, string message)
		{

			var mail = "testesemail516@gmail.com";
			var pw = "rkwx efgm wbpt byev";

			var client = new SmtpClient("smtp.gmail.com", 587)
			{
				EnableSsl = true,
				Credentials = new NetworkCredential(mail, pw)
			};

			return client.SendMailAsync(
				new MailMessage(from: mail,
								to: email,
								subject,
								message)
				);

		}

	}
}
