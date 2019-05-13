using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlightTracker.Models;
using System.Collections.Generic;
using System;

namespace FlightTracker.Tests
{
  [TestClass]
  public class ArrivalTest : IDisposable
  {

    public void Dispose()
    {
      Arrival.ClearAll();
      Airline.ClearAll();
    }

    public ArrivalTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=flight_tracker_test;";
    }

    [TestMethod]
    public void GetAll_ReturnsEmptyList_ArrivalList()
    {
      //Arrange
      List<Arrival> newList = new List<Arrival> { };

      //Act
      List<Arrival> result = Arrival.GetAll();

      //Assert
      CollectionAssert.AreEqual(newList, result);
    }

    [TestMethod]
    public void GetAll_ReturnsArrivals_ArrivalList()
    {
      //Arrange
      string city01 = "Walk the dog";
      string city02 = "Wash the dishes";
      Arrival newArrival1 = new Arrival(city01);
      newArrival1.Save();
      Arrival newArrival2 = new Arrival(city02);
      newArrival2.Save();
      List<Arrival> newList = new List<Arrival> { newArrival1, newArrival2 };

      //Act
      List<Arrival> result = Arrival.GetAll();

      //Assert
      CollectionAssert.AreEqual(newList, result);
    }

    [TestMethod]
    public void Find_ReturnsCorrectArrivalFromDatabase_Arrival()
    {
      //Arrange
      Arrival testArrival = new Arrival("Mow the lawn");
      testArrival.Save();

      //Act
      Arrival foundArrival = Arrival.Find(testArrival.Id);

      //Assert
      Assert.AreEqual(testArrival, foundArrival);
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Arrival()
    {
      // Arrange, Act
      Arrival firstArrival = new Arrival("Mow the lawn");
      Arrival secondArrival = new Arrival("Mow the lawn");

      // Assert
      Assert.AreEqual(firstArrival, secondArrival);
    }

    [TestMethod]
    public void Save_SavesToDatabase_ArrivalList()
    {
      //Arrange
      Arrival testArrival = new Arrival("Mow the lawn");

      //Act
      testArrival.Save();
      List<Arrival> result = Arrival.GetAll();
      List<Arrival> testList = new List<Arrival>{testArrival};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      //Arrange
      Arrival testArrival = new Arrival("Mow the lawn");

      //Act
      testArrival.Save();
      Arrival savedArrival = Arrival.GetAll()[0];

      int result = savedArrival.Id;
      int testId = testArrival.Id;

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void GetAirlines_ReturnsAllArrivalAirlines_AirlineList()
    {
      DateTime rightNow = DateTime.Now;
      //Arrange
      Arrival testArrival = new Arrival("Mow the lawn");
      testArrival.Save();
      Airline testAirline1 = new Airline(rightNow, "Home stuff");
      testAirline1.Save();
      Airline testAirline2 = new Airline(rightNow, "Work stuff");
      testAirline2.Save();

      //Act
      testArrival.AddAirline(testAirline1);
      List<Airline> result = testArrival.GetAirlines();
      List<Airline> testList = new List<Airline> {testAirline1};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void AddAirline_AddsAirlineToArrival_AirlineList()
    {
      //Arrange
      DateTime rightNow = DateTime.Now;
      Arrival testArrival = new Arrival("Mow the lawn");
      testArrival.Save();
      Airline testAirline = new Airline(rightNow, "Home stuff");
      testAirline.Save();

      //Act
      testArrival.AddAirline(testAirline);

      List<Airline> result = testArrival.GetAirlines();
      List<Airline> testList = new List<Airline>{testAirline};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }

  }
}
