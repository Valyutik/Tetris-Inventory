namespace Runtime.InventorySystem
{
    public static class InventoryConstants
    {
        public static class Item
        {
            public const string ConfigPath = "Configs/Items";
        }

        public static class UI
        {
            public const int CellSize = 100;

            public const string CellStyle = "cell";
            
            public const string ContentRoot = "content";
            
            public const string DeleteAreaStyle = "delete-button-ready";
            
            public const string CreateButton = "create-button";
            public const string DeleteButton = "delete-button";

            public static class Inventory
            {
                public const string Grid = "grid";
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