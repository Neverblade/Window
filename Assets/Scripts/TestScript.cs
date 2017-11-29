using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

    GameObject marker;

	// Use this for initialization
	void Start () {
        marker = GameObject.Find("ImageTarget");
	}
	
	// Update is called once per frame
	void Update () {
        StartCoroutine("LogPosition");
	}

    IEnumerator LogPosition()
    {
        WaitForSeconds wait = new WaitForSeconds(1);
        while (true)
        {
            float dist = Vector3.Distance(transform.position, marker.transform.position);
            Debug.Log("Pos: " + transform.position + "Rot: " + transform.rotation + " Dist: " + dist);
            yield return wait;
        }
    }
}
