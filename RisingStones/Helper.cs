using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RisingStones
{
    internal class Helper
    {
        public static string GetUserAppDataPath()
        {
            var assm = Assembly.GetEntryAssembly();

            var at = typeof(AssemblyCompanyAttribute);
            var r = assm?.GetCustomAttributes(at, false);
            AssemblyCompanyAttribute? ct = (AssemblyCompanyAttribute?)r?.FirstOrDefault();

            return Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.ApplicationData), ct?.Company ?? "RisingStones");
        }

        public static void OpenInDefaultBrowser(Uri uri)
        {
            var url = uri.ToString();
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
