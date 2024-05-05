using UnityEngine;

public class AsteroidSpawnerAmir : MonoBehaviour {
    public GameObject asteroidPrefab; // Assign your Asteroid prefab in the Inspector
    public int minAsteroids = 5;
    public int maxAsteroids = 15;
    public float spawnRadius = 50f;

    void Start() {
        int asteroidsToSpawn = Random.Range(minAsteroids, maxAsteroids + 1);
        for (int i = 0; i < asteroidsToSpawn; i++) {
            SpawnAsteroid();
        }
    }

    void SpawnAsteroid() {
        Vector3 spawnPosition = Random.insideUnitSphere * spawnRadius + transform.position;
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

        // Set random scale
        float minScale = Random.Range(10f, 50f); // Adjust these values as needed
        float maxScale = Random.Range(10f, 50f); // Adjust these values as needed
        AsteroidAmir asteroidScript = asteroid.GetComponent<AsteroidAmir>();
        asteroidScript.minScale = minScale;
        asteroidScript.maxScale = maxScale;

        // Set random rotation speeds
        float rotationOffset = Random.Range(50f, 150f); // Adjust these values as needed
        asteroidScript.rotationOffset = rotationOffset;

        // Add random forward movement
        float moveSpeed = Random.Range(1f, 3f); // Adjust these values as needed
        Vector3 randomDirection = Random.onUnitSphere;
        asteroidScript.InitMovement(randomDirection, moveSpeed);
    }
}
