using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ReadyMenuManager : MonoBehaviour
{
    [Header("UI Canvases")]
    [SerializeField] private GameObject readyCanvas;
    [SerializeField] private GameObject vehicleSelector;
    [SerializeField] private VehicleSelectorManager vehicleSelectorManager;

    [Header("Dependencies")]
    [SerializeField] private AudioClip outroClip;
    [SerializeField] private Animator transitionAnimator;

    private AudioSource audioSource;
    private DefaultInputMap playerInput;
    private ButtonPressSFX buttonSFX;

    private void Awake()
    {
        playerInput = new DefaultInputMap();
    }

    private void Start()
    {
        ActionEventsSubscription();
        audioSource = GetComponent<AudioSource>();
        readyCanvas.SetActive(false);
        buttonSFX = GetComponent<ButtonPressSFX>();
        GetComponent<ReadyMenuManager>().enabled = false;
    }

    private void StartGameAction(InputAction.CallbackContext context)
    {
        buttonSFX.ButtonSound();
        transitionAnimator.SetTrigger("Ready");
        if (audioSource != null && outroClip != null)
        {
            audioSource.PlayOneShot(outroClip);
        }
    }

    public void EndTransition()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + DataManager.Instance.selectedLevel + 1);
    }

    private void BackToVehicleMenuAction(InputAction.CallbackContext context)
    {
        buttonSFX.ButtonSound();
        BackToVehicleMenu();
    }

    public void BackToVehicleMenu()
    {
        vehicleSelector.SetActive(true);
        vehicleSelectorManager.enabled = true;
        readyCanvas.SetActive(false);
        GetComponent<ReadyMenuManager>().enabled = false;
    }

    #region Input System Methods
    private void ActionEventsSubscription()
    {
        ControllsSubscription();
    }

    private void ActionEventsUnSubscription()
    {
        ControllsUnSubscription();
    }

    private void OnEnable()
    {
        playerInput.Menu.Enable();
    }

    private void OnDisable()
    {
        playerInput.Menu.Disable();
    }

    #region Action Events
    private void ControllsSubscription()
    {
        playerInput.Menu.Accept.performed += StartGameAction;
        playerInput.Menu.Back.performed += BackToVehicleMenuAction;
    }

    private void ControllsUnSubscription()
    {
        playerInput.Menu.Accept.performed -= StartGameAction;
        playerInput.Menu.Back.performed -= BackToVehicleMenuAction;
    }
    #endregion
    #endregion
}
