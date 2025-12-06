using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines;

public class ChunkPlacer : MonoBehaviour
{
    [SerializeField] bool isActive = true;
    [SerializeField] SplineContainer trackSpline;
    [SerializeField] ObjectPlacer lowOrbPrefab;
    [SerializeField] ObjectPlacer highOrbPrefab;
    [SerializeField] ObjectPlacer potionBombPrefab;
    [SerializeField] float chunkPosition;
    [SerializeField] float chunkAngle;
    [SerializeField] float minDistBetweenObjects = 20f;
    [SerializeField] float maxDistBetweenObjects = 100f;
    [SerializeField] float fixedDistBetweenObjects = 50f;
    [SerializeField] float fixedAngleBetweenObjects = 0.1f;
    [SerializeField] float minAngle = -1.5f;
    [SerializeField] float maxAngle = 1.5f;
    [SerializeField] bool isFixedDistanceBetweenObjects = false;
    [SerializeField] bool isFixedAngleBetweenObjects = false;
    [SerializeField] int numOrbsInChunk = 1;
    [SerializeField][Range(0, 1)] float proportionOfBombs;
    [SerializeField][Range(0, 1)] float proportionHighOrbs = 0;

    [SerializeField] int randomSeed = 12345;


    void OnValidate()
    {
#if UNITY_EDITOR

        if (!isActive) return;
        if (!CheckAllFieldsAreFilled()) return;
        UnityEngine.Random.InitState(randomSeed);

        ClampNumAndPosition();
        InstantiateChunk();
        PlaceObjectsInChunk();
#endif
    }
    

    private void ClampNumAndPosition()
    {
        chunkPosition = Mathf.Clamp(chunkPosition, 0, trackSpline.CalculateLength());
        numOrbsInChunk = Mathf.Clamp(numOrbsInChunk, 1, numOrbsInChunk);
    }

    private void PlaceObjectsInChunk()
    {
        float position = chunkPosition;
        float angle = chunkAngle;

        for (int i = 0; i < transform.childCount; i++)
        {
            ObjectPlacer objectToPlace = transform.GetChild(i).GetComponent<ObjectPlacer>();
            objectToPlace.splinePositionUnit = PathIndexUnit.Distance;
            objectToPlace.splinePositionParameter = position;

            if (isFixedAngleBetweenObjects) { objectToPlace.angle = angle; angle += fixedAngleBetweenObjects; }
            else objectToPlace.angle = angle + UnityEngine.Random.Range(minAngle, maxAngle);

            objectToPlace.PlaceObject();

            if (!isFixedDistanceBetweenObjects) position += UnityEngine.Random.Range(minDistBetweenObjects, maxDistBetweenObjects);
            else position += fixedDistBetweenObjects;
        }
    }

    private void InstantiateChunk()
    {
#if UNITY_EDITOR
        while (transform.childCount < numOrbsInChunk)
        {
            ObjectPlacer objectPlacer;

            if (UnityEngine.Random.Range(0f, 1f) < proportionOfBombs)
            {
                objectPlacer = PrefabUtility.InstantiatePrefab(potionBombPrefab, transform) as ObjectPlacer;
            }
            else if (UnityEngine.Random.Range(0f, 1f) < proportionHighOrbs)
            {
                objectPlacer = PrefabUtility.InstantiatePrefab(highOrbPrefab, transform) as ObjectPlacer;
            }
            else
            {
                objectPlacer = PrefabUtility.InstantiatePrefab(lowOrbPrefab, transform) as ObjectPlacer;
            }

            objectPlacer.splineContainer = trackSpline;
            objectPlacer.PlaceObject();
        }
#endif
    }

    private bool CheckAllFieldsAreFilled()
    {
        if (trackSpline == null) return false;

        if (lowOrbPrefab == null) return false;

        if (potionBombPrefab == null) return false;

        return true;
    }

    public void ClearChunk()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
    public void ResetChunk()
    {
        ClearChunk();
        OnValidate();
    }
}
