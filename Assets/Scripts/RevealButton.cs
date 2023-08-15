using Photon.Pun;
using TMPro;
using UnityEngine;

public class RevealButton : MonoBehaviourPun
{
    [SerializeField] private GameObject VotingOptionsList;
    [SerializeField] private bool isRevealed = false;
    [SerializeField] public TextMeshProUGUI revealButtonText;
    [SerializeField] public GameObject[] Players;

    [PunRPC]
    public void RevealCards()
    {
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
    }
}
