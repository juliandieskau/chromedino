using log4net;
using System;
using System.Drawing.Drawing2D;
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
    public static long highscoreEasy { get; set; }
    public static long highscoreNormal { get; set; }
    public static long highscoreHard { get; set; }
    public static long highscoreImpossible { get; set; }

    public static Difficulty difficulty { get; set; } = Difficulty.HARD;

    public enum Difficulty
    {
        EASY = 10,
        NORMAL = 7,
        HARD = 3,
        IMPOSSIBLE = 1
    }

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
        // alle highscores von JSON laden
        highscoreEasy = 0;
        highscoreNormal = 0;
        highscoreHard = 0;
        highscoreImpossible = 0;
        try
        {
            Highscore.File file = Highscore.readHighscoreFromJSON();
            highscoreEasy = file.HighscoreEasy;
            highscoreNormal = file.HighscoreNormal;
            highscoreHard = file.HighscoreHard;
            highscoreImpossible = file.HighscoreImpossible;
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        while (true)
        {
            if (!gameStarted)
            {

                while (!DisplayContent.startPressed)
                {
                    startTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                }
                // je nach gewählter schwierigkeit den richtigen highscore auswählen und anzeigen
                setHighscoreForDifficulty();
                DisplayContent.startPressed = false;
                gameStarted = true;
            }
            if (DisplayContent.IsGameOver())
            {
                _obstacles.ReloadBackground();
                // erst überprüfen ob der neue score ein highscore ist für die aktuelle schwierigkeit
                if (Graphics.score > highscore)
                {
                    highscore = Graphics.score;
                    // highscore lokal speichern
                    saveHighscoreLocal();

                    // alle highscores speichern in JSON, nachdem der neue gesetzt wurde
                    Highscore.File file = new Highscore.File
                    {
                        HighscoreEasy = highscoreEasy,
                        HighscoreNormal = highscoreNormal,
                        HighscoreHard = highscoreHard,
                        HighscoreImpossible = highscoreImpossible
                    };
                    bool saved = Highscore.saveToJSON(file);
                }

                while (!DisplayContent.startPressed)
                {
                    startTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                }
                
                DisplayContent.startPressed = false;
            }

            _obstacles.UpdateBackground();
            // über Zeit schnelleres updaten
            // schwierigkeit bei 5 ist okay, 1 sehr hart
            Thread.Sleep(getFrameTime((int) difficulty));
        }
    }

    /// <summary>
    /// Setzt den Highscore der angezeigt wird auf den der richtigen Schwierigkeit
    /// </summary>
    public static void setHighscoreForDifficulty()
    {
        switch (difficulty)
        {
            case Difficulty.EASY:
                highscore = highscoreEasy;
                break;
            case Difficulty.NORMAL:
                highscore = highscoreNormal;
                break;
            case Difficulty.HARD:
                highscore = highscoreHard;
                break;
            case Difficulty.IMPOSSIBLE:
                highscore = highscoreImpossible;
                break;
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Berechne die Zeit in Millisekunden pro Frame für die Hindernisse
    /// Abhängig von wie viel Zeit bereits vergangen ist.
    /// </summary>
    /// <returns>int zwischen 25 und 45</returns>
    private static int getFrameTime(int difficulty)
    {
        // score ist wert >= 0
        int score = Convert.ToInt32(Graphics.score);
        int startTime = 45;
        int endTime = 20;
        int change = score / difficulty;
        int returnTime = startTime - change;
        if (returnTime <= endTime)
        {
            return endTime;
        }
        return returnTime;
    }

    

    /// <summary>
    /// Speichere den neuen Highscore in den Highscore der aktuellen Schwierigkeit
    /// Achtung: Ersetzt den Highscore der Schwierigkeit, ohne zu checken, dass korrekt
    /// </summary>
    private static void saveHighscoreLocal()
    {
        switch (difficulty)
        {
            case Difficulty.EASY:
                highscoreEasy = highscore;
                break;
            case Difficulty.NORMAL:
                highscoreNormal = highscore;
                break;
            case Difficulty.HARD:
                highscoreHard = highscore;
                break;
            case Difficulty.IMPOSSIBLE:
                highscoreImpossible = highscore;
                break;
        }
    }

    #endregion
}