namespace DALTUDTXD_LOPNV90_2025_28967.Model
{
    public class ColumnModel
    {
        public string Id { get; set; }
        public string Name { get; set; }           
        public string Width { get; set; }          
        public string Height { get; set; }         
        public string Length { get; set; }        
        public string Level { get; set; }          
        public string Load { get; set; }
        public string MomentX { get; set; }
        public string MomentY { get; set; }
        public string ConcreteGrade { get; set; }
        public string LienKet { get; set; }
        public double Psi { get; set; }

        public string AsTinhToan { get; set; } = "—";
        public string ChonThep { get; set; } = "—";
        public string AsThucTe { get; set; } = "—";
        public string TrangThaiTinhToan { get; set; } = "Chưa tính";
    }

}