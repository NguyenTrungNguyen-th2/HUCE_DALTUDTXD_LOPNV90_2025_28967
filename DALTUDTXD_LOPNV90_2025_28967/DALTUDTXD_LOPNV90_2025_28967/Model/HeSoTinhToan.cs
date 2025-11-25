using System;

namespace DALTUDTXD_LOPNV90_2025_28967.Model
{
    /// <summary>
    /// Class chứa các hệ số tính toán theo TCVN cho cột bê tông cốt thép
    /// </summary>
    public static class HeSoTinhToan
    {
        /// <summary>
        /// Hệ số điều kiện làm việc của bê tông khi chịu nén lệch tâm (Ψ)
        /// </summary>
        public const double Psi = 0.7;

        /// <summary>
        /// Hàm lượng cốt thép tối thiểu (μ_min) - 1%
        /// </summary>
        public const double MuMin = 0.01; // 1%

        /// <summary>
        /// Hàm lượng cốt thép tối đa (μ_max) - 3%
        /// </summary>
        public const double MuMax = 0.03; // 3%

        /// <summary>
        /// Hệ số ξR (giới hạn vùng nén) - có thể thay đổi theo vật liệu
        /// </summary>
        /// <param name="Rs">Cường độ chịu kéo thép (MPa)</param>
        /// <param name="Es">Modun đàn hồi thép (MPa)</param>
        /// <returns>ξR</returns>
        public static double TinhXiR(double Rs, double Es)
        {
            const double eps_b2 = 0.0035; // Biến dạng cực hạn bê tông
            const double XiR_const = 0.8;  // Hệ số theo TCVN

            double eps_s_el = Rs / Es;
            return XiR_const / (1 + eps_s_el / eps_b2);
        }
    }
}