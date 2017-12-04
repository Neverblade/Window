using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRPC : Photon.MonoBehaviour {

    // Public
    public Material[] guideMaterials;
    public Material[] cubeMaterials;

    private void Start()
    {

    }

    [PunRPC]
    public void ChangeMaterial(int matIndex)
    {
        GetComponent<Renderer>().material = guideMaterials[matIndex];
    }
}
