using System;
using System.Collections.Generic;

namespace EventsAPI.Models;

public partial class SharedEventsGuest
{
    public int Id { get; set; }

    public Guid? EventId { get; set; }

    public Guid? GuestId { get; set; }

    public virtual Event? Event { get; set; }

    public virtual Guest? Guest { get; set; }
}
