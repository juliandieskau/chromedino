﻿using log4net;
using System;
using System.Reflection;
using System.Threading;

namespace MA_Control;

/// <summary>
/// Class that starts the game.
/// </summary>
internal class Game
{
    #region Fields

    private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

    private readonly Obstacles _obstacles;

    public static bool gameStarted { get; set;  } = false;

    public static long startTime { get; set; }

    public static long highscore { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="obstacles">Obstacles reference.</param>
    public Game(Obstacles obstacles)
    {
        _obstacles = obstacles;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Starts the loop for refreshing the Obtacles at the Background.
    /// </summary>
    public void Start()
    {
        _log.Info(GetType().Name + "." + MethodBase.GetCurrentMethod());
        // highscore von JSON laden
        highscore = 0;
        while (true)
        {
            if (!gameStarted)
            {
                while (!DisplayContent.startPressed)
                {
                    // TODO: Anzeigen: "press spacebar to restart"
                    startTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                }
                DisplayContent.startPressed = false;
                gameStarted = true;
            }
            if (DisplayContent.IsGameOver())
            {
                _obstacles.ReloadBackground();
                if (Graphics.score > highscore)
                {
                    // TODO: Anzeigen: "new highscore!"
                    highscore = Graphics.score;
                    // TODO: highscore speichern in JSON
                }
                while (!DisplayContent.startPressed)
                {
                    // TODO: Anzeigen: "press spacebar to restart"
                    startTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                }
                
                DisplayContent.startPressed = false;
            }

            _obstacles.UpdateBackground();
            Thread.Sleep(50);
        }
    }

    #endregion
}