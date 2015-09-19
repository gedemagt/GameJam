﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public QFrame frame;
    public Paddle paddle;
    public Goal goal;
    public Text text;
    private int count = 0;
    public int level1 = 5;
    public int level2 = 8;
    public KeyCode cheater;

	// Use this for initialization
	void Start () {

        frame.setLevel("id2");
        paddle.scaleAccordingToVariance = false;
        goal.onCount += OnCount;
	}

    private void OnCount()
    {
        count++;
        text.text = count.ToString();
        if (count == level1) frame.setLevel("id1");
        if (count == level2) paddle.scaleAccordingToVariance = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(cheater))
        {
            OnCount();
        }
	}
}