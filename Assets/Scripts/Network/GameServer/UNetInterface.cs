/* AIN'T WORKIN BREH
  
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class UNetInterface : NetworkManager 
{
  string gameName;

  // === Unity Callbacks ===
  // Called only on a server
  void OnServerInitialized(){}

  // Called on both client and server when connected to a server
  void OnConnectedToServer(){}

  // Called on server when a client connects
  public override void OnServerConnect(NetworkConnection conn){}

  // Called on the server when a clients adds a player obj
  public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId){}

  // Called on both client and server when the UNet master server makes contact
  void OnMasterServerEvent(MasterServerEvent msEvent)
  {
    switch(msEvent)
    {
      case MasterServerEvent.HostListReceived:
        HostData[] hostList = MasterServer.PollHostList();
        bool found = false;

        foreach (HostData d in hostList)
        {
          if (d.gameName == gameName)
          {
            found = true;
            Network.Connect(d);
            break;
          }
        }

        if (!found)
        {
          MainUI.SystemMessage("No game room of name "+gameName+" was found.");
          gameName = "";
        }
      break;
    }
  }
}
*/