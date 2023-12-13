namespace MA_Control.Models;

/// <summary>
/// Model used to draw the dinosaur.
/// </summary>
public class DinoModel
{
    #region Properties

    /// <summary>
    /// Current Height of the dinosaur.
    /// </summary>
    public int CurrentHeight { get; set; } = 0;

    /// <summary>
    /// Old Height of the dinosaur.
    /// </summary>
    public int OldHeight { get; }

    #endregion
}