using UnityEngine;

public class ItemSpeed : ItemBase
{
    [SerializeField]
    private float _changeMulSpeed = 0.2f; 

    public override ItemType ItemType { get; protected set; } = ItemType.Speed;


    protected override void StartBuff()
    {
        _playerBase.SetBuff(ItemType , _changeMulSpeed);
    }

    protected override void EndBuff()
    {
        _playerBase.SetBuff(ItemType , 1);
    }
}