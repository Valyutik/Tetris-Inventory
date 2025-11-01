#if UNITY_EDITOR
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
                { true, true, false }    
            };

            var sword = new Item("id_sword", "Sword", "A strong sword", shapeSword);
            
            var axe = new Item("id_axe", "Axe", "A strong axe", shapeAxe);
            
            var success = inventory.TryAddItem(axe, new Vector2Int(0, 0));
            Debug.Log($"Placement success: {success}");
            var tryAddItem = inventory.TryAddItem(sword, new Vector2Int(3, 0));
            Debug.Log($"Placement success: {tryAddItem}");

            var grid = "";
            for (var y = 0; y < inventory.Height; y++)
            {
                for (var x = 0; x < inventory.Width; x++)
                {
                    var tile = inventory.GetCell(new Vector2Int(x, y));
                    grid += tile.IsEmpty ? "0" : "X";
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