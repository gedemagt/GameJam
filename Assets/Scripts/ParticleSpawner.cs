using UnityEngine;
using System.Collections.Generic;

public class ParticleSpawner : MonoBehaviour {

    public GameObject prefab;
    public Vector3 Bfield = new Vector3(0, 10, 0);

    int left;
    int right;

    List<GameObject> activeBalls;
    Dictionary<GameObject, float> signs = new Dictionary<GameObject, float>();

    float startTime;
    public float dt = 5;

	// Use this for initialization
	void Start () {
        activeBalls = new List<GameObject>();
	}

    public void HitGoal(GameObject ball, Paddle paddle)
    {
        if (activeBalls.Count > 2)
        {
            activeBalls.Remove(ball);
            Destroy(ball);
            ball.GetComponent<BallController>().hasHit = false;
        }
        else
        {
            paddle.Attach(ball.GetComponent<BallController>());
        }
    }

    public void InstantiateBall()
    {
        GameObject ball;

            float ySpeed = Random.Range(-3.5f, -1f);
            float sign = (float)Mathf.Sign(Random.Range(-1.0f, 1.0f));
            ball = (GameObject) Instantiate(prefab, transform.position, Quaternion.identity);
            ball.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, ySpeed, 0.0f);
            signs.Add(ball, sign);
            activeBalls.Add(ball);
        

    }
	
	// Update is called once per frame
	void Update () {
        float time = Time.time;
        if (time - startTime > dt)
        {
            InstantiateBall();
            startTime = time;
        }
        foreach (GameObject o in activeBalls)
        {
            if (!o.GetComponent<BallController>().hasHit)
            {
                o.GetComponent<Rigidbody>().AddForce(Bfield * signs[o]);
            }
            
        }
	}
}
