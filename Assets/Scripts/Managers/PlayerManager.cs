using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviourPunCallbacks
{
	void Start()
	{
		if (photonView.IsMine)
		{
			CreateController();
		}
	}

	void CreateController()
	{
		PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerBlue"), Vector3.zero, Quaternion.identity);
	}
}