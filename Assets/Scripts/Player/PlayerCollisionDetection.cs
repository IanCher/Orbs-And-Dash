using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollisionDetection : MonoBehaviour
{
    [SerializeField] GameObject loseOrbsAnimator;
    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<ICollideable>(out ICollideable collidable))
        {

            if (other.GetComponent<PotionBomb>() || other.GetComponent<StaticWall>())
            {
                var loseOrbAnim = Instantiate(
                    loseOrbsAnimator,
                    transform.position,
                    transform.rotation
                );
                Destroy(loseOrbAnim.gameObject, 3f);
            } 
             
            collidable.HandlePlayerCollision();
        }
    }
}
