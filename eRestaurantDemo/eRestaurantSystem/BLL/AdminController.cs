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
using System.Data.Entity; 
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

        #region POCO for Reports
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

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<WaiterBilling> GetWaiterBillingReport()
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                var results = from aBillRow in context.Bills
                                where aBillRow.BillDate.Month == 5
                                orderby aBillRow.BillDate, aBillRow.Waiter.LastName, aBillRow.Waiter.FirstName
                                select new WaiterBilling // <--- name of the entity from the POCO
                                {
	                                BillDate = aBillRow.BillDate,
	                                Name = aBillRow.Waiter.LastName + " " + aBillRow.Waiter.FirstName,
	                                BillID = aBillRow.BillID,
	                                BillTotal = aBillRow.Items.Sum(bitem => bitem.Quantity * bitem.SalePrice),
	                                PartySize = aBillRow.NumberInParty,
	                                Contact = aBillRow.Reservation.CustomerName
                                }; // <----- semi colon

                return results.ToList();
            }
        }
        #endregion

        #region Front Desk
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public DateTime GetLastBillDateTime()
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                var result = context.Bills.Max(x => x.BillDate);
                return result;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<ReservationCollection> ReservationsByTime(DateTime date)
        {
            using (var context = new eRestaurantContext())
            {
                var result = (from data in context.Reservations
                              where data.ReservationDate.Year == date.Year
                              && data.ReservationDate.Month == date.Month
                              && data.ReservationDate.Day == date.Day
                                  // && data.ReservationDate.Hour == timeSlot.Hours
                              && data.ReservationStatus == Reservation.Booked
                              select new ReservationSummary()
                              {
                                  ID = data.ReservationID,
                                  Name = data.CustomerName,
                                  Date = data.ReservationDate,
                                  NumberInParty = data.NumberInParty,
                                  Status = data.ReservationStatus,
                                  Event = data.Event.Description,
                                  Contact = data.ContactPhone
                              }).ToList();

                // the second part of this method uses the results of the
                // first linq query
                // Linq to Entity will only execute the query when it deems 
                // necessary for having the results in memory
                //
                // to get your query to execute and have the resulting data
                // inside memory for further use, you can attach the .ToList()
                // to previous query

                // note: the second the query is NOT using an Entity
                // it is using the results from a previous query

                //itemGroup is a temporary in memory data collection
                // this collection can be used in selecting your final
                //data collection.
                var finalResult = from item in result
                                  //orderby item.NumberInParty
                                  group item by item.Date.Hour into itemGroup
                                  select new ReservationCollection()
                                  {
                                      Hour = itemGroup.Key,
                                      Reservations = itemGroup.ToList()
                                  };
                return finalResult.OrderBy(x => x.Hour).ToList();
            }
        }
        #endregion

        #region
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<SeatingSummary> SeatingByDateTime(DateTime date, TimeSpan time)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                // place your query(ies) here

                // Step 1 - Get the table info along with any walk-in bills and reservation bills for the specific time slot
                var step1 = from data in context.Tables
                             select new
                             {
                                Table = data.TableNumber,
                                Seating = data.Capacity,
                                // This sub-query gets the bills for walk-in customers
                                WalkIns = from walkIn in data.Bills
                                        where 
                                               walkIn.BillDate.Year == date.Year
                                            && walkIn.BillDate.Month == date.Month
                                            && walkIn.BillDate.Day == date.Day
                                            //&& walkIn.BillDate.TimeOfDay <= time
                                            && DbFunctions.CreateTime(walkIn.BillDate.Hour,
                                                                      walkIn.BillDate.Minute,
                                                                      walkIn.BillDate.Second) <= time
                                            && (!walkIn.OrderPaid.HasValue || walkIn.OrderPaid.Value >= time)
                //                          && (!walkIn.PaidStatus || walkIn.OrderPaid >= time)
                                        select walkIn,
                                 // This sub-query gets the bills for reservations
                                 Reservations = from booking in data.Reservations
                                        from reservationParty in booking.Bills
                                        where 
                                               reservationParty.BillDate.Year == date.Year
                                            && reservationParty.BillDate.Month == date.Month
                                            && reservationParty.BillDate.Day == date.Day
                                            //&& reservationParty.BillDate.TimeOfDay <= time
                                            && DbFunctions.CreateTime(reservationParty.BillDate.Hour,
                                                                      reservationParty.BillDate.Minute,
                                                                      reservationParty.BillDate.Second) <= time
                                            && (!reservationParty.OrderPaid.HasValue || reservationParty.OrderPaid.Value >= time)
                //                          && (!reservationParty.PaidStatus || reservationParty.OrderPaid >= time)
                                        select reservationParty
                             };

                // Step 2 - Union the walk-in bills and the reservation bills while extracting the relevant bill info
                // .ToList() helps resolve the "Types in Union or Concat are constructed incompatibly" error
                var step2 = from data in step1.ToList() // .ToList() forces the first result set to be in memory
                            select new
                            {
                                Table = data.Table,
                                Seating = data.Seating,
                                CommonBilling = from info in data.WalkIns.Union(data.Reservations)
                                                select new // info
                                                {
                                                    BillID = info.BillID,
                                                    BillTotal = info.Items.Sum(bi => bi.Quantity * bi.SalePrice),
                                                    Waiter = info.Waiter.FirstName,
                                                    Reservation = info.Reservation
                                                }
                            };
                //step2.Dump();

                // Step 3 - Get just the first CommonBilling item
                //         (presumes no overlaps can occur - i.e., two groups at the same table at the same time)
                var step3 = from data in step2.ToList()
                            select new
                            {
                                Table = data.Table,
                                Seating = data.Seating,
                                Taken = data.CommonBilling.Count() > 0,
                                // .FirstOrDefault() is effectively "flattening" my collection of 1 item into a 
                                // single object whose properties I can get in step 4 using the dot (.) operator
                                CommonBilling = data.CommonBilling.FirstOrDefault()
                            };
                //step3.Dump();

                // Step 4 - Build our intended seating summary info
                var step4 = from data in step3
                            select new SeatingSummary() // the DTO class to use in my BLL
                            {
                                Table = data.Table,
                                Seating = data.Seating,
                                Taken = data.Taken,
                                // use a ternary expression to conditionally get the bill id (if it exists)
                                BillID = data.Taken ?               // if(data.Taken)
                                         data.CommonBilling.BillID  // value to use if true
                                       : (int?) null,               // value to use if false
                                BillTotal = data.Taken ? 
                                            data.CommonBilling.BillTotal : (decimal?) null,
                                Waiter = data.Taken ? data.CommonBilling.Waiter : (string) null,
                                ReservationName = data.Taken ?
                                                  (data.CommonBilling.Reservation != null ?
                                                   data.CommonBilling.Reservation.CustomerName : (string) null)
                                                : (string) null
                            };
                
                
                return step4.ToList();
            }

        }

        /// <summary>
        /// ListWaiters returns the id and full name of all the current waiters
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<WaiterOnDuty> ListWaiters()
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                var result = from person in context.Waiters
                             where person.ReleaseDate == null
                             select new WaiterOnDuty()
                             {
                                 WaiterId = person.WaiterID,
                                 FullName = person.FirstName + " " + person.LastName
                             };
                return result.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<SeatingSummary> AvailableSeatingByDateTime(DateTime date, TimeSpan time)
        {
            // This query is not going directly to the entities
            // this query is using the List<> from another method as its data source
            var results = from seats in SeatingByDateTime(date, time)
                          where !seats.Taken
                          select seats;

            return results.ToList();
        }

        // the command from the code behind is NOT using an ODS
        // therefore it does NOT need [DataObjectMethod]

        public void SeatCustomer(DateTime when, byte tablenumber, int numberinparty, int waiterid)
        {
            // business logic checking should be done before any database processing
            // rule 1: table must be available 
            // rule 2: table size must be greater than or = to numberinparty

            //get the available seats
            var availableseats = AvailableSeatingByDateTime(when.Date, when.TimeOfDay);

            //start the transaction
            using (eRestaurantContext context = new eRestaurantContext())
            {
                // create a list<> to hold error messages
                List<string> errors = new List<string>();

                if(!availableseats.Exists(foreachseat => foreachseat.Table == tablenumber))
                {
                    // table is not available
                    errors.Add("Table is currently not available");
                }
                if(!availableseats.Exists(foreachseat => foreachseat.Table == tablenumber
                                            && foreachseat.Seating >= numberinparty))
                {
                    errors.Add("Insufficient seating capacity for number of customer");
                }
                
                // check of validation
                if(errors.Count > 0)
                {
                    //there is a business rule exception
                    throw new BusinessRuleException("Unable to seat customer", errors);
                }

                // we can assume that the data is valid at this point
                // in our system as soon as a customer is seated a Bill is started
                Bill seatcustomer = new Bill();

                seatcustomer.BillDate = when;
                seatcustomer.NumberInParty = numberinparty;
                seatcustomer.WaiterID = waiterid;
                seatcustomer.TableID = tablenumber;
                seatcustomer.PaidStatus = false;

                //make the request to entityframework to add a record to the database
                context.Bills.Add(seatcustomer);

                //commit 
                context.SaveChanges();

            } // end of transaction
        }
        #endregion

    }//oef class
}//eof namespace
