#if UNITY_EDITOR
using Runtime.InventorySystem.Model;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class InventoryTestEditor
    {
        [MenuItem("Tools/Test Inventory System")]
        public static void RunInventoryTest()
        {
            IInventory inventory = new Inventory(5, 5);

            bool[,] shapeSword = {
                { true, true },
                { true, true }
            };
            
            bool[,] shapeAxe = {
                { true, true, true },
                { true, true, false }    
            };

            var sword = new Item("id_sword", "Sword", "A strong sword", shapeSword);
            
            var axe = new Item("id_axe", "Axe", "A strong axe", shapeAxe);
            
            var success = inventory.TryPlaceItem(axe, new Vector2Int(0, 0));
            Debug.Log($"Placement success: {success}");
            var tryAddItem = inventory.TryPlaceItem(sword, new Vector2Int(3, 0));
            Debug.Log($"Placement success: {tryAddItem}");

            var grid = "";
            for (var y = 0; y < inventory.Height; y++)
            {
                for (var x = 0; x < inventory.Width; x++)
                {
                    grid += inventory.IsCellOccupied(new Vector2Int(x, y)) ? "0" : "X";
                }
                grid += "\n";
            }
            Debug.Log(grid);

            var itemInstance = inventory.GetItem(new Vector2Int(1, 3));
            
            Debug.Log(itemInstance == null
                ? $"Placement unsuccessful"
                : $"Placement success: {itemInstance.Name}");
        }
    }
}
#endif