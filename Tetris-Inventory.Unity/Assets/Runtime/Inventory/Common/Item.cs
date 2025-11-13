using UnityEngine;

namespace Runtime.Inventory.Common
{
    public class Item
    {
        private const int RotationStep = 90;
        
        public string Id { get; }
        public  string Name { get; }
        public  string Description { get; }
        public Vector2Int AnchorPosition { get; set; }
        public Vector2Int Size => new Vector2Int(Width, Height);
        public Vector2Int OriginalSize => new Vector2Int(OriginalWidth, OriginalHeight);
        public bool[,] Shape { get; private set; }
        
        public Sprite Visual { get; private set;}
        public Color Color { get; private set; }
        public bool IsStackable { get; }
        public bool IsFullStack =>  CurrentStack >= MaxStack;
        public int MaxStack { get; }
        public int CurrentStack { get; private set; }
        
        public int Width => Shape.GetLength(0);
        public int Height => Shape.GetLength(1);
        
        public int OriginalWidth { get; private set; }
        
        public int OriginalHeight { get; private set; }

        public int Rotation
        {
            get => _rotation;
            set
            {
                if (value >= 360)
                {
                    while (value > 360)
                    {
                        value -= 360;
                    }
                }
                
                _rotation = value;
            }
        }
        
        private bool[,] _cachedShape;
        
        private int _cachedRotation;

        private int _rotation;

        public Item(string id,
            string name,
            string description,
            Color color,
            bool isStackable = false,
            int maxStack = 1,
            int currentStack = 1,
            Sprite sprite = null,
            bool[,] shape = null)
        {
            Id = id;
            Name = name;
            Description = description;
            Color =  color;
            IsStackable = isStackable;
            MaxStack = maxStack;
            CurrentStack = currentStack;
            Shape = ValidateShape(shape);
            OriginalWidth = Width;
            OriginalHeight = Height;
            Visual = sprite;
        }

        public bool TryAddToStack(int amount)
        {
            if (!IsStackable || amount <= 0)
            {
                return false;
            }

            if (CurrentStack >= MaxStack)
            {
                return false;
            }
            
            CurrentStack = Mathf.Min(CurrentStack + amount, MaxStack);
            return true;
        }

        public void CacheShape()
        {
            _cachedShape = (bool[,])Shape.Clone();

            _cachedRotation = Rotation;
        }

        public void RestoreShape()
        {
            Shape = (bool[,])_cachedShape.Clone();
            
            Rotation = _cachedRotation;
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

            Rotation += RotationStep;
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