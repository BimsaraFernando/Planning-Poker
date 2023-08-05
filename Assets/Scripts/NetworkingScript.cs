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
    private int nextSpawnIndex = 0;

    public Transform[] spawnPositions = new Transform[10];
    [SerializeField] public bool[] SpawnPositionsAvailability = { true, true, true, true, true, true, true, true, true, true };

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

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);

        // Spawn the player when they join the room
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        // Instantiate the player prefab and position it in a spawn point
        Vector3 spawnPosition = GetSpawnPosition();
        PhotonNetwork.Instantiate("Player", spawnPosition, Quaternion.identity);

    }

    private Vector3 GetSpawnPosition()
    {
        if (nextSpawnIndex < spawnPositions.Length)
        {
            Vector3 spawnPosition = spawnPositions[nextSpawnIndex].position;
            nextSpawnIndex++;
            return spawnPosition;
        }
        else
        {
            Debug.LogWarning("No available spawn positions.");
            return Vector3.zero; // Return a default position if no spawn position is available
        }
    }
}
