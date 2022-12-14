using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MasterManager : MonoBehaviourPun
{
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
        GameObject obj = PhotonNetwork.Instantiate("Character", Vector3.zero, Quaternion.identity);
        var character = obj.GetComponent<CharacterModel>();
        dicChars[client] = character;
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
