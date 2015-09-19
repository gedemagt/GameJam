using UnityEngine;
using System.Collections;

public class KeyDragger : MonoBehaviour {

    private QFrame frame;

    private bool isReset = true;

    private Vector3 screenPoint;
    private Vector3 offset;

    public float movement = 0.03f;

    public KeyCode left;
    public KeyCode right;
    public KeyCode reset;

    // Use this for initialization
    void Start()
    {
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

    void Update()
    {
        if (Input.GetKey(reset))
        {
            OnKeyDown();
        }
        if (Input.GetKey(right))
        {
            MovingPotential(new Vector3(movement, 0, 0));
        }
        if (Input.GetKey(left))
        {
            MovingPotential(new Vector3(-movement, 0, 0));
        }
    }

    public void MovingPotential(Vector3 newVector)
    {
        if (frame.running())
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position += newVector;
            frame.updatePotential(transform.position.x, transform.position.y);
        }
    }

    public void OnKeyDown()
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
            offset = gameObject.transform.position;
        }
    }
}
