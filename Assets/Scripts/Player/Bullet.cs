using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float damage = 20f;
    public float timeBeforeDestroy = 1f;
    [SerializeField] bool isPlayerBullet = false;

    [SerializeField] GameObject brokenBullet = null;

    private float timer;

    private void Start()
    {
        //This is the timer so it doesn't hits itself.
        if (!isPlayerBullet)
            timer = 0.3f;
    }

    private void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(8) && timer <= 0)
        {
            Damageable damageable = collision.GetComponent<Damageable>();

            if (damageable.elementType != Damageable.ElementType.Blue)
            {
                damageable.TakeDamage(damage);
                damageable.Knockback(transform.position, damage);
            }
            else if (damageable.elementType == Damageable.ElementType.Blue)
            {
                damageable.Knockback(transform.position, damage / 10);
            }

            if (isPlayerBullet)
                AudioManager.instance.Play("water");

            if (brokenBullet != null)
                Destroy(Instantiate(brokenBullet, transform.position, Quaternion.identity), 0.3f);
        }

        if (collision.name.Equals("Player") && !isPlayerBullet)
        {
            collision.GetComponent<HealthController>().TakeDamage((int)damage);
            collision.GetComponent<KnockbackItself>().Knockback(0.1f, damage, transform);
        }

        if ((collision.name.Equals("Weapon") || collision.name.Equals("Weapon 2")) && !isPlayerBullet)
        {
            if (brokenBullet != null)
                Destroy(Instantiate(brokenBullet, transform.position, Quaternion.identity), 0.3f);

            if (AudioManager.instance != null)
                AudioManager.instance.Play("arrow high");

            Destroy(gameObject);
        }

        Destroy(gameObject, timeBeforeDestroy);
    }
}
