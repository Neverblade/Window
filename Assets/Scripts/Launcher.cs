using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class Launcher : Photon.PunBehaviour {

    // Entity Prefabs
    public GameObject oculusEntityPrefab;  
    public GameObject pcEntityPrefab;
    public GameObject mobileEntityPrefab;

    // Photon Settings
    public int logLevel = 3;
    public byte maxPlayers = 4;
    string _gameVersion = "1";

    Entity entity;

    void Awake()
    {
        PhotonNetwork.autoJoinLobby = false;
        PhotonNetwork.automaticallySyncScene = false;
    }

    void Start () {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        // Wait for headset initialization to finish
        yield return new WaitForSeconds(0.1f);

        // Attempt to connect
        Connect();

        // Create the appropriate entity and entity gameobject.
        GameObject entityObject = null;

#if UNITY_STANDALONE
        if (VRDevice.isPresent)
            entityObject = Instantiate(oculusEntityPrefab);
        else
            entityObject = Instantiate(pcEntityPrefab); 
#elif UNITY_IOS || UNITY_ANDROID
        entityObject = Instantiate(mobileEntityPrefab);
#endif

        entity = entityObject.GetComponent<Entity>();
    }

    public void Connect()
    {
        if (PhotonNetwork.connected)
        {
            Log("Joining room...");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Log("Connecting...");
            PhotonNetwork.ConnectUsingSettings(_gameVersion);
        }
    }

    // Called when a connection succeeds.
    public override void OnConnectedToMaster()
    {
        Log("Connected. Joining a room...");
        PhotonNetwork.JoinRandomRoom();
    }

    // Called when a connection fails.
    public override void OnDisconnectedFromPhoton()
    {
        Log("Failed to connect. :(", 1);
    }

    // Called when joining a room fails.
    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Log("Failed to join room. Creating a new one.");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = this.maxPlayers }, null);
        entity.InitializeHost();
    }

    // Called when a room was joined.
    public override void OnJoinedRoom()
    {
        Log("Joined a room.");
        if (PhotonNetwork.room.PlayerCount > 1)
            entity.InitializeClient();
    }

    public void Log(string msg, int importance = 3)
    {
        if (importance <= logLevel)
            print("Launcher: " + msg);
    }
}