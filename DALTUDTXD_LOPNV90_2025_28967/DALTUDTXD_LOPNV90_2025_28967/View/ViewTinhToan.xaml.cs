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
                    Name = cot.Name,
                    Width = cot.Width,
                    Height = cot.Height,
                    Length = cot.Length,
                    LienKet = cot.LienKet,
                    Psi = cot.Psi,

                    TaiTrong = columnVM.TaiTrong,
                    MomenX = columnVM.MomenX,
                    MomenY = columnVM.MomenY,

                    MacBeTong = material.ConcreteGrade,
                    Rb = material.Rb,
                    Eb = material.Eb,
                    MacThep = material.SteelGrade,
                    Rs = material.Rs,
                    Es = material.Es,

                   
                };

                if (double.TryParse(cot.Width, out double b) &&
                    double.TryParse(cot.Height, out double h) &&
                    double.TryParse(cot.Length, out double l))
                {
                    vm.ChieuRong = b.ToString();    
                    vm.ChieuDai = h.ToString();
                    vm.ChieuCao = l.ToString();

                   

                    vm.TinhToan(); 
                    danhSachKetQua.Add(vm);
                }
                else
                {
                   
                    continue;
                }
            }

            DataContext = new { DanhSachCotHienThi = danhSachKetQua };
        }

       
        public ViewTinhToan()
        {
#if DEBUG
            InitializeComponent();
            DataContext = new TinhToanViewModel();
#endif
        }
    }
}