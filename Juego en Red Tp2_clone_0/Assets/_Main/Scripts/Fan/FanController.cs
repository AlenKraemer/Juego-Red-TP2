using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FanController : MonoBehaviourPun
{
    [SerializeField] private float torque = 2f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

   

    public void Spin()
    {
        var aux = Random.Range(0, 2) * 2 - 1;
        torque *= aux;
        rb.AddTorque(torque,ForceMode2D.Impulse);
    }
}
