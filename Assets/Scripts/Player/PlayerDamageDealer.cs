using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageDealer : MonoBehaviour
{
    [SerializeField] Transform[] attackPoints = null;
    public float attackRange = 0.5f;
    [SerializeField] LayerMask damageableLayer = default;
    public float damageMinimum = 50f;

    private float damage;

    private FacePointer facePointerScript;

    private int hitSoundTimer = 0;

    private bool isCharging = false;

    private void Start()
    {
        facePointerScript = GetComponent<FacePointer>();
    }
    private void Update()
    {
        if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && !GameManager.instance.isPaused && !isCharging)
        {
            ChargeAttack();
        }
        else
        {
            damage = facePointerScript.GetTargetVelocity();
            if (damage > damageMinimum)
                DealDamage();
        }

        if ((Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) && isCharging)
            DropCharge();
    }

    private void DropCharge()
    {
        foreach (Transform attackPoint in attackPoints)
        {
            isCharging = false;
            if (attackPoint.gameObject.activeSelf)
                attackPoint.gameObject.GetComponent<Animator>().SetBool("isCharging", isCharging);
        }
    }

    private void DealDamage()
    {
        foreach (Transform attackPoint in attackPoints)
        {
            //Detect Enemies
            if (attackPoint.gameObject.activeSelf)
            {
                Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, damageableLayer);

                foreach (Collider2D enemy in enemiesHit)
                {
                    Damageable damageable = enemy.gameObject.GetComponentInParent<Damageable>();
                    damageable.TakeDamage(damage);
                    damageable.Knockback(transform.position);

                    EnemyController enemyController = enemy.gameObject.GetComponentInParent<EnemyController>();
                    if (enemyController != null)
                        enemyController.WaitForChase(damageable.knockTime);

                    hitSoundTimer = (hitSoundTimer + 1) % 2;

                    if (hitSoundTimer == 0)
                    {
                        AudioManager.instance.Stop("hit2");
                        AudioManager.instance.Play("hit1");
                    }
                    else
                    {
                        AudioManager.instance.Stop("hit1");
                        AudioManager.instance.Play("hit2");
                    }

                    GameManager.instance.ShakeCamera();
                }
            }
        }
    }

    private void ChargeAttack()
    {
        foreach (Transform attackPoint in attackPoints)
        {
            isCharging = true;
            if (attackPoint.gameObject.activeSelf)
                attackPoint.gameObject.GetComponent<Animator>().SetBool("isCharging", isCharging);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoints == null)
            return;

        Gizmos.color = Color.green;
        foreach (Transform attackPoint in attackPoints)
        {
            if (attackPoint.gameObject.activeSelf)
                Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
