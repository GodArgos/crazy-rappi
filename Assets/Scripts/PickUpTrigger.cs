using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpTrigger : MonoBehaviour
{
    [Header("Dependencies")]
    public Rigidbody playerRigidbody; // Rigidbody del jugador para verificar velocidad
    [HideInInspector] public DeliveryManager deliveryManager; // Referencia al DeliveryManager
    [SerializeField] private float requiredStopSpeed = 0.1f; // Velocidad m�xima permitida para recoger el paquete

    private bool isPickedUp = false; // Bandera para evitar m�ltiples activaciones

    private void OnTriggerStay(Collider other)
    {
        if (isPickedUp) return; // Evitar m�ltiples activaciones
        Debug.Log("HEREREE");
        if (other.CompareTag("Player")) // Verifica que el jugador haya entrado
        {
            Debug.Log("AJAJAJAJ");
            // Si el jugador est� dentro del trigger, verifica su velocidad
            if (playerRigidbody.velocity.magnitude <= requiredStopSpeed)
            {
                Debug.Log("JAOSDJAOJS");
                PickupPackage();
            }
        }
    }

    private void PickupPackage()
    {
        isPickedUp = true; // Marca el paquete como recogido

        // Notifica al DeliveryManager para pasar al siguiente paso (entrega)
        Debug.Log("Package picked up!");
        deliveryManager.OnPackagePickedUp();

        // Destruir el trigger despu�s de recoger el paquete
        Destroy(gameObject);
    }
}
