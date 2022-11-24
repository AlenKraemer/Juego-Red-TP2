using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.PUN;

public class UIController : MonoBehaviourPun
{
    private UIManager uIManager;
    

    private void Start()
    {
        if (photonView.IsMine)
        {
            uIManager = FindObjectOfType<UIManager>();
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            var voice = PunVoiceClient.Instance.PrimaryRecorder.TransmitEnabled;
            uIManager.ShowMicUI(voice);


        }
    }
}
