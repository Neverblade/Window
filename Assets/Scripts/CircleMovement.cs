using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** The object this script is attached to will move in a circle.
 *  The circle's center is defined by its parent's position (or origin if no parent).
 *  The circle's radius is defined by the distance between object and parent.
 */
public class CircleMovement : MonoBehaviour {

    [Tooltip("Speed of movement in revolutions per second.")]
    public float speed = 1;

    private Vector3 center = Vector3.zero;

	// Use this for initialization
	void Start () {
        if (transform.parent != null)
            center = transform.parent.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
