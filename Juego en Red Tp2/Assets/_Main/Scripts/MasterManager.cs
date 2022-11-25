using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class MasterManager : MonoBehaviourPun
{
    [SerializeField] private Transform[] posPlayers;
    [SerializeField] private WallManager playersWalls;
    private int counter = 0;
    private static MasterManager instance;
    Dictionary<Player, CharacterModel> dicChars = new Dictionary<Player, CharacterModel>();
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
        dicChars[client] = character;
        if(counter < 2)
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
        //timer para que inicialice
        PhotonNetwork.Instantiate("Ball", Vector3.zero, Quaternion.identity);
    }

    public void ActivateWalls(int index)
    {
        playersWalls.RPC_ActivateWalls(index);
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
