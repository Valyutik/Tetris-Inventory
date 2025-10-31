namespace _Project.Model
{
    public class Tilemap
    {
        public Tile[,] Tiles { get; protected set; }

        protected Tilemap(int  width, int height) => Tiles = new Tile[width, height];
        
        protected Tilemap(Tile[,] tiles) => Tiles = tiles;
    }
}