using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    #region IPunObservable implementation
    public bool IsVoting = false;
    [SerializeField] public TextMeshProUGUI Vote ;
    [SerializeField] private bool isRevealed = false;
    [SerializeField] private bool isVoted = false;
    [SerializeField] public TextMeshProUGUI playerNameText;
    private Quaternion initialRotation = Quaternion.Euler(0f, 0f, 0f); // Initial rotation of the card
    [SerializeField] private GameObject rotatingItems;

    public void Start()
    {
        if (photonView.IsMine)
        {
            //PhotonNetwork.LocalPlayer.NickName = PhotonNetwork.LocalPlayer.UserId.Substring(0, 4);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(Vote.text);
            //stream.SendNext(playerNameText.text);
        }
        else
        {
            // Network player, receive data
            this.Vote.text = (string)stream.ReceiveNext();
            //this.playerNameText.text = (string)stream.ReceiveNext();
        }
    }
    [PunRPC]
    public void RevealCards()
    {
        //rotatingItems.transform.Rotate(0f, 180f, 0f);
        if (isRevealed)
        {
            StartCoroutine(RotateCardSmoothly(initialRotation));
        }
        else
        {
            Quaternion targetRotation = initialRotation * Quaternion.Euler(0f, 180f, 0f);
            StartCoroutine(RotateCardSmoothly(targetRotation));
        }

        isRevealed = !isRevealed; // Toggle the reveal statuscard.transform.Rotate(0f, 90f, 0f);
    }
    private IEnumerator RotateCardSmoothly(Quaternion targetRotation)
    {
        float startTime = Time.time;
        float duration = 0.75f; // Adjust the duration of the rotation

        Quaternion startRotation = rotatingItems.transform.rotation;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            rotatingItems.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }

        // Ensure the rotation is exactly the target rotation
        rotatingItems.transform.rotation = targetRotation;
    }

    [PunRPC]
    public void SelectVoteRPC(string newVote)
    {
        if (photonView.IsMine)
        {
            Vote.text = newVote;
            isVoted = true;
        }
    }

    public void SelectVote(string newVote)
    {
        if (photonView.IsMine)
            Vote.text = newVote;
        photonView.RPC("SelectVoteRPC", RpcTarget.All, newVote);
    }

    #endregion
}
