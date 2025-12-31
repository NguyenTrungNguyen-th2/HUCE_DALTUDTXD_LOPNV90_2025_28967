using DALTUDTXD_LOPNV90_2025_28967.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DALTUDTXD_LOPNV90_2025_28967.ViewModel
{
    public class TinhToanViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string DisplayName { get; set; } = "";
        public string Level { get; set; } = "";
        public string Width { get; set; } = "";
        public string Height { get; set; } = "";
        public string Length { get; set; } = "";
        public string LienKet { get; set; } = "";
        public double Psi { get; set; }

        private string _chieuRong = "";
        public string ChieuRong
        {
            get => _chieuRong;
            set { _chieuRong = value; OnPropertyChanged(); }
        }

        private string _chieuDai = "";
        public string ChieuDai
        {
            get => _chieuDai;
            set { _chieuDai = value; OnPropertyChanged(); }
        }

        private string _chieuCao = "";
        public string ChieuCao
        {
            get => _chieuCao;
            set { _chieuCao = value; OnPropertyChanged(); }
        }

        private double _taiTrong;
        public double TaiTrong
        {
            get => _taiTrong;
            set { _taiTrong = value; OnPropertyChanged(); }
        }

        private double _momenX;
        public double MomenX
        {
            get => _momenX;
            set { _momenX = value; OnPropertyChanged(); }
        }

        private double _momenY;
        public double MomenY
        {
            get => _momenY;
            set { _momenY = value; OnPropertyChanged(); }
        }

        private string _macBeTong = "";
        public string MacBeTong
        {
            get => _macBeTong;
            set { _macBeTong = value; OnPropertyChanged(); }
        }

        private double _rb;
        public double Rb
        {
            get => _rb;
            set { _rb = value; OnPropertyChanged(); }
        }

        private double _eb;
        public double Eb
        {
            get => _eb;
            set { _eb = value; OnPropertyChanged(); }
        }

        private string _macThep = "";
        public string MacThep
        {
            get => _macThep;
            set { _macThep = value; OnPropertyChanged(); }
        }

        private double _rs;
        public double Rs
        {
            get => _rs;
            set { _rs = value; OnPropertyChanged(); }
        }

        private double _es;
        public double Es
        {
            get => _es;
            set { _es = value; OnPropertyChanged(); }
        }

        private double _asValue;
        public double AsValue
        {
            get => _asValue;
            set { _asValue = value; OnPropertyChanged(); }
        }

        private string _caseName = "";
        public string CaseName
        {
            get => _caseName;
            set { _caseName = value; OnPropertyChanged(); }
        }

        private string _selectedRebarText = "";
        public string SelectedRebarText
        {
            get => _selectedRebarText;
            set { _selectedRebarText = value; OnPropertyChanged(); }
        }

        private string _selectedRebarArea = "";
        public string SelectedRebarArea
        {
            get => _selectedRebarArea;
            set { _selectedRebarArea = value; OnPropertyChanged(); }
        }

        public ICommand cmTinhToan { get; }
        private readonly TinhToanCotService _service;
        private readonly double[] _availableDiameters = {  16, 18, 20, 22, 25, 28 };

        public TinhToanViewModel()
        {
            _service = new TinhToanCotService();
            cmTinhToan = new RelayCommand(_ => TinhToan());
        }
     
        public void TinhToan()
        {
            if (!double.TryParse(ChieuRong, out double b) || b <= 0)
            {
                SetError("Thiếu hoặc sai chiều rộng tiết diện.");
                return;
            }
            if (!double.TryParse(ChieuDai, out double h) || h <= 0)
            {
                SetError("Thiếu hoặc sai chiều cao tiết diện.");
                return;
            }

            double a = 40;
            double mu_min = 0.01;
            double mu_max = 0.03;

            try
            {
                var (AsCalc, caseName) = _service.TinhThep(
                    TaiTrong, MomenX, MomenY,
                    b, h, a,
                    Rb, Rs, Es, Eb,
                    mu_min, mu_max, Psi
                );

                AsValue = Math.Max(AsCalc, 0);
                CaseName = caseName;

                if (AsCalc > 0)
                {
                    var (rebarText, rebarArea) = RecommendRebar(AsCalc);
                    SelectedRebarText = rebarText;
                    SelectedRebarArea = rebarArea.ToString("0.##");
                }
                else
                {
                    SelectedRebarText = "";
                    SelectedRebarArea = "";
                }
            }
            catch (Exception ex)
            {
                SetError($"LỖI: {ex.Message}");
            }
        }

        private (string description, double area) RecommendRebar(double requiredArea)
        {
            (string desc, double area, double excess) best = ("", 0, double.MaxValue);

            foreach (double dia in _availableDiameters)
            {
                double areaPerBar = Math.PI * dia * dia / 4.0;
                int minCount = (int)Math.Ceiling(requiredArea / areaPerBar);
                if (minCount < 4) minCount = 4;
                if (minCount % 2 != 0) minCount++;

                for (int count = minCount; count <= 12; count += 2)
                {
                    double totalArea = count * areaPerBar;
                    double excess = totalArea - requiredArea;

                    if (excess < best.excess ||
                        (Math.Abs(excess - best.excess) < 1e-6 && dia < ExtractDiameter(best.desc)))
                    {
                        best = ($"{count}Φ{dia}", totalArea, excess);
                    }
                }
            }

            if (string.IsNullOrEmpty(best.desc))
            {
                double fallbackArea = 4 * Math.PI * 16 * 16 / 4.0;
                return ("4Φ16", fallbackArea);
            }

            return (best.desc, best.area);
        }

        private double ExtractDiameter(string desc)
        {
            if (string.IsNullOrEmpty(desc)) return double.MaxValue;
            var parts = desc.Split('Φ');
            if (parts.Length < 2) return double.MaxValue;
            string diaPart = parts[1];
            return double.TryParse(diaPart, out double d) ? d : double.MaxValue;
        }

        private void SetError(string message)
        {
            AsValue = 0;
            CaseName = message;
            SelectedRebarText = "";
            SelectedRebarArea = "";
        }
    }
}