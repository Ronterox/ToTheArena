using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackItself : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigidbdy = null;

    public IEnumerator Knockback(float knockDur, float knockbackPwr, Transform obj)
    {

        float timer = 0;

        while (knockDur > timer)
        {
            timer += Time.deltaTime;
            Vector2 direction = (transform.position - obj.transform.position).normalized;
            rigidbdy.AddForce(direction * knockbackPwr);
        }

        yield return 0;
    }
}
