using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
public class SplineCreator : MonoBehaviour
{
    [SerializeField] TextAsset splineDataJSON;

    [Serializable]
    public class BlendersSpline
    {
        public BlenderKnot[] knots;
    }

    [Serializable]
    public class BlenderKnot
    {
        public float3 position;
        public float3 tangentIn;
        public float3 tangentOut;
    }

    public void CreateSpline()
    {
        string jsonString = splineDataJSON.text;
        BlendersSpline blenderSpline = JsonUtility.FromJson<BlendersSpline>(jsonString);
        BlenderKnot[] blenderKnots = blenderSpline.knots;

        SplineContainer splineContainer = GetComponent<SplineContainer>();
        for (int i = splineContainer.Spline.Knots.Count() - 1; i >= 0; i--)
        {
            splineContainer.Spline.RemoveAt(i);
        }

        foreach (BlenderKnot knot in blenderKnots)
        {
            splineContainer.Spline.Add(
                new BezierKnot(
                    knot.position,
                    knot.tangentIn,
                    knot.tangentOut
                )
            );
        }

        splineContainer.Spline.SetTangentMode(TangentMode.Continuous);
    }
}
