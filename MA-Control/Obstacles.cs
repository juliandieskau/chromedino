using System;
using System.Drawing;

namespace MA_Control;

/// <summary>
/// Class used for the background with the obstacles.
/// </summary>
internal class Obstacles
{
    #region Fields

    private Point[,] _obstacles;

    private const int TRIANGLE_COUNT = 10;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    public Obstacles()
    {
        _obstacles = GetObstacles();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Regenerates the list of obstacles.
    /// </summary>
    public void ReloadBackground()
    {
        _obstacles = GetObstacles();
    }

    /// <summary>
    /// Updates the background by updating the obstacles object and redrawing the obstacles.
    /// </summary>
    public void UpdateBackground()
    {
        DisplayContent.ClearObstacles();

        for (var triangle = 0; triangle < TRIANGLE_COUNT; triangle++)
        {
            _obstacles[triangle, 0] = new Point(_obstacles[triangle, 0].X - 1, 29);
            _obstacles[triangle, 1] = new Point(_obstacles[triangle, 1].X - 1, 29);
            _obstacles[triangle, 2] = new Point(_obstacles[triangle, 2].X - 1, _obstacles[triangle, 2].Y);

            Point[] newTriangleEdges =
            {
                _obstacles[triangle, 0],
                _obstacles[triangle, 1],
                _obstacles[triangle, 2]
            };

            DisplayContent.DrawObstacle(newTriangleEdges, new Pen(Color.DarkRed));
        }

        DisplayContent.DrawFloor();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Gets an object with the generated the obstacles.
    /// </summary>
    /// <returns>Object with all obstacles.</returns>
    private static Point[,] GetObstacles()
    {
        var random = new Random();
        var previousTriangle = 0;

        var obstacleList = new Point[TRIANGLE_COUNT, 3];

        for (var triangle = 0; triangle < TRIANGLE_COUNT; triangle++)
        {
            // Generate random offset
            var heightOffset = random.Next(0, 5);

            previousTriangle += random.Next(40, 60);

            //Create points to define polygon
            var point1 = new Point(1 + previousTriangle, 29);
            var point2 = new Point(5 + previousTriangle, 29);
            var point3 = new Point(3 + previousTriangle, 27 - heightOffset);

            obstacleList[triangle, 0] = point1;
            obstacleList[triangle, 1] = point2;
            obstacleList[triangle, 2] = point3;
        }

        return obstacleList;
    }

    #endregion
}