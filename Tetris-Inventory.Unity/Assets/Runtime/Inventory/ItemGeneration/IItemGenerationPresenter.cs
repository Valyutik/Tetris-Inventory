using System;
using System.Collections.Generic;

namespace Runtime.Inventory.ItemGeneration
{
    public interface IItemGenerationPresenter
    {
        event Action<IEnumerable<Item.Item>> OnItemGenerated;
    }
}