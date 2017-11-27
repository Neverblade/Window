using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalAvatarOwner : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!GetComponent<PhotonView>().isMine)
        {
            //GetComponent<OvrAvatar>().enabled = false;
        }
        else
        {

        }
	}
}