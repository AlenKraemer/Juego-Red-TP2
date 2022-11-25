using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

   
}
