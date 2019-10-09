using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(PathFindingGrid))]
public class PathFindingGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();

        PathFindingGrid pathFindingGrid = (PathFindingGrid)target;

        if (GUILayout.Button("Create Grid"))
        {
            pathFindingGrid.Create();
        }

        if (GUILayout.Button("Clear Grid"))
        {
            pathFindingGrid.Clear();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
