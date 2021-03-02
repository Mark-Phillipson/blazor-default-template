using MSPApplication.Shared;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSPApplication.UI.Services
{
	public interface IEmailService
	{
		Task<Response> SendEmailAsync(Email email);

	}
}
