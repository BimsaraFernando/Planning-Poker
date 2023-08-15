using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    #region IPunObservable implementation
    public bool IsVoting = false;
    public TextMeshProUGUI Vote;
    public TextMeshProUGUI playerNameText;

    [SerializeField] private bool isRevealed = false;
    [SerializeField] private bool hasVoted = false;
    [SerializeField] private GameObject rotatingItems;
    [SerializeField] private GameObject checkmark;

    private Quaternion initialRotation = Quaternion.Euler(0f, 0f, 0f); // Initial rotation of the card

    void Update()
    {
        if (hasVoted)
        {
            checkmark.SetActive(true);
        }
        else
        {
            checkmark.SetActive(false);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(Vote.text);
            stream.SendNext(hasVoted);
            stream.SendNext(playerNameText.text);
        }
        else
        {
            // Network player, receive data
            this.Vote.text = (string)stream.ReceiveNext();
            this.hasVoted = (bool)stream.ReceiveNext();
            this.playerNameText.text = (string)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void SelectVoteRPC(string newVote)
    {
        if (photonView.IsMine)
        {
            Vote.text = newVote;
            hasVoted = true;
        }
    }

    public void SelectVote(string newVote)
    {
        if (photonView.IsMine)
            Vote.text = newVote;
        photonView.RPC("SelectVoteRPC", RpcTarget.All, newVote);
    }

    [PunRPC]
    public void RevealCards()
    {
        if (isRevealed)
        {
            StartCoroutine(RotateCardSmoothly(initialRotation));
            hasVoted = false;
            Vote.text = "";
        }
        else
        {
            Quaternion targetRotation = initialRotation * Quaternion.Euler(0f, 180f, 0f);
            StartCoroutine(RotateCardSmoothly(targetRotation));
        }

        isRevealed = !isRevealed; // Toggle the reveal status
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
    #endregion
}
