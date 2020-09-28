using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using Microsoft.JSInterop;
using MSPToDoList.Pages;
using MSPToDoList.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Blazored.LocalStorage.StorageOptions;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace MSPToDoList.Shared
{
    public partial class CopyToClipboard
    {
        [Inject]
        public ILocalStorageService LocalStorage { get; set; }

        [Parameter] public int Rows { get; set; }
        private List<ToDoList> todos;
        public string Text { get; set; }
        public string Result { get; set; }
        private async Task CopyTextToClipboard()
        {
            await JavascriptRuntime.InvokeVoidAsync(
                "clipboardCopy.copyText", Text);
            Result = $"Copied Successfully at {DateTime.Now:hh:mm}";
        }
        private async Task ClearDictationAsync()
        {
            Text = null;
            Result = null;
            await JavascriptRuntime.InvokeVoidAsync("setFocus","DictationBox");
        }

        private async Task LoadData()
        {
            todos = await LocalStorage.GetItemAsync<List<ToDoList>>("todo");
            if (todos == null || todos.Count == 0)
            {
                todos = await Http.GetFromJsonAsync<List<ToDoList>>("sample-data/todo.json");
            }
        }

        private async Task AddToDoAsync()
        {
            if (Text== null  || Text.Length<1)
            {
                return;
            }
            await LoadData();
            var titleLength = Text.Length;
            if (titleLength>30)
            {
                titleLength = 30;
            }
            ToDoList toDoList = new ToDoList { DateCreated = DateTime.Now.Date,Title=$"{Text.Substring(0,titleLength).ToUpper()}..",Description=Text,Completed=false };
            todos.Add(toDoList);
            await LocalStorage.SetItemAsync<List<ToDoList>>("todo", todos);
        }

    }
}