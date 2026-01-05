using DALTUDTXD_LOPNV90_2025_28967.Model;
using DALTUDTXD_LOPNV90_2025_28967.Services;
using DALTUDTXD_LOPNV90_2025_28967.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DALTUDTXD_LOPNV90_2025_28967.View
{
    public partial class ViewThongSoCot : Window
    {
        private readonly ColumnViewModel _viewModel;
        public ViewThongSoCot(ColumnViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }
        private void btntinhtoan_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = DataGridCot.SelectedItems.Cast<ColumnModel>().ToList();

            if (selectedItems.Count == 0)
            {
                if (_viewModel.DanhSachCotDaGanNoiLuc == null || _viewModel.DanhSachCotDaGanNoiLuc.Count == 0)
                {
                    MessageBox.Show("Chưa có cột nào. Vui lòng nhấn 'Nhập từ Revit'.");
                    return;
                }
                selectedItems = _viewModel.DanhSachCotDaGanNoiLuc.ToList();
            }

            var material = SharedState.CurrentMaterial;
            if (material == null)
            {
                MessageBox.Show("Chưa chọn vật liệu. Vui lòng vào phần vật liệu và lưu.");
                return;
            }

            var ketQuaList = new System.Collections.ObjectModel.ObservableCollection<TinhToanViewModel>();

            foreach (var cot in selectedItems)
            {
                if (!double.TryParse(cot.Width, out double b) || b <= 0) continue;
                if (!double.TryParse(cot.Height, out double h) || h <= 0) continue;
                if (!double.TryParse(cot.Length, out double length)) length = 3000;

                var vm = new TinhToanViewModel
                {
                    DisplayName = cot.DisplayName,
                    RevitId = cot.Id,
                    Width = cot.Width,
                    Level = cot.Level,
                    Height = cot.Height,
                    Length = cot.Length,
                    LienKet = cot.LienKet,
                    Psi = cot.Psi,
                    TaiTrong = cot.LoadValue,
                    MomenX = cot.MomentXValue,
                    MomenY = cot.MomentYValue,
                    MacBeTong = material.ConcreteGrade,
                    Rb = material.Rb,
                    Eb = material.Eb,
                    MacThep = material.SteelGrade,
                    Rs = material.Rs,
                    Es = material.Es,
                    ChieuRong = b.ToString(),
                    ChieuDai = h.ToString(),
                    ChieuCao = length.ToString()
                };


                vm.TinhToan();
                ketQuaList.Add(vm);
            }

            if (ketQuaList.Count == 0)
            {
                MessageBox.Show("Không có cột nào hợp lệ để tính toán.");
                return;
            }

            var viewTinhToan = new ViewTinhToan(ketQuaList, _viewModel.UIDoc)
            {
                Owner = this
            };
            viewTinhToan.ShowDialog();
        }
        private void btnXoaCot_Click(object sender, RoutedEventArgs e)
        {
            var dataGrid = this.FindName("DataGridCot") as DataGrid;
            if (dataGrid == null || dataGrid.SelectedItems.Count == 0) return;

            var selectedItems = dataGrid.SelectedItems.Cast<ColumnModel>().ToList();

            _viewModel.XoaNhieuCot(selectedItems);
        }
    }
}