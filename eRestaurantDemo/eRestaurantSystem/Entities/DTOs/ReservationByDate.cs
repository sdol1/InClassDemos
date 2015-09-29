using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using System.Collections; //required by IEnumerable
#endregion 

namespace eRestaurantSystem.Entities.DTOs
{
    public class ReservationByDate
    {
        public string Description { get; set; }
        //the rest of the data will be a collection of POCO rows
        //the actual POCO will be defined in the LINQ query
        public IEnumerable Reservations { get; set; }
    }
}
