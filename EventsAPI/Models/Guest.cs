using System;
using System.Collections.Generic;

namespace EventsAPI.Models;

public partial class Guest
{
    public Guid GuestId { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public string Email { get; set; } = null!;

    public virtual ICollection<SharedEventsGuest> SharedEventsGuests { get; set; } = new List<SharedEventsGuest>();
}
