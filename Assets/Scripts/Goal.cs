using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Goal : MonoBehaviour {

    int Count = 0;
    public Text score;

	// Use this for initialization
	void Start () {
        Count = 0;
	}
	
	// Update is called once per frame
	void Update () {
        score.text = Count.ToString();
	}

    public int GetCount() { return Count; }

    void OnCollisionEnter(Collision collision)
    {
        Count++;
        collision.gameObject.GetComponent<BallController>().Reset();
    }
}
