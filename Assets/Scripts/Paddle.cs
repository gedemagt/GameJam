using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {

    public QFrame frame;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        int maxIndex = FindMax(frame.getWaveFunctionPhysics());
        float x = frame.getXAxis(QFrame.LOCAL)[maxIndex];
        transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
    }

    private int FindMax(double[] input)
    {
        double max = double.NegativeInfinity;
        int index = 0;
        for (int i = 0; i < input.Length; i++)
        {
            double val = input[i];
            if (val > max)
            {
                max = val;
                index = i;
            }
        }
        return index;
    }

}
