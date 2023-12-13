using log4net;
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
        while (true)
        {
            if (!gameStarted)
            {
                while (!DisplayContent.startPressed)
                {
                    // TODO: Anzeigen: "press spacebar to restart"
                }
                DisplayContent.startPressed = false;
                gameStarted = true;
            }
            if (DisplayContent.IsGameOver())
            {
                _obstacles.ReloadBackground();
                Thread.Sleep(1000);
                while (!DisplayContent.startPressed)
                {
                    // TODO: Anzeigen: "press spacebar to restart"
                }
                DisplayContent.startPressed = false;
            }

            _obstacles.UpdateBackground();
            Thread.Sleep(50);
        }
    }

    #endregion
}