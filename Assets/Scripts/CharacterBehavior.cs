using UnityEngine;

public class CharacterBehavior : MonoBehaviour
{
    public float detectionRadius = 5f; // Radio para detectar objetos cercanos en el estado Scape.
    public float walkSpeed = 2f; // Velocidad de caminar.
    public float scapeSpeed = 5f; // Velocidad de escape.
    public Transform[] waypoints; // Puntos a los que caminará.

    private int currentWaypointIndex = 0; // Índice del waypoint actual.
    private float stateChangeTimer = 0f; // Temporizador para cambiar de estado.
    private float stateDuration = 30f; // Duración de cada estado (10 segundos).
    private State currentState = State.Walk; // Estado inicial.
    private Vector3 initialPosition; // Posición inicial para evitar bloqueos.

    private enum State
    {
        Scape,
        Walk,
        Stalk
    }

    void Start()
    {
        initialPosition = transform.position; // Guardar la posición inicial.
    }

    void Update()
    {
        stateChangeTimer += Time.deltaTime;

        // Cambiar estado cada 10 segundos.
        if (stateChangeTimer >= stateDuration)
        {
            stateChangeTimer = 0f;
            SwitchState();
        }

        // Ejecutar el comportamiento según el estado actual.
        ExecuteStateBehavior();
    }

    private void SwitchState()
    {
        // Cambiar al siguiente estado en secuencia.
        State previousState = currentState;
        currentState = (State)(((int)currentState + 1) % 3);

        // Reiniciar datos específicos para evitar bloqueos.
        if (currentState == State.Walk)
        {
            currentWaypointIndex = 0; // Reinicia waypoint al comenzar a caminar.
        }

        // Log para depuración.
        Debug.Log($"Cambio de estado: {previousState} → {currentState}");
    }

    private void ExecuteStateBehavior()
    {
        switch (currentState)
        {
            case State.Scape:
                ScapeBehavior();
                break;
            case State.Walk:
                WalkBehavior();
                break;
            case State.Stalk:
                StalkBehavior();
                break;
        }
    }

    private void ScapeBehavior()
    {
        // Detectar todos los colliders dentro del radio.
        Collider[] objectsNearby = Physics.OverlapSphere(transform.position, detectionRadius);

        if (objectsNearby.Length > 0)
        {
            // Escapar en la dirección opuesta al primer objeto detectado.
            Vector3 scapeDirection = (transform.position - objectsNearby[0].transform.position).normalized;
            transform.position += scapeDirection * scapeSpeed * Time.deltaTime;

            Debug.Log("Escapando de un objeto detectado.");
        }
        else
        {
            Debug.Log("Nada que escapar. Sin objetos cerca.");
        }
    }

    private void WalkBehavior()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("No hay waypoints configurados.");
            return;
        }

        // Caminar hacia el siguiente waypoint.
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        // Mover hacia el waypoint si no está lo suficientemente cerca.
        if (Vector3.Distance(transform.position, targetWaypoint.position) > 0.1f)
        {
            transform.position += direction * walkSpeed * Time.deltaTime;
        }
        else
        {
            // Cambiar al siguiente waypoint si se alcanza el actual.
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        Debug.Log("Caminando hacia el siguiente waypoint.");
    }

    private void StalkBehavior()
    {
        // Debug para verificar que está en estado Stalk.
        Debug.Log("En estado Stalk: personaje inmóvil.");
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar el radio de detección para el estado Scape.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
