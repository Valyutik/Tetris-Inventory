using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using System.Linq;

namespace Runtime.Utilities
{
    public static class AddressablesLoader
    {
        public static async Task<T> LoadAsync<T>(string label) where T : UnityEngine.Object
        {
            var handle = Addressables.LoadAssetAsync<T>(label);
            var asset = await handle.Task;
            return asset;
        }
        
        public static async Task<T[]> LoadAllAsync<T>(string label) where T : UnityEngine.Object
        {
            var handle = Addressables.LoadAssetsAsync<T>(label);
            var asset = await handle.Task;
            return asset.ToArray();
        }

        public static void Release<T>(T asset) where T : UnityEngine.Object
        {
            if (asset != null)
            {
                Addressables.Release(asset);
            }
        }
        
        public static void ReleaseAll<T>(T[] assets) where T : UnityEngine.Object
        {
            if (assets == null) return;
            foreach (var asset in assets)
            {
                if (asset != null)
                    Addressables.Release(asset);
            }
        }
    }
}