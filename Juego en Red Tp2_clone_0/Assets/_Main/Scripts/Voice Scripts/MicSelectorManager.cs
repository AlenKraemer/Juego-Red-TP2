using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Voice.Unity;
using Photon.Voice;

public class MicSelectorManager : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    [SerializeField] private Recorder recorder;
    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        var list = new List<string>(Microphone.devices);
        dropdown.AddOptions(list);
    }


    public void SetMicrophone(int i)
    {
        var mic = Microphone.devices[i];
        recorder.MicrophoneDevice = new DeviceInfo(mic);
    }
}
