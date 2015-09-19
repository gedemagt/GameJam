using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(0, 10, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
