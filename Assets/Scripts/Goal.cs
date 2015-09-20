using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Goal : MonoBehaviour {

    public Paddle left;
    public delegate void OnCount();
    public OnCount onCount;
    public ParticleSpawner spawner;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnCollisionEnter(Collision collision)
    {
        spawner.HitGoal(collision.gameObject, left);
		Debug.Log (collision.gameObject.GetComponent<BallController> ().hasHit);
        if(collision.gameObject.GetComponent<BallController>().hasHit) onCount();
    }
}
