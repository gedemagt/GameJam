using UnityEngine;
using System.Collections;

public class WallController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public float bounciness;
    private Vector3 lastVelocity;

    void OnCollisionEnter(Collision other)
    {
        Rigidbody rb = other.rigidbody;
        Vector3 lastVelocity = rb.velocity;
        Vector3 normal = Vector3.zero;

        foreach (ContactPoint c in other.contacts)
        {
            normal += c.normal;
        }

        normal.Normalize();

        Vector3 inVelocity = lastVelocity;

        Vector3 outVelocity = bounciness * (-2f * (Vector3.Dot(inVelocity, normal) * normal) + inVelocity);
        outVelocity.x = -outVelocity.x;
        Debug.Log(outVelocity.x + " " + outVelocity.y + " " + outVelocity.z, rb);

        rb.velocity = outVelocity;
    }
}
