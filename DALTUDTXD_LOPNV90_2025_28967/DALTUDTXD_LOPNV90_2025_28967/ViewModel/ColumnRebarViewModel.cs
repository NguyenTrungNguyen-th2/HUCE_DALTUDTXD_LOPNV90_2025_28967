using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DALTUDTXD_LOPNV90_2025_28967.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using DALTUDTXD_LOPNV90_2025_28967.classcolumnrebar;
using DALTUDTXD_LOPNV90_2025_28967.ClassColumnRebar;

using Point = System.Windows.Point;

namespace DALTUDTXD_LOPNV90_2025_28967.ViewModel
{
    public class ColumnRebarViewModel : ObservableObject
    {
        // ========== FIELD MỚI ==========
        private readonly MainViewModel _mainVM; // 👈 THÊM

        // ========== CÁC PROPERTY CỦA BẠN (GIỮ NGUYÊN) ==========
        public LanType SeletedLanType
        {
            get => _seletedLanType;
            set
            {
                if (Equals(objA: value, objB: _seletedLanType)) return;
                _seletedLanType = value;
                OnPropertyChanged();
            }
        }

        public List<LanType> LanType
        {
            get => _lanType;
            set => _lanType = value;
        }

        public List<RebarHookType> HookTypes
        {
            get => _hookTypes;
            set
            {
                if (Equals(objA: value, objB: _hookTypes)) return;
                _hookTypes = value;
                OnPropertyChanged();
            }
        }

        public RebarHookType HookTypet
        {
            get => _hookTypet;
            set
            {
                if (Equals(objA: value, objB: _hookTypet)) return;
                _hookTypet = value;
                OnPropertyChanged();
            }
        }

        public RebarHookType HookTyped
        {
            get => _hookType;
            set
            {
                if (Equals(objA: value, objB: _hookType)) return;
                _hookType = value;
                OnPropertyChanged();
            }
        }

        public int NumberOfXRebar
        {
            get => _numberOfXRebar;
            set
            {
                if (value == _numberOfXRebar) return;
                _numberOfXRebar = value;
                OnPropertyChanged();
                vematcatcot();
            }
        }

        public int NumberOfYRebar
        {
            get => _numberOfYRebar;
            set
            {
                if (value == _numberOfYRebar) return;
                _numberOfYRebar = value;
                OnPropertyChanged();
                vematcatcot();
            }
        }

        public RebarBarType XDiameter
        {
            get => _xDiameter;
            set
            {
                if (Equals(objA: value, objB: _xDiameter)) return;
                _xDiameter = value;
                OnPropertyChanged();
                vematcatcot();
            }
        }

        public RebarBarType YDiameter
        {
            get => _yDiameter;
            set
            {
                if (Equals(objA: value, objB: _yDiameter)) return;
                _yDiameter = value;
                OnPropertyChanged();
                vematcatcot();
            }
        }

        public List<RebarBarType> Diameters
        {
            get => _diameters;
            set
            {
                if (Equals(objA: value, objB: _diameters)) return;
                _diameters = value;
                OnPropertyChanged();
            }
        }

        public int StirrupSpacing
        {
            get => _stirrupSpacing;
            set
            {
                if (value == _stirrupSpacing) return;
                _stirrupSpacing = value;
                OnPropertyChanged();
            }
        }

        public RebarBarType StirrupDiameter
        {
            get => _stirrupDiameter;
            set
            {
                if (Equals(objA: value, objB: _stirrupDiameter)) return;
                _stirrupDiameter = value;
                OnPropertyChanged();
                vematcatcot();
            }
        }

        public double Cover
        {
            get => _cover;
            set
            {
                if (value.Equals(obj: _cover)) return;
                _cover = value;
                OnPropertyChanged();
                vematcatcot();
            }
        }

        public RebarShape StirrupShape
        {
            get => _stirrupShape;
            set
            {
                if (Equals(value, _stirrupShape)) return;
                _stirrupShape = value;
                OnPropertyChanged();
            }
        }

        public List<RebarShape> StirrupRebarShapes
        {
            get => _stirrupRebarShapes;
            set
            {
                if (Equals(value, _stirrupRebarShapes)) return;
                _stirrupRebarShapes = value;
                OnPropertyChanged();
            }
        }

        public int Lan
        {
            get => _lan;
            set
            {
                if (value.Equals(obj: _lan)) return;
                _lan = value;
                OnPropertyChanged();
                vematcatcot();
            }
        }

        public DistributeStirrupType DistributeStirrupType
        {
            get => _distributeStirrupType;
            set
            {
                if (Equals(value, _distributeStirrupType)) return;
                _distributeStirrupType = value;
                OnPropertyChanged();
            }
        }

        public List<DistributeStirrupType> DistributeStirrupTypes
        {
            get => _distributeStirrupTypes;
            set
            {
                if (Equals(value, _distributeStirrupTypes)) return;
                _distributeStirrupTypes = value;
                OnPropertyChanged();
            }
        }

        public MainWindow MainWindow
        {
            get => _mainWindow;
            set => _mainWindow = value;
        }

        public ColumnRebarModel columnModel;

        public RelayCommand OkCommand
        {
            get => _okCommand;
            set => _okCommand = value;
        }

        public RelayCommand CloseCommand
        {
            get => _closeCommand;
            set => _closeCommand = value;
        }

        public RelayCommand AddColumnCommand { get; set; }

        // ========== FIELD CỦA BẠN (GIỮ NGUYÊN, CHỈ THÊM _mainVM) ==========
        private Document doc;
        private UIDocument uiDoc;
        private int _numberOfXRebar = 4;
        private int _numberOfYRebar = 4;
        private RebarBarType _xDiameter;
        private RebarBarType _yDiameter;
        private List<RebarBarType> _diameters;
        private int _stirrupSpacing = 200;
        private RebarBarType _stirrupDiameter;
        private List<RebarHookType> _hookTypes;
        private RebarHookType _hookType;
        private RebarHookType _hookTypet;
        private LanType _seletedLanType;
        private Geometry _profileGeometry;
        private Geometry _profileStirrup;
        private Geometry _profiletopXRebar;
        private Geometry _profilebotXRebar;
        private Geometry _profileleftYRebar;
        private Geometry _profilerightYGeometry;
        private int _lan = 30;
        private double _cover = 30;
        private RebarShape _stirrupShape;
        private List<RebarShape> _stirrupRebarShapes;
        private bool _distributeMode;
        private List<LanType> _lanType;
        private MainWindow _mainWindow;
        private RelayCommand _okCommand;
        private RelayCommand _closeCommand;
        private DistributeStirrupType _distributeStirrupType;
        private List<DistributeStirrupType> _distributeStirrupTypes;

        // ========== CONSTRUCTOR ĐÃ SỬA ==========
        public ColumnRebarViewModel(Document doc, UIDocument uiDoc, MainViewModel mainVM)
        {
            if (mainVM == null)
                throw new ArgumentNullException(nameof(mainVM));
            _mainVM = mainVM; // 👈 GÁN THAM CHIẾU

                LanType = new List<LanType>
                {
                    new LanType { Name = "So le" },
                    new LanType { Name = "Đồng nhất" }
                };

                DistributeStirrupTypes = new List<DistributeStirrupType>()
                {
                    new DistributeStirrupType()
                    {
                        Name = "Phân bố đều",
                        ImagePath = "/DALTUDTXD_LOPNV90_2025_28967;component/resources/images/thepdaipbd.png"
                    },
                    new DistributeStirrupType()
                    {
                        Name = "Tùy chỉnh",
                        ImagePath = "/DALTUDTXD_LOPNV90_2025_28967;component/resources/images/thepdai.png"
                    }
                };

                DistributeStirrupType = DistributeStirrupTypes.FirstOrDefault();
                SeletedLanType = LanType.FirstOrDefault();

                this.doc = doc;
                this.uiDoc = uiDoc;

                // 👇 THEO DÕI KHI CỘT ĐƯỢC CHỌN TRONG DATAGRID
                _mainVM.ColumnVM.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(ColumnViewModel.SelectedColumn))
                    {
                        LoadColumnFromSelection();
                    }
                };

                OkCommand = new RelayCommand(_ => Run());
                CloseCommand = new RelayCommand(_ => Close());
                AddColumnCommand = new RelayCommand(_ => AddColumn());
            }

            // ========== CÁC HÀM CỦA BẠN (GIỮ NGUYÊN) ==========
            void AddColumn()
            {
            }

        // 🔥 HÀM GetData() BỊ XÓA HOÀN TOÀN (VÌ KHÔNG DÙNG ĐẾN)

        void Run()
        {
            // 👇 1. LẤY CỘT TỪ SELECTEDCOLUMN
            var selectedCol = _mainVM.ColumnVM.SelectedColumn;
            if (selectedCol == null)
            {
                TaskDialog.Show("Lỗi", "Vui lòng chọn cột trong DataGrid trước khi vẽ thép!");
                return;
            }

            if (!int.TryParse(selectedCol.Id, out int id))
            {
                TaskDialog.Show("Lỗi", "ID cột không hợp lệ!");
                return;
            }

            var column = doc.GetElement(new ElementId(id)) as FamilyInstance;
            if (column == null)
            {
                TaskDialog.Show("Lỗi", "Không tìm thấy cột trong mô hình Revit!");
                return;
            }

            // 👇 2. GÁN columnModel
            columnModel = new ColumnRebarModel(column);

            // 👇 3. KIỂM TRA THÉP ĐÃ TỒN TẠI CHƯA — CHÈN ĐOẠN CODE NÀY Ở ĐÂY
            var existingRebars = new FilteredElementCollector(doc)
                .OfClass(typeof(Rebar))
                .Cast<Rebar>()
                .Where(r => r.GetHostId() == column.Id).ToList();

            if (existingRebars.Any())
            {
                var result = TaskDialog.Show(
                    "Thông báo",
                    "Cột này đã có thép. Bạn có muốn xóa và tạo lại?",
                    TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No
                );
                if (result == TaskDialogResult.No)
                    return;

                using (var txDelete = new Transaction(doc, "Xóa thép cũ"))
                {
                    txDelete.Start();
                    foreach (var rebar in existingRebars)
                    {
                        doc.Delete(rebar.Id);
                    }
                    txDelete.Commit();
                }
            }

            // 👇 4. TẠO THÉP MỚI
            using (var tx = new Transaction(doc, "Vẽ thép cột"))
            {
                try
                {
                    tx.Start();
                    CreateStirrup();
                    CreateXMainRebar();
                    if (NumberOfYRebar > 2)
                    {
                        CreateYMainRebar();
                    }
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.RollBack();
                    TaskDialog.Show("Lỗi", $"Không thể vẽ thép: {ex.Message}");
                }
            }

            MainWindow?.Close();
        }

        void CreateStirrup()
            {
                var o1 = columnModel.D.Add(source: columnModel.XVector * Cover.MmToFeet()).Add(source: columnModel.YVector * Cover.MmToFeet());
                o1 = new XYZ(x: o1.X, y: o1.Y, z: columnModel.BotElevation + Cover.MmToFeet() + StirrupDiameter.BarNominalDiameter / 2);
                var rebar = Rebar.CreateFromRebarShape(doc: doc, rebarShape: StirrupShape, barType: StirrupDiameter, host: columnModel.Column, origin: o1, xVec: columnModel.XVector,
                    yVec: columnModel.YVector);
                var shapeDrivenAccessor = rebar.GetShapeDrivenAccessor();
                shapeDrivenAccessor.ScaleToBox(origin: o1, xVec: columnModel.XVector * (columnModel.Width - 2 * Cover.MmToFeet()), yVec: (columnModel.YVector) * (columnModel.Height - 2 * Cover.MmToFeet()));
                shapeDrivenAccessor.SetLayoutAsMaximumSpacing(spacing: StirrupSpacing.MmToFeet(), arrayLength: (columnModel.TopElevation - columnModel.BotElevation) - 2 * Cover.MmToFeet() - StirrupDiameter.BarNominalDiameter, barsOnNormalSide: true, includeFirstBar: true, includeLastBar: true);
            }

            void CreateXMainRebar()
            {
                var spacing2Rebars =
                    (columnModel.Width - 2 * Cover.MmToFeet() - 2 * StirrupDiameter.BarNominalDiameter -
                     XDiameter.BarNominalDiameter) / (NumberOfXRebar - 1);
                var columnHeight = columnModel.TopElevation - columnModel.BotElevation;
                var topRebars = new List<Rebar>();
                if (NumberOfXRebar > 2)
                {
                    for (int i = 0; i < NumberOfXRebar; i++)
                    {
                        var o2 = columnModel.A
                            .Add(source: columnModel.XVector * (Cover.MmToFeet() + StirrupDiameter.BarNominalDiameter +
                                                                XDiameter.BarNominalDiameter / 2 + spacing2Rebars * i))
                            .Add(source: -columnModel.YVector *
                                         (Cover.MmToFeet() + StirrupDiameter.BarNominalDiameter + XDiameter.BarNominalDiameter / 2));
                        o2 = new XYZ(x: o2.X, y: o2.Y, z: columnModel.BotElevation);
                        var o3 = columnModel.D
                            .Add(source: columnModel.XVector * (Cover.MmToFeet() + StirrupDiameter.BarNominalDiameter +
                                                                XDiameter.BarNominalDiameter / 2 + spacing2Rebars * i))
                            .Add(source: columnModel.YVector *
                                         (Cover.MmToFeet() + StirrupDiameter.BarNominalDiameter + XDiameter.BarNominalDiameter / 2));
                        o3 = new XYZ(x: o3.X, y: o3.Y, z: columnModel.BotElevation);
                        if (Lan > 0)
                        {
                            var line20 = Line.CreateBound(endpoint1: o2,
                                endpoint2: o2.Add(source: XYZ.BasisZ * (columnHeight + (Lan - 10) * XDiameter.BarNominalDiameter)));
                            var line30 = Line.CreateBound(endpoint1: o2,
                                endpoint2: o2.Add(source: XYZ.BasisZ * (columnHeight + Lan * XDiameter.BarNominalDiameter)));
                            var line20_o3 = Line.CreateBound(endpoint1: o3,
                                endpoint2: o3.Add(source: XYZ.BasisZ * (columnHeight + (Lan - 10) * XDiameter.BarNominalDiameter)));
                            var line30_o3 = Line.CreateBound(endpoint1: o3,
                                endpoint2: o3.Add(source: XYZ.BasisZ * (columnHeight + Lan * XDiameter.BarNominalDiameter)));

                            if (i % 2 == 0)
                            {
                                var rebarA = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: XDiameter, startHook: HookTyped,
                                    endHook: HookTypet,
                                    host: columnModel.Column,
                                    norm: columnModel.XVector, curves: new List<Curve>() { line20 }, startHookOrient: RebarHookOrientation.Right,
                                    endHookOrient: RebarHookOrientation.Right, useExistingShapeIfPossible: true, createNewShape: true);
                                topRebars.Add(item: rebarA);
                                var rebarD = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: XDiameter, startHook: HookTyped,
                                    endHook: HookTypet,
                                    host: columnModel.Column,
                                    norm: columnModel.XVector, curves: new List<Curve>() { line30_o3 }, startHookOrient: RebarHookOrientation.Left,
                                    endHookOrient: RebarHookOrientation.Left, useExistingShapeIfPossible: true, createNewShape: true);
                                topRebars.Add(item: rebarD);
                            }
                            else
                            {
                                var rebarA = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: XDiameter, startHook: HookTyped,
                                    endHook: HookTypet,
                                    host: columnModel.Column,
                                    norm: columnModel.XVector, curves: new List<Curve>() { line30 }, startHookOrient: RebarHookOrientation.Right,
                                    endHookOrient: RebarHookOrientation.Right, useExistingShapeIfPossible: true, createNewShape: true);
                                topRebars.Add(item: rebarA);
                                var rebarD = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: XDiameter, startHook: HookTyped,
                                    endHook: HookTypet,
                                    host: columnModel.Column,
                                    norm: columnModel.XVector, curves: new List<Curve>() { line20_o3 }, startHookOrient: RebarHookOrientation.Left,
                                    endHookOrient: RebarHookOrientation.Left, useExistingShapeIfPossible: true, createNewShape: true);
                                topRebars.Add(item: rebarD);
                            }
                        }
                        else
                        {
                            var line0 = Line.CreateBound(endpoint1: o2,
                                endpoint2: o2.Add(source: XYZ.BasisZ * (columnHeight)));
                            var line01 = Line.CreateBound(endpoint1: o2,
                                endpoint2: o2.Add(source: XYZ.BasisZ * (columnHeight)));
                            var line02 = Line.CreateBound(endpoint1: o3,
                                endpoint2: o3.Add(source: XYZ.BasisZ * (columnHeight)));
                            var line03 = Line.CreateBound(endpoint1: o3,
                                endpoint2: o3.Add(source: XYZ.BasisZ * (columnHeight)));

                            if (i % 2 == 0)
                            {
                                var rebarA = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: XDiameter, startHook: HookTyped,
                                    endHook: HookTypet,
                                    host: columnModel.Column,
                                    norm: columnModel.XVector, curves: new List<Curve>() { line0 }, startHookOrient: RebarHookOrientation.Right,
                                    endHookOrient: RebarHookOrientation.Right, useExistingShapeIfPossible: true, createNewShape: true);
                                topRebars.Add(item: rebarA);
                                var rebarD = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: XDiameter, startHook: HookTyped,
                                    endHook: HookTypet,
                                    host: columnModel.Column,
                                    norm: columnModel.XVector, curves: new List<Curve>() { line03 }, startHookOrient: RebarHookOrientation.Left,
                                    endHookOrient: RebarHookOrientation.Left, useExistingShapeIfPossible: true, createNewShape: true);
                                topRebars.Add(item: rebarD);
                            }
                            else
                            {
                                var rebarA = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: XDiameter, startHook: HookTyped,
                                    endHook: HookTypet,
                                    host: columnModel.Column,
                                    norm: columnModel.XVector, curves: new List<Curve>() { line01 }, startHookOrient: RebarHookOrientation.Right,
                                    endHookOrient: RebarHookOrientation.Right, useExistingShapeIfPossible: true, createNewShape: true);
                                topRebars.Add(item: rebarA);
                                var rebarD = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: XDiameter, startHook: HookTyped,
                                    endHook: HookTypet,
                                    host: columnModel.Column,
                                    norm: columnModel.XVector, curves: new List<Curve>() { line02 }, startHookOrient: RebarHookOrientation.Left,
                                    endHookOrient: RebarHookOrientation.Left, useExistingShapeIfPossible: true, createNewShape: true);
                                topRebars.Add(item: rebarD);
                            }
                        }
                    }
                }
                else
                {
                    TaskDialog.Show(title: "loi", mainInstruction: "ban hay nhap lai so thanh");
                    return;
                }
            }

        void CreateYMainRebar()
        {
            var columnHeight = columnModel.TopElevation - columnModel.BotElevation;
            var spacing2Rebars =
                (columnModel.Height - 2 * Cover.MmToFeet() - 2 * StirrupDiameter.BarNominalDiameter -
                 YDiameter.BarNominalDiameter) / (NumberOfYRebar - 1);
            var topRebars = new List<Rebar>();
            if (NumberOfYRebar > 2)
            {
                for (int i = 1; i <= NumberOfYRebar - 2; i++)
                {
                    var o2 = columnModel.A
                        .Add(source: columnModel.XVector *
                                     (Cover.MmToFeet() + StirrupDiameter.BarNominalDiameter + YDiameter.BarNominalDiameter / 2))
                        .Add(source: -columnModel.YVector *
                                     (Cover.MmToFeet() + StirrupDiameter.BarNominalDiameter + YDiameter.BarNominalDiameter / 2 +
                                      i * spacing2Rebars));
                    o2 = new XYZ(x: o2.X, y: o2.Y, z: columnModel.BotElevation);
                    var o3 = columnModel.C
                        .Add(source: -columnModel.XVector *
                                     (Cover.MmToFeet() + StirrupDiameter.BarNominalDiameter + YDiameter.BarNominalDiameter / 2))
                        .Add(source: columnModel.YVector *
                                     (Cover.MmToFeet() + StirrupDiameter.BarNominalDiameter + YDiameter.BarNominalDiameter / 2 +
                                      i * spacing2Rebars));
                    o3 = new XYZ(x: o3.X, y: o3.Y, z: columnModel.BotElevation);
                    if (Lan > 0)
                    {
                        var line20 = Line.CreateBound(endpoint1: o2,
                            endpoint2: o2.Add(source: XYZ.BasisZ * (columnHeight + 20 * YDiameter.BarNominalDiameter)));
                        var line30 = Line.CreateBound(endpoint1: o2,
                            endpoint2: o2.Add(source: XYZ.BasisZ * (columnHeight + 30 * YDiameter.BarNominalDiameter)));
                        var line20_o3 = Line.CreateBound(endpoint1: o3,
                            endpoint2: o3.Add(source: XYZ.BasisZ * (columnHeight + 20 * YDiameter.BarNominalDiameter)));
                        var line30_o3 = Line.CreateBound(endpoint1: o3,
                            endpoint2: o3.Add(source: XYZ.BasisZ * (columnHeight + 30 * YDiameter.BarNominalDiameter)));

                        if (i % 2 == 0)
                        {
                            var rebarA = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: YDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.YVector, curves: new List<Curve>() { line20 }, startHookOrient: RebarHookOrientation.Right,
                                endHookOrient: RebarHookOrientation.Right, useExistingShapeIfPossible: true, createNewShape: true);
                            topRebars.Add(item: rebarA);
                            var rebarC = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: YDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.YVector, curves: new List<Curve>() { line20_o3 }, startHookOrient: RebarHookOrientation.Left,
                                endHookOrient: RebarHookOrientation.Left, useExistingShapeIfPossible: true, createNewShape: true);
                            topRebars.Add(item: rebarC);
                        }
                        else
                        {
                            var rebarA = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: YDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.YVector, curves: new List<Curve>() { line30 }, startHookOrient: RebarHookOrientation.Right,
                                endHookOrient: RebarHookOrientation.Right, useExistingShapeIfPossible: true, createNewShape: true);
                            var rebarC = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: YDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.YVector, curves: new List<Curve>() { line30_o3 }, startHookOrient: RebarHookOrientation.Left,
                                endHookOrient: RebarHookOrientation.Left, useExistingShapeIfPossible: true, createNewShape: true);
                            topRebars.Add(item: rebarC);
                        }
                    }
                    else
                    {
                        var line20 = Line.CreateBound(endpoint1: o2,
                            endpoint2: o2.Add(source: XYZ.BasisZ * (columnHeight)));
                        var line30 = Line.CreateBound(endpoint1: o2,
                            endpoint2: o2.Add(source: XYZ.BasisZ * (columnHeight)));
                        var line20_o3 = Line.CreateBound(endpoint1: o3,
                            endpoint2: o3.Add(source: XYZ.BasisZ * (columnHeight)));
                        var line30_o3 = Line.CreateBound(endpoint1: o3,
                            endpoint2: o3.Add(source: XYZ.BasisZ * (columnHeight)));

                        if (i % 2 == 0)
                        {
                            var rebarA = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: YDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.YVector, curves: new List<Curve>() { line20 }, startHookOrient: RebarHookOrientation.Right,
                                endHookOrient: RebarHookOrientation.Right, useExistingShapeIfPossible: true, createNewShape: true);
                            topRebars.Add(item: rebarA);
                            var rebarC = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: YDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.YVector, curves: new List<Curve>() { line20_o3 }, startHookOrient: RebarHookOrientation.Left,
                                endHookOrient: RebarHookOrientation.Left, useExistingShapeIfPossible: true, createNewShape: true);
                            topRebars.Add(item: rebarC);
                        }
                        else
                        {
                            var rebarA = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: YDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.YVector, curves: new List<Curve>() { line30 }, startHookOrient: RebarHookOrientation.Right,
                                endHookOrient: RebarHookOrientation.Right, useExistingShapeIfPossible: true, createNewShape: true);
                            var rebarC = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: YDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.YVector, curves: new List<Curve>() { line30_o3 }, startHookOrient: RebarHookOrientation.Left,
                                endHookOrient: RebarHookOrientation.Left, useExistingShapeIfPossible: true, createNewShape: true);
                            topRebars.Add(item: rebarC);
                        }
                    }
                }
            }
        }

        void Close()
        {
            MainWindow.Close();
        }

        void UpdatePreview()
        {
            vematcatcot();
        }

        // ========== HÀM VẼ ĐÃ THÊM KIỂM TRA NULL ==========
        void vematcatcot()
        {
            if (columnModel == null) return; // 👈 BẮT BUỘC

            var columnCanvas = new ColumnCanvas();
            columnCanvas.Origin = new Point(x: 170, y: 300);
            PathGeometry Geo_profile = new PathGeometry();
            PathFigure Figure_profile = new PathFigure();
            Figure_profile.IsClosed = true;
            Figure_profile.StartPoint = new Point(x: columnCanvas.Origin.X - columnModel.Width.FeetToPx() / 2,
                y: columnCanvas.Origin.Y + columnModel.Height.FeetToPx() / 2);
            Geo_profile.Figures.Add(value: Figure_profile);
            PolyLineSegment Segment_profile = new PolyLineSegment();
            Segment_profile.Points.Add(value: new Point(x: columnCanvas.Origin.X + columnModel.Width.FeetToPx() / 2,
                y: columnCanvas.Origin.Y + columnModel.Height.FeetToPx() / 2));
            Segment_profile.Points.Add(value: new Point(x: columnCanvas.Origin.X + columnModel.Width.FeetToPx() / 2,
                y: columnCanvas.Origin.Y - columnModel.Height.FeetToPx() / 2));
            Segment_profile.Points.Add(value: new Point(x: columnCanvas.Origin.X - columnModel.Width.FeetToPx() / 2,
                y: columnCanvas.Origin.Y - columnModel.Height.FeetToPx() / 2));
            Figure_profile.Segments.Add(value: Segment_profile);
            //gán vào Path
            ProfileGeometry = Geo_profile;
            ////ve thep dai
            RectangleGeometry rectangleGeometry = new RectangleGeometry();
            Rect rect = new Rect(point1: new Point(x: columnCanvas.Origin.X - columnModel.Width.FeetToPx() / 2 + Cover.MmToFeet().FeetToPx() + StirrupDiameter.GetDiameterInFeet().FeetToPx() / 2,
                    y: columnCanvas.Origin.Y + columnModel.Height.FeetToPx() / 2 - Cover.MmToFeet().FeetToPx() - StirrupDiameter.GetDiameterInFeet().FeetToPx() / 2)
                , point2: new Point(x: columnCanvas.Origin.X + columnModel.Width.FeetToPx() / 2 - Cover.MmToFeet().FeetToPx() - StirrupDiameter.GetDiameterInFeet().FeetToPx() / 2, y: columnCanvas.Origin.Y - columnModel.Height.FeetToPx() / 2 + Cover.MmToFeet().FeetToPx() + StirrupDiameter.GetDiameterInFeet().FeetToPx() / 2));
            rectangleGeometry.Rect = rect;
            rectangleGeometry.RadiusX = StirrupDiameter.GetDiameterInFeet().FeetToPx();
            rectangleGeometry.RadiusY = StirrupDiameter.GetDiameterInFeet().FeetToPx();
            ProfileStirrup = rectangleGeometry;
            GeometryGroup topGeometryGroup = new GeometryGroup();
            GeometryGroup botGeometryGroup = new GeometryGroup();
            double kcX = ((columnModel.Width.FeetToPx() - 2 * Cover.MmToFeet().FeetToPx() - 2 * StirrupDiameter.GetDiameterInFeet().FeetToPx() - XDiameter.GetDiameterInFeet().FeetToPx()) / (NumberOfXRebar - 1));
            for (int i = 0; i < NumberOfXRebar; i++)
            {
                EllipseGeometry topxRebarGeometry = new EllipseGeometry();
                topxRebarGeometry.Center =
                    new Point(x: columnCanvas.Origin.X - columnModel.Width.FeetToPx() / 2 + Cover.MmToFeet().FeetToPx() + StirrupDiameter.GetDiameterInFeet().FeetToPx() + i * kcX + XDiameter.GetDiameterInFeet().FeetToPx() / 2,
                       y: columnCanvas.Origin.Y - columnModel.Height.FeetToPx() / 2 + Cover.MmToFeet().FeetToPx() + StirrupDiameter.GetDiameterInFeet().FeetToPx() + XDiameter.GetDiameterInFeet().FeetToPx() / 2);
                topxRebarGeometry.RadiusX = XDiameter.GetDiameterInFeet().FeetToPx() / 2;
                topxRebarGeometry.RadiusY = XDiameter.GetDiameterInFeet().FeetToPx() / 2;
                topGeometryGroup.Children.Add(value: topxRebarGeometry);
                EllipseGeometry botxRebarGeometry = new EllipseGeometry();
                botxRebarGeometry.Center =
                    new Point(x: columnCanvas.Origin.X - columnModel.Width.FeetToPx() / 2 + Cover.MmToFeet().FeetToPx() + StirrupDiameter.GetDiameterInFeet().FeetToPx() + i * kcX + XDiameter.GetDiameterInFeet().FeetToPx() / 2,
                        y: columnCanvas.Origin.Y + columnModel.Height.FeetToPx() / 2 - Cover.MmToFeet().FeetToPx() - StirrupDiameter.GetDiameterInFeet().FeetToPx() - XDiameter.GetDiameterInFeet().FeetToPx() / 2);
                botxRebarGeometry.RadiusX = XDiameter.GetDiameterInFeet().FeetToPx() / 2;
                botxRebarGeometry.RadiusY = XDiameter.GetDiameterInFeet().FeetToPx() / 2;
                botGeometryGroup.Children.Add(value: botxRebarGeometry);
            }
            ProfilebotXRebar = topGeometryGroup;
            ProfiletopXRebar = botGeometryGroup;
            //ve thep phuong Y
            GeometryGroup leftGeometryGroup = new GeometryGroup();
            GeometryGroup rightGeometryGroup = new GeometryGroup();
            double kcY = ((columnModel.Height.FeetToPx() - 2 * Cover.MmToFeet().FeetToPx() - 2 * StirrupDiameter.GetDiameterInFeet().FeetToPx() - YDiameter.GetDiameterInFeet().FeetToPx()) / (NumberOfYRebar - 1));
            if (NumberOfYRebar > 2)
            {
                for (int i = 1; i <= NumberOfYRebar - 2; i++)
                {
                    EllipseGeometry leftyRebarGeometry = new EllipseGeometry();
                    leftyRebarGeometry.Center =
                        new Point(
                            x: columnCanvas.Origin.X - columnModel.Width.FeetToPx() / 2 + Cover.MmToFeet().FeetToPx() + StirrupDiameter.GetDiameterInFeet().FeetToPx()
                               + YDiameter.GetDiameterInFeet().FeetToPx() / 2,
                            y: columnCanvas.Origin.Y - columnModel.Height.FeetToPx() / 2 + Cover.MmToFeet().FeetToPx() + StirrupDiameter.GetDiameterInFeet().FeetToPx() +
                               YDiameter.GetDiameterInFeet().FeetToPx() / 2 + i * kcY);
                    leftyRebarGeometry.RadiusX = YDiameter.GetDiameterInFeet().FeetToPx() / 2;
                    leftyRebarGeometry.RadiusY = YDiameter.GetDiameterInFeet().FeetToPx() / 2;
                    topGeometryGroup.Children.Add(value: leftyRebarGeometry);
                    EllipseGeometry rightyRebarGeometry = new EllipseGeometry();
                    rightyRebarGeometry.Center =
                        new Point(
                            x: columnCanvas.Origin.X + columnModel.Width.FeetToPx() / 2 - Cover.MmToFeet().FeetToPx() - StirrupDiameter.GetDiameterInFeet().FeetToPx()
                               - YDiameter.GetDiameterInFeet().FeetToPx() / 2,
                            y: columnCanvas.Origin.Y + columnModel.Height.FeetToPx() / 2 - Cover.MmToFeet().FeetToPx() - StirrupDiameter.GetDiameterInFeet().FeetToPx() -
                               YDiameter.GetDiameterInFeet().FeetToPx() / 2 - i * kcY);
                    rightyRebarGeometry.RadiusX = YDiameter.GetDiameterInFeet().FeetToPx() / 2;
                    rightyRebarGeometry.RadiusY = YDiameter.GetDiameterInFeet().FeetToPx() / 2;
                    rightGeometryGroup.Children.Add(value: rightyRebarGeometry);
                }
            }
            ProfilerightYGeometry = rightGeometryGroup;
            ProfileleftYRebar = leftGeometryGroup;
        }

        // ========== PROPERTY GEOMETRY (GIỮ NGUYÊN) ==========
        public Geometry ProfileGeometry
        {
            get => _profileGeometry;
            set
            {
                if (Equals(objA: value, objB: _profileGeometry)) return;
                _profileGeometry = value;
                OnPropertyChanged();
            }
        }

        public Geometry ProfileStirrup
        {
            get => _profileStirrup;
            set
            {
                if (Equals(objA: value, objB: _profileStirrup)) return;
                _profileStirrup = value;
                OnPropertyChanged();
            }
        }

        public Geometry ProfiletopXRebar
        {
            get => _profiletopXRebar;
            set
            {
                if (Equals(objA: value, objB: _profiletopXRebar)) return;
                _profiletopXRebar = value;
                OnPropertyChanged();
            }
        }

        public Geometry ProfilebotXRebar
        {
            get => _profilebotXRebar;
            set
            {
                if (Equals(objA: value, objB: _profilebotXRebar)) return;
                _profilebotXRebar = value;
                OnPropertyChanged();
            }
        }

        public Geometry ProfileleftYRebar
        {
            get => _profileleftYRebar;
            set
            {
                if (Equals(objA: value, objB: _profileleftYRebar)) return;
                _profileleftYRebar = value;
                OnPropertyChanged();
            }
        }

        public Geometry ProfilerightYGeometry
        {
            get => _profilerightYGeometry;
            set
            {
                if (Equals(objA: value, objB: _profilerightYGeometry)) return;
                _profilerightYGeometry = value;
                OnPropertyChanged();
            }
        }

        // ========== HÀM MỚI: LOAD CỘT TỪ SELECTION ==========
        private void LoadColumnFromSelection()
        {
            var selectedCol = _mainVM.ColumnVM.SelectedColumn;
            if (selectedCol == null || !int.TryParse(selectedCol.Id, out int id)) return;

            var column = doc.GetElement(new ElementId(id)) as FamilyInstance;
            if (column == null) return;

            columnModel = new ColumnRebarModel(column);

            var FilterStirrupName = new HashSet<string> { "M_T1", "M_T2", "M_T6" };
            var FilterHookTypeName = new HashSet<string> { "Standard - 90 deg.", "Standard - 180 deg." };

            StirrupRebarShapes = new FilteredElementCollector(doc).OfClass(typeof(RebarShape)).Cast<RebarShape>()
                .Where(x => FilterStirrupName.Contains(x.Name)).ToList();

            HookTypes = new FilteredElementCollector(doc)
                .OfClass(typeof(RebarHookType))
                .Cast<RebarHookType>()
                .Where(x => FilterHookTypeName.Contains(x.Name))
                .OrderBy(x => x.Name)
                .ToList();

            Diameters = new FilteredElementCollector(document: doc).OfClass(type: typeof(RebarBarType))
                .Cast<RebarBarType>()
                .OrderBy(keySelector: x => x.Name)
                .ToList();

            XDiameter = Diameters.FirstOrDefault(predicate: x => x.BarNominalDiameter.FeetToMm() > 20);
            YDiameter = Diameters.FirstOrDefault(predicate: x => x.BarNominalDiameter.FeetToMm() > 20);
            StirrupDiameter = Diameters.FirstOrDefault(predicate: x => x.BarNominalDiameter.FeetToMm() < 10);
            HookTyped = HookTypes.FirstOrDefault(x => x.Name == "Standard - 90 deg.");
            StirrupShape = StirrupRebarShapes.FirstOrDefault(x => x.Name == "M_T1");

            vematcatcot(); // Cập nhật preview
        }
    }
}