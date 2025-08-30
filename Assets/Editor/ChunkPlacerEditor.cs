using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ChunkPlacer))]
[CanEditMultipleObjects]
public class ChunkPlacerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector for the CubeCreator script
        DrawDefaultInspector();

        // Get a reference to the CubeCreator script we are editing
        ChunkPlacer ChunkPlacer = (ChunkPlacer)target;

        // Add a button to the inspector
        if (GUILayout.Button("Reset Chunk"))
        {
            // Call the GenerateCube method when the button is pressed
            ChunkPlacer.ResetChunk();
        }
    }
}
