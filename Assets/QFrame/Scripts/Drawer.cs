using UnityEngine;
using System.Collections;
using QPhysics;

public class Drawer : MonoBehaviour {

	private QFrame frame;
	private MeshFilter potentialMesh;
	private MeshFilter waveMesh;

    [Range(0, 0.5f)]
	public float lineWidth = 0.01f;
    [Range(0, 0.1f)]
    public float scaleWave = 0.05f;

    public int resolution = 128;
    
	void Start() {
		frame = transform.GetComponent<QFrame>();
		potentialMesh = (transform.FindChild ("Potential") as Transform).GetComponent<MeshFilter>();
		waveMesh = (transform.FindChild ("Wave") as Transform).GetComponent<MeshFilter>();
		Draw();
		frame.onFrameUpdate += Draw;
		frame.onFrameReset += Draw;
	}
	
	public void Draw() {
		// Get the evaluated x-values
		double[] xValues = frame.getXAxisPhysics();
		double[] wave = frame.getWaveFunctionPhysics();

        // Interpolate
        double[] newXValues = Generate.LinearSpaced(resolution, xValues[0], xValues[frame.getNSpatialPts() - 1]);
        double[] newWave = new double[resolution];
        alglib.spline1dconvcubic(xValues, wave, newXValues, out newWave);
        xValues = newXValues;
        wave = newWave;

        double[] potentialValuesTop = frame.getPotentialPhysics(newXValues);

        float[] unityXValuesTop = frame.localAxis.physicsXToUnity(xValues);
        float[] unityPotentialValuesTop = frame.localAxis.physicsYToUnity(potentialValuesTop);

        float[] unityXValuesBottom = new float[resolution];
        float[] unityPotentialValuesBottom = new float[resolution];

        CalculatePotentialBottom(unityXValuesTop, unityPotentialValuesTop, unityXValuesBottom, unityPotentialValuesBottom);

        float[] unityWave = new float[resolution];

		
		// Get the potential for the above values
		for (int i=0; i<resolution; i++) {
            if (unityPotentialValuesTop[i] > 1.0f) unityPotentialValuesTop[i] = 1.0f;
            if (unityPotentialValuesBottom[i] > 1.0f) unityPotentialValuesBottom[i] = 1.0f;
            unityWave[i] = frame.localAxis.physicsYToUnity(wave[i] * frame.localAxis.unityHToPhysics(scaleWave) + potentialValuesTop[i]);
            if (unityWave[i] > 1.0f) unityWave[i] = 1.0f;
		}
        
		
		// Renew the meshes
        if (potentialMesh.sharedMesh == null || potentialMesh.sharedMesh.vertexCount != 4 * 2 * resolution + 8) potentialMesh.sharedMesh = MeshBuilder.fillLineMesh(unityXValuesTop, unityPotentialValuesTop, unityXValuesBottom, unityPotentialValuesBottom, 1);
        else MeshBuilder.RecomputeLineMeshVertices(unityXValuesTop, unityPotentialValuesTop, unityXValuesBottom, unityPotentialValuesBottom, 1, potentialMesh.sharedMesh);
        if (waveMesh.sharedMesh == null || waveMesh.sharedMesh.vertexCount != 4 * 2 * resolution + 8) waveMesh.sharedMesh = MeshBuilder.fillLineMesh(unityXValuesTop, unityWave, unityXValuesBottom, unityPotentialValuesTop, 1);
        else MeshBuilder.RecomputeLineMeshVertices(unityXValuesTop, unityWave, unityXValuesBottom, unityPotentialValuesTop, 1, waveMesh.sharedMesh);
	}

    private void CalculatePotentialBottom(float[] unityXValuesTop, float[] unityPotentialValuesTop, float[] unityXValuesBottom, float[] unityPotentialValuesBottom)
    {
        int l = unityXValuesTop.Length;

        unityPotentialValuesBottom[0] = unityPotentialValuesTop[0] - lineWidth;
        unityXValuesBottom[0] = unityXValuesTop[0];
        for (int i = 1; i < l - 1; i++)
        {
            Vector2 P_before = new Vector2(unityXValuesTop[i-1], unityPotentialValuesTop[i-1]);
            Vector2 P_mid = new Vector2(unityXValuesTop[i], unityPotentialValuesTop[i]);
            Vector2 P_after = new Vector2(unityXValuesTop[i + 1], unityPotentialValuesTop[i + 1]);

            Vector2 P_n = CalculateBottomState(P_before, P_mid, P_after);

            unityPotentialValuesBottom[i] = P_n.y;  
            unityXValuesBottom[i] = P_n.x;
        }

        unityPotentialValuesBottom[l-1] = unityPotentialValuesTop[l-1] - lineWidth;
        unityXValuesBottom[l-1] = unityXValuesTop[l-1];
    }

    private Vector2 CalculateBottomState(Vector2 P_before, Vector2 P_mid, Vector2 P_after)
    {
        Vector2 P_diff = (P_after - P_before);
        Vector2 P_hat = new Vector2(-P_diff.y, P_diff.x);
        P_hat.Normalize();
        return P_mid - P_hat * lineWidth;
    }

    /// <summary>
    /// To be used by the editor
    /// </summary>
    public void refresh()
    {
        frame = transform.GetComponent<QFrame>();
        potentialMesh = (transform.FindChild("Potential") as Transform).GetComponent<MeshFilter>();
        waveMesh = (transform.FindChild("Wave") as Transform).GetComponent<MeshFilter>();
        Draw();
    }

}
