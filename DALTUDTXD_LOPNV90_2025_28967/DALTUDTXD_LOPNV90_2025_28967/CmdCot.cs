using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DALTUDTXD_LOPNV90_2025_28967.View;
using DALTUDTXD_LOPNV90_2025_28967.ViewModel;
using System;

namespace DALTUDTXD_LOPNV90_2025_28967
{
    // CmdCot.cs
    [Transaction(TransactionMode.Manual)]
    public class CmdCot : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            if (uiDoc == null || uiDoc.Document == null)
            {
                TaskDialog.Show("Lỗi", "Vui lòng mở file Revit trước.");
                return Result.Failed;
            }

            try
            {
                // ✅ DUY NHẤT Ở ĐÂY: truyền uiDoc
                var tinhToanVM = new TinhToanViewModel();
                var columnVM = new ColumnViewModel(uiDoc, tinhToanVM);

                // → Mở ViewThongSoCot với ViewModel đã có uiDoc
                var view = new ViewThongSoCot(columnVM);
                view.ShowDialog();

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Lỗi", ex.Message);
                return Result.Failed;
            }
        }
        public static void OpenCotView(UIApplication uiApp)
        {
            var uiDoc = uiApp.ActiveUIDocument;
            if (uiDoc == null || uiDoc.Document == null)
            {
                TaskDialog.Show("Lỗi", "Không có file Revit đang mở.");
                return;
            }

            var vm = new ColumnViewModel(uiDoc);
            var view = new ViewThongSoCot(vm);
            view.ShowDialog();
        }
    }
}