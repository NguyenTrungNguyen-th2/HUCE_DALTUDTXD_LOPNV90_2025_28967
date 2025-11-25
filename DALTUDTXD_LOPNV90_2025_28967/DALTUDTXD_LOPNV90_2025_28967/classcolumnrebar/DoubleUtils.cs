using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALTUDTXD_LOPNV90_2025_28967.ClassColumnRebar
{
    public static class DoubleUtils
    {  // Chuyển từ feet (đơn vị Revit) sang mét
        public static double FeetToMet(this double feet) => feet * 0.3048;

        // Chuyển từ feet sang milimét
        public static double FeetToMm(this double feet) => feet * 304.8;
        public static double pxtofeet(this double px) => px * 304.8;

        // Chuyển từ mét sang feet
        public static double MetToFeet(this double met) => met * 3.28084;

        // Chuyển từ milimét sang feet
        public static double MmToFeet(this double mm) => mm / 304.8;

        // (Tùy chọn) Chuyển từ int feet → double met/mm

        public static double MmToFeet(this int mm) => mm / 304.8;
        public static double FeetToPx(this double feet) => feet * 304.8 * 0.3;
        public static double MmToPx(this double mm) => mm * 0.4;
        public static double PxToPx(this int px) => px / 9.5;
        public static double toCover1(this int cv) => cv /400;
    }
}
