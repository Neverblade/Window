using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideRPC : Photon.MonoBehaviour {

    // Public
    public Material[] guideMaterials;
    public Material[] cubeMaterials;

    [PunRPC]
    public void ChangeMaterial(int matIndex)
    {
        GetComponent<Renderer>().material = guideMaterials[matIndex];
    }
}
