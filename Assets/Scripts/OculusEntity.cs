using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusEntity : Entity {

    public static Vector3 guideOffset = new Vector3(0, 0, 0);

    public Vector3[] corners = new Vector3[4];
    public Vector3 markerCenter;
    public Vector3 markerNormal;

    public GameObject guidePrefab;

    [HideInInspector]
    public GameObject cameraRig, localAvatar;

    GameObject head;
    GameObject leftController;
    GameObject rightController;
    float previousLeftTriggerValue;
    float leftTriggerValue;
    float previousRightTriggerValue;
    float rightTriggerValue;

    bool trackingInput = false;
    int cornersFilled = 0;
    GameObject leftGuide;
    GameObject rightGuide;

    private void Start()
    {
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
                Instantiate(guidePrefab, corners[cornersFilled], Quaternion.identity);
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
                Instantiate(guidePrefab, corners[cornersFilled], Quaternion.identity);
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
        Destroy(leftGuide);
        Destroy(rightGuide);

        // Average to get the center of the 4 points
        markerCenter = Vector3.zero;
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
        markerNormal = Vector3.zero;
        foreach (Vector3 normal in normals)
            markerNormal += normal;
        markerNormal /= 4;

        // Ready to join room.
        OnHostInitializationFinished();
    }

    // Asks for player to mark out the boundaries of the marker.
    public override void InitializeHost()
    {
        StartTracking();
    }

    // Called when player has finished host setup.
    public override void OnHostInitializationFinished()
    {
        PhotonNetwork.LoadLevel("TestMap");
    }
}
