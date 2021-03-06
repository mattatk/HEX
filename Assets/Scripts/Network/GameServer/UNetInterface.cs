﻿/*
 * Copyright (c) 2015 Colin James Currie.
 * All rights reserved.
 * Contact: cj@cjcurrie.net
 */

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class UNetInterface : NetworkManager
{
  const string gameID = "369Hex963";

  static string gameName;
  public static UNetInterface instance;
  //public static List<NetworkBus> clientBuses;
  public static List<NetworkConnection> clientConnections;

  static NetworkClient myClient;

  short playerID;

  public void Initialize ()
  {
    instance = this;
    //clientBuses = new List<NetworkBus>();
    clientConnections = new List<NetworkConnection>();
  }

  public void UnInitialize()
  {
    
  }

  public static void HostP2PServer(string gn)
  {
    gameName = gn;

    myClient = instance.StartHost();
    Network.InitializeServer(32, 1337, !Network.HavePublicAddress());
    MasterServer.RegisterHost(gameID, gameName);
  }

  public static void JoinP2PServer(string gn)
  {
    gameName = gn;

    myClient = instance.StartClient();
    myClient.RegisterHandler(MyMsgType.UpdateSeed, OnSeedReceived);

    MasterServer.RequestHostList(gameID);
  }

  // === Unity Callbacks ===
  void OnServerInitialized()
  {
    ChatUI.SystemMessage("You are now hosting a public room called "+gameName);
    RegisterCallbacks();

    //GameController.OnNetworkInitialized();    This should be here but must be on the player instead
  }

  void OnConnectedToServer()
  {
    RegisterCallbacks();
    //GameController.networkBus.CmdSetUsername(Settings.username);
    //GameController.networkBus.CmdAskForSeed();

    WelcomeMessages();
  }

  public override void OnServerConnect(NetworkConnection conn)
  {
    //conn.Send(MyMsgType.UpdateSeed, new StringMessage(GameController.seed));
    base.OnServerConnect(conn);
  }

  public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
  {
    GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.up*15, Quaternion.identity);
    ClientHub bus = player.GetComponent<ClientHub>();

    NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
  }

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
          ChatUI.SystemMessage("No game room of name "+gameName+" was found.");
          gameName = "";
        }
      break;
    }
  }
  // === /Unity Callbacks ===
  
  void WelcomeMessages()
  {
    ChatUI.SystemMessage("You have joined the public game "+gameName);
  }

  void RegisterCallbacks()
  {
    NetworkServer.RegisterHandler(MyMsgType.UpdateSeed, OnSeedReceived);

    //NetworkServer.Instance.Listen(7070);
    //NetworkServer.Instance.RegisterHandler(MsgType.SYSTEM_CONNECT, OnConnected);
  }

  static void OnSeedReceived(NetworkMessage netMsg)
  {
    //GameController.OnSeedReceived(netMsg.ReadMessage<StringMessage>().value);
  }
  
  public void OnMyApplicationQuit()
  {
  }
}
