using UnityEngine;

public class Character : PlayerBase
{
    private const int LAYER_RADISH = 8;
    private const int LAYER_CHARACTER = 11;
    private const int LAYER_PLATFORM = 14;
    [SerializeField] private PlayerType _playerType;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool isOnFloor;
    [SerializeField] private float currentJumpSpeed;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool canJump;
    public CharacterSetting characterSetting;
    public CharacterKeySetting characterKeySetting;
    public QTE qte;
    private int _buff_Direction = 1;

    private float _buff_MoveSpeed = 1;

    private Rigidbody2D GetRigidBody => GetComponent<Rigidbody2D>();

    private void FixedUpdate()
    {
        int moveDirection = GetInputMoveDirection() * _buff_Direction;
        float horizontalMoveSpeed = GetHorizontalMoveSpeed(moveDirection) * _buff_MoveSpeed;
        float verticalMoveSpeed = GetVerticalMoveSpeed();

        GetRigidBody.velocity = new Vector2(horizontalMoveSpeed * Time.fixedDeltaTime, verticalMoveSpeed);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (IsCollideOn(col, LAYER_PLATFORM))
            EnterCollidePlatform();
        if (IsCollideOn(col, LAYER_CHARACTER))
            EnterCollideCharacter(col);
        if (IsCollideOn(col, LAYER_RADISH))
            EnterCollideRadish(col);
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (IsCollideOn(col, LAYER_PLATFORM))
            ExitCollidePlatform();
    }

    public override PlayerType GetPlayerType()
    {
        return _playerType;
    }

    public override void SetBuff(ItemType itemType, object data)
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

    private void EnterCollidePlatform()
    {
        currentJumpSpeed = characterSetting.jumpForce;
        isOnFloor = true;
        isJumping = false;
        canJump = true;
    }

    private void EnterCollideRadish(Collision2D col)
    {
    }

    private void EnterCollideCharacter(Collision2D col)
    {
        ContactPoint2D[] contactPoints = col.contacts;
        ContactPoint2D contactPoint = contactPoints[0];

        if (HaveCollisionTarget(col, out Character collisionCharacter))
            collisionCharacter.BeStroked(moveSpeed, contactPoint);
    }

    private bool HaveCollisionTarget<T>(Collision2D col, out T target) where T : MonoBehaviour
    {
        target = col.gameObject.GetComponent<T>();
        return target != null;
    }

    private void ExitCollidePlatform()
    {
        isOnFloor = false;
        if (isJumping == false)
            canJump = false;
    }

    private void BeStroked(float targetMoveSpeed, ContactPoint2D contactPoint)
    {
        Vector2 strikeVector = new Vector2(contactPoint.normal.x * targetMoveSpeed * 0.1f, characterSetting.strikeRiseForce);
        Debug.Log($"{gameObject.name} BeStroked, normal = {contactPoint.normal}, targetMoveSpeed = {targetMoveSpeed}, strikeVector = {strikeVector}");
    }

    private bool IsCollideOn(Collision2D col, int collisionTargetLayer)
    {
        bool isCollidePlatform = col.gameObject.layer == collisionTargetLayer;
        return isCollidePlatform;
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