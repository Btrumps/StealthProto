using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static PlayerController instance;

    public float speed = 5.0f;
    public float smoothMoveTime = 0.1f;
    public float rotateSpeed = 5.0f;

    Rigidbody rb;
    Vector3 velocity;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        // rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0f, rotateSpeed * Input.GetAxis("Mouse X"), 0f));

        Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        // Multiplying this by the Camera's rotation seems to get our controls lined up
        inputDir = Camera.main.transform.rotation * inputDir;
        rb.MovePosition(rb.position + inputDir * speed * Time.deltaTime);
    }

    private void Update() {
        // rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0f, rotateSpeed * Input.GetAxis("Mouse X"), 0f));
        RotatePlayer(); // Seems smoother
    }

    void RotatePlayer() {
        float mouseHorizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        transform.Rotate(0, mouseHorizontal, 0);
    }
}
