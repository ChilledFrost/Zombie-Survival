using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerList : MonoBehaviour
{
    public TMP_Text text;
    public void Init(string _info) => text.text = _info;
}
