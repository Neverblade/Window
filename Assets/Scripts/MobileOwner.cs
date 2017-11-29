using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileOwner : MonoBehaviour {

    public Transform transformToFollow;

    public Vector3 posOffset = Vector3.zero;
    public Vector3 rotOffset = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        // Disable all of my children if I'm the owner.
        if (GetComponent<PhotonView>().isMine)
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);
        } else
        {
            GetComponent<Camera>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transformToFollow != null)
        {
            transform.position = transformToFollow.position;
            transform.rotation = transformToFollow.rotation;

            transform.position = transformToFollow.TransformPoint(posOffset);
            transform.rotation = transformToFollow.rotation * Quaternion.Euler(rotOffset);
        }
    }
}
