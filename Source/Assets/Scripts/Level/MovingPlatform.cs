using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3[] positions;
    public int positionNumber = 0;
    public float speed;
    public float timeout;

    private Vector3 currentTarget;
    private float timeoutTimer;

    private void Start()
    {
        if (positions.Length > 0)
        {
            currentTarget = positions[1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != currentTarget)
        {
            MovePlatform();
        }
        else
        {
            UpdateTarget();
        }
    }

    private void MovePlatform()
    {
        // Move platform
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentTarget) < 0.001f)
        {
            // Set next target and start timeout
            transform.position = currentTarget;
            timeoutTimer = timeout;
        }
    }

    private void UpdateTarget()
    {
        if (timeoutTimer >= 0.0f)
        {
            timeoutTimer -= Time.deltaTime;
        }
        else
        {
            NextPlatform();
        }
    }

    private void NextPlatform()
    {
        positionNumber++;
        if (positionNumber >= positions.Length)
        {
            positionNumber = 0;
        }

        currentTarget = positions[positionNumber];
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }
}
