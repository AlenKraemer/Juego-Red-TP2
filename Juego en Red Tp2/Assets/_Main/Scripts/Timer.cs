using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System;

public class Timer : MonoBehaviourPun
{

    [SerializeField] private TextMeshProUGUI timerText;


    public void RPC_TimerStart(int secondsToWait)
    {
        photonView.RPC(nameof(TimerStart), RpcTarget.All, secondsToWait);

    }

    [PunRPC]
    public void TimerStart(int secondsToWait)
    {
        StartCoroutine(WaitToStart(secondsToWait));
    }


    IEnumerator WaitToStart(int secondsToWait)
    {
        for (int i = secondsToWait; i > 0; i--)
        {
            timerText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        timerText.text = null;
        if (PhotonNetwork.IsMasterClient)
        {
            MasterManager.Instance.RPCMaster("InstantiateBall");
        }
    }

  

}
