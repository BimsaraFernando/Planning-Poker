using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;

public class NetworkingScript : MonoBehaviourPunCallbacks
{
    [SerializeField] public TextMeshProUGUI connectingText;
    [SerializeField] public GameObject ConnetMenuItems;
    [SerializeField] public GameObject GamePlayItems;
    [SerializeField] public GameObject JoinRoomMenuItems;
    [SerializeField] public string nickname;

    public GameObject myCard;
    public TextMeshProUGUI onlineCount;
    public int NextSpawnIndex = 0;

    [SerializeField] public GameObject[] Players;

    //to be done - mirror the values
    public static Vector3[] spawnPositions = new Vector3[] {
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

    public static NetworkingScript Instance;


    // Start is called before the first frame update
    void Start()
    {
        connectingText.text = "Ready ...";

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

        ExitGames.Client.Photon.Hashtable initialRoomProperties = new ExitGames.Client.Photon.Hashtable();
        initialRoomProperties["NextSpawnIndex"] = 0;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 10;
        roomOptions.CustomRoomProperties = initialRoomProperties;
        PhotonNetwork.JoinOrCreateRoom("Estimation", roomOptions, TypedLobby.Default);

    }

    public override void OnJoinedRoom()
    {
        ExitGames.Client.Photon.Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        if (roomProperties.ContainsKey("NextSpawnIndex"))
        {
            NextSpawnIndex = (int)roomProperties["NextSpawnIndex"];
            NextSpawnIndex = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        }

        GamePlayItems.SetActive(true);
        JoinRoomMenuItems.SetActive(false);

        int nextSpawnIndex = GameManager.Instance.GetNextSpawnIndex();

        if (nextSpawnIndex < 11)
        {
            Vector3 spawnposition = GetSpawnPosition(nextSpawnIndex);
            photonView.RPC("Updatespawnindex", RpcTarget.All);

            // Spawn the player when they join the room
            SpawnPlayer(spawnposition);
        }
    }
    [PunRPC]
    public void Updatespawnindex()
    {
        ExitGames.Client.Photon.Hashtable updatedProperties = new ExitGames.Client.Photon.Hashtable();
        updatedProperties["NextSpawnIndex"] = ++NextSpawnIndex;
        // Add more variables
        PhotonNetwork.CurrentRoom.SetCustomProperties(updatedProperties);
    }


    [PunRPC]
    private void SpawnPlayer(Vector3 spawnPosition)
    {
        // Instantiate the player prefab and position it in a spawn point
        if (spawnPosition != Vector3.zero)
        {
            myCard = PhotonNetwork.Instantiate("Player", spawnPosition, Quaternion.identity);
            myCard.transform.Find("PlayerNameText").GetComponent<TextMeshProUGUI>().text = myCard.GetComponent<PhotonView>().Owner.NickName;
            photonView.RPC("SetCanvasAsParentRPC", RpcTarget.All);
        }
    }


    [PunRPC]
    public void SetCanvasAsParentRPC()
    {
        StartCoroutine(checkedOtherExistingPlayers());
    }


    IEnumerator checkedOtherExistingPlayers()
    {
        yield return new WaitForSeconds(1);
        SetCanvasAsParent();
    }

    public void SetCanvasAsParent()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        if (Players.Length > 0)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].transform.SetParent(GamePlayItems.transform);
            }
        }
    }

    private Vector3 GetSpawnPosition(int spawnIndex)
    {
        if (spawnIndex >= 0)
        {
            return spawnPositions[spawnIndex];

        }

        else
        {
            //add checking any empty spawn positions logic here
            Debug.LogWarning("No available spawn positions.");
            return Vector3.zero; // Return a default position if no spawn position is available
        }
    }

    public void AddVote(string VotingOptionText)
    {
        myCard.GetComponent<Player>().SelectVote(VotingOptionText);
    }


    private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            NetworkingScript.Instance.onlineCount.text = "Online :" + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {

        GamePlayItems.SetActive(false);
        ConnetMenuItems.SetActive(true);
        print("Disconnected from reason " + cause.ToString());
        connectingText.text = "Disconnected.";
    }
}