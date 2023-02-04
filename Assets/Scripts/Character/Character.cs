using UnityEngine;

public class Character : MonoBehaviour, IPlayer
{
    private const int LAYER_PLATFORM = 9;
    [SerializeField] private PlayerType _playerType;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool isOnFloor;
    [SerializeField] private float currentJumpSpeed;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool canJump;
    public CharacterSetting characterSetting;
    public CharacterKeySetting characterKeySetting;

    private float _buff_MoveSpeed = 1;
    private int _buff_Direction = 1;

    private Rigidbody2D GetRigidBody => GetComponent<Rigidbody2D>();

    private void FixedUpdate()
    {
        var moveDirection = GetInputMoveDirection() * _buff_Direction;
        float horizontalMoveSpeed = GetHorizontalMoveSpeed(moveDirection) * _buff_MoveSpeed;
        float verticalMoveSpeed = GetVerticalMoveSpeed();

        GetRigidBody.velocity = new Vector2(horizontalMoveSpeed * Time.fixedDeltaTime, verticalMoveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        bool isCollidePlatform = col.gameObject.layer == LAYER_PLATFORM;
        if (isCollidePlatform)
        {
            currentJumpSpeed = characterSetting.jumpForce;
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

    public PlayerType GetPlayerType()
    {
        return _playerType;
    }

    public void SetBuff(ItemType itemType , object data)
    {
        if (data == null)
            return;

        switch (itemType)
        {
            case ItemType.Speed:
                _buff_MoveSpeed = (float)data;
                break;
            case ItemType.Control:
                _buff_Direction = (int)data;
                break;
        }
    }

    private float GetVerticalMoveSpeed()
    {
        float verticalMoveSpeed = 0;
        if (Input.GetKey(characterKeySetting.moveUpKey))
        {
            if (canJump)
            {
                verticalMoveSpeed = currentJumpSpeed * Time.fixedDeltaTime;
                isJumping = true;
                if (isOnFloor == false) currentJumpSpeed = Mathf.Max(currentJumpSpeed - characterSetting.jumpForceConsume, 0);
            }
        }
        else
        {
            if (isJumping)
                canJump = false;
        }

        if (verticalMoveSpeed <= 0)
            verticalMoveSpeed = GetRigidBody.velocity.y;

        return verticalMoveSpeed;
    }

    private float GetHorizontalMoveSpeed(int direction)
    {
        switch (direction)
        {
            case 1:
                moveSpeed = Mathf.Min(moveSpeed + characterSetting.acceleration, characterSetting.speedLimit);
                break;
            case -1:
                moveSpeed = Mathf.Max(moveSpeed - characterSetting.acceleration, -characterSetting.speedLimit);
                break;
            case 0:
            {
                if (moveSpeed > 0)
                    moveSpeed = Mathf.Max(moveSpeed - characterSetting.breakAcceleration, 0);
                else if (moveSpeed < 0)
                    moveSpeed = Mathf.Min(moveSpeed + characterSetting.breakAcceleration, 0);
                break;
            }
        }

        return moveSpeed;
    }

    private int GetInputMoveDirection()
    {
        int direction = 0;
        if (Input.GetKey(characterKeySetting.moveLeftKey) && Input.GetKey(characterKeySetting.moveRightKey)) direction = 0;
        else if (Input.GetKey(characterKeySetting.moveLeftKey)) direction = -1;
        else if (Input.GetKey(characterKeySetting.moveRightKey)) direction = 1;

        return direction;
    }
}