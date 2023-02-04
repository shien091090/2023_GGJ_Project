using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;

public class Character : MonoBehaviour
{
    private const int LAYER_PLATFORM = 9;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool isOnFloor;
    [SerializeField] private float currentJumpSpeed;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool canJump;
    public float acceleration;
    public float breakAcceleration;
    public float speedLimit;
    public float jumpForceConsume;

    private Rigidbody2D GetRigidBody => GetComponent<Rigidbody2D>();

    private void FixedUpdate()
    {
        float horizontalMoveSpeed = GetHorizontalMoveSpeed(GetInputMoveDirection());
        
        float verticalMoveSpeed = 0;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (canJump)
            {
                verticalMoveSpeed = currentJumpSpeed * Time.fixedDeltaTime;
                isJumping = true;
                if (isOnFloor == false) currentJumpSpeed = Mathf.Max(currentJumpSpeed - jumpForceConsume, 0);
            }
        }
        else
        {
            if(isJumping)
                canJump = false;
        }

        if (verticalMoveSpeed <= 0)
            verticalMoveSpeed = GetRigidBody.velocity.y;

        GetRigidBody.velocity = new Vector2(horizontalMoveSpeed * Time.fixedDeltaTime, verticalMoveSpeed);
    }

    private float GetHorizontalMoveSpeed(int direction)
    {
        switch (direction)
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

        return moveSpeed;
    }

    private int GetInputMoveDirection()
    {
        int direction = 0;
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow)) direction = 0;
        else if (Input.GetKey(KeyCode.LeftArrow)) direction = -1;
        else if (Input.GetKey(KeyCode.RightArrow)) direction = 1;

        return direction;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        bool isCollidePlatform = col.gameObject.layer == LAYER_PLATFORM;
        if (isCollidePlatform)
        {
            currentJumpSpeed = jumpForce;
            isOnFloor = true;
            isJumping = false;
            canJump = true;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        bool isCollidePlatform = col.gameObject.layer == LAYER_PLATFORM;
        if (isCollidePlatform)
        {
            isOnFloor = false;
            if (isJumping == false)
                canJump = false;
        }
    }
}