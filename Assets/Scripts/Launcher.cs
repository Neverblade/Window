using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : Photon.PunBehaviour {

    public int logLevel = 3;

    public GameObject oculusEntityPrefab;
    public GameObject oculusPrefab;
    public GameObject touchPrefab;

    public byte maxPlayers = 4;

    string _gameVersion = "1";

    Entity entity;

    void Awake()
    {
        PhotonNetwork.autoJoinLobby = false;
        PhotonNetwork.automaticallySyncScene = false;
    }

    void Start () {

        // Attempt to connect
        Connect();

        // Create the appropriate entity and entity gameobject.
        GameObject entityObject = null;

#if UNITY_STANDALONE
        entityObject = Instantiate(oculusEntityPrefab);
        OculusEntity oculusEntity = entityObject.GetComponent<OculusEntity>();
        oculusEntity.cameraRig = Instantiate(oculusPrefab);
        oculusEntity.localAvatar = Instantiate(touchPrefab);
#elif UNITY_IOS || UNITY_ANDROID

#endif

        DontDestroyOnLoad(entityObject);
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
        Log("Connected.");
        Log("Joining a room...");
        PhotonNetwork.JoinRandomRoom();
    }

    // Called when a connection fails.
    public override void OnDisconnectedFromPhoton()
    {
        Log("Failed to connect.", 1);
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
    }

    public void Log(string msg, int importance = 3)
    {
        if (importance <= logLevel)
        {
            print("Launcher: " + msg);
        }
    }
}