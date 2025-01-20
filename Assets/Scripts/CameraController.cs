using UnityEngine;

public class CameraController : MonoBehaviour
{
 //   public Transform cameraPivot;          // where the camera pivots around, the gameobject i added to the right of the player 
    public Transform cameraTransform;      // the actual camera we are moving

    private float rotationSpeed = 2f;       // how fast the camera rotates horizontally
    private float verticalSpeed = 2f;       // how fast the camera rotates vertically
    private float minVerticalAngle = -30f;  // lowest the camera can look down
    private float maxVerticalAngle = 45f;   // highest the camera can look up

    private float yaw = 0f;                // stores the horizontal rotation angle
    private float pitch = -25f;            // stores the vertical rotation angle (starts looking down a bit)

    void Start()
    {
        yaw = transform.eulerAngles.y;     // initialize the yaw to current horizontal rotation
    }

    void Update()
    {
        RotateGameObject();                // handle the player's rotation based on mouse input
    }

    // rotate the player left and right, and look up and down based on mouse movement
    void RotateGameObject()
    {
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;     // horizontal rotation (yaw) with mouse movement
        pitch -= Input.GetAxis("Mouse Y") * verticalSpeed;   // vertical rotation (pitch) with mouse movement
        pitch = Mathf.Clamp(pitch, minVerticalAngle, maxVerticalAngle); // keep the pitch within limits so you can't look too far up/down
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);  // rotate the player only on the Y-axis
    }

}
