using System;
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

    public List<Item> GetItems()
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
      airlineIdParameter.Value = _id;
      cmd.Parameters.Add(airlineIdParameter);
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Item> arrivals = new List<Item>{};
      while(rdr.Read())
      {
        int airlineId = rdr.GetInt32(0);
        string airlineCity = rdr.GetString(1);
        Item newItem = new Item(airlineCity, airlineId);
        arrivals.Add(newItem);
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
      cmd.CommandText = @"INSERT INTO airline (name) VALUES (@name);";
      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
