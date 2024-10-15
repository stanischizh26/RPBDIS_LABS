﻿using System;
using System.Collections.Generic;

namespace BD_lab_2.Models;

public partial class ViewProductsComparison
{
    public string EnterpriseName { get; set; } = null!;

    public string ProductTypeName { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string? ProductCharacteristics { get; set; }

    public byte[]? Photo { get; set; }
}
