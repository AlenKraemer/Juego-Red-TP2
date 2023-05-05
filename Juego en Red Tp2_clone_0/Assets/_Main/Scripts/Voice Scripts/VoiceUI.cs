using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Voice.Unity;
using Photon.Realtime;

public class VoiceUI : MonoBehaviour
{
    public TextMeshProUGUI text;
    private Dictionary<Speaker, Player> speakerDic = new Dictionary<Speaker, Player>();
    
    public void AddSpeaker(Speaker speaker, Player player)
    {
        speakerDic[speaker] = player;
    }


    private void Update()
    {
        string voiceChat = "";

        foreach (var item in speakerDic)
        {
            var speaker = item.Key;
            if (speaker.IsPlaying)
            {
                voiceChat += item.Value.NickName + "\n";
            }
        }
        text.text = voiceChat;
    }
}
