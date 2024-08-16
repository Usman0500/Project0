using System;
using System.Collections.Generic;

namespace Project0.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? AccNo { get; set; }

    public DateTime? TransactionDate { get; set; }

    public string? TransactionType { get; set; }

    public double? TransactionAmount { get; set; }

    public virtual AccountInfo? AccNoNavigation { get; set; }
}
