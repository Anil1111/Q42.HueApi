using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Streaming.Sample
{
  public class StreamingSetup
  {
    public static async Task<StreamingGroup> SetupAndReturnGroup()
    {
      //string ip = "192.168.0.4";
      //string key = "8JwWAj5J1tSsKLxyUOdAkWmcCQFcNc51AKRhxdH9";
      //string entertainmentKey = "AFFD322C34C993C19503D369481869FD";
      //var useSimulator = false;

      //string ip = "10.70.16.29";
      //string key = "WzWypCKxLFGvmC8xRyaANsSsrbMX7NXitFO6wXru";
      //string entertainmentKey = "77168F2CCF453508EC6D5A37EC1F4B09";
      //var useSimulator = false;

      string ip = "127.0.0.1";
      string key = "aSimulatedUser";
      string entertainmentKey = "01234567890123456789012345678901";
      var useSimulator = true;


      //Initialize streaming client
      StreamingHueClient client = new StreamingHueClient(ip, key, entertainmentKey);

      //Get the entertainment group
      var all = await client.LocalHueClient.GetEntertainmentGroups();
      var group = all.FirstOrDefault();

      if (group == null)
        throw new Exception("No Entertainment Group found. Create one using the Q42.HueApi.UniversalWindows.Sample");
      else
        Console.WriteLine($"Using Entertainment Group {group.Id}");

      //Create a streaming group
      var stream = new StreamingGroup(group.Locations);
      stream.IsForSimulator = useSimulator;


      //Connect to the streaming group
      await client.Connect(group.Id, simulator: useSimulator);

      //Start auto updating this entertainment group
      client.AutoUpdate(stream, new System.Threading.CancellationToken(), 50, onlySendDirtyStates: true);

      //Optional: Check if streaming is currently active
      var bridgeInfo = await client.LocalHueClient.GetBridgeAsync();
      Console.WriteLine(bridgeInfo.IsStreamingActive ? "Streaming is active" : "Streaming is not active");
      return stream;
    }
  }
}
