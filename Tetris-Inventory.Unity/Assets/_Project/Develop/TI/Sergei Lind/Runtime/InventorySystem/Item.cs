namespace TI.Sergei_Lind.Runtime.InventorySystem
{
    public class Item
    {
        public string Id { get; }
        public  string Name { get; }
        public  string Description { get; }
        public bool[,] Shape { get; }
        
        public int Width => Shape.GetLength(0);
        public int Height => Shape.GetLength(1);
        
        public Item(string id, string name, string description, bool[,] shape = null)
        {
            Id = id;
            Name = name;
            Description = description;
            Shape = shape ?? CreateDefaultShape();
        }

        private bool[,] CreateDefaultShape()
        {
            var shape = new bool[1, 1];
            shape[0, 0] = true;
            return shape;
        }
    }
}