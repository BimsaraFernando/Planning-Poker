using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VotingOption : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI VotingOptionText;
    public void SelectVote()
    {
        GameManager.Instance.AddVote(VotingOptionText.text);
    }


}
