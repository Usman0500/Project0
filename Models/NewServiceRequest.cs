using System;
using System.Collections.Generic;

namespace Project0.Models;

public partial class NewServiceRequest
{
    public int RequestId { get; set; }

    public int? AccNo { get; set; }

    public string ServiceType { get; set; } = null!;

    public string RequestStatus { get; set; } = null!;

    public DateTime? RequestDate { get; set; }

    public virtual AccountInfo? AccNoNavigation { get; set; }
}
