using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CharacterModel : MonoBehaviourPun
{
    [SerializeField] private float speed = 10f;
    private Rigidbody2D rb;
    private Camera playerCamera;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCamera = Camera.main;
        
    }

    public void Move(Vector3 dir)
    {
        rb.velocity = (transform.right * dir.x) * speed * Time.deltaTime;
    }

    
    public void RPC_FreezeRigidBody(bool option)
    {
        photonView.RPC(nameof(FreezeRigidBody), RpcTarget.All, option);
    }

    [PunRPC]
    public void FreezeRigidBody(bool option)
    {
        if (option == true)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
