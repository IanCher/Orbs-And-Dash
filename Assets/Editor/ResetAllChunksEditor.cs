using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ResetAllChunks))]
public class ResetAllChunksEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector for the CubeCreator script
        DrawDefaultInspector();

        // Get a reference to the CubeCreator script we are editing
        ResetAllChunks resetAllChunks = (ResetAllChunks)target;

        // Add a button to the inspector
        if (GUILayout.Button("Reset All Chunk"))
        {
            // Call the GenerateCube method when the button is pressed
            resetAllChunks.ResetAll();
        }
    }
}
