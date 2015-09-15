<Query Kind="Expression">
  <Connection>
    <ID>c90243ac-27fa-4aa8-bd94-4d778034355a</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

//simple where clause

//list all tables that hold more than 3 people
from row in Tables
where row.Capacity > 3
select row

//list all items with more than 500 calories
from row in Items
where row.Calories > 500
select row

//list all items with more than 500 calories and selling for
// more than 10.00
from row in Items
where row.Calories > 500 && 
		row.CurrentPrice > 10.00m
select row

//list all items with more than 500 calories and
//are Entrees on the menu
//HINT: navigational properties of the database and LINQPad knowledge
from row in Items
where row.Calories > 500 && 
		row.MenuCategory.Description.Equals("Entree")
select row


//method syntax
Items.Where(row => (row.Calories > (Int32?)500))