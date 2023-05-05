using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;

public class UIController : MonoBehaviourPun
{
    public Speaker speaker;
    private UIManager uIManager;
    

    private void Start()
    {
        if (photonView.IsMine)
        {
            uIManager = FindObjectOfType<UIManager>();
        }
        else
        {
            FindObjectOfType<VoiceUI>().AddSpeaker(speaker, photonView.Owner);
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
