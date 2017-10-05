using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : Photon.PunBehaviour {

    public PhotonLogLevel logLevel = PhotonLogLevel.Informational; // Debugging purposes
    public byte maxPlayersPerRoom = 4;

    public GameObject controlPanel;
    public GameObject progressLabel;

    string _gameVersion = "1";
    bool isConnecting;

    void Awake()
    {
        PhotonNetwork.logLevel = logLevel;
        PhotonNetwork.autoJoinLobby = false; // Can view list of rooms without joining lobby
        PhotonNetwork.automaticallySyncScene = true; // Master client brings all others to to same scene

    }

    // Use this for initialization
    void Start () {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
	}

    public void Connect()
    {
        isConnecting = true;
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        if (PhotonNetwork.connected)
        {
            PhotonNetwork.JoinRandomRoom();
        } else
        {
            PhotonNetwork.ConnectUsingSettings(_gameVersion);
        }
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("Launcher: OnConnectedToMaster() was called by PUN");
        if (isConnecting)
            PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnectedFromPhoton()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        Debug.Log("Launcher: OnDisconnectedFromPhoton() was called by PUN");
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("Launcher: OnPhotonRandomJoinFailed() was called by PUN. No random room available. Creating one.");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = maxPlayersPerRoom }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Launcher: OnJoinedRoom() was called by PUN. Client is now in room.");

        if (PhotonNetwork.room.PlayerCount == 1)
        {
            Debug.Log("We load 'Room for 1'");
            PhotonNetwork.LoadLevel("Room for 1");
        }
    }
}