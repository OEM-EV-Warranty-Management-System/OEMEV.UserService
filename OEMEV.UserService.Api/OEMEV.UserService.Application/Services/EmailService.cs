using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using OEMEV.UserService.Application.Interfaces;
using OEMEV.UserService.Infrastructure.Base;

namespace OEMEV.UserService.Application.Services
{
	public class EmailService : IEmailService
	{
		private readonly EmailSettings _emailSettings;

		public EmailService(IOptions<EmailSettings> emailSettings)
		{
			_emailSettings = emailSettings.Value;
		}

		public async Task SendEmailAsync(string toEmail, string subject, string message)
		{
			var email = new MimeMessage();
			email.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.From));
			email.To.Add(MailboxAddress.Parse(toEmail));
			email.Subject = subject;

			var builder = new BodyBuilder
			{
				HtmlBody = message
			};
			email.Body = builder.ToMessageBody();

			using var smtp = new SmtpClient();
			await smtp.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
			await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
			await smtp.SendAsync(email);
			await smtp.DisconnectAsync(true);
		}
	}
}