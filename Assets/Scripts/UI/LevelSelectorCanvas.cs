using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelSelectorCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI deliveriesText;

    private void Start()
    {
        // Suscribirse a los eventos de DataManager
        DataManager.Instance.OnMoneyChanged += UpdateMoneyUI;
        DataManager.Instance.OnDeliveryChanged += UpdateDeliveriesUI;

        // Inicializar los valores al cargar
        UpdateMoneyUI(DataManager.Instance.totalMoney);
        UpdateDeliveriesUI(DataManager.Instance.totalDeliveries);
    }

    private void OnEnable()
    {
        // Inicializar los valores al cargar
        UpdateMoneyUI(DataManager.Instance.totalMoney);
        UpdateDeliveriesUI(DataManager.Instance.totalDeliveries);
    }

    //private void OnDisable()
    //{
    //    // Desuscribirse de los eventos de DataManager
    //    if (DataManager.Instance != null)
    //    {
    //        DataManager.Instance.OnMoneyChanged -= UpdateMoneyUI;
    //        DataManager.Instance.OnDeliveryChanged -= UpdateDeliveriesUI;
    //    }
    //}

    private void UpdateMoneyUI(float money)
    {
        moneyText.text = money.ToString("000 000");
    }

    private void UpdateDeliveriesUI(int deliveries)
    {
        deliveriesText.text = deliveries.ToString("000 000");
    }

}
