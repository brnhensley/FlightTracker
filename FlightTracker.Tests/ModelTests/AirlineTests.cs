using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlightTracker.Models;
using System.Collections.Generic;
using System;

namespace FlightTracker.Tests
{
  [TestClass]
  public class AirlineTest : IDisposable
  {

    public CategoryTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=flight_tracker_test;";
    }

    public void Dispose()
    {
      Category.ClearAll();
      Item.ClearAll();
    }

  }
}
