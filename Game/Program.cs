using System;
using System.IO;
using System.Linq;
using Game.Main;
using Newtonsoft.Json;

namespace Game
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.OutputEncoding = System.Text.Encoding.Unicode;
                BaseGame game = new BaseGame(16, 14);
                game.MenuInvoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }
       
    }
}
