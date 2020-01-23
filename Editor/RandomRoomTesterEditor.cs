using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomRoomTester))]
public class RandomRoomTesterEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        //base.OnInspectorGUI();

        //referencing the RandomRoodEditor object - script
        var script = (RandomRoomTester)target;
        if(GUILayout.Button("Generate Island"))
        {
            if (Application.isPlaying)
            {
                script.MakeMap();
            }
        }
    }
}
