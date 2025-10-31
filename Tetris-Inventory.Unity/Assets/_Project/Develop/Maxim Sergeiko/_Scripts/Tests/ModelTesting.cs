using System.Collections;
using _Project.Model;
using UnityEngine;

namespace _Project.Develop.Maxim_Sergeiko._Scripts
{
    public class ModelTesting : MonoBehaviour
    {
        private void Start()
        {
            var inventory = new Inventory(4, 4);
            
            StartCoroutine(TestRoutine(inventory));
        }

        private IEnumerator TestRoutine(Inventory inventory)
        {
            inventory.LogMatrixState();
            Debug.Log("-----------------------");
            
            yield return new WaitForSeconds(1);

            var tilemap = new Tile[,]
            {
                { Tile.Exists , Tile.Exists, Tile.Empty},
                { Tile.Exists , Tile.Empty, Tile.Empty},
                { Tile.Empty , Tile.Empty, Tile.Empty}
            };
            
            var item = new Item(tilemap);

            var tilePosition = new TilePosition();
            
            inventory.PlaceItem(tilePosition, item);
            
            inventory.LogMatrixState();
            
            Debug.Log("-----------------------");
            
            yield return new WaitForSeconds(1);
            
            var item2 = new Item(tilemap);
            
            var tilePosition2 = new TilePosition(2, 0);
            
            inventory.PlaceItem(tilePosition2, item2);
            
            inventory.LogMatrixState();
            
            yield return new WaitForSeconds(4);
            
            inventory.RemoveItem(new TilePosition(0, 1));
            
            inventory.LogMatrixState();
        }
    }
}