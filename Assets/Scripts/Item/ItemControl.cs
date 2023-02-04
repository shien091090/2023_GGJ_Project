public class ItemControl : ItemBase
{
    public override ItemType ItemType { get; protected set; } = ItemType.Control;

    protected override void StartBuff()
    {
        _playerBase.SetBuff(ItemType , -1);
    }

    protected override void EndBuff()
    {
        _playerBase.SetBuff(ItemType , 1);
    }
}