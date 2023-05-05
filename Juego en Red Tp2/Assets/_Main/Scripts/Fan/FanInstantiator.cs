using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FanInstantiator : MonoBehaviour
{
    [SerializeField] private float timerSet = 10f;
    [SerializeField] private int maxFans = 3;
    private float timer;
    private int counter = 0;

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
        if (timer <= 0 && counter < maxFans)
        {
            int x = Random.Range(-4,5);
            int y = Random.Range(-2,3);
            Vector3 pos = new Vector3(x, y, 0);
            MasterManager.Instance.RPCMaster("InstantiateFan", pos);
            counter++;
            timer = timerSet;
        }
    }
}
