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
    var date = new DateTime(2014, 10, 24);
	date.Dump();
	ReservationsByTime(date).Dump();
}
// Define other methods and classes here
public List<dynamic> ReservationsByTime(DateTime date)
{
  var result = from data in Reservations
  			   where data.ReservationDate.Year == date.Year
			      && data.ReservationDate.Month == date.Month
			      && data.ReservationDate.Day == date.Day
			      && data.ReservationStatus == 'B' // Reservation.Booked
			   select new // DTOs.ReservationSummary()
				{
					Name = data.CustomerName,
					Date = data.ReservationDate,
					NumberInParty = data.NumberInParty,
					Status = data.ReservationStatus,
					Event = data.SpecialEvents.Description,
					Contact = data.ContactPhone
// /*Just for curiosity's sake*/      , Tables = from seat in data.ReservationTables
//                                               select seat.Table.TableNumber
				};
	result.Dump();
	// the data source of the second query is the results from the first query
	// you do not need to always go to your entities
	var finalResult = from item in result
					  group item by item.Date.TimeOfDay;
	return finalResult.ToList<dynamic>();
}