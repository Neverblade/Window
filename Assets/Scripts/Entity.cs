using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Entity : MonoBehaviour {

    [Header("Base Entity Properties")]
    public GameObject markerPrefab;

    // Position/Rotation of the marker relative to you
    public Vector3 markerPosition;
    public Quaternion markerRotation;

    public bool isHost = false;

    public GameObject marker;
    public string mapName = "TestMap";
    public string markerName = "_Marker";

    public virtual void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public virtual void InitializeHost()
    {

    }

    // Should happen AFTER any entity specific behaviour.
    public virtual void OnHostInitializationFinished()
    {
        PhotonNetwork.LoadLevel(mapName);
    }

    public virtual void InitializeSpectator()
    {

    }

    public virtual void OnInitializeSpectatorFinished()
    {
        PhotonNetwork.LoadLevel(mapName);
    }

    public virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Only applicable when joining the room's scene.
        if (!scene.name.Equals(mapName))
            return;

        // Check if the marker object exists.
        // If it doesn't, spawn and position it.
        // If it does, position yourself relative to it.
        marker = GameObject.Find(markerName);
        if (isHost)
        {
            Destroy(marker);
            marker = PhotonNetwork.Instantiate(markerName, markerPosition, markerRotation, 0);
        } else
        {
            GameObject tempObject = new GameObject("TempObject");
            tempObject.transform.SetPositionAndRotation(markerPosition, markerRotation);
            Vector3 localPos = tempObject.transform.InverseTransformPoint(transform.position);
            Vector3 localEuler = tempObject.transform.InverseTransformDirection(transform.eulerAngles);
            transform.position = marker.transform.TransformPoint(localPos);
            transform.eulerAngles = marker.transform.TransformDirection(localEuler);
            Destroy(tempObject);
        }
    }
}