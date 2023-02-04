using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;


public enum RadishState
{
    Idle,
    Busy,
    Complete    
}

public class Radish : MonoBehaviour
{
    [SerializeField]
    private float flyOutDistance;
    [SerializeField]
    private float flyOutTime;
    [Space]
    [SerializeField]
    private float pullOutTime;

    private float radishHeight;
    private RadishState nowState;
    private SpriteRenderer sprite;
    [SerializeField]
    private int radishNowHp;
    private int radishMaxHp;


    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        radishHeight = sprite.sprite.pivot.y * 0.01f;
        radishMaxHp = radishNowHp;
    }

    public bool GetIsBusy()
    {
        if (nowState == RadishState.Busy)
            return true;
        else
            return false;

    }

    public void StartPull()
    {
        nowState = RadishState.Busy;
    }

    public bool PullQteSuccess()
    {
        radishNowHp--;
        if (radishNowHp > 0)
        {
            transform.DOLocalMoveY(radishHeight / radishMaxHp * (radishMaxHp - radishNowHp), pullOutTime).SetEase(Ease.Linear);
            return false;
        }
        else
        {
            nowState = RadishState.Complete;
            transform.DOLocalMoveY(flyOutDistance, flyOutTime).SetEase(Ease.Linear).OnComplete(() => {
                sprite.DOFade(0, flyOutTime).SetEase(Ease.Linear).OnComplete(() => {
                    DestroyImmediate(gameObject);
                });
            });
            return true;

        }
        
    }

    public void PullQteFail()
    {
        nowState = RadishState.Idle;
    }
}
