using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {
    Transform myTransform;
    [Range (10, 300)]
    public float speed = 50;
    void Start () {
        myTransform = transform;
    }

    void Update () {
        transform.eulerAngles += Vector3.forward * Time.deltaTime * speed;
    }
}