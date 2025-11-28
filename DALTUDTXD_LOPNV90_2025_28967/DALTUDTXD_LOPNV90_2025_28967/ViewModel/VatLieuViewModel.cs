using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DALTUDTXD_LOPNV90_2025_28967.Model;

namespace DALTUDTXD_LOPNV90_2025_28967.ViewModel
{
    public class VatLieuViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event Action OnSaveRequested;
        private void OnProp([CallerMemberName] string n = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
        public ObservableCollection<string> ConcreteGrades { get; } =
            new ObservableCollection<string> { "B15", "B20", "B25", "B30", "B35", "B40" };

        public ObservableCollection<string> SteelGrades { get; } =
            new ObservableCollection<string> { "CI", "CII", "CIII", "CIV" };

        private string _selectedConcrete;
        public string SelectedConcrete
        {
            get => _selectedConcrete;
            set
            {
                if (_selectedConcrete != value)
                {
                    _selectedConcrete = value;
                    OnProp();
                    UpdateConcrete();
                }
            }
        }

        private string _selectedSteel;
        public string SelectedSteel
        {
            get => _selectedSteel;
            set
            {
                if (_selectedSteel != value)
                {
                    _selectedSteel = value;
                    OnProp();
                    UpdateSteel();
                }
            }
        }

        private double _rb;
        public double Rb { get => _rb; set { _rb = value; OnProp(); } }

        private double _eb;
        public double Eb { get => _eb; set { _eb = value; OnProp(); } }

        private double _rs;
        public double Rs { get => _rs; set { _rs = value; OnProp(); } }

        private double _es;
        public double Es { get => _es; set { _es = value; OnProp(); } }

        public ObservableCollection<MaterialModel> clsVatLieu { get; } = new ObservableCollection<MaterialModel>();

        private MaterialModel _selectedMaterial;
        public MaterialModel SelectedMaterial
        {
            get => _selectedMaterial;
            set
            {
                _selectedMaterial = value;
                OnProp();
                ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand DeleteCommand { get; }

        public VatLieuViewModel()
        {
            SaveCommand = new RelayCommand(_ => SaveMaterial());
            ClearCommand = new RelayCommand(_ => ClearInputs());
            DeleteCommand = new RelayCommand(_ => DeleteMaterial(), _ => SelectedMaterial != null);

            // Khởi tạo mặc định
            SelectedConcrete = "B20";
            SelectedSteel = "CII";

            // 🔥 Thêm 2 dòng này để hiển thị giá trị ngay khi mở form
            UpdateConcrete();
            UpdateSteel();
        }


        private void UpdateConcrete()
        {
            if (string.IsNullOrEmpty(SelectedConcrete)) return;

            switch (SelectedConcrete)
            {
                case "B15": Rb = 8.5; Eb = 24000; break;
                case "B20": Rb = 11.5; Eb = 27000; break;
                case "B25": Rb = 14.5; Eb = 30000; break;
                case "B30": Rb = 17.0; Eb = 32500; break;
                case "B35": Rb = 19.5; Eb = 34000; break;
                case "B40": Rb = 22.0; Eb = 36000; break;
                default: Rb = 0; Eb = 0; break;
            }
        }

        private void UpdateSteel()
        {
            if (string.IsNullOrEmpty(SelectedSteel)) return;

            switch (SelectedSteel)
            {
                case "CI": Rs = 210; Es = 210000; break;
                case "CII": Rs = 280; Es = 210000; break;
                case "CIII": Rs = 350; Es = 210000; break;
                case "CIV": Rs = 435; Es = 210000; break;
                default: Rs = 0; Es = 0; break;
            }
        }

        private void SaveMaterial()
        {
            var m = new MaterialModel
            {
                ConcreteGrade = SelectedConcrete,
                Rb = Rb,
                Eb = Eb,
                SteelGrade = SelectedSteel,
                Rs = Rs,
                Es = Es
            };

            SharedState.CurrentMaterial = m; // ✅ Lưu vào nơi chung

            OnSaveRequested?.Invoke(); // Đóng cửa sổ
        }

        private void ClearInputs()
        {
            clsVatLieu.Clear();
            SelectedConcrete = ConcreteGrades.Count > 0 ? ConcreteGrades[0] : null;
            SelectedSteel = SteelGrades.Count > 0 ? SteelGrades[0] : null;
        }

        private void DeleteMaterial()
        {
            if (SelectedMaterial != null)
            {
                clsVatLieu.Remove(SelectedMaterial);
                SelectedMaterial = null;
            }
        }
        public void LoadFromMaterial(MaterialModel material)
        {
            if (material == null) return;

            SelectedConcrete = material.ConcreteGrade;
            Rb = material.Rb;
            Eb = material.Eb;
            SelectedSteel = material.SteelGrade;
            Rs = material.Rs;
            Es = material.Es;
        }
    }
}
