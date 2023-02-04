using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public enum RadishState
{
    Underground,
    Head,
    Half,
    almost,
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

    private bool isPulling;
    private float radishHeight;
    private RadishState nowState;
    private SpriteRenderer sprite;


    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        radishHeight = sprite.size.y;
    }

    public void StartPull()
    {
        if(isPulling != true)
            isPulling = true;
    }

    public void PullQteSuccess()
    {
        LevelUp();
    }
    public void PullQteFail()
    {
        isPulling = false;
        nowState = RadishState.Underground;
    }

    private void LevelUp()
    {
        if ((int)nowState < 4)
        { 
            nowState++;
            transform.DOMoveY(radishHeight * (int)nowState / 5,pullOutTime).SetEase(Ease.Linear);
        }
        if (nowState == RadishState.Complete)
        {
            isPulling = false;
            sprite.DOFade(0, flyOutTime).SetEase(Ease.Linear);
            transform.DOMoveY(flyOutDistance, flyOutTime).SetEase(Ease.Linear).OnComplete(() => {
                DestroyImmediate(gameObject);
            });
        }
    }
}
