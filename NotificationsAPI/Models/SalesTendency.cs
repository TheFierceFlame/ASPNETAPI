using System;
using System.Collections.Generic;

namespace NotificationsAPI.Models;

public partial class SalesTendency
{
    public int Id { get; set; }

    public string Category { get; set; } = null!;

    public int Monthid { get; set; }

    public decimal Tendency { get; set; }

    public DateTime Calculationdate { get; set; }
}
