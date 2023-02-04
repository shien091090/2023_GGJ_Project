public interface IPlayer
{
    public PlayerType GetPlayerType();
    public void SetBuff(ItemType itemType, object data);
}