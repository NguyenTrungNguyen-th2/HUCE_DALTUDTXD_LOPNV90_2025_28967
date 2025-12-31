// File: ViewModel/ColumnViewModel.cs
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Autodesk.Revit.DB;
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

        public ICommand cmNhapRevit { get; private set; }
        public ICommand cmThemNoiLuc { get; private set; }
        public ICommand cmXoaCot { get; private set; }

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
                "Ngàm ngàm", "Ngàm khớp", "Khớp khớp", "Ngàm", "Khung cứng", "Khung khớp"
            };
            LienKetDangChon = DanhSachLienKet.First();
            HeSoPsi = HeSoTinhToan.Psi;

            DanhSachCotTuRevit = ClassBienToanCuc.RevitColumns;
            DanhSachCotDaGanNoiLuc = ClassBienToanCuc.Columns;

            cmNhapRevit = new RelayCommand(_ => NhapTuRevit());
            cmThemNoiLuc = new RelayCommand(_ => ThemNoiLuc());
            cmXoaCot = new RelayCommand(_ => XoaCot());
        }

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

        // ←←← TRỎ VÀO STATIC CLASS
        public ObservableCollection<ColumnModel> DanhSachCotTuRevit { get; private set; }
        public ObservableCollection<ColumnModel> DanhSachCotDaGanNoiLuc { get; private set; }

        private ColumnModel _cotDangChon;
        public ColumnModel CotDangChon
        {
            get => _cotDangChon;
            set { _cotDangChon = value; OnPropertyChanged(); }
        }

        private ColumnModel _selectedCot;
        public ColumnModel SelectedCot
        {
            get => _selectedCot;
            set { _selectedCot = value; OnPropertyChanged(); }
        }

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

        private void ThemNoiLuc()
        {
            if (CotDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn một cột trong danh sách Revit.");
                return;
            }

            string mark = CotDangChon.Mark;
            int soLan = DanhSachCotDaGanNoiLuc.Count(c => c.Mark == mark);
            int soCombo = soLan + 1;

            var newCot = new ColumnModel
            {
                Id = CotDangChon.Id,
                Mark = mark,
                Width = CotDangChon.Width,
                Height = CotDangChon.Height,
                Length = CotDangChon.Length,
                Level = CotDangChon.Level,
                ConcreteGrade = CotDangChon.ConcreteGrade,

                LoadValue = TaiTrong,
                MomentXValue = MomenX,
                MomentYValue = MomenY,
                Psi = HeSoPsi,
                LienKet = LienKetDangChon,

                ComboDisplay = $"Combo {soCombo}"
            };

            DanhSachCotDaGanNoiLuc.Add(newCot);
        }

        private void NhapTuRevit()
        {
            ClassBienToanCuc.RevitColumns.Clear();

            var typeMarkCounter = new Dictionary<string, int>();

            var columns = new FilteredElementCollector(_doc)
                .OfCategory(BuiltInCategory.OST_StructuralColumns)
                .WhereElementIsNotElementType()
                .OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>();

            using (Transaction t = new Transaction(_doc, "Gán Mark tự động"))
            {
                t.Start();

                foreach (var fi in columns)
                {
                    string mark = fi.LookupParameter("Column Location Mark")?.AsString()?.Trim();

                    if (string.IsNullOrEmpty(mark))
                    {
                        mark = fi.LookupParameter("Mark")?.AsString()?.Trim();
                    }

                    if (string.IsNullOrEmpty(mark))
                    {
                        string typeMark = fi.Symbol.LookupParameter("Type Mark")?.AsString() ?? "DEFAULT";

                        if (!typeMarkCounter.ContainsKey(typeMark))
                            typeMarkCounter[typeMark] = 1;
                        else
                            typeMarkCounter[typeMark]++;

                        int index = typeMarkCounter[typeMark];
                        mark = $"{typeMark}-C{index}";

                        Parameter markParam = fi.LookupParameter("Mark");
                        if (markParam != null && !markParam.IsReadOnly)
                        {
                            markParam.Set(mark);
                        }
                    }

                    var symbol = _doc.GetElement(fi.GetTypeId()) as FamilySymbol;
                    var pb = symbol?.LookupParameter("b") ?? fi.LookupParameter("b");
                    var ph = symbol?.LookupParameter("h") ?? fi.LookupParameter("h");
                    if (pb == null || ph == null) continue;

                    double b = pb.AsDouble() * 304.8; // ft → mm
                    double h = ph.AsDouble() * 304.8;
                    var bb = fi.get_BoundingBox(null);
                    double len = bb != null ? Math.Abs(bb.Max.Z - bb.Min.Z) * 304.8 : 0;
                    string level = _doc.GetElement(fi.LevelId)?.Name ?? "—";

                    ClassBienToanCuc.RevitColumns.Add(new ColumnModel
                    {
                        Id = fi.Id.IntegerValue.ToString(),
                        Mark = mark,
                        Width = Math.Round(b).ToString(),
                        Height = Math.Round(h).ToString(),
                        Length = Math.Round(len).ToString(),
                        Level = level,
                        ConcreteGrade = MacBeTong ?? "B20"
                    });
                }

                t.Commit();
            }

            MessageBox.Show($"Đã nhập {ClassBienToanCuc.RevitColumns.Count} cột từ Revit.");
        }

        private void XoaCot()
        {
            if (SelectedCot == null) return;

            string mark = SelectedCot.Mark;
            if (MessageBox.Show($"Xóa tổ hợp '{SelectedCot.DisplayName}'?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DanhSachCotDaGanNoiLuc.Remove(SelectedCot);

                var cacDong = DanhSachCotDaGanNoiLuc
                    .Where(c => c.Mark == mark)
                    .OrderBy(c => DanhSachCotDaGanNoiLuc.IndexOf(c))
                    .ToList();

                for (int i = 0; i < cacDong.Count; i++)
                {
                    cacDong[i].ComboDisplay = $"Combo {i + 1}";
                }

                SelectedCot = null;
            }
        }
        public void XoaNhieuCot(List<ColumnModel> cotsCanXoa)
        {
            if (cotsCanXoa == null || cotsCanXoa.Count == 0) return;

            string danhSachTen = string.Join("\n", cotsCanXoa.Select(c => $"'{c.DisplayName}'"));
            var result = MessageBox.Show(
                $"Xác nhận xóa {cotsCanXoa.Count} tổ hợp:\n{danhSachTen}",
                "Xác nhận",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result != MessageBoxResult.Yes) return;

            var marksBiAnhHuong = cotsCanXoa.Select(c => c.Mark).ToHashSet();

            foreach (var cot in cotsCanXoa.ToList())
            {
                DanhSachCotDaGanNoiLuc.Remove(cot);
            }

            foreach (string mark in marksBiAnhHuong)
            {
                var cacDongConLai = DanhSachCotDaGanNoiLuc
                    .Where(c => c.Mark == mark)
                    .OrderBy(c => DanhSachCotDaGanNoiLuc.IndexOf(c))
                    .ToList();

                for (int i = 0; i < cacDongConLai.Count; i++)
                {
                    var cot = cacDongConLai[i];
                    cot.ComboDisplay = $"Combo {i + 1}";
                }
            }

            SelectedCot = null;
        }
    }
}