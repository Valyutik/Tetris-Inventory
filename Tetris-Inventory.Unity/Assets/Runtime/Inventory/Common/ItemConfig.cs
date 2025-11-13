using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Inventory.Common
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Items/Item")]
    public sealed class ItemConfig : ScriptableObject
    {
        [Header("Basic Info")]
        public string Id;
        public string DisplayName;
        public string Description;
        public Color Color;

        [Header("Size & Shape")]
        public int Width;
        public int Height;
        public Sprite Visual;
        
        [Header("Stacking")]
        [Min(1)] public bool IsStackable;
        [Min(1)] public int MaxStack = 1;
        
        [HideInInspector] public List<bool> flatShape = new();

        
        public bool[,] GetShapeMatrix()
        {
            var matrix = new bool[Width, Height];
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var index = y * Width + x;
                    if (index < flatShape.Count)
                        matrix[x, y] = flatShape[index];
                }
            }
            return matrix;
        }
    }
}