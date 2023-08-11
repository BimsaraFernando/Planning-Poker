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
    [SerializeField] public string nickname;

    public static int nextSpawnIndex = 0;
    public GameObject myCard;
    public TextMeshProUGUI onlineCount;
    private PhotonView PV;

    [SerializeField] public GameObject[] Players;


    [SerializeField] public static Vector3[] spawnPositions = new Vector3[] { 
        new Vector3(-399f, 114f, 0f),
        new Vector3(-312f,114f,0f),
        new Vector3(-224f,114f,0f),
        new Vector3(-138f,114f,0f),
        new Vector3(-49f,114f,0f),
        new Vector3(34f,114f,0f),
        new Vector3(124f,114f,0f),
        new Vector3(210.5f,114f,0f),
        new Vector3(300f,114f,0f),
        new Vector3(391f,114f,0f)
    };
    //[SerializeField] public static GameObject[] spawnPositionsObjects = new GameObject[10];

    public static List<int> availableSpawnIndices = new List<int> { 0,1,2,3,4,5,6,7,8,9};
    public static NetworkingScript Instance;


    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        connectingText.text = "Connecting ...";

        //for(int i = 0;i < spawnPositions.Length; i++)
        //{
        //    spawnPositions[i] = spawnPositionsObjects[i].transform.position;
        //}

    }
    private void Awake()
    {
        Instance = this;
    }

    public override void OnConnectedToMaster()
    {
        gameCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        connectingText.text = "Connected!";


    }

/*    public void JoinOrCreateRoom(string nickname)
    {
        Debug.Log("joining ...");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.JoinOrCreateRoom("Estimation", roomOptions, TypedLobby.Default);

    }*/

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
        gameCanvas.SetActive(true);
        menuCanvas.SetActive(false);

        if(availableSpawnIndices.Count > 0) { 
            // Spawn the player when they join the room
            SpawnPlayer();
        }
    }

    [PunRPC]
    private void SpawnPlayer()
    {
        //spawnPositionsObjects = GameObject.FindGameObjectsWithTag("SpawnPosition");

        // Instantiate the player prefab and position it in a spawn point
        Vector3 spawnPosition = GetSpawnPosition();
        myCard = PhotonNetwork.Instantiate("Player", spawnPosition, Quaternion.identity);
        photonView.RPC("SetCanvasAsParentRPC", RpcTarget.All);

        //StartCoroutine(checkedOtherExistingPlayers());

    }

    [PunRPC]
    public void SetCanvasAsParentRPC()
    {
        Debug.Log("setCanvasAsParentRPC");
        StartCoroutine(checkedOtherExistingPlayers());
    }


    IEnumerator checkedOtherExistingPlayers()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("setCanvasAsParent");
        SetCanvasAsParent();
    }

    /*    public void OnPlayerEnteredRoom(Player newPlayer)   
        {
            newPlayer.transform.SetParent(gameCanvas.transform);

        }*/

    public void SetCanvasAsParent()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(Players.Length);
        if (Players.Length>0) { 
        for(int i = 0; i < Players.Length; i++)
        {
            Players[i].transform.SetParent(gameCanvas.transform);
            //Players[i]..FindGameObjectsWithTag("PlayerName").text = PhotonNetwork.PlayerList..UserId.Substring(0, 4);

            }
        }

    }

    private Vector3 GetSpawnPosition()
    {
        /*        if (PhotonNetwork.CurrentRoom.PlayerCount<11)
                {
                    Vector3 spawnPosition = spawnPositions[PhotonNetwork.CurrentRoom.PlayerCount];

                    //Vector3 spawnPosition = spawnPositions[nextSpawnIndex];
                    nextSpawnIndex++;
                    return spawnPosition;
                }*/

        if (availableSpawnIndices.Count > 0)
        {
            int spawnIndex = availableSpawnIndices[0];
            availableSpawnIndices.RemoveAt(0);
            return spawnPositions[spawnIndex];
        }
        else
        {
            //add checking any empty spawn positions logic here
            Debug.LogWarning("No available spawn positions.");
            return Vector3.zero; // Return a default position if no spawn position is available
        }
    }

    public void AddVote( string VotingOptionText)
    {
        myCard.GetComponent<Player>().SelectVote(VotingOptionText);
    }


    private void Update()
    {
        if(PhotonNetwork.InRoom) { 
        NetworkingScript.Instance.onlineCount.text = "Online :" + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        }
    }
}
