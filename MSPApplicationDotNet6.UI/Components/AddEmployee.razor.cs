using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MSPApplication.Shared;
using MSPApplicationDotNet6.UI.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MSPApplicationDotNet6.UI.Components
{
	public partial class AddEmployee
	{
		[CascadingParameter] BlazoredModalInstance ModalInstance { get; set; }
		[Parameter] public ClaimsPrincipal User { get; set; }
		[Inject] public IToastService ToastService { get; set; }
		[Inject] public IJSRuntime JSRuntime { get; set; }
		public Employee Employee { get; set; } = new Employee { CountryId = 1, JobCategoryId = 1, BirthDate = DateTime.Now.AddYears(-19), JoinedDate = DateTime.Now };
		[Inject] public IEmployeeDataService EmployeeDataService { get; set; }
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await JSRuntime.InvokeVoidAsync("window.setFocus", "lastName");
			}
		}
	public void Close()
		{
			ModalInstance.CancelAsync();
		}

		protected async Task HandleValidSubmit()
		{
			await EmployeeDataService.AddEmployee(Employee);
			await ModalInstance.CloseAsync(ModalResult.Ok(true));
			ToastService.ShowSuccess($"{Employee.FullName} Added successfully.", "Success");
		}
	}
}
