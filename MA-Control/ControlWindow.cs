using Library;
using log4net;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Windows.Forms;

namespace MA_Control;

public partial class ControlWindow : Form
{
    #region Fields

    // The display object to handle graphics.
    private static DisplayContent _displayContent;
    private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

    #endregion

    #region Constructors

    /// <summary>
    /// The constructor/creator for the UI object.
    /// Hint: No changes need to be done here.
    /// </summary>
    public ControlWindow()
    {
        InitializeComponent();

        var serviceProvider = new ServiceCollection()
            .AddSingleton<IControlService, ControlService>()
            .BuildServiceProvider();

        var controlService = serviceProvider.GetService<IControlService>();

        // Thread for the drawing of background and Dino.
        _displayContent = new DisplayContent(controlService);

        // Thread for the Jump.
        var thread = new Thread(() => DisplayContent.BackgroundWorker(_displayContent));
        thread.Priority = ThreadPriority.Lowest;
        thread.Start();

        // Thread for the obstacles.
        var game = new Game(new Obstacles());
        var gameThread = new Thread(() => game.Start());
        gameThread.Start();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// An event handler for the keys pressed.
    /// Hint: No changes need to be done here.
    /// </summary>
    /// <param name="sender">Sender reference.</param>
    /// <param name="e">Info about the pressed key.</param>
    private void OnKeyPressed(object sender, PreviewKeyDownEventArgs e)
    {
        _displayContent.OnKeyPressed(e.KeyCode);
    }

    #endregion
}