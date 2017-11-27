using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRCameraRigOwner : MonoBehaviour {

    public Transform ovrAvatar;
    private Transform eye;

	// Use this for initialization
	void Start () {
		if (!GetComponent<PhotonView>().isMine)
        {
            //GetComponent<OVRCameraRig>().enabled = false;
            //GetComponent<OVRManager>().enabled = false;

            //Transform eye = transform.Find("TrackingSpace").Find("CenterEyeAnchor");
            //eye.GetComponent<Camera>().enabled = false;
            //eye.GetComponent<AudioListener>().enabled = false;

            eye = transform.Find("TrackingSpace").Find("CenterEyeAnchor");
        }
        else
        {
            // Disable the avatar
            GameObject.Find("OVRAvatarHead").SetActive(false);
        }
	}

    private void Update()
    {
        if (ovrAvatar != null)
        {

        }
    }
}