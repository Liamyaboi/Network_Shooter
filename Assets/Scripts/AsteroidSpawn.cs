using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AsteroidSpawn : NetworkBehaviour
{
    public GameObject _asteroidPrefab;
    public float spawnTime = 2f;
    public float spawnRadius = 1f;

    private AsteroidMovement a_movement;

    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        if (IsClient && !IsOwner) return;
        InvokeRepeating("SpawnAsteroidServerRpc", 0f, spawnTime);
    }

    /*public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        //a_movement = GetComponent<AsteroidMovement>();
    }*/

    [ServerRpc]
    public void SpawnAsteroidServerRpc()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();


        GameObject asteroid = Instantiate(_asteroidPrefab, spawnPosition, Quaternion.identity);
        asteroid.GetComponent<NetworkObject>().Spawn();
        //a_movement.AsteroidMovementServerRpc();
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Get screen bounds
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Convert screen bounds to world space
        Vector3 worldBottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.transform.position.z));
        Vector3 worldTopRight = mainCamera.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, mainCamera.transform.position.z));

        Vector3 spawnPosition;

        // Randomly choose a side to spawn from
        if (Random.value < 0.5f) // 50% chance to spawn on left/right side
        {
            // Decide whether to spawn on the left or right
            float xPos = Random.value < 0.5f ? worldBottomLeft.x - spawnRadius : worldTopRight.x + spawnRadius;
            float yPos = Random.Range(worldBottomLeft.y, worldTopRight.y); 
            spawnPosition = new Vector3(xPos, yPos, 0);
        }
        else // Spawn from top/bottom
        {
            float yPos = Random.value < 0.5f ? worldBottomLeft.y - spawnRadius : worldTopRight.y + spawnRadius;
            float xPos = Random.Range(worldBottomLeft.x, worldTopRight.x); 
            spawnPosition = new Vector3(xPos, yPos, 0);
        }

        return spawnPosition; // Return the spawn position
    }
}
