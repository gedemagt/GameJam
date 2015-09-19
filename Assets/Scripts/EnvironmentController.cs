using UnityEngine;
using System.Collections;

public class EnvironmentController : MonoBehaviour {
    public GameObject background;
    public float rotateionCounter = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        background.transform.rotation = Quaternion.Euler(new Vector3(background.transform.rotation.x, background.transform.rotation.y, background.transform.rotation.z + rotateionCounter) * Time.deltaTime);
        rotateionCounter += 0.1f;
	}
}
