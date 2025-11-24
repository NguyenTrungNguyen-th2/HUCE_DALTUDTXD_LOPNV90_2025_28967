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
            var pushButtonData = new PushButtonData("Vat lieu", "Vật liệu", Path, "DALTUDTXD_LOPNV90_2025_28967.Cmd");
            PushButton pushButton1=  Panelthongso.AddItem(pushButtonData) as PushButton;
            pushButton1.LargeImage= new BitmapImage(new Uri(
                "pack://application:,,,/DALTUDTXD_LOPNV90_2025_28967;component/resources/IconRibbonRevit/MaterialEditor.png"));






            return Result.Succeeded;

        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Cancelled;
        }
    }
}
