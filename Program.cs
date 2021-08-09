using System;
using SelfDrivingCar;
using SelfDrivingCar.Client;
using SelfDrivingCar.Entities;

namespace SmartTechnologies
{
  public class Program
  {
    public static void Main(string[] args)
    {
      Initialize();
      //0 Look Ahead speed logic

    }

    public static string Initialize()
    {
      Token token = new Token();
      SelfDrivingCarRestClient client = new SelfDrivingCarRestClient();
      token = client.Register(new TokenRequest { Name = "lbalogun16@gmail.com", CourseLayout = 3 });
      SelfDrivingCarRestClient newClient = new SelfDrivingCarRestClient();
      newClient.Token = token;
      client = newClient;
      Console.WriteLine(token.Id);
      CarAction actionResult = client.DoAction(new CarAction { Action = "IgnitionOn" });
      Console.WriteLine("Action " + actionResult.Action);
      Console.WriteLine("Force " + actionResult.Force.ToString());
      Car car = GetCarInfo(client);
      Road road = GetRoadInfo(client);
      // course 1
      car = Drive(client, ref car, ref road);
      return "sucess";
    }

    private static Car Drive(SelfDrivingCarRestClient client, ref Car car, ref Road road)
    {
      road = GetRoadInfo(client);
      while (road.CurrentSpeedLimit.Max != 0)
      {
        if (car.Engine.State == "Idling")
        {
          AccelerateSpeed(client, ref car, ref road);
        }
        if (car.CurrentVelocity >= road.CurrentSpeedLimit.Max)
        {
          Console.WriteLine("in if condition");

          SlowDownSpeed(ref car, ref road, client);
          road = GetRoadInfo(client);
        }
        else if (car.CurrentVelocity < road.CurrentSpeedLimit.Max)
        {
          AccelerateSpeed(client, ref car, ref road);
        }
      }
      if (road.CurrentSpeedLimit.Max == 0 && !road.SpeedLimitAhead.Max.HasValue)
      {
        CarAction actionResult;
        TurnOffIgnition(client, out actionResult);
        return car;
      }
      else
      {
        SlowDownSpeed(ref car, ref road, client);
      }
      return null;
    }

    private static void TurnOffIgnition(SelfDrivingCarRestClient client, out CarAction actionResult)
    {
      Console.WriteLine("Turning off ignition.");
      actionResult = client.DoAction(new CarAction { Action = "IgnitionOff" });
    }

    private static void SlowDownSpeed(ref Car car, ref Road road, SelfDrivingCarRestClient newClient)
    {
      car = GetCarInfo(newClient);
      road = GetRoadInfo(newClient);
      // stop sign or end of course approach
      if (road.SpeedLimitAhead.Max == 0 || !road.SpeedLimitAhead.Max.HasValue)
      {
        Console.WriteLine("Need to slow down");
        while (road.SpeedLimitAhead.Max == 0 && car.CurrentVelocity != 0)
        {
          Console.WriteLine("Braking for stop sign");
          while (car.CurrentVelocity > 5)
          {
            newClient.DoAction(new CarAction { Action = "Brake", Force = 5000 });
            car = GetCarInfo(newClient);
            road = GetRoadInfo(newClient);
          }
          road = GetRoadInfo(newClient);
          car = GetCarInfo(newClient);

        }
        if (road.SpeedLimitAhead.Max.HasValue == false && road.CurrentSpeedLimit.Max == 0)
        {
          CarAction actionResult;
          TurnOffIgnition(newClient, out actionResult);
          return;
        }
      }
      if (road.SpeedLimitAhead.Max > 0 && road.CurrentSpeedLimit.Max == 0)
      {
        Console.WriteLine("Stopping at stop sign");
        // stopping and starting
        newClient.DoAction(new CarAction { Action = "Brake", Force = 40000 });
        Drive(newClient, ref car, ref road);
      }
      newClient.DoAction(new CarAction { Action = "Brake", Force = 3 });
      Console.WriteLine("Slowing Down");
      while (car.CurrentVelocity >= road.CurrentSpeedLimit.Max)
      {
        car = GetCarInfo(newClient);
        road = GetRoadInfo(newClient);
      }
      Console.WriteLine("Stop Decelerating");
    }

    private static void AccelerateSpeed(SelfDrivingCarRestClient client, ref Car car, ref Road road)
    {
      car = GetCarInfo(client);
      road = GetRoadInfo(client);
      if (road.SpeedLimitAhead.Max == 0 || !road.SpeedLimitAhead.Max.HasValue)
      {
        Console.WriteLine("Need to slow down");

        while (road.SpeedLimitAhead.Max == 0 && car.CurrentVelocity != 0)
        {
          Console.WriteLine("Braking for stop sign");
          while (car.CurrentVelocity > 5)
          {
            client.DoAction(new CarAction { Action = "Brake", Force = 5000 });
            car = GetCarInfo(client);
            road = GetRoadInfo(client);
          }
          car = GetCarInfo(client);
          road = GetRoadInfo(client);
        }

        if (road.SpeedLimitAhead.Max.HasValue == false && road.CurrentSpeedLimit.Max == 0)
        {
          CarAction actionResult;
          TurnOffIgnition(client, out actionResult);
          return;
        }
      }
      if (road.SpeedLimitAhead.Max > 0 && road.CurrentSpeedLimit.Max == 0)
      {
        // stopping and starting
        Console.WriteLine("Stopping at stop sign");
        while (car.CurrentVelocity != 0)
        {
          client.DoAction(new CarAction { Action = "Brake", Force = 5000 });
          car = GetCarInfo(client);
          road = GetRoadInfo(client);
        }
        Drive(client, ref car, ref road);

      }
      client.DoAction(new CarAction { Action = "Accelerate", Force = 3 });
      Console.WriteLine("Speeding Up");
      while (car.CurrentVelocity <= road.CurrentSpeedLimit.Max)
      {
        road = GetRoadInfo(client);
        car = GetCarInfo(client);
      }
      Console.WriteLine("Stop Accelerating");
    }

    private static Road GetRoadInfo(SelfDrivingCarRestClient client)
    {
      Road road = client.GetRoad();
      Console.WriteLine("Current Speed Limit Max " + road.CurrentSpeedLimit.Max.ToString());
      Console.WriteLine("Current Speed Limit Min " + road.CurrentSpeedLimit.Min.ToString());
      Console.WriteLine("Speed Limit Ahead Max " + road.SpeedLimitAhead.Max.ToString());
      Console.WriteLine("Speed Limit Ahead Min " + road.SpeedLimitAhead.Min.ToString());
      return road;
    }

    private static Car GetCarInfo(SelfDrivingCarRestClient client)
    {
      Car car = client.GetCar();
      Console.WriteLine("Car Ignition " + car.Ignition.ToString());
      Console.WriteLine("Total time Car Travelled " + car.TotalTimeTravelled.ToString());
      Console.WriteLine("Current Car Velocity " + car.CurrentVelocity.ToString());
      Console.WriteLine("Total Distance Travelled " + car.TotalDistanceTravelled.ToString());
      Console.WriteLine("Car Engine State " + car.Engine.State);
      return car;
    }
  }
}
