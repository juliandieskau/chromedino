using Library.Helpers;
using log4net;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Library;

//// <inheritdoc />
public class ControlService : IControlService
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
    public ControlService()
    {
        var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

        _udpClient = new UdpClient(_port);
        // standard gateway vom minipc auf:  192.168.2.1 setzen
        // ip adressen ablesen von bildschirmen (auseinander bauen)
        // kabel direkt mit mini pc verbunden
        // ip adressen hier eingeben
        // von pc aus gesehen zur säule:
        // LINKS:  172
        // MITTE:  174
        // RECHTS: 173
        AddScoreBoard("192.168.2.172");
        AddScoreBoard("192.168.2.174");
        AddScoreBoard("192.168.2.173");

        _log.Info(GetType().Name
                  + "."
                  + MethodBase.GetCurrentMethod()
                  + " - Adding ScoreBoards with IPs: "
                  + "192.168.2.172, 192.168.2.171 and 192.168.2.170 (please consider order of boards)");
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

    /// <inheritdoc />
    public void SendBitmap(Bitmap bitmap)
    {
        var bitmaps = new List<Bitmap>();

        var shiftX = 0;
        foreach (var scoreBoard in _scoreBoards)
        {
            // Image with width and height of one LED Monitor-Segment.
            var image = new Bitmap(_widthPerSegment, _totalHeight);

            using (var graphics = Graphics.FromImage(image))
            {
                graphics.DrawImage(bitmap, new Point(0 - shiftX, 0));
                shiftX += _widthPerSegment;
            }

            bitmaps.Add(image);
        }

        SendBitmap(bitmaps);
    }

    /// <inheritdoc />
    public void SendBitmap(List<Bitmap> bitmaps)
    {
        for (var i = 0; i < bitmaps.Count; i++)
        {
            var bytes = BitmapHelper.ConvertBitmapToByteArray(bitmaps[i]);
            SendToScoreboard(_scoreBoards[i], bytes);
        }
    }

    /// <inheritdoc />
    public void SendText(string text, Color color, int shiftX)
    {
        var shift = shiftX;
        foreach (var scoreBoard in _scoreBoards)
        {
            var bytes = BitmapHelper.ConvertTextToByteArray(text, "Arial", 34, Color.Black, color, _widthPerSegment, _totalHeight, shift);
            shift += _widthPerSegment;
            SendToScoreboard(scoreBoard, bytes);
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Adds a scoreboard to the ones that are connected and recalculate the total width.
    /// </summary>
    /// <param name="ipAddress">IP Address of the scored board (LED-Monitor).</param>
    private void AddScoreBoard(string ipAddress)
    {
        _scoreBoards.Add(new IPEndPoint(IPAddress.Parse(ipAddress), _port));
        _totalWidth += _widthPerSegment;
        Console.WriteLine($"Added the scoreboard {ipAddress} and gained a total width of {_totalWidth} px.");
    }

    /// <summary>
    /// Send the byte array containing the data to the specified scoreboard.
    /// </summary>
    /// <param name="scoreboard">IP of the scoreboard to which the content is sent.</param>
    /// <param name="content">The content to be shown.</param>
    private void SendToScoreboard(IPEndPoint scoreboard, byte[] content)
    {
        _udpClient.Send(content, content.Length, scoreboard);
    }

    #endregion
}