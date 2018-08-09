using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {

    private float initialOffsetAngle;
    private float increasingNumber = 0.0f;
    bool isDecreasing = false;
    public float increaseAmount = 35.0f;
    public float maxRotationAllowed = 90.0f;

    private void Start() {
        initialOffsetAngle = transform.rotation.eulerAngles.y;
    }

    void Update () {

        // I know this doesn't work as we are handling Degrees in code, but using Quaternion functions
        if (increasingNumber >= maxRotationAllowed && isDecreasing == false) {
            isDecreasing = true;
            increaseAmount = -increaseAmount;
        } else if (increasingNumber <= 0 && isDecreasing) {
            isDecreasing = false;
            increaseAmount = -increaseAmount;
        }

        increasingNumber += increaseAmount * Time.deltaTime;


        // Cant figure out how to turn this into Space.World, as this won't accept another argument
        transform.rotation = Quaternion.AngleAxis(increasingNumber + initialOffsetAngle, Vector3.up) *
            Quaternion.AngleAxis(45.0f, Vector3.right);

        // Debug.Log(Quaternion.Angle(Quaternion.LookRotation(PlayerController.instance.transform.position - transform.position), transform.rotation));
    }
}
