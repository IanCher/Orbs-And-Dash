using System;
using UnityEngine;

public class PlayerCollisionDetection : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<ICollideable>(out ICollideable collidable))
        {
            collidable.HandlePlayerCollision();
        }
    }
}
