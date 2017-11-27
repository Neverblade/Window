using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRAvatarOwner : MonoBehaviour {

    public Transform eye;

	// Use this for initialization
	void Start () {
        // Disable myself if I'm the owner.
        if (GetComponent<PhotonView>().isMine)
        {
            transform.Find("Sphere").gameObject.SetActive(false);
            transform.Find("Cube").gameObject.SetActive(false);
        } 
	}
	
	// Update is called once per frame
	void Update () {
		if (eye != null)
        {
            transform.position = eye.position;
            transform.rotation = eye.rotation;
        }
	}
}
