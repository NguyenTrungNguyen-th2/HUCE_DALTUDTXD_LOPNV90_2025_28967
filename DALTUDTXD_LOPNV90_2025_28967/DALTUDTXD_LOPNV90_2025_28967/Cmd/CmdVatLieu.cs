using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DALTUDTXD_LOPNV90_2025_28967.View;
using DALTUDTXD_LOPNV90_2025_28967.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALTUDTXD_LOPNV90_2025_28967.Cmd
{
    [Transaction(TransactionMode.Manual)]
    public class CmdVatLieu : IExternalCommand
    {
        // CmdVatLieu.cs
        public Result Execute(ExternalCommandData data, ref string msg, ElementSet els)
        {
            RevitCommandData.UiApp = data.Application; // ← gán để dùng sau
            new ViewVatLieu().ShowDialog();
            // RevitCommandData.UiApp = null; // optional: clear sau khi đóng
            return Result.Succeeded;
        }
    }
}

