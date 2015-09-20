﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnvironmentController : MonoBehaviour {
    public GameObject background;
    public float rotateionCounter = 0;
    public GameObject crowd;
    public AudioClip cheering;
    public Goal goalLeft;
    public Goal goalRight;
    public Button startRestart;

    public PlayerController player1;
    public PlayerController player2;

    public ParticleSpawner spawner;

    public Canvas Menu;
    GameObject[] catBalls;

    public void StartGame() {
       GameObject[] catBalls = GameObject.FindGameObjectsWithTag("CatBall");
        foreach (var item in catBalls) {
            item.GetComponent<BallController>().Reset();            
        }
        player1.Reset();
        player2.Reset();
        
        //player1.count = 0;
        //player2.count = 0;
        Menu.enabled = false;

    }

    public void ExitGame() {
        Application.Quit();
    }

    // Use this for initialization
    void Start () {
        goalLeft.onCount += CallFromGoal;
        goalRight.onCount += CallFromGoal;
    }

    void CallFromGoal() {
        if (player1.count >= 10 || player2.count >= 10 && Menu.enabled == false) {
            UpDown(true);
            Menu.enabled = true;
        }


    }
	
	// Update is called once per frame
	void Update () {
        background.transform.rotation = Quaternion.Euler(new Vector3(background.transform.rotation.x, background.transform.rotation.y, background.transform.rotation.z + rotateionCounter));
        rotateionCounter += 0.1f;


    }

    void UpDown(bool up) {
        spawner.DoStop();
        if (up) {
            iTween.MoveBy(crowd, new Vector3(0f, 3f, 0), 1);
            gameObject.GetComponent<AudioSource>().clip = cheering;
            gameObject.GetComponent<AudioSource>().Play();
        }
        if (!up) {
            iTween.MoveBy(crowd, new Vector3(0f, -3f, 0), 1);
            gameObject.GetComponent<AudioSource>().Stop();
        }


    }
}
