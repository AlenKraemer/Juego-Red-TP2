using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.PUN;

public class CameraController : MonoBehaviourPun
{
    private void Awake()
    {
        if (!photonView.IsMine)
        {
            Destroy(gameObject);
        }
    }
}
