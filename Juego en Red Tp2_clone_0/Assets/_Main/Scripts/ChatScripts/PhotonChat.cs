using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;
using TMPro;


public class PhotonChat : MonoBehaviour, IChatClientListener
{
    public TextMeshProUGUI content;
    public TMP_InputField inputField;
    private ChatClient _chatClient;

    private string _commandPm = "/PM";
    private string _commandKill = "/Kill";
    private string _commandFontSize = "/FontSize";
    private string _commandMute = "/Mute";
    private string _commandUnMute = "/UnMute";
    private string _commandScore = "/Score";
    private string _commandBall = "/Ball";
    private string _channel;
    private List<string> muteList = new List<string>();

    private void Start()
    {
        _chatClient = new ChatClient(this);
        _chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, 
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion, 
            new AuthenticationValues(PhotonNetwork.NickName));
    }
    private void Update()
    {
        _chatClient.Service();
    }
    public void ChatSendMessage()
    {
        var message = inputField.text;
        if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message)) return;
        string[] words = message.Split(' ');
        

         
        if (PhotonNetwork.IsMasterClient)
        {
            //comando Kill
            if (words.Length > 1 && words[0] == _commandKill)
            {
                var target = words[1];
                foreach (var currPlayer in PhotonNetwork.PlayerList)
                {
                    if (target == currPlayer.NickName)
                    {
                        //MasterManager.Instance.RPCMaster("KillPlayerCommand", target);//esta bien esta llamada?
                        MasterManager.Instance.KillPlayerCommand(target);
                        inputField.text = "";
                        return;
                    }
                }
            }

            //comando ChangeScore
            if (words.Length > 2 && words[0] == _commandScore)
            {
                var target = words[1];
                var score = int.Parse(words[2]);

                //MasterManager.Instance.RPCMaster("ChangeScoreCommand", target, score);//esta bien esta llamada?
                MasterManager.Instance.ChangeScoreCommand(target, score);
                inputField.text = "";
                return;
            }

            //comando InstantiateBall
            if(words.Length > 1 && words[0] == _commandBall)
            {
                var quantity = int.Parse(words[1]);

                //MasterManager.Instance.RPCMaster("InstantiateBall", quantity);//esta bien esta llamada?
                MasterManager.Instance.InstantiateBall(quantity);
                inputField.text = "";
                return;
            }
        }

        //comando Mute
        if (words.Length > 1 && words[0] == _commandMute)
        {
            var target = words[1];
            foreach (var currPlayer in PhotonNetwork.PlayerList)
            {
                if (target == currPlayer.NickName)
                {
                    if (muteList.Contains(target))
                    {
                        content.text += "El jugador " + target + " ya se encuentra muteado" + "\n";
                        inputField.text = "";
                        return;
                    }
                    else
                    {
                        muteList.Add(target);
                        content.text += "No recibirás mas mensajes de " + target + "\n";
                        inputField.text = "";
                        return;
                    }
                }
            }
        }

        //comando UnMute
        if (words.Length > 1 && words[0] == _commandUnMute)
        {
            var target = words[1];
            foreach (var currPlayer in PhotonNetwork.PlayerList)
            {
                if (target == currPlayer.NickName)
                {
                    if (muteList.Contains(target))
                    {
                        muteList.Remove(target);
                        content.text += "Volverás a recibir mensajes de " + target + "\n";
                        inputField.text = "";
                        return;
                    }
                    else
                    {
                        content.text += "El jugador " + target + " no se encuentra muteado" + "\n";
                        inputField.text = "";
                        return;
                    }
                }
            }
        }

        //comando FontSize
        if (words.Length > 1 && words[0] == _commandFontSize)
        {
            var target = words[1];
            content.fontSize = int.Parse(target);
            inputField.text = "";
            return;
                
        }

        //comando PM
        if (words.Length > 2 && words[0] == _commandPm)
        {
            var target = words[1];
            foreach (var currPlayer in PhotonNetwork.PlayerList)
            {
                if (target == currPlayer.NickName)
                {
                    var currMessage = string.Join("", words, 2, words.Length - 2);
                    _chatClient.SendPrivateMessage(target, currMessage);
                    return;
                }
            }
            content.text += "No existe" + "\n";
            inputField.text = "";
        }
        else
        {
            _chatClient.PublishMessage(_channel, message);
            inputField.text = "";
        }
    }
    public void DebugReturn(DebugLevel level, string message)
    {
        
    }

    public void OnChatStateChange(ChatState state)
    {
      
    }

    public void OnConnected()
    {
        content.text += "Se pudo conectar" + "\n";
        _channel = PhotonNetwork.CurrentRoom.Name;
        _chatClient.Subscribe(_channel);
    }

    public void OnDisconnected()
    {
        content.text += "No se pudo conectar" + "\n";
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for(int i = 0; i <senders.Length; i++)
        {
            var currSender = senders[i];          
            string color;

            if (muteList.Contains(currSender)) return;
          
            if (PhotonNetwork.NickName == currSender)
            {
                color = "<color=blue>";        
            }         
            else
            {
                color = "<color=red>";                   
            }
          
            content.text += color + currSender + ": " + "</color>" + messages[i] + "\n";         
        }   
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {                 
        content.text += "<color=yellow>"+ sender + ": " + "</color>" + message + "\n";
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {

    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
      for(int i = 0; i < channels.Length; i++)
        {
            content.text += "Suscribe to " + channels[i] + content.text + "\n";
        }
    }

    public void OnUnsubscribed(string[] channels)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            content.text += "OnUnsubscribed to " + channels[i] + content.text + "\n";
        }
    }

    public void OnUserSubscribed(string channel, string user)
    {
       
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
       
    }

   
}
