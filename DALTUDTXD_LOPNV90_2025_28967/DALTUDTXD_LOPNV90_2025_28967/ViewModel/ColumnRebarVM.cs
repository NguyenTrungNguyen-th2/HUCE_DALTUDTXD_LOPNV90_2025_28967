using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using DALTUDTXD_LOPNV90_2025_28967.ClassColumnRebar;
using DALTUDTXD_LOPNV90_2025_28967.View;

namespace DALTUDTXD_LOPNV90_2025_28967.ViewModel
{
    public class ColumnRebarVM:ObservableObject
    {   
        private RebarBarType _xDiameter;
        private RebarBarType _yDiameter;
        private List<RebarBarType> _diameters;
        private Document doc;
        private UIDocument uiDoc;

        public ViewVeThep viewvethep { get; set; }
        public RebarBarType XDiameter
        {
            get => _xDiameter;
            set
            {
                if (Equals(value, _xDiameter)) return;
                _xDiameter = value;
                OnPropertyChanged();
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
            }
        }

        public List<RebarBarType> Diameters
        {
            get => _diameters;
            set
            {
                if (Equals(value, _diameters)) return;
                _diameters = value;
                OnPropertyChanged();
            }
        }

        public ColumnRebarVM(Document doc, UIDocument uiDoc,ColumnViewModel columnViewModel)
        {
            this.doc= doc;
            this.uiDoc = uiDoc;
            GetData();
        }
        void GetData()
        {
            Diameters = new FilteredElementCollector(doc).OfClass(typeof(RebarBarType))
                .Cast<RebarBarType>()
                .OrderBy(x => x.Name)
                .ToList();
            XDiameter = Diameters.FirstOrDefault(x => x.BarNominalDiameter.FeetToMm() > 20);
            YDiameter = Diameters.FirstOrDefault(x => x.BarNominalDiameter.FeetToMm() > 20);


        }


    }
}
