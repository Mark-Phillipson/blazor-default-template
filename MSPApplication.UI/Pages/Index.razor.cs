using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MSPApplication.Shared;
using MSPApplication.UI.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MSPApplication.UI.Pages
{
	public partial class Index
	{
		public Concern NewConcern { get; set; } = new Concern();

		public bool EmailSent { get; set; } = false;

		[Inject] public IEmailService EmailService { get; set; }
		[Inject] public ITaskDataService TaskService { get; set; }
		[Inject] public IToastService ToastService { get; set; }
		[Inject] public NavigationManager NavigationManager { get; set; }
		public List<HRTask> Tasks { get; set; } = new List<HRTask>();
		[CascadingParameter]
		public ClaimsPrincipal User { get; set; }
		[CascadingParameter] public IModalService Modal { get; set; }
		public string Message { get; set; }
		protected override async Task OnInitializedAsync()
		{
			Tasks = (await TaskService.GetAllTasks()).OrderBy(o => o.Status).Take(3).ToList();
		}

		public void AddTask()
		{
			NavigationManager.NavigateTo("taskedit");
		}

		public async Task SubmitConcernAsync()
		{
			var newEmail = new Email()
			{
				Body = NewConcern.Description,
				Subject = NewConcern.Title
			};

			await EmailService.SendEmailAsync(newEmail);
			EmailSent = true;
		}

		public void ResetForm()
		{
			NewConcern = new Concern();
			EmailSent = false;
		}
		async Task TestEmailAsync()
		{
			Email email = new Email();
			email.Subject = "Testing Twilio send grid ";
			email.ToAddress = "mphillipson@btopenworld.com";
			email.Body = "Test Email Body This is a test email.";
			var response = await EmailService.SendEmailAsync(email);
			if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
			{
				Message = $"Email appears to have been sent correctly to {email.ToAddress}";
			}
			else
			{
				Message = $"Email failed to send with the following status code {response.StatusCode}";
			}
		}
		async Task TestTextEditorAsync()
		{
			var parameters = new ModalParameters();
			parameters.Add(nameof(User), User);
			var Subject = "Email Subject";
			parameters.Add(nameof(Subject), Subject);
			var ToAddress = "mphillipson@btopenworld.com";
			parameters.Add(nameof(ToAddress), ToAddress);
			var EditorContent = "<img width='161' height='166' src='~/images/DefaultLogo.png' alt='Company Logo' /><br><h3>This is a header</h3><p>This is a paragraph.</p>";
			parameters.Add(nameof(EditorContent), EditorContent);
			var formModal = Modal.Show<TextEditor>("Send Email", parameters);
			var result = await formModal.Result;
			if (!result.Cancelled)
			{
				Message = (string)result.Data;
			}

		}
	}
}
