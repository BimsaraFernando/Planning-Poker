using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoinRoom : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI nickname;

    public void ConnectUsingSettings()
    {
        PhotonNetwork.NickName = nickname.text;
        PhotonNetwork.ConnectUsingSettings();
    }
    public void JoinOrCreateRoom()
    {
        NetworkingScript.Instance.JoinOrCreateRoom();
    }



}