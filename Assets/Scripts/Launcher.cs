using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : Photon.PunBehaviour {

    void Awake()
    {
        PhotonNetwork.autoJoinLobby = false;
        PhotonNetwork.automaticallySyncScene = false;
    }

    void Start () {

        // Create the appropriate entity and entity gameobject.
        GameObject entityObject = new GameObject();
        DontDestroyOnLoad(entityObject);
        Entity entity = null;

#if UNITY_STANDALONE
        entity = entityObject.AddComponent<OculusEntity>();
#elif UNITY_IOS || UNITY_ANDROID
        entity = entityObject.AddComponent<MobileEntity>();
#endif

        // Initialize lobby logic for the corresponding device.
        entity.InitializeLobby();
    }
}
