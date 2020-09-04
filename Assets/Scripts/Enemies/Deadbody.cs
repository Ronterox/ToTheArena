using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadbody : MonoBehaviour
{
    [SerializeField] Vector2 forceDirection = Vector2.zero;
    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(forceDirection, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Player"))
            AudioManager.instance.Play("bones");
    }
}
