using System;

namespace DALTUDTXD_LOPNV90_2025_28967.Services
{
    public class TinhToanCotService
    {
        public (double AsCalc, string caseName) TinhThep(
            double N, double Mx, double My,
            double b, double h, double a,
            double Rb, double Rs, double Es, double Eb,
            double mu_min, double mu_max, double Psi
        )
        {
          
            N *= 1000;      
            Mx *= 1e6;      
            My *= 1e6;      

            double Cx = b;  
            double Cy = h;  

            double Esel = Rs / Es;   
            double Eb2 = 0.0035;     
            double ER = 0.8 / (1 + Esel / Eb2);

            double Lox = Psi * 3300;   
            double Loy = Psi * 3300;   

            double eax = Math.Max(Math.Max(Lox / 600.0, Cx / 30.0), 10.0); 
            double eay = Math.Max(Math.Max(Loy / 600.0, Cy / 30.0), 10.0); 

            double λx = Lox / (0.288 * Cx);
            double λy = Loy / (0.288 * Cy);

            double nx = 1.0;
            double ny = 1.0;

            if (λx > 14)
            {
                double Ix = Cy * Math.Pow(Cx, 3) / 12.0;
                double Ncrx = 2.5 * Eb * Ix / (Lox * Lox);
                nx = 1.0 / (1.0 - N / Ncrx);
            }

            if (λy > 14)
            {
                double Iy = Cx * Math.Pow(Cy, 3) / 12.0;
                double Ncry = 2.5 * Eb * Iy / (Loy * Loy);
                ny = 1.0 / (1.0 - N / Ncry);
            }

            double Mtx = nx * Mx;
            double Mty = ny * My;

            double MCX = Mtx / Cx;
            double MCY = Mty / Cy;

            double AsCalc = 0;
            string caseName = "";

            if (MCX >= MCY)
            {
                double h1 = Cx;    
                double b1 = Cy;     
                double ho = h1 - a; 
                double z = h1 - 2 * a;

                double M1 = Mtx;    
                double M2 = Mty;    

                double x1 = N / (Rb * b1);

                double mo = (x1 <= ho) ? (1 - 0.6 * x1 / ho) : 0.4;

                double M = M1 + mo * M2 * (h1 / b1);

                double e1 = M / N;
                double e0 = Math.Max(e1, eax);

                double e = e0 + 0.5 * h1 - a;

                double ε = e / ho;

                if (ε <= 0.3)
                {
                    double λ = Math.Max(λx, λy); 
                    double φ; 
                    if (λ <= 14)
                        φ = 1.0;
                    else if (λ < 104)
                        φ = 1.028 - 0.0000288 * λ * λ - 0.0016 * λ;
                    else
                        φ = 0.9; 

                    double γe = 1.0 / ((0.5 - ε) * (2 + ε)); 
                    double φe = φ + (1 - φ) * ε / 0.3;

                    double AsMinByFormula = (γe * N / φe) - (Rb * b1 * h1 / (Rs - Rb));
                    AsCalc = Math.Max(AsMinByFormula, mu_min * b * h);

                    caseName = "NL rất bé (ε ≤ 0.3)";
                }
                else if (x1 > ER * ho)
                {
                    double numerator = N * e0 - Rb * b1 * x1 * (ho - x1 / 2);
                    double denominator = 0.4 * Rs * z;
                    AsCalc = numerator / denominator;

                    AsCalc = Math.Max(AsCalc, mu_min * b * h);
                    caseName = "NL bé (ε > 0.3 & x1 > ξR*ho)";
                }
                else
                {
                    AsCalc = (N * (e + 0.5 * x1 - ho)) / (0.4 * Rs * z);
                    caseName = "NL lớn (ε > 0.3 & x1 ≤ ξR*ho)";
                }
            }
            else
            {
               
                double h1 = Cy;   
                double b1 = Cx;    
                double ho = h1 - a;
                double z = h1 - 2 * a; 
                double M1 = Mty;    
                double M2 = Mtx;   

                double x1 = N / (Rb * b1); 

                double mo = (x1 <= ho) ? (1 - 0.6 * x1 / ho) : 0.4;

                double M = M1 + mo * M2 * (h1 / b1);

                double e1 = M / N;
                double e0 = Math.Max(e1, eay);

                double e = e0 + 0.5 * h1 - a;

                double ε = e / ho;

                if (ε <= 0.3)
                {
                    double λ = Math.Max(λx, λy);
                    double φ;
                    if (λ <= 14)
                        φ = 1.0;
                    else if (λ < 104)
                        φ = 1.028 - 0.0000288 * λ * λ - 0.0016 * λ;
                    else
                        φ = 0.9;

                    double γe = 1.0 / ((0.5 - ε) * (2 + ε));
                    double φe = φ + (1 - φ) * ε / 0.3;

                    double AsMinByFormula = (γe * N / φe) - (Rb * b1 * h1 / (Rs - Rb));
                    AsCalc = Math.Max(AsMinByFormula, mu_min * b * h);

                    caseName = "NL rất bé (ε ≤ 0.3)";
                }
                else if (x1 > ER * ho)
                {
                    double numerator = N * e0 - Rb * b1 * x1 * (ho - x1 / 2);
                    double denominator = 0.4 * Rs * z;
                    AsCalc = numerator / denominator;

                    AsCalc = Math.Max(AsCalc, mu_min * b * h);
                    caseName = "NL bé (ε > 0.3 & x1 > ER*ho)";
                }
                else
                {
                    AsCalc = (N * (e + 0.5 * x1 - ho)) / (0.4 * Rs * z);
                    caseName = "NL lớn (ε > 0.3 & x1 ≤ ER*ho)";
                }
            }

            double As_min = mu_min * b * h;
            double As_max = mu_max * b * h;
                
            if (AsCalc < As_min)
            {
                AsCalc = As_min;
                caseName = "Giới hạn μ_min";
            }
            else if (AsCalc > As_max)
            {
                AsCalc = As_max;
                caseName = "Giới hạn μ_max";
            }

            return (Math.Round(Math.Max(AsCalc, 0), 2), caseName);
        }
    }
}