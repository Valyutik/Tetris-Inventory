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
            
            public const string ContentRoot = "Content";
            
            public const string DeleteAreaStyle = "delete-button-ready";
            
            public static class DeleteConfirmationConst
            {
                public const string PopupRootTitle = "Popup-container";
                public const string ConfirmButtonTitle = "Confirm-button";
                public const string CancelButtonTitle = "Cancel-button";
            }
        }
    }
}