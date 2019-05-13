using System;
using MySql.Data.MySqlClient;
using FlightTracker;

namespace FlightTracker.Models
{

  public class Arrival
  {
    private string City {get; set;}
    private int Id {get; set;}

    public Item (string city, int id = 0)
    {
      City = city;
      Id = id;
    }


  }
}
