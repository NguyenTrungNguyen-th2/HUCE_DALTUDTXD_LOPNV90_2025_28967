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

        // Constructor — nhận ColumnViewModel từ CmdCot
        public ViewThongSoCot(ColumnViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void btntinhtoan_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.DanhSachCot == null || _viewModel.DanhSachCot.Count == 0)
            {
                MessageBox.Show("Chưa có cột nào. Vui lòng nhấn 'Nhập từ Revit'.");
                return;
            }

            var viewTinhToan = new ViewTinhToan(_viewModel.DanhSachCot, _viewModel)
            {
                Owner = this
            };
            viewTinhToan.ShowDialog();
        }
    }
}