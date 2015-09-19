﻿using UnityEngine;
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
        if (outVelocity.x > 0) imageSprite.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
        else if (outVelocity.x < 0) imageSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

}
