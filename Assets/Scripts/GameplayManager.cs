using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameplayManager : MonoBehaviour
{
    [SerializeField] string playerPrefab;
    public int counter = 0;
    [SerializeField]
    private Transform[] _spawnPoint;
    int positionNumber=0;
    // Start is called before the first frame update
    void Start()
    {
       
        //  PhotonNetwork.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        PhotonNetwork.Instantiate(playerPrefab, _spawnPoint[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.position, _spawnPoint[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform.rotation);
    }
        // Update is called once per frame
    void Update()
    {
        
    }
}
