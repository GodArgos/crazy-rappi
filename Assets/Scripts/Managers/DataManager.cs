using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class DataManager : MonoBehaviour
{
    #region Singleton
    public static DataManager Instance { get ; private set ; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        saveFilePath = GetSavePath("gameData.json");
        LoadData();
    }
    #endregion

    #region DataClass
    [System.Serializable]
    private class GameData
    {
        public float totalMoney;
        public int totalDeliveries;
        public List<int> availableLevels;
        public List<int> purchasedVehicles;
    }
    #endregion

    #region Events
    public event Action<float> OnMoneyChanged;
    public event Action<int> OnDeliveryChanged;
    #endregion

    [Header("Attributes")]
    public float totalMoney;
    public int totalDeliveries;
    public List<int> availableLevels;
    public List<int> purchasedVehicles;

    [Header("Temp Data")]
    public int selectedLevel = 0;
    public int selectedVehicle = 0;

    private string saveFilePath;
    private float MAX_MONEY = 999999f;
    private int MAX_DELIVERY = 999999;

    public Dictionary<int, float> vehiclesPrice = new Dictionary<int, float>
    {
        { 0, 0 },
        { 1, 2000 },
        { 2, 5000 }
    };

    private void LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            string jsonData = File.ReadAllText(saveFilePath);
            GameData data = JsonUtility.FromJson<GameData>(jsonData);

            totalMoney = data.totalMoney;
            totalDeliveries = data.totalDeliveries;
            availableLevels = new List<int>(data.availableLevels);
            purchasedVehicles = new List<int>(data.purchasedVehicles);
        }
        else
        {
            // Crear datos iniciales si no existe el archivo
            totalMoney = 0;
            totalDeliveries = 0;
            availableLevels = new List<int> { 0 }; // Nivel inicial disponible
            purchasedVehicles = new List<int> { 0 }; // Vehículos inicial disponible
            SaveData();
        }
    }

    // Guardar datos en el archivo JSON
    public void SaveData()
    {
        GameData data = new GameData
        {
            totalMoney = totalMoney,
            totalDeliveries = totalDeliveries,
            availableLevels = availableLevels,
            purchasedVehicles = purchasedVehicles
        };

        string jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, jsonData);
    }

    // Ejemplo de funciones para modificar datos y guardar automáticamente
    public void UpdateMoney(float amount)
    {
        totalMoney += amount;
        if (totalMoney >= MAX_MONEY)
        {
            totalMoney = MAX_MONEY;
        }

        OnMoneyChanged?.Invoke(totalMoney); // Dispara el evento
        SaveData();
    }

    public void UpdateDelivery(int amount)
    {
        totalDeliveries += amount;
        if (totalDeliveries >= MAX_DELIVERY)
        {
            totalDeliveries = MAX_DELIVERY;
        }

        OnDeliveryChanged?.Invoke(totalDeliveries); // Dispara el evento
        SaveData();
    }

    public void UnlockLevel(int levelIndex)
    {
        if (!availableLevels.Contains(levelIndex))
        {
            availableLevels.Add(levelIndex);
            SaveData();
        }
    }

    public void PurchaseVehicle(int vehicleIndex)
    {
        if (!purchasedVehicles.Contains(vehicleIndex))
        {
            purchasedVehicles.Add(vehicleIndex);
            SaveData();
        }
    }

    private string GetSavePath(string filename)
    {
        var path = Application.persistentDataPath;
#if UNITY_WEBGL && !UNITY_EDITOR
         path = "/idbfs/crazy-rappi";
          if (!Directory.Exists(path)) {
             Directory.CreateDirectory(path);
             Debug.Log("Creating save directory: " + path);
         }
#endif
        var result = Path.Combine(path, filename);
        Debug.Log("Save path: " + result);
        return result;
    }
}
