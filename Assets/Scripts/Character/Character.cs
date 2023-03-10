using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : PlayerBase
{
    [SerializeField] private PlayerType _playerType;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool isOnFloor;
    [SerializeField] private bool isPlayingQte;
    [SerializeField] private Radish collisionRadish;
    [SerializeField] private float moveTimer;
    [SerializeField] private bool isRunning;
    [SerializeField] private int moveDirection;
    public Transform footPoint;
    public SpriteRenderer spriteRenderer;
    public CharacterSetting characterSetting;
    public CharacterKeySetting characterKeySetting;
    public GameController gameController;
    public QTE qte;
    private int _buff_Direction = 1;
    private float _buff_MoveSpeed = 1;
    private float _buff_Jump = 1;
    private int radishNowHp;
    private Rigidbody2D GetRigidBody => GetComponent<Rigidbody2D>();
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerAnimaInfo playerAnimaInfo;
    private List<ItemType> _ownBuffs = new List<ItemType>();

    private bool HaveCollidingRadish => collisionRadish;

    private void Update()
    {
        if (isPlayingQte)
            return;

        CheckOnFloor();
        HorizontalMove();
        CheckToJump();

        if (HaveCollidingRadish &&
            Input.GetKeyDown(characterKeySetting.actKey) &&
            collisionRadish.GetIsBusy() == false)
            PullRadish();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (IsCollideOn(col, LayerDefine.LAYER_CHARACTER))
            EnterCollideCharacter(col);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (IsTriggerOn(col, LayerDefine.LAYER_RADISH))
            EnterTriggerRadish(col);

        if (IsTriggerOn(col, LayerDefine.LAYER_ITEM))
        {
            AudioManagerScript.Instance.PlayAudioClip("trigger_item");
            var itemBase = col.gameObject.GetComponent<ItemBase>();
            if (itemBase != null)
                ItemManager.Instance.OnCollider_Item(this, itemBase);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (IsTriggerOn(col, LayerDefine.LAYER_RADISH))
            ExitTriggerRadish();
    }

    private void CheckOnFloor()
    {
        isOnFloor = Physics2D.OverlapCircle(footPoint.position, characterSetting.footRadius, LayerMask.GetMask("Platform"));
    }

    private void CheckToJump()
    {
        if (Input.GetKeyDown(characterKeySetting.jumpKey))
        {
            if (isOnFloor) Jump();
        }
    }

    private void Jump()
    {
        GetRigidBody.AddForce(Vector2.up * characterSetting.jumpForce * _buff_Jump);
    }

    private void HorizontalMove()
    {
        moveDirection = GetInputMoveDirection() * _buff_Direction;
        float horizontalMoveSpeed = GetHorizontalMoveSpeed(moveDirection) * _buff_MoveSpeed;

        if (isRunning && horizontalMoveSpeed == 0)
        {
            isRunning = false;
            animator.SetBool("IsRunning", isRunning);
        }
        else if (isRunning == false && Mathf.Abs(horizontalMoveSpeed) > 0)
        {
            isRunning = true;
            animator.SetBool("IsRunning", isRunning);
        }

        if (horizontalMoveSpeed != 0 && moveDirection != 0) spriteRenderer.flipX = moveDirection == 1;

        transform.Translate(Vector3.right * Time.deltaTime * horizontalMoveSpeed);
    }

    public override PlayerType GetPlayerType()
    {
        return _playerType;
    }

    public override void SetBuff(ItemType itemType, bool isOnOrOff, object data)
    {
        if (data == null)
            return;

        if (isOnOrOff)
            _ownBuffs.Add(itemType);
        else
            _ownBuffs.Remove(itemType);

        switch (itemType)
        {
            case ItemType.Speed:
                _buff_MoveSpeed = (float)data;
                break;
            case ItemType.Control:
                _buff_Direction = (int)data;
                break;
            case ItemType.Jump:
                _buff_Jump = (float)data;
                break;
        }
    }

    public override bool IsOwnBuff(ItemType itemType)
    {
        return _ownBuffs.Contains(itemType);
    }

    private void EnterTriggerRadish(Collider2D col)
    {
        TryGetCollisionTarget(col, out collisionRadish);
    }

    private void ExitTriggerRadish()
    {
        if (isPlayingQte)
        {
            qte.QteUnexpectedStop();
            collisionRadish.PullQteFail();
            isPlayingQte = false;
        }

        if (HaveCollidingRadish)
            collisionRadish = null;
    }

    private void PullRadish()
    {
        isPlayingQte = true;
        moveSpeed = 0;

        StopAllCoroutines();
        StartCoroutine(Cor_PlayPullRadishSound());

        collisionRadish.StartPull();
        RequestQte();
    }

    private void ReceiveQteResult(bool isQteSuccess)
    {
        if (isQteSuccess && HaveCollidingRadish)
        {
            collisionRadish.PullQteSuccess();
            if (collisionRadish.GetIsComplete())
            {
                animator.SetTrigger("PullFinish");
                isPlayingQte = false;
                gameController.RadishFraction(_playerType, 1);
                AudioManagerScript.Instance.PlayAudioClip("pull_radish_success");
            }
            else
                RequestQte();
        }
        else
        {
            AudioManagerScript.Instance.PlayAudioClip("qte_fail");
            collisionRadish.PullQteFail();
            animator.SetTrigger("PullFinish");
            isPlayingQte = false;
        }
    }

    private void RequestQte()
    {
        qte.StartQte(ReceiveQteResult);
        radishNowHp = collisionRadish.RemainHp();
        animator.Play(playerAnimaInfo.GetPullLevelAnimaName(radishNowHp));
    }

    private void EnterCollideCharacter(Collision2D col)
    {
        ContactPoint2D[] contactPoints = col.contacts;
        ContactPoint2D contactPoint = contactPoints[0];

        if (TryGetCollisionTarget(col, out Character collisionCharacter))
            collisionCharacter.BeStroked(moveSpeed, contactPoint);
    }

    private bool TryGetCollisionTarget<T>(Collision2D col, out T target) where T : MonoBehaviour
    {
        target = col.gameObject.GetComponent<T>();
        return target != null;
    }

    private bool TryGetCollisionTarget<T>(Collider2D col, out T target) where T : MonoBehaviour
    {
        target = col.gameObject.GetComponent<T>();
        return target != null;
    }

    private void BeStroked(float targetMoveSpeed, ContactPoint2D contactPoint)
    {
        AudioManagerScript.Instance.PlayAudioClip("character_strike");
        CharacterStrikeEffectManager.Instance.PlayStrikeEffect(_playerType, contactPoint.point);

        Vector2 strikeVector = new Vector2(-contactPoint.normal.x * characterSetting.horizontalStrikeCurve.Evaluate(Mathf.Abs(targetMoveSpeed)),
            characterSetting.verticalStrikeCurve.Evaluate(Mathf.Abs(targetMoveSpeed)));
        GetRigidBody.AddForce(strikeVector, ForceMode2D.Impulse);
    }

    private bool IsCollideOn(Collision2D col, int collisionTargetLayer)
    {
        bool isCollidePlatform = col.gameObject.layer == collisionTargetLayer;
        return isCollidePlatform;
    }

    private bool IsTriggerOn(Collider2D col, int triggerTargetLayer)
    {
        bool isCollidePlatform = col.gameObject.layer == triggerTargetLayer;
        return isCollidePlatform;
    }

    private float GetHorizontalMoveSpeed(int direction)
    {
        switch (direction)
        {
            case 1:
                moveSpeed = characterSetting.moveSpeedCurve.Evaluate(moveTimer);
                moveTimer += Time.deltaTime;
                break;
            case -1:
                moveSpeed = -characterSetting.moveSpeedCurve.Evaluate(moveTimer);
                moveTimer += Time.deltaTime;
                break;
            case 0:
            {
                moveTimer = 0;

                if (moveSpeed > -0.1f && moveSpeed < 0.1f)
                    moveSpeed = 0;
                else if (moveSpeed > 0)
                    moveSpeed -= characterSetting.breakAcceleration * Time.deltaTime;
                else if (moveSpeed < 0)
                    moveSpeed += characterSetting.breakAcceleration * Time.deltaTime;
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

    private IEnumerator Cor_PlayPullRadishSound()
    {
        while (isPlayingQte)
        {
            Debug.Log("Cor_PlayPullRadishSound");
            AudioManagerScript.Instance.PlayAudioClip("pulling_radish");
            yield return new WaitForSeconds(0.7f);
        }
    }
}