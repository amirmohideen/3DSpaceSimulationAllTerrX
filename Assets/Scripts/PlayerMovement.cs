using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 100;
    void Update()
    {
        transform.position += transform.forward * movementSpeed *Time.deltaTime;
    }
}
