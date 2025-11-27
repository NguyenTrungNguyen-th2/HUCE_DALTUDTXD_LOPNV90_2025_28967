using Autodesk.Revit.UI;
using DALTUDTXD_LOPNV90_2025_28967.ViewModel;

namespace DALTUDTXD_LOPNV90_2025_28967.ViewModel
{
    public class MainViewModel
    {
        public VatLieuViewModel VatLieuVM { get; }
        public TinhToanViewModel TinhToanVM { get; }
        public ColumnViewModel ColumnVM { get; private set; } // ✅ private set
        public ColumnRebarViewModel ColumnRebarVM { get; private set; } // ✅ private set

        // Constructor nhận các dependency từ ngoài
        public MainViewModel(
            UIDocument uiDoc,
            VatLieuViewModel vatLieuVM = null,
            TinhToanViewModel tinhToanVM = null)
        {
            VatLieuVM = vatLieuVM ?? new VatLieuViewModel();
            TinhToanVM = tinhToanVM ?? new TinhToanViewModel();
            ColumnVM = new ColumnViewModel(uiDoc, TinhToanVM);

            // ✅ Gán ColumnVM nếu có uiDoc
            if (uiDoc != null)
            {
                ColumnVM = new ColumnViewModel(uiDoc, TinhToanVM);

                var doc = uiDoc.Document;
                if (doc != null)
                {
                    ColumnRebarVM = new ColumnRebarViewModel(doc, uiDoc, this);
                }
            }
        }
    }
}