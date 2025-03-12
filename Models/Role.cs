using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Role
    {
        public int Role_id {  get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<Guest> Guests { get; set; } = new List<Guest>();
    }
}
