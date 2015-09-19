using UnityEngine;
using System.Collections;
using QPhysics;

public class Target : MonoBehaviour {

    private QFrame frame;

	// Use this for initialization
	void Start () {
        refresh();
	}
	
	// Update is called once per frame
	void Update () {
	}

    /// <summary>
    /// To be used by the editor
    /// </summary>
    public void refresh()
    {
        if (frame == null) frame = transform.parent.GetComponent<QFrame>();
        if (frame.getTargetAreaPhysics() is AllTargetArea) gameObject.SetActive(false);
        else 
        {
            gameObject.SetActive(true);
            Vector3 newPos = frame.getTargetArea().center;
            newPos.z = transform.position.z;
            transform.position = newPos;
            transform.localScale = ((Vector3) frame.getTargetArea(QFrame.LOCAL).size) + (transform.localScale.z * Vector3.forward);            
        }

    }
}
