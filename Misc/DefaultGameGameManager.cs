using DefaultGame.Entities;
using DefaultGame.UI;
using MonoGameLibrary.Managers;
using System;

namespace DefaultGame.Misc;

public class DefaultGameGameManager : GameManager
{
    internal static DefaultGameGameManager s_instance;

    /// <summary>
    /// Gets a reference to the Core instance.
    /// </summary>
    public static DefaultGameGameManager Instance => s_instance;

    public bool paused = false;

    public Player player;

    public DefaultGameGameManager() : base()
    {
        // Ensure that multiple cores are not created.
        if (s_instance != null)
        {
            throw new InvalidOperationException($"Only a single DefaultGameGameManager instance can be created");
        }

        // Store reference to engine for global member access.
        s_instance = this;

        GameScale = 2;
    }

    public void PauseGame()
    {
        ((GameSceneUI)UIManager.Instance.currentUIEntity).PauseGame();
    }
}
