using System;
using System.Collections.Generic;

namespace OEMEV.UserService.Data.Models;

public partial class Role
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
