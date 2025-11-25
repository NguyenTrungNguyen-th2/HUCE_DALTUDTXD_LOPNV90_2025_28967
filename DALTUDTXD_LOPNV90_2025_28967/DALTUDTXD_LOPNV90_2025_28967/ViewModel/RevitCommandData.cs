using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace DALTUDTXD_LOPNV90_2025_28967.ViewModel
{
    public static class RevitCommandData
    {
        public static UIApplication UiApp { get; set; }
        public static UIDocument UiDoc => UiApp?.ActiveUIDocument;
        public static Document Doc => UiDoc?.Document;
    }
}