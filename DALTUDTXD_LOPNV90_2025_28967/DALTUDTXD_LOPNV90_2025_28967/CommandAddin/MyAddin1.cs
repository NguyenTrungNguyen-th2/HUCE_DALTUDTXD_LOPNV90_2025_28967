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
            application.CreateRibbonTab("addin");
            var Panelthongso = application.CreateRibbonPanel("addin", "Thông số đầu vào");

            var Path = Assembly.GetExecutingAssembly().Location;

            var pushButtonData1 = new PushButtonData("VatLieu1", "Vật Liệu", Path, "DALTUDTXD_LOPNV90_2025_28967.Cmd.CmdVatLieu");
            PushButton btn1 = Panelthongso.AddItem(pushButtonData1) as PushButton;
            btn1.LargeImage = new BitmapImage(new Uri(
                "pack://application:,,,/DALTUDTXD_LOPNV90_2025_28967;component/resources/IconRibbonRevit/MaterialEditor.png"));

            var pushButtonData2 = new PushButtonData("VatLieu2", "Cột", Path, "DALTUDTXD_LOPNV90_2025_28967.Cmd.CmdCot");
            PushButton btn2 = Panelthongso.AddItem(pushButtonData2) as PushButton;
            btn2.LargeImage = new BitmapImage(new Uri(
                "pack://application:,,,/DALTUDTXD_LOPNV90_2025_28967;component/resources/IconRibbonRevit/Column.png"));


            var Paneltinhtoan = application.CreateRibbonPanel("addin", "Tính Toán");

            var Path1 = Assembly.GetExecutingAssembly().Location;
            var pushButtonData3 = new PushButtonData("VatLieu3", "Tinh toán", Path1, "DALTUDTXD_LOPNV90_2025_28967.Cmd.CmdTinhToan");
            PushButton btn3 = Paneltinhtoan.AddItem(pushButtonData3) as PushButton;
            btn3.LargeImage = new BitmapImage(new Uri(
                "pack://application:,,,/DALTUDTXD_LOPNV90_2025_28967;component/resources/IconRibbonRevit/Calculator.png"));

            #region DatRegion
            var Panelvethep = application.CreateRibbonPanel("addin", "Vẽ thép");
            var pushButtonDataDraw = new PushButtonData("ThongSoVe", "Thông số vẽ", Path, "DALTUDTXD_LOPNV90_2025_28967.Cmd.CmdThongSoVe");
            PushButton pushButtonDraw = Panelvethep.AddItem(pushButtonDataDraw) as PushButton;
            pushButtonDraw.LargeImage = new BitmapImage(new Uri(
                "pack://application:,,,/DALTUDTXD_LOPNV90_2025_28967;component/resources/IconRibbonRevit/Draw.png"));
            var pushButtonDataColumn = new PushButtonData("Ve thep", "Chọn thép", Path, "DALTUDTXD_LOPNV90_2025_28967.Cmd.CmdChonThep");
            PushButton pushButtonColumn = Panelvethep.AddItem(pushButtonDataColumn) as PushButton;
            pushButtonColumn.LargeImage = new BitmapImage(new Uri(
                "pack://application:,,,/DALTUDTXD_LOPNV90_2025_28967;component/resources/IconRibbonRevit/concrete.png"));



            #endregion







            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Cancelled;
        }
    }
}
