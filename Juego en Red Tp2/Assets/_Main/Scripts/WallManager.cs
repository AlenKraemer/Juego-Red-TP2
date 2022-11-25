using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;


public class WallManager : MonoBehaviourPun
{
    [SerializeField] private GameObject[] playersWalls;


    public void RPC_ActivateWalls(int index)
    {
        photonView.RPC(nameof(ActivateWall), RpcTarget.All, index);
    }


    [PunRPC]
    public void ActivateWall(int index)
    {
        playersWalls[index].SetActive(true);
    }
}
