using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

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
    }
}
