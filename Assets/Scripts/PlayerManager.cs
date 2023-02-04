using JetBrains.Annotations;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    
    [SerializeField]
    private PlayerBase player1 = null;
    
    
    [SerializeField]
    private PlayerBase player2 = null;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        PlayerManager.Instance = null;
    }

    public PlayerBase GetAnotherPlayer(PlayerType playerType)
    {
        if (playerType == PlayerType.Player1)
            return player2;

        return player1;
    }
}