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

namespace MSPToDoList.Pages
{
    public partial class TodosPage
    {
        private List<ToDoList> todos;
        [Inject]
        public ILocalStorageService LocalStorage { get; set; }

#pragma warning disable 414, 649,169
        private string message = "";
        private bool _loadFailed = false;
        ElementReference SearchInput;
#pragma warning restore 414, 649,169
        private string SearchTerm { get; set; }
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

            message = $"Saved!";// {DateTime.Now.TimeOfDay:hh:mm}";
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
            await JSRuntime.InvokeVoidAsync("setFocus", $"{toDoList.Id}TitleInput");
        }
        private void DeleteToDo(ToDoList toDoList)
        {
            todos.Remove(toDoList);
        }

        private async Task CallChangeAsync(string elementId)
        {
            //message = $" Call Change Now On element ID: {elementId}";
            await JSRuntime.InvokeVoidAsync("CallChange", elementId);
            if (elementId == "SearchInput")
            {
                ApplyFilter();
            }
        }
        private void ApplyFilter()
        {
            if (!string.IsNullOrEmpty(SearchTerm) && SearchTerm?.Length > 0)
            {
                todos = todos.Where(v => v.Title.ToLower().Contains(SearchTerm.Trim().ToLower())).ToList();
                title = $"Todos with {SearchTerm} Contained within the Title Found: {todos.Count}";
            }
            else
            {
                title = $"All Todos {todos.Count}";
            }
            message = $"AppliedFilter just ran! {DateTime.Now.TimeOfDay} Search Term: {SearchTerm}";
        }
        private async Task ReloadTodosAsync()
        {
            await LoadData();
        }

    }
}
