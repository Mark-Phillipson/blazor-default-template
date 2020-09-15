using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MSPToDoList.Shared
{
    public partial class CopyToClipboard
    {
        [Parameter] public int Rows { get; set; }

        public string Text { get; set; }
        public string Result { get; set; }
        private async Task CopyTextToClipboard()
        {
            await JavascriptRuntime.InvokeVoidAsync(
                "clipboardCopy.copyText", Text);
            Result = $"Copied Successfully at {DateTime.Now.ToString("hh:mm")}";
        }
        private async Task ClearDictationAsync()
        {
            Text = null;
            Result = null;
            await JavascriptRuntime.InvokeVoidAsync("setFocus","DictationBox");
        }
    }
}