using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using eRestaurantSystem.Entities;
using eRestaurantSystem.DAL;
using System.ComponentModel; // use for ODS access
using eRestaurantSystem.Entities.DTOs;
using eRestaurantSystem.Entities.POCOs; 
#endregion

namespace eRestaurantSystem.BLL
{
    [DataObject]
    public class AdminController
    {
        #region Query Samples
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<SpecialEvent> SpecialEvent_List()
        {
            using (var context = new eRestaurantContext())
            {
                //retreieve the data from the SpecialEvents Table sql
                //to do so we will use te DbSet in eResturantContext
                //      call SpecialEvents (done by mapping)

                //method syntax
                //return context.SpecialEvents.OrderBy(x => x.Description).ToList();

                //query syntax
                var results = from item in context.SpecialEvents
                              orderby item.Description
                              select item;
                return results.ToList();

            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Reservation> GetReservationsByEventCode(string eventcode)
        {
            using (var context = new eRestaurantContext())
            {
                //retreieve the data from the SpecialEvents Table sql
                //to do so we will use te DbSet in eResturantContext
                //      call SpecialEvents (done by mapping)

                //method syntax
                //return context.SpecialEvents.OrderBy(x => x.Description).ToList();

                //query syntax
                var results = from item in context.Reservations
                              where item.EventCode.Equals(eventcode)
                              orderby item.CustomerName, item.ReservationDate
                              select item;
                return results.ToList();

            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<ReservationByDate> GetReservationsByDate(string reservationDate)
        {
            using (var context = new eRestaurantContext())
            {
                // remember LINQ does not like using DateTime casting
                int theYear = (DateTime.Parse(reservationDate)).Year;
                int theMonth = (DateTime.Parse(reservationDate)).Month;
                int theDay = (DateTime.Parse(reservationDate)).Day;

                //query status
                var results = from item in context.SpecialEvents
                              orderby item.Description
                              select new ReservationByDate()
                              {
                                  Description = item.Description,
                                  Reservations = from row in item.Reservations //collection of navigated rows of ICollection in SpecialEvent entity
                                                 where row.ReservationDate.Year == theYear
                                                 && row.ReservationDate.Month == theMonth
                                                 && row.ReservationDate.Day == theDay
                                                 select new ReservationDetail() //POCO 
                                                 {
                                                     CustomerName = row.CustomerName,
                                                     ReservationDate = row.ReservationDate,
                                                     NumberInParty = row.NumberInParty,
                                                     ContactPhone = row.ContactPhone,
                                                     ReservationStatus = row.ReservationStatus
                                                 }
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Entities.DTOs.CategoryMenuItems> CategoryMenuItems_List()
        {
            using (var context = new eRestaurantContext())
            {
                //query status
                var results = from category in context.MenuCategories
                              orderby category.Description
                              select new Entities.DTOs.CategoryMenuItems()
                              {
                                  Description = category.Description,
                                  MenuItems = from row in category.MenuItems //collection of navigated rows of ICollection in SpecialEvent entity
                                                 select new MenuItem() //POCO 
                                                 {
                                                     Description = row.Description,
                                                     Price = row.CurrentPrice,
                                                     Calories = row.Calories,
                                                     Comment = row.Comment
                                                 }
                              };
                return results.ToList();
            }


        }

        //Waiter List
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Waiter> Waiters_List()
        {
            using (var context = new eRestaurantContext())
            {
                //retreieve the data from the SpecialEvents Table sql
                //to do so we will use te DbSet in eResturantContext
                //      call SpecialEvents (done by mapping)

                //method syntax
                //return context.SpecialEvents.OrderBy(x => x.Description).ToList();

                //query syntax
                var results = from waiter in context.Waiters
                              orderby waiter.LastName + " " + waiter.FirstName
                              select waiter;
                return results.ToList();

            }
        }

        //Waiter Search
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Waiter GetWaiterByID(int waiterid)
        {
            using (var context = new eRestaurantContext())
            {
                //retreieve the data from the SpecialEvents Table sql
                //to do so we will use te DbSet in eResturantContext
                //      call SpecialEvents (done by mapping)

                //method syntax
                //return context.SpecialEvents.OrderBy(x => x.Description).ToList();

                //query syntax
                var result = from waiter in context.Waiters
                              where waiter.WaiterID == waiterid
                              select waiter;
                return result.FirstOrDefault();

            }
        }
        #endregion

        #region CRUD Insert, Update, Delete for CQRS
        [DataObjectMethod(DataObjectMethodType.Insert,false)]
        public void SpecialEvents_Add(SpecialEvent item)
        {
            //input into this method is at the instance level
            using (eRestaurantContext context = new eRestaurantContext())
            {
                //create a pointer variable for the instance type 
                //set this pointer to null
                SpecialEvent added = null;

                //set up the add request for the dbContext
                added = context.SpecialEvents.Add(item);
                
                //saving the chnages will cause the .Add to execute
                //commits the add to the database
                //evaluates the annotations (validation) on your entity
                context.SaveChanges();
            }
        }
        
        [DataObjectMethod(DataObjectMethodType.Update,false)]
        public void SpecialEvents_Update(SpecialEvent item)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                context.Entry<SpecialEvent>(context.SpecialEvents.Attach(item)).State =
                    System.Data.Entity.EntityState.Modified;

                context.SaveChanges();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete,false)]
        public void SpecialEvent_Delete(SpecialEvent item)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                //look  the item instance on the databse to detemine if the instance exists
                //on the delete ensure your reference the Primary Key
                SpecialEvent existing = context.SpecialEvents.Find(item.EventCode);

                //set up the delete request command
                context.SpecialEvents.Remove(existing);
                //commit the action to happen
                context.SaveChanges();
            }
        }

        //Waiter CRUD SHOULD BE WAITER_ADD
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public int Waiters_Add(Waiter item)
        {
            //input into this method is at the instance level
            using (eRestaurantContext context = new eRestaurantContext())
            {
                //create a pointer variable for the instance type 
                //set this pointer to null
                Waiter added = null;

                //set up the add request for the dbContext
                added = context.Waiters.Add(item);

                //saving the chnages will cause the .Add to execute
                //commits the add to the database
                //evaluates the annotations (validation) on your entity
                context.SaveChanges();
                // added contains the data of the newly added waiter
                // including the pkey value.
                return added.WaiterID;
            }
        }

        // SHOULD BE WAITER_UPDATE
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void Waiters_Update(Waiter item)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                context.Entry<Waiter>(context.Waiters.Attach(item)).State =
                    System.Data.Entity.EntityState.Modified;

                context.SaveChanges();
            }
        }

        // SHOULD BE WAITER_DELETE
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void Waiters_Delete(Waiter item)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                //look  the item instance on the databse to detemine if the instance exists
                //on the delete ensure your reference the Primary Key
                Waiter existing = context.Waiters.Find(item.WaiterID);

                //set up the delete request command
                context.Waiters.Remove(existing);
                //commit the action to happen
                context.SaveChanges();
            }
        }
        #endregion

        #region POCO for Report1
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Entities.POCOs.CategoryMenuItems> GetReportCategoryMenuItems()
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                var results = from cat in context.Items
                              orderby cat.Category.Description, cat.Description
                              select new Entities.POCOs.CategoryMenuItems
                              {
                                  CategoryDescription = cat.Category.Description,
                                  ItemDescription = cat.Description,
                                  Price = cat.CurrentPrice,
                                  Calories = cat.Calories,
                                  Comment = cat.Comment
                              };

                return results.ToList(); // this was .Dump() in Linqpad
            }
        }
        #endregion

    }//oef class
}//eof namespace
