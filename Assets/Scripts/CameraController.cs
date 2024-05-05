using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Assign your spaceship as the target
    public Vector3 offset = new Vector3(0f, 2f, -5f); // Offset from the target object
    public float sensitivity = 2f; // Mouse sensitivity

    void LateUpdate()
    {
        // Follow the target
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.LookAt(target);
        }

        // Mouse look behavior
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        transform.RotateAround(target.position, Vector3.up, mouseX);
        transform.RotateAround(target.position, transform.right, -mouseY);
    }
}
