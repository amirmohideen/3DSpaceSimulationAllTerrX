using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedMovement : MonoBehaviour
{
    public float minSpeed = 1f;
    public float maxSpeed = 3f;
    public float boundaryRadius = 20f; // Radius of the boundary sphere
    public float minScale = 0.5f; // Minimum scale for each axis
    public float maxScale = 1.2f; // Maximum scale for each axis

    private Vector3 direction;
    private float speed;

    void Start()
    {
        // Generate random scales for each axis
        float scaleX = Random.Range(minScale, maxScale);
        float scaleY = Random.Range(minScale, maxScale);
        float scaleZ = Random.Range(minScale, maxScale);

        // Apply the random scales to the obstacle's transform
        transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

        // Generate a random direction
        direction = Random.insideUnitSphere.normalized;

        // Generate a random speed for the obstacle
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        // Move the obstacle based on the direction and speed
        transform.position += direction * speed * Time.deltaTime;

        // Check if the obstacle is outside the boundary
        if (transform.position.magnitude > boundaryRadius)
        {
            // Move the obstacle back inside the boundary
            transform.position = transform.position.normalized * boundaryRadius;

            // Generate a new random direction
            direction = Random.insideUnitSphere.normalized;

            // Generate a new random speed
            speed = Random.Range(minSpeed, maxSpeed);
        }
    }
}
