using System;
using System.Collections.Generic;

namespace OEMEV.UserService.Domain;

public partial class User
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? FullName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public DateTime CreatedAt { get; set; }

    public long RoleId { get; set; }

    public long? ServiceCenterId { get; set; }

    public string? RefreshToken { get; set; }

    public bool IsActive { get; set; }

    public long? ManufacturerId { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual ServiceCenter? ServiceCenter { get; set; }
}
