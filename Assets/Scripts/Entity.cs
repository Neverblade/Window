using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Entity : MonoBehaviour {

    [Header("Base Entity Properties")]

    // Position/Rotation of the marker relative to you
    public Vector3 markerPosition;
    public Quaternion markerRotation;

    public GameObject marker;
    public string markerPrefabName = "_Marker";

    public virtual void InitializeHost()
    {

    }

    public virtual void OnHostInitializationFinished()
    {

    }

    public virtual void InitializeClient()
    {

    }

    public virtual void OnClientInitializationFinished()
    {

    }

    // Should happen BEFORE any entity specific behaviour.
    public void PositionEntity()
    {
        // Check if the marker object exists.
        // If it doesn't, spawn and position it.
        // If it does, position yourself relative to it.
        marker = GameObject.Find(markerPrefabName + "(Clone)");
        if (marker == null)
        {
            print("Entity: Marker doesn't exist, creating it.");
            marker = PhotonNetwork.Instantiate(markerPrefabName, markerPosition, markerRotation, 0);
        } else
        {
            print("Entity: Marker exists, positioning myself.");
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