using System.Collections.Generic;
using UnityEngine;

namespace Runtime.InventorySystem.Common
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Items/Item")]
    public sealed class ItemConfig : ScriptableObject
    {
        [Header("Basic Info")]
        public string id;
        public string displayName;
        public string description;
        public Color color;

        [Header("Size & Shape")]
        public int width;
        public int height;
        
        [Header("Stacking")]
        [Min(1)] public bool isStackable;
        [Min(1)] public int MaxStack = 1;
        
        [HideInInspector] public List<bool> flatShape = new();
        
        public bool[,] GetShapeMatrix()
        {
            var matrix = new bool[width, height];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var index = y * width + x;
                    if (index < flatShape.Count)
                        matrix[x, y] = flatShape[index];
                }
            }
            return matrix;
        }
    }
}