using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DALTUDTXD_LOPNV90_2025_28967.Model
{
    public class ColumnModel : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Length { get; set; }
        public string Level { get; set; }
        public string ConcreteGrade { get; set; }
        public string ComboDisplay { get; set; } = "—";
        private string _lienKet = "—";
        public string LienKet
        {
            get => _lienKet;
            set { _lienKet = value; OnPropertyChanged(); }
        }

        private double _psi;
        public double Psi
        {
            get => _psi;
            set
            {
                _psi = value;
                PsiDisplay = (System.Math.Abs(value) < 1e-6) ? "—" : value.ToString("0.00");
                OnPropertyChanged();
            }
        }

        private string _psiDisplay = "—";
        public string PsiDisplay
        {
            get => _psiDisplay;
            set { _psiDisplay = value; OnPropertyChanged(); }
        }

        public double LoadValue { get; set; }
        public double MomentXValue { get; set; }
        public double MomentYValue { get; set; }

        private string _load = "—";
        public string Load
        {
            get => _load;
            set { _load = value; OnPropertyChanged(); }
        }

        private string _momentX = "—";
        public string MomentX
        {
            get => _momentX;
            set { _momentX = value; OnPropertyChanged(); }
        }

        private string _momentY = "—";
        public string MomentY
        {
            get => _momentY;
            set { _momentY = value; OnPropertyChanged(); }
        }

        private string _asTinhToan = "—";
        public string AsTinhToan
        {
            get => _asTinhToan;
            set { _asTinhToan = value; OnPropertyChanged(); }
        }

        private string _chonThep = "—";
        public string ChonThep
        {
            get => _chonThep;
            set { _chonThep = value; OnPropertyChanged(); }
        }

        private string _asThucTe = "—";
        public string AsThucTe
        {
            get => _asThucTe;
            set { _asThucTe = value; OnPropertyChanged(); }
        }

        private string _trangThaiTinhToan = "Chưa tính";
        public string TrangThaiTinhToan
        {
            get => _trangThaiTinhToan;
            set { _trangThaiTinhToan = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}