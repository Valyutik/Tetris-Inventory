using Runtime.Systems.ContentManager;
using UnityEngine.UIElements;

namespace Runtime.InventorySystem.ItemTooltip
{
    public class ItemTooltipView
    {
        public VisualElement Root { get; }
        public TextElement TooltipTitle { get; }
        public TextElement TooltipDescription { get; }
        
        public ItemTooltipView(PopupContent popupContent)
        {
            Root = popupContent.PopupRoot.Q<VisualElement>(InventoryConstants.UI.Tooltip.Root);
            
            TooltipTitle = Root.Q<TextElement>(InventoryConstants.UI.Tooltip.Title);
            TooltipDescription = Root.Q<TextElement>(InventoryConstants.UI.Tooltip.Description);
        }

        public void Show()
        {
            Root.AddToClassList(InventoryConstants.UI.Tooltip.Visible);
            Root.RemoveFromClassList(InventoryConstants.UI.Tooltip.Invisible);
        }

        public void Hide()
        {
            Root.RemoveFromClassList(InventoryConstants.UI.Tooltip.Visible);
            Root.AddToClassList(InventoryConstants.UI.Tooltip.Invisible);
        }
    }
}