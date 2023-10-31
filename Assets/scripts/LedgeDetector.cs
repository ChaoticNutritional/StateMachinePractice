using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LedgeDetector : MonoBehaviour
{
    public event Action<Vector3, Vector3> OnLedgeDetected;
    private void OnTriggerEnter(Collider other)
    {
        // 1st arg: where did our hands touch ledge
        // Other = ledge
        // Transform = our ledge detector (i.e. our hands)
        // closestpoint returns a Vector3 position on the collider, closest to the vector3 passed to it
        // SIMPLE: the closest point on the ledge to our hands

        // 2nd arg: director the ledge is facing in, to make sure the player faces the right way
        // other transform forward, gets forward vector of the ledge

        OnLedgeDetected?.Invoke(other.ClosestPoint(transform.position), other.transform.forward);
        Debug.Log("detected ledge");
    }
}
