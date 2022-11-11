using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    [SerializeField] private float force = 100f;
    [SerializeField] private float torque = 50f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 dir)
    {
        rb.AddForce(dir * force * Time.fixedDeltaTime);
        rb.AddTorque(dir * torque * Time.fixedDeltaTime);
    }
}
