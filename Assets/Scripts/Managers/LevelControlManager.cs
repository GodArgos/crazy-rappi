using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelControlManager : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectorCanvas;
    [SerializeField] private GameObject vehicleSelectorCanvas;
    [SerializeField] private VehicleSelectorManager vehicleSelectorManager;
    //[SerializeField] private List<CinemachineVirtualCamera> levelCameras;
    [SerializeField] private List<GameObject> levelLabels;
    [SerializeField] private List<GameObject> levelPreviews;
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private AudioClip transitionClip;
    private AudioSource audioSource;
    //private Dictionary<int, CinemachineVirtualCamera> indexedCameras;
    private Dictionary<int, GameObject> indexedLabels;
    private Dictionary<int, GameObject> indexedLevels;
    public int currentLevel = 0;
    private int maxPriority = 100;
    private int minPriority = 0;
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
        //ArrangeCameras();
        ArrangeLabels();
        ArrangeLevels();
        buttonSFX = GetComponent<ButtonPressSFX>();
        vehicleSelectorCanvas.SetActive(false);
    }

    //private void ArrangeCameras()
    //{
    //    indexedCameras = new Dictionary<int, CinemachineVirtualCamera>();

    //    for (int i = 0; i < levelCameras.Count;i++)
    //    {
    //        indexedCameras[i] = levelCameras[i];
    //    }

    //    ActivateNCamera(currentLevel);
    //}

    private void ArrangeLabels()
    {
        indexedLabels = new Dictionary<int, GameObject>();

        for (int i = 0; i < levelLabels.Count; i++)
        {
            indexedLabels[i] = levelLabels[i];
        }

        ActivateNLabel(currentLevel);
    }

    private void ArrangeLevels()
    {
        indexedLevels = new Dictionary<int, GameObject>();

        for (int i = 0; i < levelPreviews.Count; i++)
        {
            indexedLevels[i] = levelPreviews[i];
        }

        ActivateNLevel(currentLevel);
    }

    private void ActivateNLevel(int index)
    {
        foreach (var level in indexedLevels)
        {
            if (level.Key == index)
            {
                level.Value.SetActive(true);
            }
            else
            {
                level.Value.SetActive(false);
            }
        }
    }

    //private void ActivateNCamera(int index)
    //{
    //    foreach (var cam in indexedCameras)
    //    {
    //        if (cam.Key == index)
    //        {
    //            cam.Value.Priority = maxPriority;
    //        }
    //        else
    //        {
    //            cam.Value.Priority = minPriority;
    //        }
    //    }
    //}

    private void ActivateNLabel(int index)
    {
        foreach (var label in indexedLabels)
        {
            if (label.Key == index)
            {
                label.Value.SetActive(true);
            }
            else
            {
                label.Value.SetActive(false);
            }
        }
    }

    public void ChangeCameraLevel(int value)
    {
        currentLevel += value;
        if (currentLevel >= levelPreviews.Count)
        {
            currentLevel = 0;
        }
        else if (currentLevel < 0)
        {
            currentLevel = levelPreviews.Count - 1;
        }

        transitionAnimator.SetTrigger("LevelChanged");
        audioSource.PlayOneShot(transitionClip);
        ActivateNLevel(currentLevel);
        ActivateNLabel(currentLevel);
    }

    private void ChangeCameraLevelPrevious(InputAction.CallbackContext context)
    {
        buttonSFX.ButtonSound();
        ChangeCameraLevel(-1);
    }

    private void ChangeCameraLevelNext(InputAction.CallbackContext context)
    {
        buttonSFX.ButtonSound();
        ChangeCameraLevel(1);
    }


    private void ChooseLevel(InputAction.CallbackContext context)
    {
        if (DataManager.Instance.availableLevels.Contains(currentLevel))
        {
            // Desactivar UI de Seleccion de nivel
            levelSelectorCanvas.SetActive(false);
            vehicleSelectorCanvas.SetActive(true);
            vehicleSelectorManager.enabled = true;
            DataManager.Instance.selectedLevel = currentLevel;
            GetComponent<LevelControlManager>().enabled = false;
        }
        buttonSFX.ButtonSound();
    }

    private void BackToMainMenuAction(InputAction.CallbackContext context)
    {
        buttonSFX.ButtonSound();
        BackToMainMenu();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
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
        playerInput.Menu.Accept.performed += ChooseLevel;
        playerInput.Menu.Back.performed += BackToMainMenuAction;
        playerInput.Menu.Left.performed += ChangeCameraLevelPrevious;
        playerInput.Menu.Right.performed += ChangeCameraLevelNext;
    }

    private void ControllsUnSubscription()
    {
        playerInput.Menu.Accept.performed -= ChooseLevel;
        playerInput.Menu.Back.performed -= BackToMainMenuAction;
        playerInput.Menu.Left.performed -= ChangeCameraLevelPrevious;
        playerInput.Menu.Right.performed -= ChangeCameraLevelNext;
    }
    #endregion
    #endregion
}
