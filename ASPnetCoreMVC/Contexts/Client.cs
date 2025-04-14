using System;
using System.Collections.Generic;

namespace ASPnetCoreMVC.Contexts;

public partial class Client
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

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
