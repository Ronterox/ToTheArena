using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] int healing = 20;
    [SerializeField] float timer = 3f;

    private void Start()
    {
        Destroy(gameObject, timer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))
        {
            collision.gameObject.GetComponent<HealthController>().Heal(healing);
            AudioManager.instance.Play("pop");
            Destroy(gameObject);
        }
    }
}
