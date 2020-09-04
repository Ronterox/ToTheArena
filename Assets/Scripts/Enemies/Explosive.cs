using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public float cooldown = 5f;
    public float explosionRange = 2f;

    public float power = 10f;

    [SerializeField] bool explodeAutomatically = false;

    [SerializeField] LayerMask damageableLayer = default;

    [SerializeField] GameObject explosionObject = null;

    public SpriteRenderer weaponSprite = null;

    private float timer;

    [HideInInspector]
    public Color defaultColor;

    private void Start()
    {
        if (weaponSprite != null)
            defaultColor = weaponSprite.color;

        if (explodeAutomatically)
            timer = cooldown;
    }

    private void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;

        if (weaponSprite != null)
        {
            if (timer <= 0 && weaponSprite.color != Color.red)
            {
                if (defaultColor != weaponSprite.color)
                    defaultColor = weaponSprite.color;
                weaponSprite.color = Color.red;
                AudioManager.instance.Play("explosion");
            }
        }

        if (timer <= 0 && !GameManager.instance.isPaused)
        {
            if (((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !explodeAutomatically))
                Explode();
            else if (explodeAutomatically)
                Explode();
        }
    }

    private void Explode()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, explosionRange, damageableLayer);

        foreach (Collider2D collider in collisions)
        {
            KnockbackItself knockbackItself = collider.GetComponent<KnockbackItself>();
            Damageable damageable = collider.GetComponent<Damageable>();
            HealthController healthPlayer = collider.GetComponent<HealthController>();

            if (damageable != null)
            {
                if (damageable.elementType != Damageable.ElementType.Red)
                    damageable.TakeDamage(power);
            }

            if (knockbackItself != null)
                knockbackItself.Knockback(0.1f, power, transform);
            if (healthPlayer != null)
                healthPlayer.TakeDamage(50);
        }

        if (explosionObject != null)
        {
            GameObject explosionInstance = Instantiate(explosionObject, transform.position, Quaternion.identity);
            Destroy(explosionInstance, 0.3f);
        }

        AudioManager.instance.Play("explosion");
        GameManager.instance.ShakeCamera();

        if (weaponSprite != null)
            weaponSprite.color = defaultColor;

        timer = cooldown;

        if (explodeAutomatically)
        {
            GameManager.IncreaseKills(1);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
