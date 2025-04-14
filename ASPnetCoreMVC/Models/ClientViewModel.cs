namespace ASPnetCoreMVC.Models
{
    public class ClientViewModel
    {
        public int IdClient { get; set; }
        public string LastNameClient { get; set; } = null!;
        public string FirstNameClient { get; set; } = null!;
        public string? PatronymicClient { get; set; }
        public string? PhoneClient { get; set; }
        public string? EmailClient { get; set; }
        public string? StreetClient { get; set; }
        public string? HouseClient { get; set; }
        public string? ApartmentNumberClient { get; set; }


        // Дополнительное свойство для отображения полного имени клиента
        public string FullName => $"{LastNameClient} {FirstNameClient} {PatronymicClient}";
    }
}
