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
        radishHeight = sprite.sprite.pivot.y * 0.01f * transform.localScale.y;
        radishMaxHp = radishNowHp;
    }

    public bool GetIsBusy()
    {
        if (nowState == RadishState.Busy)
            return true;
        else
            return false;

    }
    public bool GetIsComplete()
    {
        if (nowState == RadishState.Complete)
            return true;
        else
            return false;
    }

    public void StartPull()
    {
        nowState = RadishState.Busy;
    }   

    public void PullQteSuccess()
    {
        radishNowHp--;
        if (radishNowHp > 0)
        {
            var endValue = transform.localPosition.y + (radishHeight / radishMaxHp * (radishMaxHp - radishNowHp));
            transform.DOLocalMoveY(endValue, pullOutTime).SetEase(Ease.Linear);
        }
        else
        {
            nowState = RadishState.Complete;
            transform.DOLocalMoveY(flyOutDistance, flyOutTime).SetEase(Ease.Linear).OnComplete(() => {
                sprite.DOFade(0, flyOutTime).SetEase(Ease.Linear).OnComplete(() => {
                    DestroyImmediate(gameObject);
                });
            });
        }
        
    }

    public void PullQteFail()
    {
        nowState = RadishState.Idle;
    }
}
