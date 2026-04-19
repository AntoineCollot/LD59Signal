using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float moveSpeed = 3;
    [SerializeField] float separationDist = 1;
    [SerializeField] float bodySize = 1;

    [Header("Rules")]
    [SerializeField] float targetWeight = 1;
    [SerializeField] float separationWeight = 1;

    Vector2 movement;
    float lookAtAngle, refLookAtAngle;
    const float LOOK_AT_SMOOTH = 0.2f;
    EnemyAttack attack;

    public Vector2 Position2D
    {
        get
        {
            return transform.position.ToVector2();
        }
    }
    float spawnTime;

    void Start()
    {
        spawnTime = GameManager.Instance.gameTime;
        attack = GetComponent<EnemyAttack>();

        EnemyGrid.Instance.Register(this);
    }

    private void OnDestroy()
    {
        if (EnemyGrid.Instance != null)
            EnemyGrid.Instance.Deregister(this);
    }

    void Update()
    {
        Move();
    }

    Vector2 TargetRule()
    {
        Vector2 currentPos = Position2D;
        Transform target = EnemyTargetManager.Instance.GetTargetForPos(currentPos);
        if (target == null)
            return Vector2.zero;

        return (target.position.ToVector2() - currentPos).normalized;
    }

    Vector2 SeparationRule()
    {
        Vector2 currentPos = Position2D;
        List<EnemyMovement> neighbours = EnemyGrid.Instance.GetEnemiesAround(currentPos);
        Vector2 separationVector = Vector2.zero;

        foreach (EnemyMovement neighbour in neighbours)
        {
            if (neighbour == this || neighbour == null)
                continue;
            Vector2 toNeighbour = currentPos - neighbour.Position2D;
            float magnitude = toNeighbour.magnitude + Mathf.Epsilon;
            separationVector += (toNeighbour / magnitude) * MathUtils.Inv(magnitude, separationDist * neighbour.bodySize);
        }

        return separationVector;
    }

    void Move()
    {
        if (!GameManager.Instance.GameIsPlaying)
            return;

        if (attack.isInCooldown)
            return;

        Vector2 dir = Vector2.zero;
        dir += TargetRule() * targetWeight;
        dir += SeparationRule() * separationWeight;

        dir.Normalize();

        Vector3 movement = dir.ToVector3() * moveSpeed * Time.deltaTime;
        if (GameManager.Instance.gameTime > spawnTime + EnemySpawner.TIME_BEFORE_MOVEMENT) //Make sure the enemy spawned early enough to move
            transform.Translate(movement, Space.World);

        float targetAngle = -Vector2.SignedAngle(Vector2.up, movement.ToVector2());
        lookAtAngle = Mathf.SmoothDampAngle(lookAtAngle, targetAngle, ref refLookAtAngle, LOOK_AT_SMOOTH);
        transform.rotation = Quaternion.Euler(0, lookAtAngle, 0);
    }
}
