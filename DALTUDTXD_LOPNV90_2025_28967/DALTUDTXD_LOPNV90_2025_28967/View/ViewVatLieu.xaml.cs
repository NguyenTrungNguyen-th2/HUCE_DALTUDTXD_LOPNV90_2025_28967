using DALTUDTXD_LOPNV90_2025_28967.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DALTUDTXD_LOPNV90_2025_28967.View
{
    /// <summary>
    /// Interaction logic for ViewVatLieu.xaml
    /// </summary>
    public partial class ViewVatLieu : Window
    {
        public ViewVatLieu()
        {
            InitializeComponent();
            DataContext = new VatLieuViewModel();
        }

        // ViewVatLieu.xaml.cs
        private void ChuyenSangCot_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            // Lấy UIApplication từ RevitCommandData (bạn cần gán nó trong CmdVatLieu)
            var uiApp = RevitCommandData.UiApp;
            if (uiApp != null)
            {
                CmdCot.OpenCotView(uiApp);
            }
            else
            {
                MessageBox.Show("Không thể mở form Cột vì không có kết nối Revit.");
            }
        }
    }
}
