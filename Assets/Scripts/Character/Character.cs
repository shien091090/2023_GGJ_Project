using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private int moveDirection;
    public float acceleration;
    public float breakAcceleration;
    public float speedLimit;

    private Rigidbody2D GetRigidBody => GetComponent<Rigidbody2D>();

    private void FixedUpdate()
    {
        moveDirection = 0;
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)) moveDirection = 0;
        else if (Input.GetKey(KeyCode.LeftArrow)) moveDirection = -1;
        else if (Input.GetKey(KeyCode.RightArrow)) moveDirection = 1;

        switch (moveDirection)
        {
            case 1:
                moveSpeed = Mathf.Min(moveSpeed + acceleration, speedLimit);
                break;
            case -1:
                moveSpeed = Mathf.Max(moveSpeed - acceleration, -speedLimit);
                break;
            case 0:
            {
                if (moveSpeed > 0)
                    moveSpeed = Mathf.Max(moveSpeed - breakAcceleration, 0);
                else if (moveSpeed < 0)
                    moveSpeed = Mathf.Min(moveSpeed + breakAcceleration, 0);
                break;
            }
        }
         
        GetRigidBody.velocity = new Vector2(moveSpeed * Time.fixedDeltaTime, 0);
    }
}