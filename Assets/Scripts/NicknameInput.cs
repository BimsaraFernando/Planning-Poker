using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NicknameInput : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI nicknameDisplay;

    private void Start()
    {
        // Hook up the InputField's onValueChanged event to update the nickname display.
        inputField.onValueChanged.AddListener(UpdateNicknameDisplay);
    }

    public void UpdateNicknameDisplay(string newNickname)
    {
        nicknameDisplay.text = newNickname;
    }

    public string GetNickname()
    {
        return inputField.text;
    }
}
