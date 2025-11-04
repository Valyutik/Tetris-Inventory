using System.Collections.Generic;
using UnityEngine;

namespace Runtime.InventorySystem.Config
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Inventory/Item")]
    public sealed class ItemConfig : ScriptableObject
    {
        [Header("Basic Info")]
        public string id;
        public string displayName;
        public string description;
        public Sprite icon;

        [Header("Size & Shape")]
        public int width;
        public int height;
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