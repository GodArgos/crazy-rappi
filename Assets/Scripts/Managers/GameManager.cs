using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    [Header("Attributes")]
    public int timer = 30;
    public int totalMoney = 0;
    public int currentMoney = 0;
    public int deliveriesQuantity = 0;

    [Header("DEPENDENCIES")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private Transform playerParent;
    [SerializeField] private ArrowPointer arrow;
    [SerializeField] private DeliveryManager deliveryManager;
    [SerializeField] private SummaryMenuLogic summaryLogic;
    [SerializeField] private MusicController musicController;
    [HideInInspector] public GameObject currentPlayer;
    private float timerAccumulator = 0f;
    private Rigidbody playerRigidbody;

    #region Events
    public Action<int> OnTimerChanged;
    public Action<int> OnTotalMoneyChanged;
    public Action<int> OnCurrentMoneyChanged;
    #endregion

    private void Start()
    {
        SpawnPlayer();
        OnTimerChanged?.Invoke(timer);
        OnTotalMoneyChanged?.Invoke(totalMoney);
        OnCurrentMoneyChanged?.Invoke(currentMoney);

        deliveryManager.player = currentPlayer.transform;
        deliveryManager.playerRigidbody = playerRigidbody;
        deliveryManager.StartDeliveryCycle();
    }

    private void Update()
    {
        UpdateTimer(Time.deltaTime);
    }

    private void SpawnPlayer()
    {
        int selectedPrefab = DataManager.Instance.selectedVehicle;
        int prefabsCount = playerParent.childCount;
        Transform selectedSpawn = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];

        if (selectedPrefab < prefabsCount)
        {
            // Desactivar todos los hijos primero
            for (int i = 0; i < prefabsCount; i++)
            {
                playerParent.GetChild(i).gameObject.SetActive(false);
            }

            // Activar el vehículo seleccionado
            Transform selectedVehicleTransform = playerParent.GetChild(selectedPrefab);
            selectedVehicleTransform.gameObject.SetActive(true);

            // Mover el vehículo activado a la posición de spawn
            selectedVehicleTransform.position = selectedSpawn.position;
            selectedVehicleTransform.rotation = selectedSpawn.rotation;

            currentPlayer = selectedVehicleTransform.gameObject;
            arrow.player = currentPlayer.transform;
            playerRigidbody = selectedVehicleTransform.GetComponent<TwoWheelVehicleController>().centerRB;
            virtualCamera.Follow = selectedVehicleTransform;
            virtualCamera.LookAt = selectedVehicleTransform.GetComponent<TwoWheelVehicleController>().lookCamera;

            selectedVehicleTransform.parent = null;
        }
        else
        {
            Debug.LogError("Selected vehicle index out of range!");
        }
    }

    public void UpdateTimer(float deltaTime)
    {
        timerAccumulator += deltaTime;

        // Restar 1 segundo cada vez que el acumulador supere 1
        while (timerAccumulator >= 1f)
        {
            timer--;
            timerAccumulator -= 1f;
            
            // Verificar si el tiempo se agotó
            if (timer <= 0)
            {
                timer = 0;
                EndGame();
                return;
            }

            OnTimerChanged?.Invoke(timer);
        }
    }

    public void AddTimeBonification(int bonification)
    {
        timer += bonification;
        OnTimerChanged?.Invoke(timer);
    }

    public void AddToTotalMoney(int amount)
    {
        totalMoney += amount;
        OnTotalMoneyChanged?.Invoke(totalMoney);
    }

    public void UpdateCurrentMoney(int amount)
    {
        currentMoney = amount;
        OnCurrentMoneyChanged?.Invoke(currentMoney);
    }

    public void UpdateDeliveriesQuantity()
    {
        deliveriesQuantity++;
    }

    public void ResetCurrentMoney()
    {
        currentMoney = 0;
        OnCurrentMoneyChanged?.Invoke(currentMoney);
    }

    private void EndGame()
    {
        musicController.StopAll();
        summaryLogic.ShowSummary(deliveriesQuantity, totalMoney);
    }
}
