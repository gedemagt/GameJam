using UnityEngine;
using System.Collections;

public class SpinParticle : MonoBehaviour {

    public float moment = 1.0f;
    public float sign = -1.0f;
    public Vector3 Bfield;
    private Rigidbody body;
    public delegate void OnCollision(GameObject gameObject, GameObject particle);
    public OnCollision onCollision;

	// Use this for initialization
	void Start () {
        body = transform.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        body.AddForce(Bfield * moment * sign);
	}

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.GetComponent<Goal>() != null)
        {
            gameObject.SetActive(false);
            Destroy(this);
        }
        else 
        {
            onCollision(c.gameObject, gameObject);

        }
    }
}
