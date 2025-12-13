using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using DALTUDTXD_LOPNV90_2025_28967.Model;

namespace DALTUDTXD_LOPNV90_2025_28967.ViewModel
{
    public class ColumnViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private readonly UIDocument _uiDoc;
        private readonly Document _doc;
        public readonly TinhToanViewModel TinhToanVM;

        // ===== LỆNH (Commands) =====
        public ICommand cmNhapRevit { get; private set; }
        public ICommand cmThemNoiLuc { get; private set; } // 👈 ĐÃ KHAI BÁO

        public ColumnViewModel(UIDocument uiDoc, TinhToanViewModel tinhToanVM = null)
        {
            _uiDoc = uiDoc ?? throw new ArgumentNullException(nameof(uiDoc));
            _doc = _uiDoc.Document ?? throw new ArgumentException("Document is null");

            TinhToanVM = tinhToanVM ?? new TinhToanViewModel();

            DanhSachCot = new ObservableCollection<ColumnModel>();

            ConcreteGrades = new ObservableCollection<string> { "B15", "B20", "B25", "B30", "B35", "B40" };
            SelectedConcrete = ConcreteGrades.FirstOrDefault();

            DanhSachLienKet = new ObservableCollection<string>()
            {
                "Ngàm ngàm",
                "Ngàm khớp",
                "Khớp khớp",
                "Ngàm",
                "Khung cứng",
                "Khung khớp"
            };
            LienKetDangChon = DanhSachLienKet.First();
            HeSoPsi = HeSoTinhToan.Psi;

            // 👇 KHỞI TẠO CÁC LỆNH — BẠN THIẾU DÒNG THỨ 2 TRƯỚC ĐÂY!
            cmNhapRevit = new RelayCommand(_ => NhapTuRevit());
            cmThemNoiLuc = new RelayCommand(_ => ThemNoiLuc()); // ✅ ĐÃ THÊM!
            cmXoaCot = new RelayCommand(_ => XoaCot());
        }

        // ===== Các thuộc tính cho TextBox nhập liệu =====
        private double _taiTrong;
        public double TaiTrong
        {
            get => _taiTrong;
            set { _taiTrong = value; OnPropertyChanged(); }
        }

        private double _momenX;
        public double MomenX
        {
            get => _momenX;
            set { _momenX = value; OnPropertyChanged(); }
        }

        private double _momenY;
        public double MomenY
        {
            get => _momenY;
            set { _momenY = value; OnPropertyChanged(); }
        }

        private string _selectedConcrete;
        public string SelectedConcrete
        {
            get => _selectedConcrete;
            set { _selectedConcrete = value; OnPropertyChanged(); }
        }

        private string _macBeTong;
        public string MacBeTong
        {
            get => _macBeTong;
            set { _macBeTong = value; OnPropertyChanged(); }
        }

        public ObservableCollection<string> ConcreteGrades { get; }

        public ObservableCollection<ColumnModel> DanhSachCot { get; }

        private ColumnModel _selectedColumn;
        public ColumnModel SelectedColumn
        {
            get => _selectedColumn;
            set
            {
                if (_selectedColumn != value)
                {
                    _selectedColumn = value;
                    OnPropertyChanged();

                    if (_selectedColumn != null)
                    {
                        SelectColumnInRevit();
                        SyncToTinhToan();
                    }
                }
            }
        }
        // Danh sách cột từ Revit — chỉ để chọn trong ComboBox
        public ObservableCollection<ColumnModel> DanhSachCotTuRevit { get; }
            = new ObservableCollection<ColumnModel>();

        // Cột đang chọn trong ComboBox
        private ColumnModel _cotDangChon;
        public ColumnModel CotDangChon
        {
            get => _cotDangChon;
            set { _cotDangChon = value; OnPropertyChanged(); }
        }

        // Danh sách cột đã gán nội lực → hiển thị trong DataGrid
        public ObservableCollection<ColumnModel> DanhSachCotDaGanNoiLuc { get; }
            = new ObservableCollection<ColumnModel>();
        // Trong ColumnViewModel.cs
       
        public ObservableCollection<string> DanhSachLienKet { get; set; }

        private string _lienKetDangChon;
        public string LienKetDangChon
        {
            get => _lienKetDangChon;
            set
            {
                _lienKetDangChon = value;
                OnPropertyChanged();
                CapNhatHeSoPsi();
            }
        }

        private double _heSoPsi;
        public double HeSoPsi
        {
            get => _heSoPsi;
            set { _heSoPsi = value; OnPropertyChanged(); }
        }

        private void CapNhatHeSoPsi()
        {
            if (HeSoTinhToan.PsiTheoLienKet.ContainsKey(LienKetDangChon))
            {
                HeSoPsi = HeSoTinhToan.PsiTheoLienKet[LienKetDangChon];
                HeSoTinhToan.Psi = HeSoPsi;
            }
        }

        private void SelectColumnInRevit()
        {
            try
            {
                if (!int.TryParse(SelectedColumn?.Id, out int idInt)) return;

                ElementId id = new ElementId(idInt);
                Element ele = _doc.GetElement(id);
                if (ele == null) return;

                _uiDoc.Selection.SetElementIds(new[] { id });

                BoundingBoxXYZ bb = ele.get_BoundingBox(null);
                if (bb == null) return;

                var view = _uiDoc.GetOpenUIViews()
                    .FirstOrDefault(v => v.ViewId == _uiDoc.ActiveView.Id);

                if (view != null)
                {
                    double scale = 1.3;
                    XYZ c = (bb.Min + bb.Max) * 0.5;
                    XYZ half = (bb.Max - bb.Min) * 0.5 * scale;

                    view.ZoomAndCenterRectangle(c - half, c + half);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Zoom lỗi: {ex.Message}");
            }
        }

        public void SyncToTinhToan()
        {
            try
            {
                TinhToanVM.ChieuRong = SelectedColumn?.Width ?? "";
                TinhToanVM.ChieuDai = SelectedColumn?.Height ?? "";
                TinhToanVM.ChieuCao = SelectedColumn?.Length ?? "";

                TinhToanVM.TaiTrong = TaiTrong;
                TinhToanVM.MomenX = MomenX;
                TinhToanVM.MomenY = MomenY;

                var material = SharedState.CurrentMaterial;

                if (material != null)
                {
                    TinhToanVM.MacBeTong = material.ConcreteGrade ?? "B20";
                    TinhToanVM.Rb = material.Rb;
                    TinhToanVM.Eb = material.Eb;
                    TinhToanVM.MacThep = material.SteelGrade ?? "CII";
                    TinhToanVM.Rs = material.Rs;
                    TinhToanVM.Es = material.Es;
                }
                else
                {
                    TinhToanVM.MacBeTong = "B20";
                    TinhToanVM.Rb = 11.5;
                    TinhToanVM.Eb = 27000;
                    TinhToanVM.MacThep = "CII";
                    TinhToanVM.Rs = 280;
                    TinhToanVM.Es = 210000;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đồng bộ dữ liệu sang tính toán: {ex.Message}");
            }
        }

        // ===== PHƯƠNG THỨC XỬ LÝ "THÊM NỘI LỰC" =====
        private void ThemNoiLuc()
        {
            if (CotDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn một cột trong danh sách Revit.");
                return;
            }

            // Tạo bản sao mới (không ảnh hưởng dữ liệu gốc)
            var newCot = new ColumnModel
            {
                Id = CotDangChon.Id,
                Name = CotDangChon.Name,
                Width = CotDangChon.Width,
                Height = CotDangChon.Height,
                Length = CotDangChon.Length,
                Level = CotDangChon.Level,
                ConcreteGrade = CotDangChon.ConcreteGrade,

                // Gán nội lực và thông số từ UI
                LoadValue = TaiTrong,
                MomentXValue = MomenX,
                MomentYValue = MomenY,
                Psi = HeSoPsi,
                LienKet = LienKetDangChon,

                // Cập nhật hiển thị
                Load = TaiTrong != 0 ? TaiTrong.ToString("0.##") : "—",
                MomentX = MomenX != 0 ? MomenX.ToString("0.##") : "—",
                MomentY = MomenY != 0 ? MomenY.ToString("0.##") : "—"
            };

            DanhSachCotDaGanNoiLuc.Add(newCot);
            MessageBox.Show($"Đã thêm nội lực cho cột: {newCot.Name}");
        }
        // ===== NHẬP CỘT TỪ REVIT =====
        private void NhapTuRevit()
        {
            DanhSachCotTuRevit.Clear();
            var columns = new FilteredElementCollector(_doc)
                .OfCategory(BuiltInCategory.OST_StructuralColumns)
                .WhereElementIsNotElementType()
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>();

            int count = 0;
            foreach (var fi in columns)
            {
                var symbol = _doc.GetElement(fi.GetTypeId()) as FamilySymbol;
                var pb = symbol?.LookupParameter("b") ?? fi.LookupParameter("b");
                var ph = symbol?.LookupParameter("h") ?? fi.LookupParameter("h");
                if (pb == null || ph == null) continue;

                double b = pb.AsDouble() * 304.8;
                double h = ph.AsDouble() * 304.8;
                var bb = fi.get_BoundingBox(null);
                double len = bb != null ? Math.Abs(bb.Max.Z - bb.Min.Z) * 304.8 : 0;
                string level = _doc.GetElement(fi.LevelId)?.Name ?? "—";

                DanhSachCotTuRevit.Add(new ColumnModel
                {
                    Id = fi.Id.IntegerValue.ToString(),
                    Name = fi.Name,
                    Width = Math.Round(b).ToString(),
                    Height = Math.Round(h).ToString(),
                    Length = Math.Round(len).ToString(),
                    Level = level,
                    ConcreteGrade = MacBeTong ?? "—"
                    // KHÔNG gán Load, Moment, Psi, LienKet ở đây
                });
                count++;
            }
            MessageBox.Show($"Đã nhập {count} cột từ Revit.");
        }
        private ColumnModel _selectedCot;
        public ColumnModel SelectedCot
        {
            get => _selectedCot;
            set { _selectedCot = value; OnPropertyChanged(); }
        }
        public ICommand cmXoaCot { get; private set; }
        private void XoaCot()
        {
            if (SelectedCot == null)
            {
                MessageBox.Show("Vui lòng chọn một cột trong danh sách để xóa.");
                return;
            }

            string tenCot = SelectedCot.Name;
            if (MessageBox.Show($"Bạn có chắc muốn xóa cột '{tenCot}'?", "Xác nhận xóa",
                                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DanhSachCotDaGanNoiLuc.Remove(SelectedCot);
                SelectedCot = null; // Đặt lại selection
                MessageBox.Show($"Đã xóa cột: {tenCot}");
            }
        }
    }
}