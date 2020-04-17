using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Game.Exceptions;
using Game.GameObjects;
using Game.Weapons;

namespace Game.Main
{
   // public enum Menu { Start, Continue, Exit}
   [Serializable]
    public class BaseGame
    {
        public Character Character1 { get; set; }
        public Character Character2 { get; set; }
        public int Turn { get; set; }
        public int MapSize1 { get; set; }
        public int MapSize2 { get; set; }

        [JsonIgnore]
        public Map World { get; set; }

        public List<string> GameList { get; set; } = new List<string>();
       // public Menu menuChoose { get; set; }
        public List<GameObject> GameObjects { get; set; } = new List<GameObject>();
        protected BaseGame() { }
        public BaseGame(int mapSize1, int mapSize2)
        {
            MapSize1 = mapSize1;
            MapSize2 = mapSize2;
        }

        public Character InitPlayer(int id, bool team)
        {
            Console.Write("Enter player name: ");
            string name = Console.ReadLine();
            return new Character(name, team);
        }

        public Map InitWorld()
        {
            Random randWeapon = new Random();
            GameObjects.AddRange(Enumerable.Range(10, 7).Select(x => new Bot($"Friend {x}", true)));
            GameObjects.AddRange(Enumerable.Range(2, 8).Select(x => new Bot($"Enemy {x}", false)));
            GameObjects.AddRange(Enumerable.Range(0, 15).Select(x => new Heart()));
            GameObjects.AddRange(Enumerable.Range(0, 9).Select(x => randWeapon.Next(0, 2) == 0
                        ? new Knife() as GameObject
                        : new Sword() as GameObject));
            Map world = new Map(MapSize1, MapSize2, Season.None);

            world.GenerateMap();
            world.InitGameObject(Character1, 1, 1);
            world.InitGameObject(Character2, MapSize1 - 2, MapSize2 - 2);
            world.SetGameObjects(GameObjects.ToArray());
            return world;
        }

        public void Start()
        {
            int v = 0;
            Character1 = InitPlayer(0, true);
            Character2 = InitPlayer(1, false);
            World = InitWorld();
            while (Character1.Alive && !World.HasWinner())
            {
                try
                {
                    Turn++;
                    World.Show(Character1, Character2, Turn, true);
                    Character1.Moving();
                    Console.WriteLine("Tap <Enter> to finish move...");
                    Console.ReadLine();
                    World.Show(Character1, Character2, Turn, false);
                    Character2.Moving();
                    Console.WriteLine("Tap <Enter> to finish move...");
                    Console.ReadLine();
                    World.Show(Character1, Character2, Turn);
                    foreach (Bot item in GameObjects.Where(x => x is Bot b && b.Alive))
                    {
                        item.Move("");
                    }
                    if ((Console.ReadKey().Key) == ConsoleKey.Escape)
                        SaveGame();
                    if (Turn == 2)
                    {
                        var a = 5 / v;
                        throw new GameException("Buy this game to continue");
                    }
                    
                }
                catch (GameException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (DivideByZeroException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.ReadLine();
                    Console.WriteLine("End turn.");
                }
            }

            World.Show(Character1, Character2, Turn);
            Console.WriteLine($"Congrats {World.GetWinner().Name}!");
        }
        public void SaveGame()
        {
            Console.WriteLine("Do you want to save game? Press 1 if YES");
            if (Console.ReadLine() == "1")
            {
                Console.WriteLine("Please input the name for game saving.");
                string savedGame = Console.ReadLine();
                JsonSerialize(savedGame, this);
                GameList.Add(savedGame);
            }
            else
                ExitGame();

        }
        public void Continue()
        {
            GameList
                .ForEach(x => Console.WriteLine(x));
            Console.ReadKey();
            JsonDeserialize(Console.ReadLine()).Start();
        }
        public void ExitGame()
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        public void MenuInvoke()
        {
            Console.WriteLine("Menu:");
            int top = Console.CursorTop;
            int y = top;

            Console.WriteLine("Start");
            Console.WriteLine("Continue");
            Console.WriteLine("Exit");
            int down = Console.CursorTop;
            Console.CursorSize = 100;
            Console.CursorTop = top;
            ConsoleKey key;
            while ((key = Console.ReadKey(true).Key) != ConsoleKey.Enter)
            {
                if (key == ConsoleKey.UpArrow)
                {
                    if (y > top)
                    {
                        y--;
                        Console.CursorTop = y;
                    }
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    if (y < down - 1)
                    {
                        y++;
                        Console.CursorTop = y;
                    }
                }
            }
            Console.CursorTop = down;

            if (y == top)
                Start();
            else if (y == top + 1)
                Continue();
            else if (y == top + 2)
                ExitGame();
           
        }
        public static void JsonSerialize<T>(string path, T obj) where T : class
        {
            using (var fs = new FileStream($"{path}.json", FileMode.OpenOrCreate))
            {
                string strObj = JsonConvert.SerializeObject(obj);
                byte[] data = strObj
                    .Select(x => (byte)x)
                    .ToArray();
                fs.Write(data, 0, data.Length);
            }
        }
        public static BaseGame JsonDeserialize(string path)
        {
            using (var streamReader = new StreamReader($"{path}.json"))
            {

                string dataStr = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<BaseGame>(dataStr);
            }
        }
    }
}
