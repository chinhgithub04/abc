using System.Collections.Generic;

namespace Scholarship_Distribution_Management_System.Models.ViewModel
{
    public class ActivityDetailViewModel
    {
        public string IDDon { get; set; }
        public string IDSinhVien { get; set; }
        public string StudentName { get; set; }
        public string StudentClass { get; set; }
        public string ScholarshipName { get; set; }
        public bool IsDuyetHoatDong { get; set; }        public List<ActivityItem> RegisteredActivities { get; set; } = new List<ActivityItem>();
        public List<ActivityItem> ApprovedActivities { get; set; } = new List<ActivityItem>();
        public List<ActivityItem> PendingActivities { get; set; } = new List<ActivityItem>();
        
        public bool AllActivitiesApproved => PendingActivities.Count == 0;
    }
    
    public class ActivityItem
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }
}
