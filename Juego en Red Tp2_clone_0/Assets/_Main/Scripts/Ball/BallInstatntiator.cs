using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.PUN;
using System;

public class BallInstatntiator : MonoBehaviour
{
    [SerializeField] private float timerSet = 5f;
    private float timer;

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        timer = timerSet;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if(timer<= 0)
        {
            MasterManager.Instance.RPCMaster("InstantiateBall");
            timer = timerSet;
        }
    }
}
