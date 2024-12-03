using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SummaryMenuLogic : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI deliveriesText; // Texto para mostrar las entregas completadas
    public TextMeshProUGUI totalMoneyText; // Texto para mostrar el dinero total
    public GameObject summaryMenuUI; // El contenedor del menú de resumen
    public GameObject HudUI;
    public PauseMenuLogic pausedMenu;

    private int totalDeliveries = 0;
    private int totalMoney = 0;

    private void Start()
    {
        summaryMenuUI.SetActive(false);
    }

    public void ShowSummary(int deliveries, int money)
    {
        totalDeliveries = deliveries;
        totalMoney = money;

        // Actualizar textos del menú de resumen
        deliveriesText.text = totalDeliveries.ToString();
        totalMoneyText.text = $"S/. {totalMoney}";

        // Mostrar el menú de resumen
        pausedMenu.enabled = false;
        HudUI.SetActive(false);
        summaryMenuUI.SetActive(true);

        // Pausar el tiempo del juego
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        // Actualizar valores en el DataManager antes de reiniciar
        DataManager.Instance.UpdateDelivery(totalDeliveries);
        DataManager.Instance.UpdateMoney(totalMoney);

        // Reanudar el tiempo del juego y reiniciar el nivel
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToPreviousScene()
    {
        // Actualizar valores en el DataManager antes de regresar
        DataManager.Instance.UpdateDelivery(totalDeliveries);
        DataManager.Instance.UpdateMoney(totalMoney);

        // Reanudar el tiempo del juego y cargar la escena anterior
        Time.timeScale = 1f;
        int previousSceneIndex = Mathf.Max(SceneManager.GetActiveScene().buildIndex - 1, 0);
        SceneManager.LoadScene(previousSceneIndex);
    }
}
