
using System;
using System.Text;

namespace Filler_Mac
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
            var filler = new ConsoleFiller();
            filler.Run();
        }
    }
}