using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class RoomList : MonoBehaviour
{
    public TMP_Text text;
    public void Init(string _info) => text.text = _info;
    public void OnClick() => NetworkManager.instance.JoinRoom(text.text);
}
