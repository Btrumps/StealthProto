using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Vector3 offset;
    public Transform thingToFollow;
    public float focalRange = 8.0f;

	void Start () {
        // Gets the distance between the camera and player (SET UP IN SCENE)
        // offset = transform.position - thingToFollow.transform.position;
        // offset = distanceBehindPlayer * Vector3.back + distanceAbovePlayer * Vector3.up;
	}
	
	void Update () {       
        // Sets the camera position equal to the player's transform, plus the offset we set up in the scene
        Vector3 desiredCameraPos = thingToFollow.transform.position + 
            Quaternion.AngleAxis(thingToFollow.transform.eulerAngles.y, Vector3.up) * offset;

        transform.position = desiredCameraPos;

        transform.LookAt(thingToFollow.position + thingToFollow.transform.forward * focalRange);
    }
}
