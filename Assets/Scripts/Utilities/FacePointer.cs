using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePointer : MonoBehaviour
{
    [SerializeField] Camera cam = null;
    [SerializeField] Transform target = null;

    [SerializeField] Transform fakeMouse = null;
    [SerializeField] bool useFake = false;

    private Vector3 lastTargetPos;

    private void Start()
    {
        lastTargetPos = target.position;
    }

    private void Update()
    {
        FollowMouse();
    }

    private void FollowMouse()
    {
        Vector2 direction;
        if (useFake)
            direction = new Vector2(GetFakeMousePosition().x - target.position.x, GetFakeMousePosition().y - target.position.y);
        else
            direction = new Vector2(GetMousePosition().x - target.position.x, GetMousePosition().y - target.position.y);

        if (direction.x != 0 && direction.y != 0)
            target.up = direction;
    }

    private Vector3 GetMousePosition()
    {
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private Vector3 GetFakeMousePosition()
    {
        return fakeMouse.position;
    }

    public float GetTargetVelocity()
    {
        float velocity = (transform.position - lastTargetPos).magnitude / Time.deltaTime;
        lastTargetPos = transform.position;
        return velocity;
    }
}
