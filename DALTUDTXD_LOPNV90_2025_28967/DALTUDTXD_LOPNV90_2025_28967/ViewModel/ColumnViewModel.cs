// ColumnViewModel.cs
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
using DALTUDTXD_LOPNV90_2025_28967.ViewModel;

namespace DALTUDTXD_LOPNV90_2025_28967.ViewModel
{
    public class ColumnViewModel : INotifyPropertyChanged
    {
        // ======== INPC ========
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // ======== Dependencies (BẮT BUỘC) ========
        private readonly UIDocument _uiDoc;
        private readonly Document _doc;
        public readonly TinhToanViewModel TinhToanVM;
        public readonly VatLieuViewModel VatLieuVM;

        // ======== Constructor DUY NHẤT — gọi từ CmdCot ========
        public ColumnViewModel(UIDocument uiDoc, TinhToanViewModel tinhToanVM = null)
        {
            _uiDoc = uiDoc ?? throw new ArgumentNullException(nameof(uiDoc));
            _doc = _uiDoc.Document ?? throw new ArgumentException("Document is null", nameof(uiDoc));

            TinhToanVM = tinhToanVM ?? new TinhToanViewModel();
            VatLieuVM = new VatLieuViewModel();

            // Khởi tạo collections
            DanhSachCot = new ObservableCollection<ColumnModel>();
            ConcreteGrades = new ObservableCollection<string> { "B15", "B20", "B25", "B30", "B35", "B40" };
            SelectedConcrete = ConcreteGrades.FirstOrDefault();

            // Commands
            cmNhapRevit = new RelayCommand(_ => NhapTuRevit(), _ => true);
            cmLuuCot = new RelayCommand(_ => LuuCotAll(), _ => DanhSachCot?.Count > 0);
        }

        // ======== PROPERTIES NHẬP TAY ========
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

        // ======== DANH SÁCH CỘT & CHỌN ========
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

        // ======== PRIVATE METHODS ========

        private void SelectColumnInRevit()
        {
            try
            {
                if (!int.TryParse(SelectedColumn?.Id, out int idInt)) return;
                ElementId id = new ElementId(idInt);
                Element ele = _doc.GetElement(id);
                if (ele == null) return;

                // Chọn cột
                _uiDoc.Selection.SetElementIds(new[] { id });

                // Zoom
                BoundingBoxXYZ bb = ele.get_BoundingBox(null);
                if (bb == null) return;

                double scale = 1.3;
                XYZ center = (bb.Min + bb.Max) / 2;
                XYZ half = (bb.Max - bb.Min) / 2 * scale;

                UIView activeView = _uiDoc.GetOpenUIViews()
                    .FirstOrDefault(v => v.ViewId == _uiDoc.ActiveView.Id);
                activeView?.ZoomAndCenterRectangle(center - half, center + half);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Zoom lỗi: {ex.Message}", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SyncToTinhToan()
        {
            try
            {
                // Đồng bộ kích thước
                TinhToanVM.ChieuRong = SelectedColumn.Width;
                TinhToanVM.ChieuDai = SelectedColumn.Height;
                TinhToanVM.ChieuCao = SelectedColumn.Length;

                // Đồng bộ tải trọng
                TinhToanVM.TaiTrong = TaiTrong;
                TinhToanVM.MomenX = MomenX;
                TinhToanVM.MomenY = MomenY;

                // Đồng bộ vật liệu
                TinhToanVM.MacBeTong = VatLieuVM.SelectedConcrete ?? SelectedConcrete ?? "B20";
                TinhToanVM.Rb = VatLieuVM.Rb;
                TinhToanVM.Eb = VatLieuVM.Eb;
                TinhToanVM.MacThep = VatLieuVM.SelectedSteel;
                TinhToanVM.Rs = VatLieuVM.Rs;
                TinhToanVM.Es = VatLieuVM.Es;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đồng bộ lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    FamilySymbol symbol = _doc.GetElement(fi.GetTypeId()) as FamilySymbol;
                    Parameter pb = symbol?.LookupParameter("b") ?? fi.LookupParameter("b");
                    Parameter ph = symbol?.LookupParameter("h") ?? fi.LookupParameter("h");

                    if (pb == null || ph == null) continue;

                    double b_mm = pb.AsDouble() * 304.8;
                    double h_mm = ph.AsDouble() * 304.8;

                    double len_mm = 0;
                    var bb = fi.get_BoundingBox(null);
                    if (bb != null) len_mm = Math.Abs(bb.Max.Z - bb.Min.Z) * 304.8;

                    string levelName = _doc.GetElement(fi.LevelId)?.Name ?? "—";

                    DanhSachCot.Add(new ColumnModel
                    {
                        Id = fi.Id.IntegerValue.ToString(),
                        Name = fi.Name,
                        Width = Math.Round(b_mm).ToString(),
                        Height = Math.Round(h_mm).ToString(),
                        Length = Math.Round(len_mm).ToString(),
                        Level = levelName,
                        Load = TaiTrong != 0 ? TaiTrong.ToString("0.##") : "—",
                        MomentX = MomenX != 0 ? MomenX.ToString("0.##") : "—",
                        MomentY = MomenY != 0 ? MomenY.ToString("0.##") : "—",
                        ConcreteGrade = string.IsNullOrEmpty(MacBeTong) ? "—" : MacBeTong
                    });

                    count++;
                }

                MessageBox.Show(
                    $"✅ Đã nhập {count} cột từ Revit.",
                    "Hoàn tất", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Nhập cột lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LuuCotAll()
        {
            if (DanhSachCot == null || DanhSachCot.Count == 0)
            {
                MessageBox.Show("Danh sách cột trống.", "Cảnh báo");
                return;
            }

            try
            {
                // ✅ CẬP NHẬT TẤT CẢ CỘT TRONG DANH SÁCH
                foreach (var cot in DanhSachCot)
                {
                    cot.Load = TaiTrong != 0 ? TaiTrong.ToString("0.##") : "—";
                    cot.MomentX = MomenX != 0 ? MomenX.ToString("0.##") : "—";
                    cot.MomentY = MomenY != 0 ? MomenY.ToString("0.##") : "—";
                    cot.ConcreteGrade = SelectedConcrete ?? "—";
                }

                // Đồng bộ cột đang chọn (nếu có) sang tab Tính toán
                if (SelectedColumn != null)
                {
                    SyncToTinhToan();
                }

                MessageBox.Show(
                    $"✅ Đã lưu thông số cho {DanhSachCot.Count} cột.",
                    "Thành công",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}