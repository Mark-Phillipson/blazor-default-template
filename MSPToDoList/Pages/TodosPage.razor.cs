using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using MSPToDoList.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MSPToDoList.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Components.Forms;

namespace MSPToDoList.Pages
{
	public partial class TodosPage
	{
		private List<ToDoList> todos;
		[Inject]
		public ILocalStorageService LocalStorage { get; set; }

#pragma warning disable 414, 649, 169
		private string message = "";
		private bool _loadFailed = false;
		ElementReference SearchInput;
#pragma warning restore 414, 649, 169
		private string SearchTerm { get; set; }
		public bool ShowCompleted { get; set; } = true;
		protected override async Task OnInitializedAsync()
		{
			await LoadData();
		}

		private async Task LoadData()
		{
			todos = await LocalStorage.GetItemAsync<List<ToDoList>>("todo");
			if (todos == null || todos.Count == 0)
			{
				todos = await Http.GetFromJsonAsync<List<ToDoList>>("sample-data/todo.json");
			}
		}

		protected async Task SaveToDoAsync()
		{
			await LocalStorage.SetItemAsync<List<ToDoList>>("todo", todos);
			//message = $"Saved! {DateTime.Now.TimeOfDay.ToString("hh:nn")}";
			toastService.ShowSuccess("All to dos have been saved successfully!");
		}

		private string title { get; set; } = "Todo List";
		//usersJson = LoadJson(@"services\users.json");
		//users = JsonConvert.DeserializeObject<List<AspNetUser>>(usersJson);

		private string LoadJson(string fileName)
		{
			using (StreamReader reader = new StreamReader(fileName))
			{
				string json = reader.ReadToEnd();
				return json;
			}
		}
		private async Task AddToDoAsync()
		{
			ToDoList toDoList = new ToDoList { DateCreated = DateTime.Now.Date };
			todos.Add(toDoList);
			await JSRuntime.InvokeVoidAsync("setFocus", $"{toDoList.Id}TitleInputBig");
		}
		private void DeleteToDo(ToDoList toDoList)
		{
			todos.Remove(toDoList);
		}

		private async Task CallChangeAsync(string elementId)
		{
			await JSRuntime.InvokeVoidAsync("CallChange", elementId);
		}
		private async Task ReloadTodosAsync()
		{
			await LoadData();
		}
		async Task DownloadFileAsync()
		{
			//var text = todos.ToList().ToString();
			var text = JsonConvert.SerializeObject(todos.ToList());
			var bytes = System.Text.Encoding.UTF8.GetBytes(text);
			await FileUtility.SaveAs(JSRuntime, "todo.json", bytes);

		}
		private IList<string> files = new List<string>();

		async Task OnInputFileChange(InputFileChangeEventArgs e)
		{
			var  files = e.GetMultipleFiles(1);
			var file=files.FirstOrDefault();
			List<ToDoList> todosImported;
			byte[] result;
			using (var reader = file.OpenReadStream())
			{
				try
				{
					result= new byte[reader.Length];
					await reader.ReadAsync(result,0,( int )reader.Length);
					var text=System.Text.Encoding.ASCII.GetString(result);
					todosImported = JsonConvert.DeserializeObject<List<ToDoList>>(text);
					if (todosImported.Count > 0)
					{
						foreach (var todo in todosImported)
						{
							todos.Add(todo);
						}
					}
				}
				catch (Exception exception)
				{
					message = exception.Message;
				}
			}
		}
	}
}