using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddVote : MonoBehaviourPunCallbacks
{
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
        NetworkingScript.Instance.AddVote(VotingOptionText.text);
            //photonView.RPC("SelectVoteRPC", RpcTarget.AllBuffered, VotingOptionText.text);
    }


}
