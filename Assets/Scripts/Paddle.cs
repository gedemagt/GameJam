using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {

    public QFrame frame;
    private double startVar;
    private float startScaleX;
    public float lengthScaler = 1.0f;
    public float minScale;
    public bool scaleAccordingToVariance = false;

	// Use this for initialization
	void Start () {
        startVar = frame.getXVariance();
        startScaleX = transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
        int maxIndex = FindMax(frame.getWaveFunctionPhysics());
        float x = frame.localAxis.physicsXToUnity(frame.getXMean());
        transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);

        if (scaleAccordingToVariance)
        {    
            double currentVar = frame.getXVariance();
            float newXscale = (float)(startVar / currentVar) * startScaleX * lengthScaler;
            if (newXscale < minScale) newXscale = minScale;
            if (newXscale > startScaleX) newXscale = startScaleX;
            transform.localScale = new Vector3(newXscale, transform.localScale.y, transform.localScale.z);
        }

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
