using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddVote : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI VotedValueText;
    [SerializeField] private TextMeshProUGUI VotingOptionText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectVote()
    {
        VotedValueText.text = VotingOptionText.text;
    }
}
