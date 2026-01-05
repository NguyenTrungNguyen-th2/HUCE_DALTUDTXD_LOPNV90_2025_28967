using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using DALTUDTXD_LOPNV90_2025_28967.ClassColumnRebar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Point1 = System.Windows.Point;
using Transform = Autodesk.Revit.DB.Transform;

namespace DALTUDTXD_LOPNV90_2025_28967.Model
{
    public class ColumnRebarModel:ObservableObject

    {
        private RebarBarType _xDiameter;
        private RebarBarType _yDiameter;
        private int _numberOfXRebar = 4;
        private int _numberOfYRebar = 4;
        public XYZ A { get; set; }
        public XYZ B { get; set; }
        public XYZ C { get; set; }
        public XYZ D { get; set; }
        public double BotElevation { get; set; }
        public double TopElevation { get; set; }
        public XYZ XVector { get; set; }
        public XYZ YVector { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public FamilyInstance Column { get; set; }
        public Point1 Origin { get; set; } = new Point1(100, 200); //tọa độ gốc
        public string ColumnLocationMark { get; }

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

        public double Cover { get; set; } = 30;
        public string BaseLevelName { get; }

        public RebarBarType XDiameter
        {
            get => _xDiameter;
            set
            {
                if (Equals(value, _xDiameter)) return;
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
                if (Equals(value, _yDiameter)) return;
                _yDiameter = value;
                OnPropertyChanged();
                vematcatcot();
            }
        }

        public double StirrupDiameter { get; set; } = 30.MmToFeet().FeetToPx();


        // 👇 Thêm ID và Mark như trước
        public ElementId Id => Column?.Id ?? ElementId.InvalidElementId;
        public string Mark { get; private set; }

        public ColumnRebarModel(FamilyInstance column)
        {
            Column = column;
            var type = column.Symbol;
            Width = type.LookupParameter("b").AsDouble();
            Height = type.LookupParameter("h").AsDouble();
            Transform transform = column.GetTransform();
            A = transform.OfPoint(new XYZ(-Width / 2, Height / 2, 0));
            B = transform.OfPoint(new XYZ(Width / 2, Height / 2, 0));
            C = transform.OfPoint(new XYZ(Width / 2, -Height / 2, 0));
            D = transform.OfPoint(new XYZ(-Width / 2, -Height / 2, 0));
            XVector = transform.OfVector(XYZ.BasisX);
            YVector = transform.OfVector(XYZ.BasisY);
            var bb = column.get_BoundingBox(null);
            TopElevation = bb.Max.Z;
            BotElevation = bb.Min.Z;

            Mark = column.LookupParameter("Mark")?.AsString() ?? "";
            vematcatcot();
            var levelId = Column.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM)?.AsElementId();
            ColumnLocationMark = column
                .LookupParameter("Column Location Mark")
                ?.AsString() ?? "";
            BaseLevelName = (levelId != null && levelId != ElementId.InvalidElementId)
                ? (Column.Document.GetElement(levelId) as Level)?.Name ?? "N/A"
                : "N/A";
        }
        void vematcatcot()
        {

            Origin = new Point1(150, 150);
            PathGeometry Geo_profile = new PathGeometry();
            PathFigure Figure_profile = new PathFigure();
            Figure_profile.IsClosed = true;
            Figure_profile.StartPoint = new Point1(Origin.X - Width.FeetToPx() / 2,
                y: Origin.Y + Height.FeetToPx() / 2);
            Geo_profile.Figures.Add(value: Figure_profile);
            PolyLineSegment Segment_profile = new PolyLineSegment();
            Segment_profile.Points.Add(value: new Point1(x: Origin.X + Width.FeetToPx() / 2,
                y: Origin.Y + Height.FeetToPx() / 2));
            Segment_profile.Points.Add(value: new Point1(x: Origin.X + Width.FeetToPx() / 2,
                y: Origin.Y - Height.FeetToPx() / 2));
            Segment_profile.Points.Add(value: new Point1(x: Origin.X - Width.FeetToPx() / 2,
                y: Origin.Y - Height.FeetToPx() / 2));
            Figure_profile.Segments.Add(value: Segment_profile);
            //gán vào Path
            ProfileGeometry = Geo_profile;
            ////ve thep dai
            RectangleGeometry rectangleGeometry = new RectangleGeometry();
            Rect rect = new Rect(new Point1(x: Origin.X - Width.FeetToPx() / 2 + Cover.MmToFeet().FeetToPx() + StirrupDiameter / 2,
                    y: Origin.Y + Height.FeetToPx() / 2 - Cover.MmToFeet().FeetToPx() - StirrupDiameter / 2)
                , new Point1(x: Origin.X + Width.FeetToPx() / 2 - Cover.MmToFeet().FeetToPx() - StirrupDiameter / 2, y: Origin.Y - Height.FeetToPx() / 2 + Cover.MmToFeet().FeetToPx() + StirrupDiameter / 2));
            rectangleGeometry.Rect = rect;
            rectangleGeometry.RadiusX = StirrupDiameter;
            rectangleGeometry.RadiusY = StirrupDiameter;
            ProfileStirrup = rectangleGeometry;
            GeometryGroup topGeometryGroup = new GeometryGroup();
            GeometryGroup botGeometryGroup = new GeometryGroup();

            // thep phuong x    
            double kcX = ((Width.FeetToPx() - 2 * Cover.MmToFeet().FeetToPx() - 2 * StirrupDiameter - XDiameter.GetDiameterInFeet().FeetToPx()) / (NumberOfXRebar - 1));
            for (int i = 0; i < NumberOfXRebar; i++)
            {
                EllipseGeometry topxRebarGeometry = new EllipseGeometry();
                topxRebarGeometry.Center =
                    new Point1(x: Origin.X - Width.FeetToPx() / 2 + Cover.MmToFeet().FeetToPx() + StirrupDiameter + i * kcX + XDiameter.GetDiameterInFeet().FeetToPx() / 2,
                       y: Origin.Y - Height.FeetToPx() / 2 + Cover.MmToFeet().FeetToPx() + StirrupDiameter + XDiameter.GetDiameterInFeet().FeetToPx() / 2);
                topxRebarGeometry.RadiusX = XDiameter.GetDiameterInFeet().FeetToPx() / 2;
                topxRebarGeometry.RadiusY = XDiameter.GetDiameterInFeet().FeetToPx() / 2;
                topGeometryGroup.Children.Add(value: topxRebarGeometry);
                EllipseGeometry botxRebarGeometry = new EllipseGeometry();
                botxRebarGeometry.Center =
                    new Point1(x: Origin.X - Width.FeetToPx() / 2 + Cover.MmToFeet().FeetToPx() + StirrupDiameter + i * kcX + XDiameter.GetDiameterInFeet().FeetToPx() / 2,
                        y: Origin.Y + Height.FeetToPx() / 2 - Cover.MmToFeet().FeetToPx() - StirrupDiameter - XDiameter.GetDiameterInFeet().FeetToPx() / 2);
                botxRebarGeometry.RadiusX = XDiameter.GetDiameterInFeet().FeetToPx() / 2;
                botxRebarGeometry.RadiusY = XDiameter.GetDiameterInFeet().FeetToPx() / 2;
                botGeometryGroup.Children.Add(value: botxRebarGeometry);
            }

            ProfilebotXRebar = topGeometryGroup;
            ProfiletopXRebar = botGeometryGroup;
            //ve thep phuong Y
            GeometryGroup leftGeometryGroup = new GeometryGroup();
            GeometryGroup rightGeometryGroup = new GeometryGroup();
            double kcY = ((Height.FeetToPx() - 2 * Cover.MmToFeet().FeetToPx() - 2 * StirrupDiameter - YDiameter.GetDiameterInFeet().FeetToPx()) / (NumberOfYRebar - 1));

            if (NumberOfYRebar > 2)
            {
                for (int i = 1; i <= NumberOfYRebar - 2; i++)
                {
                    EllipseGeometry leftyRebarGeometry = new EllipseGeometry();
                    leftyRebarGeometry.Center =
                        new Point1(
                            x: Origin.X - Width.FeetToPx() / 2 + Cover.MmToFeet().FeetToPx() + StirrupDiameter
                               + YDiameter.GetDiameterInFeet().FeetToPx() / 2,
                            y: Origin.Y - Height.FeetToPx() / 2 + Cover.MmToFeet().FeetToPx() + StirrupDiameter +
                               YDiameter.GetDiameterInFeet().FeetToPx() / 2 + i * kcY);
                    leftyRebarGeometry.RadiusX = YDiameter.GetDiameterInFeet().FeetToPx() / 2;
                    leftyRebarGeometry.RadiusY = YDiameter.GetDiameterInFeet().FeetToPx() / 2;
                    topGeometryGroup.Children.Add(value: leftyRebarGeometry);
                    EllipseGeometry rightyRebarGeometry = new EllipseGeometry();
                    rightyRebarGeometry.Center =
                        new Point1(
                            x: Origin.X + Width.FeetToPx() / 2 - Cover.MmToFeet().FeetToPx() - StirrupDiameter
                               - YDiameter.GetDiameterInFeet().FeetToPx() / 2,
                            y: Origin.Y + Height.FeetToPx() / 2 - Cover.MmToFeet().FeetToPx() - StirrupDiameter -
                               YDiameter.GetDiameterInFeet().FeetToPx() / 2 - i * kcY);
                    rightyRebarGeometry.RadiusX = YDiameter.GetDiameterInFeet().FeetToPx() / 2;
                    rightyRebarGeometry.RadiusY = YDiameter.GetDiameterInFeet().FeetToPx() / 2;
                    rightGeometryGroup.Children.Add(value: rightyRebarGeometry);
                }
            }

            ProfilerightYGeometry = rightGeometryGroup;
            ProfileleftYRebar = leftGeometryGroup;
            PathGeometry Geo_Dim = new PathGeometry();
            PathFigure Figure_Dim = new PathFigure();
            Figure_Dim.IsClosed = false;
            Figure_Dim.StartPoint = new Point1(Origin.X - Width.FeetToPx() / 2, Origin.Y + 200);
            PolyLineSegment Segment_Dim = new PolyLineSegment();
            Segment_Dim.Points.Add(new Point1(Origin.X + Width.FeetToPx() / 2, Origin.Y + 200));
            Figure_Dim.Segments.Add(Segment_Dim);
            Geo_Dim.Figures.Add(Figure_Dim);
            DimHor = Geo_Dim;
            TextBlock text = new TextBlock();
            text.Text = Width.ToString();
            text.FontSize = 12;
            text.Foreground = Brushes.Black;
            text.FontFamily = new FontFamily("Tahoma");
            text.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            text.Arrange(new Rect(text.DesiredSize));
            //Dimtext= text.Text;

        }

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

        public Geometry DimHor
        {
            get => _dimHor;
            set
            {
                if (Equals(value, _dimHor)) return;
                _dimHor = value;
                OnPropertyChanged();
            }
        }

        private Geometry _profileGeometry;
        private Geometry _profileStirrup;
        private Geometry _profiletopXRebar;
        private Geometry _profilebotXRebar;
        private Geometry _profileleftYRebar;
        private Geometry _profilerightYGeometry;
        private Geometry _dimHor;
        private string _dimtext;



    }
}
