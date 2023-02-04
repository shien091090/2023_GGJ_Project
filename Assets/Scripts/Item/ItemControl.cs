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