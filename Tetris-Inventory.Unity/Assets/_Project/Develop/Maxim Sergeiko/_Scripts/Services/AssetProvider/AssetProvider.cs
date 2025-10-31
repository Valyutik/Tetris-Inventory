using UnityEngine;

namespace _Project.Services
{
    public class AssetProvider : IAssetProvider
    {
        public TAsset GetAsset<TAsset>() where TAsset : Object => Resources.Load<TAsset>(typeof(TAsset).Name);

        public TAsset GetAsset<TAsset>(string path) where TAsset : Object => Resources.Load<TAsset>(path);
    }
}