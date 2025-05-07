using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendController : BaseController
{
    private List<EnemyController> _enemyList;
    private EnemyController _currentTarget;

    [SerializeField] private float followRange = 10f;

    protected override void Update()
    {
        base.Update();
        _enemyList = GameManager.instance.EnemyManager.ActiveEnemies;
    }

    protected override void HandleAction()
    {
        base.HandleAction();
        
        if (weaponHandler == null || _enemyList == null || _enemyList.Count == 0)
        {
            if (GameManager.instance.Player == null)
            {
                isAttacking = false;
                movementDirection = Vector2.zero;
                return;
            }

            Transform player = GameManager.instance.Player.transform;
            
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            float followDistance = 3f;

            if (distanceToPlayer > followDistance)
            {
                Vector2 dirToPlayer = (player.position - transform.position).normalized;
                movementDirection = dirToPlayer;
                lookDirection = dirToPlayer;
            }
            else
            {
                movementDirection = Vector2.zero;
            }
            return;
        }

        _currentTarget = FindClosestEnemy();

        float distance = Vector3.Distance(transform.position, _currentTarget.transform.position);
        Vector2 direction = (_currentTarget.transform.position - transform.position).normalized;

        isAttacking = false;

        if (distance <= followRange)
        {
            lookDirection = direction;

            if (distance <= weaponHandler.AttackRange)
            {
                int layerMaskTarget = weaponHandler.target;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, weaponHandler.AttackRange * 1.5f,
                    (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget);

                if (hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer)))
                {
                    isAttacking = true;
                }

                movementDirection = Vector2.zero;
                return;
            }

            movementDirection = direction;
        }
        else
        {
            movementDirection = Vector2.zero;
        }
    }

    private EnemyController FindClosestEnemy()
    {
        EnemyController closest = null;
        float minDist = float.MaxValue;

        foreach (var enemy in _enemyList)
        {
            if (enemy == null) continue;

            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = enemy;
            }
        }

        return closest;
    }

    public override void Death()
    {
        base.Death();
    }
}
