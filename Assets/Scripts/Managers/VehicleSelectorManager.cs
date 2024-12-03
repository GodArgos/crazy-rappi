using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class VehicleSelectorManager : MonoBehaviour
{
    [Header("UI Canvases")]
    [SerializeField] private GameObject vehicleSelector;
    [SerializeField] private GameObject levelSelectorCanvas;
    [SerializeField] private LevelControlManager levelSelectorManager;
    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private GameObject summaryCanvas;

    [Header("Dependencies")]
    [SerializeField] private List<GameObject> vehiclesLabels;
    [SerializeField] private List<GameObject> vehiclesPrefabs;
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private AudioClip transitionClip;
    [SerializeField] private Animator vehicleAnimator;
    [SerializeField] private GameObject costText;
    [SerializeField] private AudioClip purchaseClip;

    private AudioSource audioSource;
    private Dictionary<int, GameObject> indexedVehicles;
    private Dictionary<int, List<GameObject>> indexedLabels;
    public int currentVehicle = 0;
    private DefaultInputMap playerInput;

    private void Awake()
    {
        playerInput = new DefaultInputMap();
    }

    private void Start()
    {
        ActionEventsSubscription();
        ArrangeVehicles();
        ArrangeLabels();
        audioSource = GetComponent<AudioSource>();
        GetComponent<VehicleSelectorManager>().enabled = false;
    }

    private void ArrangeVehicles()
    {
        indexedVehicles = new Dictionary<int, GameObject>();

        for (int i = 0; i < vehiclesPrefabs.Count; i++)
        {
            indexedVehicles[i] = vehiclesPrefabs[i];
        }

        ActivateNVehicle(currentVehicle);
    }

    private void ArrangeLabels()
    {
        Dictionary<int, List<GameObject>> states = new Dictionary<int, List<GameObject>>();

        for (int i = 0; i < vehiclesLabels.Count; i++)
        {
            List<GameObject> aux = new List<GameObject>();

            for (int j = 0; j < vehiclesLabels[i].transform.childCount; j++)
            {
                aux.Add(vehiclesLabels[i].transform.GetChild(j).gameObject);
            }

            states[i] = aux;
        }

        indexedLabels = states;
        ActivateNLabel(currentVehicle);
    }

    private void ActivateNLabel(int index)
    {
        foreach (var label in indexedLabels)
        {
            if (label.Key == index)
            {
                if (DataManager.Instance.purchasedVehicles.Contains(currentVehicle))
                {
                    label.Value[0].SetActive(true);

                    // Verificar si existe el índice 1 antes de acceder
                    if (label.Value.Count > 1 && label.Value[1] != null)
                    {
                        label.Value[1].SetActive(false);
                    }
                }
                else
                {
                    label.Value[0].SetActive(false);

                    // Verificar si existe el índice 1 antes de acceder
                    if (label.Value.Count > 1 && label.Value[1] != null)
                    {
                        label.Value[1].SetActive(true);
                    }
                }
            }
            else
            {
                label.Value[0].SetActive(false);

                // Verificar si existe el índice 1 antes de acceder
                if (label.Value.Count > 1 && label.Value[1] != null)
                {
                    label.Value[1].SetActive(false);
                }
            }
        }
    }

    private void ActivateNVehicle(int index)
    {
        foreach (var vehicle in indexedVehicles)
        {
            if (vehicle.Key == index)
            {
                vehicle.Value.SetActive(true);
            }
            else
            {
                vehicle.Value.SetActive(false);
            }
        }
    }

    public void ChangeVehicle(int value)
    {
        currentVehicle += value;
        if (currentVehicle >= vehiclesPrefabs.Count)
        {
            currentVehicle = 0;
        }
        else if (currentVehicle < 0)
        {
            currentVehicle = vehiclesPrefabs.Count - 1;
        }

        transitionAnimator.SetTrigger("LevelChanged");
        audioSource.PlayOneShot(transitionClip);
        ActivateNVehicle(currentVehicle);
        ActivateNLabel(currentVehicle);
    }

    private void ChangeVehiclePrevious(InputAction.CallbackContext context)
    {
        ChangeVehicle(-1);
    }

    private void ChangeVehicleNext(InputAction.CallbackContext context)
    {
        ChangeVehicle(1);
    }

    private void ChooseVehicle(InputAction.CallbackContext context)
    {
        if (DataManager.Instance.purchasedVehicles.Contains(currentVehicle))
        {
            // Desactivar UI de Seleccion de nivel
            vehicleSelector.SetActive(false);
            //summaryCanvas.SetActive(true);
            GetComponent<VehicleSelectorManager>().enabled = false;
        }
        else
        {
            float value = -DataManager.Instance.vehiclesPrice[currentVehicle];

            DataManager.Instance.UpdateMoney(value);
            DataManager.Instance.purchasedVehicles.Add(currentVehicle);
            ActivateNLabel(currentVehicle);
            
            costText.GetComponent<TextMeshProUGUI>().text = value.ToString("N0").Replace(",", " ");
            vehicleAnimator.SetTrigger("CostTrigger");
            audioSource.PlayOneShot(purchaseClip);
        }
    }

    private void BackToMainMenuAction(InputAction.CallbackContext context)
    {
        levelSelectorCanvas.SetActive(true);
        levelSelectorManager.enabled = true;
        vehicleSelector.SetActive(false);
        GetComponent<VehicleSelectorManager>().enabled = false;
    }

    public void BackToMainMenu()
    {
        levelSelectorCanvas.SetActive(true);
        vehicleSelector.SetActive(false);
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
        playerInput.Menu.Accept.performed += ChooseVehicle;
        playerInput.Menu.Back.performed += BackToMainMenuAction;
        playerInput.Menu.Left.performed += ChangeVehiclePrevious;
        playerInput.Menu.Right.performed += ChangeVehicleNext;
    }

    private void ControllsUnSubscription()
    {
        playerInput.Menu.Accept.performed -= ChooseVehicle;
        playerInput.Menu.Back.performed -= BackToMainMenuAction;
        playerInput.Menu.Left.performed -= ChangeVehiclePrevious;
        playerInput.Menu.Right.performed -= ChangeVehicleNext;
    }
    #endregion
    #endregion
}
