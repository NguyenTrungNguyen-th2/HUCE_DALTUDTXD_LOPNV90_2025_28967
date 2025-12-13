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

        // Constructor — nhận ColumnViewModel từ CmdCot
        public ViewThongSoCot(ColumnViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void btntinhtoan_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.DanhSachCotDaGanNoiLuc == null || _viewModel.DanhSachCotDaGanNoiLuc.Count == 0)
            {
                MessageBox.Show("Chưa có cột nào. Vui lòng nhấn 'Nhập từ Revit'.");
                return;
            }

            var material = SharedState.CurrentMaterial;
            if (material == null)
            {
                MessageBox.Show("Chưa chọn vật liệu. Vui lòng vào phần vật liệu và lưu.");
                return;
            }

            var ketQuaList = new System.Collections.ObjectModel.ObservableCollection<TinhToanViewModel>();

            foreach (var cot in _viewModel.DanhSachCotDaGanNoiLuc)
            {
                // Bỏ qua cột không có kích thước hợp lệ
                if (!double.TryParse(cot.Width, out double b) || b <= 0) continue;
                if (!double.TryParse(cot.Height, out double h) || h <= 0) continue;
                if (!double.TryParse(cot.Length, out double length)) length = 3000; // mặc định nếu cần

                // Tạo ViewModel tính toán cho từng cột
                var vm = new TinhToanViewModel
                {
                    Name = cot.Name,
                    Width = cot.Width,
                    Height = cot.Height,
                    Length = cot.Length,
                    LienKet = cot.LienKet,
                    Psi = cot.Psi,

                    // ✅ Lấy nội lực RIÊNG của từng cột
                    TaiTrong = cot.LoadValue,
                    MomenX = cot.MomentXValue,
                    MomenY = cot.MomentYValue,

                    // Vật liệu
                    MacBeTong = material.ConcreteGrade,
                    Rb = material.Rb,
                    Eb = material.Eb,
                    MacThep = material.SteelGrade,
                    Rs = material.Rs,
                    Es = material.Es,

                    // Gán các chiều (dùng trong TinhToan)
                    ChieuRong = b.ToString(),
                    ChieuDai = h.ToString(),   // Lưu ý: đây là chiều cao tiết diện h
                    ChieuCao = length.ToString()
                };

                // Thực hiện tính toán → tự động chọn thép
                vm.TinhToan();

                ketQuaList.Add(vm);
            }

            if (ketQuaList.Count == 0)
            {
                MessageBox.Show("Không có cột nào hợp lệ để tính toán.");
                return;
            }

            var viewTinhToan = new ViewTinhToan(ketQuaList)
            {
                Owner = this
            };
            viewTinhToan.ShowDialog();
        }

    }
}