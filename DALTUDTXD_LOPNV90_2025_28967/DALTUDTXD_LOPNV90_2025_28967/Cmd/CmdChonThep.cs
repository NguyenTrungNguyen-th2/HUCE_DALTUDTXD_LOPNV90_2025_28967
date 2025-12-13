using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DALTUDTXD_LOPNV90_2025_28967.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using DALTUDTXD_LOPNV90_2025_28967.ViewModel;

namespace DALTUDTXD_LOPNV90_2025_28967.Cmd
{
    [Transaction(TransactionMode.Manual)]
    public class CmdChonThep:IExternalCommand
    {
       
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            var viewchonthep = new ViewVeThep();
            var vm = new ColumnRebarViewModel(doc, uidoc);
            viewchonthep.DataContext = vm;
            viewchonthep.ShowDialog();


            return Result.Succeeded;
        }
    }
}
