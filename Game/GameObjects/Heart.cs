using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Game.GameObjects;

namespace Game
{
    [Serializable]
    public class Heart : GameObject
    {
        [JsonIgnore]
        public bool Used { get; set; } = false;
        public Heart()
        {
            Name = "Heart";
        }
        public override void Interaction(GameObject obj)
        {
            base.Interaction(obj);
            if (obj is GamePerson person)
            {
                person.HealthPoints += 30;
                Used = true;
            }
        }
    }
}
