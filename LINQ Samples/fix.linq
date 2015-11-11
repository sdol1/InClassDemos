<Query Kind="Expression">
  <Connection>
    <ID>c90243ac-27fa-4aa8-bd94-4d778034355a</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

from walkIn in Bills
where walkIn.BillDate.Year == 2014
     && walkIn.BillDate.Month == 10
     && walkIn.BillDate.Day == 16
	 && walkIn.BillDate.Hour == 
select walkIn

from walkIn in Bills
where walkIn.BillDate >= new DateTime(2014, 10, 16, 13, 0, 0)
select walkIn
