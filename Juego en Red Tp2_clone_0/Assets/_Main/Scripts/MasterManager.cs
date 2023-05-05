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
    private CharacterModel[] winnerPlayer = new CharacterModel[1];

    private int counter = 0;
    private static MasterManager instance;

    Dictionary<int, CharacterModel> dicplayersToKill = new Dictionary<int, CharacterModel>();
    Dictionary<Player, CharacterModel> dicChars = new Dictionary<Player, CharacterModel>();
    Dictionary<CharacterModel, PlayerScore> dicVictory = new Dictionary<CharacterModel, PlayerScore>();
    Dictionary<string, int> nickToId = new Dictionary<string, int>();
    Dictionary<int, string> idToNick = new Dictionary<int, string>();
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
        GameObject obj = null;
        CharacterModel character = null;

        switch (PhotonNetwork.CurrentRoom.PlayerCount)
        {
            case 2:
                obj = PhotonNetwork.Instantiate("Character", posPlayers[0].position, posPlayers[0].rotation);
                character = obj.GetComponent<CharacterModel>();      
                break;
            case 3:
                obj = PhotonNetwork.Instantiate("Character", posPlayers[1].position, posPlayers[1].rotation);
                character = obj.GetComponent<CharacterModel>();
                break;
            case 4:
                obj = PhotonNetwork.Instantiate("Character", posPlayers[2].position, posPlayers[2].rotation);
                character = obj.GetComponent<CharacterModel>();
                break;
            case 5:
                obj = PhotonNetwork.Instantiate("Character", posPlayers[3].position, posPlayers[3].rotation);
                character = obj.GetComponent<CharacterModel>();
                break;
       
        }
        photonView.RPC("UpdatePlayer", RpcTarget.All, client, character.photonView.ViewID);
        //GameObject obj = PhotonNetwork.Instantiate("Character", posPlayers[counter].position,posPlayers[counter].rotation);
        //var character = obj.GetComponent<CharacterModel>();
        //photonView.RPC("UpdatePlayer", RpcTarget.All, client, character.photonView.ViewID);


        //dicplayersToKill[counter] = obj;
        //dicChars[client] = character;
        //dicVictory[obj] = playerScore[counter];
        //nickToId[client.NickName] = counter;
        //playerScore[counter].RPC_SetInitialScore();
        //if (counter == 0 || counter == 1)
        //{
        //    character.RPC_FreezeRigidBody(true);
        //}
        //else
        //{
        //    character.RPC_FreezeRigidBody(false);
        //}
        //counter++;
        //if (PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
        //{
        //    InitializeGame();
        //}
    }


    
    public void InitializeGame()
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
        PhotonNetwork.Instantiate("Fan Instantiator", Vector3.zero, Quaternion.identity);
    }

    [PunRPC]
    public void UpdatePlayer(Player client, int id)
    {
        PhotonView pv = PhotonView.Find(id);
        var character = pv.gameObject.GetComponent<CharacterModel>();
        dicplayersToKill[counter] = character;
        dicChars[client] = character;
        dicVictory[character] = playerScore[counter];
        nickToId[client.NickName] = counter;
        idToNick[counter] = client.NickName;
        playerScore[counter].RPC_SetInitialScore();
        
    }

    [PunRPC]
    public void UpdateCounter()
    {
        var character = dicplayersToKill[counter];
        if (counter == 0 || counter == 1)
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

    public void ActivateWalls(int index)
    {
        playersWalls.RPC_ActivateWalls(index);
    }

    [PunRPC]
    public void KillPlayer(int playerID)
    {
        var target = dicplayersToKill[playerID];
        target.photonView.RPC("Death", target.photonView.Owner);
        dicplayersToKill.Remove(playerID);
        idToNick.Remove(playerID);
        ActivateWalls(playerID);

        if(dicplayersToKill.Count == 1)
        {
            string playerNick = null;
            dicplayersToKill.Values.CopyTo(winnerPlayer, 0);
            foreach (var item in idToNick)
            {
                playerNick = item.Value;
            }
            var winner = dicVictory[winnerPlayer[0]]; 
            winner.RPC_Victory(playerNick);//esta bien este llamado?
        }
    }

    
    public void KillPlayerCommand(string nickName)
    {
        int aux = nickToId[nickName];
        KillPlayer(aux);
    }

    
    public void ChangeScoreCommand(string nickName, int newScore)
    {
        var id = nickToId[nickName];
        var player = dicplayersToKill[id];
        var playerScore = dicVictory[player];

        playerScore.RPC_ChageScore(newScore);
    }
    [PunRPC]
    public void InstantiateFan(Vector3 pos)
    {

        var fan = PhotonNetwork.Instantiate("Fan", pos, Quaternion.identity);
        fan.GetComponent<FanController>().Spin();
    }

    [PunRPC]
    public void InstantiateBall()
    {
        var ball = PhotonNetwork.Instantiate("Ball", Vector3.zero, Quaternion.identity);
        ball.GetComponent<BallController>().Launch();
    }

    
    public void InstantiateBall(int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            var ball = PhotonNetwork.Instantiate("Ball", Vector3.zero, Quaternion.identity);
            ball.GetComponent<BallController>().Launch();
        }
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
        RPC(name, PhotonNetwork.MasterClient, p);
    }

    public void RPC(string name, Player player, params object[] p)
    {
        photonView.RPC(name, player, p);
    }

}
