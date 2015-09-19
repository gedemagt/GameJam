using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Drawer))]
public class DrawerEditor : Editor
{


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Drawer drawer = (Drawer) target;
        QFrame frame = drawer.transform.GetComponent<QFrame>();

        if (GUI.changed)
        {
            frame.setLevel(frame.levelString);
            drawer.refresh();
        }
    }

}
