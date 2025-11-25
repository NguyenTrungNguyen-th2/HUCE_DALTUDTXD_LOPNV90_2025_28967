using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace DALTUDTXD_LOPNV90_2025_28967.Model
{
    public class ColumnRebarModel

    {

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
        public Point Origin { get; set; } //tọa độ gốc

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

        }
    }
}
