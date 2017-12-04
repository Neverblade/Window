using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCreator : MonoBehaviour {

    // Public
    public int materialsLength = 4;

    // Private
    int materialIndex;
    Transform left; 
    Transform right;
    bool creatingCube;
    GameObject guideCube;

	// Use this for initialization
	void Start () {
        left = transform.Find("TrackingSpace").Find("LeftHandAnchor");
        right = transform.Find("TrackingSpace").Find("RightHandAnchor");
        creatingCube = false;
	}
	
	// Update is called once per frame
	void Update () {
        bool bothGripsHeld = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > .9f
                             && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > .9f;
        if (creatingCube)
        {
            if (bothGripsHeld)
            {
                // Update transform of guide cube
                UpdateGuide();
            } else
            {
                // Spawn cube if valid, remove guideCube
                print("Releasing guide cube.");
                creatingCube = false;
                CreateCube();
            }
        } else
        {
            if (bothGripsHeld)
            {
                // Start up guide cube
                print("Creating guide cube.");
                creatingCube = true;

                // Pick a random color
                materialIndex = Random.Range(0, materialsLength);

                // Create the guide, change the material
                guideCube = PhotonNetwork.Instantiate("GuideCube", Vector3.zero, Quaternion.identity, 0);
                PhotonView photonView = guideCube.GetComponent<PhotonView>();
                photonView.RPC("ChangeMaterial", PhotonTargets.All, materialIndex);

                // Update the position of the guide (should be transmitted by the photon view)
                UpdateGuide();
            }
        }
	}

    // Updates the guide position off of your hands. Assumes guideCube exists.
    void UpdateGuide()
    {
        guideCube.transform.position = (left.position + right.position) / 2;
        guideCube.transform.LookAt(right);
        guideCube.transform.localScale = Vector3.one * Vector3.Distance(left.position, right.position);
    }

    // Replaces the guide cube with a permanent one.
    void CreateCube()
    {
        // Create the cube
        GameObject cube = PhotonNetwork.Instantiate("Cube", guideCube.transform.position, guideCube.transform.rotation, 0);
        cube.transform.localScale = guideCube.transform.localScale;
        PhotonView photonView = cube.GetComponent<PhotonView>();
        photonView.RPC("ChangeMaterial", PhotonTargets.All, materialIndex);
        StartCoroutine(ActivatePhysics(cube));


        /*GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = guideCube.transform.position;
        cube.transform.rotation = guideCube.transform.rotation;
        cube.transform.localScale = guideCube.transform.localScale;
        cube.GetComponent<Renderer>().material = cubeMaterial;
        StartCoroutine(ActivatePhysics(cube));*/

        // Destroy the guide cube
        PhotonNetwork.Destroy(guideCube);
    }

    // Delay activating physics
    IEnumerator ActivatePhysics(GameObject cube)
    {
        yield return new WaitForSeconds(1);
        Rigidbody rb = cube.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
}