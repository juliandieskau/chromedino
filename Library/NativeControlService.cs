using Library.Helpers;
using log4net;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Principal;

namespace Library;

//// <inheritdoc />
public class NativeControlService : IControlService
{
    #region Fields

    private static readonly ILog _log = LogManager.GetLogger(typeof(ControlService));

    /// <summary>
    /// Local port number from which it is intended to communicate.
    /// </summary>
    private readonly int _port = 1337;

    /// <summary>
    /// List of IPs for all the scoreboards (LED Monitors) that are used.
    /// </summary>
    private readonly List<IPEndPoint> _scoreBoards = new();

    /// <summary>
    /// Height of the scoreboard (LED-Monitor).
    /// </summary>
    private readonly int _totalHeight = 32;

    /// <summary>
    /// UDP Client used to transfer the content to the scoreboard.
    /// </summary>
    private readonly UdpClient _udpClient;

    /// <summary>
    /// Width of each scoreboard (LED-Monitor Segment).
    /// </summary>
    private readonly int _widthPerSegment = 64;

    /// <summary>
    /// Total Width of the whole scoreboards (all LED-Monitor Segments together).
    /// </summary>
    private int _totalWidth;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor for the controlling of the scoreboards.
    /// </summary>
    public NativeControlService()
    {
        var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        // connect to ControlWindow at some point.
    }

    #endregion

    #region Public Methods

    /// <inheritdoc />
    public void Connect()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public int GetHeight()
    {
        return _totalHeight;
    }

    /// <inheritdoc />
    public int GetWidth()
    {
        return _totalWidth;
    }

    /// Adapter to SendBitmap but printing it to the screen instead.
    public void SendBitmap(Bitmap bitmap)
    {
        DrawToScreen(bitmap);
    }

    /// <summary>
    /// Adapter to SendBitmap but printing it to the screen instead.
    /// Hope this works.
    /// </summary>
    /// <param name="bitmaps"></param>
    public void SendBitmap(List<Bitmap> bitmaps)
    {
        for (var i = 0; i < bitmaps.Count; i++)
        {
            DrawToScreen(bitmaps[i]);
            //SendToScoreboard(_scoreBoards[i], bytes);
        }
    }

    /// <summary>
    /// Dont use (not used anyway)
    /// </summary>
    /// <param name="text"></param>
    /// <param name="color"></param>
    /// <param name="shiftX"></param>
    public void SendText(string text, Color color, int shiftX)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Private Methods

    private void DrawToScreen(Bitmap bitmap)
    {
        

    }

    #endregion

    
}