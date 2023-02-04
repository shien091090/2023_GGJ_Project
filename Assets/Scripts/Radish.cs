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
        radishHeight = sprite.size.y;
        radishMaxHp = radishNowHp;
    }

    public bool StartPull()
    {
        if (nowState == RadishState.Busy)
            return false;
        else
        { 
            nowState = RadishState.Busy;
            return true;
        }
    }

    public bool PullQteSuccess()
    {
        if (radishNowHp > 0)
        {
            nowState--;
            transform.DOMoveY(radishHeight * radishNowHp / radishMaxHp, pullOutTime).SetEase(Ease.Linear);
            return false;
        }
        else
        {
            nowState = RadishState.Complete;
            transform.DOMoveY(flyOutDistance, flyOutTime).SetEase(Ease.Linear).OnComplete(() => {
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
