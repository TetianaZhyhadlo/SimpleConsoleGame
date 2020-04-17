using System;
using System.Text.Json.Serialization;

namespace Game.GameObjects
{
    [Serializable]
    public abstract class GameObject
    {
        [JsonIgnore]
        public string Name { get; set; }
        
        protected GameObject() { }
        public virtual void Interaction(GameObject obj)
        {
            Console.WriteLine("Interaction: {0} => {1}", Name, obj.Name);
        }
    }
}
