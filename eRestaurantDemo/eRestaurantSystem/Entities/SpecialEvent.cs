using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
#endregion 

namespace eRestaurantSystem.Entities
{
    public class SpecialEvent
    {
        [Key]
        [Required(ErrorMessage="An Event Code is required (only one character)")]
        [StringLength(1, ErrorMessage="Event Code can only use a single-character code")]
        public string EventCode { get; set; }

        [Required(ErrorMessage = "A Description is required (5-30 characters)")]
        [StringLength(30, MinimumLength=5, ErrorMessage="Description must be 5-30 characters in length")]
        public string Description { get; set; }

        public bool Active { get; set; }

        //Navigation virtual property
        public virtual ICollection<Reservation> Reservations { get; set; }

        //all classes can have their own constructor
        //constructor can contain initialization values
 
        public SpecialEvent()
        {
            Active = true;
        }


    }
}
