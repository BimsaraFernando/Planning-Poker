using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NetworkingScript : MonoBehaviourPunCallbacks
{
    [SerializeField] public TextMeshProUGUI connectingText;
    [SerializeField] public GameObject menuCanvas;
    [SerializeField] public GameObject gameCanvas;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        connectingText.text = "Connecting ...";
    }

    public override void OnConnectedToMaster()
    {
        gameCanvas.SetActive(true);
        menuCanvas.SetActive(false);
        connectingText.text = "Connected!";
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 20;
        PhotonNetwork.JoinOrCreateRoom("Estimation", roomOptions, TypedLobby.Default);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {

        gameCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        print("Disconnected from reason " + cause.ToString());
        connectingText.text = "Disconnected.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
