<Query Kind="Expression">
  <Connection>
    <ID>c90243ac-27fa-4aa8-bd94-4d778034355a</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

//grouping
from food in Items
group food by food.MenuCategory.Description

//requires the creation of an anonymous type 
from food in Items
group food by new {food.MenuCategory.Description, food.CurrentPrice}