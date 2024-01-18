using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentBreakForce;
    private bool isBreaking;

    GameManager gameManager;
    InputManager inputManager;

    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle, maxSpeed;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    // Locomotion
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float gravity = 20.0f;

    float speed = 0f;
    float accelerationMultiplier = 0.2f;
    float defaultDrag = 0.01f;
    float breakDrag = 0.1f;
    float drag = 0.2f;

    public float mouseSensitivity = 0.2f;
    public float lookUpClamp = -5f;
    public float lookDownClamp = 20f;
    float rotateYaw, rotatePitch;

    InputAction accelerate, brakePedal, turn;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        gameManager = GetComponent<GameManager>();
        inputManager = GetComponent<InputManager>();

        // Getting input actions from the Input System
        accelerate = inputManager.inputActions.Drive.Accelerate;
        brakePedal = inputManager.inputActions.Drive.BrakePedal;
        turn = inputManager.inputActions.Drive.Turn;

        // Enable input actions
        if (inputManager != null && inputManager.inputActions != null)
        {
            // Assign input actions
            accelerate = inputManager.inputActions.Drive.Accelerate;
            brakePedal = inputManager.inputActions.Drive.BrakePedal;
            turn = inputManager.inputActions.Drive.Turn;

            // Enable input actions
            if (accelerate != null)
                accelerate.Enable();
            if (brakePedal != null)
                brakePedal.Enable();
            if (turn != null)
                turn.Enable();
        }
        else
        {
            Debug.LogError("GameManager or InputManager is null!");
        }
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        Locomotion();

        // Check if input actions are properly initialized
        if (accelerate != null && brakePedal != null && turn != null)
        {
            Debug.Log("Turn: " + turn.ReadValue<Vector2>().x);
            Debug.Log("Accelerate: " + accelerate.ReadValue<float>());
            Debug.Log("Break Pedal: " + brakePedal.ReadValue<float>());

            Locomotion();
        }
        else
        {
            Debug.LogError("Input actions are not properly initialized.");
        }
    }

    private void GetInput()
    {
        // Driving Input
        accelerate = inputManager.inputActions.Drive.Accelerate;
        brakePedal = inputManager.inputActions.Drive.BrakePedal;
        turn = inputManager.inputActions.Drive.Turn;
    }

    private void HandleMotor()
    {
        //frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        //frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        //currentBreakForce = isBreaking ? breakForce : 0f;
        //ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        //frontRightWheelCollider.brakeTorque = currentBreakForce;
        //frontLeftWheelCollider.brakeTorque = currentBreakForce;
        //rearLeftWheelCollider.brakeTorque = currentBreakForce;
        //rearRightWheelCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        //currentSteerAngle = maxSteerAngle * horizontalInput;
        //frontLeftWheelCollider.steerAngle = currentSteerAngle;
        //frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void Locomotion()
    {
        if (characterController.isGrounded) // When grounded, set y-axis to zero (to ignore it)
        {
            float acceleration = accelerate.ReadValue<float>();
            float breaking = brakePedal.ReadValue<float>();
            float turning = turn.ReadValue<Vector2>().x;

            drag = 1 - defaultDrag - (breakDrag * breaking);

            speed += acceleration * accelerationMultiplier;
            speed *= drag;

            if (speed <= 0.1)
            {
                speed = 0;
            }
            else if (speed >= maxSpeed)
            {
                speed = maxSpeed;
            }

            moveDirection = new Vector3(0f, 0f, speed);
            moveDirection = transform.TransformDirection(moveDirection);

            turning *= speed;
            turning = Mathf.Clamp(turning, -5f, +5f);
            transform.Rotate(0f, turning, 0f);
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void OnDisable()
    {
        // Disable input actions when the script is disabled
        accelerate.Disable();
        brakePedal.Disable();
        turn.Disable();
    }
}
