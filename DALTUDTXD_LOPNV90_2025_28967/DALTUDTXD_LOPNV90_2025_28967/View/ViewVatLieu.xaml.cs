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
using DALTUDTXD_LOPNV90_2025_28967.Cmd;

namespace DALTUDTXD_LOPNV90_2025_28967.View
{

    public partial class ViewVatLieu : Window
    {
        public ViewVatLieu()
        {
            InitializeComponent();
            var vm = new VatLieuViewModel();
            vm.OnSaveRequested += () => this.Close();
            DataContext = vm;
        }
    }
}
