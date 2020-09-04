using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] GameObject bullet = null;

    [SerializeField] Transform firePosition = null;

    public float bulletTime = 0.5f;

    private void Update()
    {
        if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && !GameManager.instance.isPaused)
            Shoot();
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bullet, firePosition.position, firePosition.rotation);
        Rigidbody2D rbBullet = bulletObj.GetComponent<Rigidbody2D>();
        Bullet blt = bulletObj.GetComponent<Bullet>();

        blt.timeBeforeDestroy = bulletTime;

        rbBullet.AddForce(firePosition.up * bulletSpeed, ForceMode2D.Impulse);
    }
}
