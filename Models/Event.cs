using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Models;

public partial class Event
{
    public Guid EventId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime EventDate { get; set; }

    public string Location { get; set; } = null!;

    public int? CategoryId { get; set; }

    public int GuestLimit { get; set; }

    public string? Image { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<SharedEventsGuest> SharedEventsGuests { get; set; } = new List<SharedEventsGuest>();
}
