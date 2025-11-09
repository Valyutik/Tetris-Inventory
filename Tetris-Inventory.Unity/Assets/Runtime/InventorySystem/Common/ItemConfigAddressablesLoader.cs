using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using System.Linq;

namespace Runtime.InventorySystem.Common
{
    public static class ItemConfigAddressablesLoader
    {
        public static async Task<ItemConfig[]> LoadAllAsync(string label)
        {
            var handle = Addressables.LoadAssetsAsync<ItemConfig>(label, null);
            var configs = await handle.Task;
            return configs.ToArray();
        }

        public static void Release(ItemConfig[] configs)
        {
            foreach (var config in configs)
            {
                Addressables.Release(config);
            }
        }
    }
}