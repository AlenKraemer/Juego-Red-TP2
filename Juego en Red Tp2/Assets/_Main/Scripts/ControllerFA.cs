using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class ControllerFA : MonoBehaviour
{
    private void Start()
    {
        //Request Para Instanciar a mi Player
        MasterManager.Instance.RPCMaster("RequestConnectPlayer", PhotonNetwork.LocalPlayer);

    }

    private void Update()
    {
        //Consigo los inputs
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;

        if(dir != Vector3.zero)
        {
            MasterManager.Instance.RPCMaster("RequestMove", PhotonNetwork.LocalPlayer, dir);
        }
    }

}
