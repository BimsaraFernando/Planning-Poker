using Photon.Pun;
using TMPro;
using UnityEngine;

public class RevealButton : MonoBehaviourPun
{
    [SerializeField] private GameObject VotingOptionsList;
    [SerializeField] private bool isRevealed = false;
    [SerializeField] public TextMeshProUGUI revealButtonText;
    [SerializeField]  public GameObject[] Players;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
    }

    [PunRPC]
    public void RevealCards()
    {
        //if (Players.Length == 0 || Players.Length!= PhotonNetwork.CurrentRoom.PlayerCount)
        Players = GameObject.FindGameObjectsWithTag("Player");

        if (isRevealed)
        {
            VotingOptionsList.SetActive(true);
            revealButtonText.text = "Reveal";
        }
        else
        {
            VotingOptionsList.SetActive(false);
            revealButtonText.text = "Restart";
        }

        isRevealed = !isRevealed;

        foreach (GameObject selectedPlayer in Players)
        {
            selectedPlayer.GetComponent<Player>().RevealCards();
        }
    }


    public void RevealAction()
    {
        photonView.RPC("RevealCards", RpcTarget.All);
 // Toggle the reveal statuscard.transform.Rotate(0f, 90f, 0f);
    }
}
