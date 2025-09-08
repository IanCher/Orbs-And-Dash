using System;
using UnityEngine;

public class Shield : MonoBehaviour, ICollideable
{
    public static event Action<ShieldData> OnCollidedWithShield;
    public ShieldData ShieldData;

    public void HandlePlayerCollision()
    {
        OnCollidedWithShield?.Invoke(ShieldData);
        Destroy(gameObject);
    }
}

[Serializable]
public class ShieldData
{
    public float DurationSeconds = 4.0f;
    public int value = 2;
}
