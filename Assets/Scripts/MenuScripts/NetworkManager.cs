using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;

    // Initialization
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = "Player_" + Random.Range(0,1000).ToString("0000");
        print(PhotonNetwork.NickName);
        MenuManager.instance.OpenScreen("LoadingScreen");
    }

    // Override Functions
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.instance.OpenScreen("MenuScreen");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.instance.OpenScreen("RoomScreen");
        MenuManager.instance.SetRoomName(PhotonNetwork.CurrentRoom.Name);
        MenuManager.instance.UpdatePlayerList(PhotonNetwork.PlayerList);
        MenuManager.instance.EnableStartButton(PhotonNetwork.LocalPlayer.IsMasterClient);
    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.LogError(message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("Failed To Join");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        MenuManager.instance.NewPlayerAddedToList(newPlayer);
        MenuManager.instance.EnableStartButton(PhotonNetwork.LocalPlayer.IsMasterClient);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer) 
    { 
        MenuManager.instance.UpdatePlayerList(PhotonNetwork.PlayerList);
        MenuManager.instance.EnableStartButton(PhotonNetwork.LocalPlayer.IsMasterClient && PhotonNetwork.PlayerList.Length > 1);
    }
    public override void OnMasterClientSwitched(Player newMasterClient) => MenuManager.instance.EnableStartButton(PhotonNetwork.LocalPlayer.IsMasterClient && PhotonNetwork.PlayerList.Length > 1);
    public override void OnRoomListUpdate(List<RoomInfo> roomList) => MenuManager.instance.UpdateRoomList(roomList);

    // Not Override Functions
    public void JoinRoom(string room)
    {
        PhotonNetwork.JoinRoom(room);
        MenuManager.instance.OpenScreen("LoadingScreen");
    }

    public void CreateRoom(string roomName)
    {
        print("Creating");
        PhotonNetwork.CreateRoom(roomName);
        MenuManager.instance.OpenScreen("LoadingScreen");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenScreen("LoadingScreen");
    }

    public void StartGame() => PhotonNetwork.LoadLevel(1);
}
