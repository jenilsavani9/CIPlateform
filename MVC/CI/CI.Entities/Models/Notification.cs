using System;
using System.Collections.Generic;

namespace CI.Entities.Models;

public partial class Notification
{
    public long NotificationId { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
