using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    [SerializeField] bool realMouseIsVisible = false;

    [SerializeField] Vector2 maxXY = Vector2.zero;
    [SerializeField] Vector2 minXY = Vector2.zero;
    [SerializeField] bool limitArea = false;

    [Space]
    [SerializeField] float range = 0f;
    [SerializeField] float minRange = 0f;
    [SerializeField] bool limitAroundParent = false;
    private void Awake()
    {
        if (!realMouseIsVisible)
            Cursor.visible = false;
    }
    private void Update()
    {
        FakeMouse();
    }
    private void FakeMouse()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        transform.position += new Vector3(x, y);

        if (limitArea)
        {
            if (transform.position.x > maxXY.x || transform.position.y > maxXY.y)
            {
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y);
                if (transform.position.x > maxXY.x)
                {
                    newPos.x = maxXY.x;
                }
                if (transform.position.y > maxXY.y)
                {
                    newPos.y = maxXY.y;
                }
                transform.position = newPos;
            }
            else if (transform.position.x < minXY.x || transform.position.y < minXY.y)
            {
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y);
                if (transform.position.x < minXY.x)
                {
                    newPos.x = minXY.x;
                }
                if (transform.position.y < minXY.y)
                {
                    newPos.y = minXY.y;
                }
                transform.position = newPos;
            }
        }

        if (limitAroundParent)
        {
            var vectorFromParent = transform.localPosition + new Vector3(x, y, 0) * Time.deltaTime;
            transform.localPosition = ClampMagnitude(vectorFromParent, range, minRange);
        }
    }

    private Vector3 ClampMagnitude(Vector3 v, float max, float min)
    {
        double sm = v.sqrMagnitude;
        if (sm > max * max) return v.normalized * max;
        else if (sm < min * min) return v.normalized * min;
        return v;
    }
}
