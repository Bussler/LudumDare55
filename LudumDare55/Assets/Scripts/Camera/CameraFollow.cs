using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target = null;
    [SerializeField]
    private Vector3 offset = new Vector3(0, 15, 0); // Camera offset from the player
    [SerializeField]
    private float damping = 0.3f; // Damping of the camera movement

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, damping);
            transform.position = smoothedPosition;
        }
    }
}
