using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] float attackDistance = 1;
    [SerializeField] float attackDelay;
    [SerializeField] float attackCooldown;
    [SerializeField] float attackDamage = 1;

    public bool isAttacking { get; private set; }
    public bool isInCooldown { get; private set; }
    Animator anim;
    static readonly int ATTACK_ANIM = Animator.StringToHash("Attack");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = transform.position.ToVector2();
        Transform target = EnemyTargetManager.Instance.GetTargetForPos(pos);
        if (target == null)
            return;

        //if in range to attack
        if (!isAttacking && Vector2.Distance(pos, target.position.ToVector2()) < attackDistance)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        anim.SetTrigger(ATTACK_ANIM);

        yield return new WaitForSeconds(attackDelay);

        FXManager.Instance.EmitAttack(FXManager.AttackType.Radio, transform.position);
        isInCooldown = true;

        yield return new WaitForSeconds(0.1f);

        Collider[] cols = Physics.OverlapSphere(transform.position, attackDistance);
        foreach (Collider col in cols)
        {
            if(col.TryGetComponent(out IHealth health))
            {
                health.Damage(gameObject, attackDamage, 0);
            }
        }

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
        isInCooldown = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
#endif
}
