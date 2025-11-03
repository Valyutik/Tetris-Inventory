using UnityEngine;

namespace Runtime.InventorySystem.Model
{
    public class Item
    {
        public string Id { get; }
        public  string Name { get; }
        public  string Description { get; }
        public bool[,] Shape { get; private set; }

        public Vector2Int? Position { get; private set; }
        
        public int Width => Shape.GetLength(1);
        public int Height => Shape.GetLength(0);
        
        public Item(string id, string name, string description, bool[,] shape = null)
        {
            Id = id;
            Name = name;
            Description = description;
            Shape = shape ?? CreateDefaultShape();
        }
        
        public void SetAnchorPosition(Vector2Int position)
        {
            Position = position;
        }

        public void ClearAnchorPosition()
        {
            Position = null;
        }
        
        public void RotateShape()
        {
            var height = Shape.GetLength(1);
            var width = Shape.GetLength(0);
            
            var newShape = new bool[height, width];

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    newShape[height - 1 - y, x] = Shape[x, y];
                }
            }

            Shape = newShape;
        }

        private bool[,] CreateDefaultShape()
        {
            var shape = new bool[1, 1];
            shape[0, 0] = true;
            return shape;
        }
    }
}