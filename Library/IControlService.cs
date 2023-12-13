using System.Drawing;

namespace Library;

/// <summary>
/// Control the scoreboards.
/// </summary>
public interface IControlService
{
    #region Public Methods

    /// <summary>
    /// Connect to the scoreboard.
    /// </summary>
    void Connect();

    /// <summary>
    /// Gets the height of all scoreboards.
    /// </summary>
    /// <returns>The total height.</returns>
    int GetHeight();

    /// <summary>
    /// Gets the width of all scoreboards.
    /// </summary>
    /// <returns>The total width.</returns>
    int GetWidth();

    /// <summary>
    /// Send bitmaps to the scoreboards.
    /// </summary>
    /// <param name="bitmap">The bitmap to be divided and shown.</param>
    void SendBitmap(Bitmap bitmap);

    /// <summary>
    /// Send bitmaps to the scoreboards.
    /// </summary>
    /// <param name="bitmaps">The bitmaps to be shown.</param>
    void SendBitmap(List<Bitmap> bitmaps);

    /// <summary>
    /// Send a text to the scoreboards.
    /// </summary>
    /// <param name="text">The text to be shown.</param>
    /// <param name="color">The color of the text.</param>
    /// <param name="shiftX">Shift the position by x.</param>
    void SendText(string text, Color color, int shiftX);

    #endregion
}