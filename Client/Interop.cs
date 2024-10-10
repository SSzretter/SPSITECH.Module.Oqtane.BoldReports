using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace SPSITECH.Module.BoldReports
{
    public class Interop
    {
        private readonly IJSRuntime _jsRuntime;

        public Interop(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }
    }
}
