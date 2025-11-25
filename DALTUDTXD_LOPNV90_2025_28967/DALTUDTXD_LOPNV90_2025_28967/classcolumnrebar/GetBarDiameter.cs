using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALTUDTXD_LOPNV90_2025_28967.ClassColumnRebar
{
    public static class GetBarDiameter
    {
        public static double GetDiameterInFeet(this RebarBarType barType)
        {
            return barType?.BarNominalDiameter ?? 0.0;
        }
    }
}
