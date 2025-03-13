using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models;

public partial class Event
{
    public Guid EventId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime EventDate { get; set; }

    public string Location { get; set; } = null!;

    public int? CategoryId { get; set; }

    public int GuestLimit { get; set; }

    public string? Image { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<SharedEventsGuest> SharedEventsGuests { get; set; } = new List<SharedEventsGuest>();
}
