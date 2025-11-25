using System;
using System.Diagnostics;
using System.Windows.Automation;


namespace DALTUDTXD_LOPNV90_2025_28967.Helpers
{
    public static class RibbonSwitcher
    {
        public static void SwitchToPanel(string tabName, string panelName)
        {
            try
            {
                IntPtr revitHandle = Process.GetCurrentProcess().MainWindowHandle;
                if (revitHandle == IntPtr.Zero) return;

                AutomationElement main = AutomationElement.FromHandle(revitHandle);
                if (main == null) return;

                // Tìm Tab trong Ribbon
                var tab = main.FindFirst(TreeScope.Descendants,
                    new PropertyCondition(AutomationElement.NameProperty, tabName));

                if (tab == null) return;

                var tabSelect = tab.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
                tabSelect?.Select();

                // Tìm Panel
                var panel = main.FindFirst(TreeScope.Descendants,
                    new PropertyCondition(AutomationElement.NameProperty, panelName));

                if (panel == null) return;

                var panelSelect = panel.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
                panelSelect?.Select();
            }
            catch
            {
                // Không cần throw lỗi, tránh crash Revit
            }
        }
    }
}