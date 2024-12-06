using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    [Header("Dependencies")]
    private GameManager gameManager; // Referencia al GameManager
    public Transform collectPointsParent; // Padre de los puntos de recojo
    public Transform deliveryPointsParent; // Padre de los puntos de entrega
    public GameObject pickupTriggerPrefab; // Prefab del trigger de recojo
    public GameObject deliveryTriggerPrefab; // Prefab del trigger de entrega
    [HideInInspector] public Transform player; // Referencia al jugador
    [SerializeField] private ArrowPointer arrowController; // Controlador de la flecha

    [HideInInspector] public Rigidbody playerRigidbody;
    private Transform currentCollectPoint;
    private Transform currentDeliveryPoint;
    private float currentDeliveryPrice = 0;
    private int timeBonus = 0;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }


    public void StartDeliveryCycle()
    {
        SelectCollectPoint();
    }

    private void SelectCollectPoint()
    {
        // Actualiza el GameManager con el precio de la entrega actual
        gameManager.UpdateCurrentMoney(0);

        // Selecciona un punto de recojo cercano al jugador
        currentCollectPoint = GetClosestPoint(collectPointsParent, player.position);

        // Actualiza la flecha para que apunte al punto de recojo
        arrowController.SetTarget(currentCollectPoint);

        // Instancia el trigger de recojo
        InstantiatePickupTrigger(currentCollectPoint);
    }

    private void SelectDeliveryPoint()
    {
        // Selecciona un punto de entrega aleatorio
        currentDeliveryPoint = GetRandomPoint(deliveryPointsParent);

        // Calcula el precio de la entrega y la bonificación de tiempo
        CalculateDeliveryReward();

        // Actualiza el GameManager con el precio de la entrega actual
        gameManager.UpdateCurrentMoney((int)currentDeliveryPrice);

        // Actualiza la flecha para que apunte al punto de entrega
        arrowController.SetTarget(currentDeliveryPoint);

        // Instancia el trigger de entrega
        InstantiateDeliveryTrigger(currentDeliveryPoint);
    }

    private Transform GetClosestPoint(Transform parent, Vector3 position)
    {
        Transform closestPoint = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform child in parent)
        {
            float distance = Vector3.Distance(position, child.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPoint = child;
            }
        }

        return closestPoint;
    }

    private Transform GetRandomPoint(Transform parent)
    {
        int index = Random.Range(0, parent.childCount);
        return parent.GetChild(index);
    }

    private void CalculateDeliveryReward()
    {
        float distance = Vector3.Distance(currentCollectPoint.position, currentDeliveryPoint.position);
        currentDeliveryPrice = distance / 10f; // Factor para calcular el precio
        timeBonus = (int)Mathf.Clamp(distance / 5f, 0, 10f); // Factor para calcular la bonificación
    }

    private void InstantiatePickupTrigger(Transform position)
    {
        GameObject pickupTrigger = Instantiate(pickupTriggerPrefab, position.position, Quaternion.identity);
        PickUpTrigger triggerScript = pickupTrigger.GetComponent<PickUpTrigger>();
        triggerScript.playerRigidbody = playerRigidbody;
        triggerScript.deliveryManager = this;
    }

    private void InstantiateDeliveryTrigger(Transform position)
    {
        GameObject deliveryTrigger = Instantiate(deliveryTriggerPrefab, position.position, Quaternion.identity);
        DeliveryTrigger triggerScript = deliveryTrigger.GetComponent<DeliveryTrigger>();
        triggerScript.playerRigidbody = playerRigidbody;
        triggerScript.deliveryManager = this;
    }

    public void OnPackagePickedUp()
    {
        // Cuando se recoge el paquete, selecciona un punto de entrega
        SelectDeliveryPoint();
    }

    public void OnPackageDelivered()
    {
        // Actualiza el dinero total y el tiempo de juego
        gameManager.AddToTotalMoney((int)currentDeliveryPrice);
        gameManager.AddTimeBonification(timeBonus);
        gameManager.UpdateDeliveriesQuantity();
        // Reinicia el ciclo seleccionando otro punto de recojo
        SelectCollectPoint();
    }
}
