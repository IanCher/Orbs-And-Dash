using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SplineCreator))]
public class SplineCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector for the CubeCreator script
        DrawDefaultInspector();

        // Get a reference to the CubeCreator script we are editing
        SplineCreator splineCreator = (SplineCreator)target;

        // Add a button to the inspector
        if (GUILayout.Button("Generate Spline"))
        {
            // Call the GenerateCube method when the button is pressed
            splineCreator.CreateSpline();
        }
    }
}