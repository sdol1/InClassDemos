<Query Kind="Statements">
  <Connection>
    <ID>c90243ac-27fa-4aa8-bd94-4d778034355a</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

from aBillRow in Bills
where aBillRow.BillDate.Month == 5
orderby aBillRow.BillDate, aBillRow.Waiter.LastName, aBillRow.Waiter.FirstName
select new 
{
	BillDate = new DateTime(aBillRow.BillDate.Year, 
							aBillRow.BillDate.Month,
							aBillRow.BillDate.Day),
	Name = aBillRow.Waiter.LastName + " " + aBillRow.Waiter.FirstName,
	BillID = aBillRow.BillID,
	BillTotal = aBillRow.BillItems.Sum(bitem => bitem.Quantity * bitem.SalePrice),
	PartySize = aBillRow.NumberInParty,
	Contact = aBillRow.Reservation.CustomerName
}