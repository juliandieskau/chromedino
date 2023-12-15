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

        private static string PATH = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName).Parent.FullName;
        private static string FILE = "\\MA-Control\\Highscore.json";

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
            try
            {
                string jsonString = System.IO.File.ReadAllText(PATH + FILE);
                File highscoreFile = JsonSerializer.Deserialize<File>(jsonString)!;
                return highscoreFile;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        }

        public class File
        {
            public long HighscoreEasy { get; set; }
            public long HighscoreNormal { get; set; }
            public long HighscoreHard { get; set; }
            public long HighscoreImpossible { get; set;}
        }
        #endregion
    }
}
