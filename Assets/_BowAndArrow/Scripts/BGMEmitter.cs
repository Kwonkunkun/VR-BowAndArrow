using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BGMEmitter : MonoBehaviour
{
    public float speed = 1.5f;
    public float rotateSpeed = 5.0f;

    Vector3 newPosition;
    bool isMoving = false;

    void Start()
    {
        PositionChange();
    }

    void PositionChange()
    {
        newPosition = new Vector2(Random.Range(0f, 10f), Random.Range(0f, 10f));
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, newPosition) < 1)
            PositionChange();

        if (LookAt2D(newPosition))
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
    }

    bool LookAt2D(Vector3 lookAtPosition)
    {
        float distanceX = lookAtPosition.x - transform.position.x;
        float distanceY = lookAtPosition.y - transform.position.y;
        float angle = Mathf.Atan2(distanceX, distanceY) * Mathf.Rad2Deg;

        Quaternion endRotation = Quaternion.AngleAxis(angle, Vector3.back);
        transform.rotation = Quaternion.Slerp(transform.rotation, endRotation, Time.deltaTime * rotateSpeed);

        if (Quaternion.Angle(transform.rotation, endRotation) < 1f)
            return true;

        return false;
    }
}