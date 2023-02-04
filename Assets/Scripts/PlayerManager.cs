using JetBrains.Annotations;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instacne { get; private set; }

    [CanBeNull]
    [SerializeField]
    private IPlayer player1 = null;
    
    
    [CanBeNull]
    [SerializeField]
    private IPlayer player2 = null;

    private void Awake()
    {
        Instacne = this;
        DontDestroyOnLoad(gameObject);
    }

    public IPlayer GetAnotherPlayer(PlayerType playerType)
    {
        if (playerType == PlayerType.Player1)
            return player2;

        return player1;
    }
}