using DALTUDTXD_LOPNV90_2025_28967.Services;
using System;
using System.ComponentModel;
using System.Linq;
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

        // ---- Thông số tiết diện ----
        private string _tenCot = "";
        public string TenCot
        {
            get => _tenCot;
            set { _tenCot = value; OnPropertyChanged(); }
        }

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

        // ---- Tải trọng / Momen ----
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

        // ---- Vật liệu ----
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

        // ---- Kết quả ----
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

        // ===============================
        //         COMMAND
        // ===============================
        public ICommand cmTinhToan { get; }

        // ===============================
        //         SERVICE
        // ===============================
        private readonly TinhToanCotService _service;

        // ===============================
        //       CONSTRUCTOR
        // ===============================
        public TinhToanViewModel()
        {
            _service = new TinhToanCotService();
            cmTinhToan = new RelayCommand(_ => TinhToan());

        }
        // Danh sách đường kính thép có sẵn (mm)
        private readonly double[] _availableDiameters = { 12, 14, 16, 18, 20, 22, 25, 28 };

        private (string description, double area) RecommendRebar(double requiredArea)
        {
            (string desc, double area, double excess) best = ("", 0, double.MaxValue);

            foreach (double dia in _availableDiameters)
            {
                double areaPerBar = Math.PI * dia * dia / 4;

                // Tính số thanh tối thiểu cần thiết
                int minCount = (int)Math.Ceiling(requiredArea / areaPerBar);

                // Đảm bảo số thanh là chẵn và ≥ 4
                if (minCount < 4) minCount = 4;
                if (minCount % 2 != 0) minCount++; // làm tròn lên số chẵn gần nhất

                // Duyệt các số chẵn từ minCount đến 12
                for (int count = minCount; count <= 12; count += 2)
                {
                    double totalArea = count * areaPerBar;
                    double excess = totalArea - requiredArea;

                    // Chọn tổ hợp tốt nhất:
                    // - Chênh lệch nhỏ nhất
                    // - Nếu bằng nhau → ưu tiên đường kính nhỏ hơn
                    if (excess < best.excess ||
                        (Math.Abs(excess - best.excess) < 1e-6 && dia < ExtractDiameter(best.desc)))
                    {
                        best = ($"{count}Φ{dia}", totalArea, excess);
                    }
                }
            }

            // Fallback an toàn
            if (string.IsNullOrEmpty(best.desc))
            {
                return ("4Φ16", 4 * Math.PI * 16 * 16 / 4); // ≈ 804 mm²
            }

            return (best.desc, best.area);
        }

        // Hàm trích đường kính (tương thích C# 7.3)
        private double ExtractDiameter(string desc)
        {
            if (string.IsNullOrEmpty(desc)) return double.MaxValue;
            var parts = desc.Split('Φ');
            if (parts.Length == 0) return double.MaxValue;
            string lastPart = parts[parts.Length - 1];
            return double.TryParse(lastPart, out double d) ? d : double.MaxValue;
        }
        // ===============================
        //         HÀM TÍNH TOÁN
        // ===============================
        private void TinhToan()
        {
            if (!double.TryParse(ChieuRong, out double b) || b <= 0) return;
            if (!double.TryParse(ChieuDai, out double h) || h <= 0) return;

            double a = 40;
            double mu_min = 0.01;
            double mu_max = 0.03;

            try
            {
                var (AsCalc, caseName) = _service.TinhThep(
                    TaiTrong, MomenX, MomenY,
                    b, h, a,
                    Rb, Rs, Es, Eb,
                    mu_min, mu_max
                );

                AsValue = Math.Max(AsCalc, 0);
                CaseName = caseName;

                // 🔥 TỰ ĐỘNG ĐỀ XUẤT THÉP SAU KHI CÓ As
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
                AsValue = 0;
                CaseName = $"LỖI: {ex.Message}";
                SelectedRebarText = "";
                SelectedRebarArea = "";
            }
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

    }
}