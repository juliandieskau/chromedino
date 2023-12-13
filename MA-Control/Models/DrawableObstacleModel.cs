using System.Drawing;

namespace MA_Control.Models;

/// <summary>
/// Model used for drawing obstacles.
/// </summary>
public class DrawableObstacleModel
{
    #region Properties

    /// <summary>
    /// Edges of the obstacle.
    /// </summary>
    public Point[] Edges { get; set; }

    /// <summary>
    /// Pen to draw the obstacle.
    /// </summary>
    public Pen Pen { get; set; }

    #endregion
}