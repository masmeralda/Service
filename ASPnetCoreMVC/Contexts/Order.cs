using System;
using System.Collections.Generic;

namespace ASPnetCoreMVC.Contexts;

public partial class Order
{
    public int IdOrder { get; set; }

    public string StatusOrder { get; set; } = null!;

    public DateTime? CreationDateOrder { get; set; }

    public DateTime? CompletionDateOrder { get; set; }

    public string? CommentOrder { get; set; }

    public int? IdSpecialist { get; set; }

    public int IdClient { get; set; }

    public int IdTechnic { get; set; }

    public int IdService { get; set; }

    public virtual ICollection<HistoryOfChangingOrderStatus> HistoryOfChangingOrderStatuses { get; set; } = new List<HistoryOfChangingOrderStatus>();

    public virtual Client IdClientNavigation { get; set; } = null!;

    public virtual Service IdServiceNavigation { get; set; } = null!;

    public virtual Specialist? IdSpecialistNavigation { get; set; }

    public virtual Technic IdTechnicNavigation { get; set; } = null!;
}
