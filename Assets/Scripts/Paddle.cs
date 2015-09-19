﻿using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {

    public QFrame frame;
    private double startVar;
    private float startScaleX;
    public float lengthScaler = 1.0f;
    public float minScale;
    public bool scaleAccordingToVariance = false;
    private float velocity = 0.0f;
    private float lastX;
    BallController attachedBall;
    public Vector3 shootVelocity;
    public KeyCode shoot;

	// Use this for initialization
	void Start () {
        startVar = frame.getXVariance();
        startScaleX = transform.localScale.x;
        lastX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
        float x = frame.localAxis.physicsXToUnity(frame.getXMean());
        transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
        velocity = (frame.worldAxis.physicsXToUnity(frame.getXMean()) - lastX) / Time.deltaTime;

        lastX = frame.worldAxis.physicsXToUnity(frame.getXMean());
        if (scaleAccordingToVariance)
        {    
            double currentVar = frame.getXVariance();
            float newXscale = (float)(startVar / currentVar) * startScaleX * lengthScaler;
            if (newXscale < minScale) newXscale = minScale;
            if (newXscale > startScaleX) newXscale = startScaleX;
            transform.localScale = new Vector3(newXscale, transform.localScale.y, transform.localScale.z);
        }
        if (attachedBall != null)
        {
            attachedBall.transform.position = transform.position;
        }
        if (Input.GetKey(shoot)) Detatch();
    }

    public void Attach(BallController ball)
    {
        attachedBall = ball;
        ball.isAttached = true;
    }

    public void Detatch()
    {
        if (attachedBall != null)
        {
            attachedBall.GetComponent<Rigidbody>().velocity = shootVelocity + Vector3.up*velocity;
            attachedBall.isAttached = false;
        }
        attachedBall = null;
    }

    public float GetVelocity() { return velocity; }

}
