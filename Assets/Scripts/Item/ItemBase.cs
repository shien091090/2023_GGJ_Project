using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField]
    private string _directions = "";

    [SerializeField]
    private SpriteRenderer _sr = null;

    [SerializeField]
    private Collider2D _collider2D = null;

    [SerializeField]
    protected float _buffTime = 0;

    [SerializeField]
    protected float _disappearTime = 0;
    
    protected float _elpasedTime = 0;
    
    protected bool _isBuffTrigger = false;

    public abstract ItemType ItemType { get; protected set; }

    protected PlayerBase _playerBase = null;
    private Action<ItemBase> _releaseItem = null;

    public TriggerTarget TriggerType { get; protected set; }
    public int TerrainId { get; protected set; } = -1;

    public Vector2 SpriteSize => _sr.bounds.size;

    public string GetDirections => _directions;

    public void InitBuff(TriggerTarget triggerTarget , int terrainId , Action<ItemBase> releaseBuff)
    {
        TerrainId = terrainId;
        TriggerType = triggerTarget;
        _releaseItem = releaseBuff;

        _elpasedTime = 0;
        _isBuffTrigger = false;
    }

    public void TriggerBuff(PlayerBase playerBase)
    {
        _collider2D.enabled = false;
        _sr.enabled = false;

        _playerBase = playerBase;
        _elpasedTime = 0;
        _isBuffTrigger = true;

        StartBuffBase();
    }


    public void Tick(float deltaTime)
    {
        _elpasedTime += deltaTime;

        if (_isBuffTrigger)
        {
            if (_elpasedTime >= _buffTime)
                EndBuffBase();
        }
        else
        {
            if (_elpasedTime >= _disappearTime)
                ReleaseBuff();
        }
    }

    private void StartBuffBase()
    {
        _isBuffTrigger = true;

        StartBuff();
    }

    protected abstract void StartBuff();

    private void EndBuffBase()
    {
        //_isBuffTrigger = false;

        EndBuff();

        ReleaseBuff();
    }

    protected abstract void EndBuff();

    protected void SetBuff(bool isOnOrOff, object data)
    {
        _playerBase.SetBuff(ItemType , isOnOrOff , data);
    }

    private void ReleaseBuff()
    {
        _releaseItem?.Invoke(this);
    }
}