using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoFollowCamera : MonoBehaviour
{
    public Transform cameraTransform; // La transform de la cámara.
    public float distanceZ = 10f;     // Distancia fija en el eje Z.

    void Update()
    {
        if (cameraTransform != null)
        {
            // Ajusta la posición del Quad para que esté frente a la cámara.
            Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * distanceZ;
            transform.position = targetPosition;

            // Asegúrate de que el Quad siempre mire a la cámara.
            transform.LookAt(cameraTransform);

            // Rota el Quad para que el lado visible quede frente a la cámara.
            transform.Rotate(0f, 180f, 0f);
        }
    }
}
