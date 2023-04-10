using System;
using System.Collections.Generic;

namespace CI.Entities.Models;

public partial class ContactU
{
    public int ContactId { get; set; }

    public string? Name { get; set; }

    public string Email { get; set; } = null!;

    public string? Subject { get; set; }

    public string? Message { get; set; }
}
