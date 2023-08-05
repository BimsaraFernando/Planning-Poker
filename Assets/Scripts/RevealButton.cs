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

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
    }


    public void RevealAction()
    {
        
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

        isRevealed = !isRevealed; // Toggle the reveal statuscard.transform.Rotate(0f, 90f, 0f);
    }
}
