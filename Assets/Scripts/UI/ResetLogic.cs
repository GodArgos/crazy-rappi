using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ResetLogic : MonoBehaviour
{
    [Header("DEPENDENCIES")]
    [SerializeField] private GameObject resetUI;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private float resetDuration = 2.5f;
    [SerializeField] private Transform[] spawnPoints;

    private TwoWheelVehicleController vehicleController;
    private float currentProgress = 0f;
    private bool isResetting = false;
    private bool isCooldown = false;
    private float cooldownTimer = 0f;
    private float cooldownDuration = 0.5f;
    private Rigidbody playerRb;
    private Rigidbody vehicleRb;
    private DefaultInputMap playerInput;
    
    private void Awake()
    {
        playerInput = new DefaultInputMap();
    }

    private void Start()
    {
        ActionEventsSubscription();
        resetUI.SetActive(false);
        progressSlider.value = 0f;
    }

    public void OnResetPerformed(InputAction.CallbackContext context)
    {
        StartReset();
    }

    public void OnResetCanceled(InputAction.CallbackContext context)
    {
        CancelReset();
    }

    private void Update()
    {
        if (isResetting)
        {
            currentProgress += Time.deltaTime / resetDuration;
            progressSlider.value = currentProgress;

            if (currentProgress >= 1f)
            {
                CompleteReset();
            }
        }
        else if (isCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownDuration)
            {
                ResetUI();
            }
        }
    }

    private void StartReset()
    {
        isResetting = true;
        isCooldown = false;
        cooldownTimer = 0f;
        resetUI.SetActive(true);
    }

    private void CancelReset()
    {
        isResetting = false;
        isCooldown = true;
    }

    private void CompleteReset()
    {
        isResetting = false;
        isCooldown = false;
        resetUI.SetActive(false);

        vehicleController = GameManager.Instance.currentPlayer.GetComponent<TwoWheelVehicleController>();
        playerRb = vehicleController.centerRB;
        vehicleRb = vehicleController.vehicleBody;

        // Desactiva el vehículo
        vehicleController.enabled = false; 
        playerRb.isKinematic = true;
        vehicleRb.isKinematic = true;

        // Teletransporta al jugador
        Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        playerRb.transform.position = randomSpawn.position;
        playerRb.transform.rotation = randomSpawn.rotation;
        vehicleRb.transform.position = randomSpawn.position;
        vehicleRb.transform.rotation = randomSpawn.rotation;

        // Reactiva el vehículo
        vehicleController.enabled = true;
        playerRb.isKinematic = false;
        vehicleRb.isKinematic = false;

        Debug.Log("SE LLEGO AL MAXIMO");

        // Reinicia el temporizador y slider
        currentProgress = 0f;
        progressSlider.value = 0f;
    }

    private void ResetUI()
    {
        resetUI.SetActive(false);
        currentProgress = 0f;
        progressSlider.value = 0f;
        isCooldown = false;
    }

    #region Input System Methods
    private void ActionEventsSubscription()
    {
        ResetSubscription();
    }

    private void ActionEventsUnSubscription()
    {
        ResetUnSubscription();
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

    #region Reseting
    private void ResetSubscription()
    {
        playerInput.InGame.Reset.performed += OnResetPerformed;
        playerInput.InGame.Reset.canceled += OnResetCanceled;
    }

    private void ResetUnSubscription()
    {
        playerInput.InGame.Reset.performed -= OnResetPerformed;
        playerInput.InGame.Reset.canceled -= OnResetCanceled;
    }
    #endregion

    #endregion
}
