using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampBoostTrigger : MonoBehaviour
{
    [Header("Boost Settings")]
    public float boostForce = 50f; // Fuerza del impulso
    public Vector3 boostDirection = new Vector3(0, 1, 1); // Dirección del impulso (Y para elevar, Z para avanzar)
    public string targetLayer = "VehicleCenter"; // Nombre de la capa del collider que debe activar el trigger

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto está en la capa especificada
        if (other.gameObject.layer == LayerMask.NameToLayer(targetLayer))
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>(); // Buscar Rigidbody en el jugador

            if (playerRigidbody != null)
            {
                // Normaliza la dirección para que la magnitud no afecte la fuerza
                Vector3 force = boostDirection.normalized * boostForce;

                // Agregar fuerza al Rigidbody del jugador
                playerRigidbody.AddForce(force, ForceMode.VelocityChange);

                Debug.Log($"Boost applied to object on layer {targetLayer}!");
            }
            else
            {
                Debug.LogWarning("No Rigidbody found on parent object!");
            }
        }
    }
}
