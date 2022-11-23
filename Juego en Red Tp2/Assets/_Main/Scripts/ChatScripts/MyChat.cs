using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class MyChat : MonoBehaviourPun
{
    public TextMeshProUGUI content;
    public TMP_InputField inputField;
    string _command = "/PM";

    public void ChatSendMessage()
    {
        var message = inputField.text;
        if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message)) return;
        string[] words = message.Split(' ');

        if(words.Length > 2 && words [0] == _command)
        {
            var target = words[1];
            foreach(var currPlayer in PhotonNetwork.PlayerList)
            {               
                if (target == currPlayer.NickName)
                {
                    var currMessage = string.Join("", words, 2, words.Length - 2);
                    photonView.RPC(nameof(GetChatMessage), currPlayer, PhotonNetwork.NickName,currMessage, true);
                    GetChatMessage(PhotonNetwork.NickName, currMessage);
                    return;
                }
            }
            content.text += "<color=pink>" + "No existe" + "</color>" + "\n";
            inputField.text = "";
        }
        else
        {
            photonView.RPC(nameof(GetChatMessage), RpcTarget.All, PhotonNetwork.NickName, message,false);
            inputField.text = "";
        }
    }

    [PunRPC]
    public void GetChatMessage(string nameClient, string message, bool pm = false)
    {
        string color;
        if(PhotonNetwork.NickName == nameClient)
        {
            color = "<color=blue>";           
        }
        else if (pm)
        {
            color = "<color=green>";
        }
        else
        {
            color = "<color=red>";          
        }
        content.text += color + nameClient + ": " + "</color>" + message + "\n";
    }
}
