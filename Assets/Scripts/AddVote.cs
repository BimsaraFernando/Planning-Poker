using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddVote : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI VotingOptionText;

    public void SelectVote()
    {
        NetworkingScript.Instance.AddVote(VotingOptionText.text);
        //photonView.RPC("SelectVoteRPC", RpcTarget.AllBuffered, VotingOptionText.text);
    }


}
