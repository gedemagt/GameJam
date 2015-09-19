using UnityEngine;

public class Axis {

	double xPhysMin,  xPhysMax, yPhysMin, yPhysMax, xUnityMin, xUnityMax, yUnityMin, yUnityMax;
	
	public Axis(double xPhysMin, double xPhysMax, double yPhysMin, double yPhysMax, double xUnityMin, double xUnityMax, double yUnityMin, double yUnityMax) {
		this.xPhysMin = xPhysMin;
		this.xPhysMax = xPhysMax;
		this.yPhysMin = yPhysMin;
		this.yPhysMax = yPhysMax;

		this.xUnityMin = xUnityMin;
		this.xUnityMax = xUnityMax;
		this.yUnityMin = yUnityMin;
		this.yUnityMax = yUnityMax;
	}
	
	
	private double map(double val, double rMinFrom, double rMaxFrom, double rMinTo, double rMaxTo) {
		return (val - rMinFrom) / (rMaxFrom - rMinFrom) * (rMaxTo - rMinTo) + rMinTo;
	}

    public float physicsWToUnity(double width)
    {
        return physicsXToUnity(width) - physicsXToUnity(0.0);
    }

    public float physicsHToUnity(double height)
    {
        return physicsYToUnity(height) - physicsYToUnity(0.0);
    }

    public double unityWToPhysics(float width)
    {
        return unityXToPhysics(width) - unityXToPhysics(0.0f);
    }

    public double unityHToPhysics(float height)
    {
        return unityYToPhysics(height) - unityYToPhysics(0.0f);
    }
	
	public float physicsXToUnity(double internalX) {
		return (float) map (internalX, xPhysMin, xPhysMax, xUnityMin, xUnityMax);
	}
	
	public float[] physicsXToUnity(double[] internalX) {
		float[] result = new float[internalX.Length];
		for (int i=0; i<internalX.Length; i++) {
			result[i] = physicsXToUnity(internalX[i]);
		}
		return result;
	}
	
	public float physicsYToUnity(double internalY) {
		return (float) map (internalY, yPhysMin, yPhysMax, yUnityMin, yUnityMax);
	}
	
	public float[] physicsYToUnity(double[] internalY) {
		float[] result = new float[internalY.Length];
		for (int i=0; i<internalY.Length; i++) {
			result[i] = physicsYToUnity(internalY[i]);
		}
		return result;
	}
	
	public double unityXToPhysics(float externalX) {
		return map ((double) externalX, xUnityMin, xUnityMax, xPhysMin, xPhysMax);
	}
	
	public double[] unityXToPhysics(float[] externalX) {
		double[] result = new double[externalX.Length];
		for (int i=0; i<externalX.Length; i++) {
			result[i] = unityXToPhysics(externalX[i]);
		}
		return result;
	}
	
	public double unityYToPhysics(float externalY) {
		return map ((double) externalY, yUnityMin, yUnityMax, yPhysMin, yPhysMax);
	}
	
	public double[] unityYToPhysics(float[] externalY) {
		double[] result = new double[externalY.Length];
		for (int i=0; i<externalY.Length; i++) {
			result[i] = unityYToPhysics(externalY[i]);
		}
		return result;
	}
}
