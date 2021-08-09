using System;
using Xunit;
using SmartTechnologies;
using SelfDrivingCar.Entities;
using SelfDrivingCar.Client;

namespace SmartTechnologies.Tests
{
  public class UnitTest1
  {
    [Fact]
    public void Test1()
    {
      var str = SmartTechnologies.Program.Initialize();
      Assert.NotNull(str);
    }
  }
}
