using Microsoft.Web.WebView2.Core;

using System.Collections.Generic;
using System.Text;

namespace RisingStones
{
    internal partial class DevToolsProtocolHelper
    {
        private readonly CoreWebView2 coreWebView2;

        public EmulationHelper Emulation { get; }
        public InputHelper Input { get; }

        public DevToolsProtocolHelper(CoreWebView2 coreWebView2)
        {
            this.coreWebView2 = coreWebView2;
            Emulation = new EmulationHelper(coreWebView2);
            Input = new InputHelper(coreWebView2);
        }
    }

    internal static class DevToolsProtocolHelperExtensions
    {
        public static DevToolsProtocolHelper GetDevToolsProtocolHelper(this CoreWebView2 coreWebView2)
        {
            return new DevToolsProtocolHelper(coreWebView2);
        }
    }
}
