using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using QPhysics;

[CustomEditor(typeof(QFrame))]
public class QFrameEditor : Editor {


    public override void OnInspectorGUI()
    {
        // Initialize components
        
        QFrame frame = (QFrame)target;
        Drawer drawer = frame.transform.GetComponent<Drawer>();
        Target targ = frame.transform.FindChild("Target").GetComponent<Target>();
            frame.startOnPlay = EditorGUILayout.Toggle("Start on play", frame.startOnPlay);

            frame.fixedTime = EditorGUILayout.Toggle("Use fixed time", frame.fixedTime);
            frame.targetDt = EditorGUILayout.DoubleField("Target delta time", frame.targetDt);

            int selected = EditorGUILayout.Popup("Level", LevelFactory.IndexOf(frame.levelString), LevelFactory.GetNames());
            if (selected != -1) frame.setLevel(LevelFactory.GetIDs()[selected]);

            // Show static GUI stuff
            EditorGUILayout.LabelField("# grid points:", frame.getNSpatialPts().ToString());
            EditorGUILayout.LabelField("X-axis: ", "[" + frame.xMin().ToString() + "," + frame.xMax().ToString() + "]");
            EditorGUILayout.LabelField("Y-axis: ", "[" + frame.yMin().ToString() + "," + frame.yMax().ToString() + "]");
            EditorGUILayout.LabelField("Start point: ", "x0=" + frame.getStartPointPhysics().x.ToString() + ", amp=" + frame.getStartPointPhysics().amp.ToString());
            EditorGUILayout.LabelField("dX/dt: ", frame.dxdtPhysics().ToString());
            EditorGUILayout.LabelField("dAmp/dt: ", frame.dampdtPhysics().ToString());
            if (frame.getTargetAreaPhysics() is AllTargetArea) EditorGUILayout.LabelField("Target area: ", "No target area");
            else EditorGUILayout.LabelField("Target area: ", "x=" + frame.getTargetAreaPhysics().x.ToString() + ", y=" + frame.getTargetAreaPhysics().y.ToString() + ", w=" + frame.getTargetAreaPhysics().width.ToString() + ", h=" + frame.getTargetAreaPhysics().height.ToString());

            PrefabUtility.RecordPrefabInstancePropertyModifications(target);
            PrefabUtility.RecordPrefabInstancePropertyModifications(drawer);
            PrefabUtility.RecordPrefabInstancePropertyModifications(targ);
        
    }

}
