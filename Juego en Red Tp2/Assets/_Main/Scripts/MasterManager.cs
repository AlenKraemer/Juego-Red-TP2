using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class MasterManager : MonoBehaviourPun
{
    [SerializeField] private Transform[] posPlayers;
    [SerializeField] private PlayerScore[] playerScore;
    [SerializeField] private WallManager playersWalls;
    [SerializeField] private Timer timer;
    [SerializeField] private int timeToStart = 5;
    private List<GameObject> playersToKill = new List<GameObject>();
    private GameObject[] winnerPlayer = new GameObject[1];

    private int counter = 0;
    private static MasterManager instance;

    Dictionary<int, GameObject> dicplayersToKill = new Dictionary<int, GameObject>();
    Dictionary<Player, CharacterModel> dicChars = new Dictionary<Player, CharacterModel>();
    Dictionary<GameObject, PlayerScore> dicVictory = new Dictionary<GameObject, PlayerScore>();
    public static MasterManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    //Instancia del Personaje
    [PunRPC]
    public void RequestConnectPlayer(Player client)
    {

        GameObject obj = PhotonNetwork.Instantiate("Character", posPlayers[counter].position,posPlayers[counter].rotation);
        var character = obj.GetComponent<CharacterModel>();
        dicplayersToKill[counter] = obj;
        dicChars[client] = character;
        dicVictory[obj] = playerScore[counter];
        playerScore[counter].RPC_SetInitialScore();
        if(counter == 0 || counter == 1)
        {
            character.RPC_FreezeRigidBody(true);
        }
        else
        {
            character.RPC_FreezeRigidBody(false);
        }
        counter++;
        if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            InitializeGame();
        }
    }

    
    private void InitializeGame()
    {
        switch (PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            case 3:
                ActivateWalls(2);
                ActivateWalls(3);
                break;
            case 4:
                ActivateWalls(3);
                break;
            default:
                break;
        }

        timer.RPC_TimerStart(timeToStart);
        PhotonNetwork.Instantiate("Ball Instantiator", Vector3.zero, Quaternion.identity);
    }

    public void ActivateWalls(int index)
    {
        playersWalls.RPC_ActivateWalls(index);
    }

    [PunRPC]
    public void KillPlayer(int playerID)
    {
        PhotonNetwork.Destroy(dicplayersToKill[playerID]);
        dicplayersToKill.Remove(playerID);
        ActivateWalls(playerID);

        if(dicplayersToKill.Count == 1)
        {
            dicplayersToKill.Values.CopyTo(winnerPlayer, 0);

            var winner = dicVictory[winnerPlayer[0]];
            winner.RPC_Victory();
        }
    }

    [PunRPC]
    public void InstantiateBall()
    {
        PhotonNetwork.Instantiate("Ball", Vector3.zero, Quaternion.identity);
        
    }

    [PunRPC]
    public void DeleteBall(PhotonView ID)
    {
        PhotonNetwork.Destroy(ID);
    }

    //Movimiento del Personaje
    [PunRPC]
    public void RequestMove(Player client, Vector3 dir)
    {
        if (dicChars.ContainsKey(client))
        {
            var character = dicChars[client];
            character.Move(dir);
        }
    }

    //RPC propio para acortar tiempo
    public void RPCMaster(string name, params object[] p)
    {
        photonView.RPC(name, PhotonNetwork.MasterClient, p);
    }
 
}
