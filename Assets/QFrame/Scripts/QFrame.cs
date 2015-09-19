using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using QPhysics;

[ExecuteInEditMode]
public class QFrame : MonoBehaviour {

    // Used to determine whether certain calculations are made in the local of world unity coordinates
    public static readonly int WORLD = 0;
    public static readonly int LOCAL = 1;

	//Delegates
	public delegate void OnStartPropagating();
	public delegate void OnStopPropagating();
	public delegate void OnFrameReset();
	public delegate void OnFrameUpdate();
	public OnStartPropagating onStartPropagating;
	public OnStopPropagating onStopPropagating;
	public OnFrameReset onFrameReset;
	public OnFrameUpdate onFrameUpdate;

    private Level level;
    public string levelString;
	public Axis worldAxis;
    public Axis localAxis;

	private WaveFunction currentWave;
    private PositionOperator posOp = new PositionOperator();
    private HamiltonOperator hamOp = new HamiltonOperator();

    private bool isRunning = false;

	private Point nextPoint;
    private Point lastPoint;

	private double _fidelity;

    public bool fixedTime = false;
	public double targetDt = 0.002;
    public bool startOnPlay = false;
    private float time = 0.0f;

    /*********************************
     *                               *
     * Private and protected methods *
     *                               *
     *********************************/

    void OnEnable()
    {
        setLevel(levelString);
    }

	void Start() {
        if (startOnPlay) start();
	}

    void Update()
    {
        if (isRunning) {
			updateSystem();
			calculateFidelity();
			if(onFrameUpdate != null) onFrameUpdate();
		}
    }

    /// <summary>
    /// Updates the potential to the curent set next position and propagates the wavefunction.
    /// dt=0.002 is a hard cap due to the underlying calculations.
    /// </summary>
    private void updateSystem()
    {
        // We try to update the system by 0.002 for every 25 ms. We
        // cannot exceed 0.002 in each frame, however, due to the 
        // underlying calculations.
        double dt = targetDt;
		if(!fixedTime) dt *= Time.deltaTime / 0.025;

        if (dt > 0.002) dt = 0.002;
        nextPoint.dTime = dt;
        level.updatePotentialAndPropagateWave(currentWave, nextPoint);
        time += Time.deltaTime;
        lastPoint = nextPoint;
    }

    private Axis getAxis(int axis)
    {
        if (axis == LOCAL) return localAxis;
        else return worldAxis;
    }

    /**************************
     *                        *
     *         Mutators       * 
     *                        *
     * ************************/
    
    /// <summary>
    /// Set the level currently maintained in the QFrame. Must be specified by a string corresponding to
    /// an already specified level.
    /// </summary>
    /// <param name="str">The identifier of the wanted level (the ID in the JSON)</param>
    public void setLevel(string str)
    {
        setLevel(LevelFactory.createLevel(str));
        levelString = str;
    }

    /// <summary>
    /// Set the level currently maintained in the QFrame. 
    /// </summary>
    /// <param name="str">The level</param>
    public void setLevel(Level level)
    {
        levelString = level.id;
        this.level = level;
        worldAxis = new Axis(level.xMinVal, level.xMaxVal, level.ampMinVal, level.ampMaxVal,
        transform.position.x,
        transform.position.x + transform.localScale.x,
        transform.position.y,
        transform.position.y + transform.localScale.y);

        localAxis = new Axis(level.xMinVal, level.xMaxVal, level.ampMinVal, level.ampMaxVal, 0.0, 1.0, 0.0, 1.0);

        currentWave = level.getInitialWaveFunction();
        nextPoint = level.getStartPoint();
        lastPoint = nextPoint;

        transform.GetComponent<Drawer>().refresh();
        transform.FindChild("Target").GetComponent<Target>().refresh();
    }

    /// <summary>
    /// Sets the extremum of the potential to be updated to this point in the next frame. This is 
    /// specified in unity coordinates, either local or world.
    /// </summary>
    /// <param name="x0">The new x-position</param>
    /// <param name="amp">The new amplitude</param>
    /// <param name="axis">Specifies whether the inputted data is in local or world unity coordinates. World is the default.</param>
    public void updatePotential(float x0, float amp, int axis = 0)
    {
        Axis a = getAxis(axis);
        nextPoint = new Point(a.unityXToPhysics(x0), a.unityYToPhysics(amp));
    }

    /// <summary>
    /// Translates the extremum of the potential by (dx0,damp) in the next frame. This is 
    /// specified in unity coordinates, either local or world.
    /// </summary>
    /// <param name="dx0">The displacement on the x-axis</param>
    /// <param name="amp">The displacement on the amplitude-axis</param>
    /// <param name="axis">Specifies whether the inputted data is in local or world unity coordinates. World is the default.</param>
    public void translatePotential(float dx0, float damp, int axis = 0)
    {
        Axis a = getAxis(axis);
        nextPoint = new Point(lastPoint.x + a.unityWToPhysics(dx0), lastPoint.amp + a.unityHToPhysics(damp));
    }

    /// <summary>
    /// Starts the propagation of the system, i.e. the potential and
    /// wavefunction is updated in each frame.
    /// </summary>
    public void start()
    {
        isRunning = true;
		if(onStartPropagating != null) onStartPropagating();
    }

    /// <summary>
    /// Stops the propagation
    /// </summary>
    public void stop()
    {
        isRunning = false;
		if(onStopPropagating != null) onStopPropagating();
    }

    /// <summary>
    /// Resets the internal system.
    /// </summary>
    public void reset()
    {
        currentWave = level.getInitialWaveFunction();
        level.reset();
		nextPoint = level.getStartPoint();
		lastPoint = nextPoint;
        time = 0.0f;
		calculateFidelity();
		if(onFrameReset != null) onFrameReset();
    }

    /**************************
     *                        *
     *         Getters        * 
     *                        *
     * ************************/

    /// <summary>
    /// Calculates the fidelity of the current wavefunction with the target wavefunction 
    /// specified by the level.
    /// </summary>
    /// <returns>The current fidelity</returns>
    public double fidelity()
    {
		return _fidelity;
    }

	/// <summary>
	/// Formally calculate fidelity, returning the value. Should max be called once, which is done in Update if running.
	/// </summary>
	/// <returns>The fidelity.</returns>
	private double calculateFidelity(){
		_fidelity = currentWave.getFidelity(level.getTargetWaveFunction());
		return _fidelity;
	}

    /// <summary>
    /// Whether the propagations is running or not.
    /// </summary>
    /// <returns></returns>
    public bool running()
    {
        return isRunning;
    }

    /// <summary>
    /// The number of grid points used in the discretization in the calculations.
    /// This specifies the length of the potential, wavefunction and x-axis arrays.
    /// </summary>
    /// <returns></returns>
    public int getNSpatialPts()
    {
        return level.getHamiltonian().nSpatialPts;
    }

    /// <summary>
    /// The real time of propagation since last restart.
    /// </summary>
    /// <returns></returns>
    public float getRealTime()
    {
        return time;
    }

    /// <summary>
    /// The physics time of propagation since last restart.
    /// </summary>
    /// <returns></returns>
    public double getPhysicsTime()
    {
        return level.getHamiltonian().getAbsoluteTime();
    }

    /// <summary>
    /// The initial point of the level in unity coordinates.
    /// </summary>
    /// <param name="axis">Specifies whether it should be in local or world coordinates</param>
    /// <returns></returns>
    public Vector2 getStartPoint(int axis=0)
    {
        Axis a = getAxis(axis);
        return new Vector2(a.physicsXToUnity(level.getStartPoint().x), a.physicsYToUnity(level.getStartPoint().amp));
    }

    /// <summary>
    /// The current point of the level in unity coordinates.
    /// </summary>
    /// <param name="axis">Specifies whether it should be in local or world coordinates</param>
    /// <returns></returns>
    public Vector2 getCurrentPoint(int axis = 0)
    {
        Axis a = getAxis(axis);
        return new Vector2(a.physicsXToUnity(lastPoint.x), a.physicsYToUnity(lastPoint.amp));
    }

	/// <summary>
	/// The current point of the level in physics coordinates.
	/// </summary>
	public Point getCurrentPointPhysics(){
		return lastPoint;
	}

    /// <summary>
    /// The initial point of the level in physics coordinates.
    /// </summary>
    /// <returns></returns>
    public Point getStartPointPhysics() 
    {
        return level.getStartPoint();
    }

    /// <summary>
    /// A discretization of the potential in unity coordinates
    /// </summary>
    /// <param name="axis">Specifies whether it should be in local or world coordinates</param>
    /// <returns></returns>
    public float[] getPotential(int axis=0)
    {
        Axis a = getAxis(axis);
        return a.physicsYToUnity(level.getHamiltonian().getDiscretePotential().ToArray());
    }

    /// <summary>
    /// A discretization of the potential in physics coordinates
    /// </summary>
    /// <returns></returns>
	public double[] getPotentialPhysics() {
        return level.getHamiltonian().getDiscretePotential().ToArray();
	}

    /// <summary>
    /// A discretization of the potential in physics coordinates on another x axis
    /// </summary>
    /// <returns></returns>
    public double[] getPotentialPhysics(double[] x)
    {
        return level.getHamiltonian().getDiscretePotential(new Vector(x)).ToArray();
    }

    /// <summary>
    /// A discretization of the wavefunction in unity coordinates (the absolute-square).
    /// </summary>
    /// <param name="axis">Specifies whether it should be in local or world coordinates</param>
    /// <returns></returns>
    public float[] getWaveFunction(int axis=0)
    {
        Axis a = getAxis(axis);
        return a.physicsYToUnity(currentWave.absSquare().ToArray());
    }

    /// <summary>
    /// A discretization of the wavefunction in physics coordinates(the absolute-square).
    /// </summary>
    /// <returns></returns>
	public double[] getWaveFunctionPhysics() {
        return currentWave.absSquare().ToArray();
	}

    /// <summary>
    /// The discrete x-axis on which the calculations are made in unity coordinates
    /// </summary>
    /// <param name="axis">Specifies whether it should be in local or world coordinates</param>
    /// <returns></returns>
    public float[] getXAxis(int axis=0)
    {
        Axis a = getAxis(axis);
        return a.physicsXToUnity(level.getHamiltonian().getXArray().ToArray());
    }

    /// <summary>
    /// The discrete x-axis on which the calculations are made in physics coordinates
    /// </summary>
    /// <returns></returns>
	public double[] getXAxisPhysics() {
        return level.getHamiltonian().getXArray().ToArray();
	}

    /// <summary>
    /// The maximally allowed step in the horizontal direction per update in
    /// physics coordinates
    /// </summary>
    /// <returns></returns>
    public double dxdtPhysics()
    {
        return level.dxdt;
    }

    /// <summary>
    /// The maximally allowed step in the horizontal direction per update in
    /// Unity coordinates
    /// </summary>
    /// <param name="axis">Specifies whether it should be in local or world coordinates</param>
    /// <returns></returns>
    public float dxdt(int axis = 0)
    {
        return getAxis(axis).physicsWToUnity(level.dxdt);
    }

    /// <summary>
    /// The maximally allowed step in the vertical direction per update in
    /// physics coordinates
    /// </summary>
    /// <returns></returns>
    public double dampdtPhysics()
    {
        return level.dampdt;
    }

    /// <summary>
    /// The maximally allowed step in the vertical direction per update in
    /// Unity coordinates
    /// </summary>
    /// <param name="axis">Specifies whether it should be in local or world coordinates</param>
    /// <returns></returns>
    public float dampdt(int axis = 0)
    {
        return getAxis(axis).physicsHToUnity(level.dampdt);
    }

    /// <summary>
    /// The dimension of the QFrame in unity coordinates
    /// </summary>
    /// <param name="axis">Specifies whether it should be in local or world coordinates</param>
    /// <returns></returns>
    public Rect getDimension(int axis=0)
    {
        Axis a = getAxis(axis);
        return new Rect(a.physicsXToUnity(level.xMinVal), a.physicsYToUnity(level.ampMaxVal), a.physicsWToUnity(level.xMaxVal - level.xMinVal), a.physicsHToUnity(level.ampMaxVal - level.ampMinVal));
    }

    /// <summary>
    /// The physical minimal x value
    /// </summary>
    /// <returns></returns>
    public double xMin()
    {
        return level.xMinVal;
    }

    /// <summary>
    /// The physical maximal x value
    /// </summary>
    /// <returns></returns>
    public double xMax()
    {
        return level.xMaxVal;
    }

    /// <summary>
    /// The physical minimum amplitude
    /// </summary>
    /// <returns></returns>
    public double yMin()
    {
        return level.ampMinVal;
    }

    /// <summary>
    /// THe physical maximum amplitude
    /// </summary>
    /// <returns></returns>
    public double yMax()
    {
        return level.ampMaxVal;
    }


    /// <summary>
    /// The target area of the level in unity coordinates
    /// </summary>
    /// <param name="axis">Specifies whether it should be in local or world coordinates</param>
    /// <returns></returns>
    public Rect getTargetArea(int axis=0)
    {
        Axis a = getAxis(axis);
        return new Rect(a.physicsXToUnity(level.targetArea.x),
                a.physicsYToUnity(level.targetArea.y),
                a.physicsWToUnity(level.targetArea.width),
                a.physicsHToUnity(level.targetArea.height));
    }

    /// <summary>
    /// The target area in physics coordinates
    /// </summary>
    /// <returns></returns>
    public TargetArea getTargetAreaPhysics()
    {
        return level.targetArea;
    }

    /// <summary>
    /// Return the expectated value of the position, i.e. the mean position of the wave
    /// </summary>
    /// <returns></returns>
    public double getXMean()
    {
        return posOp.getExpectationValue(currentWave);
    }

    /// <summary>
    /// Return the expectated value of the Hamilton, i.e. the mean energy of the wave
    /// </summary>
    /// <returns></returns>
    public double getEnergyMean()
    {
        return hamOp.getExpectationValue(currentWave);
	}

	void OnDrawGizmos() {
		Vector3 pos = transform.localPosition;
		
		Vector3 topLeft  = new Vector3 (pos.x,
		                                pos.y + transform.localScale.y,
		                                pos.z);
		Vector3 topRight = new Vector3 (pos.x + transform.localScale.x,
		                                pos.y + transform.localScale.y,
		                                pos.z);
		Vector3 botRight = new Vector3 (pos.x + transform.localScale.x,
		                                pos.y,
		                                pos.z);
		Vector3 botLeft  = new Vector3 (pos.x,
		                                pos.y,
		                                pos.z);
		
		Gizmos.DrawLine (topLeft, topRight);
		Gizmos.DrawLine (topRight, botRight);
		Gizmos.DrawLine (botRight, botLeft);
		Gizmos.DrawLine (botLeft, topLeft);
	}

}
