using UnityEngine;
using System.Collections;

public class BallManager : MonoBehaviour {

    private Vector3 StartPos;
    public Vector3 StartVelocity;

	// Use this for initialization
	void Start () {
        StartPos = transform.position;
        Reset();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void Reset()
    {
        transform.position = StartPos;
        transform.GetComponent<Rigidbody>().velocity = StartVelocity;
    }
}
