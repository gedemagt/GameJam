using UnityEngine;
using System.Collections;

public abstract class DraggerAbstract : MonoBehaviour {

    private QFrame frame;

    public bool lockX = false;
    public bool lockY = false;

    private bool isReset = true;

    private Vector3 screenPoint;
    private Vector3 offset;

    public float movement = 0.03f;


    // Use this for initialization
    void Start() {
        refresh();
    }

    /// <summary>
    /// To be used by the editor
    /// </summary>
    public void refresh() {

        Debug.Log("REFRESH");
        if (frame == null) frame = transform.parent.GetComponent<QFrame>();
        Vector3 newPos = frame.getStartPoint();
        newPos.z = transform.position.z;
        transform.position = newPos;
    }

    public abstract void abstractUpdate();

    void Update() {
        abstractUpdate();


    }

    public void MovingPotential(Vector3 newVector) {
        if (frame.running()) {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position += newVector;
            frame.updatePotential(transform.position.x, transform.position.y);
        }
    }

    public void OnKeyDown() {
        if (!isReset) {
            frame.reset();
            refresh();
            isReset = true;
        } else {
            frame.start();
            isReset = false;
            offset = gameObject.transform.position;
        }
    }
    //void OnMouseDown() {
    //    if (!isReset) {
    //        frame.reset();
    //        refresh();
    //        isReset = true;
    //    } else {
    //        frame.start();
    //        isReset = false;
    //        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
    //        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    //    }
    //}

    //void OnMouseUp() {
    //    frame.stop();
    //}
}
