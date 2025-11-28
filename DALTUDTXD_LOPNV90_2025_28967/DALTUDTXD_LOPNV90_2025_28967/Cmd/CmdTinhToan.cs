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
    public class CmdTinhToan : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            if (SharedState.MainVM?.ColumnVM?.SelectedColumn == null)
            {
                TaskDialog.Show("Lỗi", "Vui lòng chọn cột và tính toán trước.");
                return Result.Failed;
            }

            var viewModel = new TinhToanViewModel();

            // Copy dữ liệu từ cột đã chọn + vật liệu đã lưu
            var col = SharedState.MainVM.ColumnVM.SelectedColumn;
            viewModel.Name = col.Name;
            viewModel.Width = col.Width;
            viewModel.Height = col.Height;
            viewModel.Length = col.Length;
            viewModel.LienKet = col.LienKet;
            viewModel.Psi = col.Psi;

            // Dùng vật liệu đã lưu
            if (SharedState.CurrentMaterial != null)
            {
                viewModel.Rb = SharedState.CurrentMaterial.Rb;
                // ... tương tự cho các thuộc tính khác
            }

            viewModel.TaiTrong = SharedState.MainVM.ColumnVM.TaiTrong;
            viewModel.MomenX = SharedState.MainVM.ColumnVM.MomenX;
            viewModel.MomenY = SharedState.MainVM.ColumnVM.MomenY;

            viewModel.TinhToan(); // Tính luôn

            var view = new ViewTinhToan();
            view.DataContext = viewModel;
            view.ShowDialog();

            return Result.Succeeded;
        }
    }
}