using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform[] playersSpawns;
    private int playerQuantity;
    private bool isStarted;
    private float startTimer = 2f;
    public static event Action OnWin;

    private void Awake()
    {

    }

    

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient && !isStarted)
        {
            if (isStarted) return;
            playerQuantity = PhotonNetwork.CurrentRoom.PlayerCount;
            photonView.RPC("UpdatePlayerQuantity", RpcTarget.Others, playerQuantity);
            
            if (playerQuantity == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                isStarted = true;
                StartCoroutine(WaitToStart());
                
            }
        }    
    }

    

    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(startTimer);
        photonView.RPC("StartGame", RpcTarget.All);
    }

    private void UpdatePlayers()
    {
        //Debug.Log("entre a update players");
        playerQuantity--;
        photonView.RPC("UpdatePlayerQuantity", RpcTarget.Others, playerQuantity);
        //Debug.Log(playerQuantity);
        if(playerQuantity == 1)
        {
            //Debug.Log("llame al ganador");
            photonView.RPC("WinPlayer", RpcTarget.All);
        }
    }




}
