using System;
using System.Collections.Generic;

namespace ASPnetCoreMVC.Contexts;

public partial class Service
{
    public int IdService { get; set; }

    public string NameService { get; set; } = null!;

    public decimal PriceService { get; set; }

    public TimeSpan? ExecutionTimeService { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
