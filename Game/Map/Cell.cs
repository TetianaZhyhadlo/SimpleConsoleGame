using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Game.GameObjects;

namespace Game
{
    public enum CellState
    {
        HasShip,
        HadShip,
        IsEmpty, 
        IsShooted
    }
    [Serializable]
    public class Cell
    {
        [JsonIgnore]
        public GameObject GameObject { get; set; }

        public Cell()
        {
        }

        public Cell(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        public bool IsEmpty()
        {
            return GameObject == null;
        }
    }
}
