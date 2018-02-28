using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Playlister
{
    public static class Logger
    {
        private static void _log(string message, ConsoleColor color)
        {
            var temp = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = temp;
        }
        public static void Red(string message)
        {
            _log(message, ConsoleColor.Red);
        }
        public static void Green(string message)
        {
            _log(message, ConsoleColor.Green);
        }
        public static void Magenta(string message)
        {
            _log(message, ConsoleColor.Magenta);
        }

        public static void Section(string message)
        {
            var top = Enumerable.Range(0, 100).Select(x => "#");
            var sb = new StringBuilder(string.Join("", top));
            sb.AppendLine();
            sb.AppendLine($"#" + message + "#");
            sb.AppendLine(string.Join("", top));
            Console.WriteLine(sb.ToString());

        }
    }
}
