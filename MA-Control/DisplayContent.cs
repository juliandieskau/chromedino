using Library;
using log4net;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace MA_Control;

/// <summary>
/// Main class where the logic can be implemented.
/// </summary>
public class DisplayContent
{
    #region Fields

    private static readonly ILog _log = LogManager.GetLogger(typeof(Program));
    private long _timeSinceLastJump;
    private long _timestamp;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="controlService">Control Service reference.</param>
    public DisplayContent(IControlService controlService)
    {
        var graphics = new Graphics(controlService);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Thread for the processing in the background.
    /// </summary>
    /// <param name="displayContent">.</param>
    public static void BackgroundWorker(DisplayContent displayContent)
    {
        while (true)
        {
            displayContent.RefreshPlayer();
            Thread.Sleep(100);
        }
    }

    /// <summary>
    /// Call the method to clear the obstacles.
    /// </summary>
    public static void ClearObstacles()
    {
        Graphics.ClearObstacles();
    }

    /// <summary>
    /// Draws the floor at the LED-Monitors.
    /// </summary>
    public static void DrawFloor()
    {
        var darkRedPen = new Pen(Color.DarkBlue, 2);
        Point[] rectangleEdges =
        {
            new(0, 32),
            new(0, 30),
            new(192, 32),
            new(192, 30)
        };
        Graphics.UpdateObstacles(darkRedPen, rectangleEdges);
    }

    /// <summary>
    /// Draws a obstacle with the specified properties.
    /// </summary>
    /// <param name="obstacleEdges">Edges of the obstacle.</param>
    /// <param name="pen">Pen to draw the obstacle.</param>
    public static void DrawObstacle(Point[] obstacleEdges, Pen pen)
    {
        Graphics.UpdateObstacles(pen, obstacleEdges);
    }

    /// <summary>
    /// Checks if the game is over.
    /// </summary>
    /// <returns>True if the game is over, otherwise false.</returns>
    public static bool IsGameOver()
    {
        if (Graphics.GameOver)
        {
            // Reset the property at pressing space bar.

            // Graphics.GameOver = false;

            return true;
        }

        return false;
    }

    /// <summary>
    /// Method that is called when a key is pressed.
    /// </summary>
    /// <param name="pressedKey">Pressed key.</param>
    public void OnKeyPressed(Keys pressedKey)
    {
        // Print the key that is pressed.
        _log.Info("Pressed key: " + pressedKey);
        Console.WriteLine(pressedKey);

        // The up key is pressed.
        if (pressedKey == Keys.Up)
        {
            // wenn man im game over screen ist ist springen nicht möglich
            if (!IsGameOver())
            {
                // Abfragen ob auf dem Boden und nicht über Zeit die gesprungen wurde
                if (positionY <= 0.0)
                {
                    Console.Beep();
                    _timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                }
            }
            
        }
        // The down key is pressed.
        else if (pressedKey == Keys.Down)
        {
            Console.Write(".");
        }
        // The space bar is pressed.
        else if (pressedKey == Keys.Space) 
        {
            // space bar aktivieren wenn game damit gestartet wird
            if (!Game.gameStarted)
            {
                startPressed = true;
            } 
            // space bar aktivieren wenn im game over screen
            else if (IsGameOver()) {
                startPressed = true;
                Graphics.GameOver = false;
            }
        }

        // text input a-z,A-Z,0-9 eingeben, wenn in game over screen + score speichern
        // Zahl 1-9 als Schwierigkeit drücken, wenn im game over screen/spiel noch nicht gestartet
        else if (pressedKey >= Keys.D1 && pressedKey <= Keys.D4) 
        {
            if (!Game.gameStarted || IsGameOver())
            {
                if (pressedKey == Keys.D1)
                {
                    Game.difficulty = Game.Difficulty.EASY;
                } else if (pressedKey == Keys.D2)
                {
                    Game.difficulty = Game.Difficulty.NORMAL;
                } else if (pressedKey == Keys.D3)
                {
                    Game.difficulty = Game.Difficulty.HARD;
                } else if (pressedKey == Keys.D4)
                {
                    Game.difficulty = Game.Difficulty.IMPOSSIBLE;
                }

                // je nach schwierigkeit (wenn geändert) den angezeigten highscore anpassen
                Game.setHighscoreForDifficulty();
            }
        }
    }

    /// <summary>
    /// Bitte nicht setten, danke
    /// </summary>
    public static bool startPressed { get; set; } = false;

    #endregion

    #region Private Methods

    private double positionY = 0.0;
    private double velocityY = 0.0;
    private const double gravity = 16.0;

    /// <summary>
    /// Draws the jump of the Dino.
    /// </summary>
    private void RefreshPlayer()
    {
        _timeSinceLastJump = DateTimeOffset.Now.ToUnixTimeMilliseconds() - _timestamp;
        /*
        double tempValue = 0;

        
        if (_timeSinceLastJump < 1000)
        {
            // range: 0.0->16.0 Höhe, in 1s wächst linear
            tempValue = 32.0 * (_timeSinceLastJump / 2000.0);
        }
        else if (_timeSinceLastJump >= 1000)
        {
            // 16.0->0.0 Höhe, in 1s
            tempValue = 16 - 32.0 * ((_timeSinceLastJump - 1000) / 2000.0);
        }*/

        if (_timeSinceLastJump < 200 )
        {
            velocityY = 20.0;
        }
        if (_timeSinceLastJump > 0)
        {
            positionY += velocityY * (_timeSinceLastJump / 2000.0);
            // Gravitation verringern beim Fallen
            if (velocityY > 0)
            {
                velocityY -= gravity * (_timeSinceLastJump / 2000.0);
            } else
            {
                velocityY -= 0.5 * gravity * (_timeSinceLastJump / 2000.0);
            }
        }

        var yValue = (int)positionY;
        if (yValue < 0 || IsGameOver())
        {
            positionY = 0.0;
            velocityY = 0.0;

            yValue = 0;
        }

        Graphics.UpdateDino(yValue);
    }

    #endregion
}