using UnityEngine;

public class ItemSpeed : ItemBase
{
    [SerializeField]
    private float _changeMulSpeed = 0.2f; 

    public override ItemType ItemType { get; protected set; } = ItemType.Speed;


    protected override void StartBuff()
    {
        _player.SetBuff(ItemType , _changeMulSpeed);
    }

    protected override void EndBuff()
    {
        _player.SetBuff(ItemType , 1);
    }
}