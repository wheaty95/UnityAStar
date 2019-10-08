using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(GridCreator))]
public class GridCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();

        //if (GUILayout.Button("Create Grid"))
        //{
        //    GridCreator grid = (GridCreator)target;
        //    grid.CreateGrid();
        //}

        serializedObject.ApplyModifiedProperties();
    }
}
