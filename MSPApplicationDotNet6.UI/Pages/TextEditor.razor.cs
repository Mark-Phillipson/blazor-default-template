﻿using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.TextEditor;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using MSPApplication.Shared;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MSPApplicationDotNet6.UI.Services;
using MSPApplicationDotNet6.UI.Components;
using MSPApplicationDotNet6.UI.Shared;

namespace MSPApplicationDotNet6.UI.Pages
{
	public partial class TextEditor
	{
		[Inject] private IEmailService EmailService { get; set; }
		[Inject] IToastService ToastService { get; set; }
		[Parameter] public string EditorContent { get; set; } = "<h1>Header one</h1><h3>Header three</h3>";
		[Parameter] public string EditorContentHtmlSuffix { get; set; }
		[Parameter] public string Subject { get; set; } = "Email Subject";
		[Parameter] public string ToAddress { get; set; } = "mphillipson@btopenworld.com";
		[Parameter] public ClaimsPrincipal User { get; set; }
		[CascadingParameter] BlazoredModalInstance ModalInstance { get; set; }
		public string Message { get; set; }
		BlazoredTextEditor QuillHtml;
		string QuillHTMLContent;

		public async void GetHTML()
		{
			QuillHTMLContent = await this.QuillHtml.GetHTML();
			StateHasChanged();
		}

		public async void SetHTML()
		{
			string QuillContent =
				@"<a href='http://BlazorHelpWebsite.com/'>" +
				"<img src='images/DefaultLogo.png' /></a>";

			await this.QuillHtml.LoadHTMLContent(QuillContent);
			StateHasChanged();
		}
		async Task SendEmailAsync()
		{
			if (string.IsNullOrEmpty(ToAddress))
			{
				Message = "Please set a to address and try again ";
				return;
			}
			Email email = new Email();
			email.Subject = Subject;
			email.ToAddress = ToAddress;
			if (Environment.MachineName == "DESKTOP-UROO8T1" || User.Identity.Name.ToLower() == "mphillipson0@gmail.com")
			{
				email.ToAddress = "mphillipson@btopenworld.com";
			}
			if (!string.IsNullOrEmpty(EditorContent))
			{
				email.Body = await this.QuillHtml.GetHTML();
			}
			email.Body = $"{email.Body}<br>{EditorContentHtmlSuffix}";
			var response = await EmailService.SendEmailAsync(email);
			if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
			{
				ToastService.ShowSuccess($"Email appears to have been sent correctly to {email.ToAddress}", "SUCCESS");
				await ModalInstance.CloseAsync(ModalResult.Ok<string>(Message));
			}
			else
			{
				ToastService.ShowError($"Email failed to send with the following status code {response.StatusCode}", "ERROR");
			}

		}
	}
}