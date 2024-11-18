using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TwoWheelVehicleController : MonoBehaviour
{
    #region Properties
    [Header("Base Variables")]
    [SerializeField]
    private float m_maxSpeed;
    public float MaxSpeed {
        get { return m_maxSpeed; }
        set { m_maxSpeed = value; }
    }

    [SerializeField]
    private float m_acceleration;
    public float AccelerationValue {
        get { return m_acceleration; }
        set { m_acceleration = value; }
    }

    [SerializeField]
    private float m_steerStrength;
    public float SteerStrength
    {
        get { return m_steerStrength; }
        set { m_steerStrength = value; }
    }

    [SerializeField]
    [Range(1, 10)]
    private float m_brakingFactor;
    public float BrakingFactor
    {
        get { return m_brakingFactor; }
        set { m_brakingFactor = value; }
    }

    [SerializeField]
    private float m_rayLength;
    public float RayLength
    {
        get { return m_rayLength; }
        set { m_rayLength = value; }
    }

    [SerializeField]
    private float m_gravity;
    public float GravityValue
    {
        get { return m_gravity; }
        set { m_gravity = value; }
    }
    #endregion

    [Header("Dependencies")]
    [SerializeField] private Rigidbody centerRB;
    [SerializeField] private Rigidbody vehicleBody;
    [SerializeField] private LayerMask drivableSurfice;

    [Header("Handle Logic")]
    [SerializeField] private GameObject vehicleHandle;
    [SerializeField] private float handleRotVal = 30f;
    [SerializeField] private float handleRotSpeed = 0.15f;

    [Header("Other Variables")]
    [SerializeField] private float zTiltAngle = 45f;

    // UnSerialized Variables or Hidden Variables
    [HideInInspector] public Vector3 velocity;
    private DefaultInputMap playerInput;
    private float moveInput;
    private float steerInput;
    private RaycastHit hit;
    private float vehicleXTiltIncrement = 0.09f;
    private float currentVelocityOffset;

    private void Awake()
    {
        playerInput = new DefaultInputMap();
    }

    private void Start()
    {
        ActionEventsSubscription();
        
        centerRB.transform.parent = null;
        vehicleBody.transform.parent = null;

        m_rayLength = centerRB.GetComponent<SphereCollider>().radius + 0.2f;
    }

    private void Update()
    {
        steerInput = playerInput.InGame.Move.ReadValue<Vector2>().x;
        moveInput = playerInput.InGame.Move.ReadValue<Vector2>().y;

        transform.position = centerRB.transform.position;
        velocity = vehicleBody.transform.InverseTransformDirection(vehicleBody.velocity);
        currentVelocityOffset = velocity.z / m_maxSpeed;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (Grounded())
        {
            if (!brakeState)
            {
                Acceleration();
                Rotation();
            }
            Brake();
        }
        else
        {
            Gravity();
        }
        VehicleTilt();
    }

    private void Acceleration()
    {
        centerRB.velocity = Vector3.Lerp(centerRB.velocity, m_maxSpeed * moveInput * transform.forward, Time.fixedDeltaTime * m_acceleration);
    }

    private void Rotation()
    {
        transform.Rotate(0, steerInput * moveInput * m_steerStrength * Time.fixedDeltaTime, 0, Space.World);

        vehicleHandle.transform.localRotation = Quaternion.Slerp(vehicleHandle.transform.localRotation,
                                                                 Quaternion.Euler(vehicleHandle.transform.localRotation.eulerAngles.x, handleRotVal * steerInput, vehicleHandle.transform.localRotation.eulerAngles.z),
                                                                 handleRotSpeed);
    }

    private void VehicleTilt()
    {
        float xRot = (Quaternion.FromToRotation(vehicleBody.transform.up, hit.normal) * vehicleBody.transform.rotation).eulerAngles.x;
        float zRot = 0;

        if (currentVelocityOffset > 0)
        {
            zRot = -zTiltAngle * steerInput * currentVelocityOffset;
        }

        Quaternion targetRot = Quaternion.Slerp(vehicleBody.transform.rotation, Quaternion.Euler(xRot, transform.eulerAngles.y, zRot), vehicleXTiltIncrement);
        
        Quaternion newRotation = Quaternion.Euler(targetRot.eulerAngles.x, transform.eulerAngles.y, targetRot.eulerAngles.z);

        vehicleBody.MoveRotation(newRotation);
    }

    private void Brake()
    {
        if (brakeState)
        {
            centerRB.velocity *= m_brakingFactor / 10;
        }
    }

    private bool Grounded()
    {
        if (Physics.Raycast(centerRB.position, Vector3.down, out hit, m_rayLength, drivableSurfice))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Gravity()
    {
        centerRB.AddForce(m_gravity * Vector3.down, ForceMode.Acceleration);
    }

    #region Input System Methods
    private void ActionEventsSubscription()
    {
        BrakeSubscription();
    }

    private void ActionEventsUnSubscription()
    {
        BrakeUnSubscription();
    }
    
    private void OnEnable()
    {
        playerInput.InGame.Enable();
    }

    private void OnDisable()
    {
        playerInput.InGame.Disable();
    }
    #endregion

    #region Action Events

    #region Braking
    private bool brakeState = false;
    private void BrakeSubscription()
    {
        playerInput.InGame.Brake.started += OnBrakeStarted;
        playerInput.InGame.Brake.canceled += OnBrakeStopped;
    }

    private void BrakeUnSubscription()
    {
        playerInput.InGame.Brake.started -= OnBrakeStarted;
        playerInput.InGame.Brake.canceled -= OnBrakeStopped;
    }

    private void OnBrakeStarted(InputAction.CallbackContext context)
    {
        if (!brakeState)
        {
            brakeState = true;
        }
    }

    private void OnBrakeStopped(InputAction.CallbackContext context)
    {
        if (brakeState)
        {
            brakeState = false;
        }
    }
    #endregion

    #endregion
}
