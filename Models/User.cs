using System;
using System.Collections.Generic;

namespace Project0.Models;

public partial class User
{
    public string Username { get; set; } = null!;

    public string? Password { get; set; }

    public string? UserRole { get; set; }

    public virtual ICollection<AccountInfo> AccountInfos { get; set; } = new List<AccountInfo>();
}
