using UnityEngine;
using System.Collections;

public class Dragger : MonoBehaviour {

	private QFrame frame;
    public TestingDragger testingDragger;

    public bool lockX = false;
    public bool lockY = false;

	private bool isReset = true;
	
	private Vector3 screenPoint;
	private Vector3 offset;


    // Use this for initialization
    void Start () {
        //testingDragger.callRefresh += refresh;
        refresh();
	}

    /// <summary>
    /// To be used by the editor
    /// </summary>
    public void refresh()
    {
        Debug.Log("REFRESH");
        if (frame == null) frame = transform.parent.GetComponent<QFrame>();
		Vector3 newPos = frame.getStartPoint();
		newPos.z = transform.position.z;
		transform.position = newPos;
    }

	void OnMouseDrag() {
        if (frame.running())
		{
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
			transform.position = curPosition;
			frame.updatePotential(curPosition.x, curPosition.y);
        }
	}



    void OnMouseDown()
    {
        if (!isReset)
        {
            frame.reset();
			refresh();
            isReset = true;
        }
        else
        {
            frame.start();
			isReset = false;
			screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
			offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        }
    }

    void OnMouseUp()
    {
        frame.stop();
    }
}
