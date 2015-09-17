<Query Kind="Program">
  <Connection>
    <ID>c90243ac-27fa-4aa8-bd94-4d778034355a</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

void Main()
{
	//entity data set
	Waiter.Dump();
	//anonymous type
	//from food in Items
	//where food.MenuCategory.Description.Equals("Entree") &&
	//		food.Active
	//orderby food.CurrentPrice descending
	//select new
	//		{
	//			Description = food.Description,
	//			Price = food.CurrentPrice,
	//			Cost = food.CurrentCost,
	//			Profit = food.CurrentPrice - food.CurrentCost
	//		}
	//
	//
	//from food in Items
	//where food.MenuCategory.Description.Equals("Entree") &&
	//		food.Active
	//orderby food.CurrentPrice descending
	//select new
	//		{
	//			food.Description,
	//			food.CurrentPrice,
	//			food.CurrentCost,
	//			//Profit = food.CurrentPrice - food.CurrentCost
	//		}
	
	var results = from food in Items
		where food.MenuCategory.Description.Equals("Entree") &&
				food.Active
		orderby food.CurrentPrice descending
		select new FoodMargin()
				{
					Description = food.Description,
					Price = food.CurrentPrice,
					Cost = food.CurrentCost,
					Profit = food.CurrentPrice - food.CurrentCost
				};
	results.Dump();
	
	//DTO data transfer objects orders = many items
	var results2 = from orders in Bills
		where orders.PaidStatus &&
				(orders.BillDate.Month == 9 && orders.BillDate.Year == 2014)
		orderby orders.Waiter.LastName, orders.Waiter.FirstName
		select new BillOrders()
				{
					BillID = orders.BillID,
					Waiter = orders.Waiter.LastName + " " + orders.Waiter.FirstName,
					Orders = orders.BillItems
				};
	results2.Dump();
	
}

// Define other methods and classes here
// Sample of a POCO type class: flat data set no structures
public class FoodMargin
{
	public string Description{get;set;}
	public decimal Price {get;set;}
	public decimal Cost {get;set;}
	public decimal Profit {get;set;}
}

// Sample of a POCO type class: flat data set with possible structures
//DTO sample
public class BillOrders
{
	public int BillID{get;set;}
	public string Waiter{get;set;}
	public IEnumerable Orders{get;set;}
}

