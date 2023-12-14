using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace MA_Control
{
    public static class Highscore
    {
        #region Fields

        private const string PATH = @"C:\Users\HH-SoSo-2\Desktop\MyFolder\Basics\Aufgaben\Aufgaben\";
        private const string FILE = "Highscore.json";

        #endregion

        #region Public Methods

        public static bool saveToJSON(File file)
        {
            string jsonString = JsonSerializer.Serialize(file);
            try
            {
                System.IO.File.WriteAllText(PATH + FILE, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public static File readHighscoreFromJSON()
        {
            string jsonString = System.IO.File.ReadAllText(PATH + FILE);
            File highscoreFile = JsonSerializer.Deserialize<File>(jsonString)!;
            return highscoreFile;
        }

        public class File
        {
            public long Highscore { get; set; }
        }
        #endregion
    }
}
