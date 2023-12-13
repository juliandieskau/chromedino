using Library;
using log4net;
using MA_Control.Models;
using System.Collections.Concurrent;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace MA_Control;

public class Graphics
{
    #region Fields

    // Represents a thread-safe, unordered collection of obstacle-objects.
    private static readonly ConcurrentBag<DrawableObstacleModel> _obstacles = new();

    // Represent the dino-object.
    private static readonly DinoModel _dino = new();

    private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

    #endregion

    #region Properties

    /// <summary>
    /// Specifies whether the game is over (true) or not (false).
    /// </summary>
    public static bool GameOver { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="controlService">ControlService-reference.</param>
    public Graphics(IControlService controlService)
    {
        _log.Info(GetType().Name + "." + MethodBase.GetCurrentMethod());

        var refreshThread = new Thread(() => RefreshCycle(controlService));
        refreshThread.Start();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Clears the collection of obstacles-object.
    /// </summary>
    public static void ClearObstacles()
    {
        _obstacles.Clear();
    }

    /// <summary>
    /// Updates the current height of the Dino.
    /// </summary>
    /// <param name="newYPosition">New Y-Position of the Dino.</param>
    public static void UpdateDino(int newYPosition)
    {
        _dino.CurrentHeight = newYPosition;
    }

    /// <summary>
    /// Updates the obstacles-object with a new obstacle with the specified properties.
    /// </summary>
    /// <param name="pen">Pen of the obstacle.</param>
    /// <param name="obstacleEdges">Edges of the obstacle.</param>
    public static void UpdateObstacles(Pen pen, Point[] obstacleEdges)
    {
        _obstacles.Add(new DrawableObstacleModel
        {
            Pen = pen,
            Edges = obstacleEdges
        });
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Checks if there is a collision.
    /// </summary>
    /// <param name="playerHitbox"></param>
    /// <returns></returns>
    private static bool CheckCollision(Point[] playerHitbox)
    {

        // Rectangle(linksoben.X, linksoben.Y, breite nach rechts, höhe nach unten)
        var hitboxTop = new Rectangle(23, -_dino.CurrentHeight + 19, 4, 5);
        var hitboxBottom = new Rectangle(20, -_dino.CurrentHeight + 23, 4, 6);
         
        foreach (var obstacle in _obstacles)
        {
            // If the obstacle is not where the dino is, then do not check for collision.
            if (obstacle.Edges[0].X < 20 && obstacle.Edges[0].X > 27)
            {
                continue;
            }

            var bottomLeft = new Point((obstacle.Edges[0].X + obstacle.Edges[2].X) / 2, 29);
            var bottomRight = new Point((obstacle.Edges[1].X + obstacle.Edges[2].X) / 2, 29);
            var topRight = new Point((obstacle.Edges[1].X + obstacle.Edges[2].X) / 2, obstacle.Edges[2].Y + 1);
            var topLeft = new Point((obstacle.Edges[0].X + obstacle.Edges[2].X) / 2, obstacle.Edges[2].Y + 1);

            var rectangleX = topLeft.X;
            var rectangleY = topLeft.Y;
            var rectangleWidth = topRight.X - topLeft.X;
            var rectangleHeight = bottomLeft.Y - topLeft.Y;

            var obstacleHitbox = new Rectangle(rectangleX, rectangleY, rectangleWidth, rectangleHeight);

            if (hitboxTop.IntersectsWith(obstacleHitbox) || hitboxBottom.IntersectsWith(obstacleHitbox))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Gets the object for drawing the dinosaur.
    /// </summary>
    /// <returns>Object for drawing the dinosaur.</returns>
    private static DrawableObstacleModel GetDino()
    {
        // Create dinosaur
        Point[] dinoEdges =
        {
            new(23, 19 - _dino.CurrentHeight),
            new(23, 23 - _dino.CurrentHeight),
            new(22, 23 - _dino.CurrentHeight),
            new(22, 25 - _dino.CurrentHeight),
            new(21, 25 - _dino.CurrentHeight),
            new(21, 23 - _dino.CurrentHeight),
            new(20, 23 - _dino.CurrentHeight),
            new(20, 26 - _dino.CurrentHeight),
            new(21, 26 - _dino.CurrentHeight),
            new(21, 27 - _dino.CurrentHeight),
            new(22, 27 - _dino.CurrentHeight),
            new(22, 29 - _dino.CurrentHeight),
            new(24, 29 - _dino.CurrentHeight),
            new(24, 28 - _dino.CurrentHeight),
            new(23, 28 - _dino.CurrentHeight),
            new(23, 27 - _dino.CurrentHeight),
            new(24, 27 - _dino.CurrentHeight),
            new(24, 26 - _dino.CurrentHeight),
            new(25, 26 - _dino.CurrentHeight),
            new(25, 24 - _dino.CurrentHeight),
            new(27, 24 - _dino.CurrentHeight),
            new(27, 23 - _dino.CurrentHeight),
            new(25, 23 - _dino.CurrentHeight),
            new(25, 21 - _dino.CurrentHeight),
            new(27, 21 - _dino.CurrentHeight),
            new(27, 19 - _dino.CurrentHeight),
            new(25, 19 - _dino.CurrentHeight),
            new(25, 20 - _dino.CurrentHeight),
            new(24, 20 - _dino.CurrentHeight),
            new(24, 19 - _dino.CurrentHeight)
        };

        var greenPen = new Pen(Color.DarkOliveGreen, 1);

        var dino = new DrawableObstacleModel
        {
            Edges = dinoEdges,
            Pen = greenPen
        };

        return dino;
    }

    /// <summary>
    /// Draws the obstacles according to the content of the collection of obstacles and
    /// draws the dinosaur according to the current Dino's height.
    /// </summary>
    /// <param name="controlService">Control Service reference.</param>
    private static void RefreshCycle(IControlService controlService)
    {
        var displayWidth = controlService.GetWidth();
        var displayHeight = controlService.GetHeight();

        while (true)
        {
            using (var bitmap = new Bitmap(displayWidth, displayHeight))
            {
                using (var graphics = System.Drawing.Graphics.FromImage(bitmap))
                {
                    // Draw obstacles.
                    foreach (var obstacle in _obstacles)
                    {
                        if (obstacle != null && obstacle.Edges[1].X <= displayWidth && obstacle.Edges[1].X > -5)
                        {
                            graphics.DrawPolygon(obstacle.Pen, obstacle.Edges);
                        }
                    }

                    // Draw dinosaur.
                    var dino = GetDino();
                    graphics.DrawPolygon(dino.Pen, dino.Edges);

                    Point[] playerHitbox =
                    {
                        new(22, 29), // bottom left
                        new(20, 26), // top left
                        new(25, 26), // top right
                        new(24, 29) // bottom right
                    };

                    // Draw hitbox
                    /*
                    var whitePen = new Pen(Color.White, 1);
                    var hitboxTop = new Rectangle(23, -_dino.CurrentHeight + 19, 4, 5);
                    var hitboxBottom = new Rectangle(20, -_dino.CurrentHeight + 23, 4, 6);

                    graphics.DrawRectangle(whitePen, hitboxTop);
                    graphics.DrawRectangle(whitePen, hitboxBottom);     
                    */

                    if (CheckCollision(playerHitbox))
                    {
                        GameOver = true;

                        graphics.FillRectangle(new SolidBrush(Color.Transparent), 0, 0, displayWidth, displayHeight);

                        // Show 'Game Over'
                        var font = new Font("Arial", 7);
                        graphics.DrawString("GAME OVER", font, new SolidBrush(Color.White), 65, 1);
                    }

                    // Send to display
                    controlService.SendBitmap(bitmap);

                    // Break
                    Thread.Sleep(100);
                }
            }
        }
    }

    #endregion
}