using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoom : MonoBehaviour
{
    [SerializeField] public string nickname;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JoinOrCreateRoom(string roomName)
    {
        NetworkingScript.Instance.JoinOrCreateRoom(roomName);
        Debug.Log("calling join room ...");

        /*        if (!string.IsNullOrEmpty(nickname))
                {
                    return;
                }*/
    }
}
