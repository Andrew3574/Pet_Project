using System;
using System.Collections.Generic;

namespace Models;

public partial class Guest
{
    public Guid GuestId { get; set; } 

    public int Role_id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public string Email { get; set; } = null!;

    public virtual Role? Role {  get; set; } 

    public virtual ICollection<SharedEventsGuest> SharedEventsGuests { get; set; } = new List<SharedEventsGuest>();
}
