using System;
using System.Collections.Generic;

namespace DALTUDTXD_LOPNV90_2025_28967.Model
{
    public static class HeSoTinhToan
    {
        public static double Psi { get; set; } = 1.0;

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
        public const double MuMin = 0.01;

        public const double MuMax = 0.03;

        public static double TinhXiR(double Rs, double Es)
        {
            const double eps_b2 = 0.0035;
            const double XiR_const = 0.8;

            double eps_s_el = Rs / Es;
            return XiR_const / (1 + eps_s_el / eps_b2);
        }
    }
}