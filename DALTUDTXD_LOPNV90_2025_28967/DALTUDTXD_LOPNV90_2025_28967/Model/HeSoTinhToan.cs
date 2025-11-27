using System;
using System.Collections.Generic;

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
        public static double Psi { get; set; } = 1.0;

        /// <summary>
        /// Bảng hệ số Psi theo loại liên kết
        /// </summary>
        public static readonly Dictionary<string, double> PsiTheoLienKet
            = new Dictionary<string, double>()
            {
                { "Ngàm ngàm", 0.5 },
                { "Ngàm khớp", 0.7 },
                { "Khớp khớp", 1.0 },
                { "Ngàm", 2.0 },
                { "Khung cứng", 1.3 },
                { "Khung khớp", 1.5 },
            };

        /// <summary>
        /// Hàm lượng cốt thép tối thiểu (μ_min) - 1%
        /// </summary>
        public const double MuMin = 0.01;

        /// <summary>
        /// Hàm lượng cốt thép tối đa (μ_max) - 3%
        /// </summary>
        public const double MuMax = 0.03;

        /// <summary>
        /// Hệ số ξR (giới hạn vùng nén)
        /// </summary>
        public static double TinhXiR(double Rs, double Es)
        {
            const double eps_b2 = 0.0035;
            const double XiR_const = 0.8;

            double eps_s_el = Rs / Es;
            return XiR_const / (1 + eps_s_el / eps_b2);
        }
    }
}