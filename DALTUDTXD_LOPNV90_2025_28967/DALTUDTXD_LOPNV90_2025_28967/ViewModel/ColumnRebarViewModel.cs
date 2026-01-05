using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DALTUDTXD_LOPNV90_2025_28967.classcolumnrebar;
using DALTUDTXD_LOPNV90_2025_28967.ClassColumnRebar;
using DALTUDTXD_LOPNV90_2025_28967.Model;
using DALTUDTXD_LOPNV90_2025_28967.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Point = System.Windows.Point;

namespace DALTUDTXD_LOPNV90_2025_28967.ViewModel
{
    public class ColumnRebarViewModel : ObservableObject
    {
        public double Cover
        {
            get => _cover;
            set
            {
                if (value.Equals(_cover)) return;
                _cover = value;
                OnPropertyChanged();
            }
        }

        public RebarCoverType CoverType
        {
            get => _coverType1;
            set
            {
                if (Equals(value, _coverType1)) return;
                _coverType1 = value;
                OnPropertyChanged();
            }
        }

        public List<RebarCoverType> RebarCoverTypes { get; set; }


        private RebarCoverType _coverType;
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

        public ViewVeThep viewvethep
        {
            get;
            set;
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
        public ColumnRebarModel SelectedColumn
        {
            get => columnModel;
            set
            {
                if (Equals(objA: value, objB: columnModel)) return;
                columnModel = value;
                OnPropertyChanged();
            }
        }


        private Document doc;
        private UIDocument uiDoc;
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
        private RebarShape _stirrupShape;
        private List<RebarShape> _stirrupRebarShapes;
        private bool _distributeMode;
        private List<LanType> _lanType;
        private MainWindow _mainWindow;
        private ViewVeThep _viewVeThep;
        private RelayCommand _okCommand;
        private RelayCommand _closeCommand;
        private DistributeStirrupType _distributeStirrupType;
        private List<DistributeStirrupType> _distributeStirrupTypes;
        private List<RebarBarType> _diameters1;
        private double _cover = 30.MmToFeet();

        private RebarCoverType _coverType1;
        private ViewVeThep _viewvethep;
        public ObservableCollection<ColumnRebarModel> DanhSachCot { get; set; } = new ObservableCollection<ColumnRebarModel>();
        private readonly Dictionary<string, TinhToanViewModel> _ketQuaTheoCot;


            public ColumnRebarViewModel(Document doc, UIDocument uiDoc)
            {
                this.doc = doc;
                this.uiDoc = uiDoc;

                LanType = new List<LanType>
        {
            new LanType { Name = "So le" },
            new LanType { Name = "Đồng nhất" }
        };

                DistributeStirrupTypes = new List<DistributeStirrupType>
        {
            new DistributeStirrupType
            {
                Name = "Phân bố đều",
                ImagePath = "/DALTUDTXD_LOPNV90_2025_28967;component/resources/images/thepdaipbd.png"
            },
            new DistributeStirrupType
            {
                Name = "Tùy chỉnh",
                ImagePath = "/DALTUDTXD_LOPNV90_2025_28967;component/resources/images/thepdai.png"
            }
        };

                DistributeStirrupType = DistributeStirrupTypes.FirstOrDefault();
                SeletedLanType = LanType.FirstOrDefault();

                OkCommand = new RelayCommand(_ => Run());
                CloseCommand = new RelayCommand(w =>
                {
                    if (w is Window window)
                        window.Close();
                });

                GetData();
            }

       

        void GetData()
        {
            var references = uiDoc.Selection.PickObjects(
                ObjectType.Element,
                new ColumnSelectionFilter(),
                "Select Columns!"
            );

            DanhSachCot.Clear();

            foreach (var reference in references)
            {
                var column = doc.GetElement(reference) as FamilyInstance;
                if (column != null)
                {
                    DanhSachCot.Add(new ColumnRebarModel(column));
                }
            }

            SelectedColumn = DanhSachCot.FirstOrDefault();
            var FilterStirrupName = new HashSet<string> { "M_T1", "M_T2", "M_T6" };

            StirrupRebarShapes = new FilteredElementCollector(doc).OfClass(typeof(RebarShape)).Cast<RebarShape>()
                .Where(x => FilterStirrupName.Contains(x.Name)).ToList();
            Diameters = new FilteredElementCollector(doc)
                .OfClass(typeof(RebarBarType))
                .Cast<RebarBarType>()
                .OrderBy(x => x.Name)
                .ToList();
            SelectedColumn.XDiameter = Diameters.FirstOrDefault(x => x.BarNominalDiameter.FeetToMm() > 20)
                                       ?? Diameters.FirstOrDefault()
                                       ?? throw new InvalidOperationException("Không tìm thấy loại thép nào!");
            SelectedColumn.YDiameter = Diameters.FirstOrDefault(x => x.BarNominalDiameter.FeetToMm() > 20)
                                       ?? Diameters.FirstOrDefault()
                                       ?? throw new InvalidOperationException("Không tìm thấy loại thép nào!");

            StirrupDiameter = Diameters.FirstOrDefault(x => x.BarNominalDiameter.FeetToMm() < 10)
                              ?? Diameters.LastOrDefault();
            RebarCoverTypes = new FilteredElementCollector(doc)
                .OfClass(typeof(RebarCoverType))
                .Cast<RebarCoverType>()
                .OrderBy(x => x.Name)
                .ToList();
            CoverType = RebarCoverTypes.FirstOrDefault();
            StirrupShape = StirrupRebarShapes.FirstOrDefault(x => x.Name == "M_T1");
        }

        public void Run()
        {

            using (var tx = new Transaction(document: doc, name: "create columns"))
            {
                tx.Start();
                string mode = $"{SeletedLanType.Name}";
                switch (mode)
                {
                    case "So le":
                        foreach (var column in DanhSachCot)
                        {
                            columnModel = column;
                            CreateStirrup();
                            CreateXMainRebar();
                            CreateYMainRebar();

                        }
                        break;
                    case "Đồng nhất":
                        foreach (var column in DanhSachCot)
                        {
                            columnModel = column;
                            CreateStirrup();
                            CreateXMainRebar1();
                            CreateYMainRebar1();

                        }
                        break;
                }




                tx.Commit();

            }

        }
        void CreateStirrup()
        {
            var o1 = columnModel.D.Add(source: columnModel.XVector * Cover).Add(source: columnModel.YVector * Cover);
            o1 = new XYZ(x: o1.X, y: o1.Y, z: columnModel.BotElevation + Cover + StirrupDiameter.BarNominalDiameter / 2);
            var rebar = Rebar.CreateFromRebarShape(doc: doc, rebarShape: StirrupShape, barType: StirrupDiameter, host: columnModel.Column, origin: o1, xVec: columnModel.XVector,
                yVec: columnModel.YVector);
            var shapeDrivenAccessor = rebar.GetShapeDrivenAccessor();
            shapeDrivenAccessor.ScaleToBox(origin: o1, xVec: columnModel.XVector * (columnModel.Width - 2 * Cover), yVec: (columnModel.YVector) * (columnModel.Height - 2 * Cover));
            shapeDrivenAccessor.SetLayoutAsMaximumSpacing(spacing: StirrupSpacing.MmToFeet(), arrayLength: (columnModel.TopElevation - columnModel.BotElevation) - 2 * Cover - StirrupDiameter.BarNominalDiameter, barsOnNormalSide: true, includeFirstBar: true, includeLastBar: true);
            SetPartition(rebar);
        }

        void CreateXMainRebar()
        {
            var spacing2Rebars =
                (columnModel.Width - 2 * Cover - 2 * StirrupDiameter.BarNominalDiameter -
                 SelectedColumn.XDiameter.BarNominalDiameter) / (SelectedColumn.NumberOfXRebar - 1);
            var columnHeight = columnModel.TopElevation - columnModel.BotElevation;

            var XRebars = new List<Rebar>();
            var XRebars1 = new List<Rebar>();
            if (SelectedColumn.NumberOfXRebar > 2)
            {
                for (int i = 0; i < SelectedColumn.NumberOfXRebar; i++)
                {
                    var o2 = columnModel.A
                        .Add(source: columnModel.XVector * (Cover + StirrupDiameter.BarNominalDiameter +
                                                            SelectedColumn.XDiameter.BarNominalDiameter / 2 + spacing2Rebars * i))
                        .Add(source: -columnModel.YVector *
                                     (Cover + StirrupDiameter.BarNominalDiameter + SelectedColumn.XDiameter.BarNominalDiameter / 2));
                    o2 = new XYZ(x: o2.X, y: o2.Y, z: columnModel.BotElevation);
                    var o3 = columnModel.D
                        .Add(source: columnModel.XVector * (Cover + StirrupDiameter.BarNominalDiameter +
                                                            SelectedColumn.XDiameter.BarNominalDiameter / 2 + spacing2Rebars * i))
                        .Add(source: columnModel.YVector *
                                     (Cover + StirrupDiameter.BarNominalDiameter + SelectedColumn.XDiameter.BarNominalDiameter / 2));
                    o3 = new XYZ(x: o3.X, y: o3.Y, z: columnModel.BotElevation);
                    if (Lan > 0)
                    {
                        var line20 = Line.CreateBound(endpoint1: o2,
                            endpoint2: o2.Add(source: XYZ.BasisZ * (columnHeight + (Lan - 10) * SelectedColumn.XDiameter.BarNominalDiameter)));
                        var line30 = Line.CreateBound(endpoint1: o2,
                            endpoint2: o2.Add(source: XYZ.BasisZ * (columnHeight + Lan * SelectedColumn.XDiameter.BarNominalDiameter)));
                        var line20_o3 = Line.CreateBound(endpoint1: o3,
                            endpoint2: o3.Add(source: XYZ.BasisZ * (columnHeight + (Lan - 10) * SelectedColumn.XDiameter.BarNominalDiameter)));
                        var line30_o3 = Line.CreateBound(endpoint1: o3,
                            endpoint2: o3.Add(source: XYZ.BasisZ * (columnHeight + Lan * SelectedColumn.XDiameter.BarNominalDiameter)));

                        if (i % 2 == 0)
                        {
                            var rebarA = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: SelectedColumn.XDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.XVector, curves: new List<Curve>() { line20 }, startHookOrient: RebarHookOrientation.Right,
                                endHookOrient: RebarHookOrientation.Right, useExistingShapeIfPossible: true, createNewShape: true);
                            XRebars.Add(item: rebarA);
                            var rebarD = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: SelectedColumn.XDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.XVector, curves: new List<Curve>() { line30_o3 }, startHookOrient: RebarHookOrientation.Left,
                                endHookOrient: RebarHookOrientation.Left, useExistingShapeIfPossible: true, createNewShape: true);
                            XRebars.Add(item: rebarD);
                            
                                SetPartition(rebarA);
                                SetPartition(rebarD);

                        }
                        else
                        {
                            var rebarA = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: SelectedColumn.XDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.XVector, curves: new List<Curve>() { line30 }, startHookOrient: RebarHookOrientation.Right,
                                endHookOrient: RebarHookOrientation.Right, useExistingShapeIfPossible: true, createNewShape: true);
                            XRebars1.Add(item: rebarA);
                            var rebarD = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: SelectedColumn.XDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.XVector, curves: new List<Curve>() { line20_o3 }, startHookOrient: RebarHookOrientation.Left,
                                endHookOrient: RebarHookOrientation.Left, useExistingShapeIfPossible: true, createNewShape: true);
                            XRebars1.Add(item: rebarD);
                            foreach (var VARIABLE in XRebars1)
                            {
                                SetPartition(rebarA);
                                SetPartition(rebarD);
                            }
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
                            var rebarA = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: SelectedColumn.XDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.XVector, curves: new List<Curve>() { line0 }, startHookOrient: RebarHookOrientation.Right,
                                endHookOrient: RebarHookOrientation.Right, useExistingShapeIfPossible: true, createNewShape: true);
                            XRebars.Add(item: rebarA);
                            var rebarD = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: SelectedColumn.XDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.XVector, curves: new List<Curve>() { line03 }, startHookOrient: RebarHookOrientation.Left,
                                endHookOrient: RebarHookOrientation.Left, useExistingShapeIfPossible: true, createNewShape: true);
                            XRebars.Add(item: rebarD);
                                SetPartition(rebarA);
                                SetPartition(rebarD);
                            

                        }
                        else
                        {
                            var rebarA = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: SelectedColumn.XDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.XVector, curves: new List<Curve>() { line01 }, startHookOrient: RebarHookOrientation.Right,
                                endHookOrient: RebarHookOrientation.Right, useExistingShapeIfPossible: true, createNewShape: true);
                            XRebars1.Add(item: rebarA);
                            var rebarD = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: SelectedColumn.XDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.XVector, curves: new List<Curve>() { line02 }, startHookOrient: RebarHookOrientation.Left,
                                endHookOrient: RebarHookOrientation.Left, useExistingShapeIfPossible: true, createNewShape: true);
                            XRebars1.Add(item: rebarD);
                           
                                SetPartition(rebarA);
                                SetPartition(rebarD);
                            

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
                (columnModel.Height - 2 * Cover - 2 * StirrupDiameter.BarNominalDiameter -
                 SelectedColumn.YDiameter.BarNominalDiameter) / (SelectedColumn.NumberOfYRebar - 1);
            var YRebars = new List<Rebar>();
            var YRebars1 = new List<Rebar>();
            if (SelectedColumn.NumberOfYRebar > 2)
            {

                for (int i = 1; i <= SelectedColumn.NumberOfYRebar - 2; i++)
                {


                    var o2 = columnModel.A
                        .Add(source: columnModel.XVector *
                                     (Cover + StirrupDiameter.BarNominalDiameter + SelectedColumn.YDiameter.BarNominalDiameter / 2))
                        .Add(source: -columnModel.YVector *
                                     (Cover + StirrupDiameter.BarNominalDiameter + SelectedColumn.YDiameter.BarNominalDiameter / 2 +
                                      i * spacing2Rebars));
                    o2 = new XYZ(x: o2.X, y: o2.Y, z: columnModel.BotElevation);

                    var o3 = columnModel.C
                        .Add(source: -columnModel.XVector *
                                     (Cover + StirrupDiameter.BarNominalDiameter + SelectedColumn.YDiameter.BarNominalDiameter / 2))
                        .Add(source: columnModel.YVector *
                                     (Cover + StirrupDiameter.BarNominalDiameter + SelectedColumn.YDiameter.BarNominalDiameter / 2 +
                                      i * spacing2Rebars));

                    o3 = new XYZ(x: o3.X, y: o3.Y, z: columnModel.BotElevation);
                    if (Lan > 0)
                    {
                        var line20 = Line.CreateBound(endpoint1: o2,
                            endpoint2: o2.Add(source: XYZ.BasisZ * (columnHeight + 20 * SelectedColumn.YDiameter.BarNominalDiameter)));
                        var line30 = Line.CreateBound(endpoint1: o2,
                            endpoint2: o2.Add(source: XYZ.BasisZ * (columnHeight + 30 * SelectedColumn.YDiameter.BarNominalDiameter)));
                        var line20_o3 = Line.CreateBound(endpoint1: o3,
                            endpoint2: o3.Add(source: XYZ.BasisZ * (columnHeight + 20 * SelectedColumn.YDiameter.BarNominalDiameter)));
                        var line30_o3 = Line.CreateBound(endpoint1: o3,
                            endpoint2: o3.Add(source: XYZ.BasisZ * (columnHeight + 30 * SelectedColumn.YDiameter.BarNominalDiameter)));

                        if (i % 2 == 0)
                        {
                            var rebarA = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: SelectedColumn.YDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.YVector, curves: new List<Curve>() { line20 }, startHookOrient: RebarHookOrientation.Right,
                                endHookOrient: RebarHookOrientation.Right, useExistingShapeIfPossible: true, createNewShape: true);
                            YRebars1.Add(item: rebarA);
                            var rebarC = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: SelectedColumn.YDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.YVector, curves: new List<Curve>() { line20_o3 }, startHookOrient: RebarHookOrientation.Left,
                                endHookOrient: RebarHookOrientation.Left, useExistingShapeIfPossible: true, createNewShape: true);
                            YRebars1.Add(item: rebarC);
                                SetPartition(rebarA);
                                SetPartition(rebarC);
                            
                        }
                        else
                        {
                            var rebarA = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: SelectedColumn.YDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.YVector, curves: new List<Curve>() { line30 }, startHookOrient: RebarHookOrientation.Right,
                                endHookOrient: RebarHookOrientation.Right, useExistingShapeIfPossible: true, createNewShape: true);
                            YRebars.Add(item: rebarA);
                            var rebarC = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: SelectedColumn.YDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.YVector, curves: new List<Curve>() { line30_o3 }, startHookOrient: RebarHookOrientation.Left,
                                endHookOrient: RebarHookOrientation.Left, useExistingShapeIfPossible: true, createNewShape: true);
                            YRebars.Add(item: rebarC);
                                SetPartition(rebarA);
                                SetPartition(rebarC);
                            

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
                            var rebarA = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: SelectedColumn.YDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.YVector, curves: new List<Curve>() { line20 }, startHookOrient: RebarHookOrientation.Right,
                                endHookOrient: RebarHookOrientation.Right, useExistingShapeIfPossible: true, createNewShape: true);
                            YRebars1.Add(item: rebarA);
                            var rebarC = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: SelectedColumn.YDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.YVector, curves: new List<Curve>() { line20_o3 }, startHookOrient: RebarHookOrientation.Left,
                                endHookOrient: RebarHookOrientation.Left, useExistingShapeIfPossible: true, createNewShape: true);
                            YRebars1.Add(item: rebarC);
                           
                                SetPartition(rebarA);
                                SetPartition(rebarC);
                            
                        }
                        else
                        {
                            var rebarA = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: SelectedColumn.YDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.YVector, curves: new List<Curve>() { line30 }, startHookOrient: RebarHookOrientation.Right,
                                endHookOrient: RebarHookOrientation.Right, useExistingShapeIfPossible: true, createNewShape: true);
                            YRebars.Add(item: rebarA);
                            var rebarC = Rebar.CreateFromCurves(doc: doc, style: RebarStyle.Standard, barType: SelectedColumn.YDiameter, startHook: HookTyped,
                                endHook: HookTypet,
                                host: columnModel.Column,
                                norm: columnModel.YVector, curves: new List<Curve>() { line30_o3 }, startHookOrient: RebarHookOrientation.Left,
                                endHookOrient: RebarHookOrientation.Left, useExistingShapeIfPossible: true, createNewShape: true);
                            YRebars.Add(item: rebarC);
                                SetPartition(rebarA);
                                SetPartition(rebarC);
                            
                        }

                    }

                }
            }
        }
        void CreateXMainRebar1()
        {
            var spacing2Rebars =
                (columnModel.Width - 2 * Cover - 2 * StirrupDiameter.BarNominalDiameter -
                SelectedColumn.XDiameter.BarNominalDiameter) / (SelectedColumn.NumberOfXRebar - 1);

            var topRebars = new List<Rebar>();
            for (int i = 0; i < SelectedColumn.NumberOfXRebar; i++)
            {
                var o2 = columnModel.A
                    .Add(source: columnModel.XVector * (Cover + StirrupDiameter.BarNominalDiameter +
                    SelectedColumn.XDiameter.BarNominalDiameter / 2 + spacing2Rebars * i))
                    .Add(source: -columnModel.YVector *
                                 (Cover + StirrupDiameter.BarNominalDiameter +
                    SelectedColumn.XDiameter.BarNominalDiameter / 2));
                o2 = new XYZ(o2.X, o2.Y, columnModel.BotElevation);
                var columnHeight = columnModel.TopElevation - columnModel.BotElevation;
                var line1 = Line.CreateBound(endpoint1: o2,
                    endpoint2: o2.Add(source: XYZ.BasisZ *
                                              (columnHeight + Lan * SelectedColumn.XDiameter.BarNominalDiameter)));
                var rebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, SelectedColumn.XDiameter, HookTyped, HookTypet,
                    columnModel.Column, columnModel.XVector, new List<Curve> { line1 }, RebarHookOrientation.Right,
                    RebarHookOrientation.Right, true, true);
                topRebars.Add(item: rebar);
            }
            var centerPointY = columnModel.A.Add(columnModel.YVector.Multiply(-columnModel.Height / 2));
            var mirrorPlane = Plane.CreateByNormalAndOrigin(columnModel.YVector, centerPointY);
            ElementTransformUtils.MirrorElements(doc, topRebars.Select(x => x.Id).ToList(), mirrorPlane, true);
        }
        void CreateYMainRebar1()
        {
            var spacing2Rebars =
                (columnModel.Height - 2 * Cover - 2 * StirrupDiameter.BarNominalDiameter -
                SelectedColumn.YDiameter.BarNominalDiameter) / (SelectedColumn.NumberOfYRebar - 1);

            var topRebars = new List<Rebar>();
            if (SelectedColumn.NumberOfYRebar > 2)
            {
                for (int i = 1; i <= SelectedColumn.NumberOfYRebar - 2; i++)
                {
                    var o2 = columnModel.A
                        .Add(source: columnModel.XVector *
                                     (Cover + StirrupDiameter.BarNominalDiameter +
                        SelectedColumn.YDiameter.BarNominalDiameter / 2))
                        .Add(source: -columnModel.YVector *
                                     (Cover + StirrupDiameter.BarNominalDiameter +
                        SelectedColumn.YDiameter.BarNominalDiameter / 2 +
                                      i * spacing2Rebars));
                    o2 = new XYZ(o2.X, o2.Y, columnModel.BotElevation);
                    var columnHeight = columnModel.TopElevation - columnModel.BotElevation;
                    var line1 = Line.CreateBound(endpoint1: o2,
                        endpoint2: o2.Add(source: XYZ.BasisZ *
                                                  (columnHeight + Lan * SelectedColumn.YDiameter.BarNominalDiameter)));
                    var rebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, SelectedColumn.YDiameter, HookTyped, HookTypet,
                        columnModel.Column, columnModel.YVector, new List<Curve> { line1 }, RebarHookOrientation.Right,
                        RebarHookOrientation.Right, true, true);
                    topRebars.Add(item: rebar);
                }

                var centerPointX = columnModel.A.Add(columnModel.XVector.Multiply(columnModel.Width / 2));
                var mirrorPlane = Plane.CreateByNormalAndOrigin(columnModel.XVector, centerPointX);
                ElementTransformUtils.MirrorElements(doc, topRebars.Select(x => x.Id).ToList(), mirrorPlane, true);
            }
        }
        void Close()
        {
            viewvethep?.Close();
        }

        private bool _applyToAllColumns;
        public bool ApplyToAllColumns
        {
            get => _applyToAllColumns;
            set
            {
                if (_applyToAllColumns == value) return;
                _applyToAllColumns = value;
                OnPropertyChanged();

                if (value && SelectedColumn != null)
                {
                    // Áp dụng thông số từ SelectedColumn sang tất cả các cột
                    ApplyCurrentSettingsToAllColumns();
                }
            }
        }
        private void ApplyCurrentSettingsToAllColumns()
        {
            if (SelectedColumn == null) return;

            var source = SelectedColumn;

            foreach (var col in DanhSachCot)
            {
               
                col.NumberOfXRebar = source.NumberOfXRebar;
                col.NumberOfYRebar = source.NumberOfYRebar;
                col.XDiameter = source.XDiameter;
                col.YDiameter = source.YDiameter;
            }
        }

        void SetPartition(Rebar rb)
        {
            if (rb == null) return;
            string locationMark = columnModel.ColumnLocationMark;
            string baseLevel = columnModel.BaseLevelName;

            string partitionName =
                $"{locationMark}_{baseLevel}";
            Parameter partParam = rb.LookupParameter("Partition");

            if (partParam != null && !partParam.IsReadOnly)
            {
                partParam.Set(partitionName);
            }
        }


    }
}