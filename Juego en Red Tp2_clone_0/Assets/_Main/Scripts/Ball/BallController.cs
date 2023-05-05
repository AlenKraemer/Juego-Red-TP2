using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallController : MonoBehaviourPun
{
    [SerializeField] private float speed = 5f;
    private Vector2[] initialLaunch = { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0), new Vector2(1, 1), new Vector2(-1, 1), new Vector2(1, -1), new Vector2(-1, -1) };
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

  


    public void Launch()
    {
        int index = Random.Range(0, 8);
        rb.velocity = initialLaunch[index] * speed;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var obj = collision.GetComponent<PlayerScore>();
            obj.RPC_UpdateScore();

            MasterManager.Instance.RPCMaster("DeleteBall",photonView);
        }
    }
}