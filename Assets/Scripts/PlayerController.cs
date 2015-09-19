using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public QFrame frame;
    public Paddle paddle;
    public Goal goal;


	// Use this for initialization
	void Start () {
        frame.setLevel("id2");
        paddle.scaleAccordingToVariance = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (goal.GetCount() > 5) frame.setLevel("id1");
        if (goal.GetCount() > 8) paddle.scaleAccordingToVariance = true;
	}
}
