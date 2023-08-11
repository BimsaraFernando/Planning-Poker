using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoom : MonoBehaviour
{
    [SerializeField] public string nickname;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JoinOrCreateRoom(string NickName)
    {
        //NetworkingScript.Instance.JoinOrCreateRoom("Nickname");
        //Debug.Log("calling join room ...");

        Debug.Log("joining ...");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.JoinOrCreateRoom("Estimation", roomOptions, TypedLobby.Default);

    }
}
