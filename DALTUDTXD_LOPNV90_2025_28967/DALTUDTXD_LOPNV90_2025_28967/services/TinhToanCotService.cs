using System;

namespace DALTUDTXD_LOPNV90_2025_28967.Services
{
    public class TinhToanCotService
    {
        public (double AsCalc, string caseName) TinhThep(
            double N, double Mx, double My,
            double b, double h, double a,
            double Rb, double Rs, double Es, double Eb,
            double mu_min, double mu_max,double Psi
        )
        {
            // 🔹 BƯỚC 1: CHUYỂN ĐƠN VỊ
            N *= 1000;        // kN → N
            Mx *= 1e6;        // kNm → N·mm
            My *= 1e6;        // kNm → N·mm

            // 🔹 BƯỚC 2: KÍCH THƯỚC TIẾT DIỆN
            double Cx = b; // chiều cao theo phương X
            double Cy = h; // chiều rộng theo phương Y

            // 🔹 TÍNH ξR CHO DEBUG (nếu cần)
            double ξR = 0.8 / (1 + ((Rs / Es)/0.0035)); // công thức gần đúng theo TCXDVN 356:2005


            // 🔹 BƯỚC 4: CHIỀU DÀI TÍNH TOÁN
            double Ψ = Psi;
            double Lox = Ψ * 3300;
            double Loy = Ψ * 3300;

            // 🔹 BƯỚC 3: LỆCH TÂM NGẪU NHIÊN
            double eax = Math.Max(Math.Max(3300.0 / 600.0, Cx / 30.0), 10.0);
            double eay = Math.Max(Math.Max(3300.0 / 600.0, Cy / 30.0), 10.0);

          

            // 🔹 BƯỚC 5: HỆ SỐ UỐN DỌC
            double λx = Lox / (0.288 * Cx);
            double λy = Loy / (0.288 * Cy);


            // 🔹 Khai báo ηx, ηy trước
            double ηx = 1.0;  // mặc định = 1 nếu không bị mất ổn định
            double ηy = 1.0;

            if (λx > 14)
            {
                double Ix = Cy * Math.Pow(Cx, 3) / 12.0;
                double Ncrx = 2.5 * Eb * Ix / (Lox * Lox);
                ηx = 1.0 / (1.0 - N / Ncrx);
            }

            if (λy > 14)
            {
                double Iy = Cx * Math.Pow(Cy, 3) / 12.0;
                double Ncry = 2.5 * Eb * Iy / (Loy * Loy);
                ηy = 1.0 / (1.0 - N / Ncry);
            }

            // 🔹 BƯỚC 6: MOMEN SAU UỐN DỌC
            double Mtx = ηx * Mx;
            double Mty = ηy * My;

            // 🔹 BƯỚC 7: XÁC ĐỊNH PHƯƠNG TÍNH TOÁN
            double ratioX = Mtx / Cx;
            double ratioY = Mty / Cy;

            // 🔸 Khai báo các biến dùng chung (để có thể debug sau if-else)
            double AsCalc = 0;
            string caseName = "";
            double x1 = 0;
            double ho = 0;
            double h_eff = 0;
            double b_eff = 0;
            double e = 0;
            double ε = 0;

            if (ratioX >= ratioY)
            {
                // TÍNH THEO PHƯƠNG X
                h_eff = Cx;        // = h = 250
                b_eff = Cy;        // = b = 300
                ho = h_eff - a;    // = 210
                double z = h_eff - 2 * a; // = 170

                double M1 = Mtx;
                double M2 = Mty;

                x1 = N / (Rb * b_eff); // = 91940 / (11.5 * 300) = 26.65

                double mo = x1 <= ho ? (1 - 0.6 * x1 / ho) : 0.4;
                double M = M1 + mo * M2 * (h_eff / b_eff);

                e = Math.Max(M / N, eax) + 0.5 * h_eff - a;
                ε = e / ho;

                if (ε > 0.3)
                {
                    AsCalc = (N * (e + 0.5 * x1 - ho)) / (0.4 * Rs * z);
                    caseName = "Tính theo phương X (NL lớn)";
                }
                else
                {
                    AsCalc = mu_min * b * h;
                    caseName = "Tính theo phương X (NL bé)";
                }
            }
            else
            {
                // TÍNH THEO PHƯƠNG Y
                h_eff = Cy;        // = b = 300
                b_eff = Cx;        // = h = 250
                ho = h_eff - a;    // = 260
                double z = h_eff - 2 * a; // = 220

                double M1 = Mty;
                double M2 = Mtx;
                x1 = N / (Rb * h); // = 91940 / (11.5 * 250) = 31.98

                double mo = x1 <= ho ? (1 - 0.6 * x1 / ho) : 0.4;
                double M = M1 + mo * M2 * (h_eff / b_eff);

                e = Math.Max(M / N, eay) + 0.5 * h_eff - a;
                ε = e / ho;

                if (ε > 0.3)
                {
                    AsCalc = (N * (e + 0.5 * x1 - ho)) / (0.4 * Rs * z);
                    caseName = "Tính theo phương Y (NL lớn)";
                }
                else
                {
                    AsCalc = mu_min * b * h;
                    caseName = "Tính theo phương Y (NL bé)";
                }
            }


            // 🔹 BƯỚC 8: GIỚI HẠN HÀM LƯỢNG
            double As_min = mu_min * b * h;
            double As_max = mu_max * b * h;

            if (AsCalc < As_min)
            {
                AsCalc = As_min;
                caseName = "Lệch tâm bé (μ_min)";
            }
            else if (AsCalc > As_max)
            {
                AsCalc = As_max;
                caseName = "Lệch tâm lớn (μ_max)";
            }

            return (Math.Round(Math.Max(AsCalc, 0), 2), caseName);
        }
    }
}