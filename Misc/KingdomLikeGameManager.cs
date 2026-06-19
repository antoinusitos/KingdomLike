using KingdomLike.Entities;
using KingdomLike.UI;
using MonoGameLibrary.Managers;
using System;

namespace KingdomLike.Misc;

public class KingdomLikeGameManager : GameManager
{
    internal static KingdomLikeGameManager instance;

    /// <summary>
    /// Gets a reference to the Core instance.
    /// </summary>
    public static KingdomLikeGameManager Instance => instance;

    public bool paused = false;

    public Player player;

    public KingdomLikeGameManager() : base()
    {
        // Ensure that multiple cores are not created.
        if (instance != null)
        {
            throw new InvalidOperationException($"Only a single DefaultGameGameManager instance can be created");
        }

        // Store reference to engine for global member access.
        instance = this;

        GameScale = 2;
        UseInteractionSystem = true;
    }

    public void PauseGame()
    {
        ((GameSceneUI)UIManager.Instance.currentUIEntity).PauseGame();
    }
}
