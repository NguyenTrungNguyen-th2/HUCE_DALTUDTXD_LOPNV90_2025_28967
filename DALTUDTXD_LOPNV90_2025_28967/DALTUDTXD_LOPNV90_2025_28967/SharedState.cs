// SharedState.cs
using DALTUDTXD_LOPNV90_2025_28967.Model;
using DALTUDTXD_LOPNV90_2025_28967.ViewModel;

namespace DALTUDTXD_LOPNV90_2025_28967
{
    public static class SharedState
    {
        public static MaterialModel CurrentMaterial { get; set; }
        public static MainViewModel MainVM { get; set; } // Tùy chọn: lưu toàn bộ trạng thái app
    }
}