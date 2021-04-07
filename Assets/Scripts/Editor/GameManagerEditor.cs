using System.Collections;
using System.Collections.Generic;
using FruitBowl;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI() 
    {
        DrawDefaultInspector();

        // Force gameplay
        GameManager gameManager = (GameManager)target;
    }
}
