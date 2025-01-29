using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    //float vlaues (mostly for physics and movement)
    private float gravity = -20f; // gravity value
    private float movementSpeed = 10f; // speed at which the character moves around
    private float jumpHeight = 2.0f; //how high the character can jump
    private float slidePower = 1.5f; //how fast the player can slide
    private Vector3 velocity; // handles gravity and falling speed

    //boolean values to check for certain player actions
    private CharacterController characterController; 
    private bool isWalking = false; //checking if player is walking (?)
    private bool isWallRiding = false; //checking if player is wall riding
    private bool isSliding = false; //checking if the player is sliding
    private bool isGrounded; // to check if we're on solid ground or falling
    private bool canDoubleJump; //check if player is allowed to double jump

    public Transform cameraTransform; 

    void Start()
    {
        characterController = GetComponentInParent<CharacterController>(); // Grab the CharacterController from the parent object
    }

    void FixedUpdate()
    {
        MoveCharacter(); // move character evry frame
    }

    void MoveCharacter()
    {
        // Get input from the player (WASD)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        if (inputVector.magnitude > 1)
        {
            inputVector.Normalize(); // if the player is pressing too many keys
        }

        isWalking = inputVector.magnitude > 0; // If we're pressing something the character is walking

        // Get the direction the camera is facing
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;  // Ignore any weird up/down angles
        cameraForward.Normalize(); // Keep it clean and normalized

        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0; 
        cameraRight.Normalize(); 

        // Combine camera's forward and right directions with player input
        Vector3 moveDirection = cameraForward * inputVector.y + cameraRight * inputVector.x;

        // Check if the character is on the ground
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep the character grounded

            if (Input.GetKey(KeyCode.Space)) //if space is pressed
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); //sets the y (vertical) velocity to the jumpheight which makes the player jump
            }

            if (Input.GetKey(KeyCode.LeftShift)) //if shift is pressed and player can slide
            {
                isSliding = true;
                characterController.Move(moveDirection * movementSpeed * slidePower * Time.deltaTime); //move the player at a slighlty higher/lower speed depending on slide power

                if (slidePower >= -0.5) {
                    slidePower -= 0.1f; //gradually decrease the sliding speed of the player until a certain point
                }

            } else
            {
                if (slidePower <= 1.5f) {
                    slidePower = 1.5f; //reset sliding power once the player stops sliding
                }

                //if statement check so the isSliding is not constantly being set to false every frame
                if (isSliding)
                {
                    isSliding = false;
                }
            }
        }
        // Move the character based on the movement direction and speed
        characterController.Move(moveDirection * movementSpeed * Time.deltaTime);

        // If we're moving, rotate the character to face where we're going
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward); // Face the way the camera is pointing
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f); // Smooth rotate
        }

        //Giving the character the ability to wall ride by ignoring gravity and any changes to the Y axis
        if(!isWallRiding || (isWallRiding && moveDirection == Vector3.zero)) //if character is NOT wall riding OR if character IS wall riding but not moving
        {
            // Apply gravity/changes in Y axis to the player
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }
    }

    //Detecting collision between player and rideable walls
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("RideableWall"))
        {
            isWallRiding = true;
        } 
    }

    //Detecting when player jumps off the wall
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("RideableWall"))
        {
            isWallRiding = false;
        }
    }

    //return wether the player is sliding or not
    public bool returnSlide()
    {
        return isSliding;
    }

    //return wether the player is wall riding or not
    //TODO need to make player rotate slightly to the side when wall riding
    public bool returnWallRiding()
    {
        return isWallRiding;
    }
}
