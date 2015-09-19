using UnityEngine;
using System.Collections;

public class EnvironmentController : MonoBehaviour {
    public GameObject background;
    public float rotateionCounter = 0;
    public GameObject crowd;
    public AudioClip cheering;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        background.transform.rotation = Quaternion.Euler(new Vector3(background.transform.rotation.x, background.transform.rotation.y, background.transform.rotation.z + rotateionCounter));
        rotateionCounter += 0.1f;
        if (Input.GetMouseButtonDown(0)) UpDown(true);
        if (Input.GetMouseButtonDown(1)) UpDown(false);
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
