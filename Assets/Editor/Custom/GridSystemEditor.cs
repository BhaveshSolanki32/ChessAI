using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridGenerator))]
public class GridSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridGenerator _gridSystem = (GridGenerator)target;

        if (GUILayout.Button("Draaw level"))
        {
            _gridSystem.CreateGrid();
        }
    }
}
