using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDLogic : MonoBehaviour
{
    [Header("DEPENDENCIES")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI totalMoneyText;
    [SerializeField] private TextMeshProUGUI currentMoneyText;

    private void Start()
    {
        GameManager.Instance.OnTimerChanged += UpdateTimerText;
        GameManager.Instance.OnTotalMoneyChanged += UpdateTotalMoneyText;
        GameManager.Instance.OnCurrentMoneyChanged += UpdateCurrentMoneyText;
    }

    private void UpdateTimerText(int time)
    {
        timerText.text = time.ToString("000");
    }

    private void UpdateTotalMoneyText(int total)
    {
        totalMoneyText.text = $"S/. {total}";
    }

    private void UpdateCurrentMoneyText(int amount)
    {
        currentMoneyText.text = $"S/. {amount}";
    }
}
