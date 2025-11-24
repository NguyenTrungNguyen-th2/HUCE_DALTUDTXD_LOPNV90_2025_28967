using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using JetBrains.Annotations;
using MaterialDesignThemes.Wpf;
using Nice3point.Revit.Toolkit.External;

namespace DALTUDTXD_LOPNV90_2025_28967
{
    [Transaction(TransactionMode.Manual)]
    public class Cmd : IExternalCommand
    {

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            var view = new MainWindow();
            view.ShowDialog();
            return Result.Succeeded;
        }
    }
}

