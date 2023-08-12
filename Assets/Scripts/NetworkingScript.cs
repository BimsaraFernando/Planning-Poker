using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NetworkingScript : MonoBehaviourPunCallbacks
{
    [SerializeField] public TextMeshProUGUI connectingText;
    [SerializeField] public GameObject ConnetMenuItems;
    [SerializeField] public GameObject GamePlayItems;
    [SerializeField] public GameObject JoinRoomMenuItems;
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

    public static List<int> availableSpawnIndices = new List<int> { 0,1,2,3,4,5,6,7,8,9};
    public static NetworkingScript Instance;


    // Start is called before the first frame update
    void Start()
    {
        connectingText.text = "Connecting ...";

    }
    private void Awake()
    {
        Instance = this;
    }

    public override void OnConnectedToMaster()
    {
        ConnetMenuItems.SetActive(false);
        JoinRoomMenuItems.SetActive(true);
        connectingText.text = "Connected!";
    }

    public void JoinOrCreateRoom()
    {
        Debug.Log("joining ...");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.JoinOrCreateRoom("Estimation", roomOptions, TypedLobby.Default);

    }

    public override void OnDisconnected(DisconnectCause cause)
    {

        GamePlayItems.SetActive(false);
        ConnetMenuItems.SetActive(true);
        print("Disconnected from reason " + cause.ToString());
        connectingText.text = "Disconnected.";
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        GamePlayItems.SetActive(true);
        JoinRoomMenuItems.SetActive(false);

        if(availableSpawnIndices.Count > 0) { 
            // Spawn the player when they join the room
            SpawnPlayer();
        }
    }

    [PunRPC]
    private void SpawnPlayer()
    {
        // Instantiate the player prefab and position it in a spawn point
        Vector3 spawnPosition = GetSpawnPosition();
        myCard = PhotonNetwork.Instantiate("Player", spawnPosition, Quaternion.identity);
        nickname = PhotonNetwork.LocalPlayer.NickName;
        myCard.transform.Find("PlayerNameText").GetComponent<TextMeshProUGUI>().text   = nickname;
        photonView.RPC("SetCanvasAsParentRPC", RpcTarget.All);
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

    public void SetCanvasAsParent()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(Players.Length);
        if (Players.Length>0) { 
        for(int i = 0; i < Players.Length; i++)
        {
            Players[i].transform.SetParent(GamePlayItems.transform);
            }
        }
    }

    private Vector3 GetSpawnPosition()
    {

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