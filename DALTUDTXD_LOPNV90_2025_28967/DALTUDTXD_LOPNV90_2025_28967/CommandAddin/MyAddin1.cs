using Autodesk.Revit.UI;
using Nice3point.Revit.Toolkit.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;


namespace DALTUDTXD_LOPNV90_2025_28967.CommandAddin
{
    public class MyAddin1:IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            application.CreateRibbonTab("My addin");

            var Panelthongso = application.CreateRibbonPanel("My addin", "Thông số đầu vào");
            var Paneltinhtoan = application.CreateRibbonPanel("My addin", "Tính toán");
            var Panelvethep = application.CreateRibbonPanel("My addin", "Vẽ thép");

            var Path = Assembly.GetExecutingAssembly().Location;

            // Nút 1
            var pushButtonData1 = new PushButtonData("VatLieu1", "Vật liệu 1", Path, "DALTUDTXD_LOPNV90_2025_28967.Cmd");
            PushButton btn1 = Panelthongso.AddItem(pushButtonData1) as PushButton;
            btn1.LargeImage = new BitmapImage(new Uri(
                "pack://application:,,,/DALTUDTXD_LOPNV90_2025_28967;component/resources/IconRibbonRevit/MaterialEditor.png"));

            // Nút 2
            var pushButtonData2 = new PushButtonData("VatLieu2", "Vật liệu 2", Path, "DALTUDTXD_LOPNV90_2025_28967.Cmd");
            PushButton btn2 = Panelthongso.AddItem(pushButtonData2) as PushButton;
            btn2.LargeImage = new BitmapImage(new Uri(
                "pack://application:,,,/DALTUDTXD_LOPNV90_2025_28967;component/resources/IconRibbonRevit/MaterialEditor.png"));

            // Nút 3
            var pushButtonData3 = new PushButtonData("VatLieu3", "Vật liệu 3", Path, "DALTUDTXD_LOPNV90_2025_28967.Cmd");
            PushButton btn3 = Panelthongso.AddItem(pushButtonData3) as PushButton;
            btn3.LargeImage = new BitmapImage(new Uri(
                "pack://application:,,,/DALTUDTXD_LOPNV90_2025_28967;component/resources/IconRibbonRevit/MaterialEditor.png"));

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Cancelled;
        }
    }
}
