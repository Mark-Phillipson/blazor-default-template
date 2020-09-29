using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSPToDoList.Services
{
    public static class FileUtility
    {
            public async static Task SaveAs(IJSRuntime js, string filename, byte[] data)
            {
                await js.InvokeAsync<object>(
                    "saveAsFile",
                    filename,
                    Convert.ToBase64String(data));
            }

    }
}
