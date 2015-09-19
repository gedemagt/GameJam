using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BallController : MonoBehaviour {
	Rigidbody rb;
    public Vector3 StartVelocity;
    public Paddle startPaddle;
    public Sprite[] catBall;
    public Image imageSprite;
    private int catAniCount = 0;
    public bool isAttached = false;
	// Use this for initialization
	void Start () {
        startPaddle.Attach(this);
		bounciness = Mathf.Clamp(bounciness, 0f, 1f);
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        AnimateCat();
    }

    void AnimateCat() {
        imageSprite.sprite = catBall[catAniCount];
        catAniCount++;
        if (catAniCount >= 12) catAniCount = 0;
    }

	public float bounciness;
	private Vector3 lastVelocity;
	
	void FixedUpdate()
	{
		lastVelocity = rb.velocity;
	}

    public Transform paddleTransform;

    void OnCollisionEnter(Collision collision)
	{
        if(collision.transform.tag != null) {
            switch (collision.transform.tag) {
                case "RightQFrame":
                    transform.GetComponent<TrailRenderer>().material.SetColor("_TintColor", Color.red);
                    break;
                case "LeftQFrame":
                    transform.GetComponent<TrailRenderer>().material.SetColor("_TintColor", Color.green);
                    break;
                default:
                    break;
            }

        }

        Vector3 normal = Vector3.zero;
        Paddle paddle = collision.transform.GetComponent<Paddle>();
        if (paddle != null)
        {
            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 paddlePos = paddle.transform.position;
            paddlePos.x = (float)(paddlePos.x + Mathf.Sign(paddle.transform.position.x) * 0.4);
            Vector3 relPos = contactPoint - paddlePos;
            relPos.z = 0;
            Vector3 relPosNorm = relPos / relPos.magnitude;
            rb.velocity = rb.velocity.magnitude * relPosNorm;
        }

        foreach (ContactPoint c in collision.contacts)
        {
            normal += c.normal;
        }

        normal.Normalize();

        Vector3 inVelocity = lastVelocity;

        Wall wall = collision.transform.GetComponent<Wall>();
        if (wall != null)
        {
            //          outVelocity += Vector3.left * paddle.GetVelocity();
            Vector3 outVelocity = bounciness * (-2f * (Vector3.Dot(inVelocity, normal) * normal) + inVelocity);
            rb.velocity = outVelocity;
            //if (Mathf.Sign(wall.transform.position.y) == 1)
            //{
            //    rb.position = rb.position + Vector3.down * ;
            //}
            //rb.position = rb.position + outVelocity * ((float) 0.2);
            //position = newPos;

        }
        //rb.velocity = outVelocity;

		//rb.velocity = outVelocity;
        if (rb.velocity.x > 0) imageSprite.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
        else if (rb.velocity.x < 0) imageSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

}
