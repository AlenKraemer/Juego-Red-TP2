using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System;
public class PlayerScore : MonoBehaviourPun
{
    [SerializeField] private int playerID;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private int score = 20;


    public void RPC_UpdateScore()
    {
        photonView.RPC(nameof(UpdateScore), RpcTarget.All);

    }

    public void RPC_SetInitialScore()
    {
        photonView.RPC(nameof(SetInitialScore), RpcTarget.AllBuffered);
    }

    public void RPC_DeleteScore()
    {
        photonView.RPC(nameof(DeleteScore),RpcTarget.All);
    }

    public void RPC_Victory()
    {
        photonView.RPC(nameof(Victory), RpcTarget.All);
    }

    [PunRPC]
    public void DeleteScore()
    {
        playerScoreText.text = null;
    }


    [PunRPC]
    public void UpdateScore()
    {
        score--;
        playerScoreText.text = score.ToString();

        if (PhotonNetwork.IsMasterClient)
        {
            if(score <= 0)
            {
                MasterManager.Instance.RPCMaster("KillPlayer", playerID);
            }
        }
    }

    [PunRPC]
    public void SetInitialScore()
    {
        playerScoreText.text = score.ToString();
    }

    [PunRPC]
    public void Victory()
    {
        victoryScreen.SetActive(true);
    }
}
