using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public enum MenuScreen {LoadingScreen, MenuScreen, SearchScreen, RoomScreen, CreateMenuScreen};

    public Menu[] menuArray;
    public static MenuManager instance;

    [Header("Prefabs")]
    public GameObject roomListPrefab;
    public GameObject playerListPrefab;

    [Header("List Holders")]
    public Transform roomListHolder;
    public Transform playerListHolder;

    [Header("Extras")]
    public TMP_Text roomNameText;
    public GameObject startRoomButton;

    private void Awake()
    {
        instance = this;
    }

    public void OpenScreen(string mName)
    {
        for (int i = 0; i < menuArray.Length; i++)
        {
            if(menuArray[i].menuName == mName)
            {
                menuArray[i].Open();
            }
            else if(menuArray[i].isOpen)
            {
                menuArray[i].Close();
            }
        }
    }
    public void CreateRoom(TMP_InputField input)
    {
        if (string.IsNullOrEmpty(input.text)) return;
        NetworkManager.instance.CreateRoom(input.text);
    }

    private void ClearChildrenInHolder(Transform holder)
    {
        foreach(Transform transform in holder)
        {
            Destroy(transform.gameObject);
        } 
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        ClearChildrenInHolder(roomListHolder);

        foreach (RoomInfo roomInfo in roomList)
        {
            print(roomInfo.Name);
            if (roomInfo.RemovedFromList) return;

            RoomList roomPrefab = Instantiate(roomListPrefab, roomListHolder).GetComponent<RoomList>();
            roomPrefab.Init(roomInfo.Name);
        }
    }

    public void UpdatePlayerList(Player[] playerList)
    {
        ClearChildrenInHolder(playerListHolder);
        for (int i = 0; i < playerList.Length; i++)
        {
            PlayerList playerPrefab = Instantiate(playerListPrefab, playerListHolder).GetComponent<PlayerList>();
            playerPrefab.Init(playerList[i].NickName);
        }
    }

    // Lambda Things
    public void NewPlayerAddedToList(Player newPlayer) => Instantiate(playerListPrefab, playerListHolder).GetComponent<PlayerList>().Init(newPlayer.NickName);
    public void SetRoomName(string roomName) => roomNameText.text = roomName;
    public void EnableStartButton(bool isHost) => startRoomButton.SetActive(isHost);


}
