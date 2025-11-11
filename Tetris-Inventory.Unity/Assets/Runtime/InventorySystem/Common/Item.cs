using UnityEngine;

namespace Runtime.InventorySystem.Common
{
    public class Item
    {
        public string Id { get; }
        public  string Name { get; }
        public  string Description { get; }
        public Vector2Int AnchorPosition { get; set; }
        public bool[,] Shape { get; private set; }
        public Color Color { get; private set; }
        
        public int Width => Shape.GetLength(0);
        public int Height => Shape.GetLength(1);
        
        public Item(string id, string name, string description, Color color, bool[,] shape = null)
        {
            Id = id;
            Name = name;
            Description = description;
            Color =  color; 
            Shape = ValidateShape(shape);
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
        
        private bool[,] ValidateShape(bool[,] shape)
        {
            if (shape == null)
                return CreateDefaultShape();

            var width = shape.GetLength(0);
            var height = shape.GetLength(1);

            var hasAny = false;
            for (var x = 0; x < width && !hasAny; x++)
            for (var y = 0; y < height && !hasAny; y++)
                if (shape[x, y]) hasAny = true;

            return hasAny ? shape : CreateDefaultShape();
        }
    }
}