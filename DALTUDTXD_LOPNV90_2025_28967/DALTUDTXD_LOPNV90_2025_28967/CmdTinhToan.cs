using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DALTUDTXD_LOPNV90_2025_28967.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALTUDTXD_LOPNV90_2025_28967
{
    [Transaction(TransactionMode.Manual)]
    public class CmdTinhToan : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var view = new ViewTinhToan(); // Bạn cần tạo view này
            view.ShowDialog();
            return Result.Succeeded;
        }
    }
}