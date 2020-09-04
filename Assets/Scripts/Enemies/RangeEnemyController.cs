using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyController : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] float stoppingDistance = 5;
    [SerializeField] float retreatDistance = 5;

    private float timeBtwShots;
    [SerializeField] float startTimeBtwShots = 1;

    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float timeBFDestroy = 1;

    private Transform player = null;
    [SerializeField] Transform weapon = null;
    [SerializeField] GameObject projectile = null;

    [SerializeField] GameObject particles = null;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeBtwShots = startTimeBtwShots;
    }

    void Update()
    {
        weapon.up = player.position;

        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            if (particles != null)
                Destroy(Instantiate(particles, transform.position, Quaternion.identity), 0.3f);
        }
        else if (Vector2.Distance(transform.position, player.position) <= retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
            if (particles != null)
                Destroy(Instantiate(particles, transform.position, Quaternion.identity), 0.3f);
        }

        if (Vector2.Distance(transform.position, player.position) > 50)
            GetComponent<Damageable>().Die();

        if (timeBtwShots <= 0)
        {
            if (AudioManager.instance != null)
                AudioManager.instance.Play("arrow");

            GameObject bulletObj = Instantiate(projectile, weapon.position, weapon.rotation);
            Rigidbody2D rbBullet = bulletObj.GetComponent<Rigidbody2D>();
            Bullet blt = bulletObj.GetComponent<Bullet>();

            Animator weaponAnimator = weapon.GetComponent<Animator>();

            if (weaponAnimator != null)
                weaponAnimator.SetTrigger("shoot");

            blt.timeBeforeDestroy = timeBFDestroy;

            rbBullet.AddForce(weapon.up * bulletSpeed, ForceMode2D.Impulse);

            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }
}
