using System.ComponentModel;
using System.Diagnostics;

namespace Presentation.Helpers
{
    internal static class DesignModeHelper
    {
        public static bool IsDesignMode(Control? control = null)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return true;

            if (control?.Site?.DesignMode == true)
                return true;

            string processName = Process.GetCurrentProcess().ProcessName;
            return processName.Equals("devenv", StringComparison.OrdinalIgnoreCase)
                || processName.Contains("DesignToolsServer", StringComparison.OrdinalIgnoreCase)
                || processName.Contains("WinFormsDesigner", StringComparison.OrdinalIgnoreCase);
        }
    }
}
