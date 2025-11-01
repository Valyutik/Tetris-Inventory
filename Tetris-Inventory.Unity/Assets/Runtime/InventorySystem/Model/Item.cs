using UnityEngine;

namespace Runtime.InventorySystem.Model
{
    public class Item
    {
        public string Id { get; }
        public  string Name { get; }
        public  string Description { get; }
        public bool[,] Shape { get; }
        
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

        private bool[,] CreateDefaultShape()
        {
            var shape = new bool[1, 1];
            shape[0, 0] = true;
            return shape;
        }
    }
}