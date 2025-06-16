namespace Scholarship_Distribution_Management_System.Models.ViewModel
{
    public class HoatDongCuaSinhVienViewModel
    {
        public string IDDon { get; set; }
        public string IDHoatDong { get; set; }
        public string IDSinhVien { get; set; }
        public string TenHoatDong { get; set; }
        public bool DaDuyet { get; set; }
        
        // New properties
        public bool AllActivitiesApproved { get; set; }
        public bool AnyActivityApproved { get; set; }
        public List<string> ActivityNames { get; set; } = new List<string>();
    }
}
