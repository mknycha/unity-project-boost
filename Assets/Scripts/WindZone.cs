using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour
{
    [SerializeField] public Vector3 direction;
    [SerializeField] public float strength;

    public void ApplyForce(Rigidbody rb)
    {
        rb.AddForce(direction * strength);
    }
}
