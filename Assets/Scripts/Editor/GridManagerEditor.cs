using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    public override void OnInspectorGUI() 
    {
        DrawDefaultInspector();

        // Force gameplay
        GridManager gridManager = (GridManager)target;
    }
}
