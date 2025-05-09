using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private float gravity = -20f; // Gravity value
    private float movementSpeed = 10f; // Speed at which the character moves around
    private float jumpHeight = 2.0f; //How high the character can jump
    private float slidePower = 1.5f; //how fast the player can slide

    private CharacterController characterController; 
    public bool isRunning = false; //checking if player is walking (?)
    public bool isWallRiding = false; //checking if player is wall riding
    public bool isSliding = false; //checking if the player is sliding
    public bool isAttacking = false; //checking if the player is attacking
    public bool isIdle = false; //checking if the player is idle
    public bool damageTaken = false; //checking if player took damage
    public bool isJumping = false; //checking if player is jumping

    private Vector3 velocity; // Handles gravity and falling speed
    private bool isGrounded; // To check if we're on solid ground or falling

    public Transform cameraTransform;

    Animator animator; // animator component

    void Start()
    {
        characterController = GetComponentInParent<CharacterController>(); // Grab the CharacterController from the parent object
        animator = GetComponent<Animator>(); // Assign animator component
    }

    void Update()
    {
        MoveCharacter(); // move character evry frame
        setAnimator(); //update animation bools every frame
        attack(); //check if player is attacking every frame
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

        isRunning = inputVector.magnitude > 0; // If we're pressing something the character is walking


        // Get the direction the camera is facing
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;  // Ignore any weird up/down angles
        cameraForward.Normalize(); // Keep it clean and normalized

        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0; 
        cameraRight.Normalize(); 

        // Combine camera's forward and right directions with player input
        Vector3 moveDirection = cameraForward * inputVector.y + cameraRight * inputVector.x;

        float forwardAmount = Vector3.Dot(cameraTransform.forward, moveDirection.normalized); //get float for forward amount - for blend tree animations
        animator.SetFloat("forwardAmount", forwardAmount); //set the forward amount to the animator

        bool wasGrounded = isGrounded; // Store the previous grounded state for comparison
        // Check if the character is on the ground
        isGrounded = checkGrounded();
        if (isGrounded && !wasGrounded)
        {
            isJumping = false; //if play just landed set isJumping to false
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep the character grounded

            if (Input.GetKeyDown(KeyCode.Space)) //if space is pressed
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); //sets the y (vertical) velocity to the jumpheight which makes the player jump
                isJumping = true;
                isGrounded = false; //set isGrounded to false so the player can't jump again

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

                isSliding = false;
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

    private void attack()
    {
        if (Input.GetKey(KeyCode.Mouse0)) //if left mouse button is pressed
        {
            isAttacking = true; //set the isAttacking parameter to true
            Invoke(nameof(endAttackAnimation), 1.5f); // Auto-reset after animation length

        }
    }

    public void endAttackAnimation()
    {
        isAttacking = false;
    }

    private bool checkGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit);
    }

    private void setAnimator()
    {
        animator.SetBool("isRunning", isRunning); // set the isRunning parameter
        animator.SetBool("isAttacking", isAttacking); // set the isAttacking parameter
        animator.SetBool("isJumping", isJumping); // set the jumping parameter

        // animator.SetBool("isSliding", isSliding); // set the isSliding parameter (need slide animation)
    }
}
