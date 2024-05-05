using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public float movementSpeed = 10f;
    public float rotationSpeed = 100f;

    private float pitch = 0f;
    private float yaw = 0f;

    void Start()
    {
        // Optional: Lock cursor to center of screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Forward/backward movement
        float moveForward = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        // Left/right movement
        float moveRight = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        // Apply movement
        Vector3 move = transform.right * moveRight + transform.forward * moveForward;
        transform.Translate(move, Space.World);

        // Mouse look behavior
        yaw += rotationSpeed * Input.GetAxis("Mouse X") * Time.deltaTime;
        pitch -= rotationSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -90f, 90f);  // Limit the pitch to prevent flipping

        // Apply rotation
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }
}
