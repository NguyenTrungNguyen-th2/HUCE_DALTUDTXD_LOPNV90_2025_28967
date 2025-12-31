using Autodesk.Revit.UI;
using DALTUDTXD_LOPNV90_2025_28967.ViewModel;

namespace DALTUDTXD_LOPNV90_2025_28967.ViewModel
{
    public class MainViewModel
    {
        public VatLieuViewModel VatLieuVM { get; }
        public TinhToanViewModel TinhToanVM { get; }
        public ColumnViewModel ColumnVM { get; private set; } 
        public ColumnRebarViewModel ColumnRebarVM { get; private set; } 

        public MainViewModel(
            UIDocument uiDoc,
            VatLieuViewModel vatLieuVM = null,
            TinhToanViewModel tinhToanVM = null)
        {
            VatLieuVM = vatLieuVM ?? new VatLieuViewModel();
            TinhToanVM = tinhToanVM ?? new TinhToanViewModel();
            ColumnVM = new ColumnViewModel(uiDoc, TinhToanVM);

            if (uiDoc != null)
            {
                ColumnVM = new ColumnViewModel(uiDoc, TinhToanVM);

                var doc = uiDoc.Document;
                if (doc != null)
                {
                }
            }
        }
    }
}