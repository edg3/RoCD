using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RoCD.Helpers
{
    public class CombatLog
    {
        private static List<string> _log = new List<string>();

        public static void Log(string message)
        {
            _log.Add(message);
        }

        static CombatLog()
        {
            Log("welcome to rocd alpha");
        }

        public static List<string> Get()
        {
            var lst = new List<string>();

            for (int i = _log.Count - 1; (i > -1) && (i > _log.Count - 6); i--)
                lst.Add(_log[i]);

            return lst;
        }

        public static List<string> GetAll()
        {
            return _log;
        }

        public static void GameLog(string message)
        {
            using (var sw = new StreamWriter("game.log", true))
            {
                sw.WriteLine(message);
            }
        }
    }
}
