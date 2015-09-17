<Query Kind="Expression">
  <Connection>
    <ID>c90243ac-27fa-4aa8-bd94-4d778034355a</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

//Count Example
from category in MenuCategories
select new
{
    Category = category.Description,
    Items = category.Items.Count()
}

//Count Example using select statement *bad way
from category in MenuCategories
select new
{
    Category = category.Description,
    Items = (from x in category.Items
				select x).Count()
}

//Sum Example 1
(from theBill in BillItems
where theBill.BillID == 104
select theBill.SalePrice * theBill.Quantity).Sum()

//Sum Example 2 using method syntax
BillItems
    .Where (theBill => theBill.BillID == 104)
    .Select(theBill => theBill.SalePrice * theBill.Quantity)
    .Sum()

//Sum Example 3 mix of method and query syntax
(from customer in Bills
where customer.BillID == 104
select customer.BillItems.Sum (theBill => theBill.SalePrice * theBill.Quantity)).Max()

from customer in Bills
where customer.PaidStatus
select customer.BillItems.Sum (theBill => theBill.SalePrice * theBill.Quantity)

(from customer in Bills
where customer.PaidStatus
select customer.BillItems.Count()).Average()

