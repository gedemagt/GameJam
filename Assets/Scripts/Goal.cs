using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

    int Count = 0;

	// Use this for initialization
	void Start () {
        Count = 0;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public int GetCount() { return Count; }

    void OnCollisionEnter(Collision collision)
    {
        Count++;
        collision.gameObject.GetComponent<BallManager>().Reset();
    }
}
