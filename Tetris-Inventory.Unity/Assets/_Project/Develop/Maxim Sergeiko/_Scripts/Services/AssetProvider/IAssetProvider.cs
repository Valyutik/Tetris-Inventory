namespace _Project.Services
{
    public interface IAssetProvider : IService
    {
        TAsset GetAsset<TAsset>() where TAsset : UnityEngine.Object;
        TAsset GetAsset<TAsset>(string path) where TAsset : UnityEngine.Object;
    }
}