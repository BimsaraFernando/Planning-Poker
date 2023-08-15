using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI connectingText;
    public GameObject ConnetMenuItems;
    public GameObject GamePlayItems;
    public GameObject JoinRoomMenuItems;
    public string nickname;

    public GameObject myCard;
    public TextMeshProUGUI onlineCount;
    public int NextSpawnIndex = 0;
    public GameObject[] Players;
    Vector3 spawnposition = new Vector3(-399f, 114f, 0f);

    public static GameManager Instance;

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
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 10;
        roomOptions.CustomRoomProperties = initialRoomProperties;
        PhotonNetwork.JoinOrCreateRoom("Estimation", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        GamePlayItems.SetActive(true);
        JoinRoomMenuItems.SetActive(false);

        // Spawn the player when they join the room
        SpawnPlayer(spawnposition);
    }


    [PunRPC]
    private void SpawnPlayer(Vector3 spawnPosition)
    {
        // Instantiate the player prefab and position it in the spawn point
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
        StartCoroutine(CheckedOtherExistingPlayers());
    }

    IEnumerator CheckedOtherExistingPlayers()
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

    public void AddVote(string VotingOptionText)
    {
        myCard.GetComponent<Player>().SelectVote(VotingOptionText);
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            GameManager.Instance.onlineCount.text = "Online :" + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
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