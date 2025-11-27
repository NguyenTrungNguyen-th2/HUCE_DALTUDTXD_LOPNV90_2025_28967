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
        // ======== INPC ========
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // ======== Dependencies ========
        private readonly UIDocument _uiDoc;
        private readonly Document _doc;
        public readonly TinhToanViewModel TinhToanVM;
        public readonly VatLieuViewModel VatLieuVM;

        // ======== Constructor ========
        public ColumnViewModel(UIDocument uiDoc, TinhToanViewModel tinhToanVM = null)
        {
            _uiDoc = uiDoc ?? throw new ArgumentNullException(nameof(uiDoc));
            _doc = _uiDoc.Document ?? throw new ArgumentException("Document is null");

            TinhToanVM = tinhToanVM ?? new TinhToanViewModel();
            VatLieuVM = new VatLieuViewModel();

            // Collections
            DanhSachCot = new ObservableCollection<ColumnModel>();

            ConcreteGrades = new ObservableCollection<string> { "B15", "B20", "B25", "B30", "B35", "B40" };
            SelectedConcrete = ConcreteGrades.FirstOrDefault();

            // *** FIX PHẦN BẠN VIẾT SAI Ở DƯỚI ***
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

            // Commands
            cmNhapRevit = new RelayCommand(_ => NhapTuRevit());
            cmLuuCot = new RelayCommand(_ => LuuCotAll(), _ => DanhSachCot?.Count > 0);
        }

        // ======== PROPERTIES ========
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

        // ======== Danh sách cột ========
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

        // ======== COMMANDS ========
        public ICommand cmNhapRevit { get; }
        public ICommand cmLuuCot { get; }

        // ======== LIÊN KẾT – HỆ SỐ PSI ========
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
                HeSoTinhToan.Psi = HeSoPsi;     // cập nhật static
            }
        }

        // ======== METHODS ========

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
                TinhToanVM.ChieuRong = SelectedColumn.Width;
                TinhToanVM.ChieuDai = SelectedColumn.Height;
                TinhToanVM.ChieuCao = SelectedColumn.Length;

                TinhToanVM.TaiTrong = TaiTrong;
                TinhToanVM.MomenX = MomenX;
                TinhToanVM.MomenY = MomenY;

                TinhToanVM.MacBeTong = VatLieuVM.SelectedConcrete ?? SelectedConcrete ?? "B20";
                TinhToanVM.Rb = VatLieuVM.Rb;
                TinhToanVM.Eb = VatLieuVM.Eb;
                TinhToanVM.MacThep = VatLieuVM.SelectedSteel;
                TinhToanVM.Rs = VatLieuVM.Rs;
                TinhToanVM.Es = VatLieuVM.Es;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đồng bộ lỗi: {ex.Message}");
            }
        }

        private void NhapTuRevit()
        {
            try
            {
                DanhSachCot.Clear();

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

                    DanhSachCot.Add(new ColumnModel
                    {
                        Id = fi.Id.IntegerValue.ToString(),
                        Name = fi.Name,
                        Width = Math.Round(b).ToString(),
                        Height = Math.Round(h).ToString(),
                        Length = Math.Round(len).ToString(),
                        Level = level,
                        Load = TaiTrong != 0 ? TaiTrong.ToString("0.##") : "—",
                        MomentX = MomenX != 0 ? MomenX.ToString("0.##") : "—",
                        MomentY = MomenY != 0 ? MomenY.ToString("0.##") : "—",
                        ConcreteGrade = MacBeTong ?? "—",
                        LienKet = LienKetDangChon,
                        Psi = HeSoPsi
                    });

                    count++;
                }

                MessageBox.Show($"Đã nhập {count} cột từ Revit.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nhập lỗi: {ex.Message}");
            }
        }

        private void LuuCotAll()
        {
            try
            {
                foreach (var c in DanhSachCot)
                {
                    c.Load = TaiTrong != 0 ? TaiTrong.ToString("0.##") : "—";
                    c.MomentX = MomenX != 0 ? MomenX.ToString("0.##") : "—";
                    c.MomentY = MomenY != 0 ? MomenY.ToString("0.##") : "—";
                    c.ConcreteGrade = SelectedConcrete ?? "—";
                }

                if (SelectedColumn != null)
                    SyncToTinhToan();

                MessageBox.Show($"Đã lưu thông số cho {DanhSachCot.Count} cột.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu: {ex.Message}");
            }
        }

    }
}
