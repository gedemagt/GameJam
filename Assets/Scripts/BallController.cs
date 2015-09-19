using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {
	Rigidbody rb;

	private Vector3 init;
	// Use this for initialization
	void Start () {
		bounciness = Mathf.Clamp(bounciness, 0f, 1f);
		rb = GetComponent<Rigidbody>();
		init = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	if (Input.GetMouseButtonDown (0)) {
			transform.position = init;
			rb.AddForce(2, -10 * 10, 0);
		}
		if (Input.GetMouseButtonDown (1)) {
			transform.position = init;
			rb.AddForce(-2, 10 * 10, 0);
		}
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
		
		rb.velocity = outVelocity;
	}

}
