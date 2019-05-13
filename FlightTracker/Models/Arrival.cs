using System;
using MySql.Data.MySqlClient;
using FlightTracker;
using System.Collections.Generic;

namespace FlightTracker.Models
{


  public class Arrival
  {
    public string City {get; set;}
    public int Id {get; set;}

    public Arrival (string city, int id = 0)
    {
      City = city;
      Id = id;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO arrivals (city) VALUES (@city);";
      MySqlParameter city = new MySqlParameter();
      city.ParameterName = "@city";
      city.Value = this.City;
      cmd.Parameters.Add(city);
      cmd.ExecuteNonQuery();
      Id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void AddAirline(Airline newAirline)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO flights (airlines_id, cities_id) VALUES (@AirlineId, @AttivalId);";
      MySqlParameter airlines_id = new MySqlParameter();
      airlines_id.ParameterName = "@AirlineId";
      airlines_id.Value = newAirline.Id;
      cmd.Parameters.Add(airlines_id);
      MySqlParameter cities_id = new MySqlParameter();
      cities_id.ParameterName = "@AttivalId";
      cities_id.Value = Id;
      cmd.Parameters.Add(cities_id);
      cmd.ExecuteNonQuery();
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Arrival> GetAll()
    {
      List<Arrival> allArrivals = new List<Arrival> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM arrivals;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int arrivalId = rdr.GetInt32(0);
        string arrivalCity = rdr.GetString(1);
        Arrival newArrival = new Arrival(arrivalCity, arrivalId);
        allArrivals.Add(newArrival);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allArrivals;
    }

    public List<Airline> GetAirlines()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT airlines_id FROM flights WHERE cities_id = @arrivalId;";
      MySqlParameter arrivalIdParameter = new MySqlParameter();
      arrivalIdParameter.ParameterName = "@arrivalId";
      arrivalIdParameter.Value = Id;
      cmd.Parameters.Add(arrivalIdParameter);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<int> airlineIds = new List<int> {};
      while(rdr.Read())
      {
        int airlineId = rdr.GetInt32(0);
        airlineIds.Add(airlineId);
      }
      rdr.Dispose();
      List<Airline> airlines = new List<Airline> {};
      foreach (int airlineId in airlineIds)
      {
        var airlineQuery = conn.CreateCommand() as MySqlCommand;
        airlineQuery.CommandText = @"SELECT * FROM airlines WHERE id = @AirlineId;";
        MySqlParameter airlineIdParameter = new MySqlParameter();
        airlineIdParameter.ParameterName = "@AirlineId";
        airlineIdParameter.Value = airlineId;
        airlineQuery.Parameters.Add(airlineIdParameter);
        var airlineQueryRdr = airlineQuery.ExecuteReader() as MySqlDataReader;
        while(airlineQueryRdr.Read())
        {
          int thisAirlineId = airlineQueryRdr.GetInt32(0);
          DateTime airlineTime = airlineQueryRdr.GetDateTime(1);
          string airlineCity = airlineQueryRdr.GetString(2);
          Airline foundAirline = new Airline(airlineTime, airlineCity);
          airlines.Add(foundAirline);
        }
        airlineQueryRdr.Dispose();
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return airlines;
      }

      public static Arrival Find(int id)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM arrivals WHERE id = @searchId;";
        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = id;
        cmd.Parameters.Add(searchId);
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        int arrivalId = 0;
        string city = "";
        while(rdr.Read())
        {
          arrivalId = rdr.GetInt32(0);
          city = rdr.GetString(1);
        }
        Arrival newArrival = new Arrival(city, arrivalId);
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return newArrival;
      }

      public override bool Equals(System.Object otherArrival)
      {
        if (!(otherArrival is Arrival))
        {
          return false;
        }
        else
        {
          Arrival newArrival = (Arrival) otherArrival;
          bool idEquality = this.Id == newArrival.Id;
          bool cityEquality = this.City == newArrival.City;
          //Console.WriteLine("id"+idEquality+"city"+cityEquality);
          return (idEquality && cityEquality);
        }
      }

      public static void ClearAll()
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"DELETE FROM arrivals;";
        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
      }

    }
  }
