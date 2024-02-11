using Hydriuk.UnturnedModules.Extensions;
using LiteDB;
using System;
using UHighlight.Models;
using UnityEngine;

namespace AmbianceEffects.Models
{
    internal class AmbianceEffect
    {
        public string Name { get; set; } = string.Empty;
        public Guid EffectGUID { get; set; }
        public ZoneProperty.EEvent Event { get; set; }

        [BsonIgnore]
        public Vector3 Position { get; set; }
        public float[] position { get => Position.Serialize(); set => Position = value.Deserialize(); }

        public bool OwnerOnly { get; set; }
        public float VisibiltyRadius { get; set; } = 32f;

        public int MinRepeat { get; set; } = -1;
        public int MaxRepeat { get; set; } = -1;
    }
}
