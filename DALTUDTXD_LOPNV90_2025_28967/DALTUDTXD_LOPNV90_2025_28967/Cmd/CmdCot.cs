using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using DALTUDTXD_LOPNV90_2025_28967.View;
using DALTUDTXD_LOPNV90_2025_28967.ViewModel;
using System;

namespace DALTUDTXD_LOPNV90_2025_28967.Cmd
{
    [Transaction(TransactionMode.Manual)]
    public class CmdCot : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            if (uiDoc == null) return Result.Failed;

            if (SharedState.CurrentMaterial == null)
            {
                TaskDialog.Show("Cảnh báo", "Vui lòng nhập và lưu thông số vật liệu trước!");
                return Result.Cancelled;
            }
            var vatLieuVM = new VatLieuViewModel();
            vatLieuVM.LoadFromMaterial(SharedState.CurrentMaterial); 
            var tinhToanVM = new TinhToanViewModel();
            var columnVM = new ColumnViewModel(uiDoc, tinhToanVM);
            tinhToanVM.Rb = SharedState.CurrentMaterial.Rb;
            tinhToanVM.Eb = SharedState.CurrentMaterial.Eb;
            tinhToanVM.Rs = SharedState.CurrentMaterial.Rs;
            tinhToanVM.Es = SharedState.CurrentMaterial.Es;
            tinhToanVM.MacBeTong = SharedState.CurrentMaterial.ConcreteGrade;
            tinhToanVM.MacThep = SharedState.CurrentMaterial.SteelGrade;

            var view = new ViewThongSoCot(columnVM);
            view.ShowDialog();

            return Result.Succeeded;
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