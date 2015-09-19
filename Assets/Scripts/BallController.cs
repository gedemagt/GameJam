using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {
	Rigidbody rb;
    private Vector3 StartPos;
    public Vector3 StartVelocity;

	// Use this for initialization
	void Start () {
		bounciness = Mathf.Clamp(bounciness, 0f, 1f);
		rb = GetComponent<Rigidbody>();
        StartPos = transform.position;
        Reset();
	}

    public void Reset()
    {
        transform.position = StartPos;
        transform.GetComponent<Rigidbody>().velocity = StartVelocity;
    }
	
	// Update is called once per frame
	void Update () {

	}

	public float bounciness;
	private Vector3 lastVelocity;
	
	void FixedUpdate()
	{
		lastVelocity = rb.velocity;
	}
	
	void OnCollisionEnter(Collision collision)
	{
		Vector3 normal = Vector3.zero;
		
		foreach(ContactPoint c in collision.contacts)
		{
			normal += c.normal;
		}
		
		normal.Normalize();
		
		Vector3 inVelocity = lastVelocity;
		
		Vector3 outVelocity = bounciness * ( -2f * (Vector3.Dot(inVelocity,normal) * normal) + inVelocity );
        Paddle paddle = collision.transform.GetComponent<Paddle>();
        if (paddle != null)
        {
            outVelocity += Vector3.left * paddle.GetVelocity();
        }
		rb.velocity = outVelocity;
	}

}
