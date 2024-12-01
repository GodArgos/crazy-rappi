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

        saveFilePath = Path.Combine(Application.persistentDataPath, "gameData.json");
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

    [Header("Attributes")]
    public float totalMoney;
    public int totalDeliveries;
    public List<int> availableLevels;
    public List<int> purchasedVehicles;

    private string saveFilePath;

    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            AddDelivery();
        }
    }

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
            availableLevels = new List<int> { 1 }; // Nivel inicial disponible
            purchasedVehicles = new List<int> { 1 }; // Vehículos inicial disponible
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
    public void AddMoney(float amount)
    {
        totalMoney += amount;
        SaveData();
    }

    public void AddDelivery()
    {
        totalDeliveries++;
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
}
