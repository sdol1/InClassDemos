<Query Kind="Statements">
  <Connection>
    <ID>c90243ac-27fa-4aa8-bd94-4d778034355a</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

//step 1 connect to the desired database
//click on Add connection
//take defaults press Next
//Change server to . (dot), select existing database from drop down list
//optionally press test Connection
//press OK
//remember to check the Connection drop down list to see which database is the active connection
//language = C# Expression

//results should show database tables in connection object area
//expanding a table will reveal the table attributes and any relationships 

//view Waiter data
Waiters

//query syntax to also view Waiter data
from item in Waiters
select item

//method syntax to view Waiter Data  "=>" is lambda
Waiters.Select (item => item)

//alter the query syntax into a C# statement *use c# statement
var results = from item in Waiters
				select item;
results.Dump();

//once the quesry is create, tested, you will be able to
//transfer the query with minor modification into your 
//BLL methods
//public List<pocoObject> SomeBLLMethodName()
//{
//	//connect to your DAL Object : var context variable
//	//do your query
//	
//	var results = from item in contextvariable.Waiters
//				select item;
//	return results.ToList();
//}