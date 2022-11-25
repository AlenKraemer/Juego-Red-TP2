using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.PUN;
using System;

public class ControllerFA : MonoBehaviour
{
    public static event Action<bool> onChatHide;
    public static event Action<bool> onVoiceMute;


    private bool showChat = true;

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Destroy(Camera.main.gameObject);
            Destroy(this);
        }
    }
    private void Start()
    {
        //Request Para Instanciar a mi Player
        MasterManager.Instance.RPCMaster("RequestConnectPlayer", PhotonNetwork.LocalPlayer);
        PhotonNetwork.Instantiate("VoiceObject", Vector3.zero, Quaternion.identity);
        PunVoiceClient.Instance.PrimaryRecorder.TransmitEnabled = false;
        onVoiceMute?.Invoke(PunVoiceClient.Instance.PrimaryRecorder.TransmitEnabled);
    }

    private void Update()
    {
        //Consigo los inputs
        var h = Input.GetAxisRaw("Horizontal");


        Vector3 dir = new Vector3(h, 0, 0);

        //le pido al master que me mueva al personaje
        if(dir != Vector3.zero)
        {
            MasterManager.Instance.RPCMaster("RequestMove", PhotonNetwork.LocalPlayer, dir);
        }

        //mutea o desmutea el mic
        if (Input.GetKeyDown(KeyCode.T))
        {
            PunVoiceClient.Instance.PrimaryRecorder.TransmitEnabled = !PunVoiceClient.Instance.PrimaryRecorder.TransmitEnabled;
            onVoiceMute?.Invoke(PunVoiceClient.Instance.PrimaryRecorder.TransmitEnabled);
        }


        //esconde el chat

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            showChat = !showChat;
            onChatHide?.Invoke(showChat);
        }
    }

}
