using DALTUDTXD_LOPNV90_2025_28967.Model;
using DALTUDTXD_LOPNV90_2025_28967.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace DALTUDTXD_LOPNV90_2025_28967.View
{
    public partial class ViewTinhToan : Window
    {
        public ViewTinhToan(ObservableCollection<TinhToanViewModel> danhSachKetQua)
        {
            InitializeComponent();
            DataContext = new { DanhSachCotHienThi = danhSachKetQua };
        }

        // Constructor debug (nếu cần)
        public ViewTinhToan()
        {
#if DEBUG
            InitializeComponent();
            DataContext = new TinhToanViewModel();
#endif
        }
    }
}