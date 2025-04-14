namespace ASPnetCoreMVC.Models
{
    public class HistoryOfChangingOrderStatusViewModel
    {
        public int IdHistory { get; set; }
        public string StatusHistory { get; set; } = null!;
        public DateTime? ChangedAt { get; set; }
        public int IdOrder { get; set; }
        public int IdSpecialist { get; set; }

    }
}
