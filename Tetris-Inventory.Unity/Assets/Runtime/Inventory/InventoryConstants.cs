using UnityEngine;

namespace Runtime.Inventory
{
    public static class InventoryConstants
    {
        public static class Game
        {
            public static readonly Vector2Int UserGridSize = new Vector2Int(5, 7);
        }
        
        public static class UI
        {
            public const int CellSize = 100;

            public const string CellStyle = "cell";
            public const string ItemStyle = "item";
            
            public const string ContentRoot = "content";
            
            public const string DeleteAreaStyle = "delete-button-ready";
            
            public const string CreateButton = "create-button";
            public const string DeleteButton = "delete-button";
            
            public static class Inventory
            {
                public const string Grid = "grid";
                public const string ItemCountLabel = "item-count-label";
            }

            public static class Projection
            {
                public const string ItemProjection = "item-projection";
                public const string ItemProjectionCanPlace = "item-projection-can-place";
                public const string ItemProjectionCannotPlace = "item-projection-cannot-place";
            }
            
            public static class Tooltip
            {
                public const string Root = "tooltip";
                public const string Title = "title";
                public const string Description = "description";
                public const string Visible = "tooltip-visible";
                public const string Invisible = "tooltip-invisible";
            }
            
            public static class DeleteConfirmationConst
            {
                public const string PopupRootTitle = "popup-container";
                public const string ConfirmButtonTitle = "confirm-button";
                public const string CancelButtonTitle = "cancel-button";
            }
        }
    }
}