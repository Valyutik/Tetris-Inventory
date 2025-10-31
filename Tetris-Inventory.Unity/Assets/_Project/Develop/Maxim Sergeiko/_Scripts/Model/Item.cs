namespace _Project.Model
{
    public class Item : Tilemap
    {
        public Item(int width, int height) : base(width, height)
        {
            
        }

        public Item(Tile[,] tiles) : base(tiles)
        {
            
        }
    }
}