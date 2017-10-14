using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusEntity : Entity {

    public static Vector3 guideOffset = new Vector3(0, 0, 0);

    public Vector3[] corners = new Vector3[4];
    public GameObject guidePrefab;

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
        leftController = transform.Find("TrackingSpace").Find("LeftHandAnchor").gameObject;
        rightController = transform.Find("TrackingSpace").Find("RightHandAnchor").gameObject;

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
                cornersFilled++;
                if (cornersFilled >= 4)
                {
                    StopTracking();
                    return;
                }
            }
        }
    }

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

    private void StopTracking()
    {
        trackingInput = false;

        Destroy(leftGuide);
        Destroy(rightGuide);
    }

    // Asks for player to mark out the boundaries of the marker.
    public override void InitializeHost()
    {
        StartTracking();
    }
}
