using _Project.Services;
using UnityEngine;

public class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        var serviceLocator = new ServiceLocator();

        var inputService = new InputService(new MouseInputHandler());
        
        var assetProvider = new AssetProvider();
        
        serviceLocator.RegisterService<IInputService>(inputService);
        
        serviceLocator.RegisterService<IAssetProvider>(assetProvider);
    }
}