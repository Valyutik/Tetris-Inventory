#if UNITY_EDITOR
using Shared.Model;
using TI.Sergei_Lind.Runtime.InventorySystem;
using UnityEditor;
using UnityEngine;

namespace TI.Sergei_Lind.Tests.Editor
{
    public static class InventoryTestEditor
    {
        [MenuItem("Tools/Test Inventory System")]
        public static void RunInventoryTest()
        {
            var inventory = new Inventory(5, 5);

            bool[,] shapeSword = {
                { true, true },
                { true, true }
            };
            
            bool[,] shapeAxe = {
                { true, true, true },
                { true, true, true }    
            };

            var sword = new Item("id_sword", "Sword", "A strong sword", shapeSword);
            var swordInstance = new ItemInstance(sword, "inst_001");
            
            var axe = new Item("id_axe", "Axe", "A strong axe", shapeAxe);
            var axeInstance = new ItemInstance(axe, "inst_002");
            
            var success = inventory.TryAddItem(axeInstance, new TilePosition(0, 0));
            Debug.Log($"Placement success: {success}");
            // var tryAddItem = inventory.TryAddItem(swordInstance, new TilePosition(3, 0));
            // Debug.Log($"Placement success: {tryAddItem}");

            var grid = "";
            for (var y = 0; y < inventory.Height; y++)
            {
                for (var x = 0; x < inventory.Width; x++)
                {
                    var tile = inventory.TileMap.GetTile(x, y);
                    grid += tile.IsEmpty ? "0" : "X";
                }
                grid += "\n";
            }
            Debug.Log(grid);

            var itemInstance = inventory.GetItem(new TilePosition(0, 2));
            
            Debug.Log(itemInstance == null
                ? $"Placement unsuccessful"
                : $"Placement success: {itemInstance.Item.Name}");
        }
    }
}
#endif