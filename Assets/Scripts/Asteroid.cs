using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private Vector3 randomDirection;
    private float rotationSpeed;

    void Start()
    {
        // Generate a random direction by creating a random normalized vector
        randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        
        // Generate a random rotation speed
        rotationSpeed = Random.Range(20f, 100f);  // You can adjust the min and max speed as needed
    }

    void Update()
    {
        // Move the sphere in the chosen random direction
        transform.Translate(randomDirection * Time.deltaTime, Space.World);

        // Rotate the sphere around its center
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
