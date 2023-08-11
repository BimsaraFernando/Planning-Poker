
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System;
using TMPro;

public class TestConnect : MonoBehaviourPunCallbacks
{
    [SerializeField] public TextMeshProUGUI _LoadingText;
    [SerializeField] public TextMeshProUGUI _playerNameInput;

    [SerializeField] public GameObject _startButton;

    public override void OnConnectedToMaster()
    {
        print("connected!");

        _startButton.SetActive(true);
        _LoadingText.text = "Connected!";
        //_playerName.text = PhotonNetwork.LocalPlayer.NickName;
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected from reason " + cause.ToString());
        // base.OnDisconnected(cause);
    }




    public void onclick_start_items()
    {
        if (!String.IsNullOrEmpty(_playerNameInput.text))
        {
            PhotonNetwork.NickName = _playerNameInput.text;
            PhotonNetwork.GameVersion = "0.0.1";
            PhotonNetwork.ConnectUsingSettings();
            _startButton.SetActive(false);
        }

    }
}