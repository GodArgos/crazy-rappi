using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    [Header("Dependencies")]
    public Transform player; // Referencia al jugador
    public Transform target; // Objetivo al que la flecha debe apuntar
    [SerializeField] private float yOffset = 2.0f;

    private Vector3 initialRotation; // Rotación inicial de la flecha (X y Z)

    private void Start()
    {
        // Guardar la rotación inicial de la flecha (solo X y Z)
        initialRotation = transform.eulerAngles;
    }

    private void LateUpdate()
    {
        if (player == null || target == null) return;

        // Seguir la posición del jugador
        Vector3 playerPosition = player.position;
        playerPosition.y += yOffset; // Ajustar altura encima del jugador
        transform.position = playerPosition;

        // Calcular la dirección hacia el objetivo
        Vector3 directionToTarget = target.position - transform.position;
        directionToTarget.y = 0; // Ignorar la altura para que solo gire en el eje Y

        // Actualizar la rotación solo en el eje Y
        if (directionToTarget.sqrMagnitude > 0.01f) // Evitar rotación errática si está muy cerca del objetivo
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            Vector3 finalRotation = targetRotation.eulerAngles;
            transform.rotation = Quaternion.Euler(initialRotation.x, initialRotation.y + finalRotation.y, initialRotation.z);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
