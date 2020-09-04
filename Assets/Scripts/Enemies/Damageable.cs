using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    public float health = 100f;

    [SerializeField] Rigidbody2D rigidbdy = null;

    public float knockTime = 0.5f;
    private float timer = 0;

    [SerializeField] float knockbackForce = 5f;

    [SerializeField] bool knockback = true;

    [SerializeField] GameObject scoreUpText = null;

    [SerializeField] GameObject drop = null;

    [SerializeField] GameObject deadbody = null;

    [HideInInspector]
    public bool isDead;

    [SerializeField] Animator animator=null;

    public ElementType elementType = ElementType.Red;

    public enum ElementType
    {
        Red,
        Blue,
        Green
    }

    private void Start()
    {
        if (health != maxHealth)
            health = maxHealth;
    }

    private void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        health -= damage;

        timer = knockTime;

        GameManager.IncreaseScore((int)damage);

        if (scoreUpText != null)
        {
            GameObject scorePOP = Instantiate(scoreUpText, transform.position, Quaternion.identity);
            scorePOP.GetComponent<TextMesh>().text = (int)damage + "";
        }

        if (animator != null)
        {
            animator.SetTrigger("hit");
        }

        if (health < 1 && !isDead)
        {
            Die();
        }
    }

    public void Knockback(Vector3 damageDealer, float knockbackForce)
    {
        if (!knockback)
            return;

        Vector3 difference = transform.position - damageDealer;

        rigidbdy.AddForce(difference * knockbackForce, ForceMode2D.Impulse);
    }

    public void Knockback(Vector3 damageDealer)
    {
        if (!knockback)
            return;

        Vector3 difference = transform.position - damageDealer;

        rigidbdy.AddForce(difference * knockbackForce, ForceMode2D.Impulse);
    }

    public void Die()
    {
        AudioManager.instance.Play("squish");

        isDead = true;

        GameManager.IncreaseKills(1);

        EnemyController enemyController = GetComponent<EnemyController>();
        if (enemyController != null)
            enemyController.StopChase();

        if (drop != null)
            Instantiate(drop, transform.position, Quaternion.identity);

        if (deadbody != null)
            Instantiate(deadbody, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
