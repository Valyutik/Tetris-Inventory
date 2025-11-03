using Runtime.InventorySystem.Model;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Editor
{
    public static class RotationItemTest
    {
        [MenuItem("Tools/RotationItemTest")]
        public static void DrawRotationItem()
        {
            var shape = new[,]
            {
                {true, true, true},
                {true, true, false},
            };
            var axe = new Item("axe", "Axe", "Very", shape);

            DrawShape(axe);
            
            axe.RotateShape();
            
            DrawShape(axe);
            
            axe.RotateShape();
            
            DrawShape(axe);
        }

        private static void DrawShape(Item axe)
        {
            var grid = "";
            for (var y = 0; y < axe.Shape.GetLength(1); y++)
            {
                for (var x = 0; x < axe.Shape.GetLength(0); x++)
                {
                    grid += axe.Shape[x, y] ? "X" : "0";
                }
                grid += "\n";
            }
            Debug.Log(grid);
        }
    }
}
#endif