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
        public Result Execute(ExternalCommandData data, ref string msg, ElementSet els)
        {
            RevitCommandData.UiApp = data.Application; 
            new ViewVatLieu().ShowDialog();
            return Result.Succeeded;
        }
    }
}

