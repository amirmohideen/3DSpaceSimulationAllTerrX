using UnityEngine;
using System.Collections;

public class AsteroidAmir : MonoBehaviour {
    [SerializeField] public float minScale = 0.8f;
    [SerializeField] public float maxScale = 1.2f;
    [SerializeField] public float rotationOffset = 100f;

    Transform myT;
    Vector3 randomRotation;

    void Awake() {
        myT = transform;
    }

    void Start() {
        //random size
        Vector3 scale = Vector3.one;
        scale.x = Random.Range(minScale, maxScale);
        scale.y = Random.Range(minScale, maxScale);
        scale.z = Random.Range(minScale, maxScale);
        myT.localScale = scale;

        //random rotation
        randomRotation.x = Random.Range(-rotationOffset, rotationOffset);
        randomRotation.y = Random.Range(-rotationOffset, rotationOffset);
        randomRotation.z = Random.Range(-rotationOffset, rotationOffset);

        Debug.Log(randomRotation);
    }

    void Update () {
        myT.Rotate(randomRotation * Time.deltaTime);
    }

    public void InitMovement(Vector3 direction, float speed) {
    GetComponent<Rigidbody>().velocity = direction * speed;
}

}
