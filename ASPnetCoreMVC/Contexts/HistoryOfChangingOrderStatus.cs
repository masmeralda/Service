using System;
using System.Collections.Generic;

namespace ASPnetCoreMVC.Contexts;

public partial class HistoryOfChangingOrderStatus
{
    public int IdHistory { get; set; }

    public string StatusHistory { get; set; } = null!;

    public DateTime? ChangedAt { get; set; }

    public int IdOrder { get; set; }

    public int IdSpecialist { get; set; }

    public virtual Order IdOrderNavigation { get; set; } = null!;

    public virtual Specialist IdSpecialistNavigation { get; set; } = null!;
}
