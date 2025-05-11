using UnityEngine;

public class CameraController : MonoBehaviour
{
 //   public Transform cameraPivot;          // where the camera pivots around, the gameobject i added to the right of the player 
    public Transform cameraTransform; // the actual camera we are moving
    //TODO
    public Transform cameraAnchor; //gameobject camera is attached to (for stability)
    //

    private float rotationSpeed = 2f;       // how fast the camera rotates horizontally
    private float verticalSpeed = 2f;       // how fast the camera rotates vertically
    private float minVerticalAngle = -30f;  // lowest the camera can look down
    private float maxVerticalAngle = 45f;   // highest the camera can look up

    private float yaw = 0f;                // stores the horizontal rotation angle
    private float pitch = -25f;            // stores the vertical rotation angle (starts looking down a bit)

    //TODO
    public Vector3 cameraOffset = new Vector3(0f, 0.01f, 0.01f); // camera offset to anchor
    //


    private GameObject player;

    void Start()
    {
        yaw = transform.eulerAngles.y;     // initialize the yaw to current horizontal rotation
        player = GameObject.Find("Player"); //getting the player component
        Cursor.visible = false; //make cursor invisible

        //TODO
        Cursor.lockState = CursorLockMode.Locked; //lock the cursor to the center of the screen

        cameraTransform.position = cameraAnchor.position + cameraOffset; // initially set the camera position to the anchor's position plus the offset
        //
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

        if (player.GetComponent<CharacterMovement>().returnSlide() == false) { //if player is not sliding, rotate normally, otherwise rotate the player further down to appear like you are sliding
            player.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);  // rotate the player only on the Y-axis
        } else {
            player.transform.rotation = Quaternion.Euler(pitch-50, yaw, 0f);
        }

        //TODO
        cameraAnchor.rotation = Quaternion.Euler(pitch, yaw, 0f); // rotate the camera anchor to match the player's rotation
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f); // Rotate the camera vertically (pitch)

        cameraTransform.position = cameraAnchor.position + cameraOffset; // set the camera position to the anchor's position plus the offset
        //
    }

}
