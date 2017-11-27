using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCOwner : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!GetComponent<PhotonView>().isMine)
        {
            GetComponent<Camera>().enabled = false;
            GetComponent<AudioListener>().enabled = false;
        }
        else
        {
            // Disable the avatar
            transform.Find("Avatar").gameObject.SetActive(false);
        }
	}
}