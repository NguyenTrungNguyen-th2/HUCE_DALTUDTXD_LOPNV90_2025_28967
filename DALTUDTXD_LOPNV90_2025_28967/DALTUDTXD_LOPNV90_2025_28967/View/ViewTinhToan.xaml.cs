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

            // 🔑 LẤY VẬT LIỆU ĐÃ LƯU TỪ NƠI CHUNG
            var material = SharedState.CurrentMaterial;

            if (material == null)
            {
                MessageBox.Show("Chưa có thông số vật liệu! Vui lòng nhập và lưu vật liệu trước.", "Thiếu dữ liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            foreach (var cot in danhSachCot)
            {
                var vm = new TinhToanViewModel
                {
                    // --- Thông tin cột ---
                    Name = cot.Name,
                    Width = cot.Width,
                    Height = cot.Height,
                    Length = cot.Length,
                    LienKet = cot.LienKet,
                    Psi = cot.Psi,

                    // --- Tải trọng từ ColumnViewModel ---
                    TaiTrong = columnVM.TaiTrong,
                    MomenX = columnVM.MomenX,
                    MomenY = columnVM.MomenY,

                    // --- VẬT LIỆU TỪ SharedState (KHÔNG DÙNG columnVM.VatLieuVM) ---
                    MacBeTong = material.ConcreteGrade,
                    Rb = material.Rb,
                    Eb = material.Eb,
                    MacThep = material.SteelGrade,
                    Rs = material.Rs,
                    Es = material.Es,

                    // --- Kích thước để tính (phải là số thực, không phải chuỗi) ---
                    // ❗ CẢNH BÁO: Width, Height trong ColumnModel là string!
                    // → Cần parse sang double để tính toán
                };

                // ✅ Parse kích thước (vì trong ColumnModel là string)
                if (double.TryParse(cot.Width, out double b) &&
                    double.TryParse(cot.Height, out double h) &&
                    double.TryParse(cot.Length, out double l))
                {
                    vm.ChieuRong = b.ToString();    // Tạm giữ dạng string để hiển thị
                    vm.ChieuDai = h.ToString();
                    vm.ChieuCao = l.ToString();

                    // Gán lại giá trị số cho dịch vụ tính toán (nếu TinhToanViewModel cần)
                    // (Hiện tại TinhToanViewModel tự parse từ string → OK)

                    vm.TinhToan(); // ✅ Tính toán
                    danhSachKetQua.Add(vm);
                }
                else
                {
                    // Bỏ qua hoặc ghi log lỗi
                    continue;
                }
            }

            DataContext = new { DanhSachCotHienThi = danhSachKetQua };
        }

        // Constructor design-time
        public ViewTinhToan()
        {
#if DEBUG
            InitializeComponent();
            DataContext = new TinhToanViewModel();
#endif
        }
    }
}