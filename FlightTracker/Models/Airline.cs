using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using FlightTracker;

namespace FlightTracker.Models
{
  public class Airline
  {
    public int Id {get; set;}
    public DateTime DepartTime {get; set;}
    public string DepartCity {get; set;}

    public Airline()
    {

    }

    public Airline(DateTime departTime, string departCity, int id = 0)
    {
      Id = id;
      DepartTime = departTime;
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM airlines;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Arrival> GetArrivals()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT arrivals.* FROM airlines
      JOIN airlines_arrivals ON (airlines.id = airlines_arrivals.airline_id)
      JOIN arrivals ON (airlines_arrivals.airline_id = arrivals.id)
      WHERE airlines.id = @AirlineId;";
      MySqlParameter airlineIdParameter = new MySqlParameter();
      airlineIdParameter.ParameterName = "@AirlineId";
      airlineIdParameter.Value = Id;
      cmd.Parameters.Add(airlineIdParameter);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Arrival> arrivals = new List<Arrival>{};
      while(rdr.Read())
      {
        int airlineId = rdr.GetInt32(0);
        string airlineCity = rdr.GetString(1);
        Arrival newArrival = new Arrival(airlineCity, airlineId);
        arrivals.Add(newArrival);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return arrivals;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO airlines (depart_city, depart_time) VALUES (@city, @departTime);";
      cmd.Parameters.AddWithValue("@city", DepartCity);
      cmd.Parameters.AddWithValue("@departTime", DepartTime);
      cmd.ExecuteNonQuery();
      Id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
