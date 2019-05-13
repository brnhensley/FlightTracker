using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlightTracker.Models;
using System.Collections.Generic;
using System;

namespace FlightTracker.Tests
{
  [TestClass]
  public class CityTest : IDisposable
  {

    public void Dispose()
    {
      Item.ClearAll();
      Category.ClearAll();
    }

    public ItemTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=flight_tracker_test;";
    }
  }
}
