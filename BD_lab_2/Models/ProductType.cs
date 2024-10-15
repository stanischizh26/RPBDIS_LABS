using System;
using System.Collections.Generic;

namespace BD_lab_2.Models;

public partial class ProductType
{
    public int ProductTypeId { get; set; }

    public string Name { get; set; } = null!;

    public int ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;
}
