using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    //[SerializeField] public TextMeshProUGUI TotalPlayersCount;
    public int nextSpawnIndex { get; set; }

    public int GetNextSpawnIndex()
    {
        return nextSpawnIndex;
    }

    public void SetNextSpawnIndex()
    {
        nextSpawnIndex++;
    }

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TotalPlayersCount.text = "Total Online :" + PhotonNetwork.CountOfPlayers.ToString();
    }

    void NewVoteAdded() {


    }
}
