using System.Drawing;
using System.Drawing.Text;
using System.Text;

namespace Library.Helpers;

/// <summary>
/// Class that manages the Bitmaps.
/// </summary>
public static class BitmapHelper
{
    #region Public Methods

    /// <summary>
    /// Combine multiple bitmap images to a big one.
    /// </summary>
    /// <param name="sources">List of bitmaps to be added.</param>
    /// <returns>The combined bitmap</returns>
    public static Bitmap Combine(List<Bitmap> sources)
    {
        var imageHeights = sources.First().Height;
        var imageWidths = sources.First().Width * sources.Count;

        var result = new Bitmap(imageWidths, imageHeights);

        using (var g = Graphics.FromImage(result))
        {
            for (var i = 0; i < sources.Count; i++)
            {
                g.DrawImage(sources[i], new Point(64 * i, 0));
            }
        }

        return result;
    }

    /// <summary>
    /// Convert a bitmap to a byte array.
    /// </summary>
    /// <param name="bitmap">The bitmap containing the data-</param>
    /// <returns>A byte array.</returns>
    public static byte[] ConvertBitmapToByteArray(Bitmap bitmap)
    {
        var width = 64;
        var height = 32;

        var pixelData = new byte[width * height * 3];

        for (var x = 0; x < width; ++x)
        {
            for (var y = 0; y < height; ++y)
            {
                var currentPixel = (y * width + x) * 3;
                pixelData[currentPixel] = bitmap.GetPixel(x, y).R;
                pixelData[currentPixel + 1] = bitmap.GetPixel(x, y).G;
                pixelData[currentPixel + 2] = bitmap.GetPixel(x, y).B;
            }
        }

        var enc = new ASCIIEncoding();
        var test = enc.GetBytes("P6\n64 32\n255\n");

        var combined = new byte[test.Length + pixelData.Length];

        for (var i = 0; i < combined.Length; ++i)
        {
            combined[i] = i < test.Length ? test[i] : pixelData[i - test.Length];
        }

        return combined;
    }

    /// <summary>
    /// Convert text to Bitmap returned as a byte array.
    /// </summary>
    /// <param name="txt">The text to be shown</param>
    /// <param name="fontName">The font to be used.</param>
    /// <param name="fontSize">The font size of the text.</param>
    /// <param name="backgroundColor">The background color.</param>
    /// <param name="frontColor">The text color.</param>
    /// <param name="width">The width of the segment</param>
    /// <param name="height">The height of the segment</param>
    /// <param name="shift">The shift of the text.</param>
    /// <returns>A byte array containing a bitmap.</returns>
    public static byte[] ConvertTextToByteArray(string txt, string fontName, int fontSize, Color backgroundColor, Color frontColor,
        int width, int height, int shift)
    {
        return ConvertBitmapToByteArray(ConvertTextToImage(txt, fontName, fontSize, backgroundColor, frontColor, width, height, shift));
    }

    /// <summary>
    /// Convert text to a bitmap
    /// </summary>
    /// <param name="txt">The text to be shown</param>
    /// <param name="fontName">The font to be used.</param>
    /// <param name="fontSize">The font size of the text.</param>
    /// <param name="backgroundColor">The background color.</param>
    /// <param name="frontColor">The text color.</param>
    /// <param name="width">The width of the segment</param>
    /// <param name="height">The height of the segment</param>
    /// <param name="shift">The shift of the text.</param>
    /// <returns>A bitmap.</returns>
    public static Bitmap ConvertTextToImage(string txt, string fontName, int fontSize, Color backgroundColor, Color frontColor, int width,
        int height, int shift)
    {
        var bmp = new Bitmap(width, height);
        using (var graphics = Graphics.FromImage(bmp))
        {
            var font = new Font(fontName, fontSize);
            var txtWidth = (int)graphics.MeasureString(txt, font).Width;
            graphics.FillRectangle(new SolidBrush(backgroundColor), 0, 0, bmp.Width, bmp.Height);
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.DrawString(txt, font, new SolidBrush(frontColor), 64 - txtWidth + 6 + shift, -9);
            graphics.Flush();
            font.Dispose();
        }

        return bmp;
    }

    #endregion
}