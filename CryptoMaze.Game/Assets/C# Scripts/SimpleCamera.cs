using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCamera : MonoBehaviour
{
    public Transform target;
    public float speed = 4f;

    private Vector3 _position;

    void Start()
    {
        _position = target.TransformPoint(transform.position);
    }

    void Update()
    {
        var oldRotation = target.rotation;
        target.rotation = Quaternion.Euler(oldRotation.eulerAngles. x, 0, oldRotation.eulerAngles. z);
        var currentPosition = target.TransformPoint(_position);
        transform.position = Vector3.Lerp(transform.position, currentPosition, speed * Time.deltaTime);
       // var currentRotation = Quaternion.LookRotation(target.position - transform.position);
       // transform.rotation = Quaternion.Lerp(transform.rotation, currentRotation, speed * Time.deltaTime);
    }
}
