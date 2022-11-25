using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetManager : MonoBehaviourPunCallbacks

{
    [SerializeField] private Button hostRoomButton;
    [SerializeField] private Button joinRoomButton;
    [SerializeField] private Button hostRoomButtonActivate;
    [SerializeField] private Button joinRoomButtonActivate;
    [SerializeField] private GameObject joinRoom;
    [SerializeField] private GameObject hostRoom;
    [SerializeField] private TextMeshProUGUI status;
    [SerializeField] private TMP_InputField nicknameHost;
    [SerializeField] private TMP_InputField nicknameJoin;
    [SerializeField] private TMP_InputField maxPlayers;
    [SerializeField] private TMP_InputField roomNameHost;
    [SerializeField] private TMP_InputField roomNameJoin;
    [SerializeField] private string[] randomNickname;

    public GameObject Panel;


    private void Start()
    {       
        PhotonNetwork.ConnectUsingSettings();
        hostRoomButtonActivate.interactable = false;
        joinRoomButtonActivate.interactable = false;
        status.text = "Connecting to Master";
    }
    public void OpenPanel()
    {
        if (Panel != null)
        {
            bool isActive = Panel.activeSelf;

            Panel.SetActive(!isActive);
            Debug.Log("IsOpenedPanel");
        }
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
        OnJoinedLobby();
        OnConnectedToMaster();
    }
    public void ExitGame()
    {
        //Application.Quit();
        Debug.Log("Exit Game");
    }
    public override void OnConnectedToMaster()
    {
        hostRoomButtonActivate.interactable = false;
        joinRoomButtonActivate.interactable = false;
        PhotonNetwork.JoinLobby();
        status.text = "Connecting to Lobby";
    }

    public override void OnJoinedLobby()
    {
        hostRoomButtonActivate.interactable = true;
        joinRoomButtonActivate.interactable = true;
        status.text = "Connected to Lobby";
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        status.text = "Conection Failed";
    }
    public override void OnLeftLobby()
    {
        status.text = "Conection to Lobby Failed";
    }

    public override void OnCreatedRoom()
    {
        status.text = "Room Created";
    }
    public override void OnJoinedRoom()
    {
        status.text = "Joined Room";
        PhotonNetwork.LoadLevel("Level");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        status.text = "Cannot Join Room";
        joinRoomButton.interactable = true;
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        status.text = "Cannot Create Room";
        hostRoomButton.interactable = true;
    }
    public void OnHost()
    {
        if (string.IsNullOrEmpty(roomNameHost.text) || string.IsNullOrWhiteSpace(roomNameHost.text)) return;
        if (string.IsNullOrEmpty(nicknameHost.text) || string.IsNullOrWhiteSpace(nicknameHost.text)) return;
        if (string.IsNullOrEmpty(maxPlayers.text) || string.IsNullOrWhiteSpace(maxPlayers.text)) return;

        PhotonNetwork.NickName = nicknameHost.text;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = byte.Parse(maxPlayers.text);
        options.IsOpen = true;
        options.IsVisible = true;
        PhotonNetwork.JoinOrCreateRoom(roomNameHost.text, options, TypedLobby.Default);
        hostRoomButton.interactable = false;
    } 
    public void OnJoin()
    {
        if (string.IsNullOrEmpty(roomNameJoin.text) || string.IsNullOrWhiteSpace(roomNameJoin.text)) return;
        if (string.IsNullOrEmpty(nicknameJoin.text) || string.IsNullOrWhiteSpace(nicknameJoin.text)) return;

        PhotonNetwork.NickName = nicknameJoin.text;

        RoomOptions options = new RoomOptions();
        PhotonNetwork.JoinRoom(roomNameJoin.text);
        joinRoomButton.interactable = false;
    }

    public void HostRoomButton()
    {
        nicknameHost.text = randomNickname[Random.Range(0, randomNickname.Length - 1)];
        hostRoom.SetActive(true);
        hostRoomButtonActivate.gameObject.SetActive(false);
        joinRoomButtonActivate.gameObject.SetActive(false);

    }

    public void JoinRoomButton()
    {
        nicknameJoin.text = randomNickname[Random.Range(0, randomNickname.Length - 1)];
        joinRoom.SetActive(true);
        joinRoomButtonActivate.gameObject.SetActive(false);
        hostRoomButtonActivate.gameObject.SetActive(false);
    }

    public void BackButton(GameObject currentRoom)
    {
        currentRoom.SetActive(false);
        hostRoomButtonActivate.gameObject.SetActive(true);
        joinRoomButtonActivate.gameObject.SetActive(true);
    }

    
}
