using UnityEngine;
using System.Collections;

public class SetNumbers : MonoBehaviour {

    public PlayerController player1;
    public PlayerController player2;

	// Use this for initialization
	void Start () {
	    
	}

    void Erik()
    {
        player1.text.text = player1.count.ToString();
        player2.text.text = player2.count.ToString();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
