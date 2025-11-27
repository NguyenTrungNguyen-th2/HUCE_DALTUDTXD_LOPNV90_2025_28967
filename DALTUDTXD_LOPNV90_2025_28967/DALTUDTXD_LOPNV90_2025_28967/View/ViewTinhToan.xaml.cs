using DALTUDTXD_LOPNV90_2025_28967.Model;
using DALTUDTXD_LOPNV90_2025_28967.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace DALTUDTXD_LOPNV90_2025_28967.View
{
    public partial class ViewTinhToan : Window
    {
        public ViewTinhToan(ObservableCollection<ColumnModel> danhSachCot, ColumnViewModel columnVM)
        {
            InitializeComponent();

            var danhSachKetQua = new ObservableCollection<TinhToanViewModel>();

            foreach (var cot in danhSachCot)
            {
                var vm = new TinhToanViewModel
                {
                    // --- Thông tin cột để hiển thị ---
                    Name = cot.Name,
                    Width = cot.Width,
                    Height = cot.Height,
                    Length = cot.Length,
                    LienKet = cot.LienKet,
                    Psi = cot.Psi,

                    // --- Thông số chung từ ViewThongSoCot ---
                    TaiTrong = columnVM.TaiTrong,
                    MomenX = columnVM.MomenX,
                    MomenY = columnVM.MomenY,
                    MacBeTong = columnVM.SelectedConcrete ?? "B20",
                    Rb = columnVM.VatLieuVM.Rb,
                    Eb = columnVM.VatLieuVM.Eb,
                    Rs = columnVM.VatLieuVM.Rs,
                    Es = columnVM.VatLieuVM.Es,

                    // --- Gán lại kích thước để tính ---
                    ChieuRong = cot.Width,
                    ChieuDai = cot.Height,
                    ChieuCao = cot.Length
                };

                // ✅ TÍNH TOÁN NGAY TẠI ĐÂY
                vm.TinhToan();

                danhSachKetQua.Add(vm);
            }

            // Gán DataContext để DataGrid binding
            DataContext = new { DanhSachCotHienThi = danhSachKetQua };
        }

        // Constructor design-time (giữ nguyên)
        public ViewTinhToan()
        {
#if DEBUG
            InitializeComponent();
            DataContext = new TinhToanViewModel();
#endif
        }
    }
}