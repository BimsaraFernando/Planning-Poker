using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RevealButton : MonoBehaviour
{
    [SerializeField] private GameObject VotingOptionsList;
    [SerializeField] private GameObject card;
    [SerializeField] private bool isRevealed = false;
    [SerializeField] public TextMeshProUGUI revealButtonText;
    private Quaternion initialRotation; // Initial rotation of the card

    // Start is called before the first frame update
    void Start()
    {
        initialRotation = transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
    }


    public void RevealAction()
    {
        
        if (isRevealed)
        {
            StartCoroutine(RotateCardSmoothly(initialRotation));
            VotingOptionsList.SetActive(true);
            revealButtonText.text = "Reveal";
        }
        else
        {
            Quaternion targetRotation = initialRotation * Quaternion.Euler(0f, 180f, 0f);
            StartCoroutine(RotateCardSmoothly(targetRotation));
            VotingOptionsList.SetActive(false);
            revealButtonText.text = "Restart";
        }

        isRevealed = !isRevealed; // Toggle the reveal statuscard.transform.Rotate(0f, 90f, 0f);
    }
    private IEnumerator RotateCardSmoothly(Quaternion targetRotation)
    {
        float startTime = Time.time;
        float duration = 1.0f; // Adjust the duration of the rotation

        Quaternion startRotation = card.transform.rotation;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            card.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }

        // Ensure the rotation is exactly the target rotation
        card.transform.rotation = targetRotation;
    }
}
