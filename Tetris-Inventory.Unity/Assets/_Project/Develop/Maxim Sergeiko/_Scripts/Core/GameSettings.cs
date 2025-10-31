using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    private static GameSettings Instance
    {
        get
        {
            if (_instance != null) return _instance;
            
            var settings = Resources.FindObjectsOfTypeAll<GameSettings>().First();
            
            if (settings == null) Debug.LogError("No GameSettings found!");
            
            _instance = settings;
            
            return _instance;
        }
    }

    private static GameSettings _instance;

    public bool LoadBootstrapperSceneOnStart => _loadBootstrapperSceneOnStart;
    
    [SerializeField] private bool _loadBootstrapperSceneOnStart;
}