using LiteDB;
using System.Collections.Generic;

namespace AmbianceEffects.Models
{
    internal class AmbianceZone
    {
        public ObjectId Id { get; set; } = ObjectId.Empty;

        public string Name { get; set; }

        public List<AmbianceEffect> Effects { get; set; } = new List<AmbianceEffect>();

        public AmbianceZone(string name)
        {
            Name = name;
        }
    }
}
