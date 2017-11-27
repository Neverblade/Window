using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;   

public class OculusEntity : Entity {

    public static Vector3 guideOffset = new Vector3(0, 0, 0);

    public Vector3[] corners = new Vector3[4];

    // Initialization Prefabs
    public GameObject guidePrefab;
    public GameObject preCameraRigPrefab;
    public GameObject preLocalAvatarPrefab;

    // Post-Initialization Prefab Names
    public GameObject cameraRigPrefab;
    public GameObject localAvatarPrefab;
    public string ovrAvatarPrefabName;

    // CameraRig and LocalAvatar
    private GameObject cameraRig;
    private GameObject localAvatar;

    // Initialization Variables
    GameObject head;
    GameObject leftController;
    GameObject rightController;
    GameObject leftGuide;
    GameObject rightGuide;
    float previousLeftTriggerValue;
    float leftTriggerValue;
    float previousRightTriggerValue;
    float rightTriggerValue;
    bool trackingInput = false;
    int cornersFilled = 0;
    List<GameObject> cornerObjects = new List<GameObject>();

    public void Start()
    {
        // Create CameraRig and Avatar
        cameraRig = Instantiate(preCameraRigPrefab);
        cameraRig.name = preCameraRigPrefab.name;
        localAvatar = Instantiate(preLocalAvatarPrefab);
        localAvatar.name = preLocalAvatarPrefab.name;

        // Assign variables
        head = cameraRig.transform.Find("TrackingSpace").Find("CenterEyeAnchor").gameObject;
        leftController = cameraRig.transform.Find("TrackingSpace").Find("LeftHandAnchor").gameObject;
        rightController = cameraRig.transform.Find("TrackingSpace").Find("RightHandAnchor").gameObject;

    }

    private void Update()
    {
        if (trackingInput)
        {
            previousLeftTriggerValue = leftTriggerValue;
            previousRightTriggerValue = rightTriggerValue;
            leftTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
            rightTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

            bool leftTriggered = previousLeftTriggerValue < .9 && leftTriggerValue >= .9;
            bool rightTriggered = previousRightTriggerValue < .9 && rightTriggerValue >= .9;

            if (leftTriggered)
            {
                corners[cornersFilled] = leftGuide.transform.position;
                GameObject g = Instantiate(guidePrefab, corners[cornersFilled], Quaternion.identity);
                cornerObjects.Add(g);
                cornersFilled++;
                if (cornersFilled >= 4)
                {
                    StopTracking();
                    return;
                }
            }

            if (rightTriggered)
            {
                corners[cornersFilled] = rightGuide.transform.position;
                GameObject g = Instantiate(guidePrefab, corners[cornersFilled], Quaternion.identity);
                cornerObjects.Add(g);
                cornersFilled++;
                if (cornersFilled >= 4)
                {
                    StopTracking();
                    return;
                }
            }
        }
    }

    // Ask for input on labeling the corners of the marker.
    private void StartTracking()
    {
        // Clear corners
        corners = new Vector3[4];
        cornersFilled = 0;

        // Add position guides to the controllers.
        leftGuide = Instantiate(guidePrefab, leftController.transform);
        rightGuide = Instantiate(guidePrefab, rightController.transform);

        // Turn on tracking
        trackingInput = true;
    }

    // Assume input has been received. Cease tracking.
    private void StopTracking()
    {
        trackingInput = false;

        // Remove the guides
        Destroy(leftGuide); leftGuide = null;
        Destroy(rightGuide); rightGuide = null;

        // Average to get the center of the 4 points
        Vector3 markerCenter = Vector3.zero;
        foreach (Vector3 pos in corners)
            markerCenter += pos;
        markerCenter /= 4;

        // Compute normals (multiply by -1 if necessary)
        Vector3 centerToFace = Vector3.Normalize(head.transform.position - markerCenter);
        Vector3[] normals = new Vector3[4];
        for (int i = 0; i < 4; i++)
        {
            Vector3 vec1 = corners[(i+1)%4] - corners[i];
            Vector3 vec2 = corners[(i+2)%4] - corners[i];
            Vector3 normal = Vector3.Normalize(Vector3.Cross(vec1, vec2));
            if (Vector3.Dot(-normal, centerToFace) > Vector3.Dot(normal, centerToFace))
                normal *= -1;
            normals[i] = normal;
        }

        // Average normals
        Vector3 markerNormal = Vector3.zero;
        foreach (Vector3 normal in normals)
            markerNormal += normal;
        markerNormal /= 4;

        // Fill in markerTransform data
        markerPosition = markerCenter;
        markerRotation = Quaternion.LookRotation(markerNormal, Vector3.up);

        // Remove the guide objects.
        foreach (GameObject g in cornerObjects)
            Destroy(g);
        cornerObjects.Clear();

        // Ready to join room.
        OnHostInitializationFinished();
    }

    // Asks for player to mark out the boundaries of the marker.
    public override void InitializeHost()
    {
        StartTracking();
    }

    // Same behavior as host.
    public override void InitializeClient()
    {
        StartTracking();
    }

    public override void OnHostInitializationFinished()
    {
        // Clean up setup objects.
        Destroy(cameraRig);
        Destroy(localAvatar);

        // Position entity
        PositionEntity();

        // Set up new objects.
        cameraRig = Instantiate(cameraRigPrefab, transform.position, transform.rotation);
        localAvatar = Instantiate(localAvatarPrefab, transform.position, transform.rotation);
        GameObject avatar = PhotonNetwork.Instantiate(ovrAvatarPrefabName, transform.position, transform.rotation, 0);
        avatar.GetComponent<OVRAvatarOwner>().eye = cameraRig.transform.Find("TrackingSpace").Find("CenterEyeAnchor");
    }

    public override void OnClientInitializationFinished()
    {
        OnHostInitializationFinished();
    }
}
