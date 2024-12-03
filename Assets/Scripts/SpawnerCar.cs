using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerCar : MonoBehaviour
{
    [SerializeField]
    private GameObject[] cars;
    private int spawnIndex;
    [SerializeField]
    private float spawnTime = 5f;

    private int alreadySpawnedCount = 0;

    [SerializeField]
    private bool isInfinity = false;
    private int maxSpawnCount = 20;

    [SerializeField]
    private int rotation;

    private float elapsedTime = 0f;
    private int vehiclesInCollider = 0;

    private void Start()
    {
        spawnIndex = 0;
    }

    private void Update()
    {
        if (alreadySpawnedCount >= maxSpawnCount && !isInfinity)
            return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= spawnTime && vehiclesInCollider == 0)
        {
            SpawnCar();
            elapsedTime = 0f;
        }
    }

    private void SpawnCar()
    {
        spawnIndex = Random.Range(0, cars.Length - 1);
        Instantiate(cars[spawnIndex], transform.position, Quaternion.Euler(0, rotation, 0));
        alreadySpawnedCount++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("AutonomousVehicle"))
        {
            vehiclesInCollider++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("AutonomousVehicle"))
        {
            vehiclesInCollider--;

            if (vehiclesInCollider < 0)
                vehiclesInCollider = 0;
        }
    }
}
