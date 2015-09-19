using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Goal : MonoBehaviour {

    public Paddle left;
    public delegate void OnCount();
    public OnCount onCount;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnCollisionEnter(Collision collision)
    {
        left.Attach(collision.gameObject.GetComponent<BallController>());
        onCount();
    }
}
