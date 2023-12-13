using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MA_Control;

internal static class Program
{
    #region Private Methods

    /// <summary>
    /// The main entry point for the application.
    /// Hint: No changes need to be done here.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

        var window = new ControlWindow();
        window.Show();

        // Start the GUI
        Application.Run();
    }

    #endregion
}