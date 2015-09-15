<Query Kind="Expression">
  <Connection>
    <ID>c90243ac-27fa-4aa8-bd94-4d778034355a</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

//anonymous type
from food in Items
where food.MenuCategory.Description.Equals("Entree") &&
		food.Active
orderby food.CurrentPrice descending
select new
		{
			Description = food.Description,
			Price = food.CurrentPrice,
			Cost = food.CurrentCost,
			Profit = food.CurrentPrice - food.CurrentCost
		}


from food in Items
where food.MenuCategory.Description.Equals("Entree") &&
		food.Active
orderby food.CurrentPrice descending
select new
		{
			food.Description,
			food.CurrentPrice,
			food.CurrentCost,
			//Profit = food.CurrentPrice - food.CurrentCost
		}