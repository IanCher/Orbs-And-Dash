using UnityEngine;

public class ResetAllChunks : MonoBehaviour
{
    public void ResetAll()
    {
        foreach (ChunkPlacer chunkPlacer in GetComponentsInChildren<ChunkPlacer>())
        {
            chunkPlacer.ResetChunk();
        }
    }
}
