using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemJump : ItemBase
{
    public override ItemType ItemType { get; protected set; } = ItemType.Jump;

    [SerializeField]
    private float _deBuffValue = 0.5f;

    protected override void StartBuff()
    {
        SetBuff(true , _deBuffValue);
    }

    protected override void EndBuff()
    {
        SetBuff(false , 1.0f);
    }
}
