<Query Kind="Expression">
  <Connection>
    <ID>c90243ac-27fa-4aa8-bd94-4d778034355a</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

//orderby

//default ascending
from food in Items
orderby food.Description
select food

//descending
from food in Items
orderby food.Description descending
select food

//descending
from food in Items
orderby food.Description descending, food .Calories ascending
select food

//descending * you can switch order of orderby and where clause
from food in Items
orderby food.Description descending, food .Calories ascending
where food.MenuCategory.Description.Equals("Entree")
select food
