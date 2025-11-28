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
           // CHUYỂN ĐƠN VỊ
            N *= 1000;      
            Mx *= 1e6;        
            My *= 1e6;        

            // KÍCH THƯỚC TIẾT DIỆN
            double Cx = b; 
            double Cy = h; 

            // TÍNH ξR 
            double ER = 0.8 / (1 + ((Rs / Es)/0.0035)); 


            // CHIỀU DÀI TÍNH TOÁN
            double Ψ = Psi;
            double Lox = Ψ * 3300;
            double Loy = Ψ * 3300;

            // LỆCH TÂM NGẪU NHIÊN
            double eax = Math.Max(Math.Max(Lox / 600.0, Cx / 30.0), 10.0);
            double eay = Math.Max(Math.Max(Loy / 600.0, Cy / 30.0), 10.0);

          

            //HỆ SỐ UỐN DỌC
            double λx = Lox / (0.288 * Cx);
            double λy = Loy / (0.288 * Cy);


            // Khai báo ηx, ηy
            double ηx = 1.0;  
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

            //MOMEN SAU UỐN DỌC
            double Mtx = ηx * Mx;
            double Mty = ηy * My;

            //XÁC ĐỊNH PHƯƠNG TÍNH TOÁN
            double MCX = Mtx / Cx;
            double MCY = Mty / Cy;

            // Khai báo các biến
            double AsCalc = 0;
            string caseName = "";
            double x1 = 0;
            double ho = 0;
            double h1 = 0;
            double b1 = 0;
            double e = 0;
            double ε = 0;

            if (MCX >= MCY)
            {
                // TÍNH THEO PHƯƠNG X
                h1 = Cx;     
                b1 = Cy;        
                ho = h1 - a;   
                double z = h1 - 2 * a; 

                double M1 = Mtx;
                double M2 = Mty;

                x1 = N / (Rb * b1);

                double mo = x1 <= ho ? (1 - 0.6 * x1 / ho) : 0.4;
                double M = M1 + mo * M2 * (h1 / b1);

                e = Math.Max(M / N, eax) + 0.5 * h1 - a;
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
                h1 = Cy;       
                b1 = Cx;      
                ho = h1 - a;    
                double z = h1 - 2 * a; 

                double M1 = Mty;
                double M2 = Mtx;
                x1 = N / (Rb * h); 

                double mo = x1 <= ho ? (1 - 0.6 * x1 / ho) : 0.4;
                double M = M1 + mo * M2 * (h1 / b1);

                e = Math.Max(M / N, eay) + 0.5 * h1 - a;
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


            //GIỚI HẠN HÀM LƯỢNG
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