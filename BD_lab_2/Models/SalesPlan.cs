using System;
using System.Collections.Generic;

namespace BD_lab_2.Models;

public partial class SalesPlan
{
    public int SalesPlanId { get; set; }

    public int EnterpriseId { get; set; }

    public int ProductId { get; set; }

    public int PlannedSales { get; set; }

    public int ActualSales { get; set; }

    public byte Quarter { get; set; }

    public int Year { get; set; }

    public virtual Enterprise Enterprise { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
