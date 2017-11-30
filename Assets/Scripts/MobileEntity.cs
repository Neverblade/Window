using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileEntity : Entity {

    // Client Initialization Prefabs
    public GameObject preCameraPrefab;
    public GameObject preCanvasPrefab;
    public GameObject preEventSystemPrefab;

    // Vuforia prefabs
    public GameObject arCameraPrefab;
    public GameObject imageTargetPrefab;

    public string mobileAvatarPrefabName;

    // Client Initialization Variables
    GameObject preCamera;
    GameObject preCanvas;
    GameObject preEventSystem;

    GameObject arCamera;
    GameObject imageTarget;
    GameObject avatar;

    public override void InitializeHost()
    {
        Debug.Log("MOBILE HOST NOT YET IMPLEMENTED");
    }

    public override void OnHostInitializationFinished()
    {
        Debug.Log("MOBILE HOST NOT YET IMPLEMENTED");
    }

    public override void InitializeClient()
    {
        Debug.Log("INITIALIZING MOBILE CLIENT");

        // Spawn pre cam and canvas
        preCamera = Instantiate(preCameraPrefab);
        preCanvas = Instantiate(preCanvasPrefab);

        // Set up callback
        preCanvas.transform.Find("ConnectButton").GetComponent<Button>().onClick.AddListener(OnClientInitializationFinished);
        preEventSystem = Instantiate(preEventSystemPrefab);
    }

    public override void OnClientInitializationFinished()
    {
        Debug.Log("FINISHING MOBILE INITIALIZATION");

        // Remove pre objects.
        Destroy(preCamera);
        Destroy(preCanvas);
        Destroy(preEventSystem);

        // Spawn ARCamera and image target, assume marker exists.
        arCamera = Instantiate(arCameraPrefab, Vector3.zero, Quaternion.identity);
        GameObject markerObj = GameObject.Find(markerPrefabName + "(Clone)");
        imageTarget = Instantiate(imageTargetPrefab, markerObj.transform.position, markerObj.transform.rotation);
        imageTarget.transform.Rotate(new Vector3(-90, 180, 0)); // Orient the image plane correctly

        // Spawn avatar over network. Set it to follow the arcamera's movements. Avatar contains camera.
        avatar = PhotonNetwork.Instantiate(mobileAvatarPrefabName, arCamera.transform.position, arCamera.transform.rotation, 0);
        avatar.GetComponent<MobileOwner>().transformToFollow = arCamera.transform;
    }
}