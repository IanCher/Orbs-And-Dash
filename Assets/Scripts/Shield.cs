using System;
using UnityEngine;

public class Shield : MonoBehaviour, ICollideable
{
    public static event Action<ShieldData> OnCollidedWithShield;
    public ShieldData ShieldData;

    public void HandlePlayerCollision()
    {
        OnCollidedWithShield?.Invoke(ShieldData);
        AudioManager.instance.PlaySound("Shield");
        Destroy(gameObject);
    }
}

[Serializable]
public class ShieldData
{
    public int value = 2;
}
