using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerCollisionDetection : MonoBehaviour
{
    [SerializeField] GameObject loseOrbsAnimator;

    Animator animator;

    [SerializeField] float timeProtectedAfterBeingHit = 1.5f;
    public bool isTemporarilyProtected = false;

    PlayerStats playerStats;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ICollideable>(out ICollideable collidable))
        {
            if (isTemporarilyProtected && IsOtherObstacle(other)) return;
            if (MakesPlayerLoseOrbs(other))
            {
                StartCoroutine(SetProtectionForLimitedTime());
                
                if(!playerStats.IsInvulnerable)
                {
                    var loseOrbAnim = Instantiate(
                        loseOrbsAnimator,
                        transform.position,
                        transform.rotation
                    );

                    animator.SetTrigger("GotHit");
                    Destroy(loseOrbAnim, 3f);
                }
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

    bool MakesPlayerLoseOrbs(Collider other)
    {
        if (other.GetComponent<PotionBomb>()) return true;
        if (other.GetComponent<StaticWall>()) return true;

        return false;
    }
    IEnumerator SetProtectionForLimitedTime()
    {
        isTemporarilyProtected = true;
        yield return new WaitForSeconds(timeProtectedAfterBeingHit);
        isTemporarilyProtected = false;
    }
}
