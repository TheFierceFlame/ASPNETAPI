using System;
using System.Collections.Generic;

namespace NotificationsAPI.Models;

public partial class ProductsSale
{
    public int Id { get; set; }

    public string Category { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public decimal UnitaryPrice { get; set; }

    public int Quantity { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime Date { get; set; }
}
