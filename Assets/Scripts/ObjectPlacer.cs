using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.Timeline;

public class ObjectPlacer : MonoBehaviour
{
    public SplineContainer splineContainer;
    public float splinePositionParameter = 0;
    public PathIndexUnit splinePositionUnit = PathIndexUnit.Distance;
    public float distanceToCircleCenter = 5;
    public float angle = 0;
    public bool isWall = false;
    private PathIndexUnit previousPositionUnit = PathIndexUnit.Distance;
    private float normalisedPosition = 0f;
    private Vector3 positionOnSpline;
    private Vector3 tangentAtPosition;
    private Vector3 upAtPosition;
    private Vector3 rightAtPosition;

    void OnValidate()
    {
        if (splineContainer == null)
        {
            Debug.LogWarning("Missing Track spline - cannot update position");
            return;
        }

        PlaceObject();
    }

    public void PlaceObject()
    {
        UpdateSplineUnit();
        NormalisePositionParam();
        UpdateLocalBasis();
        PlaceObjectAlongSpline();
        RotateObject();
    }

    private void RotateObject()
    {
        Vector3 newDirection;

        if (isWall)
        {
            newDirection = Quaternion.AngleAxis(angle, tangentAtPosition) * upAtPosition;
        }
        else
        {
            Vector3 orientationDir = positionOnSpline - transform.position;
            newDirection = Vector3.RotateTowards(upAtPosition, orientationDir, Mathf.PI * 2, 0);
        }

        transform.rotation = Quaternion.LookRotation(tangentAtPosition, newDirection);
    }

    private void PlaceObjectAlongSpline()
    {
        float localX = distanceToCircleCenter * Mathf.Sin(angle);
        float localY = distanceToCircleCenter * Mathf.Cos(angle);

        transform.position = (
            positionOnSpline +
            localX * rightAtPosition -
            localY * upAtPosition
        );
    }

    private void UpdateLocalBasis()
    {
        positionOnSpline = splineContainer.EvaluatePosition(normalisedPosition);  // the center of the local basis

        tangentAtPosition = splineContainer.EvaluateTangent(normalisedPosition);
        tangentAtPosition = Vector3.Normalize(tangentAtPosition);

        upAtPosition = splineContainer.EvaluateUpVector(normalisedPosition);
        upAtPosition = Vector3.Normalize(upAtPosition);

        rightAtPosition = Vector3.Cross(upAtPosition, tangentAtPosition);
    }

    private void NormalisePositionParam()
    {
        normalisedPosition = splineContainer.Splines[0].ConvertIndexUnit(
            splinePositionParameter, splinePositionUnit, PathIndexUnit.Normalized
        );
    }

    private void UpdateSplineUnit()
    {
        if (previousPositionUnit == splinePositionUnit) return;

        splinePositionParameter = splineContainer.Splines[0].ConvertIndexUnit(
            splinePositionParameter, previousPositionUnit, splinePositionUnit
        );
        previousPositionUnit = splinePositionUnit;
    }

    private void OnDrawGizmosSelected()
    {
        Color defaultColor = Gizmos.color;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, 1f * rightAtPosition);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, 1f * upAtPosition);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, 1f * tangentAtPosition);


        Vector3 orientationDir = positionOnSpline - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, orientationDir, Mathf.PI * 2, 0);
        Gizmos.color = Color.gold;
        Gizmos.DrawRay(transform.position, newDirection);

        Gizmos.color = defaultColor;
    }
}