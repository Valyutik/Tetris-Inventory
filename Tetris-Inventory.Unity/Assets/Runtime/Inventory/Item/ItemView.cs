using UnityEngine;

namespace Runtime.Inventory.Item
{
    public struct ItemView
    {
        public Sprite Visual { get; }

        public string Name { get; }

        public string Description { get; }

        public int CurrentStack { get; }

        public int MaxStack { get; }

        public int Rotation { get; }
        
        public int OriginalWidth { get; }
        
        public int OriginalHeight { get; }
        
        public int Width { get; }
        
        public int Height { get; }
        
        public Vector2Int AnchorPosition { get; }

        public ItemView(Sprite visual, string name, string description, int currentStack, int maxStack, int rotation, int originalWidth, int originalHeight, int width, int height, Vector2Int anchorPosition)
        {
            Visual = visual;
            Name = name;
            Description = description;
            CurrentStack = currentStack;
            MaxStack = maxStack;
            Rotation = rotation;
            OriginalWidth = originalWidth;
            OriginalHeight = originalHeight;
            Width = width;
            Height = height;
            AnchorPosition = anchorPosition;
        }
    }
}