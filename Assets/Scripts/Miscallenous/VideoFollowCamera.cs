using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoFollowCamera : MonoBehaviour
{
    public Transform cameraTransform; // La transform de la c�mara.
    public float distanceZ = 10f;     // Distancia fija en el eje Z.

    void Update()
    {
        if (cameraTransform != null)
        {
            // Ajusta la posici�n del Quad para que est� frente a la c�mara.
            Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * distanceZ;
            transform.position = targetPosition;

            // Aseg�rate de que el Quad siempre mire a la c�mara.
            transform.LookAt(cameraTransform);

            // Rota el Quad para que el lado visible quede frente a la c�mara.
            transform.Rotate(0f, 180f, 0f);
        }
    }
}
