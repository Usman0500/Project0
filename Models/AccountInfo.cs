using System;
using System.Collections.Generic;

namespace Project0.Models;

public partial class AccountInfo
{
    public int AccNo { get; set; }

    public string? AccName { get; set; }

    public string? AccType { get; set; }

    public double? AccBalance { get; set; }

    public bool? AccIsActive { get; set; }

    public string? AccUsername { get; set; }

    public virtual User? AccUsernameNavigation { get; set; }

    public virtual ICollection<NewServiceRequest> NewServiceRequests { get; set; } = new List<NewServiceRequest>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
