using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField]
    protected TriggerTarget _triggerTarget = TriggerTarget.Another;

    [SerializeField]
    protected float _buffTime = 0;

    [SerializeField]
    protected float _disappearTime = 0;
    
    protected float _elpasedTime = 0;
    
    protected bool _isBuffTrigger = false;

    public abstract ItemType ItemType { get; protected set; }

    protected IPlayer _player = null;
    private Action<ItemBase> _releaseItem = null;

    public TriggerTarget GetTriggerType => _triggerTarget;

    public void InitBuff(Action<ItemBase> releaseBuff)
    {
        _releaseItem = releaseBuff;

        _elpasedTime = 0;
        _isBuffTrigger = false;
    }

    public void TriggerBuff(IPlayer player)
    {
        _player = player;
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
        _isBuffTrigger = false;

        EndBuff();

        ReleaseBuff();
    }

    protected abstract void EndBuff();

    private void ReleaseBuff()
    {
        _releaseItem?.Invoke(this);
    }
}

public enum TriggerTarget
{
    Self,
    Another
}

public class ItemSpeed : ItemBase
{
    public override ItemType ItemType { get; protected set; } = ItemType.Speed;


    protected override void StartBuff()
    {
        _player.SetBuff(ItemType , -1);
    }

    protected override void EndBuff()
    {
        _player.SetBuff(ItemType , 1);
    }
}

public class ItemControl : ItemBase
{
    public override ItemType ItemType { get; protected set; } = ItemType.Control;

    protected override void StartBuff()
    {
        _player.SetBuff(ItemType , 1);
    }

    protected override void EndBuff()
    {
        _player.SetBuff(ItemType , -1);
    }
}