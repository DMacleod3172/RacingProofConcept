using UnityEngine;
using UnityEngine.InputSystem;

public class DriveLocomotion : MonoBehaviour
{
    //Input Actions
    InputAction accelerate, brakePedal, turn, boost;

    CharacterController characterController;
    public Transform cameraContainer;

    //Speed and movement
    public float maxSpeed = 1000f;
    float speed = 0f;
    float accelerationMultiplier = 0.4f;
    float defaultDrag = 0.01f;
    float breakDrag = 0.1f;
    float drag = 0.2f;
    float boostDuration = 3f;
    float boostMultiplier = 2f;
    bool isBoosting = false;
    float boostTimer = 0f;
    public float gravity = 20.0f;

    //Mouse sensitivuty/ look params
    public float mouseSensitivity = 0.2f;
    public float lookUpClamp = -5f;
    public float lookDownClamp = 20f;

    //Vectors
    Vector3 moveDirection = Vector3.zero;
    float rotateYaw, rotatePitch;

    //More references
    GameManager gameManager;
    InputManager inputManager;

    void Start()
    {
        Cursor.visible = false;

        //Get components
        characterController = GetComponent<CharacterController>();
        gameManager = GetComponent<GameManager>();
        inputManager = GetComponent<InputManager>();

        // Attempt to get input actions from GameManager
        if (inputManager != null && inputManager.inputActions != null)
        {
            // Assign input actions
            accelerate = inputManager.inputActions.Drive.Accelerate;
            brakePedal = inputManager.inputActions.Drive.BrakePedal;
            turn = inputManager.inputActions.Drive.Turn;
            boost = inputManager.inputActions.Drive.Boost;

            // Enable input actions
            if (accelerate != null)
                accelerate.Enable();
            if (brakePedal != null)
                brakePedal.Enable();
            if (turn != null)
                turn.Enable();
            if (boost != null)
                boost.Enable();
        }
        else
        {
            Debug.LogError("GameManager or InputManager is null!");
        }
    }

    void FixedUpdate()
    {
        // Check if input actions are properly initialized
        if (accelerate != null && brakePedal != null && turn != null && boost != null)
        {
            //For testing
            Debug.Log("Turn: " + turn.ReadValue<Vector2>().x);
            Debug.Log("Accelerate: " + accelerate.ReadValue<float>());
            Debug.Log("Break Pedal: " + brakePedal.ReadValue<float>());

            //Update Locomotion
            Locomotion();

            //Check boosting
            if (boost.ReadValue<float>() > 0 && !isBoosting)
            {
                StartBoost();
            }
            //Update boosting duration and end if timer reached
            if (isBoosting)
            {
                boostTimer -= Time.deltaTime;

                if (boostTimer <= 0f)
                {
                    EndBoost();
                }
        }//For testing
        //else
        //{
        //    Debug.LogError("Input actions are not properly initialized.");
        //}
    }

    void StartBoost()
    {
        isBoosting = true;
        boostTimer = boostDuration;
        speed *= boostMultiplier; // Apply boost multiplier
    }

    void EndBoost()
    {
        isBoosting = false;
        speed /= boostMultiplier; // Revert the speed to normal after boost ends
    }

    void OnDisable()
    {
        // Disable input actions when the script is disabled
        if (accelerate != null)
            accelerate.Disable();
        if (brakePedal != null)
            brakePedal.Disable();
        if (turn != null)
            turn.Disable();
        if (boost != null)
            boost.Disable();
    }


    void Locomotion()
    {
        if (characterController.isGrounded) //Check grounded
        {
            //read inputs
            float acceleration = accelerate.ReadValue<float>();
            float breaking = brakePedal.ReadValue<float>();
            float turning = turn.ReadValue<Vector2>().x;
            
            //Drag vased on braking input
            drag = 1 - defaultDrag - (breakDrag * breaking);
            
            //Update speed, apply drag
            speed += acceleration * accelerationMultiplier;
            speed *= drag;
            
            //Clamp speed to limits
            if (speed <= 0.1)
            {
                speed = 0;
            }
            else if (speed >= maxSpeed)
            {
                speed = maxSpeed;
            }
            
            //Update movement direction
            moveDirection = new Vector3(0f, 0f, speed);
            moveDirection = transform.TransformDirection(moveDirection);
            
            //Charater rotates based on turn input
            turning *= speed;
            turning = Mathf.Clamp(turning, -5f, +5f);
            transform.Rotate(0f, turning, 0f);
        }
        //gravity application & movement
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    //void RotateAndLook()
    //{
        //Vector2 lookInput = look.ReadValue<Vector2>();

        //rotateYaw = lookInput.x * mouseSensitivity;
        //rotateYaw += cameraContainer.transform.localRotation.eulerAngles.y;

        //rotatePitch -= lookInput.y * mouseSensitivity;
        //rotatePitch = Mathf.Clamp(rotatePitch, lookUpClamp, lookDownClamp);

        //cameraContainer.transform.localRotation = Quaternion.Euler(rotatePitch, rotateYaw, 0f);
    //}
    }
}