using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab; // Assign this in the inspector with your asteroid prefab
    public int minAsteroids = 5;
    public int maxAsteroids = 15;

    void Start()
    {
        int numberOfAsteroids = Random.Range(minAsteroids, maxAsteroids + 1);
        for (int i = 0; i < numberOfAsteroids; i++)
        {
            InstantiateAsteroid();
        }
    }

    void InstantiateAsteroid()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        GameObject newAsteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);

        // Set random size for the asteroid
        float randomScale = Random.Range(1f, 5f); // Scales the asteroid between normal and 5 times the size
        newAsteroid.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }
}
