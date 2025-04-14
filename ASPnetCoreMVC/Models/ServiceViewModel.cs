namespace ASPnetCoreMVC.Models
{
    public class ServiceViewModel
    {
        public int IdService { get; set; }
        public string NameService { get; set; } = null!;
        public decimal PriceService { get; set; }
        public TimeSpan? ExecutionTimeService { get; set; }

    }
}
