using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float movementSpeed = 5f;

    public MovementType movementType = MovementType.MoveTowards;

    [SerializeField] GameObject playerReference = null;

    [SerializeField] float knockbackForce = 5f;

    [SerializeField] bool causeKnockback = false;

    [SerializeField] int damage = 20;

    private float timer = 0;
    private bool chasing = true;

    public enum MovementType
    {
        Lerp,
        MoveTowards
    }
    private void Start()
    {
        if (playerReference == null)
            playerReference = GameObject.Find("Player");
    }

    private void Update()
    {
        if (playerReference == null)
            return;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (chasing)
            Chase();
    }

    private void Chase()
    {
        Transform playerPosition = playerReference.transform;

        if (movementType.Equals(MovementType.MoveTowards))
            transform.position = Vector3.MoveTowards(transform.position, playerPosition.position, movementSpeed * Time.deltaTime);
        else
            transform.position = Vector3.Lerp(transform.position, playerPosition.position, movementSpeed * Time.deltaTime);

        if (transform.position.x < playerPosition.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);

        if (Vector2.Distance(transform.position, playerPosition.position) > 50)
            GetComponent<Damageable>().Die();
    }

    public void WaitForChase(float seconds)
    {
        timer = seconds;
    }

    public void StopChase()
    {
        chasing = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.name.Equals("Player"))
        {
            collision.gameObject.GetComponent<HealthController>().TakeDamage(damage);
            if (causeKnockback) 
            {
                StartCoroutine(collision.gameObject.GetComponent<KnockbackItself>().Knockback(0.1f, knockbackForce, transform));
                WaitForChase(2);
            }
        }
    }
}
