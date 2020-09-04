using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public float movementSpeed = 5f;

    [SerializeField] new Rigidbody2D rigidbody = null;

    [SerializeField] float dashSpeed = 5f;

    public bool canDash = false;

    [SerializeField] GameObject dashParticles = null;

    private void Start()
    {
        if (rigidbody == null)
            rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        DetectMovement();
    }

    private void DetectMovement()
    {
        float playerInputHorizontal = Input.GetAxisRaw("Horizontal");
        float playerInputVertical = Input.GetAxisRaw("Vertical");

        if (playerInputHorizontal > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift) && canDash)
            {
                rigidbody.velocity = new Vector2(movementSpeed * dashSpeed, rigidbody.velocity.y);
                Destroy(Instantiate(dashParticles, transform.position, Quaternion.identity),0.3f);
            }
            else
                rigidbody.velocity = new Vector2(movementSpeed, rigidbody.velocity.y);
        }
        else if (playerInputHorizontal < 0)
        {
            if (Input.GetKey(KeyCode.LeftShift) && canDash)
            {
                rigidbody.velocity = new Vector2(-movementSpeed * dashSpeed, rigidbody.velocity.y);
                Destroy(Instantiate(dashParticles, transform.position, Quaternion.identity), 0.3f);
            }
            else
                rigidbody.velocity = new Vector2(-movementSpeed, rigidbody.velocity.y);
        }
        else
            rigidbody.velocity = new Vector2(0f, rigidbody.velocity.y);

        if (playerInputVertical > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift) && canDash)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, movementSpeed * dashSpeed);
                Destroy(Instantiate(dashParticles, transform.position, Quaternion.identity), 0.3f);
            }
            else
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, movementSpeed);
        }
        else if (playerInputVertical < 0)
        {
            if (Input.GetKey(KeyCode.LeftShift) && canDash)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, -movementSpeed * dashSpeed);
                Destroy(Instantiate(dashParticles, transform.position, Quaternion.identity), 0.3f);
            }
            else
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, -movementSpeed);
        }
        else
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0f);
    }
}
