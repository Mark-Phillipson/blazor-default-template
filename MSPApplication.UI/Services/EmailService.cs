using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSPApplication.Shared;
using SendGrid;
using System.Net;
using SendGrid.Helpers.Mail;

namespace MSPApplication.UI.Services
{
	public class EmailService : IEmailService
	{
		public async Task<Response> SendEmailAsync(Email email)
		{
			// https://app.sendgrid.com/settings/sender_auth/senders
			// Will need to either get the customer to create their own account or set up a new contact so emails don't come back to the developer
			//# Get Environment Variable
			var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");

			//# Set Environment Variable
			//Environment.SetEnvironmentVariable("SENDGRID_API_KEY", "YOUR_API_KEY");
			var client = new SendGridClient(apiKey);
			var from = new EmailAddress("MPhillipson0@Gmail.com", "Mark Phillipson");
			var subject = email.Subject;
			EmailAddress to;
			string plainTextContent;
			string htmlContent;
			SendGridMessage message;
			Response response = null;
			if (email.ToAddress.IndexOf(";") > 0)
			{
				var addresses = email.ToAddress.Split(";");
				foreach (var address in addresses)
				{
					to = new EmailAddress(address, address);
					plainTextContent = email.Body;
					htmlContent = email.Body;
					message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
					response = await client.SendEmailAsync(message).ConfigureAwait(false);
				}
			}
			else
			{
				to = new EmailAddress(email.ToAddress, email.ToAddress);
				plainTextContent = email.Body;
				htmlContent = email.Body;
				message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
				response = await client.SendEmailAsync(message).ConfigureAwait(false);
			}
			return response;
		}

	}
}
