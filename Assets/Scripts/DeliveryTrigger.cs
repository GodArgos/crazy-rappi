using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryTrigger : MonoBehaviour
{
    [Header("Dependencies")]
    [HideInInspector] public Rigidbody playerRigidbody;
    [HideInInspector] public DeliveryManager deliveryManager;
    [SerializeField] private float requiredStopSpeed = 0.1f;

    private bool isDelivered = false;

    private void OnTriggerStay(Collider other)
    {
        if (isDelivered) return;
        if (other.CompareTag("Player") && playerRigidbody.velocity.magnitude <= requiredStopSpeed)
        {
            DeliverPackage();
        }
    }

    private void DeliverPackage()
    {
        isDelivered = true;
        Debug.Log("Package delivered!");

        deliveryManager.OnPackageDelivered();

        // Destruir el trigger de entrega
        Destroy(gameObject);
    }
}
