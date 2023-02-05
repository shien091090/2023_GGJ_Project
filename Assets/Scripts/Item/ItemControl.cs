public class ItemControl : ItemBase
{
    public override ItemType ItemType { get; protected set; } = ItemType.Control;

    protected override void StartBuff()
    {
        SetBuff(true , -1);
    }

    protected override void EndBuff()
    {
        SetBuff(false , 1);
    }
}