using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;
    public Transform[] spawnPositions;
    public float timeBetweenZombiesSpawn = 2f;
    private void Awake()
    {
        instance = this;
    }

    public void StartWave(int waveNumber)
    {
        StartCoroutine("SpawnInZombies", waveNumber * 2);
    }

    IEnumerator SpawnInZombies(int amountOfZombies)
    {
        for (int i = 0; i < amountOfZombies; i++)
        {
            GameObject zombie = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Zombie"), spawnPositions[Random.Range(0, spawnPositions.Length)].position, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenZombiesSpawn);
        }
        
    }
}
