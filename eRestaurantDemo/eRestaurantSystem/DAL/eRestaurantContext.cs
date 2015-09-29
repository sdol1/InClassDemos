using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using eRestaurantSystem.Entities;
using System.Data.Entity;
#endregion

namespace eRestaurantSystem.DAL
{
    //this class should be restricted to access by only
    //the BLL methods
    //this classshould NOT be accessable outside of the
    //component library

    internal class eRestaurantContext : DbContext
    {
        public eRestaurantContext() : base ("name=EatIn")
        {
            //constructor is used to pass web config string name
        }

        //setup the DBSet Mappings
        //One Dbset for each entity I create.
        public DbSet<SpecialEvent> SpecialEvents { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<Table> Table { get; set; }

        //added the rest of the entities
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillItem> BillItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<Waiter> Waiters { get; set; }

        //When overriding ONModelCreating(), it is important to remember
        //to call the base method's implementation before you exit the method

        //The ManyToManyNavigationPropertyConfiguration.Map method  lets you
        //configure the tables and column used for many-to-many relationships
        //it takes a ManyToManyNavigationPropertyConfiguration instance in which
        //you specify the column names b calling the MapLeftKey, MapRightKey,
        //and ToTbake Methods.

        //The "left" key is the one specified in the HasMany method
        //the "right" key is the one specified in the WIthMany method

        //we have a many to many relationship between reservation and tables
        //a reservation may need many tables.
        //a table can have over time many reservations.

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Reservation>().HasMany(r => r.Tables)
                .WithMany(x => x.Reservations)
                .Map(mapping =>
                {
                    mapping.ToTable("ReservationTables");
                    mapping.MapLeftKey("TableID");
                    mapping.MapRightKey("ReservationID");
                });
            base.OnModelCreating(modelBuilder);
        }
    }
}
