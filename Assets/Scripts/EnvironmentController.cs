using UnityEngine;
using System.Collections;

public class EnvironmentController : MonoBehaviour {
    public GameObject background;
    public float rotateionCounter = 0;
    public GameObject crowd;
    public AudioClip cheering;
    public Goal goalLeft;
    public Goal goalRight;

    public PlayerController player1;
    public PlayerController player2;

    // Use this for initialization
    void Start () {
        goalLeft.onCount += CallFromGoal;
        goalRight.onCount += CallFromGoal;
    }

    void CallFromGoal() {
        if (player1.count >= 10) UpDown(true);
        if (player2.count >= 10) UpDown(false);
    }
	
	// Update is called once per frame
	void Update () {
        background.transform.rotation = Quaternion.Euler(new Vector3(background.transform.rotation.x, background.transform.rotation.y, background.transform.rotation.z + rotateionCounter));
        rotateionCounter += 0.1f;


    }

    void UpDown(bool up) {
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
