using UnityEngine;
using System.Collections;

public class ParticleSpawner : MonoBehaviour {

    public float totalTime = 10.0f;
    public GameObject prefab;
    public Vector3 Bfield;
    float lasttime;
    float starttime;
    bool run;

    public GameObject ball;
    public GameObject leftPaddle;
    public GameObject rightPaddle;
    int left;
    int right;

	// Use this for initialization
	void Start () {
        lasttime = Time.time;
        run = false;
	}

    public void StartSternGerlachMode()
    {
        left = 0;
        right = 0;
        run = true;
        starttime = Time.time;
        ball.SetActive(false);
    }

    void RegisterScore(GameObject gameObject, GameObject particle)
    {

        if (gameObject.Equals(leftPaddle))
        {
            left++;
            particle.SetActive(false);
            DestroyObject(particle);
        }
        if (gameObject.Equals(rightPaddle))
        {
            right++;
            particle.SetActive(false);
            DestroyObject(particle);
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (run)
        {
            if (Time.time - lasttime > 0.1)
            {
                float ySpeed = Random.Range(-3.5f, 0.0f);
                float sign = (float)Mathf.Sign(Random.Range(-1.0f, 1.0f));
                GameObject particle = (GameObject)Instantiate(prefab, transform.position, Quaternion.identity);
                particle.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, ySpeed, 0.0f);
                particle.GetComponent<SpinParticle>().Bfield = Bfield;
                particle.GetComponent<SpinParticle>().sign = sign;
                particle.GetComponent<SpinParticle>().onCollision += RegisterScore;
                lasttime = Time.time;

            }
            if (Time.time - starttime >= totalTime)
            {
                run = false;
                Debug.Log("LEft: " + left);
                Debug.Log("Right: " + right);
            }
        }
        else if (Input.GetKey(KeyCode.H)) run = true;
	}
}
