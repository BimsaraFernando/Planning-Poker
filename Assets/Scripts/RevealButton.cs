using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        if (Players == null)
            Players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject selectedPlayer in Players)
        {
            selectedPlayer.GetComponent<Player>().RevealCards();
        }
    }


    public void RevealAction()
    {
        
        if (isRevealed)
        {
            VotingOptionsList.SetActive(true);
            revealButtonText.text = "Reveal";
            photonView.RPC("RevealCards", RpcTarget.All);
        }
        else
        {
            VotingOptionsList.SetActive(false);
            revealButtonText.text = "Restart";
            photonView.RPC("RevealCards", RpcTarget.All);
        }

        isRevealed = !isRevealed; // Toggle the reveal statuscard.transform.Rotate(0f, 90f, 0f);
    }
}
