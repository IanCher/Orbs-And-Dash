using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCollisionDetection : MonoBehaviour
{
    [SerializeField] GameObject loseOrbsAnimator;

    Animator animator;

    [SerializeField] float timeProtectedAfterBeingHit = 1.5f;
    public bool isTemporarilyProtected = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ICollideable>(out ICollideable collidable))
        {
            if (isTemporarilyProtected && IsOtherObstacle(other)) return;
            if (other.GetComponent<PotionBomb>() || other.GetComponent<StaticWall>())
            {
                var loseOrbAnim = Instantiate(
                    loseOrbsAnimator,
                    transform.position,
                    transform.rotation
                );
                Destroy(loseOrbAnim, 3f);
                StartCoroutine(SetProtectionForLimitedTime());
                animator.SetTrigger("GotHit");
            }

            collidable.HandlePlayerCollision();
        }
    }

    bool IsOtherObstacle(Collider other)
    {
        if (other.GetComponent<PotionBomb>()) return true;
        if (other.GetComponent<StaticWall>()) return true;
        if (other.GetComponent<ParalysingSpell>()) return true;

        return false;
    }
    
    IEnumerator SetProtectionForLimitedTime()
    {
        isTemporarilyProtected = true;
        yield return new WaitForSeconds(timeProtectedAfterBeingHit);
        isTemporarilyProtected = false;
    }
}
