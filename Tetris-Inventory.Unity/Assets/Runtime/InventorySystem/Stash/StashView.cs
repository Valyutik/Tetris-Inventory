using UnityEngine.UIElements;
using UnityEngine;
using System;

namespace Runtime.InventorySystem.Stash
{
    public sealed class StashView
    {
        private const int CellSize = 80;
        private const string CellStyle = "cell";
        
        public event Action<Vector2Int> OnCellClicked;
        
        private readonly VisualElement _grid;
        private VisualElement[,] _cells;
        
        public StashView(VisualElement root)
        {
            _grid = root.Q<VisualElement>("Grid");
        }

        public void BuildGrid(int width, int height)
        {
            _grid.Clear();
            _grid.style.width = width * CellSize;
            _grid.style.height = height * CellSize;
            
            _cells = new VisualElement[width, height];

            for (var y = 0; y < height; y++)
            {
                var row = new VisualElement
                {
                    style = { flexDirection = FlexDirection.Row },
                };
                _grid.Add(row);

                for (var x = 0; x < width; x++)
                {
                    var cell = new VisualElement();
                    cell.AddToClassList(CellStyle);
                    cell.style.width = CellSize;
                    cell.style.height = CellSize;
                    
                    int cx = x, cy = x;
                    cell.RegisterCallback<ClickEvent>(_ => OnCellClicked?.Invoke(new Vector2Int(cx, cy)));
                    
                    row.Add(cell);
                    _cells[x, y] = cell;
                }
            }
        }
        
        public void SetCellVisual(Vector2Int pos, Color color)
        {
            if (_cells == null) return;
            var cell = _cells[pos.x, pos.y];
            cell.style.backgroundColor = color;
        }
        
        public void Clear() => _grid.Clear();
    }
}