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
    /// Interaction logic for ViewThongSoCot.xaml
    /// </summary>
    public partial class ViewThongSoCot : Window
    {
        // Constructor dùng trong Revit (bắt buộc)
        public ViewThongSoCot(ColumnViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel; // ✅ OK
        }

    }
}
