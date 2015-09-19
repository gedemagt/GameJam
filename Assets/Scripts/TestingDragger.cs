using UnityEngine;
using System.Collections;

public class TestingDragger : MonoBehaviour {

    public delegate void CallRefresh();
    public CallRefresh callRefresh;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space)) {
            callRefresh();
        }
	}

    
}
